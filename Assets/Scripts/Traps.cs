using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Traps : MonoBehaviour
{
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

    //UI ELEMENTS FOR UPGRADES AND VISUAL FEEDBACK EXPLOSIONS
    public Canvas upgradeCanvas;
    public Text damageUpgradeText;
    public Text radiusUpgrandeText;
    public Text cooldownUpgradeText;
    public Button damageUpgradeButton;
    public Button radiusUpgradeButton;
    public Button cooldownUpgradeButton;
    public Canvas trapCanvas;
    public GameObject explosionSprite;
    public Text cooldownText;
    public Image cooldownImage;
    //

    public int SlowRatio;

    //SUPPORT VARIABLES
    private SphereCollider myTrigger;
    private float explosionStart;
    private Vector3 explosionStartScale;
    private Vector3 adder;
    private Renderer myRenderer;
    private Color startColor;
    private int damageUpgradeCounter;
    private int radiusUpgradeCounter;
    private int cooldownUpgradeCounter;
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
        adder = explosionStartScale * explosionSpeed * 2;
        if (gameObject.tag != "TrapTesla")
        {
            damageUpgradeText.text = "Damage " + upgradeDamage[damageUpgradeCounter];
        }
        else
        {
            damageUpgradeText.text = "Slow " + upgradeDamage[damageUpgradeCounter];
        }
        radiusUpgrandeText.text = "Range " + upgradeRadius[radiusUpgradeCounter];
        cooldownUpgradeText.text = "Cooldown " + upgradeCooldown[cooldownUpgradeCounter];
        damageUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + upgradeCost[damageUpgradeCounter];
        radiusUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + upgradeCost[radiusUpgradeCounter];
        cooldownUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + upgradeCost[cooldownUpgradeCounter];
    }


    void FixedUpdate()
    {
        //EXPLOSION VISUALZATIONS OVER TRAPS
        if (!GameController.instance.isPaused)
        {
            if (myTrigger.radius < colliderRadius)
            {
                myTrigger.radius += explosionSpeed;
                explosionSprite.transform.localScale = new Vector3(explosionSprite.transform.localScale.x + adder.x, explosionSprite.transform.localScale.y + adder.y, explosionSprite.transform.localScale.z + adder.z);
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
        if (upgradeCanvas.gameObject.activeSelf)
        {
            if (damageUpgradeCounter < upgradeDamage.Length && GameController.instance.totalResources >= upgradeCost[damageUpgradeCounter])
            {
                damageUpgradeButton.interactable = true;
            }
            else
            {
                damageUpgradeButton.interactable = false;
            }
            if (radiusUpgradeCounter < upgradeRadius.Length && GameController.instance.totalResources >= upgradeCost[radiusUpgradeCounter])
            {
                radiusUpgradeButton.interactable = true;
            }
            else
            {
                radiusUpgradeButton.interactable = false;
            }
            if (cooldownUpgradeCounter < upgradeCooldown.Length && GameController.instance.totalResources >= upgradeCost[cooldownUpgradeCounter])
            {
                cooldownUpgradeButton.interactable = true;
            }
            else
            {
                cooldownUpgradeButton.interactable = false;
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
            if (!upgradeCanvas.gameObject.activeSelf)
            {
                upgradeCanvas.gameObject.SetActive(true);
            }
            else
            {
                upgradeCanvas.gameObject.SetActive(false);
            }
        }
        //
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
            if (this.gameObject.tag == "StunTrap")
            {
                //STUN TRAP
                col.gameObject.SendMessage("GetStun");
            }
            else
            {
                //DAMAGE TRAP
                col.gameObject.SendMessage("GetDamage", damage);
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
                damageUpgradeCounter++;
                if (gameObject.tag != "TrapTesla")
                {
                    if (damageUpgradeCounter < upgradeDamage.Length)
                    {
                        damageUpgradeText.text = "Damage " + upgradeDamage[damageUpgradeCounter];
                        damageUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + upgradeCost[damageUpgradeCounter];
                    }
                }
                else
                {
                    if (damageUpgradeCounter < upgradeDamage.Length)
                    {
                        damageUpgradeText.text = "Slow " + upgradeDamage[damageUpgradeCounter];
                        damageUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + upgradeCost[damageUpgradeCounter];
                    }
                }
                break;
            case 1:
                //RANGE UPGRADE
                colliderRadius = upgradeRadius[radiusUpgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[radiusUpgradeCounter]);
                radiusUpgradeCounter++;
                if (radiusUpgradeCounter < upgradeRadius.Length)
                {
                    radiusUpgrandeText.text = "Range " + upgradeRadius[radiusUpgradeCounter];
                    radiusUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + upgradeCost[radiusUpgradeCounter];
                }
                break;
            case 2:
                //COOLDOWN REDUCTION UPGRADE
                explosionCooldown = upgradeCooldown[cooldownUpgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[cooldownUpgradeCounter]);
                cooldownUpgradeCounter++;
                if (cooldownUpgradeCounter < upgradeCooldown.Length)
                {
                    cooldownUpgradeText.text = "Cooldown " + upgradeCooldown[cooldownUpgradeCounter];
                    cooldownUpgradeButton.transform.GetChild(0).GetComponent<Text>().text = "Cost " + upgradeCost[cooldownUpgradeCounter];
                }
                break;
        }
    }

    //CLOSING UI UPGRADE CANVAS
    public void CloseUpgrade()
    {
        upgradeCanvas.gameObject.SetActive(false);
    }

}
