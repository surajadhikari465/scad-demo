Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Public Class DAOValidatedPOs

    Private con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString()

    Public Function GetPOsReadyToPushByUser(ByVal UserID As Integer) As DataSet

        Dim factory As New DataFactory(con, True)
        Dim results As New DataSet
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataSet("GetPOsReadyToPushByUser", paramList)

        Catch ex As Exception
            Throw ex
        End Try

        Return results
    End Function

    Public Sub DeletePOs(ByVal UserID As Integer, ByVal POHeaderID As Integer)

        Dim factory As New DataFactory(con, True)
        Dim results As ArrayList
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "POHeaderID"
            currentParam.Value = POHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.ExecuteStoredProcedure("DeletePOs", paramList, False)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InsertPushQueue(ByVal POHeaderID As Integer, ByVal UserID As Integer)

        Dim factory As New DataFactory(con, True)
        Dim results As ArrayList
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "POHeaderID"
            currentParam.Value = POHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.ExecuteStoredProcedure("InsertPushQueue", paramList, False)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function GetVendors(ByVal UserID As Integer, ByVal DBString As String) As DataSet
        Dim factory As New DataFactory(con, True)
        Dim results As New DataSet
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            If DBString = "" Then
                currentParam = New DBParam
                currentParam.Name = "DBString"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
            Else
                currentParam = New DBParam
                currentParam.Name = "DBString"
                currentParam.Value = DBString
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
            End If

            results = factory.GetStoredProcedureDataSet("GetVendorNamesbyRegion", paramList)

        Catch ex As Exception
        End Try

        Return results
    End Function
    Public Function GetSubTeams(ByVal UserID As Integer, ByVal DBString As String) As DataSet
        Dim factory As New DataFactory(con, True)
        Dim results As New DataSet
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            If DBString = "" Then
                currentParam = New DBParam
                currentParam.Name = "DBString"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
            Else
                currentParam = New DBParam
                currentParam.Name = "DBString"
                currentParam.Value = DBString
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
            End If

            results = factory.GetStoredProcedureDataSet("GetSubTeamNamesbyRegion", paramList)

        Catch ex As Exception
        End Try

        Return results

    End Function
    Public Function GetStoreNames(ByVal UserID As Integer, ByVal DBString As String) As DataSet
        Dim factory As New DataFactory(con, True)
        Dim results As New DataSet
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

          

            If DBString = "" Then
                currentParam = New DBParam
                currentParam.Name = "DBString"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
            Else
                currentParam = New DBParam
                currentParam.Name = "DBString"
                currentParam.Value = DBString
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
            End If

            results = factory.GetStoredProcedureDataSet("GetStoreNamesbyRegion", paramList)

        Catch ex As Exception
        End Try

        Return results
    End Function
    Public Function GetPOsPushedToIRMA(ByVal UserID As Integer, ByVal Top As Integer,
                                       ByVal StartDate As Date, ByVal EndDate As Date,
                                       ByVal Store As String, ByVal Vendor As Integer,
                                       ByVal Subteam As Integer,
                                       ByVal POType As Integer) As DataSet

        Dim factory As New DataFactory(con, True)
        Dim results As New DataSet
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Top"
            currentParam.Value = Top
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            If Not StartDate = "01/01/1900" Then
                currentParam = New DBParam
                currentParam.Name = "@StartDate"
                currentParam.Value = StartDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)
            Else
                currentParam = New DBParam
                currentParam.Name = "@StartDate"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)
            End If

            If Not EndDate = "01/01/1900" Then
                currentParam = New DBParam
                currentParam.Name = "EndDate"
                currentParam.Value = EndDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)
            Else
                currentParam = New DBParam
                currentParam.Name = "EndDate"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)
            End If

            If Not Store = "0" Then
                currentParam = New DBParam
                currentParam.Name = "Store"
                currentParam.Value = Store
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
            Else
                currentParam = New DBParam
                currentParam.Name = "Store"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
            End If

            If Not Vendor = 0 Then
                currentParam = New DBParam
                currentParam.Name = "Vendor"
                currentParam.Value = Vendor
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
            Else
                currentParam = New DBParam
                currentParam.Name = "Vendor"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
            End If

            If Not Subteam = 0 Then
                currentParam = New DBParam
                currentParam.Name = "Subteam"
                currentParam.Value = Subteam
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
            Else
                currentParam = New DBParam
                currentParam.Name = "Subteam"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
            End If

            currentParam = New DBParam
            currentParam.Name = "POType"
            currentParam.Value = POType
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataSet("GetPOsPushedToIRMA", paramList)

        Catch ex As Exception
            Throw ex
        End Try

        Return results
    End Function

    Public Sub ConfirmPOInIRMA(ByVal RegionID As Integer, ByVal PONumber As Integer, ByVal UploadSessionID As Integer)

        Dim factory As New DataFactory(con, True)
        Dim results As New ArrayList
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "RegionID"
            currentParam.Value = RegionID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PONumber"
            currentParam.Value = PONumber
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UploadSessionID"
            currentParam.Value = UploadSessionID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.ExecuteStoredProcedure("ConfirmPOInIRMA", paramList)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub PoNumberUpdate(ByVal PoNumber As Integer)
        Dim factory As New DataFactory(con, True)
        Dim results As New ArrayList
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "PoNumber"
            currentParam.Value = PoNumber
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.ExecuteStoredProcedure("UpdatePOnumberforNotUsed", paramList)

        Catch ex As Exception

        End Try
    End Sub

    Public Function GetRegionalUsers(ByVal UserID As Integer) As DataTable
        Dim factory As New DataFactory(con, True)
        Dim results As New DataTable
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.GetStoredProcedureDataTable("GetRegionalUsers", paramList)

        Catch ex As Exception
            Throw ex
        End Try

        Return results
    End Function
End Class
