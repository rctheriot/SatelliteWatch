using UnityEngine;
using UnityEngine.UI;

public class MissleController : MonoBehaviour
{
    public GameObject targetSatellite;
    public GameObject explosion;

    private float thrust, angleOfTurn;

    void Start()
    {
        thrust = 85f;
        angleOfTurn = (3 * Mathf.PI / 2);

        transform.LookAt(transform.position * 4);
        transform.Rotate(90, 0, 0);

    }

    void Update()
    {
        checkTargetActive();
        trackTarget();
    }

    public void setTarget(GameObject target)
    {
        targetSatellite = target;
    }

    private void checkTargetActive()
    {
        try
        {
            if (targetSatellite.transform.parent.gameObject.activeSelf == false)
            {
                Destroy(gameObject);
            }
        }
        catch (System.NullReferenceException)
        {
            Destroy(gameObject);
        }

    }

    private void trackTarget()
    {
        //Look Towards Target
        float maxAngularSpeedDegrees = (angleOfTurn * WorldTime.getTimeMultiplier() * Time.deltaTime) / 1000;
        Vector3 targetRot = targetSatellite.transform.position - transform.position;
        transform.up = Vector3.RotateTowards(transform.up, targetRot, maxAngularSpeedDegrees, 0f);

        //Move Towards Target
        Vector3 movement = (Vector3.up * thrust * WorldTime.getTimeMultiplier() * Time.deltaTime) / 1000;
        transform.Translate(movement);

        //Random Movement to cause more interesting missiles
        System.Random random = new System.Random();
        float randomMove = (random.Next(95) * Time.deltaTime * WorldTime.getTimeMultiplier()) / 1000;
        transform.Rotate(new Vector3(randomMove * 1.7f, randomMove * 3.1f, randomMove * 2.3f));

    }

    //If the missile collides with something
    void OnTriggerEnter(Collider other)
    {
        //If the missile collided with Earth
        if (other.gameObject.tag.Equals("Earth"))
        {
            GameObject earthExplosion = (GameObject)Instantiate(explosion, transform.position, Quaternion.identity);
            earthExplosion.GetComponent<AudioSource>().Play();
            Destroy(earthExplosion, 10);
            Destroy(gameObject);
        }

        //If the missile collided with a Satellite (might not be its target satellite)
        //Goes through all the missiles active right now to make sure their target hasn't be destryoed
        //If it has be destryoed the missile is removed
        else
        {
            foreach (Transform child in GameObject.Find("Missiles").transform)
            {
                if (child.Equals(this.transform)) { } //Do nothing if the child is THIS gameObject
                else if (other.name.Equals(child.GetComponent<MissleController>().getTargetSatellite().name))
                {
                    Destroy(child.gameObject);
                }
            }

            AddTextToCanvas(other.name + " destryoed!", GameObject.Find("Canvas"));

            GameObject newExplosion = (GameObject)Instantiate(explosion, transform.position, Quaternion.identity);
            newExplosion.GetComponent<AudioSource>().Play();
            Destroy(newExplosion, 10);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

    }

    public GameObject getTargetSatellite()
    {
        return targetSatellite;
    }

    private void AddTextToCanvas(string textString, GameObject canvasGameObject)
    {
        try
        {
            Text text = canvasGameObject.AddComponent<Text>();
            text.text = textString;

            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.font = ArialFont;
            text.material = ArialFont.material;
            Destroy(text, 1.5f);
        }
        catch (System.NullReferenceException) { }

    }

}
