using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Traps : MonoBehaviour {

	public int colliderRadius=10;
    public float damage;
	public float explosionSpeed;
    public float explosionCooldown;
    public Canvas trapCanvas;
	public GameObject explosionSprite;
    public Text cooldownText;
    public Image cooldownImage;

    private SphereCollider myTrigger;
    private float explosionStart;
	private Vector3 explosionStartScale;
	private Vector3 adder;

	void Awake() {
        myTrigger = GetComponent<SphereCollider>();
		myTrigger.radius = colliderRadius;
		//this.GetComponent<SphereCollider>().enabled = true;
        Physics.queriesHitTriggers = false;
	}

	void Start()
	{
		explosionStart = 0;
		explosionStartScale = explosionSprite.transform.localScale;
		adder = explosionStartScale * explosionSpeed * 2;
	}
	

	void FixedUpdate () {

        if (!GameController.instance.isPaused)
        {
            if (myTrigger.radius < colliderRadius)
			{
                myTrigger.radius +=explosionSpeed;
				explosionSprite.transform.localScale = new Vector3( explosionSprite.transform.localScale.x +adder.x, explosionSprite.transform.localScale.y + adder.y, explosionSprite.transform.localScale.z + adder.z);
			}
            else
			{
                myTrigger.enabled = false;
				explosionSprite.gameObject.SetActive(false);
			}

            if(trapCanvas.gameObject.activeSelf)
            {
                cooldownText.text = (explosionCooldown - (Time.time - explosionStart)).ToString("00.00");
                cooldownImage.fillAmount = (explosionCooldown - (Time.time - explosionStart)) / explosionCooldown;
                if ((Time.time - explosionStart) > explosionCooldown)
                    trapCanvas.gameObject.SetActive(false);
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && (((Time.time - explosionStart) > explosionCooldown) || explosionStart==0) && !GameController.instance.isPaused)
        {
            myTrigger.enabled = true;
			explosionSprite.SetActive(true);
            explosionStart = Time.time;
            trapCanvas.gameObject.SetActive(true);
            myTrigger.radius = explosionSpeed;
			explosionSprite.transform.localScale = explosionStartScale;
        }
    }


	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Enemy") {
            col.gameObject.SendMessage("GetDamage", damage);
        }
	}

}
