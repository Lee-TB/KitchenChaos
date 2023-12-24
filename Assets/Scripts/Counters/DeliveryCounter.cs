using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if(player.HasKitchenObject())
        {
            if(player.GetKitchenObject() is PlateKitchenObject plateKitchenObject)
            {
                plateKitchenObject.DestroySelf();
            }
        }
    }
}
