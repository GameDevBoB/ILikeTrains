using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum trapType
{
    MinaTesla,
    Dynamite
};
public class Traps : MonoBehaviour
{



    public trapType myType;

    //VARIABLES WE NEED TO MAKE THE TRAP BEHAVE ON THE SCENE
    public float colliderRadius = 10;
    public float damage;
    public float explosionSpeed;
    public float explosionCooldown;
    //

    //VARIABLES WE NEED TO UPGRADE THE VARIOUS TRAPS
    public float[] upgradeDamage = new float[5];
    public float[] upgradeRadius = new float[5];
    public float[] upgradeCooldown = new float[5];
    public int[] upgradeCost = new int[5];
    //


    public Canvas trapCanvas;
    public GameObject explosionSprite;
    public GameObject rangePreviewSprite;
    public Text cooldownText;
    public Image cooldownImage;
    //

    public int SlowRatio;

    //SUPPORT VARIABLES
    private SphereCollider myTrigger;
    private float explosionStart;
    private Vector3 explosionStartScale;
    private Vector3 rangeSpriteStartScale;
    private Vector3 explosionSpriteAdder;
    private Vector3 rangeSpriteAdder;
    private Renderer myRenderer;
    private Color startColor;
    private int damageUpgradeCounter;
    private int radiusUpgradeCounter;
    private int cooldownUpgradeCounter;
    private bool beginGame = true;
    [HideInInspector]
    private bool damageArrayIsEnded = false;
    [HideInInspector]
    private bool rangeArrayIsEnded = false;
    [HideInInspector]
    private bool cooldownArrayIsEnded = false;

    //

    void Awake()
    {
        myTrigger = GetComponent<SphereCollider>();
        myTrigger.radius = colliderRadius;
        //this.GetComponent<SphereCollider>().enabled = true;
        Physics.queriesHitTriggers = false;
        myRenderer = GetComponent<Renderer>();
        startColor = myRenderer.material.color;
    }

    void Start()
    {
        //SETTING STARTING PARAMETERS ON SUPPORT VARIABLES AND UI
        damageUpgradeCounter = 0;
        radiusUpgradeCounter = 0;
        cooldownUpgradeCounter = 0;
        explosionStart = 0;
        explosionStartScale = explosionSprite.transform.localScale;
        rangeSpriteStartScale = rangePreviewSprite.transform.localScale;
        explosionSpriteAdder = explosionStartScale * explosionSpeed * 2;
        //rangeSpriteAdder = rangeSpriteStartScale * 2;
        GUIController.instance.SetCanvasElements(myType, upgradeDamage[damageUpgradeCounter], upgradeRadius[radiusUpgradeCounter], upgradeCooldown[cooldownUpgradeCounter],
             upgradeCost[damageUpgradeCounter], upgradeCost[radiusUpgradeCounter], upgradeCost[cooldownUpgradeCounter]);

    }


    void FixedUpdate()
    {
        //EXPLOSION VISUALZATIONS OVER TRAPS
        if (!GameController.instance.isPaused)
        {
            if (myTrigger.radius < colliderRadius)
            {
                myTrigger.radius += explosionSpeed;
                explosionSprite.transform.localScale = new Vector3(explosionSprite.transform.localScale.x + explosionSpriteAdder.x, explosionSprite.transform.localScale.y + explosionSpriteAdder.y, explosionSprite.transform.localScale.z + explosionSpriteAdder.z);
            }
            else
            {
                myTrigger.enabled = false;
                explosionSprite.gameObject.SetActive(false);
            }

            if (trapCanvas.gameObject.activeSelf)
            {
                cooldownText.text = (explosionCooldown - (Time.time - explosionStart)).ToString("00.00");
                cooldownImage.fillAmount = (explosionCooldown - (Time.time - explosionStart)) / explosionCooldown;
                if ((Time.time - explosionStart) > explosionCooldown)
                    trapCanvas.gameObject.SetActive(false);
            }
        }
        //

        //ACTIVATING UI ON UPGRADING TRAPS INTERACTIONS
        if (GUIController.instance.upgradeCanvas.gameObject.activeSelf && GUIController.instance.canvasOpener==this.gameObject)
        {
            if (!damageArrayIsEnded && GameController.instance.totalResources >= upgradeCost[damageUpgradeCounter])
            {
                GUIController.instance.damageUpgradeButton.interactable = true;
            }
            else
            {
                GUIController.instance.damageUpgradeButton.interactable = false;
            }
            if (!rangeArrayIsEnded && GameController.instance.totalResources >= upgradeCost[radiusUpgradeCounter])
            {
                GUIController.instance.radiusUpgradeButton.interactable = true;
            }
            else
            {
                GUIController.instance.radiusUpgradeButton.interactable = false;
            }
            if (!cooldownArrayIsEnded && GameController.instance.totalResources >= upgradeCost[cooldownUpgradeCounter])
            {
                GUIController.instance.cooldownUpgradeButton.interactable = true;
            }
            else
            {
                GUIController.instance.cooldownUpgradeButton.interactable = false;
            }
        }
        //

    }

    void OnMouseOver()
    {
        //UI VISUALIZATION OF THE TRAP EXPLOSION
        if (Input.GetMouseButtonDown(0) && (((Time.time - explosionStart) > explosionCooldown) || explosionStart == 0) && !GameController.instance.isPaused)
        {
            myTrigger.enabled = true;
            explosionSprite.SetActive(true);
            explosionStart = Time.time;
            trapCanvas.gameObject.SetActive(true);
            myTrigger.radius = explosionSpeed;
            explosionSprite.transform.localScale = explosionStartScale;
        }
        //
        //UI VISUALIZATION OF THE TRAP UPGRADES
        else if (Input.GetMouseButtonDown(1) && GameController.instance.trapIsBeingPlaced)
        {
            GUIController.instance.upgradeCanvas.gameObject.SetActive(true);
            GUIController.instance.SetCanvasOpener(this.gameObject);
            GUIController.instance.upgradeCanvas.transform.position = transform.position + Vector3.up;
            GUIController.instance.damageUpgradeButton.onClick.RemoveAllListeners();
            GUIController.instance.radiusUpgradeButton.onClick.RemoveAllListeners();
            GUIController.instance.cooldownUpgradeButton.onClick.RemoveAllListeners();
            GUIController.instance.damageUpgradeButton.onClick.AddListener(delegate { Upgrade(0); });
            GUIController.instance.radiusUpgradeButton.onClick.AddListener(delegate { Upgrade(1); });
            GUIController.instance.cooldownUpgradeButton.onClick.AddListener(delegate { Upgrade(2); });
            GUIController.instance.SetCanvasElements(myType, upgradeDamage[damageUpgradeCounter], upgradeRadius[radiusUpgradeCounter], upgradeCooldown[cooldownUpgradeCounter],
             upgradeCost[damageUpgradeCounter], upgradeCost[radiusUpgradeCounter], upgradeCost[cooldownUpgradeCounter]);
        }
        //
    }

    void OnMouseEnter()
    {
        rangePreviewSprite.SetActive(true);
        rangePreviewSprite.transform.localScale = rangeSpriteStartScale * colliderRadius * 2;
    }

    void OnMouseExit()
    {
        rangePreviewSprite.SetActive(false);
        
    }

    void OnCollisionStay(Collision col)
    {
        //AVOIDING STACKING TRAPS ON EACH OTHERS
        if (col.gameObject.tag == "Trap")
        {
            GameController.instance.isPlaceable = false;
            //GameController.instance.selectedTrap.SendMessage("ChangeColor",Color.red);
        }
        //
    }


    void OnTriggerEnter(Collider col)
    {
        //SENDING MESSAGE TO THE OBJECT ACCORDINGLY ON WICH KIND OF DAMAGE THEY WILL RECEIVE
        if (col.gameObject.tag == "Enemy")
        {
            switch (myType)
            {
                case trapType.MinaTesla:
                    //STUN TRAP
                    col.gameObject.SendMessage("GetStun", damage);
                    break;
                case trapType.Dynamite:
                    //DAMAGE TRAP
                    col.gameObject.SendMessage("GetDamage", damage);
                    break;
            }
        }

    }

    //COLOR CHANGER ON TRAP GIVING IMMEDIATE FEEDBACK TO THE PLAYER IF ITS PLACEABLE OR NOT
    public void BackToNormalColor()
    {
        myRenderer.material.color = startColor;
    }

    public void ChangeColor(Color newColor)
    {
        myRenderer.material.color = newColor;
    }
    //

    public void Upgrade(int myUpgrade)
    {
        //CHECKING WICH KIND OF UPGRADE WILL BE MADE ON TRAP DEPENDING ON THE BUTTON PRESSED ON THE OPENED CANVAS
        switch (myUpgrade)
        {
            case 0:
                //DAMAGE UPGRADE
                damage = upgradeDamage[damageUpgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[damageUpgradeCounter]);
                if (damageUpgradeCounter < upgradeDamage.Length - 1)
                {
                    damageUpgradeCounter++;

                }
                else
                {
                    damageArrayIsEnded = true;
                }
                break;
            case 1:
                //RANGE UPGRADE
                colliderRadius = upgradeRadius[radiusUpgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[radiusUpgradeCounter]);
                if (radiusUpgradeCounter < upgradeRadius.Length - 1)
                {
                    radiusUpgradeCounter++;
                }
                else
                {
                    rangeArrayIsEnded = true;
                }
                break;
            case 2:
                //COOLDOWN REDUCTION UPGRADE
                explosionCooldown = upgradeCooldown[cooldownUpgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[cooldownUpgradeCounter]);
                if (cooldownUpgradeCounter < upgradeCooldown.Length - 1)
                {
                    cooldownUpgradeCounter++;
                }
                else
                {
                    cooldownArrayIsEnded = true;
                }
                break;
        }
        GUIController.instance.SetCanvasElements(myType, upgradeDamage[damageUpgradeCounter], upgradeRadius[radiusUpgradeCounter], upgradeCooldown[cooldownUpgradeCounter],
             upgradeCost[damageUpgradeCounter], upgradeCost[radiusUpgradeCounter], upgradeCost[cooldownUpgradeCounter]);
    }

    //CLOSING UI UPGRADE CANVAS

    public void ShowPreviewRange()
    {
        rangePreviewSprite.SetActive(true);
        rangePreviewSprite.transform.localScale = rangeSpriteStartScale * upgradeRadius[radiusUpgradeCounter] * 2;
    }

    public void HidePreviewRange()
    {
        rangePreviewSprite.transform.localScale = rangeSpriteStartScale * colliderRadius * 2;
        rangePreviewSprite.SetActive(false);
    }
}
