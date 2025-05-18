using System;
using System.Linq;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private static Interactable _currentInteractable;

    public GameObject CurrentInteractable;
    public static bool Interacting { get; private set; }
    public static float InteractionProgress => _currentInteractable?.InteractionProgress ?? 0f;
    private bool _canInteract = true;
    
    public event Action<Interactable> CurrentInteractableChanged;

    private void Awake()
    {
        Interactable.InteractablesInRangeChanged += HandleInteractablesInRangeChanged;
    }

    private void OnEnable()
    {
        Minigame.s_OnMinigameUpdated += MinigameUpdated;
    }

    private void OnDestroy()
    {
        Interactable.InteractablesInRangeChanged -= HandleInteractablesInRangeChanged;
        Minigame.s_OnMinigameUpdated -= MinigameUpdated;
    }

    private void MinigameUpdated(bool hasStarted)
    {
        if(hasStarted)
        {
            _canInteract = false;
        }else
        {
            _canInteract = true;
        }
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
        if(!_canInteract)
        {
            Interacting = false;
            return;
        }

        // TODO Checkar a condicao
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