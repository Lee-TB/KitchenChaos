using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject() is PlateKitchenObject plateKitchenObject)
            {
                DeliveryManager.Instance.DeliveryRecipe(plateKitchenObject);
                plateKitchenObject.DestroySelf();
            }
        }
    }
}
