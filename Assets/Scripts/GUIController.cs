using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    public static GUIController instance;
    //PLANNINGPHATE CANVAS WITH ELEMENTS
    public Canvas planningCanvas;
    public Button instantiateButton;
    //

    //HQ UPGRADE CANVAS WITH ELEMENTS
    public Canvas upgradeTrainCanvas;
    public Button upgradeTrainButton;
    public Text phaseText;
    public Text ResourcesText;
    public Text trainSpeedUpgradeText;
    public Text trainHealthUpgradeText;
    public Text trainSprintUpgradeText;
    public Slider healthSlider;
    public Button trainSpeedUpgradeButton;
    public Button trainHealthUpgradeButton;
    public Button trainSprintUpgradeButton;
    //

    public Button trainSprintButton;
    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        phaseText.text = "PLANNING PHASE";

    }

    // Update is called once per frame
    void Update()
    {
        ResourcesText.text = "Resources: " + GameController.instance.totalResources.ToString();
    }

    public void StartGame()
    {
        //THE FIRST CANVAS ACTIVE AT THE BEGINNING OF THE GAME
        phaseText.text = "ACTION PHASE";
        //if (upgradeTrainCanvas.gameObject.activeSelf)
       //     upgradeTrainCanvas.gameObject.SetActive(false);
       
        planningCanvas.gameObject.SetActive(false);
    }

    //ACTIVATON OF BUTTON THAT CAN BE PRESSED
    public void ActivateInstanceButton()
    {

        instantiateButton.interactable = true;
    }

    public void DeactivateInstanceButton()
    {
        instantiateButton.interactable = false;
    }
    //CANVAS HQ UPGRADE ACTIVATION/DEACTIVATION
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
