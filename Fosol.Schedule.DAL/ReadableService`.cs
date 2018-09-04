using Fosol.Schedule.DAL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Schedule.DAL
{
    /// <summary>
    /// ReadableService abstract class, provides a common generic implementation for all services.  This provides a way to manage entities in the datasource.
    /// </summary>
    /// <typeparam name="EntityT"></typeparam>
    /// <typeparam name="ModelT"></typeparam>
    public abstract class ReadableService<EntityT, ModelT> : IReadableService<ModelT>
        where EntityT : class
        where ModelT : class
    {
        #region Variables
        private readonly DataSource _source;
        #endregion

        #region Properties
        /// <summary>
        /// get - The datasource.
        /// </summary>
        internal DataSource Source { get { return _source; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ReadableService object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="source"></param>
        internal ReadableService(DataSource source)
        {
            _source = source;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Find the entity for the specified model in the datasource.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="model"></param>
        /// <returns></returns>
        protected EntityT Find(ModelT model)
        {
            // TODO: Need to rewrite to handle different primary key configurations.
            var id = (int)typeof(ModelT).GetProperty("Id").GetValue(model);
            return this.Find(id);
        }

        /// <summary>
        /// Find the entity in the datasource.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        protected EntityT Find(params object[] keyValues)
        {
            var entity = _source.Context.Set<EntityT>().Find(keyValues);

            if (entity == null)
                throw new NoContentException(typeof(ModelT));

            return entity;
        }

        /// <summary>
        /// Get the model for the specified key values.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public ModelT Get(params object[] keyValues)
        {
            return _source.Mapper.Map<EntityT, ModelT>(Find(keyValues));
        }
        #endregion
    }
}
