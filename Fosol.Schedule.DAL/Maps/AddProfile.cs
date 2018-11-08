using AutoMapper;
using Fosol.Core.Extensions.Principals;
using Fosol.Core.Extensions.Strings;
using Fosol.Schedule.DAL.Interfaces;
using System;
using System.Linq;

namespace Fosol.Schedule.DAL.Maps
{
    /// <summary>
    /// AddProfile class, provides a way to map entities and models for add operations.
    /// </summary>
    public class AddProfile : BaseProfile, IProfileAddMap
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of an AddProfile object, and initializes it with the specified arguments.
        /// </summary>
        public AddProfile()
        {
        }
        #endregion
    }
}
