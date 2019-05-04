using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Fosol.Schedule.DAL.Interfaces
{
  public interface IUserService : IUpdatableService<Models.Create.User, Models.Read.User, Models.Update.User, Models.Delete.User>
  {
    /// <summary>
    /// Verify the user with the specified key or email exists.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    int Verify(string email);

    /// <summary>
    /// Get the calendar for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Models.Read.User Get(int id);

    /// <summary>
    /// Get the calendar for the specified 'key'.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Models.Read.User Get(Guid key);

    /// <summary>
    /// Get the claimed identity of the user for the specified 'userId'.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    IEnumerable<Claim> GetClaims(int userId);
  }
}