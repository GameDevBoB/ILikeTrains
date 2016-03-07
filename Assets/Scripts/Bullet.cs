using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

 
    private float duration = 3;
    public float shootPower = 200;



    private float activationTime;
    private GameObject target;
    private Rigidbody rb;
    private float bulletDamage;



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
       
    }
    // Use this for initialization
    void Start()
    {
        target = GameObject.FindWithTag("Train");


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
        Physics.IgnoreLayerCollision(10,11);
        if (col.gameObject.tag == "Train")
        {

            col.gameObject.SendMessage("GetDamage", bulletDamage);
            Deactivate();
        }

    }

    private void Deactivate()
    {
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
        activationTime = Time.time;
        rb.AddForce(aimDirection * shootPower);
        
    }
}
