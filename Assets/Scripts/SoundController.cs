using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour
{

    public static SoundController instance;



    //TRAIN AUDIO
    public AudioClip[] HQAudioClips = new AudioClip[3];

    


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    void Start()
    {

    }



}
