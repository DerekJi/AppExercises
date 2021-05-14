using AppEx.Core.Attributes;
using AppEx.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using AppEx.Core.Services;

namespace AppEx.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddScoped<IConfigurationService, ConfigurationService>(p =>
                {
                    return new ConfigurationService(configuration);
                })
                .AddSingleton<IConfiguration>(configuration)
                .AddClassesWithServiceAttribute("AppEx.*");

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static void AddClassesWithServiceAttribute(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddClassesWithServiceAttribute(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="assemblyFilters"></param>
        public static void AddClassesWithServiceAttribute(this IServiceCollection serviceCollection,
            params string[] assemblyFilters)
        {
            var assemblies = GetAssemblies(assemblyFilters);
            serviceCollection.AddClassesWithServiceAttribute(assemblies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="assemblies"></param>
        public static void AddClassesWithServiceAttribute(this IServiceCollection serviceCollection,
            params Assembly[] assemblies)
        {
            var typesWithAttributes = assemblies
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(GetExportedTypes)
                .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition)
                .Select(type => new
                {
                    type.GetCustomAttribute<ServiceAttribute>()?.Lifetime,
                    ServiceType = type,
                    ImplementationType = type.GetCustomAttribute<ServiceAttribute>()?.ServiceType
                })
                .Where(t => t.Lifetime != null);

            foreach (var type in typesWithAttributes)
                if (type.ImplementationType == null)
                    serviceCollection.Add(type.ServiceType, type.Lifetime.Value);
                else
                    serviceCollection.Add(type.ImplementationType, type.ServiceType, type.Lifetime.Value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="type"></param>
        /// <param name="lifetime"></param>
        public static void Add(this IServiceCollection serviceCollection, Type type, Lifetime lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.Singleton:
                    serviceCollection.AddSingleton(type);
                    break;
                case Lifetime.Scoped:
                    serviceCollection.AddScoped(type);
                    break;
                case Lifetime.Transient:
                    serviceCollection.AddTransient(type);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        public static void Add(this IServiceCollection serviceCollection, Type serviceType, Type implementationType,
            Lifetime lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.Singleton:
                    serviceCollection.AddSingleton(serviceType, implementationType);
                    break;
                case Lifetime.Scoped:
                    serviceCollection.AddScoped(serviceType, implementationType);
                    break;
                case Lifetime.Transient:
                    serviceCollection.AddTransient(serviceType, implementationType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="implementsType"></param>
        /// <param name="assemblies"></param>
        /// <param name="classFilter"></param>
        /// <returns></returns>
        private static IEnumerable<Type> GetTypesImplementing(Type implementsType, IEnumerable<Assembly> assemblies,
            params string[] classFilter)
        {
            var types = GetTypesImplementing(implementsType, assemblies.ToArray());
            if (classFilter != null && classFilter.Any())
                types = types.Where(type => classFilter.Any(filter => IsWildcardMatch(type.FullName, filter)));
            return types;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="implementsType"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private static IEnumerable<Type> GetTypesImplementing(Type implementsType, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0) return new Type[0];

            var targetType = implementsType;

            return assemblies
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(GetExportedTypes)
                .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition && targetType.IsAssignableFrom(type))
                .ToArray();
        }

        /// <summary>
        ///     Checks if a string matches a wildcard argument (using regex)
        /// </summary>
        private static bool IsWildcardMatch(string input, string wildcard)
        {
            return input == wildcard || Regex.IsMatch(input,
                       "^" + Regex.Escape(wildcard).Replace("\\*", ".*").Replace("\\?", ".") + "$",
                       RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyFilters"></param>
        /// <returns></returns>
        private static Assembly[] GetAssemblies(IEnumerable<string> assemblyFilters)
        {
            var assemblies = new List<Assembly>();
            foreach (var assemblyFilter in assemblyFilters)
                assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => IsWildcardMatch(assembly.GetName().Name, assemblyFilter)).ToArray());
            return assemblies.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static IEnumerable<Type> GetExportedTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetExportedTypes();
            }
            catch (NotSupportedException)
            {
                // A type load exception would typically happen on an Anonymously Hosted DynamicMethods
                // Assembly and it would be safe to skip this exception.
                return Type.EmptyTypes;
            }
            catch (FileLoadException)
            {
                // The assembly points to a not found assembly - ignore and continue
                return Type.EmptyTypes;
            }
            catch (ReflectionTypeLoadException ex)
            {
                // Return the types that could be loaded. Types can contain null values.
                return ex.Types.Where(type => type != null);
            }
            catch (Exception ex)
            {
                // Throw a more descriptive message containing the name of the assembly.
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "Unable to load types from assembly {0}. {1}",
                        assembly.FullName, ex.Message), ex);
            }
        }
    }
}
