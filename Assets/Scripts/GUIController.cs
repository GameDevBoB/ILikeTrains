using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUIController : MonoBehaviour
{
    public static GUIController instance;
    //PLANNINGPHATE CANVAS WITH ELEMENTS
    public Canvas planningCanvas;
    public Button startButton;
    public Button instantiateDynamiteButton;
    public Button instantiateTeslaButton;
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

    //UI ELEMENTS FOR UPGRADES

    public Canvas upgradeCanvas;
    public Text damageUpgradeText;
    public Text radiusUpgradeText;
    public Text cooldownUpgradeText;
    public Button damageUpgradeButton;
    public Button radiusUpgradeButton;
    public Button cooldownUpgradeButton;
    public GameObject canvasOpener;

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
    public void ActivateTrapsButton()
    {

        instantiateDynamiteButton.interactable = true;
        instantiateTeslaButton.interactable = true;
    }

    public void DeactivateTrapsButton()
    {
        instantiateDynamiteButton.interactable = false;
        instantiateTeslaButton.interactable = false;


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

    public void CloseUpgrade()
    {
        upgradeCanvas.gameObject.SetActive(false);

    }

    public void SetCanvasElements(trapType inputMyType, float inputDamage, float inputRange, float inputCooldown,
        float inputCostDamage, float inputCostRange, float inputCostCooldown)
    {
        switch (inputMyType)
        {
            case trapType.Dynamite:
                damageUpgradeText.text = "Damage " + inputDamage;
                break;
            case trapType.MinaTesla:
                damageUpgradeText.text = "Slow " + inputDamage;
                break;
        }
        radiusUpgradeText.text = "Range " + inputRange;
        cooldownUpgradeText.text = "Cooldown " + inputCooldown;
        radiusUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + inputCostRange;
        cooldownUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + inputCostCooldown;
        damageUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + inputCostDamage;


    }

    public void SetCanvasOpener(GameObject myOpener)
    {
        canvasOpener = myOpener;
        EventTrigger trigger = radiusUpgradeButton.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.Entry click = new EventTrigger.Entry();
        EventTrigger.Entry exit = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        click.eventID = EventTriggerType.PointerClick;
        exit.eventID = EventTriggerType.PointerExit;
        entry.callback.AddListener((eventData) => { myOpener.GetComponent<Traps>().ShowPreviewRange(); });
        click.callback.AddListener((eventData) => { myOpener.GetComponent<Traps>().ShowPreviewRange(); });
        exit.callback.AddListener((eventData) => { myOpener.GetComponent<Traps>().HidePreviewRange(); });
        trigger.triggers.Clear();
        trigger.triggers.Add(entry);
        trigger.triggers.Add(click);
        trigger.triggers.Add(exit);
        // .delegates.Add(entry);
    }
   
}
