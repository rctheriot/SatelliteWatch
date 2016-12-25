using UnityEngine;
using Zeptomoby.OrbitTools;

/// <summary>
/// The Earth is always located at 0,0,0 and roates in the ECI (Earth-centered inertial) model.
/// So the easiest way to solve Earth's rotation and have it accurate and in sync with the satellites
/// was to create a location in OrbitTools of a site on Earth and point the Earth towards that location.
/// The selected site was the crossing of the Prime Meridian and Equator at LAT 00.0 and LONG 000.0.
/// We just update this location in Update(), and point the Earth model towards that site.
/// </summary>

public class EarthController : MonoBehaviour
{
    private Site meridianEquatorCross;
    private Eci eci;
    private Vector3 direction;
    private float xPos, yPos, zPos;

    void Start()
    {
        meridianEquatorCross = new Site(0, 0, 500, "Origin");
    }

    void FixedUpdate()
    {
        eci = meridianEquatorCross.PositionEci(WorldTime.getUTCTime());

        direction = new Vector3((float)eci.Position.X, (float)eci.Position.Z, (float)eci.Position.Y) / 1000;

        transform.LookAt(direction);
    }

}