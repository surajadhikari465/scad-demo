Imports log4net
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Replenishment.Common.Writers.Constants
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic


Public Class AvgCostAdjDO

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)



    Private factory As DataFactory

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        CreateDatabaseConnection()
    End Sub

    ''' <summary>
    ''' CreateDatabaseConnection from the data factory
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateDatabaseConnection()
        If factory Is Nothing Then
            factory = New DataFactory(DataFactory.ItemCatalog)
        End If
    End Sub


    Public Shared Function GetSubteamList() As ArrayList

        logger.Debug("GetsubTeamList Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim subTeam As SubTeamBO
        Dim subTeamList As New ArrayList
        Dim results As SqlDataReader = Nothing
        Dim paramList As New ArrayList

        Try

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("GetSubteams")
            Dim subteamIndex As Integer
            subteamIndex = 0
            subTeam = New SubTeamBO()
            subTeam.SubTeamNo = subteamIndex
            subTeam.SubTeamName = "ALL"
            subTeamList.Add(subTeam)

            While results.Read
                subTeam = New SubTeamBO()
                subTeam.SubTeamNo = CType(results.GetInt32(results.GetOrdinal("SubTeam_No")), Integer)
                subTeam.SubTeamName = results.GetString(results.GetOrdinal("SubTeam_Name"))
                subTeamList.Add(subTeam)
            End While
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try

        logger.Debug("GetsubTeamList Exit")

        Return subTeamList
    End Function

    Public Shared Function SaveAdjustment(ByVal AvgCostItem As AvgCostAdjBO.AvgCostAdjItem) As Boolean

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        ' Execute the stored procedure 
        logger.Debug("SaveAdjustment entry")

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = AvgCostItem.ItemKey
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = AvgCostItem.BusinessUnit
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "SubTeam_No"
        currentParam.Value = AvgCostItem.SubTeam
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "AvgCost"
        currentParam.Value = AvgCostItem.AvgCost
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Reason"
        currentParam.Value = AvgCostItem.Reason
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Comments"
        currentParam.Value = AvgCostItem.Comment
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "User_ID"
        currentParam.Value = giUserID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        factory.ExecuteStoredProcedure("dbo.InsertAvgCostAdjustment", paramList)

        Return True

        logger.Debug("SaveAdjustment Exit")

    End Function

    Public Shared Function AddAdjustmentReason(ByVal Description As String, ByVal Active As Boolean) As Boolean

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        ' Execute the stored procedure 
        logger.Debug("AddAdjustmentReason entry")

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Description"
        currentParam.Value = Description
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Active"
        currentParam.Value = Active
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        factory.ExecuteStoredProcedure("dbo.InsertAvgCostAdjustmentReason", paramList)

        Return True

        logger.Debug("AddAdjustmentReason Exit")

    End Function

    Public Shared Function SetReasonStatus(ByVal ID As Integer, ByVal Active As Boolean) As Boolean

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        ' Execute the stored procedure 
        logger.Debug("SetReasonStatus entry")

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "ID"
        currentParam.Value = ID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Active"
        currentParam.Value = CInt(Active)
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        factory.ExecuteStoredProcedure("dbo.UpdateAvgCostAdjReasonStatus", paramList)

        Return True

        logger.Debug("SetReasonStatus Exit")

    End Function

    Public Shared Function IsAdjustmentReasonActive(ByVal ID As Integer) As Boolean

        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        Return CBool(factory.ExecuteScalar("SELECT dbo.fn_IsAvgCostAdjReasonActive( " & ID & ")"))

    End Function

    Public Shared Function GetAdjustmentHistory(ByVal AvgCostItem As AvgCostAdjBO.AvgCostAdjItem) As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' Execute the stored procedure 
            logger.Debug("GetAdjustmentHistory entry")

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = AvgCostItem.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            'currentParam.Value = AvgCostItem.BusinessUnit
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = AvgCostItem.SubTeam
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MaxRows"
            If ConfigurationServices.AppSettings("MaxAvgCostHistoryRows").ToString.Length = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = CInt(ConfigurationServices.AppSettings("MaxAvgCostHistoryRows"))
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = New DataSet

            results = factory.GetStoredProcedureDataSet("dbo.GetAvgCostHistory", paramList)

        Finally
            If results IsNot Nothing Then
                results.Dispose()
            End If
        End Try

        logger.Debug("GetAdjustmentHistory Exit")

        Return results.Tables(0)

    End Function

    Public Shared Function GetAvgCostForStores(ByVal AvgCostItem As AvgCostAdjBO.AvgCostAdjItem) As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' Execute the stored procedure 
            logger.Debug("GetAvgCostForStores entry")

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = AvgCostItem.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            If AvgCostItem.BusinessUnit = 0 Or AvgCostItem.AllStores Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = AvgCostItem.BusinessUnit
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AllWFM"
            If AvgCostItem.AllWFMStores = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = AvgCostItem.AllWFMStores
            End If
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AllHFM"
            If AvgCostItem.AllHFMStores = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = AvgCostItem.AllHFMStores
            End If
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Zone"
            If AvgCostItem.Zone = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = AvgCostItem.Zone
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "State"
            If AvgCostItem.State Is Nothing Then
                currentParam.Value = DBNull.Value
            Else

                currentParam.Value = AvgCostItem.State
            End If
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = AvgCostItem.SubTeam
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Current"
            currentParam.Value = AvgCostItem.Current
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Start_Date"
            currentParam.Value = AvgCostItem.StartDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "End_Date"
            currentParam.Value = AvgCostItem.EndDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MaxRows"
            If ConfigurationServices.AppSettings("MaxAvgCostHistoryRows").ToString.Length = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = CInt(ConfigurationServices.AppSettings("MaxAvgCostHistoryRows"))
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            results = New DataSet

            results = factory.GetStoredProcedureDataSet("dbo.GetAvgCostForStores", paramList)

        Finally
            If results IsNot Nothing Then
                results.Dispose()
            End If
        End Try

        logger.Debug("GetAvgCostForStores Exit")

        Return results.Tables(0)

    End Function


    Public Shared Function GetAdjustmentReasons(ByVal ApplyFilter As Boolean, ByVal Active As Boolean) As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' Execute the stored procedure 
            logger.Debug("GetAdjustmentReasons entry")

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Filter"
            currentParam.Value = ApplyFilter
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Active"
            currentParam.Value = Active
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = New DataSet

            results = factory.GetStoredProcedureDataSet("dbo.GetAvgCostAdjustmentReasons", paramList)

        Finally
            If results IsNot Nothing Then
                results.Dispose()
            End If
        End Try

        logger.Debug("GetAdjustmentReasons Exit")

        Return results.Tables(0)

    End Function

End Class
