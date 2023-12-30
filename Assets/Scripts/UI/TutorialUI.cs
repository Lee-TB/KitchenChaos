using TMPro;
using UnityEditor.Search;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KeyMoveUpText;
    [SerializeField] private TextMeshProUGUI KeyMoveDownText;
    [SerializeField] private TextMeshProUGUI KeyMoveLeftText;
    [SerializeField] private TextMeshProUGUI KeyMoveRightText;
    [SerializeField] private TextMeshProUGUI KeyInteractText;
    [SerializeField] private TextMeshProUGUI KeyInteractAlternateText;
    [SerializeField] private TextMeshProUGUI KeyPauseText;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        UpdateVisual();
        Show();
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        KeyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        KeyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        KeyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        KeyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        KeyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        KeyInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        KeyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
