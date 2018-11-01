using Fosol.Overseer.Triggers;
using Fosol.Overseer.Requesting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fosol.Overseer
{
    public static class IServiceCollectionExtensions
    {
        #region Methods
        /// <summary>
        /// Add overseer to the service collection.
        /// Add all requestors in all assemblies to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddOverseer(this IServiceCollection services)
        {
            return services.AddOverseer(AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Add overseer to the service collection.
        /// Add all requestors in all assemblies to the service collection.
        /// Add all triggers in all assemblies to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddOverseerWithTriggers(this IServiceCollection services)
        {
            return services.AddOverseerWithTriggers(AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Add overseer to the service collection.
        /// Add all requestors in all specified assemblies to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddOverseer(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddRequiredServices();
            services.AddClasses(assemblies);
            return services;
        }

        /// <summary>
        /// Add overseer to the service collection.
        /// Add all requestors in all assemblies to the service collection.
        /// Add all triggers in all specified assemblies to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddOverseerWithTriggers(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddOverseer(assemblies);
            services.AddScoped(typeof(IRequestTrigger<,>), typeof(PreRequestTrigger<,>));
            services.AddScoped(typeof(IRequestTrigger<,>), typeof(PostRequestTrigger<,>));

            // Add all pipelines to the service provider.
            var preRequestTrigger = typeof(IPreRequestTrigger<,>);
            var postRequestTrigger = typeof(IPostRequestTrigger<,>);

            foreach (var addType in new[] { preRequestTrigger, postRequestTrigger })
            {
                var concretions = new List<Type>();
                var interfaces = new List<Type>();

                foreach (var assemblyType in assemblies.SelectMany(a => a.DefinedTypes))
                {
                    IEnumerable<Type> types = assemblyType.FindInterfacesThatClose(addType).ToArray();
                    if (!types.Any()) continue;

                    if (assemblyType.IsConcrete())
                    {
                        concretions.Add(assemblyType);
                    }
                }

                concretions.ForEach(t => services.AddTransient(addType, t));
            }
            return services;
        }

        /// <summary>
        /// Add all requestors to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private static IServiceCollection AddClasses(this IServiceCollection services, params Assembly[] assemblies)
        {
            // Add all requestors to the service provider.
            var requestorType = typeof(IRequestor<,>);
            services.AddAsTransient(new[] { requestorType }, assemblies, false);
            return services;
        }

        /// <summary>
        /// Add these types to the service collection as transient.
        /// For requestors 'addIfAlreadyExists' should be set to 'false', as they should only be added once.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="addTypes"></param>
        /// <param name="assemblies"></param>
        /// <param name="addIfAlreadyExists">Whether an existing type should be added, or ignored.</param>
        /// <returns></returns>
        private static IServiceCollection AddAsTransient(this IServiceCollection services, IEnumerable<Type> addTypes, IEnumerable<Assembly> assemblies, bool addIfAlreadyExists)
        {
            foreach (var addType in addTypes)
            {
                var concretions = new List<Type>();
                var interfaces = new List<Type>();

                foreach (var assemblyType in assemblies.SelectMany(a => a.DefinedTypes))
                {
                    IEnumerable<Type> types = assemblyType.FindInterfacesThatClose(addType).ToArray();
                    if (!types.Any()) continue;

                    if (assemblyType.IsConcrete())
                    {
                        concretions.Add(assemblyType);
                    }

                    foreach (var t in types)
                    {
                        interfaces.Fill(t);
                    }
                }

                foreach (var @interface in interfaces)
                {
                    var matches = concretions.Where(t => t.CanBeCastTo(@interface)).ToList();

                    if (addIfAlreadyExists)
                    {
                        matches.ForEach(match => services.AddTransient(@interface, match));
                    }
                    else
                    {
                        if (matches.Count() > 1)
                        {
                            matches.RemoveAll(m => !IsMatchingWithInterface(m, @interface));
                        }

                        matches.ForEach(match => services.AddTransient(@interface, match));
                    }

                    if (!@interface.IsOpenGeneric())
                    {
                        services.AddConcretionsThatCouldBeClosed(@interface, concretions);
                    }
                }
            }
            return services;
        }

        private static IServiceCollection AddConcretionsThatCouldBeClosed(this IServiceCollection services, Type interfaceType, IEnumerable<Type> concretions)
        {
            foreach (var type in concretions.Where(t => t.IsOpenGeneric() && t.CouldCloseTo(interfaceType)))
            {
                try
                {
                    services.TryAddTransient(interfaceType, type.MakeGenericType(interfaceType.GenericTypeArguments));
                }
                catch
                {
                    // Ignore for now.
                }
            }

            return services;
        }

        private static bool CouldCloseTo(this Type type, Type interfaceType) // ToDO: Move to extensions.
        {
            var openInterface = interfaceType.GetGenericTypeDefinition();
            var arguments = interfaceType.GenericTypeArguments;

            var concreteArguments = type.GenericTypeArguments;
            return arguments.Length == concreteArguments.Length && type.CanBeCastTo(openInterface);
        }

        private static bool IsOpenGeneric(this Type type) // ToDO: Move to extensions.
        {
            return type.GetTypeInfo().IsGenericTypeDefinition || type.GetTypeInfo().ContainsGenericParameters;
        }

        private static bool IsMatchingWithInterface(this Type type, Type interfaceType) // ToDO: Move to extensions.
        {
            if (type == null || interfaceType == null) return false;

            if (type.IsInterface)
            {
                if (type.GenericTypeArguments.SequenceEqual(interfaceType.GenericTypeArguments)) return true;
            }
            else
            {
                return type.GetInterface(interfaceType.Name).IsMatchingWithInterface(interfaceType);
            }

            return false;
        }

        private static void Fill<T>(this IList<T> list, T value) // ToDO: Move to extensions.
        {
            if (list.Contains(value)) return;
            list.Add(value);
        }

        private static bool CanBeCastTo(this Type baseType, Type castToType) // ToDO: Move to extensions.
        {
            if (baseType == null) return false;
            if (baseType == castToType) return true;
            return castToType.GetTypeInfo().IsAssignableFrom(baseType.GetTypeInfo());
        }

        private static IEnumerable<Type> FindInterfacesThatClose(this Type assemblyType, Type findType) // ToDO: Move to extensions.
        {
            // Do not return abstract or interfaces.
            if (!assemblyType.IsConcrete()) yield break;

            if (findType.GetTypeInfo().IsInterface)
            {
                // If the assembly type implements the interface, return it.
                foreach (var interfaceType in assemblyType.GetTypeInfo().ImplementedInterfaces.Where(type => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == findType))
                {
                    yield return interfaceType;
                }
            }
            else if (assemblyType.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType && assemblyType.GetTypeInfo().BaseType.GetGenericTypeDefinition() == findType)
            {
                // If the assembly type is a generic and implements the find type.
                yield return assemblyType.GetTypeInfo().BaseType;
            }

            // Do not return base objects
            if (assemblyType == typeof(object)) yield break;
            if (assemblyType.GetTypeInfo().BaseType == typeof(object)) yield break;

            // Include each inherited type.
            foreach (var interfaceType in assemblyType.GetTypeInfo().BaseType.FindInterfacesThatClose(findType))
            {
                yield return interfaceType;
            }
        }

        /// <summary>
        /// Returns true if the type is no abstract and is not an interface.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsConcrete(this Type type) // ToDO: Move to extensions.
        {
            return !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;
        }

        private static IServiceCollection AddRequiredServices(this IServiceCollection services)
        {
            services.AddScoped<ServiceFactory>(provider => type =>
            {
                try
                {
                    // Try the default service provider first.
                    return provider.GetService(type);
                }
                catch (ArgumentException)
                {
                    // If the type is an enumerable collection of types try and get them.
                    if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        var serviceType = type.GenericTypeArguments.Single();
                        var serviceTypes = new List<Type>();
                        foreach (var service in services)
                        {
                            if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == service.ServiceType)
                            {
                                try
                                {
                                    var implementationType = service.ImplementationType.MakeGenericType(serviceType.GenericTypeArguments);
                                    serviceTypes.Add(implementationType);
                                }
                                catch
                                {
                                    // Ignore for now.
                                }
                            }
                        }

                        services.Replace(new ServiceDescriptor(type, sp =>
                        {
                            return serviceTypes.Select(sp.GetService).ToArray();
                        }, ServiceLifetime.Transient));

                        // Return an array of the type.
                        var resolved = Array.CreateInstance(serviceType, serviceTypes.Count);
                        Array.Copy(serviceTypes.Select(provider.GetService).ToArray(), resolved, serviceTypes.Count);
                        return resolved;
                    }
                    throw;
                }
            });
            services.AddScoped<IOverseer, Overseer>();

            return services;
        }
        #endregion
    }
}
