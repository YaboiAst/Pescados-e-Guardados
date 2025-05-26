using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DiaryEntryUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _nameText;
    private Button _button;
    private DiaryUI _diaryUI;
    public FishData Fish { get; private set; }

    public void SetEntry(FishData fish, bool discovered)
    {
        _icon.sprite = fish.Icon;
        Fish = fish;
        if (discovered)
        {
            _icon.color = Color.white;
            _nameText.text = Fish.DisplayName;
            _button.interactable = true;
            
        }
        else
        {
            _icon.color = Color.black;
            _nameText.text = "???";
            _button.interactable = false;
        }
    }

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectEntry);
        _diaryUI = GetComponentInParent<DiaryUI>();
    }

    private void SelectEntry()
    {
        _diaryUI.SelectEntry(Fish);
    }
}