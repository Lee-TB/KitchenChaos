using UnityEngine;

[CreateAssetMenu()]
public class BurningRecipySO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float burningTimerMax;
}
