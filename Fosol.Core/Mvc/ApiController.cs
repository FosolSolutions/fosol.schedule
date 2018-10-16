using Fosol.Core.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Core.Mvc
{
    [JsonExceptionFilter]
    public abstract class ApiController : Controller
    {
    }
}
