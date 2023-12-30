using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;

    private AudioSource audioSource;

    private bool playWarningSound;
    private float warningTimer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        const float burnedWarningProgressPoint = 0.5f;
        playWarningSound = stoveCounter.IsFried() && (e.progressNormalized > burnedWarningProgressPoint);       
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }       
    }

    private void Update()
    {
        if(stoveCounter.IsFried() && playWarningSound)
        {
           warningTimer -= Time.deltaTime;
            if(warningTimer < 0f)
            {
                float warningTimerMax = 0.5f;
                warningTimer = warningTimerMax;

                SoundManager.Instance.PlayWarningSound(Camera.main.transform.position);
            }
        }
    }
}
