using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Ins;
    public List<AudioInfo> infors;

    public int maxAudioSource = 6;
    private List<AudioSource> audios;

    public bool isMuteSound;

    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
        }

        audios = new List<AudioSource>();
    }
    
    public void PlaySound(SoundType type)
    {
        if( isMuteSound ) return;
        
        var a = GetAudioSource();    
        if (a != null)
        {
            var s = GetClip(type);
            a.volume = s.volume;
            a.PlayOneShot(s.clip);
        }
    }

    public AudioSource GetAudioSource()
    {
        foreach (var item in audios)
        {
            if (!item.isPlaying)
            {
                return item;
            }
        }

        if (audios.Count < maxAudioSource)
        {
            var a = gameObject.AddComponent<AudioSource>();
            audios.Add(a);
            return a;
        }

        return null;

    }

    public AudioInfo GetClip(SoundType type)
    {
        return infors.Find(s => s.type == type);
    }

}

[System.Serializable]
public struct AudioInfo
{
    public SoundType type;
    public AudioClip clip;
    public float volume;
}

public enum SoundType
{
    Click,
    StartGame,
    Win,
    Lose
}