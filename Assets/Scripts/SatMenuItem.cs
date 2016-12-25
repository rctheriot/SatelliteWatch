using UnityEngine;
using System.Collections;

public class SatMenuItem : MonoBehaviour
{

    public GameObject satGroup;

    private Color currentColor;


    void Start()
    {

        this.GetComponentInChildren<TextMesh>().text = transform.name;
    }


    public void SetColor(Color color)
    {
        currentColor = color;

        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Standard");
        rend.material.SetColor("_Color", currentColor);
    }

    public void SetOpacity(float opacity)
    {
        currentColor.a = opacity;

        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Standard");
        rend.material.SetColor("_Color", currentColor);

        Color textColor = this.GetComponentInChildren<TextMesh>().color;
        textColor.a = opacity;
        this.GetComponentInChildren<TextMesh>().color = textColor;

    }

    public void Activate()
    {
        satGroup.SetActive(!satGroup.activeSelf);
    }
}
