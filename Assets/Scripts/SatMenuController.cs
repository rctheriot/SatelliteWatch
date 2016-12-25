using UnityEngine;
using System.Collections;

public class SatMenuController : MonoBehaviour
{

    public GameObject wandLeft;

    public GameObject satGroupInfo;
    public Vector3 satGroupLocalPosition;

    public GameObject warning;
    public Vector3 warningLocalPosition;

    public Color activeColor, deactiveColor;

    public float radius;

    public Vector3 menuLocalPosition;

    private GameObject[] menuItems;

    int currentSelected;

    private float menuRotation;

    private int maxGroupsAllowed;
    private int curNumGroupsEnabled;
    private float warningStart;

    [Header("Audio")]
    public AudioClip leftSelect;
    public AudioClip rightSelect;
    public AudioClip hideInfoAudio;
    public AudioClip activate;
    public AudioClip deactivate;
    public AudioClip failActivate;

    private AudioSource wandLeftAudio;


    void Start()
    {
        curNumGroupsEnabled = 1;
        maxGroupsAllowed = 4;

        menuItems = new GameObject[transform.childCount - 2];

        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i] = transform.GetChild(i + 2).gameObject;
        }

        foreach (GameObject child in menuItems)
        {
            if (child.GetComponent<SatMenuItem>().satGroup.activeSelf)
            {
                child.GetComponent<SatMenuItem>().SetColor(activeColor);
            }
            else child.GetComponent<SatMenuItem>().SetColor(deactiveColor);

        }

        SetPositionOfChildren();

        currentSelected = 0;

        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].SetActive(true);
        }

        loadSatGroupInfo();

        satGroupInfo.SetActive(true);
        warning.SetActive(true);

        wandLeftAudio = wandLeft.GetComponent<AudioSource>();

    }


    void Update()
    {

        if (wandLeft.GetComponent<WandController>().TouchpadDown() && wandLeft.GetComponent<WandController>().DpadRIGHT())
        {
            changeSelection(-1);

            wandLeftAudio.clip = rightSelect;
            wandLeftAudio.Play();

        }

        if (wandLeft.GetComponent<WandController>().TouchpadDown() && wandLeft.GetComponent<WandController>().DpadLEFT())
        {
            changeSelection(1);

            wandLeftAudio.clip = leftSelect;
            wandLeftAudio.Play();
        }

        if (wandLeft.GetComponent<WandController>().TouchpadDown() && wandLeft.GetComponent<WandController>().DpadUP())
        {
            //Not Activated
            if (!menuItems[currentSelected].GetComponent<SatMenuItem>().satGroup.activeSelf)
            {
                //Check to see if we can activate
                if (curNumGroupsEnabled <= maxGroupsAllowed)
                {
                    wandLeftAudio.clip = activate;
                    wandLeftAudio.Play();

                    curNumGroupsEnabled++;
                    menuItems[currentSelected].GetComponent<SatMenuItem>().Activate();
                    menuItems[currentSelected].GetComponent<SatMenuItem>().SetColor(activeColor);
                    hideWarning();
                }
                else
                {
                    wandLeftAudio.clip = failActivate;
                    wandLeftAudio.Play();

                    warningStart = Time.time;
                    showWarning();
                }
            }
            else
            {
                wandLeftAudio.clip = deactivate;
                wandLeftAudio.Play();

                curNumGroupsEnabled--;
                menuItems[currentSelected].GetComponent<SatMenuItem>().Activate();
                menuItems[currentSelected].GetComponent<SatMenuItem>().SetColor(deactiveColor);
                hideWarning();
            }

        }

        if (wandLeft.GetComponent<WandController>().TouchpadDown() && wandLeft.GetComponent<WandController>().DpadDOWN())
        {

            wandLeftAudio.clip = hideInfoAudio;
            wandLeftAudio.Play();

            for (int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i].SetActive(!menuItems[i].activeSelf);
            }

            satGroupInfo.SetActive(!satGroupInfo.activeSelf);
        }

        if (wandLeft.GetComponent<WandController>().TriggerAxis() > 0)
        {
            if (menuItems[currentSelected].GetComponent<SatMenuItem>().satGroup.activeSelf)
            {
                foreach (Transform satellite in menuItems[currentSelected].GetComponent<SatMenuItem>().satGroup.transform)
                {
                    Behaviour halo = (Behaviour)satellite.GetComponent("Halo");
                    halo.enabled = true;
                }
            }
        }
        if (wandLeft.GetComponent<WandController>().TriggerAxis() <= 0)
        {
            if (menuItems[currentSelected].GetComponent<SatMenuItem>().satGroup.activeSelf)
            {
                foreach (Transform satellite in menuItems[currentSelected].GetComponent<SatMenuItem>().satGroup.transform)
                {
                    Behaviour halo = (Behaviour)satellite.GetComponent("Halo");
                    halo.enabled = false;
                }
            }
        }

        SetPositionOfChildren();
        SetOpacityOfChildren();

        if (Time.time - warningStart > 3)
        {
            hideWarning();
        }

    }

    private void changeSelection(int direction)
    {
        currentSelected -= direction;

        if (currentSelected >= menuItems.Length) currentSelected = 0;
        if (currentSelected < 0) currentSelected = menuItems.Length - 1;

        float newRotation = (2 * Mathf.PI / menuItems.Length) * direction;
        menuRotation += (newRotation * Mathf.Rad2Deg);
        SetPositionOfChildren();
        loadSatGroupInfo();
    }

    private void SetPositionOfChildren()
    {
        transform.position = wandLeft.transform.position;
        transform.Translate(menuLocalPosition);

        satGroupInfo.transform.position = wandLeft.transform.position;
        satGroupInfo.transform.Translate(satGroupLocalPosition);

        warning.transform.position = wandLeft.transform.position;
        warning.transform.Translate(warningLocalPosition);

        transform.rotation = wandLeft.transform.rotation;
        transform.Rotate(new Vector3(45, 0, 0));


        for (int i = 0; i < menuItems.Length; i++)
        {
            float theta = (2 * Mathf.PI / menuItems.Length) * i + (Mathf.Deg2Rad * menuRotation);
            float xPos = Mathf.Sin(theta);
            float yPos = Mathf.Cos(theta);
            menuItems[i].transform.localPosition = new Vector2(xPos, yPos) * radius;
        }


    }

    private void SetOpacityOfChildren()
    {

        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].GetComponent<SatMenuItem>().SetOpacity(0);
        }

        menuItems[currentSelected].GetComponent<SatMenuItem>().SetOpacity(1);

        int left = currentSelected - 1;
        int right = currentSelected + 1;

        if (left < 0) left = menuItems.Length - 1;
        if (right >= menuItems.Length) right = 0;

        menuItems[left].GetComponent<SatMenuItem>().SetOpacity(0.45f);
        menuItems[right].GetComponent<SatMenuItem>().SetOpacity(0.45f);

    }

    private void loadSatGroupInfo()
    {
        TextAsset groupInfoText = Resources.Load("SatGroupInfo") as TextAsset;

        string[] info = groupInfoText.ToString().Split("\n"[0]);

        string selSat = menuItems[currentSelected].GetComponent<SatMenuItem>().satGroup.name.Trim();

        for (int i = 0; i < info.Length; i++)
        {
            if (selSat.Equals(info[i].Trim()))
            {
                string text = ResolveTextSize(info[i + 2], 40);
                satGroupInfo.GetComponent<TextMesh>().text = text;
            }
        }

    }

    private string ResolveTextSize(string input, int lineLength)
    {

        // Split string by char " "         
        string[] words = input.Split(" "[0]);

        // Prepare result
        string result = "";

        // Temp line string
        string line = "";

        // for each all words        
        foreach (string s in words)
        {
            // Append current word into line
            string temp = line + " " + s;

            // If line length is bigger than lineLength
            if (temp.Length > lineLength)
            {

                // Append current line into result
                result += line + "\n";
                // Remain word append into new line
                line = s;
            }
            // Append current word into current line
            else
            {
                line = temp;
            }
        }

        // Append last line into result        
        result += line;

        // Remove first " " char
        return result.Substring(1, result.Length - 1);
    }


    public void hideInfo()
    {

        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].SetActive(false);
        }

        satGroupInfo.SetActive(false);
        warning.SetActive(false);
    }

    public void showInfo()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].SetActive(true);
        }

        satGroupInfo.SetActive(true);
    }

    public void showWarning()
    {
        warning.SetActive(true);
    }

    public void hideWarning()
    {
        warning.SetActive(false);
    }
}
