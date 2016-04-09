using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public enum trapType
{
    MinaTesla,
    Dynamite,
	Gatling,
	Barrel,
	Tar
};
public class Traps : MonoBehaviour
{

    public trapType myType;


    //VARIABLES WE NEED TO MAKE THE TRAP BEHAVE ON THE SCENE
    public float colliderRadius = 10;
    public float damage;
	public float explosionSpeed;
	public float burnDelay;
	public float burnDuration;
	public float explosionCooldown;
	public SpriteRenderer spriteUpgrade;
    //

    //VARIABLES WE NEED TO UPGRADE THE VARIOUS TRAPS
    public float[] upgradeDamage = new float[5];
    public float[] upgradeRadius = new float[5];
    public float[] upgradeCooldown = new float[5];
    public int[] upgradeCost = new int[5];
	public Sprite[] arraySpriteTrap = new Sprite[5];
    //

    //TRAPS AUDIO
    public AudioClip[] trapAudioClips = new AudioClip[2];
    //INDEX OF SOUND REFERENCES
    public int cooldownSoundIndex = 0;
    public int damageSoundIndex = 1;
    private AudioSource sourceAudio;
    //


    public Canvas trapCanvas;
    public GameObject explosionSprite;
    public ParticleSystem explosionParticle;
    public GameObject rangePreviewSprite;
    public Text cooldownText;
    public Image cooldownImage;
    public GameObject sprite;
    //

    //SUPPORT VARIABLES
    private SphereCollider myTrigger;
    private float explosionStart;
	private float burnStartTimer;
	private float barrelStartTimer;


    private Vector3 explosionStartScale;
    private Vector3 rangeSpriteStartScale;
    private Vector3 explosionSpriteAdder;
    private Vector3 rangeSpriteAdder;
    private SpriteRenderer myRenderer;
    private Color startColor;
    //private int damageUpgradeCounter;
    //private int radiusUpgradeCounter;
    //private int cooldownUpgradeCounter;
	private int upgradeCounter;



    /*[HideInInspector]
    private bool damageArrayIsEnded = false;
    [HideInInspector]
    private bool rangeArrayIsEnded = false;
    [HideInInspector]
    private bool cooldownArrayIsEnded = false;
    */
	[HideInInspector]
	private bool upgradeArrayIsEnded = false;

    //

    void Awake()
    {
        myTrigger = GetComponent<SphereCollider>();
        myTrigger.radius = colliderRadius;
        //this.GetComponent<SphereCollider>().enabled = true;
        Physics.queriesHitTriggers = false;
        myRenderer = sprite.gameObject.GetComponent<SpriteRenderer>();
        startColor = myRenderer.material.color;
        sourceAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        //SETTING STARTING PARAMETERS ON SUPPORT VARIABLES AND UI
		upgradeCounter = 0;
		//damageUpgradeCounter = 0;
        //radiusUpgradeCounter = 0;
        //cooldownUpgradeCounter = 0;
        explosionStart = 0;
        if(myType == trapType.Tar)
            explosionStartScale = explosionSprite.transform.localScale;
        rangeSpriteStartScale = rangePreviewSprite.transform.localScale;
        explosionSpriteAdder = explosionStartScale * explosionSpeed * 2;
        //rangeSpriteAdder = rangeSpriteStartScale * 2;
        //GUIController.instance.SetCanvasElements(myType, upgradeDamage[upgradeCounter], upgradeRadius[upgradeCounter], upgradeCooldown[upgradeCounter],
          //   upgradeCost[upgradeCounter], upgradeCost[upgradeCounter], upgradeCost[upgradeCounter]);
		GUIController.instance.SetCanvasElements (upgradeCost [upgradeCounter]);

    }


    void FixedUpdate()
    {
        //EXPLOSION VISUALZATIONS OVER TRAPS
        if (!GameController.instance.isPaused)
        {
            if (myTrigger.radius < colliderRadius)
            {
                myTrigger.radius += explosionSpeed;
                switch (myType)
                {
                    case trapType.Dynamite:
                    case trapType.MinaTesla:
                        explosionParticle.gameObject.transform.localScale = new Vector3(explosionParticle.gameObject.transform.localScale.x + explosionSpeed, explosionParticle.gameObject.transform.localScale.y + explosionSpeed, explosionParticle.gameObject.transform.localScale.z + explosionSpeed);
                        break;
                    case trapType.Tar:
                        explosionSprite.transform.localScale = new Vector3(explosionSprite.transform.localScale.x + explosionSpriteAdder.x, explosionSprite.transform.localScale.y + explosionSpriteAdder.y, explosionSprite.transform.localScale.z + explosionSpriteAdder.z);
                        break;
                }
            }
            else if(myTrigger.enabled)
            {
				switch(myType)
				{
					case trapType.Dynamite:
					case trapType.MinaTesla:
					EndExplosion();
					break;
				}
            }
			if((myType == trapType.Barrel || myType == trapType.Tar) && myTrigger.enabled)
			{
				if((Time.time - barrelStartTimer) > burnDuration)
					EndExplosion();
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
        if (GUIController.instance.upgradeCanvas.gameObject.activeSelf && GUIController.instance.canvasOpener == this.gameObject)
        {
            if (!upgradeArrayIsEnded && GameController.instance.totalResources >= upgradeCost[upgradeCounter])
            {
                GUIController.instance.upgradeButtonText.gameObject.SetActive(true);
            }
            else
            {
                GUIController.instance.upgradeButtonText.gameObject.SetActive(false) ;
            }
            
        }
        //

    }

	void Update()
	{
		//Debug.Log (Time.deltaTime);
	}

	private void EndExplosion()
	{
		myTrigger.enabled = false;
        switch(myType)
        {
            case trapType.Tar:
                explosionSprite.gameObject.SetActive(false);
                break;
            case trapType.Barrel:
                explosionParticle.gameObject.SetActive(false);
                break;
        }
		
		explosionStart = Time.time;
		trapCanvas.gameObject.SetActive (true);
	}

    void OnMouseOver()
    {
        //UI VISUALIZATION OF THE TRAP EXPLOSION
        if (Input.GetMouseButtonDown(0) && (((Time.time - explosionStart) > explosionCooldown) || explosionStart == 0) && !GameController.instance.isPaused)
        {
            myTrigger.enabled = true;
            switch(myType)
            {
                case trapType.Tar:
                    explosionSprite.SetActive(true);
                    break;
                case trapType.Barrel:
                    explosionParticle.gameObject.SetActive(true);
                    break;
                case trapType.Dynamite:
                case trapType.MinaTesla:
                    explosionParticle.Play();
                    break;
            }
            
            myTrigger.radius = explosionSpeed;
            if(myType == trapType.Tar)
                explosionSprite.transform.localScale = explosionStartScale;
            //AUDIO MANAGEMENT
            SoundType(damageSoundIndex);
            CameraShake.isShaking = true;
			if(myType == trapType.Barrel || myType == trapType.Tar)
				barrelStartTimer = Time.time;
            //
        }
        //AUDIO MANAGEMENT
        else if (Input.GetMouseButtonDown(0) && !((Time.time - explosionStart) > explosionCooldown) && !GameController.instance.isPaused)
        {
            SoundType(cooldownSoundIndex);

        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!upgradeArrayIsEnded && GameController.instance.totalResources >= upgradeCost[upgradeCounter])
                Upgrade();
        }
        //

        //UI VISUALIZATION OF THE TRAP UPGRADES
        
        //
    }

    void OnMouseEnter()
    {
		if (GameController.instance.trapIsBeingPlaced && GUIController.instance.upgradeCanvas.gameObject.activeSelf)
			GUIController.instance.CloseUpgrade ();
        rangePreviewSprite.SetActive(true);
        rangePreviewSprite.transform.localScale = rangeSpriteStartScale * colliderRadius * 2;

		if (GameController.instance.trapIsBeingPlaced)
		{
			GUIController.instance.upgradeCanvas.gameObject.SetActive(true);
			GUIController.instance.SetCanvasOpener(this.gameObject);
			GUIController.instance.upgradeCanvas.transform.position = transform.position + Vector3.up + Vector3.right;
			//GUIController.instance.activateUpgradeButton.onClick.RemoveAllListeners();
			//GUIController.instance.radiusUpgradeButton.onClick.RemoveAllListeners();
			//GUIController.instance.cooldownUpgradeButton.onClick.RemoveAllListeners();
			//GUIController.instance.damageUpgradeButton.onClick.AddListener(delegate { Upgrade(0); });
			//GUIController.instance.radiusUpgradeButton.onClick.AddListener(delegate { Upgrade(1); });
			//GUIController.instance.cooldownUpgradeButton.onClick.AddListener(delegate { Upgrade(2); });
			//GUIController.instance.activateUpgradeButton.onClick.AddListener(delegate { Upgrade(); });
			// GUIController.instance.SetCanvasElements(myType, upgradeDamage[upgradeCounter], upgradeRadius[upgradeCounter], upgradeCooldown[upgradeCounter],
			// upgradeCost[upgradeCounter], upgradeCost[upgradeCounter], upgradeCost[upgradeCounter]);
			GUIController.instance.SetCanvasElements(upgradeCost[upgradeCounter]);
		}
    }

    void OnMouseExit()
    {
        rangePreviewSprite.SetActive(false);
		//GUIController.instance.upgradeCanvas.gameObject.SetActive(false);
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
			case trapType.Barrel:
			case trapType.Tar:
				//enemyInTrigger.Add(col.gameObject);
				break;
			}

        }

    }

	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.tag == "Enemy")
		{
			switch (myType)
			{
			case trapType.Barrel:
				//DAMAGE TRAP
				/*if((Time.time - burnStartTimer) > burnDelay || burnStartTimer == 0)
				{
					burnStartTimer = Time.time;
					//col.gameObject.SendMessage("GetDamage", damage);
				}*/
				col.gameObject.SendMessage("GetDamage", damage);
				break;
			case trapType.Tar:
				//DAMAGE TRAP
				/*if((Time.time - burnStartTimer) > burnDelay || burnStartTimer == 0)
				{
					burnStartTimer = Time.time;
					//col.gameObject.SendMessage("GetStun", damage);

				}*/
				col.gameObject.SendMessage("GetStun", damage);
				break;
			}
		}
		//Debug.Log (col.gameObject.GetInstanceID());
	}

	void OnTriggerExit()
	{

	}

    //COLOR CHANGER ON TRAP GIVING IMMEDIATE FEEDBACK TO THE PLAYER IF ITS PLACEABLE OR NOT
    public void BackToNormalColor()
    {
        myRenderer.color = startColor;
    }

    public void ChangeColor(Color newColor)
    {
        myRenderer.color = newColor;
    }
    //
	/*
    public void Upgrade(int myUpgrade)
    {

        //CHECKING WICH KIND OF UPGRADE WILL BE MADE ON TRAP DEPENDING ON THE BUTTON PRESSED ON THE OPENED CANVAS
        switch (myUpgrade)
        {
            case 0:
                //DAMAGE UPGRADE
                damage = upgradeDamage[upgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[upgradeCounter]);
                if (upgradeCounter < upgradeDamage.Length - 1)
                {
                    upgradeCounter++;
				}
                else
                {
                    damageArrayIsEnded = true;
                }
                break;
            case 1:
                //RANGE UPGRADE
                colliderRadius = upgradeRadius[upgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[upgradeCounter]);
                if (upgradeCounter < upgradeRadius.Length - 1)
                {
                    upgradeCounter++;
                }
                else
                {
                    rangeArrayIsEnded = true;
                }
                break;
            case 2:
                //COOLDOWN REDUCTION UPGRADE
                explosionCooldown = upgradeCooldown[upgradeCounter];
                GameController.instance.UpdateResources(-upgradeCost[upgradeCounter]);
                if (upgradeCounter < upgradeCooldown.Length - 1)
                {
                    upgradeCounter++;
                }
                else
                {
                    cooldownArrayIsEnded = true;
                }
                break;
        }
        GUIController.instance.SetCanvasElements(myType, upgradeDamage[upgradeCounter], upgradeRadius[upgradeCounter], upgradeCooldown[upgradeCounter],
             upgradeCost[upgradeCounter], upgradeCost[upgradeCounter], upgradeCost[upgradeCounter]);

    }
    */
	public void Upgrade(){
		//DAMAGE UPGRADE
		damage = upgradeDamage[upgradeCounter];
		colliderRadius = upgradeRadius[upgradeCounter];
		explosionCooldown = upgradeCooldown[upgradeCounter];
		spriteUpgrade.sprite = arraySpriteTrap[upgradeCounter];
		GameController.instance.UpdateResources(-upgradeCost[upgradeCounter]);
		if (upgradeCounter < upgradeDamage.Length - 1)
		{
			upgradeCounter++;
		}
		else
		{
			upgradeArrayIsEnded = true;
		}
		//Debug.Log (upgradeCounter);
		GUIController.instance.SetCanvasElements (upgradeCost [upgradeCounter]);
        ShowPreviewRange();

    }

    //CLOSING UI UPGRADE CANVAS

    public void ShowPreviewRange()
    {
        rangePreviewSprite.SetActive(true);
        rangePreviewSprite.transform.localScale = rangeSpriteStartScale * upgradeRadius[upgradeCounter] * 2;
    }

    public void HidePreviewRange()
    {
        rangePreviewSprite.transform.localScale = rangeSpriteStartScale * colliderRadius * 2;
        rangePreviewSprite.SetActive(false);
    }

    public void PlaySound(AudioClip myclip)
    {
        sourceAudio.PlayOneShot(myclip);
    }

    public void SoundType(int mySoundArrayPosition)
    {
        PlaySound(trapAudioClips[mySoundArrayPosition]);
    }
}
