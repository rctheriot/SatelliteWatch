using System.Net;
using UnityEngine;

/// <summary>
/// Downloads and saves the satellite data (TLE) from Celestrak.com into Unity's persistantDataPath in .txt files
/// Creates all the Satellites from the saved .txt files using the SatellitePrefab. 
/// </summary>
public class SatelliteCreator : MonoBehaviour
{

    public bool downloadSatData;

    public GameObject SatellitePrefab;

    private WebClient wClient;

    private string gpsSats = "http://www.celestrak.com/NORAD/elements/gps-ops.txt";
    private string weatherSats = "http://www.celestrak.com/NORAD/elements/weather.txt";
    private string noaaSats = "http://www.celestrak.com/NORAD/elements/noaa.txt";
    private string stationsSats = "http://www.celestrak.com/NORAD/elements/stations.txt";
    private string geosSats = "http://www.celestrak.com/NORAD/elements/goes.txt";
    private string resourceSats = "http://www.celestrak.com/NORAD/elements/resource.txt";
    private string visualSats = "http://www.celestrak.com/NORAD/elements/visual.txt";
    private string sarSats = "http://celestrak.com/NORAD/elements/sarsat.txt";
    private string disasterSats = "http://celestrak.com/NORAD/elements/dmc.txt";
    private string tdrssSats = "http://celestrak.com/NORAD/elements/tdrss.txt";
    private string gorizontSats = "http://celestrak.com/NORAD/elements/gorizont.txt";
    private string molniyaSats = "http://celestrak.com/NORAD/elements/molniya.txt";
    private string iridiumSats = "http://celestrak.com/NORAD/elements/iridium.txt";
    private string orbcommSats = "http://www.celestrak.com/NORAD/elements/orbcomm.txt";
    private string globalstarSats = "http://www.celestrak.com/NORAD/elements/globalstar.txt";
    private string amateurradioSats = "http://www.celestrak.com/NORAD/elements/amateur.txt";
    private string glonassSats = "http://www.celestrak.com/NORAD/elements/glo-ops.txt";
    private string galileoSats = "http://www.celestrak.com/NORAD/elements/galileo.txt";
    private string beidouSats = "http://www.celestrak.com/NORAD/elements/beidou.txt";
    private string nnssSats = "http://www.celestrak.com/NORAD/elements/nnss.txt";
    private string cubeSats = "http://www.celestrak.com/NORAD/elements/cubesat.txt";

    void Awake()
    {
        if (downloadSatData)
        {
            downloadSatInfo();
        }

        loadSats();
    }

    private void downloadSatInfo()
    {
        wClient = new WebClient();

        requestTLEData(gpsSats, "gpsTLE.txt");
        requestTLEData(weatherSats, "weatherTLE.txt");
        requestTLEData(noaaSats, "noaaTLE.txt");
        requestTLEData(stationsSats, "stationsTLE.txt");
        requestTLEData(geosSats, "goesTLE.txt");
        requestTLEData(resourceSats, "resourceTLE.txt");
        requestTLEData(visualSats, "visualTLE.txt");
        requestTLEData(sarSats, "sarsatTLE.txt");
        requestTLEData(disasterSats, "disasterTLE.txt");
        requestTLEData(tdrssSats, "tdrssTLE.txt");
        requestTLEData(gorizontSats, "gorizontTLE.txt");
        requestTLEData(molniyaSats, "molniyaTLE.txt");
        requestTLEData(iridiumSats, "iridiumTLE.txt");
        requestTLEData(orbcommSats, "orbcommTLE.txt");
        requestTLEData(globalstarSats, "globalstarTLE.txt");
        requestTLEData(amateurradioSats, "amateurTLE.txt");
        requestTLEData(glonassSats, "glonassTLE.txt");
        requestTLEData(galileoSats, "galileoTLE.txt");
        requestTLEData(beidouSats, "beidouTLE.txt");
        requestTLEData(nnssSats, "nnssTLE.txt");
        requestTLEData(cubeSats, "cubeTLE.txt");

        wClient.Dispose();
    }

    private void loadSats()
    {
        loadSatGroup("gpsTLE.txt", "GPS");
        loadSatGroup("weatherTLE.txt", "Weather");
        loadSatGroup("noaaTLE.txt", "NOAA");
        loadSatGroup("stationsTLE.txt", "SpaceStations");
        loadSatGroup("goesTLE.txt", "GOES");
        loadSatGroup("resourceTLE.txt", "EarthResources");
        loadSatGroup("visualTLE.txt", "Visual");
        loadSatGroup("sarsatTLE.txt", "SARSAT");
        loadSatGroup("disasterTLE.txt", "Disaster");
        loadSatGroup("tdrssTLE.txt", "TDRSS");
        loadSatGroup("gorizontTLE.txt", "Gorizont");
        loadSatGroup("molniyaTLE.txt", "Molniya");
        loadSatGroup("iridiumTLE.txt", "Iridium");
        loadSatGroup("orbcommTLE.txt", "Orbcomm");
        loadSatGroup("globalstarTLE.txt", "Globalstar");
        loadSatGroup("amateurTLE.txt", "Amateur Radio");
        loadSatGroup("glonassTLE.txt", "Glonass");
        loadSatGroup("galileoTLE.txt", "Galileo");
        loadSatGroup("beidouTLE.txt", "Beidou");
        loadSatGroup("nnssTLE.txt", "NNSS");
        loadSatGroup("cubeTLE.txt", "CubeSat");
    }


    private void loadSatGroup(string filePath, string satelliteGroupName)
    {

        string line1, line2, line3;
        System.IO.StreamReader file = new System.IO.StreamReader(Application.persistentDataPath + filePath);

        while ((line1 = file.ReadLine()) != null)
        {
            line2 = file.ReadLine();
            line3 = file.ReadLine();

            GameObject newSat = (GameObject)Instantiate(SatellitePrefab, new Vector3(0, 0, 0), Quaternion.identity);

            newSat.name = line1.Trim();
            newSat.transform.parent = GameObject.Find("Satellites(" + satelliteGroupName + ")").transform;

            newSat.GetComponent<SatelliteController>().create(line1, line2, line3);

        }
        file.Close();

        //Disables all Sattllites except the GPS
        if (satelliteGroupName != "GPS")
        {
            GameObject.Find("Satellites(" + satelliteGroupName + ")").SetActive(false);
        }

    }

    private void requestTLEData(string satelliteGroup, string saveFileName)
    {
        wClient.DownloadFile(satelliteGroup, Application.persistentDataPath + saveFileName);
        Debug.Log("Successfull TLE Download for: " + satelliteGroup);
    }

}
