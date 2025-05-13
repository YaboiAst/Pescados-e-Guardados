using System;
using System.Linq;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private static Interactable _currentInteractable;

    public GameObject CurrentInteractable;
    public static bool Interacting { get; private set; }
    public static float InteractionProgress => _currentInteractable?.InteractionProgress ?? 0f;
    
    
    public event Action<Interactable> CurrentInteractableChanged;

    private void Awake()
    {
        Interactable.InteractablesInRangeChanged += HandleInteractablesInRangeChanged;
    }

    private void OnDestroy()
    {
        Interactable.InteractablesInRangeChanged -= HandleInteractablesInRangeChanged;
    }

    private void HandleInteractablesInRangeChanged(bool obj)
    {
        var nearest = Interactable.InteractablesInRange
            .OrderBy(t => Vector3.Distance(t.transform.position, transform.position))
            .FirstOrDefault();

        _currentInteractable = nearest;
        CurrentInteractableChanged?.Invoke(_currentInteractable);
    }

    private void Update()
    {
        // TODO Add condicoes para os interactables
        if (!_currentInteractable)
        {
            Interacting = false;
            return;
        }
        
        // TODO Bind customizavel pra cada interactable
        if (_currentInteractable && Input.GetKey(KeyCode.E))
        {
            _currentInteractable.Interact();
            Interacting = true;
        }
        else
        {
            Interacting = false;
        }
    }
}
