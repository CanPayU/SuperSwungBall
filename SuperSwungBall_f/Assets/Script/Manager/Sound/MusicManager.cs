using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance = null;
    /// <summary>
    /// Instance static de la class
    /// </summary>
    public static MusicManager I
    {
        get { return instance; }
    }

    private AudioSource source_;
    private AudioClip clip_;

    void Awake()
    {
        if ((instance != null && instance != this))
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        source_ = GetComponent<AudioSource>();
        clip_ = source_.clip;
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
