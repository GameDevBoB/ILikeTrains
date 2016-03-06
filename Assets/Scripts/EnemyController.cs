using UnityEngine;
using System.Collections;
public enum EnemyType
{
    Base,
    Shooter

};
[System.Serializable]
public class EnemyController : MonoBehaviour
{
    public int life;
    public int earnValue;
    public int actualLife;
    public float walkSpeed;
    public float runSpeed = 5f;
    public float rotationSpeed;
    public float damage;
    public float maxDistance;
    public GameObject target;
    public Transform spawnPointBullet;
    public GameObject[] bullets;
    public GameObject bulletPrefab;
    public EnemyType myEnemyType;
    public float fireRate;




    private float initialSpeed;
    private int bulletCount;
    private float startShooting;

    void Awake()
    {
        if (myEnemyType == EnemyType.Shooter)
        {
            bullets = new GameObject[100];
            SpawnBullets();


        }
    }

    void Start()
    {
        //rb = GetComponent<Rigidbody> ();
        target = GameObject.FindWithTag("Train");
        actualLife = life;
        initialSpeed = walkSpeed;

        //startPosition = transform;
    }

    void Update()
    {
        if (!GameController.instance.isPaused)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < maxDistance)
            {
                switch (myEnemyType)
                {
                    case EnemyType.Base:
                        Run(runSpeed);
                        break;

                    case EnemyType.Shooter:
                        Run(walkSpeed / 2 * 3);
                        Shoot();
                        break;

                }

            }
            else
            {
                Run(walkSpeed);
            }


        }
    }


    void Deactivate()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            Destroy(bullets[i]);
        }
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
        if ((Time.time - startShooting) > fireRate)
        {
            startShooting = Time.time;
            if (bulletCount < bullets.Length - 1)
            {
                bulletCount++;

            }
            else
            {
                bulletCount = 0;
            }
            bullets[bulletCount].gameObject.transform.position = spawnPointBullet.transform.position;
            bullets[bulletCount].gameObject.SetActive(true);
            bullets[bulletCount].gameObject.SendMessage("ShootBullet", spawnPointBullet.transform.forward);
        }
    }

    private void SpawnBullets()
    {

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            //bullets[i].transform.parent = this.gameObject.transform;
            bullets[i].SendMessage("GetDamageValue", damage);
            bullets[i].SetActive(false);
        }

    }


}
