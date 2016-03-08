using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{


    public float shootPower = 200;

    //USED TO SET THE OBJECT PERSISTENCE IN SCENE
    private float duration = 3;
    //
    private float activationTime;
    private Rigidbody rb;
    private float bulletDamage;
    //private GameObject target;



    void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }
    // Use this for initialization
    void Start()
    {
        //  target = GameObject.FindWithTag("Train");


    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - activationTime) > duration)
        {
            Deactivate();
        }

    }

    void OnCollisionEnter(Collision col)
    {
        //AVODING COLLISION THROUGH PLAYER/BULLET LAYERS
        Physics.IgnoreLayerCollision(10, 11);
        //
        if (col.gameObject.tag == "Train")
        {

            col.gameObject.SendMessage("GetDamage", bulletDamage);
            Deactivate();
        }

    }

    private void Deactivate()
    {
        //DEACTIVATE BULLET IN SCENE
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void GetDamageValue(float damageValue)
    {
        bulletDamage = damageValue;
    }

    void ShootBullet(Vector3 aimDirection)
    {
        //BULLET SHOOT
        activationTime = Time.time;
        rb.AddForce(aimDirection * shootPower);

    }
}
