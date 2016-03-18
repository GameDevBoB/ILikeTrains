using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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

    //THE ENEMY TYPE ENUM DECLARATION
    public EnemyType myEnemyType;
    //

    //FIRE RATIO
    public float fireRate;
    //

    //ENEMY AUDIO
    public AudioClip kamikazeAudioClip;
    public AudioClip[] horseIndianAudioClips = new AudioClip[3];
    public AudioClip[] armoredBanditAudioClips = new AudioClip[2];
    public AudioClip[] rifleBanditAudioClips = new AudioClip[2];
    public AudioClip[] indianLancerAudioClips = new AudioClip[2];
    public AudioClip[] berserkAudioClips = new AudioClip[3];
    public int deathSoundIndex = 0;
    public int damageSoundIndex = 1;
    public int extraSoundIndex = 2;
    private AudioSource sourceAudio;
    //
    public float slowTime;
    public Slider lifeSlider;
    public GameObject upSprite;
    public GameObject leftSprite;
    public GameObject rightSprite;
    public GameObject downSprite;

    private Animator upAnim;
    private Animator leftAnim;
    private Animator rightAnim;
    private Animator downAnim;
    private bool isRunning;
    private int bulletCount;
    private float startShooting;
    private float slowRatio;
    private float startSlow;
    private Vector3 distance;
    private float updateMovementTime = 0.5f;
    private float startMovement;
    private BoxCollider myBoxCollider;


    void Awake()
    {
        if (myEnemyType == EnemyType.Shooter)
        {
            bullets = new GameObject[100];
            SpawnBullets();
        }
        myBoxCollider = GetComponent<BoxCollider>();
        sourceAudio = GetComponent<AudioSource>();
        upAnim = upSprite.GetComponent<Animator>();
        leftAnim = leftSprite.GetComponent<Animator>();
        rightAnim = rightSprite.GetComponent<Animator>();
        downAnim = downSprite.GetComponent<Animator>();
    }

    void Start()
    {
        target = GameObject.FindWithTag("Train");
        actualLife = life;
        lifeSlider.value = lifeSlider.maxValue = life;
        isRunning = false;
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
                        isRunning = true;
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
                isRunning = false;
            }
        }
    }


    

    void Activate()
    {
        actualLife = life;
        //healthSlider.value = healthSlider.maxValue = actualLife;
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
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Coach")
        {
            //DEACTIVATING ENEMY ON HQ TRIGGER
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
        lifeSlider.value = actualLife;
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
        if (((Time.time - startSlow) < slowTime) && startSlow != 0)
            actualSpeed /= slowRatio;
        //CHANGE ENEMY SPEED DEPENDING ON THE INPUT VALUE
        if ((Time.time - startMovement) > updateMovementTime || startMovement == 0)
        {
            distance = target.transform.position - transform.position;
            startMovement = Time.time;
        }
        if (Mathf.Abs(distance.x) < Mathf.Abs(distance.z))
        {
            if (distance.z >= 0)
            {
                upSprite.SetActive(true);
                upAnim.SetBool("Run", isRunning);
                leftSprite.SetActive(false);
                rightSprite.SetActive(false);
                downSprite.SetActive(false);
                transform.Translate(Vector3.forward * actualSpeed * Time.deltaTime);
            }
            else
            {
                upSprite.SetActive(false);
                leftSprite.SetActive(false);
                rightSprite.SetActive(false);
                downSprite.SetActive(true);
                downAnim.SetBool("Run", isRunning);
                transform.Translate(-Vector3.forward * actualSpeed * Time.deltaTime);
            }
        }
        else
        {

            if (distance.x >= 0)
            {
                upSprite.SetActive(false);
                leftSprite.SetActive(false);
                rightSprite.SetActive(true);
                rightAnim.SetBool("Run", isRunning);
                downSprite.SetActive(false);
                transform.Translate(Vector3.right * actualSpeed * Time.deltaTime);
            }
            else
            {
                upSprite.SetActive(false);
                leftSprite.SetActive(true);
                leftAnim.SetBool("Run", isRunning);
                rightSprite.SetActive(false);
                downSprite.SetActive(false);
                transform.Translate(-Vector3.right * actualSpeed * Time.deltaTime);
            }
        }
        //transform.Translate(Vector3.forward * actualSpeed * Time.deltaTime);
        //transform.LookAt(target.transform.position);
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
    public void GetStun(float input_slowRatio)
    {
        slowRatio = input_slowRatio;
        startSlow = Time.time;
    }

    void Deactivate()
    {
        //DESTROY THE INSTANTATED BULLETS PREFABS
        for (int i = 0; i < bullets.Length; i++)
        {
            Destroy(bullets[i]);
        }
        /*TEST
        for (int i = 0; i < transform.childCount; i++)
        {

            transform.GetChild(i).gameObject.SetActive(false);
        }
        myBoxCollider.gameObject.SetActive(false);
        TEST*/
        SoundType(deathSoundIndex);
        Destroy(this.gameObject);
    }
    public void PlaySound(AudioClip myclip)
    {
        
        AudioSource.PlayClipAtPoint(myclip, new Vector3(0,13,0));
       
    }

    public void SoundType(int mySoundArrayPosition)
    {
        switch (myEnemyType)
        {
            case EnemyType.Base:
                PlaySound(kamikazeAudioClip);
                break;



        }
    }
}
