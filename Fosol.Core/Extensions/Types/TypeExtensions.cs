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
		/// <summary>
		/// Determine if the type of the specificed type is a `Nullable<>` type or is a nullable class.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsNullable(this Type type)
		{
			return type.IsClass || type.IsNullableType();
		}

		/// <summary>
		/// Determine if the type of the specificed object is a `Nullable<>` type or is a nullable class.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool IsNullable<T>(T obj)
		{
			return typeof(T).IsNullable();
		}
		/// <summary>
		/// Determine if the type of the specificed type is a `Nullable<>` type..
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsNullableType(this Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		/// <summary>
		/// Determine if the specified type is enumerable.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsEnumerable(this Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) return true;
			return type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
		}

		#endregion
	}
}
