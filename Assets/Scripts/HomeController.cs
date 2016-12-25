using UnityEngine;
using Zeptomoby.OrbitTools;

public class HomeController : MonoBehaviour
{
    public Transform earth;

    private Tle tle;
    private Site currentSite;
    private Eci eci;

    private Rigidbody rb;
    private Vector3 position;

    private Site[] siteList = new Site[14];
    private int currentSiteNum = 0;

    public GameObject wandRight;

    [Header("Audio")]
    public AudioClip leftSelect;
    public AudioClip rightSelect;

    private AudioSource wandRightAudio;

    public void Start()
    {

        loadSites();
        currentSite = siteList[currentSiteNum];
        rb = GetComponent<Rigidbody>();

        wandRightAudio = wandRight.GetComponent<AudioSource>();

    }


    void Update()
    {
        transform.LookAt(earth);

        eci = currentSite.PositionEci(WorldTime.getUTCTime());
        position = new Vector3((float)eci.Position.X, (float)eci.Position.Z, (float)eci.Position.Y) / 1000;

        rb.MovePosition(position);

        if (wandRight.GetComponent<WandController>().TouchpadDown() && wandRight.GetComponent<WandController>().DpadRIGHT())
        {
            changeSiteRight();

            wandRightAudio.clip = rightSelect;
            wandRightAudio.Play();
        }

        if (wandRight.GetComponent<WandController>().TouchpadDown() && wandRight.GetComponent<WandController>().DpadLEFT())
        {
            changeSiteLeft();

            wandRightAudio.clip = leftSelect;
            wandRightAudio.Play();
        }


    }

    public Site getSite()
    {
        return currentSite;
    }

    private void changeSiteRight()
    {
        currentSiteNum++;
        if (currentSiteNum == siteList.Length) currentSiteNum = 0;
        currentSite = siteList[currentSiteNum];
    }

    private void changeSiteLeft()
    {
        currentSiteNum--;
        if (currentSiteNum < 0) currentSiteNum = siteList.Length - 1;
        currentSite = siteList[currentSiteNum];
    }

    private void loadSites()
    {
        siteList[0] = new Site(21.2970, -157.8170, 500, "Honolulu");
        siteList[1] = new Site(37.7833, -122.4167, 500, "San Francisco");
        siteList[2] = new Site(40.7127, -74.0059, 500, "New York");
        siteList[3] = new Site(-34.6033, -58.3817, 500, "Buenos Aires");
        siteList[4] = new Site(52.5167, 13.3833, 500, "Berlin");
        siteList[5] = new Site(55.7500, 37.6167, 500, "Moscow");
        siteList[6] = new Site(51.5072, -0.1275, 500, "London");
        siteList[6] = new Site(35.6833, 139.6833, 500, "Tokyo");
        siteList[7] = new Site(37.5667, 126.9667, 500, "Seoul");
        siteList[8] = new Site(28.6139, 77.2090, 500, "Delhi");
        siteList[9] = new Site(39.9167, 116.3833, 500, "Beijing");
        siteList[10] = new Site(48.8567, 2.3508, 500, "Paris");
        siteList[11] = new Site(-22.9068, -43.1729, 500, "Rio de Janerio");
        siteList[12] = new Site(30.0500, 31.2333, 500, "Cairo");
        siteList[13] = new Site(-33.9253, 18.4239, 500, "Cape Town");
    }


}
