using UnityEngine;
using System.Collections;
public enum EnemyType
    //ENEMY TYPE ENUMERATOR
{
    Base,
    Shooter

};
[System.Serializable]
public class EnemyController : MonoBehaviour
{
    //ENEMY HEALTH VARIABLES
    public int life;
    public int actualLife;
    //
    //REWARD VALUE FOR KILLING THE ENEMY 
    public int earnValue;
    //

    //SPEED VARIABLES FOR ENEMY PREFABS
    public float walkSpeed;
    public float runSpeed = 5f;
    //

    //DAMAGE VALUE PER ENEMY
    public float damage;
    //
    //VARIABLE THAT DETECTS THE DISTANCE BETWEEN THE HQ AND HIMSELF SO THE RELATIVE SPEED CAN BE CHANGED ACCORDINGLY TO THAT DISTANCE
    public float maxDistance;
    //

    //THE HQ REFERENCE IN SCENE
    public GameObject target;
    //

    //REFERENCE ON THE PROJECTILES THAT CERTAIN ENEMIES WILL SHOOT REACHING THE HQ RANGE
    public Transform spawnPointBullet;
    public GameObject[] bullets;
    public GameObject bulletPrefab;
    //

    public EnemyType myEnemyType;

    //FIRE RATIO
    public float fireRate;
    //

    //public float rotationSpeed;




    //private float initialSpeed;
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
        target = GameObject.FindWithTag("Train");
        actualLife = life;
        //initialSpeed = walkSpeed;
        //startPosition = transform.position;
    }

    void Update()
    {
        if (!GameController.instance.isPaused)
        {
            //DETECTING THE DISTANCE TO THE TRAIN FROM THIS OBJECT
            if (Vector3.Distance(transform.position, target.transform.position) < maxDistance)
            {
                //CHECKING WICH TYPE OF ENEMY IS BEING WALKING IN THE HQ RANGE
                //THEN CHANGE ITS SPEED ACCORDINGLY
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
        //DESTROY THE INSTANTATED BULLETS PREFABS
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
            //DEACTIVATING ENEMY ON HQ COLLISION
            col.gameObject.SendMessage("GetDamage", damage); //questo GetDamage viene gestito dal treno non è lo stesso di questa classe
            Deactivate();
            //

        }

    }

    void GetDamage(int damage)
    {
        //ENEMY CAN GET DAMAGE FROM SOURCES
        //SO WE NEED TO ARRANGE THE VALUE ACCORDINGLY
        actualLife -= damage;
        if (actualLife <= 0)
        {
            //EVERY TIME AN ENEMY DIES THE TOTAL RESOURCES WILL INCREASE
            GameController.instance.UpdateResources(earnValue);
            Deactivate();
            //

        }
    }
    /* TEST
	 public void reset()
	{
		actualLife = life;
		transform.position = startPosition.position;
	}
	TEST */
    public void Run(float actualSpeed)
    {
        //CHANGE ENEMY SPEED DEPENDING ON THE INPUT VALUE
        transform.Translate(Vector3.forward * actualSpeed * Time.deltaTime);
        transform.LookAt(target.transform.position);
    }

    void Shoot()
    {
        //SHOOTING ROUTINE
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
        //BULLET INSTANTIATION AND DEACTIVATION, SO WE DONT NEED TO INSTANTIATE IT AT RUNTIME SINCE EVERY ENEMY IS SPAWNED ONE BY ONE
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            //bullets[i].transform.parent = this.gameObject.transform;
            bullets[i].SendMessage("GetDamageValue", damage);
            bullets[i].SetActive(false);
        }
        //

    }

    //STUN METHOD 
    public void GetStun(int slowRatio)
    {
        walkSpeed *= slowRatio;
        transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
        transform.LookAt(target.transform.position);
    }


}
