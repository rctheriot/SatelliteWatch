using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HelpMenu : MonoBehaviour
{
    public GameObject SatMenu, TimeMenu, SelSatMenu;
    public GameObject wandLeft, wandRight;

    [Space(10)]
    public GameObject leftTrigger;
    public Vector3 leftTriggerPos;
    [Space(10)]
    public GameObject rightTrigger;
    public Vector3 rightTriggerPos;
    [Space(10)]
    public GameObject leftGrip;
    public Vector3 leftGripPos;
    [Space(10)]
    public GameObject rightGrip;
    public Vector3 rightGripPos;
    [Space(10)]
    public GameObject leftMenu;
    public Vector3 leftMenuPos;
    [Space(10)]
    public GameObject rightMenu;
    public Vector3 rightMenuPos;
    [Space(10)]
    public GameObject leftDpadDOWN;
    public Vector3 leftDownPos;
    public GameObject leftDpadUP;
    public Vector3 leftUpPos;
    public GameObject leftDpadLEFT;
    public Vector3 leftLeftPos;
    public GameObject leftDpadRIGHT;
    public Vector3 leftRightPos;
    [Space(10)]
    public GameObject rightDpadDOWN;
    public Vector3 rightDownPos;
    public GameObject rightDpadUP;
    public Vector3 rightUpPos;
    public GameObject rightDpadLEFT;
    public Vector3 rightLeftPos;
    public GameObject rightDpadRIGHT;
    public Vector3 rightRightPos;


    private Vector3 lrShiftRight = new Vector3(0.3f, 0.0f, 0.0f);
    private Vector3 lrShiftLeft = new Vector3(0.0f, 0.0f, 0.3f);
    private Vector3 lrShiftUp = new Vector3(0.0f, 0.3f, 0.0f);
    private Vector3 lrShiftDown = new Vector3(0.0f, -0.3f, 0.0f);

    private bool wandLeftInfoDisplayed, wandRightInfoDisplayed;
    private bool showHelpLeft, showHelpRight;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    void LateUpdate()
    {
        //RightWAnd
        if (wandRight.transform.parent)
        {
            if (wandRight.GetComponent<WandController>().MenuDown())
            {
                wandRightInfoDisplayed = SelSatMenu.GetComponent<SelectedSatellite>().satInfo.activeSelf;

                SelSatMenu.GetComponent<SelectedSatellite>().hideInfo();

                showHelpRight = true;
            }

            if (wandRight.GetComponent<WandController>().MenuUp())
            {
                if (wandRightInfoDisplayed)
                {
                    SelSatMenu.GetComponent<SelectedSatellite>().showInfo();
                }

                showHelpRight = false;
            }

            if (showHelpRight)
            {
                ShowHelpRight();
            }
            else
            {
                HideHelpRight();
            }

        }

        //LeftWand
        if (wandLeft.transform.parent)
        {
            if (wandLeft.GetComponent<WandController>().MenuDown())
            {
                wandLeftInfoDisplayed = SatMenu.GetComponent<SatMenuController>().satGroupInfo.activeSelf;

                SatMenu.GetComponent<SatMenuController>().hideInfo();
                TimeMenu.GetComponent<TimeController>().hideInfo();

                showHelpLeft = true;
            }

            if (wandLeft.GetComponent<WandController>().MenuUp())
            {

                if (wandLeftInfoDisplayed)
                {
                    SatMenu.GetComponent<SatMenuController>().showInfo();
                    TimeMenu.GetComponent<TimeController>().showInfo();
                }

                showHelpLeft = false;

            }

            if (showHelpLeft)
            {
                ShowHelpLeft();
            }
            else
            {
                HideHelpLeft();
            }
        }
    }



    private void ShowHelpRight()
    {

        //RightWand
        Vector3 wandRightPos = wandRight.transform.position;
        Quaternion wandRightRot = wandRight.transform.rotation * Quaternion.Euler(95, 0, 0);

        rightTrigger.SetActive(true);
        rightGrip.SetActive(true);
        rightMenu.SetActive(false);
        rightDpadDOWN.SetActive(true);
        rightDpadUP.SetActive(true);
        rightDpadLEFT.SetActive(true);
        rightDpadRIGHT.SetActive(true);

        SetMenuObject(rightTrigger, rightTriggerPos, wandRight, wandRightPos, wandRightRot, "trigger");
        SetMenuObject(rightGrip, rightGripPos, wandRight, wandRightPos, wandRightRot, "lgrip");
        //SetMenuObject(rightMenu, rightMenuPos, wandRight, wandRightPos, wandrightRot, "button");
        SetMenuObject(rightDpadDOWN, rightDownPos, wandRight, wandRightPos, wandRightRot, "trackpad", lrShiftDown);
        SetMenuObject(rightDpadUP, rightUpPos, wandRight, wandRightPos, wandRightRot, "trackpad", lrShiftUp);
        SetMenuObject(rightDpadLEFT, rightLeftPos, wandRight, wandRightPos, wandRightRot, "trackpad", lrShiftLeft);
        SetMenuObject(rightDpadRIGHT, rightRightPos, wandRight, wandRightPos, wandRightRot, "trackpad", lrShiftRight);

    }

    private void ShowHelpLeft()
    {
        //Left Wand
        Vector3 wandLeftPos = wandLeft.transform.position;
        Quaternion wandLeftRot = wandLeft.transform.rotation * Quaternion.Euler(95, 0, 0);

        leftTrigger.SetActive(true);
        leftGrip.SetActive(true);
        leftMenu.SetActive(false);
        leftDpadDOWN.SetActive(true);
        leftDpadUP.SetActive(true);
        leftDpadLEFT.SetActive(true);
        leftDpadRIGHT.SetActive(true);

        SetMenuObject(leftTrigger, leftTriggerPos, wandLeft, wandLeftPos, wandLeftRot, "trigger");
        SetMenuObject(leftGrip, leftGripPos, wandLeft, wandLeftPos, wandLeftRot, "rgrip");
        //SetMenuObject(leftMenu, leftMenuPos, wandLeft, wandLeftPos, wandLeftRot, "button");
        SetMenuObject(leftDpadDOWN, leftDownPos, wandLeft, wandLeftPos, wandLeftRot, "trackpad", lrShiftDown);
        SetMenuObject(leftDpadUP, leftUpPos, wandLeft, wandLeftPos, wandLeftRot, "trackpad", lrShiftUp);
        SetMenuObject(leftDpadLEFT, leftLeftPos, wandLeft, wandLeftPos, wandLeftRot, "trackpad", lrShiftLeft);
        SetMenuObject(leftDpadRIGHT, leftRightPos, wandLeft, wandLeftPos, wandLeftRot, "trackpad", lrShiftRight);

    }

    private void SetMenuObject(GameObject menuObj, Vector3 menuObjPos, GameObject wand, Vector3 wandPos, Quaternion wandRot, string wandObjName)
    {
        menuObj.transform.position = wandPos;
        menuObj.transform.Translate(menuObjPos);
        menuObj.transform.rotation = wandRot;

        try
        {
            menuObj.GetComponent<LineRenderer>().SetVertexCount(2);
            menuObj.GetComponent<LineRenderer>().SetPosition(0, menuObj.transform.position);
            menuObj.GetComponent<LineRenderer>().SetPosition(1, wand.transform.GetChild(0).transform.Find(wandObjName).transform.GetChild(0).transform.position);

        }
        catch (System.NullReferenceException) { }

    }

    private void SetMenuObject(GameObject menuObj, Vector3 menuObjPos, GameObject wand, Vector3 wandPos, Quaternion wandRot, string wandObjName, Vector3 lineRendererShift)
    {
        menuObj.transform.position = wandPos;
        menuObj.transform.Translate(menuObjPos);
        menuObj.transform.rotation = wandRot;
        lineRendererShift = wandRot * lineRendererShift;

        try
        {
            menuObj.GetComponent<LineRenderer>().SetVertexCount(2);
            menuObj.GetComponent<LineRenderer>().SetPosition(0, menuObj.transform.position);
            menuObj.GetComponent<LineRenderer>().SetPosition(1, wand.transform.GetChild(0).transform.Find(wandObjName).transform.GetChild(0).transform.position + lineRendererShift);

        }
        catch (System.NullReferenceException) { }

    }

    private void HideHelpRight()
    {

        rightTrigger.SetActive(false);
        rightGrip.SetActive(false);
        rightMenu.SetActive(true);
        rightDpadDOWN.SetActive(false);
        rightDpadUP.SetActive(false);
        rightDpadLEFT.SetActive(false);
        rightDpadRIGHT.SetActive(false);

        Vector3 wandRightPos = wandRight.transform.position;
        Quaternion wandRightRot = wandRight.transform.rotation * Quaternion.Euler(95, 0, 0);

        SetMenuObject(rightMenu, rightMenuPos, wandRight, wandRightPos, wandRightRot, "button");

    }

    private void HideHelpLeft()
    {
        leftTrigger.SetActive(false);
        leftGrip.SetActive(false);
        leftMenu.SetActive(true);
        leftDpadDOWN.SetActive(false);
        leftDpadUP.SetActive(false);
        leftDpadLEFT.SetActive(false);
        leftDpadRIGHT.SetActive(false);

        Vector3 wandLeftPos = wandLeft.transform.position;
        Quaternion wandLeftRot = wandLeft.transform.rotation * Quaternion.Euler(95, 0, 0);

        SetMenuObject(leftMenu, leftMenuPos, wandLeft, wandLeftPos, wandLeftRot, "button");

    }


}
