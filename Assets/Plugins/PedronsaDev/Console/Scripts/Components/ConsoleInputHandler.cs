using System;
using TMPro;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM && __INPUTSYSTEM__
using UnityEngine.InputSystem;
#endif

namespace PedronsaDev.Console.Components
{
	public class ConsoleInputHandler : MonoBehaviour
	{
		[SerializeField] private TMP_InputField m_InputField;

#if ENABLE_INPUT_SYSTEM && __INPUTSYSTEM__
		[SerializeField] private InputAction m_ToggleAction = new InputAction("ToggleAction", InputActionType.Button, "<Keyboard>/escape");
		[SerializeField] private InputAction m_AutocompleteAction = new InputAction("AutocompleteAction", InputActionType.Button, "<Keyboard>/tab");
		[SerializeField] private InputAction m_CopyNextLogMessageAction = new InputAction("CopyNextAction", InputActionType.Button, "<Keyboard>/upArrow");
		[SerializeField] private InputAction m_CopyPreviousLogMessageAction = new InputAction("CopyPreviousAction", InputActionType.Button, "<Keyboard>/downArrow");
		[SerializeField] private InputAction m_SelectParameterAction = new InputAction("SelectParameterAction", InputActionType.Button, "<Keyboard>/shift");

		private bool m_InteractionsEnabled = false;
#else
		[SerializeField] private KeyCode m_ToggleKeyCode = KeyCode.Escape;
		[SerializeField] private KeyCode m_AutocompleteKeyCode = KeyCode.Tab;
		[SerializeField] private KeyCode m_CopyNextLogMessageKeyCode = KeyCode.UpArrow;
		[SerializeField] private KeyCode m_CopyPreviousLogMessageKeyCode = KeyCode.DownArrow;
		[SerializeField] private KeyCode m_SelectParameterKeyCode = KeyCode.LeftShift;
#endif
		public event Action OnOpenCloseInput;
		public event Action OnAutocompleteInput;
		public event Action OnCopyNextLogInput;
		public event Action OnCopyPreviousLogInput;
		public event Action OnSelectParameterInput;
		public event Action<string> OnInputFieldValueChange;

#if ENABLE_INPUT_SYSTEM && __INPUTSYSTEM__
		private void Awake()
		{
			m_ToggleAction.performed += OpenCloseInput;

			m_AutocompleteAction.performed += (ctx) => OnAutocompleteInput?.Invoke();
			m_CopyNextLogMessageAction.performed += (ctx) => OnCopyNextLogInput?.Invoke();
			m_CopyPreviousLogMessageAction.performed += (ctx) => OnCopyPreviousLogInput?.Invoke();
			m_SelectParameterAction.performed += (ctx) => OnSelectParameterInput?.Invoke();

			m_InputField.onValueChanged.AddListener(InputFieldValueChange);
			m_InputField.onSubmit.AddListener((string inputText) => SubmitInputDispatcher());

			Console.InputField = m_InputField;
		}

		private void OnEnable()
		{
			m_ToggleAction.Enable();
		}

		private void OnDisable()
		{
			m_ToggleAction.Disable();
		}

		private void OpenCloseInput(InputAction.CallbackContext ctx = default)
		{
			if (m_InteractionsEnabled)
			{
				m_AutocompleteAction.Disable();
				m_CopyNextLogMessageAction.Disable();
				m_CopyPreviousLogMessageAction.Disable();
				m_SelectParameterAction.Disable();
			}
			else
			{
				m_AutocompleteAction.Enable();
				m_CopyNextLogMessageAction.Enable();
				m_CopyPreviousLogMessageAction.Enable();
				m_SelectParameterAction.Enable();
			}

			m_InteractionsEnabled = !m_InteractionsEnabled;

			OnOpenCloseInput?.Invoke();
		}
#else
		private void Awake()
		{
			m_InputField.onValueChanged.AddListener(InputFieldValueChange);
			m_InputField.onSubmit.AddListener((string inputText) => SubmitInputDispatcher());

			Console.InputField = m_InputField;
		}

		private void Update()
		{
			if (Input.GetKeyDown(m_ToggleKeyCode))
			{
				OnOpenCloseInput?.Invoke();
			}
			
			if (Input.GetKeyDown(m_AutocompleteKeyCode))
			{
				OnAutocompleteInput?.Invoke();
			}
			
			if (Input.GetKeyDown(m_CopyNextLogMessageKeyCode))
			{
				OnCopyNextLogInput?.Invoke();
			}
			
			if (Input.GetKeyDown(m_CopyPreviousLogMessageKeyCode))
			{
				OnCopyPreviousLogInput?.Invoke();
			}

			if (Input.GetKeyDown(m_SelectParameterKeyCode))
			{
				OnSelectParameterInput?.Invoke();
			}
		}

		private void OpenCloseInput()
		{
			OnOpenCloseInput?.Invoke();
		}
#endif

		private void InputFieldValueChange(string str) =>
			OnInputFieldValueChange?.Invoke(str);

		/// <summary>
		/// Submit for the UnityEvent button assignation
		/// </summary>
		public void SubmitInputDispatcher() =>
			Console.SubmitInputField();

		/// <summary>
		/// Toggle for the UnityEvent button assignation
		/// </summary>
		public void ToggleInputDispatcher() =>
			OpenCloseInput();
	}
}
