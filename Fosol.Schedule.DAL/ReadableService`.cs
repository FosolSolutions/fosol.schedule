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
        protected int GetCurrentUserId()
        {
            int.TryParse(this.Source.Principal.GetNameIdentifier()?.Value, out int id);
            return id;
        }

        /// <summary>
        /// Get the true current user id when they are impersonating another user.
        /// </summary>
        /// <returns></returns>
        protected int GetImpersontatorId()
        {
            int.TryParse(this.Source.Principal.GetImpersonator()?.Value, out int id);
            return id;
        }

        /// <summary>
        /// Whether the current logged in user is a participant or not.
        /// </summary>
        /// <returns></returns>
        protected bool IsCurrentUserParticipant()
        {
            bool.TryParse(this.Source.Principal.GetParticipant()?.Value, out bool participant);
            return participant;
        }

        /// <summary>
        /// Checks if the current user is authenticated.
        /// If they are not it will throw a NotAuthenticatedException.
        /// </summary>
        /// <exception cref="NotAuthenticatedException">User is not authenticated.</exception>
        protected void Authenticated()
        {
            if (!this.Source.Principal.Identity.IsAuthenticated)
                throw new NotAuthenticatedException();
        }

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
        public ModelT Get(params object[] keyValues)
        {
            return _source.Mapper.Map<EntityT, ModelT>(Find(keyValues));
        }
        #endregion
    }
}
