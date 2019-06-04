using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Dashboard.Mvc.Models
{
    public class ApplicationDatabaseConfiguration
    {
        public ApplicationDatabaseConfiguration()
        {
            Connections = new List<DatabaseDefinition>();
        }

        public string Summary
        {
            get
            {
                if (NonLoggingConnections.Count == 0)
                {
                    return "None";
                }
                else if (NonLoggingConnections.Count == 1)
                {
                    if (NonLoggingConnections[0].Category == DatabaseCategoryEnum.Encrypted)
                    {
                        return "{Encrypted}";
                    }
                    return $"{NonLoggingConnections[0].Category}-{NonLoggingConnections[0].Environment}";
                }
                else
                {
                    // more than 1 non-logging datbase connection for this app
                    List<string> connections = new List<string>();
                    if (NonLoggingConnections.Any(c => c.Category == DatabaseCategoryEnum.IRMA))
                    {
                        // since IRMA has multiple databases per environment, summarize instead of listing each one
                        // also remember there may mutltiple IRMA database connections spanning environments (such as hybrid Dev/Test)
                        List<EnvironmentEnum> distinctIrmaEnvironments = NonLoggingConnections
                            .Where(c => c.Category == DatabaseCategoryEnum.IRMA)
                            .Select(c => c.Environment)
                            .Distinct()
                            .ToList();
                        connections.Add($"{DatabaseCategoryEnum.IRMA}-{string.Join("/", distinctIrmaEnvironments)}");
                    }
                    foreach (var otherConnection in NonLoggingConnections.Where(c => c.Category != DatabaseCategoryEnum.IRMA))
                    {
                        connections.Add($"{otherConnection.Category}-{otherConnection.Environment}");
                    }
                    return string.Join(" & ", connections);
                }
            }
        }

        public string LoggingSummary
        {
            get
            {
                if (LoggingConnection == null)
                {
                    return "None";
                }
                else
                {
                    return $"{LoggingConnection.Category}-{LoggingConnection.Environment}";
                }
            }
        }

        public DatabaseDefinition LoggingConnection
        {
            get
            {
                return Connections.FirstOrDefault(c => c.IsUsedForLogging);
            }
        }

        public List<DatabaseDefinition> NonLoggingConnections
        {
            get
            {
                return Connections.Where(c => !c.IsUsedForLogging).ToList();
            }
        }

        public List<DatabaseDefinition> Connections { get; set; }

        public bool HasNonLoggingConnectionOfCategory(DatabaseCategoryEnum databaseCategory)
        {
            return Connections.Any(c => c.Category == databaseCategory && !c.IsUsedForLogging);
        }

        public DatabaseDefinition GetSingleNonLoggingConnectionOfCategory(DatabaseCategoryEnum databaseCategory)
        {
            if (Connections.Count(c => c.Category == databaseCategory && !c.IsUsedForLogging) > 0)
            {
                return Connections.First(c => c.Category == databaseCategory && !c.IsUsedForLogging);
            }
            return (DatabaseDefinition)null;
        }

        public List<DatabaseDefinition> GetAllNonLoggingConnectionsOfCategory(DatabaseCategoryEnum databaseCategory)
        {
            return Connections.Where(c => c.Category == databaseCategory && !c.IsUsedForLogging).ToList();
        }
    }
}