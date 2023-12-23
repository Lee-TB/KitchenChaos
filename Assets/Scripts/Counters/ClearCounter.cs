using UnityEngine;

public class ClearCounter : BaseCounter
{
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
                } else
                {
                    // Player is carry somthing is not a plate
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }        
            } else
            {
                // Player is not carring anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        else
        {
            // No any kitchen object here.
            if (player.HasKitchenObject())
            {
                // Player is carring something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else
            {
                // Player is not carring anything
            }
        }

    }
}
