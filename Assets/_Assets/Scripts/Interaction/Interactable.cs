using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public static IReadOnlyCollection<Interactable> InteractablesInRange => s_interactablesInRange;
    private static HashSet<Interactable> s_interactablesInRange = new HashSet<Interactable>();

    [SerializeField] private InteractionType _interactionType;
    [SerializeField] private float _interactionRange;
    [SerializeField] private UnityEvent _onInteractionCompleted;
    [SerializeField] private bool _requireMinigame;
    [SerializeField] private MinigameSettings _minigameSettings;
    
    [SerializeField] private float _timeToInteract = 0f;
    
    private float _timeInteracted = 0f;

    public float InteractionProgress => _timeInteracted / _timeToInteract;
    public bool WasFullyInteracted => InteractionProgress >= 1 || _instantInteract == true;

    private bool _instantInteract;
    
    public virtual InteractionType InteractionType => _interactionType;
    
    public event Action InteractionCompleted;
    public static event Action<bool> InteractablesInRangeChanged;
    public static event Action<Interactable, string> AnyInteractionComplete;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !WasFullyInteracted)
        {
            Debug.Log("Entered");
            s_interactablesInRange.Add(this);
            InteractablesInRangeChanged?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (s_interactablesInRange.Remove(this))
                InteractablesInRangeChanged?.Invoke(s_interactablesInRange.Any());
        }
    }
    
    public void Interact()
    {
        if (WasFullyInteracted)
            return;

        if (_timeToInteract == 0f) 
            _instantInteract = true;

        _timeInteracted += Time.deltaTime;

        if (WasFullyInteracted)
        {
            if (_requireMinigame)
            {
                s_interactablesInRange.Remove(this);
                InteractablesInRangeChanged?.Invoke(s_interactablesInRange.Any());
                MinigameManager.s_Instance.StartMinigame(_minigameSettings,HandleMinigameCompleted);
            }
            else
                CompleteIteraction();
        }
        Debug.Log("Interact");
    }

    private void HandleMinigameCompleted(MinigameResult result)
    {
        if (result == MinigameResult.Won)
            CompleteIteraction();
        else if (result == MinigameResult.Fail)
        {
            _timeInteracted = 0f;
            _instantInteract = false;
            s_interactablesInRange.Add(this);
            InteractablesInRangeChanged?.Invoke(s_interactablesInRange.Any());
        }
    }
    
    private void CompleteIteraction()
    {
        InteractionCompleted?.Invoke();

        if (WasFullyInteracted)
            s_interactablesInRange.Remove(this);

        SendInteractionComplete();
    }
    
    protected void SendInteractionComplete()
    {
        InteractablesInRangeChanged?.Invoke(s_interactablesInRange.Any());
        _onInteractionCompleted?.Invoke();
        AnyInteractionComplete?.Invoke(this, _interactionType.CompletedInteraction);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _interactionRange);
        

        Gizmos.color = Color.red;
        
        if (WasFullyInteracted)
            Gizmos.color = Color.green;
        
        Gizmos.DrawSphere(transform.position + (Vector3.up * 10), 2f);
    }
}
