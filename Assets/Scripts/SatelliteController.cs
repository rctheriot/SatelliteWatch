using UnityEngine;
using Zeptomoby.OrbitTools;

public class SatelliteController : MonoBehaviour
{
    private Transform earth;

    public Tle tle;
    private Satellite sat;
    private Eci eci;

    private Rigidbody rb;
    private Vector3 position;
    private float scale;

    private int updateFrame;
    private Behaviour highlight;
    private bool trailEraseOnEnable;

    void Start()
    {
        earth = GameObject.FindGameObjectWithTag("Sun").transform;
        trailEraseOnEnable = true;
        disableHighlight();

    }

    public void create(string line1, string line2, string line3)
    {
        tle = new Tle(line1, line2, line3);
        sat = new Satellite(tle);
        rb = GetComponent<Rigidbody>();
        highlight = (Behaviour)GetComponent("Halo");
    }

    void FixedUpdate()
    {
        updateFrame++;
        if (updateFrame % 3 == 0)
        {

            try
            {
                lookAndScale();
                eci = sat.PositionEci(WorldTime.getUTCTime());
                position = new Vector3((float)eci.Position.X, (float)eci.Position.Z, (float)eci.Position.Y) / 1000;
                rb.MovePosition(position);

            }
            catch (DecayException)
            {
                Debug.Log(tle.Name.Trim() + " : INFORMATION IS DECAYED, OBJECT REMOVED");
                Destroy(gameObject);
            }
        }

        if (trailEraseOnEnable)
        {
            gameObject.GetComponent<TrailRenderer>().Clear();
            trailEraseOnEnable = false;
        }

    }

    void OnEnable()
    {
        trailEraseOnEnable = true;
        gameObject.GetComponent<TrailRenderer>().Clear();
    }

    void OnDisable()
    {
        gameObject.GetComponent<TrailRenderer>().Clear();
    }


    public float getVelocity()
    {
        return Vector3.Magnitude(new Vector3((float)eci.Velocity.X, (float)eci.Velocity.Z, (float)eci.Velocity.Y));
    }

    public Tle getTle()
    {
        return tle;
    }

    public void disableHighlight()
    {
        highlight.enabled = false;
    }

    public void enableHighlight()
    {
        highlight.enabled = true;
    }

    private void lookAndScale()
    {
        float distance = Vector3.Distance(transform.position, earth.position);

        scale = 1.2f * Mathf.Log10(distance);

        scale = Mathf.Clamp(scale, 0.6f, 4.5f);

        //transform.localScale = new Vector3(scale, scale, scale);

        //Looks at camera. rotate.LookAt() has quads facing in opposite direction
        transform.LookAt(earth);
        transform.Rotate(90, 0, 0);
    }

}
