using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
	
	public void SetBgmVolume(float volume)
	{
		audioMixer.SetFloat("BgmVolume",volume);
	}
}
