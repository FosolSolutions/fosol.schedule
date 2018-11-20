using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Core.Extensions.Types
{
	/// <summary>
	/// TypeExtensions static class, provides extension methods for types.
	/// </summary>
	public static class TypeExtensions
	{
		#region Methods
		public static bool IsNullable(this Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public static bool IsNullable<T>(T obj)
		{
			return typeof(T).IsNullable();
		}

		public static bool IsEnumerable(this Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) return true;
			return type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
		}

		#endregion
	}
}
