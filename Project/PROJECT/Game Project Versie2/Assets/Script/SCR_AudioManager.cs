using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_AudioManager : MonoBehaviour {

    public AudioClip[] allSound;
    public AudioClip BGM;

    public float EffectVolume = 1;
    public float BackgroundVolume = 1;
    public float OveralVolume = 1;
    public GameObject SFXController;
    public GameObject BGMController;
    public GameObject OverallController;

    public GameObject engineSound;

    public void SetSFXVolume()
    {
        //slider controls EffectVolume Parameter
        EffectVolume = SFXController.GetComponent<Slider>().value;
    }

    public float GetSFXVolume() { return EffectVolume; }
    public float GetBGMVolume() { return BackgroundVolume; }
    public float GetOverallVolume() { return OveralVolume; }

    public void SetBGMVolume()
    {
        BackgroundVolume = BGMController.GetComponent<Slider>().value;
    }

    public void SetOverallVolume()
    {
        OveralVolume = OverallController.GetComponent<Slider>().value;
        BackgroundVolume = OverallController.GetComponent<Slider>().value;
        BGMController.GetComponent<Slider>().value = OverallController.GetComponent<Slider>().value;
        EffectVolume = SFXController.GetComponent<Slider>().value = OverallController.GetComponent<Slider>().value;
    }

    private void Update()
    {
        GetComponent<AudioSource>().volume = BackgroundVolume;
        engineSound.GetComponent<AudioSource>().volume = EffectVolume;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        yield break;
    }

    public void PlaySound(AudioClip toPlay, GameObject locationOfSoundOutput)
    {
        locationOfSoundOutput.GetComponent<AudioSource>().volume = EffectVolume;
        locationOfSoundOutput.GetComponent<AudioSource>().clip = toPlay;
        locationOfSoundOutput.GetComponent<AudioSource>().Play();
    }

    public void PlayBGM(AudioClip toPlay)
    {
        GetComponent<AudioSource>().volume = BackgroundVolume;
        GetComponent<AudioSource>().clip = toPlay;
        GetComponent<AudioSource>().Play();
    }

    public void StopSound(float timer, GameObject locationOfSoundOutput)
    {
        IEnumerator fadeSound = FadeOut(locationOfSoundOutput.GetComponent<AudioSource>(), timer);
        StartCoroutine(fadeSound);
    }

    public AudioClip[] GetSoundEffects()
    {
        return allSound;
    }

    private void Awake()
    {
        AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Play();
        }
    }

    private void Start()
    {
        GetComponent<AudioSource>().clip = BGM;
        GetComponent<AudioSource>().Play();
    }
}
