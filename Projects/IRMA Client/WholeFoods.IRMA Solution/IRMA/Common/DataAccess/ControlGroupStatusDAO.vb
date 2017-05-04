Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports log4net

Namespace WholeFoods.IRMA.Common.DataAccess
    Public Class ControlGroupStatusDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Function GetControlGroupStatus() As ArrayList

            logger.Debug("GetControlGroupStatus Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim controlGroup As ControlGroupStatusBO
            Dim ControlGroupList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetAllControlGroupStatus")

                controlGroup = New ControlGroupStatusBO()
                controlGroup.ControlGroupStatusID = 0
                controlGroup.ControlGroupStatusName = "ALL"
                ControlGroupList.Add(controlGroup)


                While results.Read
                    controlGroup = New ControlGroupStatusBO()
                    controlGroup.ControlGroupStatusID = results.GetInt32(results.GetOrdinal("OrderInvoice_ControlGroupStatus_ID"))
                    controlGroup.ControlGroupStatusName = results.GetString(results.GetOrdinal("OrderInvoice_ControlGroupStatus_Desc"))
                    ControlGroupList.Add(controlGroup)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetControlGroupStatus Exit")

            Return ControlGroupList
        End Function
    End Class
End Namespace
