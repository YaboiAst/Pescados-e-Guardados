using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillCheck : MonoBehaviour
{
    [SerializeField] private KeyCode _interactKeyCode;
    
    [Header("UI Settings")]
    [SerializeField] protected RectTransform _progressBar;
    [SerializeField] protected RectTransform _progressBarBackground;
    [SerializeField] protected Image _durationTimeBar;

    public static Action<bool> OnSkillCheckFinished;
    
    protected float _speed = 500f;
    protected float _targetAreaSize = 10;
    protected int _criticalSuccessProgressAmount = 30;
    protected int _successProgressAmount = 15;
    protected int _failureProgressAmount = 10;
    private float _progressAmount = 0;

    private bool _decreaseProgressOvertime = false;
    private int _decreaseAmount;
    private float _decreaseTimer;
    private float _timer;

    private float _duration = 10f;
    private float _durationTimer = 0f;
    
    protected bool _isStopped = true;

    protected virtual void Update()
    {
        if(_isStopped)
            return;
        
        if (Input.GetKeyDown(_interactKeyCode))
        {
            CheckSuccess();
        }
        
        if (_decreaseProgressOvertime && _decreaseTimer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = _decreaseTimer;
                ModifyProgressAmount(-_decreaseAmount);
            }
        }
        
        if (_durationTimer <= 0)
            FailSkillCheck();
        else
            _durationTimer -= Time.deltaTime;
    }

    protected virtual void CheckSuccess()
    {

    }

    public virtual void StartSkillCheck(SkillCheckData data)
    {
        _speed = data.Speed;
        _targetAreaSize = data.TargetAreaSize;
        _criticalSuccessProgressAmount = data.CriticalSuccessProgress;
        _successProgressAmount = data.SuccessProgress;
        _failureProgressAmount = data.FailureProgress;

        _decreaseProgressOvertime = data.DecreaseProgressOvertime;
        _decreaseAmount = data.DecreaseAmount;
        _decreaseTimer = data.DecreaseTimer;
        
        _duration = data.Duration;
        _durationTimer = data.Duration;
        
        _timer = _decreaseTimer;
        _isStopped = false;
        
        _progressAmount = 0;
        
        SetProgressAmount(0);
        
        _durationTimeBar.fillAmount = 1;
        DOTween.Kill("duration");
        _durationTimeBar.DOFillAmount(0, _duration).SetEase(Ease.Linear).SetId("duration");
        _progressBar.sizeDelta = new Vector2(0, _progressBar.sizeDelta.y);
        
        ResetSkillCheck();
        Debug.Log("SC Started");
    }

    protected virtual void FailSkillCheck()
    {
        ResetUI();
        OnSkillCheckFinished?.Invoke(false);
        Debug.Log("SC Failed");
    }

    protected virtual void CompleteSkillCheck()
    {
        ResetUI();
        OnSkillCheckFinished?.Invoke(true);
        Debug.Log("SC Completed");
    }
    private void ResetUI()
    {
        _isStopped = true;
        
        _progressBar.DOSizeDelta(new Vector2(0, _progressBar.sizeDelta.y), 0.3f).SetEase(Ease.OutQuint);
        _durationTimeBar.fillAmount = 1;
        DOTween.Kill("duration");
    }

    protected virtual void ResetSkillCheck()
    {
        _progressBar.sizeDelta = new Vector2(0, _progressBar.sizeDelta.y);
        Debug.Log("SC Reseted");
    }

    protected virtual void ModifyProgressAmount(int amount)
    {
        _progressAmount += amount;

        if (_progressAmount <= 0)
        {
            _progressAmount = 0;
        }
        else if (_progressAmount >= 100)
        {
            _progressAmount = 100f;
            CompleteSkillCheck();
        }
        
        float size = (_progressBarBackground.rect.width / 100) * _progressAmount;
        Vector2 targetSize = new Vector2(size, _progressBar.rect.size.y);
        _progressBar.DOSizeDelta(targetSize, 0.5f).SetEase(Ease.OutQuint);
    }
    
    private void SetProgressAmount(int amount)
    {
        _progressAmount = amount;

        float size = (_progressBarBackground.rect.width / 100) * _progressAmount;
        Vector2 targetSize = new Vector2(size, _progressBar.rect.size.y);

        _progressBar.sizeDelta = targetSize;
    }
}

public enum SkillCheckType
{
    BAR,
    KEY,
    CIRCLE
}

[Serializable]
public struct SkillCheckData
{
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