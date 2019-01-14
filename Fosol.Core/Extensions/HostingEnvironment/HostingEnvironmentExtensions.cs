using System;
using Microsoft.AspNetCore.Hosting;

/// <summary>
/// HostingEnvironmentExtensions static class, provides extension methods for IHostingEnvironment.
/// </summary>
public static class HostingEnvironmentExtensions
{
  /// <summary>
  /// Determine if the hosting environment name is "Local".
  /// </summary>
  /// <param name="hostingEnvironment"></param>
  /// <returns></returns>
  public static bool IsLocal(this IHostingEnvironment hostingEnvironment)
  {
    return String.Compare(hostingEnvironment.EnvironmentName, "Local", true) == 0;
  }
}