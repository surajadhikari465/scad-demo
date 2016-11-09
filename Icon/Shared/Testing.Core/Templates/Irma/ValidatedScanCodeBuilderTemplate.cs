using Irma.Framework;
using System;
using System.Collections.Generic;
using Testing.Core.Helpers;

namespace Testing.Core.Templates.Irma
{
    internal class ValidatedScanCodeBuilderTemplate : IObjectBuilderTemplate<ValidatedScanCode>, ISqlBuilderTemplate<ValidatedScanCode>
    {
        public string TableName
        {
            get
            {
                return typeof(ValidatedScanCode).Name;
            }
        }

        public string IdentityColumn
        {
            get
            {
                return PropertyHelper.GetPropertyName((ValidatedScanCode x) => x.Id);
            }
        }

        public Dictionary<string, string> PropertyToColumnMapping { get { return null; } }

        public ObjectBuilder<ValidatedScanCode> BuildDefaults()
        {
            return new ObjectBuilder<ValidatedScanCode>()
                .With(x => x.InsertDate, DateTime.Now);
        }
    }
}
