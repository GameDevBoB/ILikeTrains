using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyController : MonoBehaviour
{
    public int life;
    public int earnValue;
    public int actualLife;
    public float speed;
    public float rotationSpeed;
    public float damage = 5.0f;
    public float maxDistance;
    public GameObject target;
    public Transform spawnPointBullet;


    private float speedAdder = 2f;
    private float initialSpeed;
   
   

    void Start()
    {
        //rb = GetComponent<Rigidbody> ();
        target = GameObject.FindWithTag("Train");
        actualLife = life;
        initialSpeed = speed;
       
        //startPosition = transform;
    }

    void Update()
    {
        if (!GameController.instance.isPaused)
        {
            CheckSpeed();
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            transform.LookAt(target.transform.position);
        }
    }


    void Deactivate()
    {
        Destroy(this.gameObject);
    }

    void Activate()
    {
        actualLife = life;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Train" )
        {
            col.gameObject.SendMessage("GetDamage", damage); //questo GetDamage viene gestito dal treno non è lo stesso di questa classe
            Deactivate();
        }
        if (col.gameObject.tag == "Coach")
        {

        }
    }

    void GetDamage(int damage)
    {
        actualLife -= damage;
        if (actualLife <= 0)
        {
            GameController.instance.UpdateResources(earnValue);
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
    public void CheckSpeed()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < maxDistance)
        {
            //Debug.Log(Vector3.Distance(transform.position, target.transform.position));
            speed += speedAdder;
            Shoot(); 
        }
        else
        {
            speed = initialSpeed;
        }
    }

   void Shoot()
    {

    }

    
}
