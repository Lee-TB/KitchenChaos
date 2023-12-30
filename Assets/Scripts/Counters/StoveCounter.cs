using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] FryingRecipeSO[] fryingRecipySOArray;
    [SerializeField] BurningRecipySO[] burningRecipySOArray;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    private FryingRecipeSO fryingRecipySO;
    private float fryingTimer;
    private BurningRecipySO burningRecipySO;
    private float burningTimer;

    public enum State
    {
        Idle,
        Prepare,
        Frying,
        Fried,
        Burned,
    }

    private State state;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Prepare:
                    fryingTimer = 0f;
                    burningTimer = 0f;
                    fryingRecipySO = GetFryingRecipySOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    burningRecipySO = GetBurningRecipySOWithInput(fryingRecipySO.output);
                    SetState(State.Frying);
                    break;
                case State.Frying:
                    if (fryingTimer > fryingRecipySO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipySO.output, this);
                        SetState(State.Fried);
                    }
                    else
                    {
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                        {
                            progressNormalized = fryingTimer / fryingRecipySO.fryingTimerMax
                        });
                        fryingTimer += Time.deltaTime;
                    }

                    break;
                case State.Fried:
                    if (burningTimer > burningRecipySO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipySO.output, this);
                        SetState(State.Burned);
                    }
                    else
                    {
                        burningTimer += Time.deltaTime;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                        {
                            progressNormalized = burningTimer / burningRecipySO.burningTimerMax
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            // There is a kitchen object here.
            if (player.HasKitchenObject())
            {
                // Player is carry something
                if (player.GetKitchenObject() is PlateKitchenObject plateKitchenObject)
                {
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        SetState(State.Idle);
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                // Player is not carring anything
                GetKitchenObject().SetKitchenObjectParent(player);
                SetState(State.Idle);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
                {
                    progressNormalized = 0f
                });
            }
        }
        else
        {
            // No any kitchen object here.
            if (player.HasKitchenObject())
            {
                // Player is carring something
                if (HasFryingRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    SetState(State.Prepare);
                }
            }
            else
            {
                // Player is not carring anything
            }
        }
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs()
        {
            state = state
        });
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
        {
            progressNormalized = 0f
        });
    }

    private bool HasFryingRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetFryingRecipySOWithInput(inputKitchenObjectSO) != null;
    }


    private FryingRecipeSO GetFryingRecipySOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (var fryingRecipySO in fryingRecipySOArray)
        {
            if (fryingRecipySO.input == inputKitchenObjectSO)
            {
                return fryingRecipySO;
            }
        }
        return null;
    }


    private BurningRecipySO GetBurningRecipySOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (var burningRecipySO in burningRecipySOArray)
        {
            if (burningRecipySO.input == inputKitchenObjectSO)
            {
                return burningRecipySO;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }

    public State GetState() { return state; }
}
