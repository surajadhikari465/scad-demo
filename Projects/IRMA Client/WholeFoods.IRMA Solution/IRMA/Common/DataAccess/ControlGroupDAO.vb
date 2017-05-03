Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports log4net

Namespace WholeFoods.IRMA.Common.DataAccess
    Public Class ControlGroupDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ''' <summary>
        ''' Gets list of facilities by VendorName and Vendor_ID
        ''' </summary>
        ''' <returns>ArrayList of StoreListBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetControlGroupIds() As ArrayList

            logger.Debug("GetControlGroupIds Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim controlGroup As ControlGroupBO
            Dim ControlGroupList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetAllControlGroups")

                ' Adding All as an item to the list.
                controlGroup = New ControlGroupBO()
                controlGroup.ControlGroupID = 0
                controlGroup.ControlGroupName = "ALL"
                ControlGroupList.Add(controlGroup)

                While results.Read
                    controlGroup = New ControlGroupBO()
                    controlGroup.ControlGroupID = results.GetInt32(results.GetOrdinal("OrderInvoice_ControlGroup_ID"))
                    controlGroup.ControlGroupName = results.GetInt32(results.GetOrdinal("OrderInvoice_ControlGroup_ID")).ToString
                    ControlGroupList.Add(controlGroup)
                End While

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetControlGroupIds Exit")

            Return ControlGroupList
        End Function

        Public Shared Function GetClosedControlGroupIds() As ArrayList

            logger.Debug("GetClosedControlGroupIds Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim controlGroup As ControlGroupBO
            Dim ControlGroupList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetAllClosedControlGroups")
                ' Adding All as an item to the list.
                controlGroup = New ControlGroupBO()
                controlGroup.ControlGroupID = 0
                controlGroup.ControlGroupName = ""
                ControlGroupList.Add(controlGroup)

                While results.Read
                    controlGroup = New ControlGroupBO()
                    controlGroup.ControlGroupID = results.GetInt32(results.GetOrdinal("OrderInvoice_ControlGroup_ID"))
                    controlGroup.ControlGroupName = results.GetInt32(results.GetOrdinal("OrderInvoice_ControlGroup_ID")).ToString
                    ControlGroupList.Add(controlGroup)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetClosedControlGroupIds Exit")

            Return ControlGroupList
        End Function

    End Class
End Namespace
