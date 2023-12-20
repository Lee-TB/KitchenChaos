using System;
using UnityEngine;
public class CuttingCounter : BaseCounter
{
    public event EventHandler OnCut;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    [SerializeField] protected CuttingRecipySO[] cuttingRecipySOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            // There is a kitchen object here.
            if (player.HasKitchenObject())
            {
                // Player is carry something
            }
            else
            {
                // Player is not carring anything, Player can pickup KitchenObject and reset CuttingProgress to zero;
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs()
                {
                    progressNormalized = (float)0f
                });
                cuttingProgress = 0;
            }
        }
        else
        {
            // No any kitchen object here.
            if (player.HasKitchenObject())
            {
                // Player is carring something
                //if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                //{
                    player.GetKitchenObject().SetKitchenObjectParent(this); // player put KitchenObject to this counter.                    
                //}
            }
            else
            {
                // Player is not carring anything
            }
        }
    }

    public override void InteractAlternate(Player player)
    {        
        if (!HasKitchenObject()) { return; }

        var kitchenObject = GetKitchenObject();
        var kitchenObjectSO = kitchenObject?.GetKitchenObjectSO();
        if (HasRecipeWithInput(kitchenObjectSO))
        {
            // There is an kitchen object here and it can be cut.
            var cuttingRecipySO = GetCuttingRecipySOWithInput(kitchenObjectSO);

            cuttingProgress++;
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs()
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipySO.cuttingProgressMax
            });
            OnCut?.Invoke(this, EventArgs.Empty);

            if (cuttingProgress >= cuttingRecipySO?.cuttingProgressMax)
            {
                //cuttingProgress = 0;
                KitchenObjectSO outputKitchenObjectSO = GetCuttingRecipeOutputForInput(kitchenObjectSO);
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        } else
        {
            Debug.Log("Cannot cut this KitchenObject :)");
        }
    }



    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetCuttingRecipySOWithInput(inputKitchenObjectSO) != null;
    }

    private KitchenObjectSO GetCuttingRecipeOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        if (GetCuttingRecipySOWithInput(inputKitchenObjectSO) is CuttingRecipySO cuttingRecipySO)
        {
            return cuttingRecipySO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipySO GetCuttingRecipySOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (var cuttingRecipySO in cuttingRecipySOArray)
        {
            if (cuttingRecipySO.input == inputKitchenObjectSO)
            {
                return cuttingRecipySO;
            }
        }
        return null;
    }
}
