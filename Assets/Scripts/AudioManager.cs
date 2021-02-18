using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;
    private Slider volumeSlider;

    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
            

        DontDestroyOnLoad(gameObject);

        if (GameObject.Find("Volume Slider"))
        {
            volumeSlider = GameObject.Find("Volume Slider").GetComponent<Slider>();            
        }

        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }

    private void Start()
    {
        Play("Theme");     
    }

    public void Play(string name)
    {
        Sounds s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sounds s = System.Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void SetVolume(float myVolume)
    {
        Sounds s = System.Array.Find(sounds, sound => sound.volume == myVolume);
    }
}
