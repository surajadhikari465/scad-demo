Imports log4net
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic


Public Class ShrinkCorrectionsDAO

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Public Shared Function GetShrinkCorrections(ByVal StoreNo As Int32, ByVal SubTeamNo As Int32, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal WasteType As String) As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' Execute the stored procedure 
            logger.Debug("GetShrinkCorrections entry")

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StartDate"
            currentParam.Value = StartDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EndDate"
            currentParam.Value = EndDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "WasteType"
            currentParam.Value = WasteType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            results = New DataSet

            results = factory.GetStoredProcedureDataSet("dbo.GetShrinkCorrections", paramList)

        Finally
            If results IsNot Nothing Then
                results.Dispose()
            End If
        End Try

        logger.Debug("GetShrinkCorrections Exit")

        Return results.Tables(0)

    End Function

    Public Shared Function GetWasteCorrections(ByVal StoreNo As Int32, ByVal SubTeamNo As Int32, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal WasteType As String) As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' Execute the stored procedure 
            logger.Debug("GetWasteCorrections entry")

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StartDate"
            currentParam.Value = StartDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EndDate"
            currentParam.Value = EndDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "WasteType"
            currentParam.Value = WasteType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            results = New DataSet

            results = factory.GetStoredProcedureDataSet("dbo.GetWasteCorrections", paramList)

        Finally
            If results IsNot Nothing Then
                results.Dispose()
            End If
        End Try

        logger.Debug("GetWasteCorrections Exit")

        Return results.Tables(0)

    End Function
    Public Shared Function GetIdentifierRecords(ByVal StoreNo As Int32, ByVal SubTeamNo As Int32, ByVal ParentTable As DataTable) As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim dt As DataTable

        Dim dtsum As DataTable = Nothing

        Try
            Dim dRow As DataRow
            For Each dRow In ParentTable.Rows

                paramList.Clear()

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SubTeam_No"
                currentParam.Value = SubTeamNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DateStamp"
                currentParam.Value = dRow("OriginalDateStamp")
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "WasteType"
                currentParam.Value = dRow("wType")
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Value = dRow("Identifier")
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                dt = factory.GetStoredProcedureDataTable("GetWasteCorrectionRecords", paramList)

                If (dtsum Is Nothing) Then
                    dtsum = dt
                Else
                    Dim row As DataRow
                    For Each row In dt.Rows
                        dtsum.ImportRow(row)
                    Next row
                End If
            Next dRow

            Dim Keys(2) As DataColumn

            Keys(0) = dtsum.Columns("Identifier")
            Keys(1) = dtsum.Columns("DateStamp")
            Keys(2) = dtsum.Columns("WasteType")

        Catch ex As Exception

            MsgBox(ex.Message, MsgBoxStyle.Critical, "WasteCorrectionsDAO:GetIdentifierRecords")
            logger.Error(ex.Message)
            dtsum = Nothing

        End Try

        Return dtsum

        logger.Debug("LockItem Exit")

    End Function

    Public Shared Function GetShrinkCorrectionsDetails(ByVal StoreNo As Int32, ByVal SubTeamNo As Int32, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal WasteType As String) As DataSet

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' Execute the stored procedure 
            logger.Debug("GetShrinkCorrectionsDetails entry")

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StartDate"
            currentParam.Value = StartDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EndDate"
            currentParam.Value = EndDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "WasteType"
            currentParam.Value = WasteType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            results = New DataSet

            results = factory.GetStoredProcedureDataSet("dbo.GetShrinkCorrections", paramList)

            If results.Tables(0).Rows.Count <= 0 Then
                Return results
                Exit Function
            End If

            results.Tables.Add(GetIdentifierRecords(StoreNo, SubTeamNo, results.Tables(0)))

            Dim parentColumns(2) As DataColumn
            Dim childColumns(2) As DataColumn

            parentColumns(0) = results.Tables(0).Columns("Identifier")
            parentColumns(1) = results.Tables(0).Columns("OriginalDateStamp")
            parentColumns(2) = results.Tables(0).Columns("wType")

            childColumns(0) = results.Tables(1).Columns("Identifier")
            childColumns(1) = results.Tables(1).Columns("DateStamp")
            childColumns(2) = results.Tables(1).Columns("WasteType")

            '  23486 :: 19931 "Shrink Corrections Bug - Duplicate parent rows on the grid"
            '  Pass False for the createConstraints parameter: this will prevent the DataTable
            '  from trying to enforce a pk which could cause an error in case of duplicate data
            Dim rel As New DataRelation("WasteRecords", parentColumns, childColumns, False)

            results.Relations.Add(rel)

        Finally
            If results IsNot Nothing Then
                results.Dispose()
            End If
        End Try

        logger.Debug("GetShrinkCorrectionsDetails Exit")

        Return results

    End Function
    Public Shared Function GetSubTeams() As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing

        Try
            ' Execute the stored procedure 
            logger.Debug("GetSubTeams entry")

            results = New DataSet

            results = factory.GetStoredProcedureDataSet("dbo.GetSubTeams")

        Finally
            If results IsNot Nothing Then
                results.Dispose()
            End If
        End Try

        results.Tables(0).Columns.Remove("SubTeam_Unrestricted")

        logger.Debug("GetSubTeams Exit")

        Return results.Tables(0)

    End Function

    Public Shared Function GetShrinkTypes() As DataTable

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As DataSet = Nothing

        Try
            ' Execute the stored procedure 
            logger.Debug("GetShrinkTypes entry")

            results = New DataSet

            results = factory.GetStoredProcedureDataSet("dbo.GetWasteTypes")

        Finally
            If results IsNot Nothing Then
                results.Dispose()
            End If
        End Try

        logger.Debug("GetShrinkTypes Exit")

        Return results.Tables(0)

    End Function

    Public Shared Function GetItemKey(ByVal Identifier As String) As Integer

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim reader As SqlDataReader = Nothing

        Dim ItemKey As Integer = -1

        Try
            ' Execute the stored procedure 
            logger.Debug("GetItemKey entry")


            paramList.Add(New DBParam("Item_Key", DBParamType.Int, DBNull.Value))
            paramList.Add(New DBParam("Identifier", DBParamType.String, Identifier))

            reader = factory.GetStoredProcedureDataReader("GetItem", paramList)

            If (reader.Read) Then
                ItemKey = CInt(reader.GetInt32(reader.GetOrdinal("Item_Key")))
            End If

        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try

        logger.Debug("GetItemKey Exit")

        Return ItemKey

    End Function

    Public Shared Function GetUserID(ByVal UserName As String) As Integer

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim reader As SqlDataReader = Nothing

        Dim userid As Integer = -1

        Try
            ' Execute the stored procedure 
            logger.Debug("GetUserID entry")


            paramList.Add(New DBParam("UserName", DBParamType.String, UserName))

            reader = factory.GetStoredProcedureDataReader("GetUserID", paramList)

            If (reader.Read) Then
                userid = reader.GetInt32(reader.GetOrdinal("User_ID"))
            End If

        Catch ex As Exception
            Throw ex
        Finally
            If reader IsNot Nothing Then
                reader.Close()
            End If
        End Try

        logger.Debug("GetUserID Exit")

        Return userid

    End Function

    Public Shared Function GetInventoryAdjustmentCodeID(ByVal Abbreviation As String) As Integer

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim outputParamList As New ArrayList

        Dim codeID As Integer = -1

        Try
            ' Execute the stored procedure 
            logger.Debug("GetInventoryAdjustmentCodeID entry")

            paramList.Add(New DBParam("Abbreviation", DBParamType.String, Abbreviation))
            paramList.Add(New DBParam("InventoryAdjustmentCode_ID", DBParamType.Int))

            outputParamList = factory.ExecuteStoredProcedure("GetInventoryAdjustmentIDFromCode", paramList)

            codeID = CInt(outputParamList(0))

        Catch ex As Exception
            Throw ex
        End Try

        logger.Debug("GetInventoryAdjustmentCodeID Exit")

        Return codeID

    End Function

    Public Shared Function GetFiscalCalendarInfo(ByVal dateValue As Date) As DataTable

        logger.Debug("GetFiscalCalendarInfo entry: Type=" + dateValue.ToString)

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim results As DataTable = Nothing

        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "@myDate"
        currentParam.Value = dateValue
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)

        results = factory.GetStoredProcedureDataTable("dbo.GetFiscalCalendarInfo", paramList)

        Return results

        logger.Debug("GetFiscalCalendarInfo exit")

    End Function

End Class
