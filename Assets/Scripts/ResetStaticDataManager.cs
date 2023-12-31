using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        BaseCounter.ResetStaticData();
        CuttingCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
        PlateKitchenObject.ResetStaticData();
    }
}
