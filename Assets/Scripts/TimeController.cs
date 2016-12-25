using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the time and updates the WorldTime static class
/// Gets the sliderPosition from the TimeSlider Object, On Value Change in the inspector window
/// </summary>
public class TimeController : MonoBehaviour
{

    public GameObject timeMultiText;
    public Vector3 timeMultiPosition;

    public GameObject timeText;
    public Vector3 timeTextPosition;

    public GameObject wandLeft;
    public GameObject wandRight;

    private float timeMultiplier;


    [Header("Audio")]
    public AudioClip timeChangeAudio;
    private AudioSource wandLeftAudio;
    private AudioSource wandRightAudio;

    private int timeAudioCheck;

    void Start()
    {
        timeMultiplier = 1;
        timeAudioCheck = 1;

        timeMultiText.SetActive(true);
        timeText.SetActive(true);

        wandLeftAudio = wandLeft.GetComponent<AudioSource>();
        wandRightAudio = wandRight.GetComponent<AudioSource>();

    }

    void Update()
    {
        if (wandLeft.GetComponent<WandController>().GripPress())
        {
            //timeMultiplier -= wandLeft.GetComponent<WandController>().TriggerAxis();
            timeMultiplier -= 0.25f;
        }
        else
        {
            wandLeftAudio.pitch = 1.0f;

        }

        if (wandRight.GetComponent<WandController>().GripPress())
        {

            //timeMultiplier += wandRight.GetComponent<WandController>().TriggerAxis();
            timeMultiplier += 0.25f;
        }
        else
        {
            wandRightAudio.pitch = 1.0f;
        }

        WorldTime.setTimeMultiplier((int)timeMultiplier);
        WorldTime.updateTime();
        updateTimeUIText();

        AudioCheck();

        SetPositionOfChildren();

        if (wandLeft.GetComponent<WandController>().TouchpadDown() && wandLeft.GetComponent<WandController>().DpadDOWN())
        {
            timeMultiText.SetActive(!timeMultiText.activeSelf);
            timeText.SetActive(!timeText.activeSelf);
        }


    }

    private void AudioCheck()
    {
        if (timeAudioCheck - (int)WorldTime.timeMultiplier > 0)
        {
            wandLeftAudio.clip = timeChangeAudio;
            wandLeftAudio.pitch = 0.75f;
            wandLeftAudio.Play();
            timeAudioCheck = (int)WorldTime.timeMultiplier;
        }

        else if ((int)WorldTime.timeMultiplier - timeAudioCheck > 0)
        {
            wandRightAudio.clip = timeChangeAudio;
            wandRightAudio.pitch = 1.25f;
            wandRightAudio.Play();
            timeAudioCheck = (int)WorldTime.timeMultiplier;
        }
        else
        {
            wandLeftAudio.pitch = 1.0f;
            wandRightAudio.pitch = 1.0f;
        }

    }

    private void updateTimeUIText()
    {
        timeText.GetComponent<TextMesh>().text = WorldTime.getUTCTime().ToString() + " UTC" + System.Environment.NewLine +
                        WorldTime.getLocalTime().ToString() + " Local";


        float minutes = Mathf.Floor(WorldTime.getTimeMultiplier() / 60);
        float seconds = Mathf.Abs(WorldTime.getTimeMultiplier() % 60);

        char sign = '+';

        if (WorldTime.getTimeMultiplier() < 0)
        {
            sign = '-';
            minutes = Mathf.Ceil(WorldTime.getTimeMultiplier() / 60);
            minutes = Mathf.Abs(minutes);
        }

        timeMultiText.GetComponent<TextMesh>().text = "Time Multiplier: " + sign + minutes.ToString("#.") + ":" + seconds.ToString("0#") + "/sec";
    }

    private void SetPositionOfChildren()
    {
        transform.position = wandLeft.transform.position;
        timeMultiText.transform.position = wandLeft.transform.position;
        timeText.transform.position = wandLeft.transform.position;

        timeMultiText.transform.Translate(timeMultiPosition);
        timeText.transform.Translate(timeTextPosition);

        transform.rotation = wandLeft.transform.rotation;

        transform.Rotate(new Vector3(45, 0, 0));

    }

    public void hideInfo()
    {
        timeMultiText.SetActive(false);
        timeText.SetActive(false);
    }

    public void showInfo()
    {
        timeMultiText.SetActive(true);
        timeText.SetActive(true);
    }
}
