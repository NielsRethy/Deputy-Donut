using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_VictorySound : MonoBehaviour
{
    AudioSource[] allAudioSources;

    public AudioClip ToPlay;

    void Update()
    {
        if(this.isActiveAndEnabled)
        {
            allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            foreach (AudioSource audio in allAudioSources)
            {
                audio.Stop();
            }
        }
    }
}
