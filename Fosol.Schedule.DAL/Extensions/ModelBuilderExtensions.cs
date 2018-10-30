using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Fosol.Schedule.DAL.Extensions
{
    /// <summary>
    /// ModelBuilderExtensions static class, provides extension methods for ModelBuilder objects.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        #region Methods
        /// <summary>
        /// Applies all of the IEntityTypeConfiguration objects in all of the assemblies of the current domain.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyAllConfigurations(this ModelBuilder modelBuilder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                modelBuilder.ApplyAllConfigurations(assembly);
            }

            return modelBuilder;
        }

        /// <summary>
        /// Applies all of the IEntityTypeConfiguration objects in the assembly of the specified type.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyAllConfigurations(this ModelBuilder modelBuilder, Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return modelBuilder.ApplyAllConfigurations(type.Assembly);
        }

        /// <summary>
        /// Applies all of the IEntityTypeConfiguration objects in the specified assembly.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="assemblyType"></param>
        /// <returns></returns>
        public static ModelBuilder ApplyAllConfigurations(this ModelBuilder modelBuilder, Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            var type = typeof(IEntityTypeConfiguration<>);
            var configurations = assembly.GetTypes().Where(t => t.IsClass && type.IsAssignableFrom(t));

            var method = typeof(ModelBuilder).GetMethod(nameof(ModelBuilder.ApplyConfiguration));
            foreach (var config in configurations)
            {
                method.Invoke(modelBuilder, new[] { config });
            }

            return modelBuilder;
        }
        #endregion
    }
}
