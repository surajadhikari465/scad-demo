Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Imports log4net


Namespace WholeFoods.IRMA.ProcessMonitor.DataAccess


    Public Class ProcessMDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Function GetJobStatusList() As DataSet
            logger.Debug("GetJobStatusList Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            logger.Debug("GetJobStatusList Exit")
            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("GetJobStatusList")
        End Function
    End Class

End Namespace