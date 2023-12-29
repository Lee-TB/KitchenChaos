using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    public static MusicManager Instance { get; private set; }

    private AudioSource musicSource;
    private float volume;

    private void Awake()
    {
        Instance = this;
        musicSource = GetComponent<AudioSource>();

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .5f);
        musicSource.volume = volume;
    }

    public void IncreaseVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 1f;
        }
        musicSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void DecreaseVolume()
    {
        volume -= .1f;
        if (volume < 0f)
        {
            volume = 0f;
        }
        musicSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
