using System;
using System.Collections.Generic;
using System.Reflection;

namespace Buddy_dump.BotBehaviors
{
	public static class Selector
	{
		private static readonly Dictionary<string, Type> Behaviors;

		static Selector()
		{
			Behaviors = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
			{
				{ HB2.GetBotSig, typeof(HB2) },
				{ DB.GetBotSig, typeof(DB) }
			};
		}

		public static IBotBehavior GetBehavior(string botSig, string meshPath, string offsetsPath, string mapListPath)
		{
			if (string.IsNullOrEmpty(botSig))
				return null;

			if (Behaviors.TryGetValue(botSig, out Type type))
			{
				ConstructorInfo constructor = type.GetConstructor(new Type[3]
				{
					typeof(string),
					typeof(string),
					typeof(string)
				});

				if (constructor != null)
				{
					return (IBotBehavior)constructor.Invoke(new object[3] { meshPath, offsetsPath, mapListPath });
				}
			}
			return null;
		}
	}
}
