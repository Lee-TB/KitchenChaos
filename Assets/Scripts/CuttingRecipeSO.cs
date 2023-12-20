using UnityEngine;

[CreateAssetMenu()]
public class CuttingRecipySO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingProgressMax;
}
