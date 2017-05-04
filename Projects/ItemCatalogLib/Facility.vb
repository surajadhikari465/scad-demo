Imports log4net
Imports System.Reflection

Public Enum enuItemAdjustment
    Waste = 1
    CycleCount = 2
    ManualAdjustment = 8
End Enum
Public Class Facility
    Dim m_store_no As Integer
    Dim m_store_name As String
    Private Shared ReadOnly logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)
    Public Property Store_No() As Integer
        Get
            Return m_store_no
        End Get
        Set(ByVal Value As Integer)
            m_store_no = Value
        End Set
    End Property
    Public Property Store_Name() As String
        Get
            Return m_store_name
        End Get
        Set(ByVal Value As String)
            m_store_name = Value
        End Set
    End Property
    Public Sub AddItemHistory(ByVal lItem_Key As Long, ByVal lSubTeam_No As Long, ByVal lAdjustment_ID As enuItemAdjustment, ByVal sAdjustmentReason As String, ByVal dtDateStamp As DateTime, ByVal dQuantity As Decimal, ByVal dWeight As Decimal, ByVal lCreatedBy As Long)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "InsertItemHistory"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Store_No"
            prm.Value = Me.Store_No
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Item_Key"
            prm.Value = lItem_Key
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.DateTime
            prm.ParameterName = "@DateStamp"
            prm.Value = dtDateStamp
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@Quantity"
            prm.Precision = 18
            prm.Scale = 4
            prm.Value = dQuantity
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@Weight"
            prm.Precision = 18
            prm.Scale = 4
            prm.Value = dWeight
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@ExtCost"
            prm.Precision = 10
            prm.Scale = 4
            prm.Value = System.DBNull.Value
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Adjustment_ID"
            prm.Value = lAdjustment_ID
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.String
            prm.ParameterName = "@AdjustmentReason"
            prm.Value = IIf(sAdjustmentReason.Trim = String.Empty, System.DBNull.Value, sAdjustmentReason.Trim)
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@CreatedBy"
            prm.Value = lCreatedBy
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@SubTeam_No"
            prm.Value = lSubTeam_No
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@OrderItem_ID"
            prm.Value = System.DBNull.Value
            cmd.Parameters.Add(prm)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub AddItemHistoryShrink(ByVal lItem_Key As Long, ByVal lSubTeam_No As Long, ByVal lAdjustment_ID As enuItemAdjustment, ByVal sAdjustmentReason As String, ByVal dtDateStamp As DateTime, ByVal dQuantity As Decimal, ByVal dWeight As Decimal, ByVal lCreatedBy As Long, Optional ByVal lWasteType As Decimal = 0)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "InsertItemHistoryShrink"

            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Store_No"
            prm.Value = Me.Store_No
            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Item_Key"
            prm.Value = lItem_Key
            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@Quantity"
            prm.Precision = 18
            prm.Scale = 4
            prm.Value = dQuantity
            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@Weight"
            prm.Precision = 18
            prm.Scale = 4
            prm.Value = dWeight
            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Adjustment_ID"
            prm.Value = lAdjustment_ID
            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.String
            prm.ParameterName = "@AdjustmentReason"
            prm.Value = IIf(sAdjustmentReason.Trim = String.Empty, System.DBNull.Value, sAdjustmentReason.Trim)
            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@CreatedBy"
            prm.Value = lCreatedBy
            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@SubTeam_No"
            prm.Value = lSubTeam_No
            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.String
            prm.ParameterName = "@InventoryAdjustmentCode"

            Select Case lWasteType
                Case 0 : prm.Value = "SP"
                Case 1 : prm.Value = "FB"
                Case 2 : prm.Value = "SM"
            End Select

            cmd.Parameters.Add(prm)

            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.String
            prm.ParameterName = "@Username"
            prm.Value = ""
            cmd.Parameters.Add(prm)

            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub AddItemHistory(ByVal sIdentifier As String, ByVal lAdjustment_ID As enuItemAdjustment, ByVal sAdjustmentReason As String, ByVal dtDateStamp As DateTime, ByVal dQuantity As Decimal, ByVal dWeight As Decimal, ByVal lCreatedBy As Long, Optional ByVal dPackSize As Decimal = 0)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "InsertItemHistory2"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Store_No"
            prm.Value = Me.Store_No
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.String
            prm.ParameterName = "@Identifier"
            prm.Value = sIdentifier
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.DateTime
            prm.ParameterName = "@DateStamp"
            prm.Value = dtDateStamp
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@Quantity"
            prm.Precision = 18
            prm.Scale = 4
            prm.Value = dQuantity
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@PackSize"
            prm.Precision = 9
            prm.Scale = 4
            prm.Value = dPackSize
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@Weight"
            prm.Precision = 18
            prm.Scale = 4
            prm.Value = dWeight
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Adjustment_ID"
            prm.Value = lAdjustment_ID
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.String
            prm.ParameterName = "@AdjustmentReason"
            prm.Value = IIf(sAdjustmentReason.Trim = String.Empty, System.DBNull.Value, sAdjustmentReason.Trim)
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@CreatedBy"
            prm.Value = lCreatedBy
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@OrderItem_ID"
            prm.Value = System.DBNull.Value
            cmd.Parameters.Add(prm)

            logger.Info("Execute InsertItemHistory2")
            For Each i As SqlClient.SqlParameter In cmd.Parameters
                logger.Info(vbTab & " (Parameter) " & i.ParameterName & ": " & i.Value & " (" & i.SqlDbType.ToString() & ")")
            Next

            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Sub AddItemHistory(ByVal sIdentifier As String, ByVal lInvAdjCode_Id As Integer, ByVal dtDateStamp As DateTime, ByVal dQuantity As Decimal, ByVal dWeight As Decimal, ByVal lCreatedBy As Long, Optional ByVal dPackSize As Decimal = 0)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "InsertItemHistory4"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Store_No"
            prm.Value = Me.Store_No
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.String
            prm.ParameterName = "@Identifier"
            prm.Value = sIdentifier
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.DateTime
            prm.ParameterName = "@DateStamp"
            prm.Value = dtDateStamp
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@Quantity"
            prm.Precision = 18
            prm.Scale = 4
            prm.Value = dQuantity
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@PackSize"
            prm.Precision = 9
            prm.Scale = 4
            prm.Value = dPackSize
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Decimal
            prm.ParameterName = "@Weight"
            prm.Precision = 18
            prm.Scale = 4
            prm.Value = dWeight
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@InventoryAdjustmentCode_Id"
            prm.Value = lInvAdjCode_Id
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@CreatedBy"
            prm.Value = lCreatedBy
            cmd.Parameters.Add(prm)

            With cmd.Parameters
                logger.InfoFormat("{0}InsertItemHistory4()  Store: {1} Identifier: {2} Qty: {3} PackSize: {4} Weight: {5} Adjustment: {6}", _
                vbTab, .Item("@Store_No").Value, .Item("@Identifier").Value, .Item("@Quantity").Value, .Item("@PackSize").Value, .Item("@Weight").Value, .Item("@InventoryAdjustmentCode_Id").Value)
            End With
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub


    Public Overrides Function ToString() As String
        Return Me.Store_Name
    End Function
    Public Sub New()
    End Sub
    Public Shared Function GetInventoryAdjustmentIDFromCode(ByVal sCode As String) As Integer
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetInventoryAdjustmentIDFromCode"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.String
            prm.ParameterName = "@Abbreviation"
            prm.Value = sCode
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Output
            prm.DbType = DbType.Int64
            prm.ParameterName = "@InventoryAdjustmentCode_ID"
            cmd.Parameters.Add(prm)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            Dim result As Integer = cmd.Parameters("@InventoryAdjustmentCode_ID").Value
            Return result
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function

    Public Shared Function GetFiscalPeriodBeginDate(ByVal dtDate As DateTime) As DateTime
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetBeginPeriodDate"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.DateTime
            prm.ParameterName = "@InDate"
            prm.Value = dtDate
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Output
            prm.DbType = DbType.DateTime
            prm.ParameterName = "@BP_Date"
            cmd.Parameters.Add(prm)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            Dim result As DateTime = cmd.Parameters("@BP_Date").Value
            Return result
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
    Public Shared Sub GetFiscalPeriodDates(ByVal sYear As String, ByVal sQuarter As String, ByVal sPeriod As String, ByVal sWeek As String, ByRef dtBeginDate As DateTime, ByRef dtEndDate As DateTime)
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Try
            'Validate the input
            Dim iYear As Integer
            Dim iQuarter As Integer = 0
            Dim iPeriod As Integer = 0
            Dim iWeek As Integer = 0
            Try
                iYear = Integer.Parse(sYear)
            Catch ex As System.Exception
                Throw New System.Exception("Year is invalid")
            End Try
            If sQuarter.Trim.Length > 0 Then
                iQuarter = Integer.Parse(sQuarter)
                If (iQuarter > 4) Or (iQuarter < 1) Then
                    Throw New System.Exception("Quarter must be 1-4")
                End If
            End If
            If sPeriod.Trim.Length > 0 Then
                If iQuarter = 0 Then
                    Throw New System.Exception("Quarter is required for Period")
                Else
                    Try
                        iPeriod = Integer.Parse(sPeriod)
                    Catch ex As System.Exception
                        Throw New System.Exception("Period is invalid")
                    End Try
                    If (iPeriod > 13) Or (iPeriod < 1) Then
                        Throw New System.Exception("Period is invalid")
                    End If
                End If
            End If
            If sWeek.Trim.Length > 0 Then
                If iPeriod = 0 Then
                    Throw New System.Exception("Period is required for Week")
                Else
                    Try
                        iWeek = Integer.Parse(sWeek)
                    Catch ex As System.Exception
                        Throw New System.Exception("Week is invalid")
                    End Try
                    If (iWeek > 4) Or (iWeek < 1) Then
                        Throw New System.Exception("Week is invalid")
                    End If
                End If
            End If
            'Look up the dates
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetPeriodDates"
            Dim prm As New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Year"
            prm.Value = iYear
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Quarter"
            prm.Value = IIf(iQuarter > 0, iQuarter, System.DBNull.Value)
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Period"
            prm.Value = IIf(iPeriod > 0, iPeriod, System.DBNull.Value)
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Input
            prm.DbType = DbType.Int32
            prm.ParameterName = "@Week"
            prm.Value = IIf(iWeek > 0, iWeek, System.DBNull.Value)
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Output
            prm.DbType = DbType.DateTime
            prm.ParameterName = "@BeginDate"
            cmd.Parameters.Add(prm)
            prm = New System.Data.SqlClient.SqlParameter
            prm.Direction = ParameterDirection.Output
            prm.DbType = DbType.DateTime
            prm.ParameterName = "@EndDate"
            cmd.Parameters.Add(prm)
            ItemCatalog.DataAccess.ExecuteSqlCommand(cmd, DataAccess.enuDBList.ItemCatalog)
            dtBeginDate = cmd.Parameters("@BeginDate").Value
            dtEndDate = cmd.Parameters("@EndDate").Value
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Sub
    Public Shared Function GetThisRegionsFacilities() As ArrayList
        Dim cmd As System.Data.SqlClient.SqlCommand = Nothing
        Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
        Try
            Dim results As New ArrayList
            cmd = New System.Data.SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "GetStoresAndDist"
            dr = ItemCatalog.DataAccess.GetSqlDataReader(cmd, DataAccess.enuDBList.ItemCatalog)
            If dr.HasRows Then
                While dr.Read
                    results.Add(New ItemCatalog.Store(dr!Store_No, dr!Store_Name, dr!Mega_Store, dr!WFM_Store))
                End While
            End If

            Return results
        Finally
            ItemCatalog.DataAccess.ReleaseDataObject(dr, DataAccess.enuDBList.ItemCatalog)
            ItemCatalog.DataAccess.ReleaseDataObject(cmd, DataAccess.enuDBList.ItemCatalog)
        End Try
    End Function
End Class
