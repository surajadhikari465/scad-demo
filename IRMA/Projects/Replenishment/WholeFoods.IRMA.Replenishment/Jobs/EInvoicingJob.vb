Imports log4net
Imports System.Xml
Imports System.IO
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Replenishment.EInvoicing.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess

Public Class EInvoicingJob

    ' This class (and all other classes related to the EInvoicingJob) is duplicated in the IRMA Service Library solution (IRMA/IRMA Service Library/Service Library.sln).
    ' Any changes made here to the EInvoicing job also need to made in the Service Library.


    Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Public Sub New()
        _InvoiceNumbers = New List(Of String)
    End Sub

    Public Sub LoadEInvoicingDataFromString(ByVal _xml As String)

        '#################################################################################################################################
        '# If the 'string' came from a file it will arleady have the <invoiceList></invoiceList> tags. We can load it as is.             #
        '# If the 'string' came from an already parsed XML field in the DB, the <invoiceList></invoiceList> tags have been removed.      #
        '# We will need to add them back in so the XML parsing will work correctly.                                                      #
        '#################################################################################################################################

        Dim xmld As XmlDocument = New XmlDocument
        If _xml.Contains("</invoiceList>") Then
            xmld.LoadXml(_xml)
        Else
            xmld.LoadXml("<invoiceList>" & _xml & "</invoiceList>")
        End If

        _XMLData = xmld
    End Sub

    Public Sub ClearEInvoiceData(ByVal _EInvoiceId As Nullable(Of Integer))

        Dim DAO As EInvoicingDAO = New EInvoicingDAO
        DAO.ClearEInvoicedata(_EInvoiceId)
        DAO.Dispose()
    End Sub

    Public Function GetEInvoiceXML(ByVal _EinvoiceId As Integer) As String
        Dim DAO As EInvoicingDAO = New EInvoicingDAO
        Dim retval As String = String.Empty

        retval = DAO.getEInoviceXML(_EinvoiceId)

        Return retval

    End Function

    Public Function LoadEInvoicingData(ByVal _datafile As String) As String

        Dim fi As FileInfo = New FileInfo(_datafile)
        Dim retval As String = String.Empty
        Try
            logger.Info(String.Format("Loading {0}", fi.Name))
            Dim xmld As XmlDocument = New XmlDocument
            If Not File.Exists(_datafile) Then
                Throw New Exception(String.Format("LoadEInvoicingData(): {0} does not exist.", _datafile))
            End If

            Try
                xmld.Load(_datafile)
                _XMLData = xmld
            Catch ex As Exception
                Throw New Exception(String.Format("LoadEInvoicingData(): Invalid XML [ {0} ]", ex.Message))
            End Try
        Catch ex As Exception
            logger.Error(ex.Message)
            If File.Exists(_datafile) Then
                'move to bad files directory
                If Not Directory.Exists(".\Errors") Then
                    Directory.CreateDirectory(".\Errors")
                End If

                File.Move(_datafile, Path.Combine(".\Errors\", fi.Name))
                logger.Warn(String.Format("{0} moved to .\Errors directory", _datafile))

            End If

            Throw ex


        End Try

        Return retval

    End Function

    Public Sub ParseXMLElements(ByVal _node As XmlNode, ByRef hash As Hashtable)
        For Each _child As XmlNode In _node.ChildNodes
            If Not _child.Name.Equals("lineItems") Then
                Select Case _child.ChildNodes.Count
                    Case 0
                        hash.Add(_child.Name, String.Empty)
                    Case 1
                        hash.Add(_child.Name, _child.InnerText)
                End Select
            End If
        Next

    End Sub
    Public Sub ParseInvoicesFromXML(ByVal xmld As XmlDocument, ByVal EInvoiceId As Nullable(Of Integer), Optional ByVal ReparsedPONum As String = "")
        ' overload that sets forceLoad to false so we dont break old method calls.
        ParseInvoicesFromXML(xmld, EInvoiceId, False, ReparsedPONum)
    End Sub

    Public Sub ParseInvoicesFromXML(ByVal xmld As XmlDocument, ByVal EInvoiceId As Nullable(Of Integer), ByVal forceLoad As Boolean, Optional ByVal ReparsedPONum As String = "")
        Dim _node As XmlNode
        Dim _nodelist As XmlNodeList
        Dim _invoice As EInvoicing_InvoiceBO = New EInvoicing_InvoiceBO
        Dim _currentInvoiceId As String = String.Empty
        Dim _HeaderFields As Hashtable = New Hashtable
        Dim _SummaryFields As Hashtable = New Hashtable
        Dim _ItemFields As Hashtable = New Hashtable
        Dim _KnownElements As List(Of String) = Nothing
        Dim _KnownSACCodes As List(Of String) = Nothing
        Dim _KnownItemElements As List(Of String) = Nothing
        Dim _KnownHeaderElements As List(Of String) = Nothing
        Dim _errorFound As Boolean = False
        Dim _isValidated As Boolean = False
        Dim _errorCode As Integer = -1
        Dim _hasAllRequiredData As Boolean = False
        Dim _updateEINVData As Boolean = False
        Dim _updateIRMAInvoiceData As Boolean = False


        Dim EInvBo As EInvoicingBO = New EInvoicingBO
        Dim _currentLineItemid As String = String.Empty

        ' load all known elements from EInvoicing_Config table
        With EInvBo
            _KnownElements = .getKnownElements
            _KnownSACCodes = .getKnownSACCodes
            _KnownHeaderElements = .getKnownHeaderElements
            _KnownItemElements = .getKnownitemElements
        End With

        'get a node list of all invoices from the XML docuemnt. 
        _nodelist = xmld.SelectNodes("/invoiceList/invoice")

        ' parse through each invoice.
        For Each _node In _nodelist

            Try
                EInvoicing_CurrentInvoice.Clear()

                _HeaderFields.Clear()
                _SummaryFields.Clear()
                _ItemFields.Clear()
                _updateEINVData = False
                _updateIRMAInvoiceData = False
                _hasAllRequiredData = True

                ' ##########################################################
                ' Get required fields from the invoice data. 
                ' ##########################################################
                For Each _child As XmlNode In _node.ChildNodes
                    Select Case _child.Name.ToLower
                        Case "invoice_num"
                            _invoice.Invoice_Num = NullableString(_child.InnerText)
                        Case "vendor_id"
                            _invoice.VendorId = NullableString(_child.InnerText)
                        Case "store_num"
                            _invoice.Store_Num = NullableString(_child.InnerText)
                        Case "po_num"
                            _invoice.PONum = NullableString(_child.InnerText) & ""
                        Case "invoice_date"
                            _invoice.Invoice_Date = DateTime.Parse(_child.InnerText, New System.Globalization.CultureInfo("en-US"))
                    End Select
                Next

                Dim result As String

                ' Populate the debug object with the current invoice information.
                EInvoicing_CurrentInvoice.InvoiceNumber = _invoice.Invoice_Num
                EInvoicing_CurrentInvoice.PONumber = _invoice.PONum
                EInvoicing_CurrentInvoice.StoreNumber = _invoice.Store_Num
                EInvoicing_CurrentInvoice.VendorId = _invoice.VendorId

                If EInvoiceId.HasValue Then
                    'This is a REPARSE. use the preparsed Ponum from the db. NOT the one in the xml. 
                    If Not ReparsedPONum.Equals(String.Empty) Then
                        _invoice.PONum = ReparsedPONum
                        EInvoicing_CurrentInvoice.PONumber = ReparsedPONum
                    End If
                End If

                With _invoice
                    If .PONum Is Nothing Or _
                       .VendorId Is Nothing Or _
                       .Store_Num Is Nothing Or _
                       .Invoice_Num Is Nothing Then
                        _hasAllRequiredData = False
                    End If

                End With

                If _hasAllRequiredData Then
                    logger.Info(String.Format("inv: {0}, po: {1}, vendor: {2}, store: {3}", _invoice.Invoice_Num.ToString, _invoice.PONum.ToString, _invoice.VendorId.ToString, _invoice.Store_Num.ToString))
                    result = _invoice.IsDuplicateEinvoice(_invoice.Invoice_Num, _invoice.VendorId, _invoice.PONum, forceLoad)
                    EInvoicing_CurrentInvoice.DuplicateInvoice = result
                    logger.Info(String.Format("IsDuplicateInvoice( {0} , {1} ): {2}", _invoice.Invoice_Num.ToString, _invoice.VendorId.ToString, result))
                    Select Case Split(result, "|")(0).ToLower()
                        Case "insert"
                            'this will be insert|<errorcode> or insert|ok. we need to parse it out.
                            'index 0 = insert
                            'index 1 = errorcode or 'ok'

                            EInvoiceId = Nothing
                            _updateEINVData = True
                            _updateIRMAInvoiceData = True
                            If Split(result, "|")(1).Equals("ok") Then
                                _errorCode = -1      ' insert this invoice as a new invoice.
                            Else
                                _errorCode = Integer.Parse(Split(result, "|")(1))  ' insert this invoice as a new invoice, but flag it with this errorcode.
                            End If
                        Case "error"
                            EInvoiceId = Nothing
                            _errorFound = True
                        Case Else
                            'this will be update|<einvoiceid>|OVERWRITEALL or update|<einvoiceid>|OVERWRITEEINV. we need to parse it out.
                            'index 0 = update
                            'index 1 = einvoiceid
                            'index 2 = OVERWRITEALL or OVERWRITEEINV
                            EInvoiceId = Integer.Parse(Split(result, "|")(1))
                            Select Case Split(result, "|")(2).ToString()
                                Case "OVERWRITEEINV"
                                    _updateEINVData = True
                                    _updateIRMAInvoiceData = False

                                Case "OVERWRITEALL"
                                    _updateEINVData = True
                                    _updateIRMAInvoiceData = True

                                Case Else

                                    EInvBo.SetInvoiceStatus(CInt(EInvoiceId), CInt(Split(result, "|")(2)), "Suspended")
                                    _currentInvoiceId = EInvoiceId.ToString()
                                    _errorFound = True
                                    _errorCode = 105
                            End Select
                    End Select
                Else
                    _errorCode = 101 ' Missing required data such as po_num, invoice_num, or vendorid. import and mark as suspended.
                End If
                If Not _errorFound Then

                    If Not EInvoiceId.HasValue Then
                        _currentInvoiceId = _invoice.CreateInvoiceRecord(_invoice.Invoice_Num, _invoice.VendorId, CInt(_invoice.Store_Num), _invoice.PONum, _invoice.Invoice_Date, _node.OuterXml)
                    Else
                        _currentInvoiceId = EInvoiceId.ToString()
                        logger.InfoFormat("Updating Existing Invoice Record: {0}", _currentInvoiceId)
                        Me.ClearEInvoiceData(EInvoiceId)
                    End If

                    EInvoicing_CurrentInvoice.EInvoicingId = _currentInvoiceId.ToString

                    _isValidated = EInvBo.ValidateDataElements(_node, CInt(_currentInvoiceId))

                    If Not _isValidated Then
                        EInvBo.SetInvoiceStatus(CInt(_currentInvoiceId), 1, "Suspended")
                    Else

                        ' keep a list of invoice numbers for later use.
                        _InvoiceNumbers.Add(_invoice.Invoice_Num)
                        _invoice.Dispose()

                        ' ##########################################################
                        '  Get Header data from the current Invoice
                        ' ##########################################################

                        Try
                            ParseXMLElements(_node, _HeaderFields)
                        Catch ex As Exception
                            logger.Info("Exception: Parsing Header Data from XML")
                            Throw
                        End Try

                        ' ##########################################################
                        '  Get Address data from the current Invoice
                        ' ##########################################################
                        Try
                            ParseXMLElements(_node.SelectNodes("buyer_address")(0), _HeaderFields)
                        Catch ex As Exception
                            logger.Info("Exception: Parsing Address Data from XML")
                            Throw
                        End Try

                        ' ##########################################################
                        '  Get Summary data from the current Invoice
                        ' ##########################################################
                        Try
                            ParseXMLElements(_node.SelectNodes("summary")(0), _SummaryFields)
                        Catch ex As Exception
                            logger.Info("Exception: Parsing Summary Data from XML")
                            Throw
                        End Try

                        Dim HeaderBO As EInvoicing_HeaderBO = New EInvoicing_HeaderBO
                        Dim InvoiceBO As EInvoicing_InvoiceBO = New EInvoicing_InvoiceBO
                        Try

                            Try

                                If Not ReparsedPONum.Equals("") Then

                                    _HeaderFields.Item("po_num") = ReparsedPONum

                                End If

                                HeaderBO.ImportHeaderInformation(_HeaderFields, CInt(_currentInvoiceId))
                            Catch
                                'Could not import Invoice Header information. XML Elements in marked as IsHeaderElement=True in Einvoicing_Config must match the column names in Einvoicing_Header
                                If _errorCode = -1 Then ' if error code has already been set, do not change it.
                                    _errorCode = 11
                                End If

                                Throw
                            End Try

                            Try
                                HeaderBO.ImportInvoiceSACCodeInformation(_SummaryFields, _KnownSACCodes, _currentInvoiceId)
                            Catch
                                'Could not import Invoice Summary information. The summary portion of the Einvoice is assumed to contain only SAC Codes. Check your SAC Code configuration settings.
                                _errorCode = 12
                                Throw
                            End Try



                            ' ##########################################################
                            '  Get a list of LineItems from the current Invoice
                            ' ##########################################################
                            Try
                                For Each _lineitemnode As XmlNode In _node.SelectNodes("lineItems/lineItem")

                                    'find line item id
                                    _currentLineItemid = _lineitemnode.SelectSingleNode("line_num").InnerText
                                    'get all LineItem nodes
                                    _ItemFields.Clear()
                                    ParseXMLElements(_lineitemnode, _ItemFields)
                                    Try
                                        InvoiceBO.ImportLineItemInformation(_ItemFields, _KnownItemElements, CInt(_currentInvoiceId), CInt(_currentLineItemid))
                                    Catch
                                        _errorCode = 13
                                        Throw
                                    End Try
                                    Try
                                        HeaderBO.ImportItemSACCodeInformation(_ItemFields, _KnownSACCodes, _currentInvoiceId, _currentLineItemid)
                                    Catch
                                        _errorCode = 14
                                        Throw
                                    End Try

                                Next
                            Catch
                                Throw
                            End Try


                        Catch
                            Throw
                        Finally
                            HeaderBO.Dispose()
                        End Try


                        If _errorCode <> -1 Then
                            ' an error was found.  mark the newly imported data with that error code.
                            EInvBo.SetInvoiceStatus(CInt(_currentInvoiceId), _errorCode, "Suspended")
                        Else

                            EInvBo.MatchInvoiceToPO(CInt(_currentInvoiceId), _updateIRMAInvoiceData)
                        End If



                    End If
                End If

            Catch ex As Exception

                logger.InfoFormat("# Import Error [ {0}:{1} ] Cleaning up Invoice so it can be reparsed #", _currentInvoiceId, _invoice.Invoice_Num)
                Me.ClearEInvoiceData(CInt(_currentInvoiceId))
                If _errorCode = -1 Then _errorCode = 9 ' unknown error
                EInvBo.SetInvoiceStatus(CInt(_currentInvoiceId), _errorCode, "Suspended")
                EInvoicing_CurrentInvoice.ExceptionMessage = ex.Message
                If Not ex.InnerException Is Nothing Then
                    EInvoicing_CurrentInvoice.InnerExceptionMessage = ex.InnerException.Message
                End If
                EInvoicing_CurrentInvoice.StackTrace = ex.StackTrace

            Finally

                CheckForError(EInvBo, _currentInvoiceId)

            End Try

        Next

    End Sub


    Private Sub CheckForError(ByRef EInvBO As EInvoicingBO, ByRef CurrentInvoiceId As String)

        If EInvBO.checkForError(CurrentInvoiceId) Then
            Dim dao As EInvoicingDAO = New EInvoicingDAO
            dao.InsertErrorHistory()
            dao.Dispose()
        End If

        If Not EInvBO Is Nothing Then
            EInvBO.Dispose()
        End If

    End Sub
    Private Function CreateEInvoicingRecord(ByRef _XML As XmlDocument, ByRef fname As String) As String

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As New ArrayList
        Dim outputList As New ArrayList

        currentParam = New DBParam
        currentParam.Name = "FileName"
        currentParam.Value = fname
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)


        currentParam = New DBParam
        currentParam.Name = "FileData"
        currentParam.Value = _XML.OuterXml
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "FileId"
        currentParam.Value = Nothing
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)


        'outputList = factory.ExecuteStoredProcedure("EInvoicing_CreateEInvoiceRecord", paramList)

        Return outputList(0).ToString()

    End Function

    Private Function NullableString(ByVal _input As String) As String
        Dim retval As String

        If _input = String.Empty Then
            retval = Nothing
        Else
            retval = _input
        End If
        Return retval

    End Function


#Region "Properties"


    Private _XMLData As XmlDocument
    Public Property XMLData() As XmlDocument
        Get
            Return _XMLData
        End Get
        Set(ByVal value As XmlDocument)
            _XMLData = value
        End Set
    End Property
    Private _InvoiceNumbers As List(Of String)
    Public Property InvoiceNumbers() As List(Of String)
        Get
            Return _InvoiceNumbers
        End Get
        Set(ByVal value As List(Of String))
            _InvoiceNumbers = value
        End Set
    End Property
#End Region






End Class
