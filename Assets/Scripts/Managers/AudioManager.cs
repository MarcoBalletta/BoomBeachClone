using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioManager<T> : MonoBehaviour where T: Controller 
{

    protected AudioSource source;
    protected T controller;

    protected virtual void Awake()
    {
        controller = GetComponent<T>();
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected void PlayOneShotAudio(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    protected void PlayAudioWithLoop(AudioClip clip)
    {
        source.loop = true;
        source.clip = clip;
        source.Play();
    }
}
