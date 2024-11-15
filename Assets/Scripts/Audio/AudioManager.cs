using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup sfxMixer;

    public Sound[] music;
    public Sound[] sfx;
    public Sound[] stopSfx;

    //public AudioSource audiosource;

    public static AudioManager instance;

    void Awake()
    {
        #region
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        //audiosource.outputAudioMixerGroup = audioMixerGroup;

        foreach (Sound m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            //m.source.outputAudioMixerGroup = audioMixerGroup;
            m.source.clip = m.clip;

            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;

            m.source.outputAudioMixerGroup = musicMixer;
        }

        foreach (Sound s in sfx)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            //s.source.outputAudioMixerGroup = audioMixerGroup;
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = sfxMixer;
        }

    }

    void Start()
    {
        PlayMusic("BGM");
        //PlaySFX("Lullaby");
    }

    public void PlayMusic(string name)
    {
        Sound m = Array.Find(music, sound => sound.name == name);
        if (m == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        m.source.Play();
    }

    public void StopMusic(string name)
    {
        Sound m = Array.Find(music, sound => sound.name == name);
        if (m == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        m.source.Stop();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfx, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    /*public void StopSFX(string name)
    {
        Sound s = Array.Find(sfx, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }*/

    //plug this into where you want the audio to play
    //FindObjectOfType<AudioManager>().PlayMusic("BGM");
    //FindObjectOfType<AudioManager>().PlaySFX("SFX");
}