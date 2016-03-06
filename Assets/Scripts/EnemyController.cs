using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyController : MonoBehaviour
{
    public int life;
    public int earnValue;
    public int actualLife;
    public float speed;
    public float runSpeed = 5f;
    public float rotationSpeed;
    public float damage = 5.0f;
    public float maxDistance;
    public GameObject target;
    public Transform spawnPointBullet;



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
            if (Vector3.Distance(transform.position, target.transform.position) < maxDistance)
            {
                Run(runSpeed);
            }
            else
            {
                Run(speed);
            }

            
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
        if (col.gameObject.tag == "Train")
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
    public void Run(float actualSpeed)
    {
        transform.Translate(Vector3.forward * actualSpeed * Time.deltaTime);
        transform.LookAt(target.transform.position);
    }

    void Shoot()
    {

    }


}
