using System;
using UnityEngine;

public class SkillCheckManager : MonoBehaviour
{
    public static SkillCheckManager Instance;

    private static SkillCheck currentSkillCheck;

    [SerializeField] private GameObject _skillCheckBar;
    [SerializeField] private GameObject _skillCheckKey;
    [SerializeField] private GameObject _skillCheckCircle;

    private Action _onComplete;
    private Action _onFail;

    private void Awake() => Instance = this;

    private void OnEnable() => SkillCheck.OnSkillCheckFinished += SkillCheckFinished;

    private void OnDestroy() => SkillCheck.OnSkillCheckFinished -= SkillCheckFinished;

    private void SkillCheckFinished(bool successful)
    {
        if (successful)
            _onComplete?.Invoke();
        else
            _onFail?.Invoke();
    }

    public void StartNewSkillCheck(SkillCheckType type, SkillCheckData data, Action completeAction, Action failAction)
    {
        switch (type)
        {
            case SkillCheckType.BAR:
                _skillCheckBar.SetActive(true);
                _skillCheckKey.SetActive(false);
                _skillCheckCircle.SetActive(false);
                currentSkillCheck = _skillCheckBar.GetComponentInChildren<SkillCheck>();
                break;
            case SkillCheckType.KEY:
                _skillCheckBar.SetActive(false);
                _skillCheckKey.SetActive(true);
                _skillCheckCircle.SetActive(false);
                currentSkillCheck = _skillCheckKey.GetComponentInChildren<SkillCheck>();
                break;
            case SkillCheckType.CIRCLE:
                _skillCheckBar.SetActive(false);
                _skillCheckKey.SetActive(false);
                _skillCheckCircle.SetActive(true);
                currentSkillCheck = _skillCheckCircle.GetComponentInChildren<SkillCheck>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        _onComplete = completeAction;
        _onFail = failAction;
        
        currentSkillCheck.StartSkillCheck(data);
    }
}
