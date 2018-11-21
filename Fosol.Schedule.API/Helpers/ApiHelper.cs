using Fosol.Core.Extensions.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Xml.XPath;

namespace Fosol.Schedule.API.Helpers
{
	/// <summary>
	/// ApiHelper static class, provides methods to help with describing the API endpoints.
	/// </summary>
	static class ApiHelper
	{
		#region Variables
		private static readonly IEnumerable<dynamic> _endpoints;
		private static readonly IDictionary<Type, object> _models = new Dictionary<Type, object>();
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes the static properties.
		/// </summary>
		static ApiHelper()
		{
			_endpoints = Initialize();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Get all the endpoints in the application and return their information (including documentation) as an object.
		/// When including a name it will return only the controller with the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static IEnumerable<dynamic> GetEndpoints(string name = null)
		{
			if (String.IsNullOrWhiteSpace(name)) return _endpoints;
			return _endpoints.Where(c => String.Compare(c.Name, name, true) == 0);
		}

		/// <summary>
		/// Get the endpoint with the specified name.
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static dynamic GetEndpoint(string controller, string name)
		{
			var ctrl = GetEndpoints(controller);

			return ((IEnumerable<dynamic>)ctrl.FirstOrDefault()?.Endpoints).FirstOrDefault(e => String.Compare(e.Name, name) == 0);
		}

		/// <summary>
		/// Get the model definition of the specified type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static dynamic GetModel(Type type)
		{
			ExpandoObject model = new ExpandoObject();

			if (_models.ContainsKey(type)) return _models[type];

			if (type.IsEnum)
			{
				model = GetEnum(type);
			}
			else
			{
				foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
				{
					model.TryAdd(prop.Name, new ModelPropertyInfo(prop.PropertyType));
				}
			}

			if (!_models.TryAdd(type, model)) throw new InvalidOperationException($"Unable to create a model for the specified type '{type.Name}'.");

			return model;
		}

		public struct ModelPropertyInfo
		{
			#region Properties
			public string Type { get; }

			public bool IsEnum { get; }

			public bool IsNullable { get; }

			public bool IsArray { get; }

			public bool IsPrimitive { get; }
			#endregion

			#region Constructors
			public ModelPropertyInfo(Type type)
			{
				this.Type = type.IsGenericType ? type.GetGenericArguments()[0].Name : type.Name;
				this.IsEnum = type.IsEnum;
				this.IsNullable = type.IsNullable();
				this.IsArray = type.IsArray || type.IsEnumerable() && type != typeof(string);
				this.IsPrimitive = type.IsPrimitive;
			}
			#endregion
		}

		/// <summary>
		/// Creates an example object of the specified model type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static object GetModelExample(Type type)
		{
			var model = GetModel(type);
			if (type.IsEnum) return model;

			var gmodel = (IDictionary<string, object>)model;
			var example = Activator.CreateInstance(type);
			var garray = typeof(List<>);

			foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				var info = (ModelPropertyInfo)gmodel[prop.Name];

				object pexample;
				if (info.IsArray)
				{
					var agtype = Assembly.GetAssembly(typeof(Models.Calendar)).GetType($"Fosol.Schedule.Models.{info.Type}") ?? Assembly.GetAssembly(typeof(Entities.Calendar)).GetType($"Fosol.Schedule.Entities.{info.Type}") ?? throw new InvalidOperationException($"Unable to create a default property for type '{prop.Name}'='{info.Type}'.");
					var atype = garray.MakeGenericType(agtype);
					pexample = Activator.CreateInstance(atype);
					//atype.GetMethod("Add").Invoke(pexample, new[] { Activator.CreateInstance(agtype) });
				}
				else if (prop.PropertyType == typeof(string))
				{
					if (prop.Name == "RowVersion")
					{
						pexample = "Ax34ldjk34k4";
					}
					else
					{
						pexample = "Example text";
					}
				}
				else if (prop.PropertyType.IsNullableType())
				{
					var gtype = prop.PropertyType.GetGenericArguments()[0];
					pexample = Activator.CreateInstance(gtype);
				}
				else
				{
					pexample = Activator.CreateInstance(prop.PropertyType);
				}
				prop.SetValue(example, pexample);
			}

			return example;
		}

		/// <summary>
		/// Get the model definition containing the enum values.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private static dynamic GetEnum(Type type)
		{
			ExpandoObject model = new ExpandoObject();
			foreach (var value in Enum.GetValues(type))
			{
				model.TryAdd(value.ToString(), (int)value);
			}
			return model;
		}

		/// <summary>
		/// Get all the endpoints in the application and return their information (including documentation) as an object.
		/// </summary>
		/// <returns></returns>
		private static IEnumerable<object> Initialize()
		{
			var xml = new XPathDocument($@"{AppDomain.CurrentDomain.BaseDirectory}/{typeof(Program).Assembly.GetName().Name}.xml");
			var nav = xml.CreateNavigator();

			return Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => t.IsSubclassOf(typeof(Controller))).Select(t => new
			{
				Name = GetControllerName(t),
				Endpoints = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Select(mi => new
				{
					mi.Name,
					Summary = GetValue(nav, $"{GetMemberPath(t, mi)}/summary"),
					Routes = GetRoutes(t, mi),
					Parameters = GetParameters(t, mi, nav)
				})
			});
		}

		private static string GetValue(XPathNavigator nav, string xpath)
		{
			return nav.SelectSingleNode(xpath)?.Value.Trim();
		}

		private static string GetMemberPath(Type type, MethodInfo methodInfo)
		{
			var parameters = methodInfo.GetParameters();
			if (parameters.Length == 0)
				return $"/doc/members/member[@name='M:{type.FullName}.{methodInfo.Name}']";

			var paramPath = $"({String.Join(",", parameters.Select(p => GetParameterType(p)))})";
			return $"/doc/members/member[@name='M:{type.FullName}.{methodInfo.Name}{paramPath}']";
		}

		private static string GetParameterType(ParameterInfo parameterInfo)
		{
			var type = parameterInfo.ParameterType;
			if (type.IsNullableType())
				return $"System.Nullable{{{type.GetGenericArguments()[0]}}}";

			return type.FullName;
		}

		private static string GetControllerName(Type type)
		{
			return type.Name.Replace("Controller", "");
		}

		private static IEnumerable<string> GetRoutes(Type type)
		{
			var controller = GetControllerName(type);
			var area = type.GetCustomAttribute<AreaAttribute>()?.RouteValue;
			return type.GetCustomAttributes<RouteAttribute>().Select(ra => ra.Template.Replace("[area]", area).Replace("[controller]", controller).ToLower()).Distinct();
		}

		private static IEnumerable<string> GetRoutes(Type type, MethodInfo methodInfo)
		{
			var controller = GetControllerName(type);
			var area = type.GetCustomAttribute<AreaAttribute>()?.RouteValue;
			var controller_routes = GetRoutes(type);
			var endpoint_routes = methodInfo.GetCustomAttributes<RouteAttribute>().Select(ra => ra.Template.Replace("[area]", area).Replace("[controller]", controller).ToLower());
			var http_methods = methodInfo.GetCustomAttributes<HttpMethodAttribute>().Select(hga => hga.Template?.Replace("[area]", area).Replace("[controller]", controller).ToLower() ?? methodInfo.Name.ToLower());
			return endpoint_routes.Concat(http_methods).Distinct().Select(r => r.StartsWith("/") ? new[] { r } : controller_routes.Select(cr => $"/{cr}/{r}")).SelectMany(r => r);
		}

		private static IEnumerable<object> GetParameters(Type type, MethodInfo methodInfo, XPathNavigator nav)
		{
			return methodInfo.GetParameters().Select(p => new
			{
				p.Name,
				Type = GetParameterType(p),
				Summary = GetValue(nav, $"{GetMemberPath(type, methodInfo)}/param[@name='{p.Name}']")
			});
		}
		#endregion
	}
}
