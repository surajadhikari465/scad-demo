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
            public const string HierarchyClassAssociatedToItemsOnDelete = "HierarchyClassAssociatedToItemsOnDelete";
            public const string DeleteHierarchyClassError = "DeleteHierarchyClassError";
            public const string AddOrUpdateHierarchyClassError = "AddOrUpdateHierarchyClassError";
            public const string UnableToParseHierarchyClass = "UnableToParseHierarchyClass";
            public const string UnableToArchiveVimMessage = "UnableToArchiveVimMessage";
            public const string UnableToSendHierarchyClassesToVim = "UnableToSendHierarchyClassesToVim";
            public const string UnableToGenerateNationalClassEvents = "UnableToGenerateNationalClassEvents";
        }
        public static class Descriptions
        {
            public const string UnableToArchiveMessage = "An unexpected exception occurred which caused a failure to archive message from Infor.";
            public const string HierarchyClassAssociatedToItemsOnDelete = "Hierarchy Class could not be deleted due to it being associated to one or more Items.";
            public const string DeleteHierarchyClassError = "An unexpected error occurred which caused a failure to delete this Hierarchy Class.";
            public const string AddOrUpdateHierarchyClassError = "An unexpected error occurred which caused a failure to add or update this Hierarchy Class.";
            public const string UnableToParseHierarchyClass = "Unable to parse hierarchy class from message.";
            public const string UnableToArchiveVimMessage = "Error occurred while trying to archive the message sent to VIM.";
            public const string UnableToSendHierarchyClassesToVim = "Error occurred while trying to send hierarchy classes from Infor to VIM.";
            public const string UnableToGenerateNationalClassEvents = "Error occurred while trying to generate events for National Classes.";
        }
    }
}
