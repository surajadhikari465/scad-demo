'********************************************************************************************************************************************************************************
'RipeDAO Summary
'The main class that incorporates data logic for ripe import functionality

'CREATED_BY         CREATED_DATE        FUNCTION_NAME                       FUNCTION_SUMMARY 
'--------------------------------------------------------------------------------------------
' vayals            12/14/09            Ripe_GetDataReader                  Rewrote the function in Global.vb to comply with v3 standards
'                                                                           Executes a Sproc using DATAFACTORY and returns the datareader
' vayals                                 Ripe_SystemDateTime                Rewrote the function in Global.vb to comply with v3 standards  
'                                                                           Returns teh system date and time from database
' vayals            12/14/09            Ripe_LoadCombo                      Loads the combobox data
' vayals            12/14/09            Ripe_CheckforExistingDistributions  Checks for teh existing distributions
' vayals            12/14/09            Ripe_CheckForImportErrors           Checks for errors in importing
' vayals            12/14/09            Ripe_ImportOrder                    Function that imports the order
' vayals            12/14/09            Ripe_GetRipeLocationStoreNo         Retreives store information for the given location
' vayals            12/14/09            Ripe_GetRipeCustomerStoreNo         Retreives store information for the given customer
' vayals            12/14/09            Ripe_RetreiveStoreVendorID          Retreives Vendor information for the given store
' vayals            12/14/09            Ripe_SQLOpenRecordSet               Retreives RipeCustomer By Ripe Zone and Location to load the grid
' vayals            02/05/10            Ripe_CheckReturnOrderCount          Retreives Return Order count for all orders that are already imported

'UPDATED__BY        UPDATE_DATE        FUNCTION_NAME                       UPDATE_SUMMARY 
'--------------------------------------------------------------------------------------------'
' vayals            02/05/10            Ripe_CheckforExistingDistributions  Added code to call function Ripe_CheckReturnOrderCount

'********************************************************************************************************************************************************************************

Imports Infragistics.Win.UltraWinDataSource
Imports Infragistics.Shared
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports System.Configuration
Imports System.Data.SqlClient
Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Common
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility.Encryption
Imports WholeFoods.IRMA.Tools.BusinessLogic

Namespace WholeFoods.IRMA.Tools.DataAccess
    Public Class RipeDAO
        Private Const sLocalErr As String = "RIPE Import Object"
        Public Event PrintingInvoice(ByRef iInvoiceCnt As Short)
        Private m_oCon As SqlClient.SqlConnection
        Private myConn As SqlConnection
        Public gDBInventory As DAO.Database




        Private Function Ripe_GetDataReader(ByVal StoredProcedure As String, ByVal paramList As DBParamList) As SqlClient.SqlDataReader


            'Dim factory As New DataFactory(DataFactory.dbRIPE)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As SqlDataReader = Nothing

            Try
                'validate parameters
                If StoredProcedure Is Nothing OrElse StoredProcedure.Length = 0 Then
                    Throw New ArgumentException("Invalid argument", "StoredProcedure")
                End If

                If paramList Is Nothing Then
                    reader = factory.GetStoredProcedureDataReader(StoredProcedure)
                Else
                    reader = factory.GetStoredProcedureDataReader(StoredProcedure, paramList)
                End If

            Catch ex As Exception

                Throw ex

            Finally
                factory = Nothing

            End Try


            Return reader

        End Function


        Public Function Ripe_LoadCombo(ByRef cbo As System.Windows.Forms.ComboBox, ByVal StoredProcedure As String, ByVal DataTextField As String, ByVal DataValueField As String, ByVal paramList As DBParamList) As Boolean
            'logger.Debug("LoadCombo Entry")
            Dim reader As SqlDataReader = Nothing
            Dim bStatus As Boolean = False
            'Dim objDaoRipe As New RipeDAO
            Try
                'validate parameters
                If StoredProcedure Is Nothing OrElse StoredProcedure.Length = 0 Then
                    Throw New ArgumentException("Invalid argument", "StoredProcedure")
                ElseIf DataTextField Is Nothing OrElse DataTextField.Length = 0 Then
                    Throw New ArgumentException("Invalid argument", "DataTextField")
                ElseIf DataValueField IsNot Nothing AndAlso DataValueField.Length = 0 Then
                    'DataValueField is optional for some stored procedures returning a single column
                    Throw New ArgumentException("Invalid argument", "DataValueField")
                End If
                reader = Ripe_GetDataReader(StoredProcedure, paramList)
                bStatus = Ripe_LoadCombo(cbo, reader, DataTextField, DataValueField)
            Catch ex As Exception
                'logger.Error(ex.Message)
                Throw ex
            Finally
                If reader IsNot Nothing Then
                    reader.Close()
                    reader = Nothing
                End If
            End Try
            'logger.Debug("LoadCombo")
            Return bStatus
        End Function


        Private Function Ripe_LoadCombo(ByRef cbo As System.Windows.Forms.ComboBox, ByVal reader As SqlClient.SqlDataReader, ByVal DataTextField As String, ByVal DataValueField As String) As Boolean
            'logger.Debug("LoadCombo Entry")
            Dim NewIndex As Integer = 0
            Dim iTextFieldIndex As Integer = -1     'use default to distinguish from 0-based index
            Dim iValueFieldIndex As Integer = -1
            Dim DoOnce As Boolean = False
            Dim bStatus As Boolean = False
            Try
                cbo.Items.Clear()
                'validate parameters
                If reader Is Nothing Then
                    Throw New ArgumentException("Invalid argument", "DataReader")
                ElseIf DataTextField Is Nothing OrElse DataTextField.Length = 0 Then
                    Throw New ArgumentException("Invalid argument", "DataTextField")
                ElseIf DataValueField IsNot Nothing AndAlso DataValueField.Length = 0 Then
                    'DataValueField is optional for some stored procedures returning a single column
                    Throw New ArgumentException("Invalid argument", "DataValueField")
                End If

                With reader
                    While .Read
                        If Not DoOnce Then
                            iTextFieldIndex = .GetOrdinal(DataTextField)
                            If .FieldCount > 1 Then
                                iValueFieldIndex = .GetOrdinal(DataValueField)
                            End If
                            DoOnce = True
                        End If

                        NewIndex = cbo.Items.Add(.GetString(iTextFieldIndex))
                        If iValueFieldIndex >= 0 Then
                            VB6.SetItemData(cbo, NewIndex, .GetValue(iValueFieldIndex))
                        End If
                    End While
                End With

                bStatus = True

            Catch ex As Exception
                'logger.Error(ex.Message)
                Throw ex

            End Try

            'logger.Debug("LoadCombo Exit")

            Return bStatus

        End Function



        Public Function Ripe_CheckforExistingDistributions(ByRef sDistDate As String, ByRef lRipeLocationID As Integer, ByRef sSelectedCust() As String) As Array
            'Dim rsAlreadyImported As New ADODB.Recordset
            Dim arySelectedCust() As String
            Dim iCnt As Short
            Dim aryAlreadyImported(0, 0) As String
            arySelectedCust = sSelectedCust
            ReDim aryAlreadyImported(3, 0)
            Dim drAlreadyImported As SqlClient.SqlDataReader
            Try
                'Dim cmdGetAlreadyImported As New SqlClient.SqlCommand("RIPECheckExistingDistributions"), prm As SqlClient.SqlParameter
                'Dim df As New DataFactory(DataFactory.dbRIPE)
                Dim df As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "DistDate"
                currentParam.Value = sDistDate
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "LocationID"
                currentParam.Value = lRipeLocationID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CustomerID"
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                For iCnt = 1 To UBound(arySelectedCust)
                    currentParam.Value = arySelectedCust(iCnt)
                    drAlreadyImported = df.GetStoredProcedureDataReader("RIPECheckExistingDistributions", paramList)
                    If drAlreadyImported.HasRows Then
                        'Call WriteMasterLog("Begin fill AlreadyImported Array.")
                        Do While drAlreadyImported.Read

                            ReDim Preserve aryAlreadyImported(3, UBound(aryAlreadyImported, 2) + 1)
                            aryAlreadyImported(0, UBound(aryAlreadyImported, 2)) = drAlreadyImported!IRSOrderHeaderID
                            aryAlreadyImported(1, UBound(aryAlreadyImported, 2)) = drAlreadyImported!OrderDescription
                            aryAlreadyImported(2, UBound(aryAlreadyImported, 2)) = drAlreadyImported!CompanyName
                            aryAlreadyImported(3, UBound(aryAlreadyImported, 2)) = 0

                        Loop

                        'Call WriteMasterLog("end fill AlreadyImported Array.")
                    End If
                    drAlreadyImported.Close()
                Next iCnt

                If UBound(aryAlreadyImported, 2) <> 0 Then
                    aryAlreadyImported = Ripe_CheckReturnOrderCount(aryAlreadyImported)
                End If

                Return aryAlreadyImported
            Catch ex As Exception
                'logger.Error(ex.Message)
                Throw ex

            End Try
        End Function


        Public Function Ripe_CheckReturnOrderCount(ByRef aryAlreadyImported As Array) As Array

            Try

                Dim drCheckReturnOrder As SqlClient.SqlDataReader
                Dim iCnt As Short
                Dim ireturnCount As Integer

                For iCnt = 1 To UBound(aryAlreadyImported, 2)
                    ireturnCount = 1
                    'Dim df As New DataFactory(DataFactory.dbRIPE)
                    Dim df As New DataFactory(DataFactory.ItemCatalog)
                    Dim paramList As New ArrayList
                    Dim currentParam As DBParam
                    currentParam = New DBParam
                    currentParam.Name = "OrderHeader_ID"
                    currentParam.Value = aryAlreadyImported(0, iCnt)
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)
                    drCheckReturnOrder = df.GetStoredProcedureDataReader("GetCreditOrderList", paramList)
                    If drCheckReturnOrder.HasRows Then
                        Do While drCheckReturnOrder.Read
                            aryAlreadyImported(3, iCnt) = ireturnCount
                            ireturnCount = ireturnCount + 1
                        Loop
                    End If
                    drCheckReturnOrder.Close()
                Next iCnt

                Return aryAlreadyImported
            Catch ex As Exception
                'logger.Error(ex.Message)
                Throw ex

            End Try
        End Function


        Public Function Ripe_CheckForImportErrors(ByRef sDistribution As String, ByRef lFromStore As Integer, ByRef lToStore As Integer) As Boolean
            Try
                Dim drImportErrors As SqlClient.SqlDataReader
                'Dim df As New DataFactory(DataFactory.dbRIPE)
                Dim df As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList
                Dim currentParam As DBParam
                currentParam = New DBParam
                currentParam.Name = "DistributionDate"
                currentParam.Value = sDistribution
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
                currentParam = New DBParam
                currentParam.Name = "FromStore_No"
                currentParam.Value = lFromStore
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                currentParam = New DBParam
                currentParam.Name = "ToStore_No"
                currentParam.Value = lToStore
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                drImportErrors = df.GetStoredProcedureDataReader("RIPEImportErrors", paramList)
                Ripe_CheckForImportErrors = drImportErrors.HasRows
                drImportErrors = Nothing
            Catch ex As Exception
                Throw ex
            End Try
        End Function



        Public Function Ripe_ImportOrder(ByRef sDistribution As String, ByRef lFromStore As Integer, ByRef lToStore As Integer, ByRef lUserID As Integer, ByRef sImportDateTime As String) As Integer

            Try

                Dim colOrderIDs As New Collection
                Dim dsImportRecordset As New Data.DataSet
                Dim drImportRecordset As Data.DataTableReader
                Dim objBaseOrder As clsBaseOrder = Nothing

                Dim lVendorID As Integer
                Dim lReceiveLocationID As Integer
                Dim lIRSOrderHeaderID, lRIPEOrders1ID As Integer
                Dim objItemUnit As clsBaseItemUnit
                Dim colUnits As Collection
                Dim lUnitID As Integer
                Dim lTransfer_SubTeam As Integer
                Dim lTransfer_To_SubTeam As Integer
                Dim iOrderCount As Short

                'On Error GoTo CleanUp


                colUnits = New Collection

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim daGetDists As New SqlDataAdapter

                'Dim df As New DataFactory(DataFactory.dbRIPE)
                Dim df As New DataFactory(DataFactory.ItemCatalog)
                Dim command As SqlCommand = df.GetDataCommand("RIPEGetDistributions", Nothing, True)

                Dim param1 As SqlParameter
                param1 = New SqlParameter("DistributionDate", SqlDbType.VarChar)
                param1.Value = sDistribution
                command.Parameters.Add(param1)

                Dim param2 As SqlParameter
                param2 = New SqlParameter("LocationID", SqlDbType.Int)
                param2.Value = lFromStore
                command.Parameters.Add(param2)

                Dim param3 As SqlParameter
                param3 = New SqlParameter("CustomerID", SqlDbType.Int)
                param3.Value = lToStore
                command.Parameters.Add(param3)

                daGetDists = df.GetDataAdapter(command, Nothing)

                daGetDists.Fill(dsImportRecordset)
                drImportRecordset = dsImportRecordset.CreateDataReader()
                'Call WriteMasterLog("end RIPEGetDistributions get data")

                '-- Make sure there is data to import
                Dim iCnt As Short
                Dim oTran As SqlClient.SqlTransaction = Nothing
                If drImportRecordset.HasRows Then

                    Try
                        m_oCon = New SqlConnection(df.ConnectString)
                        m_oCon.Open()
                        oTran = m_oCon.BeginTransaction()
                        '-- Get Vendor #s
                        'Call WriteMasterLog("begin get From Store VendorID")
                        lVendorID = Ripe_RetreiveStoreVendorID(Ripe_GetRipeLocationStoreNo(lFromStore, oTran), oTran)
                        'Call WriteMasterLog("end get From Store VendorID")
                        'Call WriteMasterLog("begin get To Store VendorID")
                        lReceiveLocationID = Ripe_RetreiveStoreVendorID(Ripe_GetRipeCustomerStoreNo(lToStore, oTran), oTran)
                        'Call WriteMasterLog("end get To Store VendorID")
                        lTransfer_SubTeam = -1
                        lTransfer_To_SubTeam = -1

                        '-- Call the ordering class
                        'Call WriteMasterLog("begin create base order obj")
                        objBaseOrder = New clsBaseOrder(m_oCon, oTran)
                        'Call WriteMasterLog("end create base order obj")

                        '-- Get Units
                        'Call WriteMasterLog("begin get unit collection")
                        objBaseOrder.GetUnitCollection(colUnits)
                        'Call WriteMasterLog("end get unit collection")

                        '-- Add Items
                        iOrderCount = 0
                        Dim dblCreateOrders As Double
                        Dim dblAddItemTime As Double = 0
                        dblCreateOrders = Microsoft.VisualBasic.DateAndTime.Timer
                        'Call WriteMasterLog("begin create orders")
                        Do While drImportRecordset.Read
                            System.Windows.Forms.Application.DoEvents()

                            If IsDBNull(drImportRecordset!Transfer_SubTeam) Then
                                Err.Raise(ERR_RIPE_IMPORT_NO_TRANSFER_SUBTEAM, sLocalErr, ERR_RIPE_IMPORT_NO_TRANSFER_SUBTEAM_DESC)
                            End If

                            If IsDBNull(drImportRecordset!Transfer_To_SubTeam) Then
                                Err.Raise(ERR_RIPE_IMPORT_NO_TRANSFER_TO_SUBTEAM, sLocalErr, ERR_RIPE_IMPORT_NO_TRANSFER_TO_SUBTEAM_DESC)
                            End If

                            '-- Create an Order for a new subteam
                            If (lTransfer_SubTeam <> drImportRecordset!Transfer_SubTeam) Or (lTransfer_To_SubTeam <> drImportRecordset!Transfer_To_SubTeam) Then
                                If dblAddItemTime = 0 Then
                                    dblAddItemTime = Microsoft.VisualBasic.DateAndTime.Timer
                                    'call WriteMasterLog("Begin add order items ")
                                Else
                                    'call WriteMasterLog("End add order items (total time: " & Microsoft.VisualBasic.DateAndTime.Timer - dblAddItemTime & ")")
                                    dblAddItemTime = Microsoft.VisualBasic.DateAndTime.Timer
                                    'call WriteMasterLog("Begin add order items ")
                                End If
                                'call WriteMasterLog("Begin Create new order")
                                lIRSOrderHeaderID = objBaseOrder.CreateOrder(lVendorID, lReceiveLocationID, lReceiveLocationID, drImportRecordset!Transfer_SubTeam, drImportRecordset!Transfer_To_SubTeam, drImportRecordset!Orders1ID, sDistribution, sImportDateTime, False, lUserID)
                                'call WriteMasterLog("end Create new order")

                                Call colOrderIDs.Add(lIRSOrderHeaderID)
                                'If Err.Number <> 0 Then GoTo CleanUp

                                lTransfer_SubTeam = drImportRecordset!Transfer_SubTeam
                                lTransfer_To_SubTeam = drImportRecordset!Transfer_To_SubTeam
                                iOrderCount = iOrderCount + 1
                                'sOrderHeader = sOrderHeader & rsImportRecordset!Transfer_SubTeam & "|" & rsImportRecordset!Transfer_To_SubTeam & " - " & lOrderHeaderID & vbCrLf
                            End If

                            'if the last RIPEOrders1ID has changed we need a new record in the Recipe..IRSOrderHistory table
                            If lRIPEOrders1ID <> drImportRecordset!Orders1ID Then
                                lRIPEOrders1ID = drImportRecordset!Orders1ID
                                Dim cmdInsertImportData As New SqlClient.SqlCommand("RIPEInsertImportData " & lIRSOrderHeaderID & ", " & lRIPEOrders1ID & ", '" & sDistribution & "', '" & sImportDateTime & "'", m_oCon, oTran)
                                cmdInsertImportData.CommandTimeout = 600
                                'call WriteMasterLog("Begin EXEC RIPEInsertImportData " & lIRSOrderHeaderID & ", " & lRIPEOrders1ID & ", '" & sDistribution & "', '" & sImportDateTime & "'")
                                cmdInsertImportData.ExecuteNonQuery()
                                'call WriteMasterLog("End EXEC RIPEInsertImportData " & lIRSOrderHeaderID & ", " & lRIPEOrders1ID & ", '" & sDistribution & "', '" & sImportDateTime & "'")
                                cmdInsertImportData.Dispose()
                            End If

                            'TO DO: IRMA now has ItemUnit.UnitSysCode to identify the unit by the system - needs to be added to RIPE and use those columns here to match
                            lUnitID = 0
                            For Each objItemUnit In colUnits
                                If UCase(objItemUnit.sUnitAbbreviation) = UCase(drImportRecordset!UnitAbbr) Then lUnitID = objItemUnit.lUnitID
                            Next objItemUnit
                            objBaseOrder.AddOpenOrderItem(lIRSOrderHeaderID, drImportRecordset!Item_Key, drImportRecordset!QuantityRequested, drImportRecordset!QuantityShipped, sDistribution, lUnitID)

                        Loop
                        'close all orders
                        For iCnt = 1 To colOrderIDs.Count()
                            Call objBaseOrder.CloseOrder(colOrderIDs.Item(iCnt), sDistribution)
                        Next iCnt
                        Ripe_ImportOrder = iOrderCount
                        '-- End Transaction
                        oTran.Commit()
                    Catch ex As Exception
                        oTran.Rollback()
                        Throw ex
                    Finally
                        If Not (objBaseOrder Is Nothing) Then
                            objBaseOrder.Dispose()
                            objBaseOrder = Nothing
                        End If
                        If Not (oTran Is Nothing) Then
                            oTran.Dispose()
                            oTran = Nothing
                        End If

                        colUnits = Nothing
                        drImportRecordset = Nothing
                    End Try
                Else
                    Ripe_ImportOrder = 0
                End If

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Private Function Ripe_GetRipeLocationStoreNo(ByVal lLocationID As Integer, ByVal oTran As SqlClient.SqlTransaction) As Integer
            Try
                Dim drStoreNo As SqlClient.SqlDataReader
                Dim iStoreNo As Integer

                'Dim df As New DataFactory(DataFactory.dbRIPE)
                Dim df As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "LocationID"
                currentParam.Value = lLocationID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                drStoreNo = df.GetStoredProcedureDataReader("GetRipeLocationStoreNo", paramList)
                'Dim cmdGetStoreNo As New SqlClient.SqlCommand("GetRipeLocationStoreNo " & lLocationID, m_oCon, oTran)
                'drStoreNo = cmdGetStoreNo.ExecuteReader
                If drStoreNo.HasRows Then
                    drStoreNo.Read()
                    iStoreNo = drStoreNo!Store_No
                End If
                drStoreNo.Close()
                drStoreNo = Nothing
                'cmdGetStoreNo.Dispose()

                Return iStoreNo

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function Ripe_GetRipeCustomerStoreNo(ByVal lCustomerID As Integer, ByVal oTran As SqlClient.SqlTransaction) As Integer
            Try

                Dim drStoreNo As SqlClient.SqlDataReader
                Dim iStoreNo As Integer
                'Dim df As New DataFactory(DataFactory.dbRIPE)
                Dim df As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList
                Dim currentParam As DBParam
                currentParam = New DBParam
                currentParam.Name = "CustomerID"
                currentParam.Value = lCustomerID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                drStoreNo = df.GetStoredProcedureDataReader("GetRipeCustomerStoreNo", paramList)
                'Dim cmdGetStoreNo As New SqlClient.SqlCommand("GetRipeCustomerStoreNo " & lCustomerID, m_oCon, oTran)
                'rStoreNo = cmdGetStoreNo.ExecuteReader
                If drStoreNo.HasRows Then
                    drStoreNo.Read()
                    iStoreNo = drStoreNo!StoreNo
                End If
                drStoreNo.Close()
                drStoreNo = Nothing
                'cmdGetStoreNo.Dispose()

                Return iStoreNo
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Private Function Ripe_RetreiveStoreVendorID(ByVal lStoreNo As Integer, ByVal oTran As SqlClient.SqlTransaction) As Integer
            Try

           
                Dim drVendorID As SqlClient.SqlDataReader
                Dim VendorID As Integer

                'Dim df As New DataFactory(DataFactory.dbRIPE)
                Dim df As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList
                Dim currentParam As DBParam


                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = lStoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                drVendorID = df.GetStoredProcedureDataReader("GetStoreVendor", paramList)
                'Dim cmdGetVendorID As New SqlClient.SqlCommand("GetStoreVendor " & lStoreNo, m_oCon, oTran)
                'drVendorID = cmdGetVendorID.ExecuteReader
                If drVendorID.HasRows Then
                    drVendorID.Read()
                    VendorID = IIf(IsDBNull(drVendorID!Vendor_ID), -1, drVendorID!vendor_ID)
                End If
                drVendorID.Close()
                drVendorID = Nothing
                'cmdGetVendorID.Dispose()

                Return VendorID

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function Ripe_SQLOpenRecordSet(ByRef sCall As String, ByRef param3 As Integer, ByRef Param4 As String, ByRef Param1 As Integer, ByRef Param2 As Integer, Optional ByRef bValidateLogon As Boolean = True) As ArrayList

            Dim dr As SqlDataReader
            Dim al As ArrayList = New ArrayList
            If bValidateLogon Then ValidateLogon()
            'Dim df As New DataFactory(DataFactory.dbRIPE)
            Dim df As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            currentParam = New DBParam
            currentParam.Name = "ZoneId"
            currentParam.Value = DBNull.Value
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            currentParam = New DBParam
            currentParam.Name = "LocationID"
            currentParam.Value = param3
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            currentParam = New DBParam
            currentParam.Name = "DistDate"
            currentParam.Value = Param4
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
            dr = df.GetStoredProcedureDataReader(sCall, paramList)

            For Each row As Object In dr
                al.Add(row)
            Next


            Ripe_SQLOpenRecordSet = al


        End Function


        Public Function Ripe_SystemDateTime(Optional ByRef bDateOnly As Boolean = False) As Date
            '-----------------------------------------------------
            ' Purpose: Returns system date and time from database.
            '-----------------------------------------------------

            If gbUseLocalTime Then
                'This would be used for testing purposes only.
                'It is activated by starting the applicaiton and passing "uselocaltime" as a command line parameter.
                Ripe_SystemDateTime = Now
            Else
                Try

                    'Dim factory As New DataFactory(DataFactory.dbRIPE)
                    Dim factory As New DataFactory(DataFactory.ItemCatalog)
                    Ripe_SystemDateTime = factory.ExecuteScalar("GetSystemDate")
                    'gRSRecordset = SQLOpenRecordSet("EXEC GetSystemDate", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    'Ripe_SystemDateTime = gRSRecordset.Fields("SystemDate").Value
                Finally
                    'If gRSRecordset IsNot Nothing Then
                    '    gRSRecordset.Close()
                    ''    gRSRecordset = Nothing
                    'End If
                End Try
            End If

            If bDateOnly Then
                Ripe_SystemDateTime = FormatDateTime(Ripe_SystemDateTime, DateFormat.ShortDate)
            End If

        End Function


        Public Function Ripe_GetImportedOrders(ByRef strImportDateTime As String) As String

            Try


                Dim drImportedOrder As SqlClient.SqlDataReader
                Dim strOrderIds As String
                strOrderIds = ""

                'Dim df As New DataFactory(DataFactory.dbRIPE)
                Dim df As New DataFactory(DataFactory.ItemCatalog)

                Dim paramList As New ArrayList
                Dim currentParam As DBParam


                currentParam = New DBParam
                currentParam.Name = "ImportDate"
                currentParam.Value = strImportDateTime
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                drImportedOrder = df.GetStoredProcedureDataReader("RIPEGetImportedOrders", paramList)

                If drImportedOrder.HasRows Then
                    Do While drImportedOrder.Read
                        strOrderIds = strOrderIds + drImportedOrder!IRSOrderHeaderID.ToString + ","
                    Loop
                End If
                drImportedOrder.Close()
                drImportedOrder = Nothing

                Ripe_GetImportedOrders = strOrderIds

            Catch ex As Exception
                Throw ex
            End Try


        End Function




        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

    End Class
End Namespace