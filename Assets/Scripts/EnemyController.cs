using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	//enemyVar
	public int life;
    public int earnValue;
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
        GameController.instance.UpdateResources(earnValue);
	}

	void Activate()
	{
        actualLife = life;
    }

	void OnCollisionEnter(Collision col)
	{
        if (col.gameObject.tag == "Train")
        {
            col.gameObject.SendMessage("GetDamage", damage); //questo GetDamage viene gestito dal treno non è lo stesso di questa classe
<<<<<<< HEAD
            Deactivate();
=======
            Deactivate(); ;
>>>>>>> origin/master
        }
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
	 public void Reset()
	{
		actualLife = life;
		transform.position = startPosition.position;
	}
	*/

}
