using UnityEngine;
using System;

/// <summary>
/// Since the Earth is the center in this model, we rotate the sun around the Earth.
/// The Sun's revolution around the Earth is tilted at 23.5, Earth's real world tilt.
/// We treat the positive X-Axis as pointing toward the Vernal equinox.
/// We know the Sun is located on the X-Axis on the Vernal Equinox, so that is its starting position.
/// We get the time since vernal equinox and add/subtract them and add the correct degree of rotation in Start() to put the sun in the correct location.
/// Unity appears to not like rotating something less than a degree.
/// So every second the amount of degEarthTransPerSec is added to itself, using the InvokeRepeating in Start()
/// Once it equals a number greater than 1 or less than -1, the sun will then rotate 1 degree accordingly.
/// </summary>

public class SunController : MonoBehaviour
{
    private const float earthTilt = 0.409092627749f;  //Earth's Tilt in radians
    private double degEarthTransPerSec = 0.0000115741; //The degrees earth transits around the sun per second

    void Start()
    {
        DateTime vernalEquinoxTime = new DateTime(2016, 3, 20, 4, 30, 0, DateTimeKind.Utc);
        double timeSinceVernalQuinox = (WorldTime.worldTime - vernalEquinoxTime).TotalSeconds;
        degEarthTransPerSec = timeSinceVernalQuinox * 0.0000115741;
        transform.LookAt(Vector3.zero);

        InvokeRepeating("addDegrees", 1, 1);  //addDegrees is called every second
    }

    void FixedUpdate()
    {
        while (degEarthTransPerSec >= 1)
        {
            transform.LookAt(Vector3.zero);
            transform.RotateAround(Vector3.zero, new Vector3(0, Mathf.Cos(-earthTilt), Mathf.Sin(-earthTilt)), -1);
            degEarthTransPerSec--;
        }

        while (degEarthTransPerSec <= -1)
        {
            transform.LookAt(Vector3.zero);
            transform.RotateAround(Vector3.zero, new Vector3(0, Mathf.Cos(-earthTilt), Mathf.Sin(-earthTilt)), 1);
            degEarthTransPerSec++;
        }
    }

    void addDegrees()
    {
        degEarthTransPerSec += (WorldTime.getTimeMultiplier() * 0.0000115741);
    }

}
