Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.POSPull.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports WholeFoods.Utility.FTP
Imports WholeFoods.Utility
Imports ExcelInterop = Microsoft.Office.Interop.Excel

Namespace WholeFoods.IRMA.Replenishment.POSPull.BusinessLogic
    Public Class POSPullBO
        'holds FTP information (ip,user,pass,secure flag,port) for specific writer type
        Private _ftpInfo As Hashtable = New Hashtable
        Private _posItemInfo As List(Of POSItemBO) = New List(Of POSItemBO)
        <VBFixedString(512)> Public sRecords As String

        Private Const SOURCE As Integer = 1
        Private Const TARGET As Integer = 2
        Private Const EXCEPTION_RESOLUTION_FLIE_HEADERS As String = "Identifier,Identifier Type,POS Description,Description,Item Size,Tmp Range High,Tmp Range Low,High,Units per Pallet,Sign Caption,Item Pack,Yield,Tie,Food Stamps,Tax Class,Brand,Class,National Class,Store,Sub-team,Item UOM,Retail Units,Distribution Units,Vendor Units,PS Vendor,Stop Sale,Price Change,Price Type,Promo Price End,Promo Price Start,POS Price,POS Promo Price,Promo Price Multiple,Reg Price Multiple,MSRP Price,MSRP Multiple,Sr Discount,Authorize,Auth Lock,Not Available"

        ' IBM Directory and Filenames
        Private _sourceIBMPathFileName As String
        Private _sourceIBMFileName As String
        ' NCR Directory and Filenames
        Private _sourceNCRPathFileName As String
        Private _sourceNCRFileName As String
        ' Common target
        Private _localTargetFile As String
        Private _copyTargetFile As String
        Private _DBServerSourceFile As String
        Private _POSPullLocalDir As String
        Private _POSAuditExceptionFileName As String
        Private _POSAuditExceptionsResolutionFileName As String
        Private _POSAuditExceptionFileDelimiter As String
        Private _POSAuditTextFileWriter As StreamWriter = Nothing
        Private _POSAuditFileTargetServer As String
        Private _POSAuditFileTargetFolder As String
        Private _POSAuditFileFTPUser As String
        Private _POSAuditFileFTPPwd As String
        Private _POSAuditExceptionStatus As Integer = 0

#Region "Properties"
        Public ReadOnly Property DBServerSourceFile() As String
            Get
                Return _DBServerSourceFile
            End Get
        End Property
        Public ReadOnly Property SourceIBMFileName() As String
            Get
                Return _sourceIBMFileName
            End Get
        End Property
        Public ReadOnly Property SourceIBMPathFileName() As String
            Get
                Return _sourceIBMPathFileName
            End Get
        End Property
        Public ReadOnly Property SourceNCRFileName() As String
            Get
                Return _sourceNCRFileName
            End Get
        End Property
        Public ReadOnly Property SourceNCRPathFileName() As String
            Get
                Return _sourceNCRPathFileName
            End Get
        End Property
        Public ReadOnly Property LocalTargetFile() As String
            Get
                Return _localTargetFile
            End Get
        End Property
        Public ReadOnly Property CopyTargetFile() As String
            Get
                Return _copyTargetFile
            End Get
        End Property
        Public Property POSItemInfo() As List(Of POSItemBO)
            Get
                Return _posItemInfo
            End Get
            Set(ByVal value As List(Of POSItemBO))
                _posItemInfo = value
            End Set
        End Property

        Public Property FTPInfo() As Hashtable
            Get
                Return _ftpInfo
            End Get
            Set(ByVal value As Hashtable)
                _ftpInfo = value
            End Set
        End Property
        Public ReadOnly Property POSAuditExceptionFileName() As String
            Get
                Return _POSAuditExceptionFileName
            End Get
        End Property
        Public ReadOnly Property POSAuditExceptionPathFileName() As String
            Get
                Return String.Format("{0}{1}", _POSPullLocalDir, _POSAuditExceptionFileName)
            End Get
        End Property
        Public ReadOnly Property POSAuditExceptionFileDelimiter() As String
            Get
                Return _POSAuditExceptionFileDelimiter
            End Get
        End Property

        Public ReadOnly Property POSAuditExceptionStatus() As Integer
            Get
                Return _POSAuditExceptionStatus
            End Get
        End Property
#End Region

#Region "Private Helper Methods"

        Private Function PDValue(ByVal sString As String) As String

            Dim sTemp As String
            Dim l As Integer

            sTemp = ""

            For l = 1 To Len(sString)
                sTemp = sTemp & Right("0" & Hex(Asc(Mid(sString, l, 1))), 2)
            Next l

            If UCase(Left(sTemp, 1)) = "F" Or UCase(Left(sTemp, 1)) = "D" Then
                sTemp = Mid(sTemp, 2)
            ElseIf UCase(Left(sTemp, 1)) = "A" Then
                Mid(sTemp, 1, 1) = "-"
            End If

            If sTemp = "" Then
                PDValue = CStr(0)
            Else
                If Left(sTemp, 1) = "D" Then
                    sTemp = Mid(sTemp, 2)
                End If
                PDValue = CStr(CDec(sTemp))
            End If

        End Function
        Private Function PDString(ByVal sString As String) As String

            Dim sTemp As String
            Dim l As Integer
            sTemp = String.Empty

            For l = 1 To Len(sString)
                sTemp = sTemp & Right("0" & Hex(Asc(Mid(sString, l, 1))), 2)
            Next l

            If UCase(Left(sTemp, 1)) = "F" Then
                sTemp = Mid(sTemp, 2)
            ElseIf UCase(Left(sTemp, 1)) = "A" Then
                Mid(sTemp, 1, 1) = "-"
            End If

            If sTemp = "" Then
                PDString = CStr(0)
            Else
                PDString = sTemp
            End If

        End Function
        '20080408 - DaveStacey - Added Function to sort through IBM SubteamNo Length
        Private Function GetSubTeams() As System.Collections.SortedList
            Dim row As DataRow = Nothing
            Dim subteamList As System.Collections.SortedList = New SortedList()
            Dim subTeamNoKey As String
            Dim deptNoValue As String

            Try
                For Each row In POSPullDAO.GetSubTeamDataTable().Rows
                    If (Not (IsDBNull(row.Item("Dept_No")))) Then
                        subTeamNoKey = CInt(row.Item("SubTeam_No")).ToString
                        deptNoValue = CInt(row.Item("Dept_No")).ToString
                        subteamList.Add(subTeamNoKey, deptNoValue)
                    End If
                Next
            Catch ex As Exception
                'HandleError(ex)
            Finally
                row = Nothing
            End Try
            Return subteamList
        End Function


        'Public Function TransformIBM() As Boolean
        '    ' Public Sub TransformIBM()
        '    Dim posItemBO As POSItemBO = New POSItemBO
        '    '  iterate through the item's data and map to posItemBO properties

#End Region

#Region "Public Methods"

        Public Sub TransformIBM(ByVal StoreNo As Integer)
            Dim posItem As POSItemBO = New POSItemBO()
            Dim posPullDao As POSPullDAO = New POSPullDAO
            Dim ItemDataset As DataSet = New DataSet
            Dim SubTeamSortedList As System.Collections.SortedList
            Dim strSql As String = String.Empty

            Dim i As Integer
            Dim x As Integer
            Dim y As Integer
            Dim z As Integer
            Dim sDateTime As String
            Dim FileDate As Date

            Dim sRecord As New Microsoft.VisualBasic.Compatibility.VB6.FixedLengthString(512)

            'try sRecords declared at the top of the class with an 'attribute' of <VBFixedString(512)>
            Dim lBlocks As Integer
            Dim lRecordSize As Integer
            Dim lDivisor As Integer
            Dim lKeyLength As Integer
            Dim lHashingMethod As Integer

            Try

                SubTeamSortedList = GetSubTeams()
                ' Gets the list of keys and the list of values.
                Dim myKeyList As IList = SubTeamSortedList.GetKeyList()
                Dim myValueList As IList = SubTeamSortedList.GetValueList()

                ItemDataset = posPullDao.GetItemDataset(strSql, ItemDataset, "tblItemDataset")

                ' open the source and target files on the local directory
                FileOpen(SOURCE, Me.SourceIBMPathFileName, OpenMode.Random, OpenAccess.Read, OpenShare.LockRead, 512)
                FileOpen(TARGET, Me.LocalTargetFile, OpenMode.Output)
                ' read the source file
                FileGet(SOURCE, sRecord.Value, 1, True)

                lBlocks = Asc(Mid(sRecord.Value, 43, 1)) + (Asc(Mid(sRecord.Value, 44, 1)) * 256) + (Asc(Mid(sRecord.Value, 45, 1)) * 256 * 256) + (Asc(Mid(sRecord.Value, 46, 1)) * 256 * 256 * 256)
                lRecordSize = Asc(Mid(sRecord.Value, 47, 1)) + (Asc(Mid(sRecord.Value, 48, 1)) * 256)
                lDivisor = Asc(Mid(sRecord.Value, 49, 1)) + (Asc(Mid(sRecord.Value, 50, 1)) * 256) + (Asc(Mid(sRecord.Value, 51, 1)) * 256 * 256) + (Asc(Mid(sRecord.Value, 52, 1)) * 256 * 256 * 256)
                lKeyLength = Asc(Mid(sRecord.Value, 55, 1)) + (Asc(Mid(sRecord.Value, 56, 1)) * 256)
                lHashingMethod = Asc(Mid(sRecord.Value, 180, 1))
                y = 508 \ lRecordSize


                For x = 2 To lBlocks
                    ' increment the 'read' position
                    FileGet(SOURCE, sRecord.Value, x, True)

                    For z = 1 To y

                        posItem.Identifier = Mid(sRecord.Value, 5 + (lRecordSize * (z - 1)), lKeyLength)
                        If Trim(posItem.Identifier) = "*** E" Then posItem.Identifier = String.Empty
                        posItem.Identifier = PDValue(posItem.Identifier)

                        If Val(posItem.Identifier) <> 0 Then
                            posItem.QuantityRequired = (Asc(Mid(sRecord.Value, 11 + (lRecordSize * (z - 1)), 1)) And &H20S) = &H20S
                            posItem.PriceRequired = (Asc(Mid(sRecord.Value, 11 + (lRecordSize * (z - 1)), 1)) And &H10S) = &H10S
                            posItem.RetailSale = (Asc(Mid(sRecord.Value, 11 + (lRecordSize * (z - 1)), 1)) And &H4S) = 0
                            posItem.RestrictedHours = (Asc(Mid(sRecord.Value, 11 + (lRecordSize * (z - 1)), 1)) And &H2S) = 0
                            posItem.FoodStamps = (Asc(Mid(sRecord.Value, 12 + (lRecordSize * (z - 1)), 1)) And &H8S) = &H8S
                            posItem.TaxFlag1 = (Asc(Mid(sRecord.Value, 12 + (lRecordSize * (z - 1)), 1)) And &H80S) = &H80S
                            posItem.TaxFlag2 = (Asc(Mid(sRecord.Value, 12 + (lRecordSize * (z - 1)), 1)) And &H40S) = &H40S
                            posItem.TaxFlag3 = (Asc(Mid(sRecord.Value, 12 + (lRecordSize * (z - 1)), 1)) And &H20S) = &H20S
                            posItem.TaxFlag4 = (Asc(Mid(sRecord.Value, 12 + (lRecordSize * (z - 1)), 1)) And &H10S) = &H10S
                            posItem.Discountable = (Asc(Mid(sRecord.Value, 12 + (lRecordSize * (z - 1)), 1)) And &H2S) = 0
                            posItem.IBMDiscount = (Asc(Mid(sRecord.Value, 13 + (lRecordSize * (z - 1)), 1)) And &H4S) = &H4S
                            posItem.ItemTypeID = CType(Val(PDValue(Mid(sRecord.Value, 14 + (lRecordSize * (z - 1)), 1))) Mod 10, Short)

                            'UPGRADE_WARNING: Mod has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                            posItem.PricingMethodID = CType(Val(PDValue(Mid(sRecord.Value, 14 + (lRecordSize * (z - 1)), 1))) Mod 10, Short)
                            'DaveS - Took out *10 part for subteam no so this will work
                            posItem.SubTeamNo = CType(Val(PDValue(Mid(sRecord.Value, 15 + (lRecordSize * (z - 1)), 2))), Integer)

                            For i = 0 To SubTeamSortedList.Count - 1
                                If posItem.SubTeamNo = CType((myValueList(i)), Integer) Then
                                    posItem.SubTeamNo = CType((myKeyList(i)), Integer)
                                    Exit For
                                End If
                            Next i

                            posItem.CasePrice = CType(Val(PDValue(Mid(sRecord.Value, 17 + (lRecordSize * (z - 1)), 3))) / 100, Decimal)
                            posItem.DealQuantity = CType(Val(PDValue(Mid(sRecord.Value, 21 + (lRecordSize * (z - 1)), 1))), Short)

                            Select Case posItem.PricingMethodID
                                Case 0, 1
                                    posItem.UnitPrice = CType(Val(PDValue(Mid(sRecord.Value, 22 + (lRecordSize * (z - 1)), 5))) / 100, Decimal)
                                    posItem.DealPrice = 0
                                Case 2
                                    posItem.DealQuantity = 1
                                    posItem.UnitPrice = CType(Val(Mid(PDString(Mid(sRecord.Value, 22 + (lRecordSize * (z - 1)), 5)), 6, 5)) / 100, Decimal)
                                    posItem.DealPrice = CType(Val(Mid(PDString(Mid(sRecord.Value, 22 + (lRecordSize * (z - 1)), 5)), 1, 5)) / 100, Decimal)
                                Case 3, 4
                                    posItem.DealQuantity = 1
                                    posItem.UnitPrice = CType(Val(Mid(PDString(Mid(sRecord.Value, 22 + (lRecordSize * (z - 1)), 5)), 1, 5)) / 100, Decimal)
                                    posItem.DealPrice = CType(Val(Mid(PDString(Mid(sRecord.Value, 22 + (lRecordSize * (z - 1)), 5)), 6, 5)) / 100, Decimal)
                            End Select
                            posItem.POSDescription = Trim(Mid(sRecord.Value, 29 + (lRecordSize * (z - 1)), 18))

                            strSql = "Identifier = " + "'" + CStr(Trim(posItem.Identifier)) + "'"
                            Dim foundRows As DataRow() = ItemDataset.Tables("tblItemDataset").Select(strSql)
                            If Not (foundRows.Length = 0) Then
                                For i = 0 To foundRows.GetUpperBound(0)
                                    posItem.ItemKey = CInt(foundRows(i)(0))
                                Next i
                            Else
                                posItem.ItemKey = -1
                            End If

                            FileDate = FileDateTime(Me.SourceIBMPathFileName)
                            sDateTime = String.Format("{0} {1}:{2}", FileDate.ToString("M/dd/yyyy"), FileDate.Hour.ToString(), Now.Minute.ToString())

                            ' write this posItem to the file
                            PrintLine(TARGET, posItem.ToIBMFileString(StoreNo, sDateTime))
                        End If
                    Next z

                Next x

                FileClose(SOURCE)
                FileClose(TARGET)

                ' copy the target file to the database server
                FileCopy(Me.LocalTargetFile, Me.CopyTargetFile)

            Finally
                ' Let the CATCH bubble up - don't catch here, just clean up in the finally
                ItemDataset = Nothing
                FileClose(SOURCE)
                FileClose(TARGET)
            End Try

        End Sub

        Public Sub TransformNCR(ByVal StoreNo As Integer)
            Dim posItem As POSItemBO = Nothing
            'dataset for getting itemType
            Dim posPullDao As POSPullDAO = New POSPullDAO
            Dim ItemDataset As DataSet = New DataSet
            Dim sDateTime As String
            Dim FileDate As Date
            Dim strSql As String = String.Empty
            Dim fileReader As System.IO.StreamReader = Nothing
            Dim stringReader As String
            Dim foundRows As DataRow() = Nothing
            Dim SubTeamSortedList As System.Collections.SortedList
            Dim i As Integer

            Try
                SubTeamSortedList = GetSubTeams()
                ' Gets the list of keys and the list of values.
                Dim myKeyList As IList = SubTeamSortedList.GetKeyList()
                Dim myValueList As IList = SubTeamSortedList.GetValueList()

                ItemDataset = posPullDao.GetItemDataset(strSql, ItemDataset, "tblItemDataset")

                '' open the target files on the local directory
                FileOpen(TARGET, Me.LocalTargetFile, OpenMode.Output)

                fileReader = My.Computer.FileSystem.OpenTextFileReader(Me.SourceNCRPathFileName)
                While Not fileReader.EndOfStream
                    'reset values from previous iteration of while loop
                    posItem = New POSItemBO
                    stringReader = Nothing
                    foundRows = Nothing

                    stringReader = fileReader.ReadLine()
                    Dim NCRArray() As String = Split(stringReader, ",")
                    ' iterate through the item and map to posItemBO properties

                    If NCRArray.Length = 63 Then
                        posItem.Identifier = NCRArray(0)
                        posItem.POSDescription = NCRArray(5)
                        posItem.PricingMethodID = CInt(NCRArray(6))
                        posItem.ItemTypeID = CInt(NCRArray(7))
                        posItem.SubTeamNo = CInt(NCRArray(9))
                        posItem.UnitPrice = CDec(CInt(NCRArray(11)) * 0.01)
                        posItem.ForPrice = CInt(NCRArray(15))
                        posItem.ForQuantity = CInt(NCRArray(16))
                        posItem.DealPrice = CInt(NCRArray(18))
                        posItem.DealQuantity = CInt(NCRArray(19))

                        posItem.FoodStamps = CBool(CUInt(NCRArray(32)) And &H10)
                        posItem.TaxFlag1 = CBool(CUInt(NCRArray(32)) And &H1)
                        posItem.TaxFlag2 = CBool(CUInt(NCRArray(32)) And &H2)
                        posItem.TaxFlag3 = CBool(CUInt(NCRArray(32)) And &H4)
                        posItem.TaxFlag4 = CBool(CUInt(NCRArray(32)) And &H8)
                        posItem.SoldByWeight = CBool(CULng(NCRArray(31)) And CULng(&H40))

                    ElseIf NCRArray.Length = 65 Then
                        Dim fStr As String
                        fStr = NCRArray(1)
                        fStr = NCRArray(1).Remove(NCRArray(1).Length - 1)
                        fStr = Right(fStr, 12)
                        posItem.Identifier = fStr
                        posItem.POSDescription = NCRArray(6)
                        posItem.PricingMethodID = CInt(NCRArray(7))
                        posItem.ItemTypeID = CInt(NCRArray(8))
                        posItem.SubTeamNo = CInt(NCRArray(10))
                        posItem.UnitPrice = CDec(CInt(NCRArray(12)) * 0.01)
                        posItem.ForPrice = CInt(NCRArray(16))
                        posItem.ForQuantity = CInt(NCRArray(17))
                        posItem.DealPrice = CInt(NCRArray(19))
                        posItem.DealQuantity = CInt(NCRArray(20))

                        posItem.FoodStamps = CBool(CUInt(NCRArray(33)) And &H10)
                        posItem.TaxFlag1 = CBool(CUInt(NCRArray(33)) And &H1)
                        posItem.TaxFlag2 = CBool(CUInt(NCRArray(33)) And &H2)
                        posItem.TaxFlag3 = CBool(CUInt(NCRArray(33)) And &H4)
                        posItem.TaxFlag4 = CBool(CUInt(NCRArray(33)) And &H8)
                        posItem.SoldByWeight = CBool(CULng(NCRArray(32)) And CULng(&H40))

                    End If

                    ' The controller uses SubTeam.Dept_No so this needs to be mapped back to SubTeam_No
                    For i = 0 To SubTeamSortedList.Count - 1
                        If posItem.SubTeamNo = CType((myValueList(i)), Integer) Then
                            posItem.SubTeamNo = CType((myKeyList(i)), Integer)
                            Exit For
                        End If
                    Next i

                    posItem.QuantityRequired = False
                    posItem.PriceRequired = False
                    posItem.RetailSale = True
                    posItem.RestrictedHours = False
                    posItem.Discountable = False
                    posItem.IBMDiscount = False
                    '
                    'PosItem.CasePrice = not in 
                    '

                    strSql = "PaddedIdentifier = " + "'" + CStr(Trim(posItem.Identifier)) + "'"
                    foundRows = ItemDataset.Tables("tblItemDataset").Select(strSql)
                    If Not (foundRows.Length = 0) Then

                        Try
                            For i = 0 To foundRows.GetUpperBound(0)
                                posItem.ItemKey = CInt(foundRows(i)(0))
                                posItem.Identifier = CStr(foundRows(i)(1))
                            Next i
                        Catch

                        End Try
                    Else

                        posItem.ItemKey = -1
                    End If

                    FileDate = FileDateTime(Me.SourceNCRPathFileName)
                    sDateTime = String.Format("{0} {1}:{2}", FileDate.ToString("M/dd/yyyy"), FileDate.Hour.ToString(), Now.Minute.ToString())

                    ' write this posItem to the file
                    PrintLine(TARGET, posItem.ToNCRFileString(StoreNo, sDateTime))

                    'clear out posItem object data
                    posItem.ClearPropertyValues()
                End While
                fileReader.Close()
                FileClose(TARGET)

                ' copy the target file to the database server
                FileCopy(Me.LocalTargetFile, Me.CopyTargetFile)

            Finally
                ' Let the CATCH bubble up - don't catch here, just clean up in the finally
                ItemDataset = Nothing
                fileReader.Close()
                fileReader = Nothing
                FileClose(TARGET)
            End Try
        End Sub

        Private Sub CreateAuditInfoFile(ByVal Store_No As Integer, ByVal FullPath As String, ByVal results As SqlDataReader)
            Dim FileRecord As String

            _POSAuditTextFileWriter = New StreamWriter(FullPath, False, System.Text.Encoding.UTF8)

            While (results.Read())
                FileRecord = results.GetInt32(results.GetOrdinal("Store_No")).ToString() + _POSAuditExceptionFileDelimiter + _
                             results.GetString(results.GetOrdinal("Identifier")) + _POSAuditExceptionFileDelimiter + _
                             results.GetString(results.GetOrdinal("Item_Description")) + _POSAuditExceptionFileDelimiter + _
                             results.GetString(results.GetOrdinal("SubTeam_Name")) + _POSAuditExceptionFileDelimiter + _
                             results.GetString(results.GetOrdinal("Unit_Abbreviation")) + _POSAuditExceptionFileDelimiter + _
                             results.GetInt32(results.GetOrdinal("ExceptionTypeID")).ToString() + _POSAuditExceptionFileDelimiter

                _POSAuditTextFileWriter.WriteLine(FileRecord)
            End While

            _POSAuditTextFileWriter.Close()
            _POSAuditTextFileWriter = Nothing
        End Sub

        Private Sub CreateAuditExcelFile(ByVal Store_No As Integer, ByVal FullPath As String, ByVal results As SqlDataReader)
            Dim excelApp As New ExcelInterop.Application()
            Dim workbook As ExcelInterop.Workbook = excelApp.Workbooks.Add()
            Dim workSheet As ExcelInterop.Worksheet
            Dim columnNames As String() = EXCEPTION_RESOLUTION_FLIE_HEADERS.Split(CType(",", Char))
            Dim rowIndex As Integer = 0

            If workbook.Sheets.Count = 0 Then
                workSheet = CType(workbook.Sheets.Add(), ExcelInterop.Worksheet)
            Else
                workSheet = CType(workbook.Worksheets(1), ExcelInterop.Worksheet)
            End If

            ' Settings
            workSheet.Cells(1, 1) = "Upload Types:"
            workSheet.Cells(1, 2) = "ITEM_MAINTENANCE"
            workSheet.Cells(1, 3) = "PRICE_UPLOAD"

            ' Column Headings
            For i As Integer = 0 To columnNames.Length - 1
                workSheet.Cells(2, i + 1) = columnNames(i)
            Next

            While results.Read()
                For columnIndex As Integer = 0 To results.FieldCount - 1
                    workSheet.Cells(rowIndex + 3, columnIndex + 1) = results(columnIndex)
                Next

                rowIndex += 1
            End While

            workbook.SaveAs(FullPath)
            workbook.Close()

        End Sub

        Public Sub POSAudit(ByVal Store_No As Integer)
            Dim InfoFileFullPath As String
            Dim ExcelFileFullPath As String
            Dim ftpClient As WholeFoods.Utility.FTP.FTPclient = Nothing

            Try
                ' Execute the stored procedure and write the results to an exception file

                If Store_No <> -1 Then
                    ' Insert an indication of the specified store before the file extension
                    _POSAuditExceptionFileName = _POSAuditExceptionFileName.Insert(_POSAuditExceptionFileName.LastIndexOf(CType(".", Char)), String.Format("_S{0}", Store_No))
                    _POSAuditExceptionsResolutionFileName = _POSAuditExceptionsResolutionFileName.Insert(_POSAuditExceptionsResolutionFileName.LastIndexOf(CType(".", Char)), String.Format("_S{0}", Store_No))
                End If

                InfoFileFullPath = String.Format("{0}{1}", _POSPullLocalDir, _POSAuditExceptionFileName)
                ExcelFileFullPath = String.Format("{0}{1}", _POSPullLocalDir, _POSAuditExceptionsResolutionFileName)

                If File.Exists(InfoFileFullPath) Then
                    File.Delete(InfoFileFullPath)
                End If

                If File.Exists(ExcelFileFullPath) Then
                    File.Delete(ExcelFileFullPath)
                End If

                Dim POSPullExceptionFileFlag As Boolean = False
                POSPullExceptionFileFlag = InstanceDataDAO.IsFlagActive("GeneratePOSPullExceptionFile")

                Using results As SqlDataReader = POSPullDAO.GetPOSAuditExceptions(Store_No)
                    CreateAuditInfoFile(Store_No, InfoFileFullPath, results)

                    results.NextResult()

                    If (POSPullExceptionFileFlag = True) Then
                        Dim count As Long = POSPullDAO.GetNoOfPOSExceptions(Store_No)

                        If (count < 65535) Then
                            CreateAuditExcelFile(Store_No, ExcelFileFullPath, results)
                        Else
                            _POSAuditExceptionStatus = 1
                            Logger.LogDebug("Audit Exception file is too large. Can not create Excel file...", Me.GetType)
                        End If
                    End If
                End Using

                ftpClient = New WholeFoods.Utility.FTP.FTPclient(_POSAuditFileTargetServer, _POSAuditFileFTPUser, _POSAuditFileFTPPwd, False)

                ftpClient.Upload(InfoFileFullPath, _POSAuditFileTargetFolder + "\" + _POSAuditExceptionFileName, False)

                If Not (POSPullExceptionFileFlag = False Or _POSAuditExceptionStatus = 1) Then
                    ftpClient.Upload(ExcelFileFullPath, _POSAuditFileTargetFolder + "\" + _POSAuditExceptionsResolutionFileName, False)
                End If

            Catch e As Exception
                Throw
            Finally
                If (_POSAuditTextFileWriter IsNot Nothing) Then
                    _POSAuditTextFileWriter.Close()
                End If
            End Try

        End Sub

#End Region

#Region "Constructor"

        Public Sub New(ByVal storeNumber As Integer)
            Dim storeFtpConfigDAO As New StoreFTPConfigDAO

            If storeNumber = -1 Then
                ' Get ftp info for all stores
                FTPInfo = storeFtpConfigDAO.GetFTPConfigDataForWriterType(Constants.FileWriterType_POSPULL)
            Else
                Dim oneStore As StoreFTPConfigBO
                ' Get ftp info for just the one store
                oneStore = storeFtpConfigDAO.GetFTPConfigDataForStoreAndWriterType(storeNumber, Constants.FileWriterType_POSPULL)
                FTPInfo.Add(storeNumber, oneStore)
            End If

            ' Get the IBM directory\filenames from the config file
            _sourceIBMFileName = String.Format("{0}", ConfigurationServices.AppSettings("POSPullIBMFileName"))
            _sourceIBMPathFileName = String.Format("{0}{1}", _
                    ConfigurationServices.AppSettings("POSPullLocalDir"), _
                    ConfigurationServices.AppSettings("POSPullIBMFileName"))

            ' Get the NCR directory\filenames from the config file
            _sourceNCRFileName = String.Format("{0}", ConfigurationServices.AppSettings("POSPullNCRFileName"))
            _sourceNCRPathFileName = String.Format("{0}{1}", _
                    ConfigurationServices.AppSettings("POSPullLocalDir"), _
                    ConfigurationServices.AppSettings("POSPullNCRFileName"))

            ' The target directories (local and server) and filename are the same
            ' for both IBM and NCR
            _localTargetFile = String.Format("{0}{1}", _
                    ConfigurationServices.AppSettings("POSPullLocalDir"), _
                    ConfigurationServices.AppSettings("POSPullTargetFileName"))
            _copyTargetFile = String.Format("{0}{1}", _
                ConfigurationServices.AppSettings("POSPullTargetDir"), _
                ConfigurationServices.AppSettings("POSPullTargetFileName"))

            _DBServerSourceFile = String.Format("{0}{1}", _
                ConfigurationServices.AppSettings("POSPullDBServerSourceDir"), _
                ConfigurationServices.AppSettings("POSPullTargetFileName"))

            _POSPullLocalDir = ConfigurationServices.AppSettings("POSPullLocalDir")
            _POSAuditExceptionFileName = String.Format("{0}", ConfigurationServices.AppSettings("POSAuditExceptionsFileName"))
            _POSAuditExceptionsResolutionFileName = ConfigurationServices.AppSettings("POSAuditExceptionResolutionFileName")

            _POSAuditExceptionFileDelimiter = String.Format("{0}", ConfigurationServices.AppSettings("POSAuditExceptionsFileDelimiter"))

            _POSAuditFileTargetServer = ConfigurationServices.AppSettings("POSAuditFileTargetServer")
            _POSAuditFileTargetFolder = ConfigurationServices.AppSettings("POSAuditFileTargetFolder")
            _POSAuditFileFTPUser = ConfigurationServices.AppSettings("POSAuditFileFTPUser")
            _POSAuditFileFTPPwd = ConfigurationServices.AppSettings("POSAuditFileFTPPwd")

        End Sub

#End Region

    End Class
End Namespace
