using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManagerGameManager : MonoBehaviour
{
    protected AudioSource source;
    private EventManagerGameManager eventManager;
    [SerializeField] private AudioClip simulationClip;
    [SerializeField] private AudioClip placingClip;
    [SerializeField] private AudioClip lostClip;
    [SerializeField] private AudioClip winClip;

    private void Awake()
    {
        eventManager = GetComponent<EventManagerGameManager>();
        source = GetComponent<AudioSource>();
        source.volume = 0.12f;
        PlayPlacingSong();
    }

    protected virtual void OnEnable()
    {
        eventManager.onPlacingModeStarted += PlayPlacingSong;
        eventManager.onSimulationModeStarted += PlaySimulationSong;
        eventManager.onEndMatch += PlayEndSong;
    }

    private void PlayEndSong(bool result)
    {
        AudioClip clip = result ? winClip : lostClip;
        PlayOneShotAudio(clip);
    }

    private void PlayPlacingSong()
    {
        if(source.clip != placingClip)
            PlayAudioWithLoop(placingClip);
    }

    private void PlaySimulationSong()
    {
        PlayAudioWithLoop(simulationClip);
    }

    protected virtual void OnDisable()
    {
        eventManager.onPlacingModeStarted -= PlayPlacingSong;
        eventManager.onSimulationModeStarted -= PlaySimulationSong;
        eventManager.onEndMatch -= PlayEndSong;
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
