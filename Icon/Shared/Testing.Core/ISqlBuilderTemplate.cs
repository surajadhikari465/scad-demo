namespace Testing.Core
{
    using System.Collections.Generic;

    public interface ISqlBuilderTemplate<T> where T : class
    {
        string TableName { get; }
        string IdentityColumn { get; }
        Dictionary<string, string> PropertyToColumnMapping { get; }
    }
}
