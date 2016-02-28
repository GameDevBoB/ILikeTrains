using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	//enemyVar
	public int life;
	//[HideInInspector]
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
        if (!GameController.instance.isPaused)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.LookAt(target.transform.position);
        }
	}


	void Deactivate()
	{
		gameObject.SetActive(false);
	}

	void Activate()
	{
		gameObject.SetActive(true);
	}

	void OnCollisonEnter(Collision collision)
	{
		target.SendMessage("GetDamages", damage); //questo GetDamage viene gestito dal treno non è lo stesso di questa classe
		

	}

	void GetDamage(int damage)
	{
		actualLife -= damage;
        if (actualLife <= 0)
        {
            Deactivate();
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
