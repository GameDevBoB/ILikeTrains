using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
    public static GUIController instance;
    public Canvas planningCanvas;
    public Button instantiateButton;
    public Text phaseText;

	public Text ResourcesText;


    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        phaseText.text = "PLANNING PHASE";

    }
	
	// Update is called once per frame
	void Update () {
		ResourcesText.text ="Resources: "+ GameController.instance.totalResources.ToString();
	}

    public void StartGame()
    {
        phaseText.text = "ACTION PHASE";
        planningCanvas.gameObject.SetActive(false);
    }

    public void ActivateInstanceButton()
    {
        instantiateButton.interactable=true;
    }

    public void DeactivateInstanceButton()
    {
        instantiateButton.interactable = false;
    }

}
