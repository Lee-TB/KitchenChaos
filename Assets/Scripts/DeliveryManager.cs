using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);
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
                    found = true;
                    break;
                }
            }
        }

        if (found)
            Debug.Log("correct recipe!");
        else
            Debug.Log("wrong recipe!");
    }
}
