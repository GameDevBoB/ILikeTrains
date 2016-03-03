using UnityEngine;
using System.Collections;

public class TerrainScript : MonoBehaviour
{
    public bool isPlaceable;

    private Renderer myRenderer;
    private Color startColor;

    void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        startColor = myRenderer.material.color;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



    }

    public void BackToNormalColor()
    {
        myRenderer.material.color = startColor;
    }

    public void ChangeColor()
    {
        myRenderer.material.color = (isPlaceable) ? Color.green : Color.red;
        GameController.instance.isPlaceable = isPlaceable;
    }

    void SetUnplaceable()
    {
        isPlaceable = false;
        gameObject.layer = LayerMask.NameToLayer("Unplaceable");
    }
}
