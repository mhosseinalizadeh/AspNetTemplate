using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.ApplicationService
{
    public class ServiceResult
    {
        public ServiceResultStatus Status;
        public List<string> Messages = new List<string>();
        public dynamic Result;
        public string Mode
        {
            get
            {
                if (Status == ServiceResultStatus.Success)
                    return "success";

                if (Status == ServiceResultStatus.Exception)
                    return "danger";

                if (Status == ServiceResultStatus.Warning)
                    return "warning";

                return "info";
            }
        }

        public ServiceResult(ServiceResultStatus status, dynamic result, List<string> messages)
        {
            Status = status;
            Result = result;
            Messages = messages;
        }

        public ServiceResult(ServiceResultStatus status, dynamic result, string message)
        {
            Status = status;
            Result = result;
            Messages.Add(message);
        }
        public ServiceResult(ServiceResultStatus status, dynamic result)
        {
            Status = status;
            Result = result;
        }

        public ServiceResult(ServiceResultStatus status)
        {
            Status = status;
            Result = null;
        }
    }

    public enum ServiceResultStatus
    {
        Success = 1,
        Exception = 2,
        Warning = 3,
        Info = 4
    }
}
