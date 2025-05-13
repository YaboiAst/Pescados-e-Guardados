using System;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager s_Instance;

    private static Minigame s_currentMinigame;

    [SerializeField] private GameObject _minigameBar;
    [SerializeField] private GameObject _minigameKey;
    [SerializeField] private GameObject _minigameCircle;

    private void Awake() => s_Instance = this;

    public void StartMinigame(MinigameSettings settings, Action<MinigameResult> completeMinigame)
    {
        switch (settings.Type)
        {
            case MinigameType.Bar:
                _minigameBar.SetActive(true);
                _minigameKey.SetActive(false);
                _minigameCircle.SetActive(false);
                s_currentMinigame = _minigameBar.GetComponentInChildren<Minigame>();
                break;
            case MinigameType.Key:
                _minigameBar.SetActive(false);
                _minigameKey.SetActive(true);
                _minigameCircle.SetActive(false);
                s_currentMinigame = _minigameKey.GetComponentInChildren<Minigame>();
                break;
            case MinigameType.Circle:
                _minigameBar.SetActive(false);
                _minigameKey.SetActive(false);
                _minigameCircle.SetActive(true);
                s_currentMinigame = _minigameCircle.GetComponentInChildren<Minigame>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(settings.Type), settings.Type, null);
        }
        
        s_currentMinigame.StartMinigame(settings, completeMinigame);
    }
}
