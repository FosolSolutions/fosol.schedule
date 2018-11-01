using System;

namespace Fosol.Overseer.Requesting
{
    internal abstract class RequestorBase
    {
        #region Methods
        protected static TRequestor GetRequestor<TRequestor>(ServiceFactory factory)
        {
            TRequestor requestor;

            try
            {
                requestor = factory.GetInstance<TRequestor>();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error constructing requestor for request of type {typeof(TRequestor)}. Register your requestors with the container.", e); // TODO: errors from resource file.
            }

            if (requestor == null)
            {
                throw new InvalidOperationException($"Requestor was not found for request of type {typeof(TRequestor)}. Register your requestors with the container.");
            }

            return requestor;
        }

        protected static object GetRequestor(ServiceFactory factory, Type requestorType)
        {
            object requestor;

            try
            {
                requestor = factory.GetInstance(requestorType);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Error constructing requestor for request of type {requestorType}. Register your requestors with the container.", e); // TODO: errors from resource file.
            }

            if (requestor == null)
            {
                throw new InvalidOperationException($"Requestor was not found for request of type {requestorType}. Register your requestors with the container.");
            }

            return requestor;
        }
        #endregion
    }
}
