using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private SkillCheckData _data;

    [SerializeField] private Slider _speedSlider;
    [SerializeField] private Slider _targetAreaSlider;
    [SerializeField] private Slider _cSuccessSlider;
    [SerializeField] private Slider _successSlider;
    [SerializeField] private Slider _failSlider;
    [SerializeField] private Slider _rateSlider;
    [SerializeField] private Slider _amountSlider;
    [SerializeField] private Slider _durationSlider;
    [SerializeField] private Toggle _overtimeToggle;
    
    
    private void Start()
    {
        _data = new SkillCheckData();
        
        _speedSlider.onValueChanged.AddListener((v) => { _data.Speed = v;});
        _targetAreaSlider.onValueChanged.AddListener((v) => { _data.TargetAreaSize = v;});
        _cSuccessSlider.onValueChanged.AddListener((v) => { _data.CriticalSuccessProgress = Mathf.RoundToInt(v);});
        _successSlider.onValueChanged.AddListener((v) => { _data.SuccessProgress = Mathf.RoundToInt(v);});
        _failSlider.onValueChanged.AddListener((v) => { _data.FailureProgress = Mathf.RoundToInt(v);});
        _rateSlider.onValueChanged.AddListener((v) => { _data.DecreaseTimer = v;});
        _amountSlider.onValueChanged.AddListener((v) => { _data.DecreaseAmount = Mathf.RoundToInt(v);});
        _durationSlider.onValueChanged.AddListener((v) => { _data.Duration = v;});
        _overtimeToggle.onValueChanged.AddListener((v) => { _data.DecreaseProgressOvertime = v;});
    }

    public void UseDefaultSettings()
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

        _speedSlider.value = data.Speed;
        _targetAreaSlider.value = data.TargetAreaSize;
        _cSuccessSlider.value = data.CriticalSuccessProgress;
        _successSlider.value = data.SuccessProgress;
        _failSlider.value = data.FailureProgress;
        _rateSlider.value = data.DecreaseTimer;
        _amountSlider.value = data.DecreaseAmount;
        _durationSlider.value = data.Duration;
        _overtimeToggle.isOn = data.DecreaseProgressOvertime;

        _data = data;
    }

    public void StartSkillCheck(int index)
    {
        SkillCheckType type = SkillCheckType.BAR;
        
        switch (index)
        {
            case 0:
                break;
            case 1:
                type = SkillCheckType.KEY;
                break;
            case 2:
                type = SkillCheckType.CIRCLE;
                break;
            default:
                break;
        }
        EventSystem.current.SetSelectedGameObject(null);
        SkillCheckManager.Instance.StartNewSkillCheck(type, _data, OnComplete, OnFail);
    }


    private void OnComplete()
    {
        //Fazer alguma coisa quando completar o minigame
    }

    private void OnFail()
    {
        //Fazer alguma coisa quando falhar o minigame
    }
}
