using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryUI : CanvasController
{
    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _entryPrefab;

    [Header("Selection Settings")]
    [SerializeField] private TMP_Text _entryName;
    [SerializeField] private TMP_Text _entryDescription;
    [SerializeField] private Image _entryImage;

    private readonly Dictionary<string, DiaryEntryUI> _entryUIs = new Dictionary<string, DiaryEntryUI>();

    private void Start()
    {
        foreach (FishData fish in FishManager.AllFishes)
        {
            GameObject entryObj = Instantiate(_entryPrefab, _contentParent);
            DiaryEntryUI entryUI = entryObj.GetComponent<DiaryEntryUI>();

            bool discovered = DiaryManager.Instance.IsDiscovered(fish);

            entryUI.SetEntry(fish, discovered);
            _entryUIs[fish.UniqueID] = entryUI;
        }
        
        CloseDiary();
        
        DiaryManager.Instance.OnFishDiscovered += UpdateEntry;
    }

    private void OnDestroy()
    {
        DiaryManager.Instance.OnFishDiscovered -= UpdateEntry;
    }

    private void UpdateEntry(FishData fish)
    {
        if (_entryUIs.TryGetValue(fish.UniqueID, out DiaryEntryUI entryUI))
            entryUI.SetEntry(fish, true);
    }

    private void OpenDiary()
    {
        ShowCanvas();

        DiaryEntryUI entry = _entryUIs.FirstOrDefault(t => DiaryManager.Instance.IsDiscovered(t.Value.Fish)).Value;
        if (entry)
            SelectEntry(entry.Fish);
    }

    private void CloseDiary()
    {
        HideCanvas();
    }

    private void ToggleDiary()
    {
        if (IsOpen)
            CloseDiary();
        else
            OpenDiary();
    }

    public void SelectEntry(FishData fish)
    {
        _entryImage.sprite = fish.Icon;
        _entryName.text = fish.DisplayName;
        _entryDescription.text = fish.Description;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            ToggleDiary();
    }
}