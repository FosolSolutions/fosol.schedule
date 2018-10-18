using Fosol.Core.Exceptions;
using Fosol.Core.Extensions.Principals;
using Fosol.Schedule.DAL.Interfaces;

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
        private readonly IDataSource _source;
        #endregion

        #region Properties
        /// <summary>
        /// get - The datasource.
        /// </summary>
        internal DataSource Source { get { return _source as DataSource; } }

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
                bool.TryParse(this.Source.Principal.GetParticipant()?.Value, out bool participant);
                return participant;
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
        /// Get the current user or participant's id.
        /// </summary>
        /// <returns></returns>
        protected int GetPrincipalId()
        {
            int.TryParse(this.Source.Principal.GetNameIdentifier()?.Value, out int id);
            return id;
        }

        /// <summary>
        /// Get the current user's id.
        /// Returns null if a participant is currently signed in.
        /// </summary>
        /// <returns></returns>
        protected int? GetUserId()
        {
            if (this.IsPrincipalAParticipant)
                return null;

            return this.GetPrincipalId();
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
        /// Get the current principal user.
        /// </summary>
        /// <returns></returns>
        protected Entities.User GetUser()
        {
            var id = this.GetUserId();
            if (id == null) return null;
            return this.Context.Users.Find(id);
        }

        /// <summary>
        /// Get the current principal participant.
        /// </summary>
        /// <exception cref="NoContentException">If the user does not exist.</exception>
        /// <returns></returns>
        protected Entities.Participant GetParticipant()
        {
            if (!this.IsPrincipalAParticipant) return null;
            var id = this.GetPrincipalId();
            return this.Context.Participants.Find(id) ?? throw new NoContentException(typeof(Entities.Participant));
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
        /// Creates a new instance of a <typeparamref name="EntityT"/> by mapping the specified <typeparamref name="ModelT"/>.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected EntityT Map(ModelT model)
        {
            return this.Source.Mapper.Map<EntityT>(model);
        }

        /// <summary>
        /// Creates a new instance of a <typeparamref name="ModelT"/> by mapping the specified <typeparamref name="EntityT"/>.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected ModelT Map(EntityT entity)
        {
            return this.Source.Mapper.Map<ModelT>(entity);
        }

        /// <summary>
        /// Find the entity for the specified model in the datasource.
        /// </summary>
        /// <exception cref="NoContentException">If the entity could not be found in the datasource.</exception>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual EntityT Find(ModelT model)
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
        protected virtual EntityT Find(params object[] keyValues)
        {
            var entity = this.Context.Set<EntityT>().Find(keyValues);

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
        public virtual ModelT Get(params object[] keyValues)
        {
            return this.Map(Find(keyValues));
        }
        #endregion
    }
}
