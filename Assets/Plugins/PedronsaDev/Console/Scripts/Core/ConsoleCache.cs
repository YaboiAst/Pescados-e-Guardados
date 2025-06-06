using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PedronsaDev.Console
{
	public class ConsoleCache : ScriptableObject
	{
		public Command[] CommandRegistry;
		public List<Command> AvailableCommands;
		public bool ShowLogs = true;

		private void OnEnable()
		{
			Console.RefreshAutocompleteReceived += RefreshAutocomplete;
			Console.AddAutocompleteReceived += AddAutocomplete;
			Console.RemoveAutocompleteReceived += RemoveAutocomplete;

#if UNITY_EDITOR
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
		}

		private void OnDisable()
		{
			Console.RefreshAutocompleteReceived -= RefreshAutocomplete;
			Console.AddAutocompleteReceived -= AddAutocomplete;
			Console.RemoveAutocompleteReceived -= RemoveAutocomplete;

#if UNITY_EDITOR
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
#endif
		}

		public void AddAutocomplete(Type type)
		{
			if (type == null)
			{
				Debug.LogWarning($"Smart Error: Cannot add null type to autocomplete.");
				return;
			}

			bool alreadyAdded = false;

			for (int i = 0; i < CommandRegistry.Length && !alreadyAdded; i++)
			{
				if (CommandRegistry[i].Method != null && CommandRegistry[i].Method.ReflectedType == type)
				{
					if (!AvailableCommands.Contains(CommandRegistry[i]))
					{
						// add it to available list
						AvailableCommands.Add(CommandRegistry[i]);
					}
					else
					{
						// this type has already been added to autocomplete
						// add should be ignored
						alreadyAdded = true;
					}
				}
			}
		}

		public void RemoveAutocomplete(Type type)
		{
			if (type == null)
			{
				Debug.LogWarning($"Smart Error: Cannot remove null type to autocomplete.");
				return;
			}

			for (int i = 0; i < CommandRegistry.Length; i++)
			{
				if (CommandRegistry[i].Method != null && CommandRegistry[i].Method.ReflectedType == type)
				{
					// this command needs target
					// try find target
#if UNITY_2022_3_OR_NEWER
					object target = FindAnyObjectByType(type);
#else
					object target = FindObjectOfType(type);
#endif

					if (target == null)
					{
						// target not found
						// remove it from available list
						AvailableCommands.Remove(CommandRegistry[i]);
					}
				}
			}
		}

		public void RefreshAutocomplete()
		{
			if (AvailableCommands.Count > 0)
			{
				AvailableCommands.Clear();
			}

			for (int i = 0; i < CommandRegistry.Length; i++)
			{
				if (CommandRegistry[i].Method == null)
				{
					Debug.LogWarning($"Smart Error: Command '{CommandRegistry[i].Name}' at index {i} could not be loaded.");
				}
				else
				{
					if (CommandRegistry[i].Method.IsStatic)
					{
						// this command is static and do not needs target
						// add it directly to available list
						AvailableCommands.Add(CommandRegistry[i]);
					}
					else
					{
						// this command is instance and do needs target
						// try find target
						Type targetType = CommandRegistry[i].Method.ReflectedType;

#if UNITY_2022_3_OR_NEWER
						object target = FindAnyObjectByType(targetType);
#else
						object target = FindObjectOfType(targetType);
#endif

						if (target != null)
						{
							// target found
							// add it to available list
							AvailableCommands.Add(CommandRegistry[i]);
						}
					}
				}
			}
		}

#if UNITY_EDITOR
		[SerializeField] private ConsolePreferences m_Preferences;

		private List<string> m_CommandRegistryNames = new List<string>();

		private void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.EnteredPlayMode)
			{
				if (m_Preferences.CacheReloadOnPlayMode)
				{
					Clear();
					Load();
				}

				RefreshAutocomplete();
			}
		}

		public void Load()
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();

			// reset name
			if (m_CommandRegistryNames.Count > 0)
			{
				m_CommandRegistryNames.Clear();
			}

			// get the name of the current assembly
			string currentAssemblyName = Assembly.GetExecutingAssembly().GetName().FullName;

			// find methods in all assemblies that has the [Command] attribute
			CommandRegistry = AppDomain.CurrentDomain.GetAssemblies()
				.Where(assembly => IsAssemblyConcerned(assembly, currentAssemblyName))
				.SelectMany(assembly => assembly.GetTypes())
				.SelectMany(type => type.GetMethods(
					BindingFlags.Public |
					BindingFlags.NonPublic |
					BindingFlags.Static |
					BindingFlags.Instance))
				.Where(IsValidMethod)
				.Select(CreateCommand)
				.ToArray();

			Array.Sort(CommandRegistry, (Command cmd1, Command cmd2) => string.Compare(cmd1.Name, cmd2.Name, true));

			watch.Stop();
			if (ShowLogs)
			{
				Debug.Log($"Cache loaded successfully in {watch.Elapsed.Milliseconds} ms.");
			}

			if (this != null)
			{
				EditorUtility.SetDirty(this);
			}

			AssetDatabase.SaveAssets();
		}

		public void Clear()
		{
			Array.Clear(CommandRegistry, 0, CommandRegistry.Length);
			CommandRegistry = new Command[0];

			AvailableCommands.Clear();

			if (ShowLogs)
			{
				Debug.Log($"Cache cleared successfully.");
			}

			if (this != null)
			{
				EditorUtility.SetDirty(this);
			}

			AssetDatabase.SaveAssets();
		}

		private bool IsAssemblyConcerned(Assembly assembly, string currentAssemblyName)
		{
			if (string.Equals(assembly.FullName, currentAssemblyName))
			{
				return true;
			}

			return assembly.GetReferencedAssemblies().Any(a => string.Equals(a.FullName, currentAssemblyName));
		}

		private bool IsValidMethod(MethodInfo method)
		{
			CommandAttribute commandAttribute = (CommandAttribute)method.GetCustomAttribute(typeof(CommandAttribute), false);

			if (commandAttribute == null)
			{
				return false;
			}

			if (m_CommandRegistryNames.Count == 0)
			{
				return true;
			}

			string commandName = GetCommandName(commandAttribute, method.Name);

			if (m_CommandRegistryNames.Contains(commandName))
			{
				// this command name is not available
				Debug.LogWarning($"Smart Error: Command '{commandName}' could not be added because its name is duplicated.");
				return false;
			}

			return true;
		}

		private Command CreateCommand(MethodInfo methodInfo)
		{
			CommandAttribute commandAttribute = (CommandAttribute)methodInfo.GetCustomAttribute(typeof(CommandAttribute), false);
			string commandName = GetCommandName(commandAttribute, methodInfo.Name);
			
			CommandGroupAttribute commandGroupAttribute = (CommandGroupAttribute)methodInfo.ReflectedType.GetCustomAttribute(typeof(CommandGroupAttribute), false);
			string group = "Default";

			if (commandGroupAttribute != null)
			{
				group = GetCommandGroupName(commandGroupAttribute, methodInfo.ReflectedType.Name);
			}

			m_CommandRegistryNames.Add(commandName);

			return new Command(commandName, commandAttribute.Description, commandAttribute.MonoTargetType, group, new SerializableMethodInfo(methodInfo));
		}

		private string GetCommandName(CommandAttribute commandAttribute, string defaultName)
		{
			string commandName = defaultName;

			if (commandAttribute.HasName())
			{
				commandName = commandAttribute.Name;

				if (commandName.Contains(' '))
				{
					commandName = commandName.Replace(" ", "");
				}
			}

			return commandName;
		}

		private string GetCommandGroupName(CommandGroupAttribute commandGroupAttribute, string defaultName)
		{
			string commandGroupName = defaultName;

			if (commandGroupAttribute.HasName())
			{
				commandGroupName = commandGroupAttribute.Name;

				if (commandGroupName.Contains(' '))
				{
					commandGroupName = commandGroupName.Replace(" ", "");
				}
			}

			return commandGroupName;
		}
#endif
	}
}