using Fosol.Core.Extensions.Enumerable;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fosol.Schedule.DAL.Services
{
	/// <summary>
	/// UpdatableService abstract class, provides a common generic implementation for all services.  This provides a way to manage entities in the datasource.
	/// </summary>
	/// <typeparam name="EntityT"></typeparam>
	/// <typeparam name="ModelT"></typeparam>
	public abstract class UpdatableService<EntityT, ModelT> : UpdatableService<EntityT, ModelT, ModelT, ModelT, ModelT>
	  where EntityT : class
	  where ModelT : class
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of a UpdatableService object, and initializes it with the specified properties.
		/// </summary>
		/// <param name="source"></param>
		internal UpdatableService(IDataSource source) : base(source)
		{
		}
		#endregion
	}

	/// <summary>
	/// UpdatableService abstract class, provides a common generic implementation for all services.  This provides a way to manage entities in the datasource.
	/// </summary>
	/// <typeparam name="EntityT"></typeparam>
	/// <typeparam name="TCreate"></typeparam>
	/// <typeparam name="TRead"></typeparam>
	/// <typeparam name="TUpdate"></typeparam>
	/// <typeparam name="TDelete"></typeparam>
	public abstract class UpdatableService<EntityT, TCreate, TRead, TUpdate, TDelete> : ReadableService<EntityT, TRead>, IUpdatableService<TCreate, TRead, TUpdate, TDelete>, IDisposable
	  where EntityT : class
	  where TCreate : class
	  where TRead : class
	  where TUpdate : class
	  where TDelete : class
	{
		#region Variables
		private readonly ConcurrentDictionary<EntityT, object> _tracking = new ConcurrentDictionary<EntityT, object>();
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
				this.Source.Map(track.Key, track.Value);
			}
			_tracking.Clear();
		}

		/// <summary>
		/// Keep a reference to the entity and it's model so that they can be updated after a commit to the datasource.
		/// This is used for generated property values.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="model"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		protected T Track<T>(EntityT entity, T model = default)
		  where T : class
		{
			if (model == null) model = (T)Activator.CreateInstance(typeof(T));
			_tracking.AddOrUpdate(entity, model, (key, existingValue) =>
			{
				return model;
			});

			return model;
		}

		/// <summary>
		/// Get the model for the specified expression.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		protected TUpdate GetWithTracking(Expression<Func<EntityT, bool>> expression)
		{
			return this.Source.Map<TUpdate>(this.Find(set => set.AsNoTracking().SingleOrDefault(expression)));
		}

		/// <summary>
		/// Add the specified entity to the in-memory collection, so that it can be saved to the datasource on commit.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		protected TUpdate Add(EntityT entity)
		{
			this.VerifyPrincipal(true);
			if (entity is IBaseEntity track)
			{
				track.AddedById = this.GetUserId().Value;
				track.AddedOn = DateTime.UtcNow;
			}

			this.Context.Set<EntityT>().Add(entity);
			return Track<TUpdate>(entity);
		}

		/// <summary>
		/// Add the specified model to the in-memory collection, so that it can be saved to the datasource on commit.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public virtual TUpdate Add(TCreate model)
		{
			var entity = this.Source.Map<EntityT>(model);
			return this.Add(entity);
		}

		/// <summary>
		/// Add the specified models to the in-memory collection, so that it can be saved to the datasource on commit.
		/// </summary>
		/// <param name="models"></param>
		/// <returns></returns>
		public virtual IEnumerable<TUpdate> AddRange(IEnumerable<TCreate> models)
		{
			this.VerifyPrincipal(true);
			var entities = models.Select(m => this.Source.Map<EntityT>(m));
			var result = new List<TUpdate>();
			entities.ForEach(e => result.Add(this.Update(e)));
			return result;
		}

		/// <summary>
		/// Add the specified models to the in-memory collection, so that it can be saved to the datasource on commit.
		/// </summary>
		/// <param name="models"></param>
		/// <returns></returns>
		public virtual IEnumerable<TUpdate> AddRange(params TCreate[] models)
		{
			return this.AddRange(models.AsEnumerable());
		}

		/// <summary>
		/// Remove the entity from the in-memory collection, so that it can be removed from the datasource on commit.
		/// </summary>
		/// <param name="entity"></param>
		protected void Remove(EntityT entity)
		{
			this.VerifyPrincipal(true);
			this.Context.Set<EntityT>().Remove(entity);
		}

		/// <summary>
		/// Remove the specified model from the in-memory collection, so that it can be removed to the datasource on commit.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="model"></param>
		public virtual void Remove(TDelete model)
		{
			var entity = this.Source.Map(model, this.Find<EntityT>(model));
			Remove(entity);
		}

		/// <summary>
		/// Remove the specified models from the in-memory collection, so that it can be removed to the datasource on commit.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="models"></param>
		public virtual void RemoveRange(IEnumerable<TDelete> models)
		{
			this.VerifyPrincipal(true);
			// TODO: Need to rewrite because this will make a separate request for each model.
			var entities = models.Select(m => this.Find<EntityT>(m));
			this.Context.Set<EntityT>().RemoveRange(entities);
		}

		/// <summary>
		/// Remove the specified models from the in-memory collection, so that it can be removed to the datasource on commit.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="models"></param>
		public virtual void RemoveRange(params TDelete[] models)
		{
			this.RemoveRange(models.AsEnumerable());
		}

		/// <summary>
		/// Update the specified entity from the in-memory collection, so that it can be saved to the datasource on commit.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="entity"></param>
		/// <returns></returns>
		protected TUpdate Update(EntityT entity)
		{
			this.VerifyPrincipal(true);
			if (entity is IBaseEntity track)
			{
				track.UpdatedById = this.GetUserId().Value;
				track.UpdatedOn = DateTime.UtcNow;
			}

			this.Context.Set<EntityT>().Update(entity);

			if (entity is Entities.BaseEntity baseEntity)
			{
				// This is required because EF is broken.
				this.Context.Entry(baseEntity).Property(nameof(Entities.BaseEntity.RowVersion)).OriginalValue = baseEntity.RowVersion;

				// Updates are not allowed to change who added and when it was added.
				var addedById = this.Context.Entry(baseEntity).Property(nameof(Entities.BaseEntity.AddedById));
				addedById.CurrentValue = addedById.OriginalValue;
				var addedOn = this.Context.Entry(baseEntity).Property(nameof(Entities.BaseEntity.AddedOn));
				addedOn.CurrentValue = addedOn.OriginalValue;
			}

			return Track<TUpdate>(entity);
		}

		/// <summary>
		/// Update the specified model from the in-memory collection, so that it can be saved to the datasource on commit.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="model"></param>
		/// <returns></returns>
		public virtual TUpdate Update(TUpdate model)
		{
			var entity = this.Source.Map(model, this.Find<EntityT>(model));
			return this.Update(entity);
		}

		/// <summary>
		/// Update the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="models"></param>
		/// <returns></returns>
		public virtual IEnumerable<TUpdate> UpdateRange(IEnumerable<TUpdate> models)
		{
			this.VerifyPrincipal(true);
			var entities = models.Select(m => this.Source.Map<EntityT>(m));
			var result = new List<TUpdate>();
			entities.ForEach(e => result.Add(this.Update(e)));
			return result;
		}

		/// <summary>
		/// Update the specified models from the in-memory collection, so that it can be saved to the datasource on commit.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="models"></param>
		/// <returns></returns>
		public virtual IEnumerable<TUpdate> UpdateRange(params TUpdate[] models)
		{
			return this.UpdateRange(models.AsEnumerable());
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
