using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] protected KitchenObjectSO kitchenObjectSO;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {    
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // player is holding a plate
                if (plateKitchenObject.TryAddIngredient(kitchenObjectSO))
                {
                    
                }
            }
        } else
        {
            // player don't hold anything
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }


}
