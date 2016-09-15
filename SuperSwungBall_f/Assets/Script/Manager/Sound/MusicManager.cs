using UnityEngine;
using System.Collections;

using Singleton;

public class MusicManager : Singleton<MusicManager>
{
    private AudioSource source_;
    private AudioClip clip_;


    void Start()
    {
        source_ = GetComponent<AudioSource>();
        clip_ = source_.clip;
        if (ApplicationModel.tetomaIsPlaying)
            Stop_Music();
        else
        {
            ApplicationModel.tetomaIsPlaying = true;
            Play_Music();
        }
    }

    public void Stop_Music()
    {
        source_.Stop();
    }
    public void Play_Music()
    {
        source_.Play();
    }

    private void fade_sound()
    {
        SoundFading fader = GetComponent<SoundFading>();
        fader.Fade(clip_, 1, true);
    }

    /// <summary>
    /// Prends le path pour un Resources.Load()
    /// </summary>
    public string Clip
    {
        set
        {
            clip_ = Resources.Load(value) as AudioClip;
            fade_sound();
        }
    }
    public bool Loop
    {
        set { source_.loop = value; }
    }
    public bool Mute
    {
        set { source_.mute = value; }
    }
}
