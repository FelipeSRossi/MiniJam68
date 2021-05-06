using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button[] unitButtons;
    [SerializeField] private Button topLaneButton;
    [SerializeField] private Button middleLaneButton;
    [SerializeField] private Button bottomLaneButton;

    private Lane lane = Lane.Top;

    void OnEnable()
    {
        LevelEvents.OnWon += OnWon;
        LevelEvents.OnDefeat += OnDefeat;
        LevelEvents.OnLevelStarted += OnLevelStarted;
        HUDEvents.OnUnitButtonStatusUpdate += OnUnitButtonStatusUpdate;

        for (var i = 0; i < unitButtons.Length; i++)
        {
            int index = i;
            unitButtons[index].onClick.AddListener(() => OnUnitButtonClick(index));
        }

        topLaneButton.onClick.AddListener(OnTopLaneButtonClick);
        middleLaneButton.onClick.AddListener(OnMiddleLaneButtonClick);
        bottomLaneButton.onClick.AddListener(OnBottomLaneButtonClick);
        retryButton.onClick.AddListener(OnRetryButtonClick);
    }

    void OnDisable()
    {
        LevelEvents.OnWon -= OnWon;
        LevelEvents.OnDefeat -= OnDefeat;
        LevelEvents.OnLevelStarted -= OnLevelStarted;
        HUDEvents.OnUnitButtonStatusUpdate -= OnUnitButtonStatusUpdate;

        foreach (var button in unitButtons)
        {
            button.onClick.RemoveAllListeners();
        }

        topLaneButton.onClick.RemoveAllListeners();
        middleLaneButton.onClick.RemoveAllListeners();
        bottomLaneButton.onClick.RemoveAllListeners();
        retryButton.onClick.RemoveAllListeners();
    }

    void FixedUpdate()
    {
        UpdateButtonColor(topLaneButton, lane.Equals(Lane.Top) ? Color.white : Color.grey);
        UpdateButtonColor(middleLaneButton, lane.Equals(Lane.Middle) ? Color.white : Color.grey);
        UpdateButtonColor(bottomLaneButton, lane.Equals(Lane.Bottom) ? Color.white : Color.grey);
    }

    private void OnWon()
    {
        retryButton.enabled = true;
    }

    private void OnDefeat()
    {
        retryButton.enabled = true;
    }

    private void OnLevelStarted()
    {
        retryButton.enabled = false;
    }

    private void OnUnitButtonStatusUpdate(int index, bool enabled)
    {
        var target = unitButtons[index];
        target.enabled = enabled;
        UpdateButtonColor(target, enabled ? Color.white : Color.grey);
    }

    private void OnUnitButtonClick(int index)
    {
        LevelEvents.EmitSpawnAlly(index, lane);
    }

    private void OnBottomLaneButtonClick()
    {
        lane = Lane.Bottom;
    }

    private void OnMiddleLaneButtonClick()
    {
        lane = Lane.Middle;
    }

    private void OnTopLaneButtonClick()
    {
        lane = Lane.Top;
    }

    private void OnRetryButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateButtonColor(Button button, Color color)
    {
        button.GetComponent<Image>().color = color;
    }
}
