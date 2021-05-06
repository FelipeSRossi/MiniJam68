using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private Animator mainCamera;
    [SerializeField] private Animator kid;

    void OnEnable()
    {
        LevelEvents.OnWon += OnWon;
        LevelEvents.OnDefeat += OnDefeat;
    }

    void OnDisable()
    {
        LevelEvents.OnWon -= OnWon;
        LevelEvents.OnDefeat -= OnDefeat;
    }

    private void OnWon()
    {
        mainCamera.SetTrigger("Ending");
    }

    private void OnDefeat()
    {
        mainCamera.SetTrigger("Ending");
        kid.SetTrigger("Ending");
    }
}
