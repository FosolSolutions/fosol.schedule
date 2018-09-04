using Fosol.Schedule.DAL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fosol.Schedule.DAL
{
    /// <summary>
    /// UpdatableService abstract class, provides a common generic implementation for all services.  This provides a way to manage entities in the datasource.
    /// </summary>
    /// <typeparam name="EntityT"></typeparam>
    /// <typeparam name="ModelT"></typeparam>
    public abstract class UpdatableService<EntityT, ModelT> : ReadableService<EntityT, ModelT>, IUpdatableService<ModelT>
        where EntityT : class
        where ModelT : class
    {
        #region Variables
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UpdatableService object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="source"></param>
        internal UpdatableService(DataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add the specified model to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="model"></param>
        public void Add(ModelT model)
        {
            var entity = this.Source.Mapper.Map<ModelT, EntityT>(model);
            this.Source.Context.Set<EntityT>().Add(entity);
        }

        /// <summary>
        /// Add the specified models to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="models"></param>
        public void AddRange(IEnumerable<ModelT> models)
        {
            var entities = models.Select(m => this.Source.Mapper.Map<ModelT, EntityT>(m));
            this.Source.Context.Set<EntityT>().AddRange(entities);
        }

        /// <summary>
        /// Add the specified models to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="models"></param>
        public void AddRange(params ModelT[] models)
        {
            var entities = models.Select(m => this.Source.Mapper.Map<ModelT, EntityT>(m));
            this.Source.Context.Set<EntityT>().AddRange(entities);
        }

        /// <summary>
        /// Remove the specified model from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="model"></param>
        public void Remove(ModelT model)
        {
            var entity = this.Source.Mapper.Map(model, this.Find(model));
            this.Source.Context.Set<EntityT>().Remove(entity);
        }

        /// <summary>
        /// Remove the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public void RemoveRange(IEnumerable<ModelT> models)
        {
            // TODO: Need to rewrite because this will make a separate request for each model.
            var entities = models.Select(m => this.Find(m));
            this.Source.Context.Set<EntityT>().RemoveRange(entities);
        }

        /// <summary>
        /// Remove the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public void RemoveRange(params ModelT[] models)
        {
            // TODO: Need to rewrite because this will make a separate request for each model.
            var entities = models.Select(m => this.Find(m));
            this.Source.Context.Set<EntityT>().RemoveRange(entities);
        }

        /// <summary>
        /// Update the specified model from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="model"></param>
        public void Update(ModelT model)
        {
            var entity = this.Source.Mapper.Map(model, this.Find(model));
            this.Source.Context.Set<EntityT>().Update(entity);
        }

        /// <summary>
        /// Update the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public void UpdateRange(IEnumerable<ModelT> models)
        {
            // TODO: Need to rewrite because this will make a separate request for each model.
            var entities = models.Select(m => this.Find(m));
            this.Source.Context.Set<EntityT>().UpdateRange(entities);
        }

        /// <summary>
        /// Update the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public void UpdateRange(params ModelT[] models)
        {
            // TODO: Need to rewrite because this will make a separate request for each model.
            var entities = models.Select(m => this.Find(m));
            this.Source.Context.Set<EntityT>().UpdateRange(entities);
        }
        #endregion
    }
}
