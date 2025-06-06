using System;
using System.Reflection;
using UnityEngine;
using PedronsaDev.Console.Utils;

namespace PedronsaDev.Console
{
	[Serializable]
	public class Command
	{
		public string Name;
		public string Description;
		public MonoTargetType TargetType;
		public string Group;
		[SerializeField] private SerializableMethodInfo SerializableMethod;

		public MethodInfo Method => SerializableMethod.MethodInfo;

		public Command(string name, string description, MonoTargetType targetType, string group, SerializableMethodInfo serializableMethod)
		{
			Name = name;
			Description = description;
			TargetType = targetType;
			Group = group;
			SerializableMethod = serializableMethod;
		}

		public object Use(object target, object[] parameters = null)
		{
			return Method.Invoke(target, parameters);
		}

		/// <summary>
		/// Parse input parameters into their correct type
		/// </summary>
		/// <param name="inputParams">the input parameters to parse</param>
		/// <param name="parameters">the parsed result</param>
		public void GetParameters(string[] inputParams, out object[] parameters)
		{
			var parametersInfos = Method.GetParameters();
			parameters = new object[parametersInfos.Length];

			if (parametersInfos.Length < inputParams.Length)
			{
				throw new SmartParameterTooManyException(this, inputParams.Length);
			}

			for (int i = 0; i < parametersInfos.Length; i++)
			{
				object parameterValue;

				if (i >= inputParams.Length || string.IsNullOrEmpty(inputParams[i]))
				{
					// this input parameter is null or empty

					if (parametersInfos[i].HasDefaultValue)
					{
						// this input parameter has a default value
						parameterValue = parametersInfos[i].DefaultValue;
					}
					else
					{
						// this input parameter doesn't has a default value
						throw new SmartParameterNullException(parametersInfos[i].Name, this);
					}
				}
				else
				{
					try
					{
						// this input parameter need to be casted into object value
						parameterValue = TypeUtil.GetObjectValue(parametersInfos[i].ParameterType, inputParams[i]);
					}
					catch (InvalidCastException)
					{
						throw new SmartParameterInvalidCastException(inputParams[i], this, parametersInfos[i].ParameterType.Name);
					}
					catch (NotSupportedException)
					{
						throw new SmartParameterTypeNotSupportedException(parametersInfos[i].ParameterType.Name);
					}
					catch (Exception e)
					{
						throw new SmartException($"An error occurred while parsing the input parameters {e}");
					}
				}

				parameters[i] = parameterValue;
			}
		}
	}
}
