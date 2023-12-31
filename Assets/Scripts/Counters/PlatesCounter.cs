using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount = 4;
    private int platesSpawnedAmountMax = 4;
    bool firstUpdate = true;   

    private void Update()
    {
        if(firstUpdate)
        {
            firstUpdate = false;
            SpawnFullPlates();
        }

        if (spawnPlateTimer <= 0f)
        {
            spawnPlateTimer = spawnPlateTimerMax;
            if (platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
        else spawnPlateTimer -= Time.deltaTime;
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void SpawnFullPlates()
    {
        for (int i = 0; i < platesSpawnedAmount; i++)
        {
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        }
    }
}
