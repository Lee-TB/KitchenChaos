using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";

    [SerializeField] private TextMeshProUGUI countdownText;

    private Animator animator;
    private int prevCountDownNumber;
    private float delayToHide;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else if (KitchenGameManager.Instance.IsGamePlaying())
        {
            delayToHide = 1f;
        }
    }

    private void Update()
    {
        if (KitchenGameManager.Instance.IsGamePlaying())
        {
            delayToHide -= Time.deltaTime;
            if (delayToHide < 0f)
            {
                Hide();
            }
        }

        int countDownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());

        countdownText.text = countDownNumber == 0f ? "Go!" : countDownNumber.ToString();

        if (countDownNumber != prevCountDownNumber)
        {
            prevCountDownNumber = countDownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            if (countDownNumber >= 1)
            {
                SoundManager.Instance.PlayCountdownSound(Camera.main.transform.position, 0.7f);
            }
            else
            {
                SoundManager.Instance.PlayGoSound(Camera.main.transform.position);
            }
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
