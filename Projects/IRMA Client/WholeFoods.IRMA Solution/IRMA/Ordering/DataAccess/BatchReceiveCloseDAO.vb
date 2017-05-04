Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports System.ComponentModel
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility

Public Class BatchReceiveCloseDAO
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Shared Function BRC_GetFacilities() As DataSet

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing

        Try
            logger.Debug("BRC_GetFacilities Entry")
            results = factory.GetStoredProcedureDataSet("BRC_GetFacilities")

        Catch ex As Exception
            Throw ex
        End Try

        logger.Debug("BRC_GetFacilities Exit")
        Return results

    End Function

    Public Shared Function GetSubteams() As DataSet

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing

        Try
            logger.Debug("GetSubteams Entry")
            results = factory.GetStoredProcedureDataSet("GetSubteams")

        Catch ex As Exception
            Throw ex
        End Try

        logger.Debug("GetSubteams Exit")
        Return results

    End Function


    Public Function BRC_GetOrders(ByVal b As BatchReceiveCloseBO) As DataTable
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam = Nothing

        Try
            logger.Debug("BRC_GetOrders Entry")
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            currentParam.Value = b.BRC_Vendor_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Subteam_No"
            currentParam.Value = b.BRC_Subteam_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StartDate"
            currentParam.Value = b.BRC_StartDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EndDate"
            currentParam.Value = b.BRC_EndDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)
            ' Execute the stored procedure 

            Return factory.GetStoredProcedureDataTable("BRC_GetOrders", paramList)
            logger.Debug("BRC_GetOrders Exit")

        Catch ex As Exception
            Throw ex

        End Try
    End Function

    Public Sub BRC_ReceiveOrder(ByVal o As Integer, ByVal u As Integer)

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            logger.Debug("BRC_ReceiveOrder Entry")
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = o
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = u
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("BRC_ReceiveOrder", paramList)
            logger.Debug("BRC_ReceiveOrder Exit")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
