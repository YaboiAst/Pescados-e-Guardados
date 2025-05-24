using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    [SerializeField] private GameObject _headerParent;
    [SerializeField] private Image _headerIcon;
    [SerializeField] private TextMeshProUGUI _headerField;
    [SerializeField] private TextMeshProUGUI _contentField;
    [SerializeField] private Transform _elementsParent;
    [SerializeField] private GameObject _elementPrefab;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private int _characterWrapLimit;
    
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start() => CanvasGroup.alpha = 0;

    private void LateUpdate()
    {
        Vector2 position = Input.mousePosition;
        
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        
        float finalPivotX = 0f;
        float finalPivotY = 0f;

        if (pivotX < 0.75)
            finalPivotX = -0.1f;
        else
            finalPivotX = 1.01f;

        if (pivotY < 0.75) 
            finalPivotY = 0;
        else
            finalPivotY = 1;

        _rectTransform.pivot = new Vector2(finalPivotX, finalPivotY);
        
        _rectTransform.transform.position = position;
    }

    public void SetInfo(TooltipInfo info)
    {
        ReturnElements();
        
        if (string.IsNullOrEmpty(info.Header))
        {
            _headerParent.gameObject.SetActive(false);
        }
        else
        {
            _headerParent.gameObject.SetActive(true);
            _headerField.text = info.Header;
        }

        if (!info.Icon)
        {
            _headerIcon.gameObject.SetActive(false);
        }
        else
        {
            _headerIcon.sprite = info.Icon;
            _headerIcon.gameObject.SetActive(true);
        }
        
        _contentField.text = info.Content;

        int headerLength = _headerField.text.Length;
        int contentLength = _contentField.text.Length;

        if (info.Elements.Count > 0)
        {
            _elementsParent.gameObject.SetActive(true);
            CreateElements(info.Elements);
        }
        else
        {
            _elementsParent.gameObject.SetActive(false);
        }

        _layoutElement.enabled = (headerLength > _characterWrapLimit || contentLength > _characterWrapLimit);
    }

    private void CreateElements(List<TooltipElementInfo> infos)
    {
        foreach (TooltipElementInfo info in infos)
        {
            GameObject elementGO = ObjectPoolManager.SpawnGameObject(_elementPrefab, _elementsParent, Quaternion.identity);
            TooltipElement element = elementGO.GetComponent<TooltipElement>();
            element.SetElementInfo(info);
        }
    }

    private void ReturnElements()
    {
        foreach (Transform child in _elementsParent) 
            ObjectPoolManager.ReturnObjectToPool(child.gameObject);
    }
    
}