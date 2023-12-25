using System;
using UnityEngine;
public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] protected CuttingRecipySO[] cuttingRecipySOArray;

    public event EventHandler OnCut;
    public static event EventHandler OnAnyCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            // There is a kitchen object here.
            if (player.HasKitchenObject())
            {
                // Player is carry something
                PlateKitchenObject plateKitchenObject;
                if (player.GetKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    // Player is carry somthing is not a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // Player is not carring anything, Player can pickup KitchenObject and reset CuttingProgress to zero;
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    progressNormalized = (float)0f
                });
            }
        }
        else
        {
            // No any kitchen object here.
            if (player.HasKitchenObject())
            {
                // Player is carring something
                player.GetKitchenObject().SetKitchenObjectParent(this); // player put KitchenObject to this counter.
                cuttingProgress = 0;
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
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipySO.cuttingProgressMax
            });
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            if (cuttingProgress >= cuttingRecipySO?.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetCuttingRecipeOutputForInput(kitchenObjectSO);
                kitchenObject.DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
        else
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
