using Fosol.Core.Extensions.Enumerable;
using Fosol.Schedule.DAL.Helpers;
using Fosol.Schedule.DAL.Interfaces;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fosol.Schedule.DAL.Services
{
    /// <summary>
    /// UpdatableService abstract class, provides a common generic implementation for all services.  This provides a way to manage entities in the datasource.
    /// </summary>
    /// <typeparam name="EntityT"></typeparam>
    /// <typeparam name="ModelT"></typeparam>
    public abstract class UpdatableService<EntityT, ModelT> : ReadableService<EntityT, ModelT>, IUpdatableService<ModelT>, IDisposable
        where EntityT : class
        where ModelT : class
    {
        #region Variables
        private readonly ConcurrentDictionary<EntityT, ModelT> _tracking = new ConcurrentDictionary<EntityT, ModelT>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UpdatableService object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="source"></param>
        internal UpdatableService(IDataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sync models with the tracked entities.
        /// Copies property values from the entity to the model.
        /// </summary>
        protected void Sync()
        {
            foreach (var track in _tracking)
            {
                this.Map(track.Key, track.Value);
            }
            _tracking.Clear();
        }

        /// <summary>
        /// Keep a reference to the entity and it's model so that they can be updated after a commit to the datasource.
        /// This is used for generated property values.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        protected void Track(EntityT entity, ModelT model)
        {
            _tracking.AddOrUpdate(entity, model, (key, existingValue) =>
            {
                return model;
            });
        }

        private void Transform(EntityT entity)
        {
            var type = typeof(EntityT);
            var collections = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => typeof(IEnumerable).IsAssignableFrom(p.PropertyType)).ToArray();
            var entities = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.PropertyType.IsClass && p.PropertyType != typeof(string)).ToArray();
        }

        /// <summary>
        /// Add the specified entity to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void Add(EntityT entity)
        {
            this.Context.Set<EntityT>().Add(entity);
        }

        /// <summary>
        /// Add the specified model to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="model"></param>
        public virtual void Add(ModelT model)
        {
            var entity = this.AddMap(model);
            this.Add(entity);
            Track(entity, model);
        }

        /// <summary>
        /// Add the specified models to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="models"></param>
        public virtual void AddRange(IEnumerable<ModelT> models)
        {
            models.ForEach(m => this.Add(m));
        }

        /// <summary>
        /// Add the specified models to the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <param name="models"></param>
        public virtual void AddRange(params ModelT[] models)
        {
            models.ForEach(m => this.Add(m));
        }

        /// <summary>
        /// Remove the specified model from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="model"></param>
        public virtual void Remove(ModelT model)
        {
            var entity = this.Source.UpdateMapper.Map(model, this.Find<EntityT>(model));
            this.Context.Set<EntityT>().Remove(entity);
        }

        /// <summary>
        /// Remove the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public virtual void RemoveRange(IEnumerable<ModelT> models)
        {
            // TODO: Need to rewrite because this will make a separate request for each model.
            var entities = models.Select(m => this.Find<EntityT>(m));
            this.Context.Set<EntityT>().RemoveRange(entities);
        }

        /// <summary>
        /// Remove the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public virtual void RemoveRange(params ModelT[] models)
        {
            this.RemoveRange(models.AsEnumerable());
        }

        /// <summary>
        /// Update the specified entity from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="entity"></param>
        protected virtual void Update(EntityT entity)
        {
            this.Context.Set<EntityT>().Update(entity);

            // This is required because EF is broken.
            var baseEntity = entity as Entities.BaseEntity;
            if (baseEntity != null)
            {
                this.Context.Entry(entity).Property(nameof(Entities.BaseEntity.RowVersion)).OriginalValue = (entity as Entities.BaseEntity).RowVersion;
                var addedById = this.Context.Entry(entity).Property(nameof(Entities.BaseEntity.AddedById));
                addedById.CurrentValue = addedById.OriginalValue;
                var addedOn = this.Context.Entry(entity).Property(nameof(Entities.BaseEntity.AddedOn));
                addedOn.CurrentValue = addedOn.OriginalValue;
            }
        }

        /// <summary>
        /// Update the specified model from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="model"></param>
        public virtual void Update(ModelT model)
        {
            var entity = this.Source.UpdateMapper.Map(model, this.Find<EntityT>(model));
            this.Update(entity);
            Track(entity, model);
        }

        /// <summary>
        /// Update the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public virtual void UpdateRange(IEnumerable<ModelT> models)
        {
            // TODO: Need to rewrite because this will make a separate request for each model.
            var entities = models.Select(m => new Tuple<EntityT, ModelT>(this.UpdateMap(m), m));
            this.Context.Set<EntityT>().UpdateRange(entities.Select(t => t.Item1));
            entities.ForEach(t => Track(t.Item1, t.Item2));
        }

        /// <summary>
        /// Update the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="models"></param>
        public virtual void UpdateRange(params ModelT[] models)
        {
            this.UpdateRange(models.AsEnumerable());
        }

        /// <summary>
        /// Clear tracking references.
        /// </summary>
        public void Dispose()
        {
            _tracking.Clear();
        }
        #endregion
    }
}
