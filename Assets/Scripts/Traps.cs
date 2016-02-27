using UnityEngine;
using System.Collections;

public class Traps : MonoBehaviour {

	public int ColliderRay=10;
    public float damage;
    public float explosionCooldown;

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
        if (myTrigger.radius < ColliderRay)
            myTrigger.radius += 0.5f;
        else
            myTrigger.enabled = false;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && (((Time.time - explosionStart) > explosionCooldown) || explosionStart==0))
        {
            myTrigger.enabled = true;
            explosionStart = Time.time;
            myTrigger.radius = 0;
        }
    }


	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Enemy") {
            col.gameObject.SendMessage("GetDamage", damage);
        }
	}

}
