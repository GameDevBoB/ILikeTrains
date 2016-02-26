using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	//enemyVar
	public int life;
	[HideInInspector]
	public int actualLife;
	public float speed;
	public float rotationSpeed;
	private Rigidbody rb;
	public float damage= 5.0f;

	//TrainTarget
	public GameObject target;
	//public Transform target;


	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		target = GameObject.FindWithTag ("Train");
		actualLife = life;
		//startPosition = transform;
	}

	void Update() 
	{
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
		transform.LookAt (target.transform.position);
	}

	void Activate()
	{
		gameObject.SetActive(true);
	}
	void OnCollisonEnter()
	{
		target.SendMessage("GetDamage", damage); 
	}

	void GetDamage(int damage)
	{
		actualLife -= damage;
		if (actualLife<=0)
		{
			gameObject.SetActive(false);
		}

	}
	/* Utile in futuro forse
	 public void reset()
	{
		actualLife = life;
		transform.position = startPosition.position;
	}
	*/

}
