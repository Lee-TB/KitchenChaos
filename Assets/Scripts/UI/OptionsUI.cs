using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button IncreaseSoundEffectsButton;
    [SerializeField] private Button DecreaseSoundEffectsButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;

    [SerializeField] private Button IncreaseMusicButton;
    [SerializeField] private Button DecreaseMusicButton;
    [SerializeField] private TextMeshProUGUI musicText;

    [SerializeField] private Button backButton;

    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField] private TextMeshProUGUI pressToRebindText;


    private Action onBackAction;

    private void Awake()
    {
        Instance = this;

        IncreaseSoundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.IncreaseVolume();
            UpdateVisual();
        });

        DecreaseSoundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.DecreaseVolume();
            UpdateVisual();
        });

        IncreaseMusicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.IncreaseVolume();
            UpdateVisual();
        });

        DecreaseMusicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.DecreaseVolume();
            UpdateVisual();
        });

        backButton.onClick.AddListener(() =>
        {
            Hide();
            onBackAction();
        });

        moveUpButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Up));
        moveDownButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Down));
        moveLeftButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Left));
        moveRightButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Right));
        interactButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact));
        interactAlternateButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.InteractAlternate));
        pauseButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Pause));
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnPaused += KitchenGameManager_OnGameUnPaused;
        UpdateVisual();
        Hide();
        HidePressToRebindText();
    }

    private void KitchenGameManager_OnGameUnPaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = Mathf.Round(SoundManager.Instance.GetVolume() * 100f).ToString() + "%";
        musicText.text = Mathf.Round(MusicManager.Instance.GetVolume() * 100f).ToString() + "%";

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show(Action onBackAction)
    {
        this.onBackAction = onBackAction;
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindText()
    {
        pressToRebindText.gameObject.SetActive(true);
    }

    private void HidePressToRebindText()
    {
        pressToRebindText.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindText();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindText();
            UpdateVisual();
        });
    }
}
