Imports log4net
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.ScalePush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.ScalePush.ScaleException
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.Common.Writers

    ''' <summary>
    ''' base writer for POSWriter and ScaleWriter
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BaseWriter
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ' POS Push file - text format
        Private _outFileText As StreamWriter = Nothing

        ' POS Push file - binary format
        Private _outFileBinary As BinaryWriter = Nothing

        ' Control file - text format
        Private _outControlFileText As StreamWriter = Nothing

        ' Control file - binary format
        Private _outControlFileBinary As BinaryWriter = Nothing

        ' Exempt Tag File - Text
        Private _exemptTagFileName As String = "DEFAULT"

        ' Encoding to use on the output file
        Protected _fileEncoding As Encoding = Encoding.GetEncoding(Encoding.Default.CodePage)

        ' Filename for the control file
        Protected _controlFilename As String = Nothing

        Protected _remoteSSHStoreList As New StringBuilder

        ' ------- Flags used during file processing
        ' tracks to see if main header text has already been added to the file
        Public _mainHdrAdded As Boolean = False
        ' tracks the number of lines written to a given writer for each change type; used by footer records
        Private _recordCount As Integer

        ' ------- Properties that configure the output for the writer
        ' POSFileWriterKey value
        Protected _posFileWriterKey As Integer
        ' Flag set to true if the writer enforces use of a data dictionary
        Private _enforceDictionary As Boolean
        ' Delim character used by the writer; can be Nothing
        Private _delimChar As String
        ' Flag set to true if the field id should have a delimeter between it and the data value (if delimChar = true)
        Private _fieldIdDelim As Boolean
        ' Flag set to true if the delim character appears at the start of a line
        Private _leadingDelim As Boolean
        ' Flag set to true if the delim character appears at the end of a line
        Private _trailingDelim As Boolean
        ' Flag set to true if the writer expects the section headers in the output file to correspond with the IRMA batches
        Private _outputByIrmaBatches As Boolean
        ' Flag set to true if the writer expects the data in fixed width format
        Private _fixedWidth As Boolean
        ' Tax flag true character used by the writer
        Private _taxFlagTrueChar As Char
        ' Tax flag true character used by the writer
        Private _taxFlagFalseChar As Char
        ' count of escape chars -- used to determine if escape chars should be looked up and then applied to data
        Private _escapeCharCount As Integer
        ' contains escape characters
        Private _escapeChars As Hashtable
        ' should new data be appended to existing files?  (or replace existing files?)
        Private _appendToFile As Boolean
        ' holds SCALE only writer type (POSWriter.ScaleWriterType)
        Private _scaleWriterType As String

        ' -------  Collection of POSDataElementBO objects that define the file format for each change type. 
        ' These values are populated from the POSWriterFileConfig 
        ' table when the writer is constructed.
        Private _itemIdDeleteConfig As New ArrayList
        Private _itemIdAddConfig As New ArrayList
        Private _itemDataDeleteConfig As New ArrayList
        Private _itemDataChangeConfig As New ArrayList
        Private _promoOfferConfig As New ArrayList
        Private _vendorIdAddConfig As New ArrayList
        Private _corpScaleIdAddConfig As New ArrayList
        Private _corpScaleIdDeleteConfig As New ArrayList
        Private _corpScaleItemChangeConfig As New ArrayList
        Private _corpScalePriceExceptionConfig As New ArrayList
        Private _zoneScaleItemDeleteConfig As New ArrayList
        Private _zoneScalePriceChangeConfig As New ArrayList
        Private _zoneScaleSmartXPriceChangeConfig As New ArrayList
        Private _shelfTagChangeConfig As New ArrayList
        Private _nutrifactChangeConfig As New ArrayList
        Private _extraTextChangeConfig As New ArrayList
        Private _electronicShelfTagConfig As New ArrayList

        ' Error handling information that is available to calling classes
        Private _processingRow As String
        Private _processingCol As String
        Private _processingFieldId As String
        Private _processingRowRepeat As String

        ' Local data variables used during processing
        Protected _chgType As ChangeType
        Protected _currentStoreUpdate As StoreUpdatesBO
        Protected _currentChange As SqlDataReader

#Region "Property Definitions"

        Public MustOverride Property WriterFilename(ByVal currentStore As StoreUpdatesBO) As String
        Public MustOverride Property OutputFileFormat() As FileFormat

        Public ReadOnly Property ProcessingRow() As String
            Get
                Return _processingRow
            End Get
        End Property

        Public ReadOnly Property ProcessingCol() As String
            Get
                Return _processingCol
            End Get
        End Property

        Public ReadOnly Property ProcessingFieldId() As String
            Get
                Return _processingFieldId
            End Get
        End Property

        Public ReadOnly Property ProcessingRowRepeat() As String
            Get
                Return _processingRowRepeat
            End Get
        End Property

         Public Overridable ReadOnly Property SupportsChangeType(ByVal chgType As ChangeType) As Boolean
            Get
                'base writer supports all change types
                Return True
            End Get
        End Property
        Public Overridable Property ExemptTagFileName(ByVal currentStore As StoreUpdatesBO) As String
            Get
                Return _exemptTagFileName
            End Get
            Set(ByVal value As String)
                _exemptTagFileName = value
            End Set
        End Property

        Public ReadOnly Property POSFileWriterKey() As Integer
            Get
                Return _posFileWriterKey
            End Get
        End Property

        Public Overridable ReadOnly Property RemoteSSHStoreList() As String
            Get
                Return _remoteSSHStoreList.ToString
            End Get
        End Property

        Public Overridable Property ControlFilename() As String
            Get
                Return _controlFilename
            End Get
            Set(ByVal value As String)
                _controlFilename = value
            End Set
        End Property

        Public Property OutFileText() As StreamWriter
            Get
                Return _outFileText
            End Get
            Set(ByVal value As StreamWriter)
                _outFileText = value
            End Set
        End Property

        Public Property OutFileBinary() As BinaryWriter
            Get
                Return _outFileBinary
            End Get
            Set(ByVal value As BinaryWriter)
                _outFileBinary = value
            End Set
        End Property

        Public Property OutControlFileText() As StreamWriter
            Get
                Return _outControlFileText
            End Get
            Set(ByVal value As StreamWriter)
                _outControlFileText = value
            End Set
        End Property

        Public Property OutControlFileBinary() As BinaryWriter
            Get
                Return _outControlFileBinary
            End Get
            Set(ByVal value As BinaryWriter)
                _outControlFileBinary = value
            End Set
        End Property

        Public Overridable Property FileEncoding() As Encoding
            Get
                Return _fileEncoding
            End Get
            Set(ByVal value As Encoding)
                _fileEncoding = value
            End Set
        End Property

        Public Property EnforceDictionary() As Boolean
            Get
                Return _enforceDictionary
            End Get
            Set(ByVal value As Boolean)
                _enforceDictionary = value
            End Set
        End Property

        Public Property DelimChar() As String
            Get
                Return _delimChar
            End Get
            Set(ByVal value As String)
                _delimChar = value
            End Set
        End Property

        Public Property FieldIdDelim() As Boolean
            Get
                Return _fieldIdDelim
            End Get
            Set(ByVal value As Boolean)
                _fieldIdDelim = value
            End Set
        End Property

        Public Property OutputByIrmaBatches() As Boolean
            Get
                Return _outputByIrmaBatches
            End Get
            Set(ByVal value As Boolean)
                _outputByIrmaBatches = value
            End Set
        End Property

        Public Property FixedWidth() As Boolean
            Get
                Return _fixedWidth
            End Get
            Set(ByVal value As Boolean)
                _fixedWidth = value
            End Set
        End Property

        Public Property TaxFlagTrueChar() As Char
            Get
                Return _taxFlagTrueChar
            End Get
            Set(ByVal value As Char)
                _taxFlagTrueChar = value
            End Set
        End Property

        Public Property TaxFlagFalseChar() As Char
            Get
                Return _taxFlagFalseChar
            End Get
            Set(ByVal value As Char)
                _taxFlagFalseChar = value
            End Set
        End Property

        Public Property RecordCount() As Integer
            Get
                Return _recordCount
            End Get
            Set(ByVal value As Integer)
                _recordCount = value
            End Set
        End Property

        Public Property ItemIdDeleteConfig() As ArrayList
            Get
                Return _itemIdDeleteConfig
            End Get
            Set(ByVal value As ArrayList)
                _itemIdDeleteConfig = value
            End Set
        End Property

        Public Property ItemIdAddConfig() As ArrayList
            Get
                Return _itemIdAddConfig
            End Get
            Set(ByVal value As ArrayList)
                _itemIdAddConfig = value
            End Set
        End Property

        Public Property ItemDataDeleteConfig() As ArrayList
            Get
                Return _itemDataDeleteConfig
            End Get
            Set(ByVal value As ArrayList)
                _itemDataDeleteConfig = value
            End Set
        End Property

        Public Property ItemDataChangeConfig() As ArrayList
            Get
                Return _itemDataChangeConfig
            End Get
            Set(ByVal value As ArrayList)
                _itemDataChangeConfig = value
            End Set
        End Property

        Public Property PromoOfferConfig() As ArrayList
            Get
                Return _promoOfferConfig
            End Get
            Set(ByVal value As ArrayList)
                _promoOfferConfig = value
            End Set
        End Property

        Public Property VendorIdAddConfig() As ArrayList
            Get
                Return _vendorIdAddConfig
            End Get
            Set(ByVal value As ArrayList)
                _vendorIdAddConfig = value
            End Set
        End Property

        Public Property CorpScaleIdAddConfig() As ArrayList
            Get
                Return _corpScaleIdAddConfig
            End Get
            Set(ByVal value As ArrayList)
                _corpScaleIdAddConfig = value
            End Set
        End Property

        Public Property CorpScaleIdDeleteConfig() As ArrayList
            Get
                Return _corpScaleIdDeleteConfig
            End Get
            Set(ByVal value As ArrayList)
                _corpScaleIdDeleteConfig = value
            End Set
        End Property

        Public Property CorpScaleItemChangeConfig() As ArrayList
            Get
                Return _corpScaleItemChangeConfig
            End Get
            Set(ByVal value As ArrayList)
                _corpScaleItemChangeConfig = value
            End Set
        End Property

        Public Property ZoneScaleItemDeleteConfig() As ArrayList
            Get
                Return _zoneScaleItemDeleteConfig
            End Get
            Set(ByVal value As ArrayList)
                _zoneScaleItemDeleteConfig = value
            End Set
        End Property

        Public Property ZoneScalePriceChangeConfig() As ArrayList
            Get
                Return _zoneScalePriceChangeConfig
            End Get
            Set(ByVal value As ArrayList)
                _zoneScalePriceChangeConfig = value
            End Set
        End Property

        Public Property ZoneScaleSmartXPriceChangeConfig() As ArrayList
            Get
                Return _zoneScaleSmartXPriceChangeConfig
            End Get
            Set(ByVal value As ArrayList)
                _zoneScaleSmartXPriceChangeConfig = value
            End Set
        End Property

        Public Property CorpScalePriceExceptionConfig() As ArrayList
            Get
                Return _corpScalePriceExceptionConfig
            End Get
            Set(ByVal value As ArrayList)
                _corpScalePriceExceptionConfig = value
            End Set
        End Property
        Public Property ShelfTagChangeConfig() As ArrayList
            Get
                Return _shelfTagChangeConfig
            End Get
            Set(ByVal value As ArrayList)
                _shelfTagChangeConfig = value
            End Set
        End Property

        Public Property NutrifactChangeConfig() As ArrayList
            Get
                Return _nutrifactChangeConfig
            End Get
            Set(ByVal value As ArrayList)
                _nutrifactChangeConfig = value
            End Set
        End Property

        Public Property ExtraTextChangeConfig() As ArrayList
            Get
                Return _extraTextChangeConfig
            End Get
            Set(ByVal value As ArrayList)
                _extraTextChangeConfig = value
            End Set
        End Property

        Public Property ElectronicShelfTagConfig() As ArrayList
            Get
                Return _electronicShelfTagConfig
            End Get
            Set(ByVal value As ArrayList)
                _electronicShelfTagConfig = value
            End Set
        End Property

        Public Property LeadingDelim() As Boolean
            Get
                Return _leadingDelim
            End Get
            Set(ByVal value As Boolean)
                _leadingDelim = value
            End Set
        End Property

        Public Property TrailingDelim() As Boolean
            Get
                Return _trailingDelim
            End Get
            Set(ByVal value As Boolean)
                _trailingDelim = value
            End Set
        End Property

        Public Property EscapeCharCount() As Integer
            Get
                Return _escapeCharCount
            End Get
            Set(ByVal value As Integer)
                _escapeCharCount = value
            End Set
        End Property

        Public Property EscapeChars() As Hashtable
            Get
                Return _escapeChars
            End Get
            Set(ByVal value As Hashtable)
                _escapeChars = value
            End Set
        End Property

        Public Property AppendToFile() As Boolean
            Get
                Return _appendToFile
            End Get
            Set(ByVal value As Boolean)
                _appendToFile = value
            End Set
        End Property

        Public Property ScaleWriterType() As String
            Get
                Return _scaleWriterType
            End Get
            Set(ByVal value As String)
                _scaleWriterType = value
            End Set
        End Property
#End Region

#Region "Common Header & Footer Methods"
        ''' <summary>
        ''' This method does the initial processing for adding a ChangeType to a SCALE Push File:
        ''' 1. Opens the temp file for the store being processed, creating the file if it does not exist.
        ''' 2. Appends the main header to the file, if this is a new file and this writer requires a file header.
        ''' 3. Appends the section header information to the file, if this writer requires a header for this change type.
        ''' </summary>
        ''' <param name="changeType"></param>
        ''' <param name="filename"></param>
        ''' <remarks>
        ''' If the header record requires data that is derived from the records contained within the file,
        ''' the writer should add a place holder record to the file during this method call.  The writer 
        ''' creates objects to store the necessary data for the header record and populates these objects 
        ''' during the AddRecordToFile processing.  The AddFooterToFile processing will then replace the
        ''' place holder record with the required header data.
        ''' </remarks>
        Public Sub AddHeaderToFile(ByVal changeType As ChangeType, ByVal filename As String, ByVal headerInfo As POSBatchHeaderBO)
            logger.Debug("AddHeaderToFile entry: changeType=" + changeType.ToString + ", filename=" + filename)
            ' Open the temp file for the store
            OpenTempFile(filename)

            ' Add the main header to the record if this is the first time the writer has opened the file.
            If Not _mainHdrAdded Then
                ' update error handling values
                _processingRow = "FILE HEADER"
                _processingCol = ""
                _processingFieldId = ""
                ' add the header
                AddMainHeaderTextToFile(changeType, filename, headerInfo)
                _mainHdrAdded = True
            End If

            ' Add the section header to the record.
            ' update error handling values
            _processingRow = "SECTION HEADER FOR CHANGE TYPE: " & changeType.ToString
            _processingCol = ""
            _processingFieldId = ""
            ' add the header
            AddSectionHeaderTextToFile(changeType, filename, headerInfo)
            logger.Debug("AddHeaderToFile exit")
        End Sub

        ''' <summary>
        ''' This method adds a file header to the top of a file.  This is the header that applies to the entire file -
        ''' not the header for a change type section.
        ''' </summary>
        ''' <param name="changeType"></param>
        ''' <param name="filename"></param>
        ''' <param name="headerInfo"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub AddMainHeaderTextToFile(ByVal changeType As ChangeType, ByVal filename As String, ByVal headerInfo As POSBatchHeaderBO)
            logger.Debug("AddMainHeaderTextToFile entry")
            logger.Debug("AddMainHeaderTextToFile exit")
        End Sub

        ''' <summary>
        ''' This method adds a section header to the file.  This is the header that applies to the change type being processed - 
        ''' not the entire file.
        ''' </summary>
        ''' <param name="changeType"></param>
        ''' <param name="filename"></param>
        ''' <param name="headerInfo"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub AddSectionHeaderTextToFile(ByVal changeType As ChangeType, ByVal filename As String, ByVal headerInfo As POSBatchHeaderBO)
            logger.Debug("AddSectionHeaderTextToFile entry")
            logger.Debug("AddSectionHeaderTextToFile exit")
        End Sub

        ''' <summary>
        ''' This method does the final processing for adding a ChangeType to a SCALE Push File:
        ''' 1. Appends the footer information to the file, if this writer requires a footer for this change type.
        ''' 2. Closes the temp file for the store being processed.
        ''' </summary>
        ''' <param name="changeType"></param>
        ''' <remarks></remarks>
        Public Sub AddFooterToFile(ByVal changeType As ChangeType, ByVal footerInfo As POSBatchFooterBO)
            logger.Debug("AddFooterToFile entry: changeType=" + changeType.ToString)
            ' Verify the file has been opened for writing
            If IsTempFileOpen() Then
                Try
                    ' Add the footer text for the change type
                    AddSectionFooterTextToFile(changeType, footerInfo)
                Finally
                    ' Close the file
                    CloseTempFile()
                End Try
            Else
                ' This is an error - the methods were not called in the correct order. 
                throwException("Error in POSWriter.AddFooterToTile because unable to open file.  Methods not called in correct order.")
            End If
            logger.Debug("AddFooterToFile exit")
        End Sub

        ''' <summary>
        ''' This method adds a file footer to the bottom of a file.  This is the footer that applies to the entire file -
        ''' not the footer for a change type section.
        ''' </summary>
        ''' <param name="changeType"></param>
        ''' <param name="filename"></param>
        ''' <param name="footerInfo"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub AddMainFooterTextToFile(ByVal changeType As ChangeType, ByVal filename As String, ByVal footerInfo As POSBatchFooterBO)
            logger.Debug("AddMainFooterTextToFile entry: changeType=" + changeType.ToString + ", filename=" + filename)
            'TODO - implement this in POSPushJob?
            logger.Debug("AddMainFooterTextToFile exit")
        End Sub

        ''' <summary>
        ''' This method adds a section footer to the file.  This is the footer that applies to the change type being processed - 
        ''' not the entire file.
        ''' </summary>
        ''' <param name="changeType"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub AddSectionFooterTextToFile(ByVal changeType As ChangeType, ByVal footerInfo As POSBatchFooterBO)
            logger.Debug("AddSectionFooterTextToFile entry: changeType=" + changeType.ToString)
            logger.Debug("AddSectionFooterTextToFile exit")
        End Sub

#End Region

#Region "Common methods to add a single record to the POS Push file"
        ''' <summary>
        ''' This method adds the record for a single change to the POS Push File.  It works with a file that was
        ''' opened by the AddHeaderToFile method.
        ''' </summary>
        ''' <param name="chgType"></param>
        ''' <param name="currentChange"></param>
        ''' <remarks></remarks>

        Public Sub AddRecordToFile(ByVal chgType As ChangeType, ByRef currentStoreUpdate As StoreUpdatesBO, ByRef currentChange As SqlDataReader)
            logger.Debug("AddRecordToFile entry: changeType=" + chgType.ToString)
            ' Set the local vars used during processing
            _chgType = chgType
            _currentStoreUpdate = currentStoreUpdate
            _currentChange = currentChange

            ' Verify the file has been opened for writing
            If IsTempFileOpen() Then
                ' Get the enumeration of POSDataElementBO objects for this change type
                Dim columnEnum As IEnumerator = Nothing

                Select Case chgType
                    Case ChangeType.ItemIdAdd
                        columnEnum = Me.ItemIdAddConfig.GetEnumerator()
                    Case ChangeType.ItemIdDelete
                        columnEnum = Me.ItemIdDeleteConfig.GetEnumerator()
                    Case ChangeType.ItemDataChange
                        columnEnum = Me.ItemDataChangeConfig.GetEnumerator()
                    Case ChangeType.ItemDataDelete
                        columnEnum = Me.ItemDataDeleteConfig.GetEnumerator()
                    Case ChangeType.ItemDataDeAuth
                        ' De-auth records are sent down automatically using the delete batch format
                        columnEnum = Me.ItemDataDeleteConfig.GetEnumerator()
                    Case ChangeType.ItemRefresh
                        columnEnum = Me.ItemDataChangeConfig.GetEnumerator()
                    Case ChangeType.PromoOffer
                        columnEnum = Me.PromoOfferConfig.GetEnumerator()
                    Case ChangeType.VendorIDAdd
                        columnEnum = Me.VendorIdAddConfig.GetEnumerator()
                    Case ChangeType.CorpScaleItemIdAdd
                        columnEnum = Me.CorpScaleIdAddConfig.GetEnumerator()
                    Case ChangeType.CorpScaleItemIdDelete
                        columnEnum = Me.CorpScaleIdDeleteConfig.GetEnumerator()
                    Case ChangeType.CorpScaleItemChange
                        columnEnum = Me.CorpScaleItemChangeConfig.GetEnumerator()
                    Case ChangeType.ZoneScaleItemDelete
                        columnEnum = Me.ZoneScaleItemDeleteConfig.GetEnumerator()
                    Case ChangeType.ZoneScalePriceChange
                        columnEnum = Me.ZoneScalePriceChangeConfig.GetEnumerator()
                    Case ChangeType.ZoneScaleItemAuthPriceChange
                        columnEnum = Me.ZoneScalePriceChangeConfig.GetEnumerator()
                    Case ChangeType.ZoneScaleItemDeAuthPriceChange
                        columnEnum = Me.ZoneScalePriceChangeConfig.GetEnumerator()
                    Case ChangeType.ZoneScaleSmartXPriceChange
                        columnEnum = Me.ZoneScaleSmartXPriceChangeConfig.GetEnumerator()
                    Case ChangeType.CorpScalePriceExceptions
                        columnEnum = Me.CorpScalePriceExceptionConfig.GetEnumerator()
                    Case ChangeType.shelfTagChange
                        columnEnum = Me.ShelfTagChangeConfig.GetEnumerator()
                    Case ChangeType.NutriFact
                        columnEnum = Me.NutrifactChangeConfig.GetEnumerator()
                    Case ChangeType.ExtraText
                        columnEnum = Me.ExtraTextChangeConfig.GetEnumerator()
                    Case ChangeType.ElectronicShelfTagChange
                        columnEnum = Me.ElectronicShelfTagConfig.GetEnumerator()
                End Select

                ' Add an entry to the file for the change
                AddLineToFile(columnEnum, False)
            Else
                ' This is an error - the methods were not called in the correct order.  
                throwException("Error in AddRecordToFile because unable to open file.  Methods not called in correct order.")
            End If
            logger.Debug("AddRecordToFile exit")
        End Sub

        ''' <summary>
        ''' Writer-specific business rules to determine if a column should be included in the file output for the writer.
        ''' </summary>
        ''' <param name="currentRowNum"></param>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function IncludeColumnInFile(ByVal currentRowNum As Integer, ByRef currentColumn As POSDataElementBO) As Boolean
            logger.Debug("IncludeColumnInFile entry: currentRowNum=" + currentRowNum.ToString + ", currentColumn.ColumnOrder=" + currentColumn.ColumnOrder.ToString)
            Dim includeCol As Boolean = True
            logger.Debug("IncludeColumnInFile exit: includeCol=" + includeCol.ToString)
            Return includeCol
        End Function

        ''' <summary>
        ''' Writer-specific business rules to determine if a row should be included in the file output for the writer.
        ''' </summary>
        ''' <param name="currentRowNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function IncludeRowInFile(ByVal currentRowNum As Integer) As Boolean
            logger.Debug("IncludeRowInFile entry: currentRowNum=" + currentRowNum.ToString)
            Dim includeRow As Boolean = True
            logger.Debug("IncludeRowInFile exit: includeCol=" + includeRow.ToString)
            Return includeRow
        End Function

        ''' <summary>
        ''' Writer-specific business rules to determine if a row should be repeated in the file output for the writer.
        ''' </summary>
        ''' <param name="currentRowNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function RepeatRowInFile(ByVal currentRowNum As Integer, ByVal rowRepeatCount As Integer) As Boolean
            logger.Debug("RepeatRowInFile entry: currentRowNum=" + currentRowNum.ToString + ", rowRepeatCount=" + rowRepeatCount.ToString)
            Dim repeatRow As Boolean = False
            logger.Debug("RepeatRowInFile exit: repeatRow=" + repeatRow.ToString)
            Return repeatRow
        End Function

        ''' <summary>
        ''' Method to add a single column to the file.  This performs the formatting for the column.
        ''' </summary>
        ''' <param name="useDelim"></param>
        ''' <param name="isDataWritten"></param>
        ''' <param name="lineStr"></param>
        ''' <param name="currentColumn"></param>
        ''' <param name="columnEnum"></param>
        ''' <param name="currentRowNum"></param>
        ''' <param name="rowRepeatCount"></param>
        ''' <remarks></remarks>
        Private Sub AddDataToCurrentLine(ByVal useDelim As Boolean, ByVal isDataWritten As Boolean, ByRef lineStr As StringBuilder, ByRef currentColumn As POSDataElementBO, ByRef columnEnum As IEnumerator, ByVal currentRowNum As Integer, ByVal rowRepeatCount As Integer)
            logger.Debug("AddDataToCurrentLine entry: useDelim=" + useDelim.ToString + ", isDataWritten=" + isDataWritten.ToString + ", currentColumn.ColumnOrder=" + currentColumn.ColumnOrder.ToString + ", currentRowNum=" + currentRowNum.ToString + ", rowRepeatCount=" + rowRepeatCount.ToString)
            Dim currentData As String

            ' write the data for this column to the file
            ' Add the delim character for the writer, if there is one
            If useDelim Then
                'Check for inclusion of leading delim
                If isDataWritten Then
                    lineStr.Append(Me.DelimChar)
                ElseIf Me.LeadingDelim Then
                    lineStr.Append(Me.DelimChar)
                End If
            End If

            ' Append the field id, if there is one
            If currentColumn.FieldId IsNot Nothing AndAlso Not currentColumn.FieldId.Equals("") AndAlso Not Me.EnforceDictionary Then
                ' update error handling values
                _processingFieldId = currentColumn.FieldId

                lineStr.Append(currentColumn.FieldId)

                'check to see if delim should be added between field id and data item
                If useDelim AndAlso Me.FieldIdDelim Then
                    lineStr.Append(Me.DelimChar)
                End If
            End If

            ' Process the column based on the Data Element configuration
            If (currentColumn.IsBinaryInt) Then
                ' NOTE: Must check for IsBinaryInt before IsLiteral or IsTaxFlag because it can contain literal 
                ' or tax flag settings for each bit position
                ' Adding a binary int value is unique because it is the concatenation of 8 columns in the writer -
                ' one for each bit position
                Dim binaryIntValues As New ArrayList
                ' add bit position 0 - the current column the writer is on
                binaryIntValues.Add(ReadBinaryIntValueForBit(currentColumn))
                ' advance the column enumerator to add bit positions 1 thru 7
                For i As Integer = 1 To 7
                    If columnEnum.MoveNext Then
                        binaryIntValues.Add(ReadBinaryIntValueForBit(CType(columnEnum.Current, POSDataElementBO)))
                    End If
                Next
                ' Build the string from the array of bit values
                currentData = BuildBinaryIntString(binaryIntValues)
            ElseIf (currentColumn.IsPackedDecimal) Then
                ' NOTE: Must check for IsPackedDecimal before IsLiteral because it can contain literal settings 
                ' Add a packed decimal value to the file
                currentData = BuildPackString(currentColumn)
            ElseIf (currentColumn.IsLiteral) Then
                ' Add the literal string to the file
                currentData = BuildLiteralDataString(currentColumn)
            ElseIf (currentColumn.IsTaxFlag) Then
                ' Add the tax flag value to the file
                currentData = FormatTaxDataString(BuildTaxDataString(currentColumn), currentColumn)
            Else
                ' Add the dynamic data element value to the file
                currentData = BuildDataElementDataString(currentColumn, currentRowNum, rowRepeatCount)
            End If

            'append data to line
            lineStr.Append(currentData)
            If currentData IsNot Nothing Then
                logger.Debug("AddDataToCurrentLine exit: currentData=" + currentData.ToString)
            Else
                logger.Debug("AddDataToCurrentLine exit: currentData is nothing")
            End If

        End Sub

        ''' <summary>
        ''' If the writer needs to repeat a single row of data more than once in the file, 
        ''' this method will handle the repetition of the line based on the writer-specific
        ''' business rules.
        ''' </summary>
        ''' <param name="currentRowConfig"></param>
        ''' <param name="currentRow"></param>
        ''' <param name="rowRepeatCount"></param>
        ''' <remarks></remarks>
        Private Sub AddRepeatRowsToFile(ByRef currentRowConfig As ArrayList, ByVal currentRow As Integer, ByVal rowRepeatCount As Integer, ByVal useDelim As Boolean)
            logger.Debug("AddRepeatRowsToFile entry: currentRowConfig.count=" + currentRowConfig.Count.ToString + ", currentRow=" + currentRow.ToString + ", rowRepeatCount=" + rowRepeatCount.ToString + ", useDelim=" + useDelim.ToString)
            Dim lineStr As New StringBuilder
            Dim repeatColumn As POSDataElementBO
            Dim isDataWritten As Boolean = False
            Dim includeCurrentCol As Boolean = False

            ' Did the current row get included in the file?  If so, check to see if this row
            ' is repeated, usign the same data set.
            Dim repeatEnum As IEnumerator = currentRowConfig.GetEnumerator()
            While RepeatRowInFile(currentRow, rowRepeatCount)
                rowRepeatCount += 1
                repeatEnum.Reset()

                'start new StringBuilder for the repeated row
                lineStr = New StringBuilder
                isDataWritten = False

                'Append each of the columns for the repeated row
                While (repeatEnum.MoveNext)
                    repeatColumn = CType(repeatEnum.Current, POSDataElementBO)

                    ' update error handling values
                    _processingRowRepeat = rowRepeatCount.ToString
                    _processingCol = repeatColumn.ColumnOrder.ToString
                    _processingFieldId = ""

                    ' check with the writer to see if this column should be included in the output
                    includeCurrentCol = IncludeColumnInFile(currentRow, repeatColumn)

                    If includeCurrentCol Then
                        ' write the data for this column to the file
                        AddDataToCurrentLine(useDelim, isDataWritten, lineStr, repeatColumn, repeatEnum, currentRow, rowRepeatCount)
                        isDataWritten = True
                    End If

                End While ' End adding columns for repeated row

                ' add the delim after the last column
                If isDataWritten AndAlso useDelim AndAlso Me.TrailingDelim Then
                    lineStr.Append(Me.DelimChar)
                End If

                Dim line As String = lineStr.ToString
                ' write the line to the file
                If isDataWritten Then
                    WriteDataLine(lineStr.ToString)
                End If
            End While ' End adding repeating rows
            logger.Debug("AddRepeatRowsToFile exit")
        End Sub

        ''' <summary>
        ''' Iterate through the collection of column definition objects, adding a new entry
        ''' to the file for the change.
        ''' </summary>
        ''' <param name="columnEnum"></param>
        ''' <param name="deleteData"></param>
        ''' <remarks></remarks>
        Protected Sub AddLineToFile(ByRef columnEnum As IEnumerator, ByVal deleteData As Boolean)
            logger.Debug("AddLineToFile entry: deleteData=" + deleteData.ToString)
            Dim currentColumn As POSDataElementBO
            Dim lineStr As New StringBuilder
            Dim isDataWritten As Boolean = False
            Dim includeCurrentCol As Boolean = False
            Dim includeCurrentRow As Boolean = False
            Dim currentRow As Integer = -1
            Dim previousRow As Integer = -1

            ' Some writers need to repeat the same row of data more that once, with writer specific logic applied 
            ' on the repeat.
            ' Keep an ArrayList of the column definitions for the current row as we iterate through the row in case
            ' this is row is repeated.
            Dim rowRepeatCount As Integer = 0
            Dim currentRowConfig As New ArrayList

            ' Do we use delimiters
            Dim useDelim As Boolean = (Me.DelimChar IsNot Nothing And Me.DelimChar <> "")

            'Append each of the columns for the row
            While (columnEnum.MoveNext)
                currentColumn = CType(columnEnum.Current, POSDataElementBO)
                currentRowConfig.Add(currentColumn)
                currentRow = currentColumn.RowOrder

                ' update error handling values
                _processingRow = currentRow.ToString
                _processingCol = currentColumn.ColumnOrder.ToString
                _processingFieldId = ""
                _processingRowRepeat = rowRepeatCount.ToString

                logger.Debug("AddLineToFile - processing: currentColumn.ColumnOrder=" + currentColumn.ColumnOrder.ToString + ", currentColumn.DataElement=" + currentColumn.DataElement)
                'check if still on current row
                If previousRow <> -1 AndAlso currentRow <> previousRow AndAlso isDataWritten Then
                    ' moving on to the next row
                    ' add the delim after the last column
                    If useDelim AndAlso Me.TrailingDelim Then
                        lineStr.Append(Me.DelimChar)
                    End If

                    'write previous row of data to file
                    If includeCurrentRow Then
                        WriteDataLine(lineStr.ToString)
                    End If

                    ' Did the current row get included in the file?  If so, check to see if this row
                    ' is repeated, usign the same data set.
                    If includeCurrentRow Then
                        AddRepeatRowsToFile(currentRowConfig, previousRow, rowRepeatCount, useDelim)
                    End If

                    ' reset the data for processing repeating rows
                    rowRepeatCount = 0
                    currentRowConfig.Clear()

                    'start new StringBuilder for current row
                    lineStr = New StringBuilder
                    isDataWritten = False

                    'check with the writer to see if this row should be included in the output
                    includeCurrentRow = IncludeRowInFile(currentRow)

                ElseIf previousRow = -1 AndAlso Not isDataWritten Then
                    ' this is the first row
                    ' do we include it in the output?
                    includeCurrentRow = IncludeRowInFile(currentRow)
                End If

                ' check with the writer to see if this column should be included in the output
                includeCurrentCol = IncludeColumnInFile(currentRow, currentColumn)

                If includeCurrentCol Then
                    ' write the data for this column to the file
                    AddDataToCurrentLine(useDelim, isDataWritten, lineStr, currentColumn, columnEnum, currentRow, rowRepeatCount)
                    isDataWritten = True
                End If
                previousRow = currentRow
            End While ' End column enumerator for the row

            '-------handle final row of data-----------
            ' add the delim after the last column
            If isDataWritten AndAlso useDelim AndAlso Me.TrailingDelim Then
                lineStr.Append(Me.DelimChar)
            End If

            ' write the line to the file
            If isDataWritten AndAlso includeCurrentRow Then
                WriteDataLine(lineStr.ToString)

                ' Check to see if this row is repeated, using the same data set.
                AddRepeatRowsToFile(currentRowConfig, previousRow, rowRepeatCount, useDelim)
            End If
 
            logger.Debug("AddLineToFile exit")
        End Sub

        ''' <summary>
        ''' This function returns a Boolean value for one bit of a data element that is included in a binary integer column.
        ''' </summary>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function ReadBinaryIntValueForBit(ByRef currentColumn As POSDataElementBO) As Boolean
            logger.Debug("ReadBinaryIntValueForBit entry: currentColumn=" + currentColumn.ColumnOrder.ToString)
            Dim bitValue As Boolean = False

            ' Process the column based on the Data Element configuration
            Try
                If currentColumn.IsLiteral Then
                    ' Literals map to a true or false value
                    bitValue = CType(currentColumn.DataElement, Boolean)
                ElseIf currentColumn.IsTaxFlag Then
                    ' Tag flags should be set to true or false based on the item
                    ' the data for the tax flag is determined by the TaxFlagBO
                    ' the text in the DataElement column defines which tax flag should be displayed
                    Dim currentTaxFlagKey As String = currentColumn.DataElement

                    ' read the tax flag values for the current item and store from the StoreUpdatesBO
                    Dim currentItemKey As Integer = CType(_currentChange.GetValue(_currentChange.GetOrdinal("Item_Key")), Integer)
                    Dim currentTaxFlag As TaxFlagBO = _currentStoreUpdate.GetItemTaxFlagData(currentItemKey, currentTaxFlagKey)

                    ' write the true or false value for the tax flag
                    If currentTaxFlag IsNot Nothing Then
                        bitValue = currentTaxFlag.TaxFlagValue
                    End If
                Else
                    ' Map to the current data element
                    Dim iColumnOrdinal As Integer = _currentChange.GetOrdinal(currentColumn.DataElement)
                    If (Not _currentChange.IsDBNull(iColumnOrdinal)) Then
                        Try
                            bitValue = CBool(_currentChange.GetValue(iColumnOrdinal))
                        Catch ex As Exception
                            Select Case System.Type.GetTypeCode(_currentChange.GetFieldType(iColumnOrdinal))
                                Case System.TypeCode.String
                                    Try
                                        Select Case Left(_currentChange.GetValue(iColumnOrdinal).ToString, 1).ToUpper
                                            Case "Y", "T"
                                                bitValue = True
                                            Case Else
                                                bitValue = False
                                        End Select

                                    Catch ex1 As Exception
                                        ' use false by default
                                        bitValue = False
                                    End Try

                                Case Else
                                    bitValue = False
                            End Select

                        End Try
                    End If
                End If

            Catch ex As Exception
                logger.Error("ReadBinaryIntValueForBit - Exception: ", ex)

                Dim args(2) As String
                args(0) = "Error:  " & ex.GetType.ToString
                args(1) = "Store No:  " & _currentStoreUpdate.StoreNum.ToString
                args(2) = "Data Element:  " & currentColumn.DataElement
                args(3) = "Processing Row:  " & _processingRow
                args(4) = "Processing Column:  " & _processingCol
                args(5) = "Processing Field ID:  " & _processingFieldId
                args(6) = "Processing Row Repeat:  " & _processingRowRepeat
                ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Fatal, ex)

                Throw

            End Try

            logger.Debug("ReadBinaryIntValueForBit exit: bitValue=" + bitValue.ToString)
            Return bitValue
        End Function

        ''' <summary>
        ''' This function combines all 8 bits that make up a binary integer column to the single value for the column.
        ''' </summary>
        ''' <param name="binaryIntValues"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function BuildBinaryIntString(ByRef binaryIntValues As ArrayList) As Char
            logger.Debug("BuildBinaryIntString entry: binaryIntValues.Count=" + binaryIntValues.Count.ToString)
            Dim bitValue As Char
            Dim currentVal As Integer
            Dim currentMultiple As Integer = 1
            Dim currentBitVal As Integer = 0
            Dim bitTotal As Integer = 0

            For i As Integer = 0 To 7
                ' Read the current boolean value
                If CType(binaryIntValues.Item(i), Boolean) Then
                    currentVal = 1
                Else
                    currentVal = 0
                End If
                ' Multiple the flag by the bit position value
                currentBitVal = Math.Abs(currentVal * currentMultiple)
                ' Increment the bit total and the current bit position multiplier
                bitTotal = bitTotal + currentBitVal
                currentMultiple = currentMultiple * 2
            Next

            bitValue = Chr(bitTotal)
            logger.Debug("BuildBinaryIntString exit")
            Return bitValue
        End Function

        ''' <summary>
        ''' This function builds a packed decimal representation of a number for inclusion in a binary output file.
        ''' </summary>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function BuildPackString(ByRef currentColumn As POSDataElementBO) As String
            logger.Debug("BuildPackString entry: currentColumn=" + currentColumn.ColumnOrder.ToString)
            'Converts a string representation of a number into a packed byte string.
            Dim packString As New String(Chr(0), currentColumn.PackLength)

            ' Get the value to pack based on the Data Element configuration
            Dim dataContent As String = Nothing
            If currentColumn.IsLiteral Then
                dataContent = CType(currentColumn.DataElement, String)
            Else
                If (Not _currentChange.IsDBNull(_currentChange.GetOrdinal(currentColumn.DataElement))) Then
                    dataContent = _currentChange.GetValue(_currentChange.GetOrdinal(currentColumn.DataElement)).ToString()
                Else
                    ' if the string is null, set it to the default value
                    dataContent = currentColumn.DefaultValue
                    If dataContent Is Nothing Then
                        dataContent = ""
                    End If
                End If
            End If

            'Removes all spaces from string representation of the number.
            dataContent = dataContent.Replace(" ", "")

            ' Format the number to include/not include the decimal point and any implied decimal places
            dataContent = BuildDecimalValue(dataContent, currentColumn)

            'Left pads number with 0’s until Len(dataContent) = (PackLength * 2)
            ' Example: “123456” becomes “000000123456” if PackLength = 6
            ' Example: “23” becomes “0023” if PackLength = 2
            dataContent = dataContent.PadLeft(currentColumn.PackLength * 2, "0"c)

            logger.Debug("BuildPackString exit")
            Return BuildPackedDecimal(dataContent, currentColumn.PackLength)
        End Function

        ''' <summary>
        ''' Converts the number string into a packed binary string. (not your normal everyday 
        ''' string format) See the warning at end of function.
        ''' </summary>
        ''' <param name="unpackedString"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function BuildPackedDecimal(ByVal unpackedString As String, ByVal packLength As Integer) As String
            logger.Debug("BuildPackedDecimal entry: unpackedString=" + unpackedString + ", packLength=" + packLength.ToString)
            Dim mid1 As String = String.Empty
            Dim mid2 As String = String.Empty
            Dim int1 As Integer = 0
            Dim int2 As Integer = 0
            Dim fromBase As Integer = 16  'base for converting from HEX (base 16) to DECIMAL (base 10)
            Dim packString As New String(Chr(0), packLength)

            'Function: 
            '           Convert.ToInt32(ByVal value As String, ByVal fromBase As Integer) As Integer
            'Summary:
            '           Converts the System.String representation of a number in a specified base to an equivalent 32-bit signed integer.
            'Parameters:
            '           fromBase: The base of the number in value (must be 2, 8, 10, or 16). 
            '           value:    A System.String containing a number. 
            For i As Integer = 1 To packLength
                Try
                    mid1 = unpackedString.Substring(2 * i - 2, 1)
                    int1 = Convert.ToInt32(mid1, fromBase) * fromBase

                Catch ex As Exception
                    int1 = 0

                End Try

                Try
                    mid2 = unpackedString.Substring(2 * i - 1, 1)
                    int2 = Convert.ToInt32(mid2, fromBase)

                Catch ex As Exception
                    int2 = 0

                End Try

                If (int1 <> CType((Val(mid1) * 16), Integer)) OrElse (int2 <> CType(Val(mid2), Integer)) Then
                    Console.WriteLine("   ~~~~~~~~~~~~~~~~~ WARNING: UNEQUAL PACK VALUES ~~~~~~~~~~~~~~~~~")
                    Console.WriteLine("mid1: Val('{0}') = '{1}'; Convert.ToInt32('{0}', {2}) = '{3}'", mid1, CType((Val(mid1) * 16), Integer), fromBase, int1)
                    Console.WriteLine("mid2: Val('{0}') = '{1}'; Convert.ToInt32('{0}', {2}) = '{3}'", mid2, CType(Val(mid2), Integer), fromBase, int2)
                    Console.WriteLine("  <OLD> int1 + int2 = '{0}';  <NEW> int1 + int2 = '{1}'", (CType((Val(mid1) * 16), Integer) + CType(Val(mid2), Integer)), int1 + int2)
                End If

                Mid(packString, i, 1) = Chr(int1 + int2)
            Next i

            ' Warning: at this point, packString will always appear to be an empty string.
            ' It is actually a packed byte string which is not visible to the Visual Studio 
            ' debugger.        
            logger.Debug("BuildPackedDecimal exit")
            Return packString

        End Function



        ''' <summary>
        ''' This function returns a String for the literal columns (IsLiteral = true) or
        ''' returns the string representation for the character code when enclosed in braces: {0} => CHR(0)
        ''' </summary>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function BuildLiteralDataString(ByRef currentColumn As POSDataElementBO) As String
            logger.Debug("BuildLiteralDataString entry: currentColumn.DataElement=" + currentColumn.DataElement)
            Dim dataContent As String = Nothing

            If currentColumn.DataElement.StartsWith("{") AndAlso currentColumn.DataElement.EndsWith("}") Then
                ' evaluate the expression and append the result as text
                Dim charCode As Integer = CType(currentColumn.DataElement.Substring(1, currentColumn.DataElement.Length - 2), Integer)
                dataContent = Chr(charCode)
            Else
                ' append the text that appears in the DataElement column
                dataContent = currentColumn.DataElement
            End If

            logger.Debug("BuildLiteralDataString exit: dataContent=" + dataContent)
            Return dataContent
        End Function

        ''' <summary>
        ''' This function returns a String for the tax flag columns (IsLiteral = False and IsTaxFlag = true).
        ''' </summary>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function BuildTaxDataString(ByRef currentColumn As POSDataElementBO) As String
            logger.Debug("BuildTaxDataString entry: currentColumn.DataElement=" + currentColumn.DataElement)
            Dim dataContent As String = Nothing
            ' the data for the tax flag is determined by the TaxFlagBO
            ' the text in the DataElement column defines which tax flag should be displayed

            Dim currentTaxFlagKey As String = currentColumn.DataElement

            Try
                ' read the tax flag values for the current item and store from the StoreUpdatesBO
                Dim currentItemKey As Integer = CType(_currentChange.GetValue(_currentChange.GetOrdinal("Item_Key")), Integer)
                Dim currentTaxFlag As TaxFlagBO = _currentStoreUpdate.GetItemTaxFlagData(currentItemKey, currentTaxFlagKey)

                ' write the true or false value for the tax flag
                If currentTaxFlag.TaxFlagValue Then
                    dataContent = TaxFlagTrueChar
                Else
                    dataContent = TaxFlagFalseChar
                End If
            Catch ex As Exception
                logger.Info("BuildTaxDataString, currentItemKey = " + _currentChange.GetValue(_currentChange.GetOrdinal("Item_Key")).ToString())
                logger.Info("BuildTaxDataString, currentTaxFlagKey = " + currentTaxFlagKey)
                Throw
            End Try

            logger.Debug("BuildTaxDataString exit: dataContent=" + dataContent)
            Return dataContent
        End Function

        ''' <summary>
        ''' add data formatting to tax data value
        ''' </summary>
        ''' <param name="dataContent"></param>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function FormatTaxDataString(ByVal dataContent As String, ByRef currentColumn As POSDataElementBO) As String
            'add data formatting to tax flag value, if any
            Return ApplyDataFormatting(dataContent, currentColumn)
        End Function

        ''' <summary>
        ''' This function returns a String for the data element columns (IsLiteral = False and IsTaxFlag = False).
        ''' </summary>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function BuildDataElementDataString(ByRef currentColumn As POSDataElementBO, ByVal currentRowNum As Integer, ByVal rowRepeatCount As Integer) As String
            logger.Debug("BuildDataElementDataString entry: currentColumn.DataElement=" + currentColumn.DataElement + ", currentRowNum=" + currentRowNum.ToString + ", rowRepeatCount=" + rowRepeatCount.ToString)
            Dim lineStr As New StringBuilder
            Dim dataContent As String = Nothing

            ' append the data from currentChange specified by DataElement
            If (Not _currentChange.IsDBNull(_currentChange.GetOrdinal(currentColumn.DataElement))) Then
                dataContent = _currentChange.GetValue(_currentChange.GetOrdinal(currentColumn.DataElement)).ToString()
            Else
                ' if the string is null, set it to the default value
                dataContent = currentColumn.DefaultValue

                If dataContent Is Nothing Then
                    dataContent = ""
                End If
            End If

            logger.Debug("BuildDataElementDataString exit")
            Return ApplyDataFormatting(dataContent, currentColumn)
        End Function

        ''' <summary>
        ''' applies data formatting values (max width, padding, etc) to dataContent value
        ''' </summary>
        ''' <param name="dataContent"></param>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ApplyDataFormatting(ByVal dataContent As String, ByRef currentColumn As POSDataElementBO) As String
            logger.Debug("ApplyDataFormatting entry: currentColumn.DataElement=" + currentColumn.DataElement)
            If dataContent Is Nothing Then
                dataContent = ""
            End If

            'apply leading/trailing chars to data value
            If currentColumn.LeadingChars IsNot Nothing Then
                dataContent = currentColumn.LeadingChars + dataContent
            End If
            If currentColumn.TrailingChars IsNot Nothing Then
                dataContent = dataContent + currentColumn.TrailingChars
            End If

            'remove any escape chars if any
            If Me.EscapeCharCount > 0 Then
                Dim escapeCharEnum As IDictionaryEnumerator = Me.EscapeChars.GetEnumerator
                Dim currentEscape As String
                Dim currentReplace As String

                While escapeCharEnum.MoveNext
                    currentEscape = CType(escapeCharEnum.Key, String)
                    currentReplace = CType(escapeCharEnum.Value, String)

                    'replace escape char with replacement char
                    dataContent = dataContent.Replace(currentEscape, currentReplace)
                End While
            End If

            ' is this fixed width?
            If Me.FixedWidth Or currentColumn.FixedWidthField Then
                If currentColumn.IsDecimalValue Then
                    dataContent = BuildDecimalValue(dataContent, currentColumn)
                ElseIf currentColumn.IsBoolean Then
                    dataContent = BuildBooleanValue(dataContent, currentColumn)
                Else ' handle non-decimal values
                    ' are we too short?
                    If dataContent.Length < currentColumn.MaxFieldWidth Then
                        dataContent = PadData(dataContent, currentColumn)
                    ElseIf dataContent.Length > currentColumn.MaxFieldWidth Then
                        ' we're too long
                        dataContent = TruncateData(dataContent, currentColumn)
                    End If
                End If
            ElseIf currentColumn.MaxFieldWidth > 0 Then
                If currentColumn.IsDecimalValue Then
                    dataContent = BuildDecimalValue(dataContent, currentColumn)
                Else
                    ' is there a maximum field width
                    If dataContent.Length > currentColumn.MaxFieldWidth Then
                        ' we're too long
                        dataContent = TruncateData(dataContent, currentColumn)
                    End If
                End If
            ElseIf currentColumn.IsBoolean Then
                dataContent = BuildBooleanValue(dataContent, currentColumn)
            End If

            ' is there any writer specific formatting that should be applied to the value?
            ' this handles formatting that is not configured through the database / UI
            dataContent = ApplyWriterFormatting(dataContent, currentColumn)

            logger.Debug("ApplyDataFormatting exit: dataContent=" + dataContent)
            Return dataContent
        End Function

        ''' <summary>
        ''' This function allows writers to provide specific data formatting that is not handled by the 
        ''' writer configuration values.  This should only be used in rare instances.  In most cases, the 
        ''' wrtier configuration should be enhanced to capture this functionality.
        ''' </summary>
        ''' <param name="dataContent"></param>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function ApplyWriterFormatting(ByVal dataContent As String, ByRef currentColumn As POSDataElementBO) As String
            Return dataContent
        End Function

        ''' <summary>
        ''' Converts a numeric string into a packed byte string.
        ''' Left pads sNumber with 0’s until Len(sNumber) = (lLength * 2).
        ''' Converts the string into a packed binary string.
        ''' </summary>
        ''' <param name="InputNumber">numeric string to convert</param>
        ''' <param name="OutputLength">fixed length of returned packed byte string</param>
        ''' <remarks>
        ''' Warning! At this point, output string will appear to be an empty string, although
        ''' it's actually a packed byte string which is not visible to the Visual Studio debugger.
        ''' </remarks>
        Public Function Pack(ByVal InputNumber As String, ByVal OutputLength As Integer) As String
            logger.Debug("Pack entry: InputNumber=" + InputNumber + ", OutputLength=" + OutputLength.ToString)
            Dim sPackedByte As String
            Dim iPos As Integer

            'Removes all spaces from InputNumber.
            InputNumber = Replace(InputNumber, " ", "", 1, -1, CompareMethod.Binary)

            'Left pads InputNumber with 0’s until Len(InputNumber) = (OutputLength * 2)
            ' Example: “123456” becomes “000000123456” if OutputLength = 6
            ' Example: “23” becomes “0023” if OutputLength = 2
            While Len(InputNumber) < OutputLength * 2
                InputNumber = "0" & InputNumber
            End While

            sPackedByte = New String(Chr(0), OutputLength)

            ' Converts the string into a packed binary string. (not your normal everyday string format) See the warning below.
            For iPos = 1 To OutputLength
                Mid(sPackedByte, iPos, 1) = Chr(CInt((Val(Mid(InputNumber, ((iPos - 1) * 2) + 1, 1)) * 16) + Val(Mid(InputNumber, ((iPos - 1) * 2) + 2, 1))))
            Next iPos

            ' Warning: at this point, sPackedByte will always appear to be an empty string.
            ' It is actually a packed byte string which is not visible to the Visual Studio debugger.        
            logger.Debug("Pack exit")
            Return sPackedByte

        End Function

        ''' <summary>
        ''' pad data content appropriately based on max width values
        ''' </summary>
        ''' <param name="dataContent"></param>
        ''' <param name="currentColumn"></param>
        ''' <returns>modified dataContent value</returns>
        ''' <remarks></remarks>
        Public Function PadData(ByVal dataContent As String, ByRef currentColumn As POSDataElementBO) As String
            logger.Debug("PadData entry: dataContent=<" + dataContent + ">, currentColumn.PadLeft=" + currentColumn.PadLeft.ToString)
            ' do we pad right or left?
            If (currentColumn.PadLeft) Then
                dataContent = dataContent.PadLeft(currentColumn.MaxFieldWidth, currentColumn.FillChar)
            Else
                dataContent = dataContent.PadRight(currentColumn.MaxFieldWidth, currentColumn.FillChar)
            End If

            logger.Debug("PadData exit: dataContent=<" + dataContent + ">")
            Return dataContent
        End Function

        ''' <summary>
        ''' truncate data content appropriately based on max width values
        ''' </summary>
        ''' <param name="dataContent"></param>
        ''' <param name="currentColumn"></param>
        ''' <returns>modified dataContent value</returns>
        ''' <remarks></remarks>
        Public Function TruncateData(ByVal dataContent As String, ByRef currentColumn As POSDataElementBO) As String
            logger.Debug("TruncateData entry: dataContent=<" + dataContent + ">, currentColumn.TruncLeft=" + currentColumn.TruncLeft.ToString)
            If (currentColumn.TruncLeft) Then
                dataContent = dataContent.Remove(0, dataContent.Length - currentColumn.MaxFieldWidth)
            Else
                dataContent = dataContent.Remove(currentColumn.MaxFieldWidth, dataContent.Length - currentColumn.MaxFieldWidth)
            End If

            logger.Debug("TruncateData exit: dataContent=<" + dataContent + ">")
            Return dataContent
        End Function

        ''' <summary>
        ''' if data item is a decimal value, then use the max width + precision values to build the final
        ''' decimal value to be written to the file
        ''' </summary>
        ''' <param name="dataContent"></param>
        ''' <param name="currentColumn"></param>
        ''' <returns>modified dataContent value</returns>
        ''' <remarks></remarks>
        Public Function BuildDecimalValue(ByVal dataContent As String, ByRef currentColumn As POSDataElementBO) As String
            logger.Debug("BuildDecimalValue entry: dataContent=<" + dataContent + ">")
            'split data content at the decimal point
            Dim decimalData() As String = dataContent.Split("."c)

            'pad numeric data before decimal w/ zeros on the left up to the max length
            decimalData(0) = decimalData(0).PadLeft(currentColumn.MaxFieldWidth, "0"c)

            If decimalData.Length > 1 Then
                'check if data length exceeds precision length
                If decimalData(1).Length > currentColumn.DecimalPrecision Then
                    'remove data from the right
                    decimalData(1) = decimalData(1).Remove(currentColumn.DecimalPrecision, decimalData(1).Length - currentColumn.DecimalPrecision)
                Else
                    'pad data to meet the precision length
                    decimalData(1) = decimalData(1).PadRight(currentColumn.DecimalPrecision, "0"c)
                End If
            End If

            'build new data value
            Dim newDecimalValue As New StringBuilder
            ' add the data before the decimal
            newDecimalValue.Append(decimalData(0))
            ' check to include the decimal point
            If currentColumn.IncludeDecimal Then
                newDecimalValue.Append(".")
            End If
            ' check to include the data after the decimal
            If currentColumn.DecimalPrecision > 0 Then
                If decimalData.Length > 1 Then
                    ' add the values after the decimal - formatted above
                    newDecimalValue.Append(decimalData(1))
                Else
                    ' there was no input value for after the decimal - pad with zeros
                    newDecimalValue.Append("0".PadRight(currentColumn.DecimalPrecision, "0"c))
                End If
            End If

            logger.Debug("BuildDecimalValue exit: newDecimalValue=<" + newDecimalValue.ToString + ">")
            Return newDecimalValue.ToString
        End Function

        ''' <summary>
        ''' if data item is a boolean value, then use the true or false character values to build the final
        ''' string to be written to the file
        ''' </summary>
        ''' <param name="dataContent"></param>
        ''' <param name="currentColumn"></param>
        ''' <returns>modified dataContent value</returns>
        ''' <remarks></remarks>
        Public Function BuildBooleanValue(ByVal dataContent As String, ByRef currentColumn As POSDataElementBO) As String
            logger.Debug("BuildBooleanValue entry: dataContent=<" + dataContent + ">")
            Dim newBooleanValue As String = currentColumn.BooleanFalseChar()
            Try
                Dim booleanContent As Boolean = CType(dataContent, Boolean)
                If booleanContent Then
                    newBooleanValue = currentColumn.BooleanTrueChar()
                Else
                    newBooleanValue = currentColumn.BooleanFalseChar()
                End If
            Catch ex As Exception
                ' return the false value by default
            End Try
            logger.Debug("BuildBooleanValue exit: newBooleanValue=<" + newBooleanValue + ">")
            Return newBooleanValue
        End Function

        ''' <summary>
        ''' Write the contents of a data line to the file.  If a writer includes specific information before or 
        ''' after a line of data that is not covered by the common configuration, it should override this method.
        ''' </summary>
        ''' <param name="line"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub WriteDataLine(ByVal line As String)
            logger.Debug("WriteDataLine entry: line" + line)
            WriteLine(line)
            logger.Debug("WriteDataLine exit")
        End Sub

#End Region

#Region "Common methods for additional business logic"
        ''' <summary>
        ''' Allow for the Writer to create an additional control file, separate from the POS or Scale Writer, that
        ''' can be sent to the store to kick off a job.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Function CreateControlFile(ByRef currentStore As StoreUpdatesBO) As Boolean
            logger.Debug("CreateControlFile entry")
            logger.Debug("CreateControlFile exit")

            Return True
        End Function

        ''' <summary>
        ''' Allow for the Writer to call a process on a remote server prior to successfully transmitting data
        ''' ie: the IBM binary writer will call a process to execute something from the IBM server once the 
        ''' IBM binary data has been transmitted
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Function CallRemoteJobProcess(ByRef currentStore As StoreUpdatesBO) As Boolean
            logger.Debug("CallRemoteJobProcess entry")
            logger.Debug("CallRemoteJobProcess exit")

            Return True
        End Function

        Public Overridable Function CallSSHRemoteJobProcess(ByRef currentStore As StoreUpdatesBO, ByVal filename As String) As Boolean
            logger.Debug("CallSSHRemoteJobProcess entry")
            logger.Debug("CallSSHRemoteJobProcess exit")

            Return True
        End Function
#End Region

#Region "Common file helper methods"
        ''' <summary>
        ''' Verifies the output file is open for writing.  Defaults to the batch output file.  
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function IsTempFileOpen() As Boolean
            Return IsTempFileOpen(True)
        End Function

        Public Function IsOpen() As Boolean
            Return IsTempFileOpen(True)
        End Function

        ''' <summary>
        ''' Verifies the output file is open for writing.  If batchFile is true, it will check the 
        ''' batch file.  If batch file is false, it will check the control file.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function IsTempFileOpen(ByVal batchFile As Boolean) As Boolean
            Dim isOpen As Boolean = False
            If batchFile Then
                Select Case Me.OutputFileFormat
                    Case FileFormat.Binary
                        If (_outFileBinary IsNot Nothing) AndAlso (_outFileBinary.BaseStream.CanWrite) Then
                            isOpen = True
                        End If
                    Case FileFormat.Text
                        If (_outFileText IsNot Nothing) AndAlso (_outFileText.BaseStream.CanWrite) Then
                            isOpen = True
                        End If
                End Select
            Else
                Select Case Me.OutputFileFormat
                    Case FileFormat.Binary
                        If (_outControlFileBinary IsNot Nothing) AndAlso (_outControlFileBinary.BaseStream.CanWrite) Then
                            isOpen = True
                        End If
                    Case FileFormat.Text
                        If (_outControlFileText IsNot Nothing) AndAlso (_outControlFileText.BaseStream.CanWrite) Then
                            isOpen = True
                        End If
                End Select
            End If

            Return isOpen
        End Function

        ''' <summary>
        ''' Opens the output file.  Defaults to the batch output file.
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <remarks></remarks>
        Public Sub OpenTempFile(ByVal filename As String)
            OpenTempFile(filename, True)
        End Sub


        ''' <summary>
        ''' Opens the output file.  If batchFile is true, it will open the 
        ''' batch file.  If batch file is false, it will open the control file.
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <remarks></remarks>
        Protected Sub OpenTempFile(ByVal filename As String, ByVal batchFile As Boolean)
            If batchFile Then
                ' Check to see if the file already exists and set the _mainHdrAdded flag accordingly.
                If File.Exists(filename) Then
                    _mainHdrAdded = True
                Else
                    _mainHdrAdded = False
                End If

                ' Open the file for processing if it is not already open.
                ' If file exits then data will be appended.
                If Not IsTempFileOpen(batchFile) Then
                    Select Case Me.OutputFileFormat
                        Case FileFormat.Binary
                            If File.Exists(filename) Then
                                _outFileBinary = New BinaryWriter(File.Open(filename, FileMode.Append), Me.FileEncoding)
                            Else
                                _outFileBinary = New BinaryWriter(File.Open(filename, FileMode.CreateNew), Me.FileEncoding)
                            End If
                        Case FileFormat.Text
                            _outFileText = New StreamWriter(filename, True, Me.FileEncoding)
                    End Select
                End If
            Else
                ' Open the file for processing if it is not already open.
                ' If file exits then data will be appended.
                If Not IsTempFileOpen(batchFile) Then
                    Select Case Me.OutputFileFormat
                        Case FileFormat.Binary
                            If File.Exists(filename) Then
                                _outControlFileBinary = New BinaryWriter(File.Open(filename, FileMode.Append), Me.FileEncoding)
                            Else
                                _outControlFileBinary = New BinaryWriter(File.Open(filename, FileMode.CreateNew), Me.FileEncoding)
                            End If
                        Case FileFormat.Text
                            _outControlFileText = New StreamWriter(filename, True, Me.FileEncoding)
                    End Select
                End If
            End If

        End Sub

        ''' <summary>
        ''' Closes the output file.  Defaults to the batch output file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CloseTempFile()
            CloseTempFile(True)
        End Sub

        ''' <summary>
        ''' Closes the output file.  If batchFile is true, it will close the 
        ''' batch file.  If batch file is false, it will close the control file.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CloseTempFile(ByVal batchFile As Boolean)
            Try
                If batchFile Then
                    Select Case Me.OutputFileFormat
                        Case FileFormat.Binary
                            _outFileBinary.Flush()
                            _outFileBinary.Close()
                            _outFileBinary = Nothing
                        Case FileFormat.Text
                            _outFileText.Flush()
                            _outFileText.Close()
                            _outFileText = Nothing
                    End Select
                Else
                    Select Case Me.OutputFileFormat
                        Case FileFormat.Binary
                            _outControlFileBinary.Flush()
                            _outControlFileBinary.Close()
                            _outControlFileBinary = Nothing
                        Case FileFormat.Text
                            _outControlFileText.Flush()
                            _outControlFileText.Close()
                            _outControlFileText = Nothing
                    End Select
                End If
            Catch ignore As Exception
            End Try
        End Sub

        ''' <summary>
        ''' Writes the contents of a String to the output file.  
        ''' </summary>
        ''' <param name="line"></param>
        ''' <remarks></remarks>
        Protected Sub WriteLine(ByVal line As String)
            WriteLine(line, True)
        End Sub

        ''' <summary>
        ''' Writes the contents of a String to the output file.  If batchFile is true, it will write to the 
        ''' batch file.  If batch file is false, it will write to the control file.
        ''' </summary>
        ''' <param name="line"></param>
        ''' <remarks></remarks>
        Protected Sub WriteLine(ByVal line As String, ByVal batchFile As Boolean)
            ' write the line to the file
            If batchFile Then
                Select Case Me.OutputFileFormat
                    Case FileFormat.Binary
                        _outFileBinary.Write(line.ToCharArray)      'use CharArray rather than string so byte prefix won't be included
                        _outFileBinary.Write(vbNewLine)
                        _outFileBinary.Flush()
                    Case FileFormat.Text
                        _outFileText.WriteLine(line)
                End Select
            Else
                Select Case Me.OutputFileFormat
                    Case FileFormat.Binary
                        _outControlFileBinary.Write(line.ToCharArray)      'use CharArray rather than string so byte prefix won't be included
                        _outControlFileBinary.Write(vbNewLine)
                        _outControlFileBinary.Flush()
                    Case FileFormat.Text
                        _outControlFileText.WriteLine(line)
                End Select
            End If
        End Sub

        Protected Sub WriteLine(ByVal line As String, ByVal batchFile As Boolean, ByVal IncludeCRLF As Boolean)
            ' write the line to the file
            If batchFile Then
                Select Case Me.OutputFileFormat
                    Case FileFormat.Binary
                        _outFileBinary.Write(line.ToCharArray)      'use CharArray rather than string so byte prefix won't be included
                        If IncludeCRLF Then
                            _outFileBinary.Write(vbNewLine)
                        End If
                        _outFileBinary.Flush()
                    Case FileFormat.Text
                        _outFileText.WriteLine(line)
                End Select
            Else
                Select Case Me.OutputFileFormat
                    Case FileFormat.Binary
                        _outControlFileBinary.Write(line.ToCharArray)      'use CharArray rather than string so byte prefix won't be included
                        If IncludeCRLF Then
                            _outControlFileBinary.Write(vbNewLine)
                        End If
                        _outControlFileBinary.Flush()
                    Case FileFormat.Text
                        _outControlFileText.WriteLine(line)
                End Select
            End If
        End Sub
        Public Function Copy() As BaseWriter
            Return CType(Me.MemberwiseClone(), BaseWriter)
        End Function
#End Region

        ''' <summary>
        ''' Log an error and throw a new POSWriterInitializationException.
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="innerException"></param>
        ''' <remarks></remarks>
        Protected MustOverride Sub throwException(ByVal message As String, Optional ByVal innerException As Exception = Nothing)

    End Class

End Namespace
