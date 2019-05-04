using Fosol.Core.Exceptions;
using Fosol.Core.Extensions.Principals;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fosol.Schedule.DAL.Services
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
		private readonly IDataSource _source;
		#endregion

		#region Properties
		/// <summary>
		/// get - The datasource.
		/// </summary>
		protected DataSource Source { get { return _source as DataSource; } }

		/// <summary>
		/// get - The DbContext used to communicate with the datasource.
		/// </summary>
		internal ScheduleContext Context { get { return this.Source.Context; } }

		/// <summary>
		/// get - Whether the user is currently authenticated.
		/// </summary>
		protected bool IsAuthenticated { get { return this.Source.Principal.Identity.IsAuthenticated; } }

		/// <summary>
		/// get - Whether the current logged in principal is a participant or not.
		/// </summary>
		/// <returns></returns>
		protected bool IsPrincipalAParticipant
		{
			get
			{
				var userId = this.GetUserId();
				var participantId = this.GetParticipantId();
				return userId == null && participantId != null;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of a ReadableService object, and initializes it with the specified properties.
		/// </summary>
		/// <param name="source"></param>
		internal ReadableService(IDataSource source)
		{
			_source = source;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Get the current user or participant's key.
		/// </summary>
		/// <returns></returns>
		protected string GetPrincipalId()
		{
			var key = this.Source.Principal.GetNameIdentifier()?.Value;
			return key;
		}

		/// <summary>
		/// Get the current user's id.
		/// Returns null if a participant is currently signed in.
		/// </summary>
		/// <returns></returns>
		protected int? GetUserId()
		{
			int.TryParse(this.Source.Principal.GetUser()?.Value, out int id);
			return id == 0 ? (int?)null : id;
		}

		/// <summary>
		/// Get the current participant's id.
		/// Returns null if user is not signed in as a participant.
		/// </summary>
		/// <returns></returns>
		protected int? GetParticipantId()
		{
			int.TryParse(this.Source.Principal.GetParticipant()?.Value, out int id);
			return id == 0 ? (int?)null : id;
		}

		/// <summary>
		/// Get the true current principal id when they are impersonating another user.
		/// </summary>
		/// <returns></returns>
		protected int GetImpersontatorId()
		{
			int.TryParse(this.Source.Principal.GetImpersonator()?.Value, out int id);
			return id;
		}

		/// <summary>
		/// Get the current user's active account id.
		/// </summary>
		/// <returns></returns>
		protected int GetAccountId()
		{
			int.TryParse(this.Source.Principal.GetAccount()?.Value, out int id);
			return id;
		}

		/// <summary>
		/// Get the current user's active calendar id.
		/// </summary>
		/// <returns></returns>
		protected int GetCalendarId()
		{
			int.TryParse(this.Source.Principal.GetCalendar()?.Value, out int id);
			return id;
		}

		/// <summary>
		/// Checks if the current user is authenticated and authorized.
		/// </summary>
		/// <exception cref="NotAuthenticatedException">User is not authenticated.</exception>
		/// <exception cref="NotAuthorizedException">User is not authorized.</exception>
		/// <param name="mustBeUser"></param>
		protected void VerifyPrincipal(bool mustBeUser = false)
		{
			if (!this.IsAuthenticated) throw new NotAuthenticatedException();
			if (mustBeUser && this.IsPrincipalAParticipant) throw new NotAuthorizedException();
		}

		/// <summary>
		/// Find the entity for the specified model in the datasource.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="model"></param>
		/// <returns></returns>
		protected virtual EntityT Find(ModelT model)
		{
			return Find<EntityT>(model);
		}

		/// <summary>
		/// Find the entity for the specified model in the datasource.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="model"></param>
		/// <returns></returns>
		protected virtual T Find<T>(ModelT model) where T : class
		{
			//var keys = ScheduleMapper.Map.GetMap<T>().GetPrimaryKeyValues(this.Source.UpdateMapper.Map<T>(model));
			var entity = this.Source.Map<T>(model);
			var keys = this.Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(p => p.PropertyInfo.GetValue(entity)).ToArray();
			return this.Find<T>(keys);
		}

		/// <summary>
		/// Find the entity in the datasource.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="keyValues"></param>
		/// <returns></returns>
		protected virtual T Find<T>(params object[] keyValues) where T : class
		{
			var entity = this.Context.Set<T>().Find(keyValues);

			if (entity == null)
				throw new NoContentException(typeof(ModelT));

			return entity;
		}

		/// <summary>
		/// Find the entity in the datasource.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="action"></param>
		/// <returns></returns>
		protected virtual T Find<T>(Func<DbSet<T>, T> action) where T : class
		{
			var entity = action?.Invoke(this.Context.Set<T>());

			if (entity == null)
				throw new NoContentException(typeof(ModelT));

			return entity;
		}

		/// <summary>
		/// Find the entity in the datasource.
		/// </summary>
		/// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
		/// <param name="action"></param>
		/// <returns></returns>
		protected virtual EntityT Find(Func<DbSet<EntityT>, EntityT> action)
		{
			var entity = action?.Invoke(this.Context.Set<EntityT>());

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
		public virtual ModelT Find(params object[] keyValues)
		{
			return this.Source.Map<ModelT>(this.Find<EntityT>(keyValues));
		}

		/// <summary>
		/// Get the model for the specified expression.
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		protected ModelT GetNoTracking(Expression<Func<EntityT, bool>> expression)
		{
			return this.Source.Map<ModelT>(this.Find(set => set.AsNoTracking().SingleOrDefault(expression)));
		}
		#endregion
	}
}
