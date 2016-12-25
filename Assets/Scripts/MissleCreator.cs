using UnityEngine;
using System.Collections;

public class MissleCreator : MonoBehaviour
{

    public GameObject MisslePrefab;
    public GameObject Home;
    public GameObject Satellites;
    public GameObject MissileGroup;
    public GameObject Explosion;

    public GameObject wandRight;

    public GameObject warning;
    public Vector3 warningLocalPosition;
    public Color missileFired, noTarget;
    private float warningStart;

    [Header("Audio")]
    public AudioClip fireMissileAudio;
    public AudioClip failFireMissileAudio;

    private AudioSource wandRightAudio;

    void Start()
    {
        wandRightAudio = wandRight.GetComponent<AudioSource>();

        warning.SetActive(true);
    }


    void Update()
    {
        warning.transform.position = wandRight.transform.position;
        warning.transform.Translate(warningLocalPosition);

        warning.transform.rotation = wandRight.transform.rotation;
        warning.transform.Rotate(new Vector3(45, 0, 0));

        if (wandRight.GetComponent<WandController>().TouchpadDown() && wandRight.GetComponent<WandController>().DpadUP())
        {
            GameObject[] eligbleTargets;
            eligbleTargets = GameObject.FindGameObjectsWithTag("Satellite");

            if (eligbleTargets.Length != 0)
            {
                wandRightAudio.clip = fireMissileAudio;
                wandRightAudio.Play();

                System.Random random = new System.Random();
                int randomTarget = random.Next(eligbleTargets.Length);
                GameObject target = eligbleTargets[randomTarget];

                Vector3 homePos = new Vector3(Home.transform.position.x, Home.transform.position.y, Home.transform.position.z);

                GameObject newMissle = (GameObject)Instantiate(MisslePrefab, homePos, Quaternion.identity);

                newMissle.transform.SetParent(MissileGroup.transform);
                newMissle.GetComponent<MissleController>().setTarget(target);
                newMissle.GetComponent<MissleController>().explosion = Explosion;
                newMissle.name = "Missle - Target : " + target.name.Trim();

                showFireMissile();


            }
            else
            {
                wandRightAudio.clip = failFireMissileAudio;
                wandRightAudio.Play();

                showNoTarget();
            }
        }


        if (Time.time - warningStart > 3)
        {
            hideWarning();
        }

    }

    public void showNoTarget()
    {

        warning.GetComponent<TextMesh>().text = "NO TARGET";
        warning.GetComponent<TextMesh>().color = noTarget;
        warning.SetActive(true);
        warningStart = Time.time;
    }

    public void showFireMissile()
    {

        warning.GetComponent<TextMesh>().text = "MISSILE LAUNCHED";
        warning.GetComponent<TextMesh>().color = missileFired;
        warning.SetActive(true);
        warningStart = Time.time;
    }

    public void hideWarning()
    {
        warning.SetActive(false);
    }
}
