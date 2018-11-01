﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Overseer.Requesting
{
    internal abstract class RequestorWrapper<TResponse> : RequestorBase
    {
        public abstract Task<TResponse> Execute(IRequest<TResponse> request, CancellationToken cancellationToken, ServiceFactory serviceFactory);
    }
}
