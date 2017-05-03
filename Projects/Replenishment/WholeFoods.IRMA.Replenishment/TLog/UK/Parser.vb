Imports System.IO
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports IRMA.TLog

Namespace WholeFoods.IRMA.Replenishment.TLog


    Public Class Parser
        Implements TlogParserInterface, IDisposable

        Private _FileData As List(Of String) = New List(Of String)
        Private _Transactions As List(Of Transaction) = New List(Of Transaction)
        Private _Voids As List(Of VoidItem) = New List(Of VoidItem)
        Private _MiscRecords As List(Of String) = New List(Of String)
        Private _DateList As List(Of String) = New List(Of String)

        ' // Required Events //
        Private Event Notify(ByVal msg As String) Implements TlogParserInterface.Notfiy
        Private Event UpdateProgress(ByVal value As Integer, ByVal max As Integer) Implements TlogParserInterface.UpdateProgress
        Private Event Finished() Implements TlogParserInterface.Finished
        Private Event Start() Implements TlogParserInterface.Start
        Private Event Failure(ByVal ErrMsg As String, ByVal InnerMsg As String) Implements TlogParserInterface.Failure

        Public Sub New()

        End Sub

        Public Sub ParseDataFromMemoryStream(ByRef Buffer As Byte()) Implements TlogParserInterface.ParseDataFromMemoryStream
            Dim text As String = System.Text.Encoding.Default.GetString(Buffer)
            _FileData.Clear()
            _FileData.AddRange(text.Split(vbCrLf.ToCharArray))
            ParseData()
        End Sub

        Public Sub ParseDataFromFile(ByVal DataFile As String) Implements TlogParserInterface.ParseDataFromFile
            _FileData.Clear()
            Dim FileExists As Boolean = False
            Try
                FileExists = File.Exists(DataFile)
            Catch ex As Exception
                Throw New Exception("FILE EXISTS: " & ex.Message)
            End Try


            If FileExists Then
                _FileData.AddRange(File.ReadAllLines(DataFile))
            Else
                Throw New FileNotFoundException(DataFile & " cannot be found.")
            End If
            ParseData()
        End Sub
        Private Sub ParseData()

            Dim RecordItendifier As String
            'Dim Filedate As DateTime = DateTime.Now
            ' Dim DataFileInfo As FileInfo = New FileInfo(DataFile)
            Dim cnt As Integer = 0

            ' Holds the Current Transaction Information while processing the file.
            Dim CurrentTransaction As Transaction = New Transaction()

            ' Holds the Current Store and Register Information while building the current
            ' transaction. 
            Dim CurrentStore As Integer = 0
            Dim CurrentRegister As Integer
            Dim CurrentOperator As Integer
            Dim PaymentCount As Integer = 0
            Dim LastItem As String = String.Empty

            _Transactions.Clear()
            _MiscRecords.Clear()
            RaiseEvent Start()
            'Read data file into memory. 
            Dim factory As New DataFactory(DataFactory.ItemCatalog, DataFactory.ItemCatalog)

            Try

                ' Iterate through each line item in the data file and process.
                RaiseEvent Notify("Read " & _FileData.Count.ToString() & " Line(s).")
                RaiseEvent Notify("Parsing...")
                RaiseEvent UpdateProgress(0, _FileData.Count)
                For Each item As String In _FileData
                    cnt += 1
                    If item.Length > 0 Then

                        RecordItendifier = item.Substring(0, 1)
                        Select Case RecordItendifier
                            Case "A"

                                'Transaction Summary Record
                                'Found at the end of a transaction. Start a new transaction after
                                'parsing this record.

                                ' reset payment count
                                PaymentCount = 0
                                'parse current transaction  number and times.
                                CurrentTransaction.TransactionNumber = CType(item.Substring(17, 6), Integer)
                                CurrentTransaction.StartTime = ParseTime(item.Substring(27, 6))
                                CurrentTransaction.TenderTime = ParseTime(item.Substring(33, 6))
                                CurrentTransaction.EndTime = ParseTime(item.Substring(39, 6))
                                CurrentTransaction.TransactionDate = ParseDate(item.Substring(1, 8))
                                CurrentTransaction.TransactionValue = CType(item.Substring(45, 8), Single) / 100


                                'add current date to a list of date included in the tlog file.
                                'used for updating aggregates later.
                                If Not _DateList.Contains(CDate(CurrentTransaction.TransactionDate).ToShortDateString()) Then
                                    _DateList.Add(CDate(CurrentTransaction.TransactionDate).ToShortDateString())
                                End If

                                'add current transaction to collection
                                _Transactions.Add(CurrentTransaction)

                                'free memory used by the currenttransaction.
                                CurrentTransaction.Dispose()

                                'Begin a new transaction and set Current Store and Register Info
                                CurrentTransaction = New Transaction()
                                CurrentTransaction.StoreNumber = CurrentStore
                                CurrentTransaction.RegisterNumber = CurrentRegister
                                CurrentTransaction.OperatorNumber = CurrentOperator

                            Case "B", "R", "U"      'Item Sales record
                                CurrentTransaction.AddItem(item, cnt)
                                LastItem = item
                            Case "C"                ' Charge/Tender record
                                PaymentCount += 1
                                CurrentTransaction.AddPayment(item, PaymentCount)
                            Case "D"                'Discount Record
                                CurrentTransaction.AddDiscount(item)
                            Case "F"                'Line Discount 
                                CurrentTransaction.AddDiscount(item)
                            Case "H"                'Sign On/Off
                                If item.Substring(1, 3) = "ON " Then
                                    'Sign On record
                                    Try
                                        ' test validity of HON record format. If valis use new value.
                                        ' if not valid use previous values.
                                        Dim TempStore As Integer
                                        Dim TempRegister As Integer
                                        Dim TempOperator As Integer

                                        TempStore = CType(item.Substring(12, 6), Integer)
                                        TempRegister = CType(item.Substring(18, 2), Integer)
                                        TempOperator = CType(item.Substring(26, 4), Integer)

                                        CurrentStore = CType(item.Substring(12, 6), Integer)
                                        CurrentRegister = CType(item.Substring(18, 2), Integer)
                                        CurrentOperator = CType(item.Substring(26, 4), Integer)

                                    Catch ex As Exception
                                        RaiseEvent Notify("Invalid Login (HON) record found.")
                                        RaiseEvent Notify("Using Previous Values. Store: " & CurrentStore.ToString() & " Register: " & CurrentRegister.ToString() & " Operator: " & CurrentOperator.ToString())
                                    End Try

                                    CurrentTransaction.StoreNumber = CurrentStore
                                    CurrentTransaction.RegisterNumber = CurrentRegister
                                    CurrentTransaction.OperatorNumber = CurrentOperator
                                Else
                                    'Sign Off record
                                End If
                            Case "M"
                                'Special offer discount
                                'Item sold at a special offer price.
                                CurrentTransaction.AddOffer(item, LastItem)
                                'Case "N"                'NO Sale record
                                'Case "Q"                'Check/Card Details
                                'Case "T"                'Tender Change record
                            Case "V"
                                'Void Transaction
                                'Entire Transaction was cancelled at the register. Remove from collection.
                                '_Transactions.RemoveAt(_Transactions.Count - 1)
                                _Voids.Add(New VoidItem(item))
                            Case Else
                                _MiscRecords.Add(item)
                        End Select


                    End If
                    RaiseEvent UpdateProgress(cnt, _FileData.Count)
                Next
                If CurrentTransaction IsNot DBNull.Value Then CurrentTransaction.Dispose()

                RaiseEvent Notify("Found " & _Transactions.Count.ToString() & " Transaction(s).")
                RaiseEvent Notify("Importing into IRMA...")
                cnt = 0
                If Not _IsDebug Then
                    ClearLoadingTables(factory)
                End If

                RaiseEvent UpdateProgress(0, _Transactions.Count)
                For Each item As Transaction In _Transactions
                    cnt += 1
                    ProcessTransaction(factory, item)
                    RaiseEvent UpdateProgress(cnt, _Transactions.Count)
                Next

                For Each item As VoidItem In _Voids
                    ProcessVoid(factory, item)
                Next
                RaiseEvent Notify("Updating Aggregates...")
                UpdateAggregates(factory, _Transactions)
            Catch Ex As Exception
                RaiseEvent Failure(Ex.Message, Ex.InnerException.Message)
            Finally
                RaiseEvent Finished()
            End Try


        End Sub

        Private Sub ClearLoadingTables(ByRef dbconnection As DataFactory)
            Try
                dbconnection.ExecuteStoredProcedure("Replenishment_Tlog_Uk_ClearLoadingTables")
            Catch e As Exception
                Throw New Exception("Could not clear loading tables: " & e.Message)
            End Try

        End Sub
        Private Sub UpdateAggregates(ByRef dbconnection As DataFactory, ByRef Transactions As List(Of Transaction))
            Dim paramList As ArrayList = New ArrayList
            Dim currentParam As DBParam
            Dim DateString As String = String.Empty
            For Each store As Integer In GetStoreNumbers()

                For Each DateString In _DateList

                    paramList.Clear()

                    currentParam = New DBParam
                    currentParam.Name = "Date"
                    currentParam.Value = DateString
                    currentParam.Type = DBParamType.DateTime
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Store_No"
                    currentParam.Value = store
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    Try
                        RaiseEvent Notify("Updating Aggregates for Store_No: " & store.ToString & " Date: " & DateString)
                        dbconnection.ExecuteStoredProcedure("Replenishment_Tlog_Uk_UpdateSalesAggregates", paramList)
                    Catch e As Exception
                        Dim Msg As String
                        Msg = e.Message
                        Throw New Exception("Update Aggregates failed: " & Msg)
                    End Try
                Next
            Next
        End Sub

        Private Sub ProcessVoid(ByRef dbconnection As DataFactory, ByRef CurrentVoid As VoidItem)
            CreateVoidRecord(dbconnection, CurrentVoid)
        End Sub

        Private Sub ProcessTransaction(ByRef dbconnection As DataFactory, ByRef CurrentTransaction As Transaction)

            CreateTransactionRecord(dbconnection, CurrentTransaction)
            CreateItemRecords(dbconnection, CurrentTransaction)
            CreateDiscountRecords(dbconnection, CurrentTransaction)
            CreatePaymentRecords(dbconnection, CurrentTransaction)
            CreateOfferRecords(dbconnection, CurrentTransaction)


        End Sub

        Private Sub CreateVoidRecord(ByRef dbconnection As DataFactory, ByRef CurrentVoid As VoidItem)
            Dim paramlist As ArrayList = New ArrayList
            Dim currentParam As DBParam

            paramlist.Clear()
            currentParam = New DBParam
            currentParam.Name = "TimeKey"
            currentParam.Value = DateTime.Parse(ParseDate(CurrentVoid.VoidDate) & " " & ParseTime(CurrentVoid.VoidTime))
            currentParam.Type = DBParamType.DateTime
            paramlist.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = CurrentVoid.StoreNumber
            currentParam.Type = DBParamType.Int
            paramlist.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Register_No"
            currentParam.Value = CurrentVoid.RegisterNumber
            currentParam.Type = DBParamType.Int
            paramlist.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Transaction_No"
            currentParam.Value = CurrentVoid.TransactionNumber
            currentParam.Type = DBParamType.Int
            paramlist.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Operator_No"
            currentParam.Value = CurrentVoid.OperatorNumber
            currentParam.Type = DBParamType.Int
            paramlist.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Sales_Value"
            currentParam.Value = CurrentVoid.SalesValue
            currentParam.Type = DBParamType.Money
            paramlist.Add(currentParam)
            Try
                dbconnection.ExecuteStoredProcedure("Replenishment_TLog_UK_CreateVoidRecord", paramlist)
            Catch ex As Exception
                Debug.WriteLine(ex.Message & vbCrLf & ex.InnerException.Message)
            End Try
        End Sub

        Private Function GetStoreNumbers() As ArrayList
            Dim retval As ArrayList = New ArrayList

            For Each item As Transaction In _Transactions
                If Not retval.Contains(item.StoreNumber) Then
                    retval.Add(item.StoreNumber)
                End If
            Next

            Return retval
        End Function


        Private Sub CreateOfferRecords(ByRef dbconnection As DataFactory, ByRef CurrentTransaction As Transaction)
            Dim paramList As ArrayList = New ArrayList
            Dim currentParam As DBParam


            For Each item As OfferItem In CurrentTransaction.OfferItems
                paramList.Clear()
                currentParam = New DBParam
                currentParam.Name = "TimeKey"
                currentParam.Value = Date.Parse(CurrentTransaction.TransactionDate.ToShortDateString() & " " & CurrentTransaction.EndTime.ToShortTimeString())
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TransactionNo"
                currentParam.Value = CurrentTransaction.TransactionNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreNo"
                currentParam.Value = CurrentTransaction.StoreNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RegisterNo"
                currentParam.Value = CurrentTransaction.RegisterNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Barcode"
                currentParam.Value = item.ItemBarcode
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Offer_Quantity"
                currentParam.Value = item.OfferQty
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Offer_Amount"
                currentParam.Value = item.OfferDiscount
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Table_Number"
                currentParam.Value = item.TableNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Offer_Description"
                currentParam.Value = item.OfferDescription
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Offer_Reference"
                currentParam.Value = item.OfferReference
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
                Try
                    dbconnection.ExecuteStoredProcedure("Replenishment_TLog_UK_CreateOfferRecord", paramList)
                Catch ex As Exception
                    Debug.WriteLine(ex.Message & vbCrLf & ex.InnerException.Message)
                End Try



            Next
        End Sub
        Private Sub CreatePaymentRecords(ByRef dbconnection As DataFactory, ByRef CurrentTransaction As Transaction)
            Dim paramList As ArrayList = New ArrayList
            Dim currentParam As DBParam

            For Each item As PaymentItem In CurrentTransaction.PaymentItems
                If item.PaymentAmount <> 0 Then
                    paramList.Clear()

                    currentParam = New DBParam
                    currentParam.Name = "TimeKey"
                    currentParam.Value = Date.Parse(CurrentTransaction.TransactionDate.ToShortDateString() & " " & CurrentTransaction.EndTime.ToShortTimeString())
                    currentParam.Type = DBParamType.DateTime
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "TransactionNo"
                    currentParam.Value = CurrentTransaction.TransactionNumber
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "StoreNo"
                    currentParam.Value = CurrentTransaction.StoreNumber
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "RegisterNo"
                    currentParam.Value = CurrentTransaction.RegisterNumber
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "PaymentType"
                    currentParam.Value = item.PaymentType
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "PaymentAmt"
                    currentParam.Value = item.PaymentAmount
                    currentParam.Type = DBParamType.Money
                    paramList.Add(currentParam)
                    Try
                        dbconnection.ExecuteStoredProcedure("Replenishment_TLog_UK_CreatePaymentRecord", paramList)
                    Catch ex As Exception
                        Debug.WriteLine(ex.Message & vbCrLf & ex.InnerException.Message)
                    End Try


                End If
            Next

        End Sub

        Private Sub CreateItemRecords(ByRef dbconnection As DataFactory, ByRef CurrentTransaction As Transaction)
            Dim paramList As ArrayList = New ArrayList
            Dim currentParam As DBParam

            For Each item As TransactionItem In CurrentTransaction.TransactionItems
                paramList.Clear()

                currentParam = New DBParam
                currentParam.Name = "TimeKey"
                currentParam.Value = Date.Parse(CurrentTransaction.TransactionDate.ToShortDateString() & " " & CurrentTransaction.EndTime.ToShortTimeString())
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TransactionNo"
                currentParam.Value = CurrentTransaction.TransactionNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreNo"
                currentParam.Value = CurrentTransaction.StoreNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RegisterNo"
                currentParam.Value = CurrentTransaction.RegisterNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SalesQty"
                currentParam.Value = item.SalesQty
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Weight"
                currentParam.Value = item.Weight
                currentParam.Type = DBParamType.Float
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "SalesAmt"
                currentParam.Value = item.Salesvalue
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Value = item.BarCode
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Dept_No"
                currentParam.Value = item.DepartmentNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "VatCode"
                currentParam.Value = item.VatCode
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Turnover_Dept"
                currentParam.Value = item.TurnoverDepartment
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Row_No"
                currentParam.Value = item.RowNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Retail_Price"
                currentParam.Value = item.RetailPrice
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Trans_Type"
                currentParam.Value = item.TransType
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)


                Try
                    dbconnection.ExecuteStoredProcedure("Replenishment_TLog_UK_CreateItemRecord", paramList)
                Catch ex As Exception
                    Debug.WriteLine(ex.Message & vbCrLf & ex.InnerException.Message)
                End Try


            Next
        End Sub

        Private Sub CreateDiscountRecords(ByRef dbconnection As DataFactory, ByRef CurrentTransaction As Transaction)
            Dim paramList As ArrayList = New ArrayList
            Dim currentParam As DBParam

            For Each item As DiscountItem In CurrentTransaction.DiscountItems
                paramList.Clear()

                currentParam = New DBParam
                currentParam.Name = "TimeKey"
                currentParam.Value = Date.Parse(CurrentTransaction.TransactionDate.ToShortDateString() & " " & CurrentTransaction.EndTime.ToShortTimeString())
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TransactionNo"
                currentParam.Value = CurrentTransaction.TransactionNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreNo"
                currentParam.Value = CurrentTransaction.StoreNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RegisterNo"
                currentParam.Value = CurrentTransaction.RegisterNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DiscountAmt"
                currentParam.Value = item.DiscountAmount
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DiscountReference"
                currentParam.Value = Trim(item.DiscountReference)
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DiscountReason"
                currentParam.Value = Trim(item.DiscountReason)
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Barcode"
                currentParam.Value = item.DiscountBarcode
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "DiscountIdentifier"
                currentParam.Value = item.DiscountIdentifier
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)
                Try
                    dbconnection.ExecuteStoredProcedure("Replenishment_TLog_UK_CreateDiscountRecord", paramList)
                Catch ex As Exception
                    Debug.WriteLine(ex.Message & vbCrLf & ex.InnerException.Message)
                End Try

            Next
        End Sub

        ''' <summary>
        ''' Inserts a record into TLog_UK_Transaction and returns the Identity value.
        ''' </summary>
        ''' <param name="dbconnection">Datafactory object representing and open connection to the database</param>
        ''' <param name="CurrentTransaction">Transaction object to generate a Transaction record for.</param>


        Private Sub CreateTransactionRecord(ByRef dbconnection As DataFactory, ByRef CurrentTransaction As Transaction)
            Dim paramList As ArrayList = New ArrayList
            Dim currentParam As DBParam
            Dim returnIndex As Integer
            Dim outputList As ArrayList

            paramList.Clear()
            currentParam = New DBParam
            currentParam.Name = "TimeKey"
            currentParam.Value = Date.Parse(CurrentTransaction.TransactionDate.ToShortDateString() & " " & CurrentTransaction.EndTime.ToShortTimeString())
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TransactionNo"
            currentParam.Value = CurrentTransaction.TransactionNumber
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StoreNo"
            currentParam.Value = CurrentTransaction.StoreNumber
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RegisterNo"
            currentParam.Value = CurrentTransaction.RegisterNumber
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OperatorNo"
            currentParam.Value = CurrentTransaction.OperatorNumber
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TransactionDate"
            currentParam.Value = CurrentTransaction.TransactionDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "StartTime"
            currentParam.Value = Date.Parse(CurrentTransaction.TransactionDate.ToShortDateString() & " " & CurrentTransaction.StartTime.ToShortTimeString())
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TenderTime"
            currentParam.Value = Date.Parse(CurrentTransaction.TransactionDate.ToShortDateString() & " " & CurrentTransaction.TenderTime.ToShortTimeString())
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EndTime"
            currentParam.Value = Date.Parse(CurrentTransaction.TransactionDate.ToShortDateString() & " " & CurrentTransaction.EndTime.ToShortTimeString())
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemCount"
            currentParam.Value = CurrentTransaction.TransactionItems.Count
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TransactionAmount"
            currentParam.Value = CurrentTransaction.TransactionValue
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Voided"
            currentParam.Value = IIf(CurrentTransaction.IsVoided, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "TransactionId"
            currentParam.Type = DBParamType.Int
            returnIndex = paramList.Add(currentParam)

            Try

                outputList = dbconnection.ExecuteStoredProcedure("Replenishment_TLog_UK_CreateTransactionRecord", paramList)
                CurrentTransaction.TransactionId = CType(outputList(0), Integer)
            Catch ex As Exception
                Debug.WriteLine(ex.Message & vbCrLf & ex.InnerException.Message)
            End Try



        End Sub

#Region "obsolete code, commented out."
        'Private Sub GenerateSalesfacts(ByRef dbconnection As DataFactory, ByRef CurrentTransaction As Transaction)
        '    Dim paramList As ArrayList = New ArrayList
        '    Dim currentParam As DBParam

        '    For Each item As TransactionItem In CurrentTransaction.TransactionItems
        '        paramList.Clear()
        '        currentParam = New DBParam
        '        currentParam.Name = "DateKey"
        '        currentParam.Value = Date.Parse(CurrentTransaction.TransactionDate.ToShortDateString() & " " & CurrentTransaction.EndTime.ToShortTimeString())
        '        currentParam.Type = DBParamType.DateTime
        '        paramList.Add(currentParam)
        '        currentParam = New DBParam

        '        currentParam.Name = "StoreNo"
        '        currentParam.Value = CurrentTransaction.StoreNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "TransactionNo"
        '        currentParam.Value = CurrentTransaction.TransactionNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "RegisterNo"
        '        currentParam.Value = CurrentTransaction.RegisterNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "RowNo"
        '        currentParam.Value = item.RowNo
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "CashierId"
        '        currentParam.Value = CurrentTransaction.OperatorNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "SalesQty"
        '        currentParam.Value = item.SalesQty
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "Weight"
        '        currentParam.Value = item.Weight
        '        currentParam.Type = DBParamType.Decimal

        '        paramList.Add(currentParam)
        '        currentParam = New DBParam
        '        currentParam.Name = "SalesAmt"
        '        currentParam.Value = item.SalesQty
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)


        '    Next

        'End Sub


        'Private Sub GeneratePaymentFacts(ByRef DBConnection As DataFactory, ByVal CurrentTransaction As Transaction)
        '    Dim paramList As ArrayList = New ArrayList
        '    Dim currentParam As DBParam

        '    For Each item As PaymentItem In CurrentTransaction.PaymentItems

        '        paramList.Clear()
        '        currentParam = New DBParam
        '        currentParam.Name = "TimeKey"
        '        currentParam.Value = Date.Parse(CurrentTransaction.TransactionDate.ToShortDateString() & " " & CurrentTransaction.EndTime.ToShortTimeString())
        '        currentParam.Type = DBParamType.DateTime
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "StoreNo"
        '        currentParam.Value = CurrentTransaction.StoreNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "TransactionNo"
        '        currentParam.Value = CurrentTransaction.TransactionNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "RegisterNo"
        '        currentParam.Value = CurrentTransaction.RegisterNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "OperatorNo"
        '        currentParam.Value = CurrentTransaction.OperatorNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "RowNo"
        '        currentParam.Value = CurrentTransaction.PaymentItems.IndexOf(item) + 1
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "PaymentType"
        '        currentParam.Value = item.PaymentType
        '        currentParam.Type = DBParamType.String
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "PaymentAmount"
        '        currentParam.Value = item.PaymentAmount
        '        currentParam.Type = DBParamType.Money
        '        paramList.Add(currentParam)

        '        dbconnection.ExecuteStoredProcedure("Replenishment_UKTLog_CreatePaymentFact", paramList)

        '    Next
        'End Sub
        'Private Sub GenerateItemFact(ByRef DBConnection As DataFactory, ByVal CurrentTransaction As Transaction)

        '    ' Insert a record into the IRMA_TLog_ItemFact Table
        '    Dim paramList As ArrayList = New ArrayList
        '    Dim currentParam As DBParam


        '    For Each item As TransactionItem In CurrentTransaction.TransactionItems
        '        paramList.Clear()
        '        currentParam = New DBParam
        '        currentParam.Name = "DateKey"
        '        currentParam.Value = Date.Parse(CurrentTransaction.TransactionDate.ToShortDateString() & " " & CurrentTransaction.EndTime.ToShortTimeString())
        '        currentParam.Type = DBParamType.DateTime
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "StoreNo"
        '        currentParam.Value = CurrentTransaction.StoreNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "RegisterNo"
        '        currentParam.Value = CurrentTransaction.RegisterNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "TransactionNo"
        '        currentParam.Value = CurrentTransaction.TransactionNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "OperatorNo"
        '        currentParam.Value = CurrentTransaction.OperatorNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "SubTeamNo"
        '        currentParam.Value = item.DepartmentNumber
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)
        '        currentParam = New DBParam

        '        currentParam.Name = "BarCode"
        '        currentParam.Value = item.BarCode
        '        currentParam.Type = DBParamType.String
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "SalesQty"
        '        currentParam.Value = item.SalesQty
        '        currentParam.Type = DBParamType.Int
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "RetailPrice"
        '        currentParam.Value = item.RetailPrice
        '        currentParam.Type = DBParamType.Money
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "Weight"
        '        currentParam.Value = item.Weight
        '        currentParam.Type = DBParamType.Money
        '        paramList.Add(currentParam)

        '        currentParam = New DBParam
        '        currentParam.Name = "SalesValue"
        '        currentParam.Value = item.Salesvalue
        '        currentParam.Type = DBParamType.Money
        '        paramList.Add(currentParam)

        '        DBConnection.ExecuteStoredProcedure("Replenishment_TLog_CreateItemFact", paramList)

        '    Next



        'End Sub
#End Region

        Private Function ParseTime(ByVal t As String) As DateTime
            Return CDate(t.Substring(0, 2) & ":" & t.Substring(2, 2) & ":" & t.Substring(4, 2))
        End Function
        Private Function ParseDate(ByVal d As String) As DateTime
            Return CDate(d.Substring(4, 2) & "/" & d.Substring(6, 2) & "/" & d.Substring(0, 4))
        End Function


        


        Private _IsDebug As Boolean
        Public Property IsDebug() As Boolean Implements TlogParserInterface.IsDebug
            Get
                Return _IsDebug
            End Get
            Set(ByVal value As Boolean)
                _IsDebug = value
            End Set
        End Property

        



        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free unmanaged resources when explicitly called
                End If

                ' TODO: free shared unmanaged resources
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose, TlogParserInterface.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class


End Namespace