using Icon.Esb.Schemas.Wfm.Contracts;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    public class BuildMessageResult
    {
        public bool Success { get; private set; }
        public items Contract { get; private set; }
        public List<string> Errors { get; private set; }

        public BuildMessageResult(bool success, items contract, List<string> errors)
        {
            this.Success = success;
            this.Contract = contract;
            this.Errors = errors;
        }
    }
}