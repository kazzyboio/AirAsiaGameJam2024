using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public static SoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mixer;
        }
    }

    void Start()
    {
        Play("BGM"); 
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }

        s.source.Play();
    }

    public void PlayHarvestSound()
    {
        int ran = Random.Range(0, 3);

        if (ran == 0)
        {
            Play("Harvest1");
        }
        else if (ran == 1)
        {
            Play("Harvest2");
        }
        else if (ran == 2)
        {
            Play("Harvest3");
        }
    }
}