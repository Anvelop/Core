// Шут Андрей С.15:22

using System;
using UnityEngine;

namespace Anvelop.Core.Extensions
{
	public static class GenericExtensions
	{
		public static T Using<T>(this T component, Action<T> action)
		{
			action(component);
			return component;
		}
	}
}