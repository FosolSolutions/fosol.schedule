using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Fosol.Core.Extensions.Types;

namespace Fosol.Core.Reflection
{
	public static class Mapper
	{
		/// <summary>
		/// Copy the source properties into the specified destination type.
		/// This will iterate through enumerable objects and map each item.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="action"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Map<T>(object source, Action<T> action = null)
			where T : class
		{
			var dest_type = typeof(T);
			return (T)Map(source, dest_type, new Action<object>(o => action((T)o)));
		}

		/// <summary>
		/// Copy the source properties into the specified destination type.
		/// This will iterate through enumerable objects and map each item.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destinationType"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		public static object Map(object source, Type destinationType, Action<object> action = null)
		{
			if (destinationType == null) throw new ArgumentNullException(nameof(destinationType));
			if (source == null && !destinationType.IsNullable()) throw new ArgumentNullException(nameof(source));
			else if (source == null) return null;

			var destination = Activator.CreateInstance(destinationType);
			Map(source, destination, action);
			return destination;
		}

		/// <summary>
		/// Copy the source properties into the specified destination.
		/// This will iterate through enumerable objects and map each item.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <param name="action"></param>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TDestination"></typeparam>
		public static void Map<TSource, TDestination>(TSource source, TDestination destination, Action<TDestination> action = null)
		{
			Map(source, destination, new Action<object>(o => action((TDestination)o)));
		}

		/// <summary>
		/// Copy the source properties into the specified destination.
		/// This will iterate through enumerable objects and map each item.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="destination"></param>
		/// <param name="action"></param>
		public static void Map(object source, object destination, Action<object> action = null)
		{
			if (destination == null) throw new ArgumentNullException(nameof(destination));

			var dest_type = destination.GetType();

			if (source == null && !dest_type.IsNullable()) throw new ArgumentNullException(nameof(source));
			else if (source == null)
			{
				destination = null;
				return;
			}

			var source_type = source.GetType();
			var dest_props = dest_type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

			foreach (var prop in dest_props)
			{
				// If the source has the property then attempt to copy.
				var source_prop = source_type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty).FirstOrDefault(p => p.Name == prop.Name);
				if (source_prop != null)
				{
					var source_value = source_prop.GetValue(source);

					// The properties are the same type - copy the value.
					if (source_prop.PropertyType == prop.PropertyType)
					{
						prop.SetValue(destination, source_value);
					}
					else if (source_prop.PropertyType.IsNullableType() && source_prop.PropertyType.GetGenericType() == prop.PropertyType && source_value != null)
					{
						prop.SetValue(destination, source_value);
					}
					else if (prop.PropertyType.IsNullableType() && prop.PropertyType.GetGenericType() == source_prop.PropertyType && source_value != null)
					{
						prop.SetValue(destination, source_value);
					}
					else if (prop.PropertyType.IsEnum)
					{
						if (source_prop.PropertyType == typeof(int) || source_prop.PropertyType == typeof(string))
						{
							var dest_value = Enum.Parse(prop.PropertyType, $"{source_value}");
							prop.SetValue(destination, dest_value);
						}
						else if (source_prop.PropertyType.IsEnum)
						{
							var dest_value = Enum.Parse(prop.PropertyType, $"{(int)source_value}");
							prop.SetValue(destination, dest_value);
						}
					}
					else if (prop.PropertyType.IsArray)
					{
						var item_type = prop.PropertyType.GetElementType();
						if (source_prop.PropertyType.IsArray && item_type == source_prop.PropertyType.GetElementType())
						{
							// The array is of the same type, copy the array.
							var source_array = source_value as object[];
							var dest_array = Activator.CreateInstance(prop.PropertyType, source_array.Length) as object[];
							for (var i = 0; i < dest_array.Length; i++)
							{
								dest_array[i] = Map(source_array[i], item_type);
							}
							source_array.CopyTo(dest_array, 0);
							prop.SetValue(destination, dest_array);
						}
						else if (item_type == typeof(byte) && source_prop.PropertyType.GetElementType() == typeof(string))
						{
							// Convert the string into a byte array.
							var dest_value = Convert.FromBase64String((string)source_value);
							prop.SetValue(destination, dest_value);
						}
						else if (source_prop.PropertyType.IsEnumerable())
						{
							// The source is not an array.
							if (source_value is IEnumerable enumerable)
							{
								var items = Activator.CreateInstance(typeof(List<>).MakeGenericType(item_type));
								var add_method = items.GetType().GetMethod("Add");
								var count = 0;
								foreach (var item in enumerable)
								{
									if (item.GetType() == item_type)
									{
										count++;
										var value = Map(item, item_type);
										add_method.Invoke(items, new object[] { value });
									}
								}
								var dest_array = Array.CreateInstance(prop.PropertyType, count);
								prop.SetValue(destination, ((List<object>)items).ToArray());
							}
						}
					}
					else if (prop.PropertyType == typeof(string) && source_prop.PropertyType.IsArray && source_prop.PropertyType.GetElementType() == typeof(byte))
					{
						// Convert the byte array into a string.
						var dest_value = Convert.ToBase64String((byte[])source_value);
						prop.SetValue(destination, dest_value);
					}
					else if (prop.PropertyType.IsEnumerable())
					{
						if (source_prop.PropertyType.IsEnumerable())
						{
							if (source_value is IEnumerable enumerable)
							{
								var item_type = prop.PropertyType.GetGenericType();
								var items = Activator.CreateInstance(typeof(List<>).MakeGenericType(item_type));
								var add_method = items.GetType().GetMethod("Add");
								foreach (var item in enumerable)
								{
									if (item.GetType() == item_type)
									{
										var value = Map(item, item_type);
										add_method.Invoke(items, new object[] { value });
									}
								}

								if (!prop.PropertyType.IsInterface)
								{
									prop.SetValue(destination, Activator.CreateInstance(prop.PropertyType), new object[] { items });
								}
								else
								{
									// Figure out what type of class to create for the interface.
									if (prop.PropertyType.IsAssignableFrom(typeof(List<>).MakeGenericType(item_type)))
									{
										var type = typeof(List<>).MakeGenericType(item_type);
										prop.SetValue(destination, Activator.CreateInstance(type, new object[] { items }));
									}
									else if (prop.PropertyType.IsAssignableFrom(typeof(Collection<>).MakeGenericType(item_type)))
									{
										var type = typeof(Collection<>).MakeGenericType(item_type);
										prop.SetValue(destination, Activator.CreateInstance(type, new object[] { items }));
									}
								}
							}
						}
					}
					else if (prop.PropertyType.IsClass && source_prop.PropertyType.IsClass)
					{
						var item = Map(source_value, prop.PropertyType);
						prop.SetValue(destination, item);
					}
					else
					{
						try
						{
							var dest_value = Convert.ChangeType(source_value, prop.PropertyType);
							prop.SetValue(destination, dest_value);
						}
						catch
						{
							// Ignore error, value can't be copied.
						}
					}
				}
			}

			action?.Invoke(destination);
		}
	}
}