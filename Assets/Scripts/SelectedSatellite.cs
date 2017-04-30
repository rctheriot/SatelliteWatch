using System;
using UnityEngine;
using UnityEngine.UI;
using Zeptomoby.OrbitTools;

public class SelectedSatellite : MonoBehaviour
{
    public GameObject home;

    public GameObject satInfo;
    public GameObject lavaLogo;
    public Vector3 satInfoPosition;
    public Vector3 lavaLogoPosition;

    public GameObject wandRight;

    public LineRenderer lineRenderer;
    public Color lineColorHit;
    public Color lineColorMiss;

    public GameObject halo;

    private Tle tle;
    private Satellite sat;
    private Orbit orbit;
    private Eci eci;
    private Geo geo;
    private Site site;

    private GameObject selSat;

    private Vector3 position;

    [Header("Audio")]
    public AudioClip satSelectAudio;
    public AudioClip hideInfoAudio;


    private AudioSource wandRightAudio;


    void Start()
    {

        GameObject firstSat = GameObject.Find("Satellites(GPS)").transform.GetChild(0).gameObject;
        setSatelliteStart(firstSat.GetComponent<SatelliteController>().getTle(), firstSat.gameObject);

        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.positionCount = 0;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.startWidth = 0.03f;

        satInfo.SetActive(true);
        lavaLogo.SetActive(true);

        wandRightAudio = wandRight.GetComponent<AudioSource>();

    }

    void Update()
    {


        SetPositionOfChildren();

        if (wandRight.GetComponent<WandController>().TriggerAxis() > 0.0f)
        {
            SelectSatRayCast();
        }
        else
        {
            lineRenderer.positionCount = 0;
        }

        if (wandRight.GetComponent<WandController>().TouchpadDown() && wandRight.GetComponent<WandController>().DpadDOWN())
        {

            satInfo.SetActive(!satInfo.activeSelf);
            lavaLogo.SetActive(!lavaLogo.activeSelf);

            wandRightAudio.clip = hideInfoAudio;
            wandRightAudio.Play();
        }

        UpdateHalo();

    }

    private void UpdateHalo()
    {
        if (selSat)
        {
            if (selSat.transform.parent.gameObject.activeSelf)
            {
                halo.SetActive(true);
                halo.transform.position = new Vector3((float)eci.Position.X, (float)eci.Position.Z, (float)eci.Position.Y) / 1000;
            }
            else
            {
                halo.transform.position = Vector3.zero;
            }
        }
        else
        {
            halo.transform.position = Vector3.zero;
        }
    }

    void FixedUpdate()
    {

        eci = sat.PositionEci(WorldTime.getUTCTime());

        Julian time = new Julian(WorldTime.getUTCTime());
        geo = new Geo(eci, time);

        site = new Site(geo);

        updateSatUIText();

    }

    public void setSatelliteStart(Tle selectedTLE, GameObject satObject)
    {
        selSat = satObject;
        tle = selectedTLE;
        sat = new Satellite(tle);
        orbit = new Orbit(tle);

    }

    public void setSatellite(GameObject satObject)
    {
        if (satObject != selSat)
        {
            wandRightAudio.clip = satSelectAudio;
            wandRightAudio.Play();
        }

        selSat = satObject;
        tle = satObject.GetComponent<SatelliteController>().tle;
        sat = new Satellite(tle);
        orbit = new Orbit(tle);

        updateSatUIText();


    }

    private void updateSatUIText()
    {
        Site currentHome = home.GetComponent<HomeController>().getSite();
        Topo topoLook = currentHome.GetLookAngle(sat.PositionEci(WorldTime.getUTCTime()));

        double azi = topoLook.AzimuthDeg;
        double elev = topoLook.ElevationDeg;

        float lat = (float)site.LatitudeDeg;
        float lon = (float)site.LongitudeDeg;

        string latNS = "N";
        string lonEW = "E";

        if (lon > 180) lon = lon - 360;
        if (lat < 0) latNS = "S";
        if (lon < 0) lonEW = "W";

        lat = Mathf.Abs(lat);
        lon = Mathf.Abs(lon);

        double vel = Vector3.Magnitude(new Vector3((float)this.eci.Velocity.X, (float)this.eci.Velocity.Z, (float)this.eci.Velocity.Y));

        string latitude = lat.ToString("#.000");
        string longitude = lon.ToString("#.000");
        string velocity = vel.ToString("#.000");
        string azimuth = azi.ToString("#.00");
        string elevation = elev.ToString("#.00");

        satInfo.GetComponent<TextMesh>().text = "Home:      " + currentHome.Name + System.Environment.NewLine +
                                                "Satellite: " + orbit.SatName.Trim() + System.Environment.NewLine +
                                                "Latitude:  " + latNS + " " + latitude + System.Environment.NewLine +
                                                "Longitude: " + lonEW + " " + longitude + System.Environment.NewLine +
                                                "Velocity:  " + velocity + "m /s" + System.Environment.NewLine +
                                                "Azimuth:   " + azimuth + "deg" + System.Environment.NewLine +
                                                "Elevation: " + elevation + "deg";

    }

    private void SetPositionOfChildren()
    {
        transform.position = wandRight.transform.position;

        satInfo.transform.position = wandRight.transform.position;
        lavaLogo.transform.position = wandRight.transform.position;

        satInfo.transform.Translate(satInfoPosition);
        lavaLogo.transform.Translate(lavaLogoPosition);

        transform.rotation = wandRight.transform.rotation;

        transform.Rotate(new Vector3(45, 0, 0));

    }


    private void SelectSatRayCast()
    {
        RaycastHit hit;
        Vector3 startPos = wandRight.transform.position;
        startPos += (0.75f * -wandRight.transform.up);

        Vector3 direction = wandRight.transform.forward;

        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, startPos + (direction * 1000));
        lineRenderer.startColor = lineColorMiss;
        lineRenderer.endColor = lineColorMiss;

        if (Physics.Raycast(startPos, direction, out hit, 1000.0f))
        {
            if (hit.collider.gameObject.tag.Equals("Satellite"))
            {
                setSatellite(hit.collider.gameObject);
                lineRenderer.SetPosition(1, hit.point);
                lineRenderer.startColor = lineColorHit;
                lineRenderer.endColor = lineColorHit;
            }
        }
    }

    public void hideInfo()
    {
        satInfo.SetActive(false);
        lavaLogo.SetActive(false);
    }

    public void showInfo()
    {
        satInfo.SetActive(true);
        lavaLogo.SetActive(true);
    }

}
