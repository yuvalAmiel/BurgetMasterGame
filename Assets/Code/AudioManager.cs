using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sound m_MainSound;
    public Sound[] m_SFXSounds;
    public Slider m_SFXSlider, m_BackGroundSlider; // need one for BG too - m_BackGroundSlider

    public static AudioManager instance;
    void Awake ()
    {
        /* if (instance == null)
             instance = this;
         else
         {
             Destroy(gameObject);
             return;
         }
         DontDestroyOnLoad(gameObject);
        */
        //DontDestroyOnLoad(gameObject);
        foreach (Sound s in m_SFXSounds) 
            soundSettings(s);
        soundSettings(m_MainSound);
    }

    void Start()
    {
        Play("MainSound");
    }

    void soundSettings(Sound s)
    {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
    }

    public void UpdateSFXSound()
    {
        foreach (Sound s in m_SFXSounds)
        {
            s.source.volume = m_SFXSlider.value;
        }
    }

    public void UpdateMainSound()
    {
        m_MainSound.source.volume = m_BackGroundSlider.value;
    }

    public void Play(string name)
    {
        if (SettingsMenu.GameIsPaused)
            return;
        Sound s = Array.Find(m_SFXSounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
        else if (m_MainSound.name == name)
            m_MainSound.source.Play();
        else
            Debug.Log("no such audio");
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(m_SFXSounds, sound => sound.name == name);
        if (s != null)
        {
            if (s.source.isPlaying)
                return true;
        }
        return false;
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(m_SFXSounds, sound => sound.name == name);
        if (s != null)
            s.source.Stop();
    }

}
