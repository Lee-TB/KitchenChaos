using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 15f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();        
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (KitchenGameManager.Instance.IsGamePlaying() && spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliveryRecipe(PlateKitchenObject plateKitchenObject)
    {
        bool found = false;
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            var recipeKitchenObjectSOList = waitingRecipeSOList[i].kitchenObjectSOList;
            var plateKitchenObjectSOList = plateKitchenObject.GetKitchenObjectSOList();

            if (recipeKitchenObjectSOList.Count == plateKitchenObjectSOList.Count)
            {
                if (recipeKitchenObjectSOList.All(plateKitchenObjectSOList.Contains))
                {
                    // Intersect of two list have the same length with them
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    found = true;
                    break;
                }
            }
        }

        if (found)
        {
            successfulRecipesAmount++;
            OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
