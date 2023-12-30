using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }


    [SerializeField] AudioClipRefsSO audioClipRefsSO;

    private float volume;

    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, .5f);
    }

    private void Start()
    {
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.trash, Camera.main.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectDrop, Camera.main.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Camera.main.transform.position);
        //PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position, 0.5f);

    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        //CuttingCounter cuttingCounter = sender as CuttingCounter;
        //PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position, 5f);
        PlaySound(audioClipRefsSO.chop, Camera.main.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess, Camera.main.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail, Camera.main.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume * this.volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume * this.volume);
    }

    public void PlayFootstepsSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }

    public void PlayCountdownSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipRefsSO.warning, position, volume);
    }

    public void PlayGoSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipRefsSO.go, position, volume);
    }

    public void PlayWarningSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipRefsSO.warning, position, volume);
    }



    public void IncreaseVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 1f;
        }
        PlaySound(audioClipRefsSO.warning[0], Camera.main.transform.position, volume);
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void DecreaseVolume()
    {
        volume -= .1f;
        if (volume < 0f)
        {
            volume = 0f;
        }
        PlaySound(audioClipRefsSO.warning[0], Camera.main.transform.position, volume);
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
