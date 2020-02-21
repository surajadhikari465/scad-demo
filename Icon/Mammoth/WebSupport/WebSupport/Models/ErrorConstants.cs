﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSupport.Models
{
    public static class ErrorConstants
    {
        public static class Codes
        {
            public const string NoPricesExist = "NoPricesExist";
            public const string NoJobProvided = "NoJobProvided";
            public const string ItemDoesNotExist = "ItemDoesNotExist";
            public const string UnexpectedError = "UnexpectedError";
            public const string SequenceIdOrPatchFamilyIdNotExist = "SequenceIdOrPatchFamilyIdNotExist";
        }

        public static class Details
        {
            public const string NoPricesFound = "Unable to send prices because no prices were found for the given business units.";
            public const string NoPricesExist = "Unable to send prices because no prices were found for the given scan codes and business units.";
            public const string NoJobProvided = "Unable to send a message to kick off the job because there was no valid job provided for the message.";
            public const string ItemDoesNotExist = "Please enter valid scan code.";
        }
    }
}