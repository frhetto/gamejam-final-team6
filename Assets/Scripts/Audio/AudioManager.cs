using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioMixer mixer;

    const string MUSIC_KEY = "MusicVolume";
    const string SFX_KEY   = "SFXVolume";

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        ApplyAll();
    }

    public void SetMusicVolume(float linear)
    {
        mixer.SetFloat("MusicVol", ToDb(linear));
        PlayerPrefs.SetFloat(MUSIC_KEY, linear);
    }

    public void SetSFXVolume(float linear)
    {
        mixer.SetFloat("SFXVol", ToDb(linear));
        PlayerPrefs.SetFloat(SFX_KEY, linear);
    }

    public float GetMusicVolume() => PlayerPrefs.GetFloat(MUSIC_KEY, 0.75f);
    public float GetSFXVolume()   => PlayerPrefs.GetFloat(SFX_KEY,   0.75f);

    void ApplyAll()
    {
        SetMusicVolume(GetMusicVolume());
        SetSFXVolume(GetSFXVolume());
    }

    static float ToDb(float linear) => Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
}
