using System;

[Serializable]
public struct MinigameSettings
{
    public MinigameType Type;
    public float Speed;
    public float TargetAreaSize;

    public int CriticalSuccessProgress;
    public int SuccessProgress;
    public int FailureProgress;

    public bool DecreaseProgressOvertime;
    public int DecreaseAmount;
    public float DecreaseTimer;

    public float Duration;
}