using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Constants
{
    public static class ApplicationErrors
    {
        public static class Codes
        {
            public const string UnableToArchiveMessage = "UnableToArchiveMessage";
            public const string DeleteHierarchyClassError = "DeleteHierarchyClassError";
            public const string AddOrUpdateHierarchyClassError = "AddOrUpdateHierarchyClassError";
            public const string UnableToParseHierarchyClass = "UnableToParseHierarchyClass";
            public const string UnableToArchiveVimMessage = "UnableToArchiveVimMessage";
            public const string UnableToSendHierarchyClassesToVim = "UnableToSendHierarchyClassesToVim";
            public const string UnableToGenerateNationalClassEvents = "UnableToGenerateNationalClassEvents";
            public const string GenerateHierarchyClassEventsError = "GenerateHierarchyClassEventsError";
            public const string GenerateHierarchyClassMessagesError = "GenerateHierarchyClassMessagesError";
            public const string UnexpectedError = "UnexpectedError";
        }

        public static class Descriptions
        {
            public const string UnableToArchiveMessage = "An unexpected exception occurred which caused a failure to archive message from Infor.";
            public const string DeleteHierarchyClassError = "An unexpected error occurred which caused a failure to delete this Hierarchy Class.";
            public const string AddOrUpdateHierarchyClassError = "An unexpected error occurred which caused a failure to add or update this Hierarchy Class.";
            public const string UnableToParseHierarchyClass = "Unable to parse hierarchy class from message.";
            public const string UnableToArchiveVimMessage = "Error occurred while trying to archive the message sent to VIM.";
            public const string UnableToSendHierarchyClassesToVim = "Error occurred while trying to send hierarchy classes from Infor to VIM.";
            public const string UnableToGenerateNationalClassEvents = "Error occurred while trying to generate events for National Classes.";
            public const string GenerateHierarchyClassEventsError = "An unexpected error occurred which caused events to not be generated to IRMA.";
            public const string GenerateHierarchyClassMessagesError = "An unexpected error occurred which caused messages to not be sent to the ESB and downstream systems.";
            public const string UnexpectedError = "An unexpected error occurred when consuming the message from Infor.";
        }
    }
}
