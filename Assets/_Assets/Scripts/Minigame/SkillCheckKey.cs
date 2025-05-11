using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckKey : SkillCheck
{
    [SerializeField] private RectTransform _pointer;
    [SerializeField] private Image _targetArea;
    [SerializeField] private Image _criticalTargetArea;
    
    private float _targetAreaAngle = 90f;
    private float _criticalAreaAngle;
    private float _criticalAreaRange;
    private float _currentAngle;
    private bool _isMovingRight = true;

    [Button]
    public void StartButton()
    {
        SkillCheckData data = new SkillCheckData
        {
            Speed = 5f,
            TargetAreaSize = 20,
            CriticalSuccessProgress = 25,
            SuccessProgress = 15,
            FailureProgress = 10,
            DecreaseProgressOvertime = false,
            DecreaseAmount = 0,
            DecreaseTimer = 0f,
            Duration = 20f
        };
        
        StartSkillCheck(data);
    }
    
    public override void StartSkillCheck(SkillCheckData data)
    {
        base.StartSkillCheck(data);
        _speed *= 50f;
        _targetAreaSize *= 1.8f;
        GenerateNewTargetArea();
    }

    protected override void Update()
    {
        if (_isStopped)
            return;

        MovePointer();
        
        base.Update();
    }
    private void MovePointer()
    {
        if (_isMovingRight)
        {
            _currentAngle += _speed * Time.deltaTime;
            if (_currentAngle >= 360f) _currentAngle -= 360f;
        }
        else
        {
            _currentAngle -= _speed * Time.deltaTime;
            if (_currentAngle <= 0f) _currentAngle += 360f;
        }
        
        _currentAngle %= 360f;
        _pointer.localEulerAngles = new Vector3(0, 0, -_currentAngle);
    }
    
    private void GenerateNewTargetArea(bool animate = true)
    {
        float targetAreaSize = _targetAreaSize * Random.Range(0.85f, 1.15f);
        _criticalAreaRange = targetAreaSize / 5;
        _targetArea.fillAmount = targetAreaSize * 2 / 360f;
        _criticalTargetArea.fillAmount = _criticalAreaRange * 2 / 360f;
        
        _targetAreaAngle = Random.Range(0f, 360f);
        _criticalAreaAngle = _targetAreaAngle - (targetAreaSize - _criticalAreaRange);

        Vector3 rotation = new Vector3(0, 0, -_targetAreaAngle + targetAreaSize);
        
        if (animate)
        {
            _targetArea.rectTransform.DOLocalRotate(rotation, 0.2f).SetEase(Ease.OutQuint);
        }
        else
        {
            _targetArea.rectTransform.localEulerAngles = rotation;
        }
    }
    protected override void CheckSuccess()
    {
        float angleDiff = Mathf.DeltaAngle(_currentAngle, _targetAreaAngle);
        float angleDiff2 = Mathf.DeltaAngle(_currentAngle, _criticalAreaAngle);

        if (Mathf.Abs(angleDiff2) <= _criticalAreaRange)
        {
            _isMovingRight = !_isMovingRight;
            GenerateNewTargetArea();
            ModifyProgressAmount(_criticalSuccessProgressAmount);
        }
        else if (Mathf.Abs(angleDiff) <= _targetAreaSize)
        {
            _isMovingRight = !_isMovingRight;
            GenerateNewTargetArea();
            ModifyProgressAmount(_successProgressAmount);
        }
        else
        {
            ModifyProgressAmount(-_failureProgressAmount);
        }
    }

    protected override void CompleteSkillCheck()
    {
        _currentAngle = 0;
        _pointer.DORotate(Vector3.zero, 0.2f).SetEase(Ease.OutQuint);
        _targetArea.DOFillAmount(0, 0.2f);
        _criticalTargetArea.DOFillAmount(0, 0.2f);
        base.CompleteSkillCheck();
    }

    protected override void FailSkillCheck()
    {
        _currentAngle = 0f;
        _pointer.DORotate(Vector3.zero, 0.2f).SetEase(Ease.OutQuint);
        _targetArea.DOFillAmount(0, 0.2f);
        _criticalTargetArea.DOFillAmount(0, 0.2f);
        base.FailSkillCheck();
    }

    protected override void ResetSkillCheck()
    {
        _currentAngle = 0f;
        _pointer.localEulerAngles = Vector3.zero;
        _targetArea.fillAmount = 0;
        _criticalTargetArea.fillAmount = 0;
        GenerateNewTargetArea(false);
        base.ResetSkillCheck();
    }
}