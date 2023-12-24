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
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            var recipeKitchenObjectSOList = waitingRecipeSOList[i].kitchenObjectSOList;
            var plateKitchenObjectSOList = plateKitchenObject.GetKitchenObjectSOList();

            if (recipeKitchenObjectSOList.Count == plateKitchenObjectSOList.Count)
            {
                // Have the same numbers of ingredients
                var intersect = recipeKitchenObjectSOList.Intersect(plateKitchenObjectSOList);
                if (intersect.Count() == recipeKitchenObjectSOList.Count)
                {
                    // Intersect of two list have the same length with them
                    Debug.Log("correct recipe!");
                    waitingRecipeSOList.RemoveAt(i);
                    break;
                }
            }
        }

        Debug.Log("wrong recipe");
    }
}
