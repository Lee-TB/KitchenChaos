using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnedFlashingBarUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";
    [SerializeField] private StoveCounter stoveCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetBool(IS_FLASHING, false);
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        const float burnedWarningProgressPoint = 0.5f;
        bool isFlashingBar = stoveCounter.IsFried() && e.progressNormalized > burnedWarningProgressPoint;

        if (isFlashingBar)
        {
            animator.SetBool(IS_FLASHING, true);
        } else
        {
            animator.SetBool(IS_FLASHING, false);
        }
    }
}
