using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    public float shakeTimer;
    public float shakeAmount;
    public float shakePowerInput = 0.05f;
    public float shakeDurationInput = 0.5f;
    public static bool isShaking;
    private Vector3 startCameraPos;

    void Start()
    {
        isShaking = false;
        startCameraPos = transform.position;
    }


    void Update()
    {
        if (isShaking)
        {
            ShakeCamera(shakePowerInput, shakeDurationInput);
        }
        if (shakeTimer >= 0)
        {
            Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;
            transform.position = new Vector3(transform.position.x + ShakePos.x, transform.position.y + ShakePos.y, transform.position.z);
            shakeTimer -= Time.deltaTime;


        }
        else
        {
            transform.position = startCameraPos;
        }

    }

    public void ShakeCamera(float shakePower, float shakeDuration)
    {
        shakeAmount = shakePower;
        shakeTimer = shakeDuration;
        isShaking = false;

    }
}