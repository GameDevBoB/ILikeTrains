using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Traps : MonoBehaviour {

	public int ColliderRay=10;
    public float damage;
    public float explosionCooldown;
    public Canvas trapCanvas;
    public Text cooldownText;
    public Image cooldownImage;

    private SphereCollider myTrigger;
    private float explosionStart;

	void Awake() {
        myTrigger = GetComponent<SphereCollider>();
		myTrigger.radius = ColliderRay;
		//this.GetComponent<SphereCollider>().enabled = true;
        Physics.queriesHitTriggers = false;
        explosionStart = 0;
	}
	

	void FixedUpdate () {
        if (!GameController.instance.isPaused)
        {
            if (myTrigger.radius < ColliderRay)
                myTrigger.radius += 0.5f;
            else
                myTrigger.enabled = false;
<<<<<<< HEAD

            if(trapCanvas.gameObject.activeSelf)
            {
                cooldownText.text = (explosionCooldown - (Time.time - explosionStart)).ToString("00.00");
                cooldownImage.fillAmount = (explosionCooldown - (Time.time - explosionStart)) / explosionCooldown;
                if ((Time.time - explosionStart) > explosionCooldown)
                    trapCanvas.gameObject.SetActive(false);
            }
=======
>>>>>>> origin/master
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && (((Time.time - explosionStart) > explosionCooldown) || explosionStart==0) && !GameController.instance.isPaused)
        {
            myTrigger.enabled = true;
            explosionStart = Time.time;
            trapCanvas.gameObject.SetActive(true);
            myTrigger.radius = 0;
        }
    }


	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Enemy") {
            col.gameObject.SendMessage("GetDamage", damage);
        }
	}

}
