using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnedWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        Hide();
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged; ;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {        
        const float burnedWarningProgressPoint = 0.5f;
        bool show = stoveCounter.IsFried() && (e.progressNormalized > burnedWarningProgressPoint);
        
        if (show)
        {
            Show();
        } else
        {
            Hide();
        }        
    }


    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);        
    }
}
