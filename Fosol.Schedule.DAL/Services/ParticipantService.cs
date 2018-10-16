using Fosol.Core.Exceptions;
using Fosol.Schedule.DAL.Interfaces;
using System;
using System.Linq;

namespace Fosol.Schedule.DAL.Services
{
    /// <summary>
    /// ParticipantService sealed class, provides a way to manage participants in the datasource.
    /// </summary>
    public sealed class ParticipantService : UpdatableService<Entities.Participant, Models.Participant>, IParticipantService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParticipantService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal ParticipantService(IDataSource source) : base(source)
        {
            //Authenticated();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the participant for the specified 'key'.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Models.Participant Get(Guid key)
        {
            var participant = this.Context.Participants.FirstOrDefault(p => p.Key == key) ?? throw new NoContentException(typeof(Models.Participant));

            return this.Source.Mapper.Map<Models.Participant>(participant);
        }

        /// <summary>
        /// Get the participant for the specified 'id'.
        /// Validates whether the current user is authorized to view the participant.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Participant Get(int id)
        {
            var participant = this.Find(id);

            return this.Source.Mapper.Map<Models.Participant>(participant);
        }
        #endregion
    }
}
