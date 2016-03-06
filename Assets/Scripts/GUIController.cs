using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {
    public static GUIController instance;
    public Canvas planningCanvas;
    public Button instantiateButton;
    public Canvas upgradeTrainCanvas;
    public Button upgradeTrainButton;
    public Text phaseText;
    public Text ResourcesText;
    public Text trainSpeedUpgradeText;
    public Text trainHealthUpgradeText;
    public Text trainSprintUpgradeText;


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
        if(upgradeTrainCanvas.gameObject.activeSelf)
        upgradeTrainCanvas.gameObject.SetActive(false);
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

    public void ActivateTrainUpgradeCanvas()
    {
        upgradeTrainCanvas.gameObject.SetActive(true);
        upgradeTrainButton.interactable = false;
    }

    public void DeactivateTrainUpgradeCanvas()
    {
        upgradeTrainCanvas.gameObject.SetActive(false);
        upgradeTrainButton.interactable = true;
    }
}
