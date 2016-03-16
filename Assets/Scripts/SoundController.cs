using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public static SoundController instance;
	public AudioClip[]  TrapSound=new AudioClip[2];
	public AudioClip[] Enemy=new AudioClip[3];
 //Other sounds




	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}else
		{
			DestroyImmediate(gameObject);
		}
	}



}
