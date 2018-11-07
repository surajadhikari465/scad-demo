Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Administration.POSPush.BusinessLogic

    Public Enum POSWriterBOStatus
        Valid
        Error_Required_Delimiter
        Error_Required_FileWriterCode
        Error_Required_FileWriterType
        Error_Required_WriterClass
        Error_Required_ScaleWriterType
        Error_Integer_BatchIdMin
        Error_Integer_BatchIdMax
        Error_Range_BatchId
    End Enum

    Public Class POSWriterBO

#Region "Property Definitions"
        ' Values for the Writer Types
        Public Const WRITER_TYPE_SCALE As String = "SCALE"
        Public Const WRITER_TYPE_POS As String = "POS"
        Public Const WRITER_TYPE_TAG As String = "TAG"
        Public Const WRITER_TYPE_ELECTRONICSHELFTAG As String = "EST"
        Public Const WRITER_TYPE_TLOG As String = "TLOG"
        Public Const WRITER_TYPE_POSPULL As String = "POSPull"
        Public Const WRITER_TYPE_PLUMSTORE As String = "PLUMStore"
        Public Const WRITER_TYPE_REPRINTTAG As String = "REPRINTTAG"
        Public Const WRITER_TYPE_FILEIN As String = "FileIn"
        Public Const WRITER_TYPE_FILEOUT As String = "FileOut"

        ' Values for the Scale Types
        Public Const SCALE_WRITER_TYPE_STORE As String = "STORE"
        Public Const SCALE_WRITER_TYPE_CORPORATE As String = "CORPORATE"
        Public Const SCALE_WRITER_TYPE_SMARTX_ZONE As String = "SMARTX ZONE"
        Public Const SCALE_WRITER_TYPE_ZONE As String = "ZONE"

        ' POS Writer properties
        Private _POSFileWriterKey As Integer
        Private _POSFileWriterCode As String
        Private _POSFileWriterClass As String
        Private _delimChar As String
        Private _leadingDelim As Boolean
        Private _trailingDelim As Boolean
        Private _fieldIdDelim As Boolean
        Private _outputByIrmaBatches As Boolean
        Private _fixedWidth As Boolean
        Private _disabled As Boolean
        Private _taxFlagTrueChar As String
        Private _taxFlagFalseChar As String
        Private _enforceDictionary As Boolean
        Private _escapeCharCount As Integer
        Private _escapeChars As Hashtable  'key=EscapeCharValue; value=EscapeCharReplacement
        Private _appendToFile As Boolean
        Private _writerType As String
        Private _scaleWriterType As String
        Private _scaleWriterTypeDesc As String
        Private _batchIdMin As String
        Private _batchIdMax As String
#End Region

#Region "constructors and helper methods to initialize the data"
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Create a new instance of the object, populated from the DataGridViewRow
        ''' for a selected row on the UI.
        ''' </summary>
        ''' <param name="selectedRow"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef selectedRow As DataGridViewRow)
            _POSFileWriterKey = CType(selectedRow.Cells("POSFileWriterKey").Value, Integer)
            _POSFileWriterCode = CType(selectedRow.Cells("POSFileWriterCode").Value, String)
            _POSFileWriterClass = CType(selectedRow.Cells("POSFileWriterClass").Value, String)

            If selectedRow.Cells("ScaleWriterType").Value IsNot DBNull.Value Then
                _scaleWriterType = CType(selectedRow.Cells("ScaleWriterType").Value, String)
            End If

            If selectedRow.Cells("ScaleWriterTypeDesc").Value IsNot DBNull.Value Then
                _scaleWriterTypeDesc = CType(selectedRow.Cells("ScaleWriterTypeDesc").Value, String)
            End If

            If selectedRow.Cells("DelimChar").Value IsNot DBNull.Value Then
                _delimChar = CType(selectedRow.Cells("DelimChar").Value, String)
            End If

            _fixedWidth = CType(selectedRow.Cells("FixedWidth").Value, Boolean)

            If selectedRow.Cells("OutputByIrmaBatches").Value IsNot DBNull.Value Then
                _outputByIrmaBatches = CType(selectedRow.Cells("OutputByIrmaBatches").Value, Boolean)
            End If

            If selectedRow.Cells("AppendToFile").Value IsNot DBNull.Value Then
                _appendToFile = CType(selectedRow.Cells("AppendToFile").Value, Boolean)
            End If

            If selectedRow.Cells("LeadingDelim").Value IsNot DBNull.Value Then
                _leadingDelim = CType(selectedRow.Cells("LeadingDelim").Value, Boolean)
            End If

            If selectedRow.Cells("TrailingDelim").Value IsNot DBNull.Value Then
                _trailingDelim = CType(selectedRow.Cells("TrailingDelim").Value, Boolean)
            End If

            If selectedRow.Cells("FieldIdDelim").Value IsNot DBNull.Value Then
                _fieldIdDelim = CType(selectedRow.Cells("FieldIdDelim").Value, Boolean)
            End If

            _disabled = CType(selectedRow.Cells("Disabled").Value, Boolean)

            If selectedRow.Cells("TaxFlagTrueChar").Value IsNot DBNull.Value Then
                _taxFlagTrueChar = CType(selectedRow.Cells("TaxFlagTrueChar").Value, String).Trim
            End If

            If selectedRow.Cells("TaxFlagFalseChar").Value IsNot DBNull.Value Then
                _taxFlagFalseChar = CType(selectedRow.Cells("TaxFlagFalseChar").Value, String).Trim
            End If

            _enforceDictionary = CType(selectedRow.Cells("EnforceDictionary").Value, Boolean)
            _escapeCharCount = CType(selectedRow.Cells("EscapeCharCount").Value, Integer)
            _writerType = CType(selectedRow.Cells("FileWriterType").Value, String)

            If selectedRow.Cells("BatchIdMin").Value IsNot DBNull.Value Then
                _batchIdMin = CType(selectedRow.Cells("BatchIdMin").Value, String)
            End If

            If selectedRow.Cells("BatchIdMax").Value IsNot DBNull.Value Then
                _batchIdMax = CType(selectedRow.Cells("BatchIdMax").Value, String)
            End If
        End Sub
#End Region

#Region "Property access methods"
        Public Property POSFileWriterKey() As Integer
            Get
                Return _POSFileWriterKey
            End Get
            Set(ByVal value As Integer)
                _POSFileWriterKey = value
            End Set
        End Property

        Public Property POSFileWriterCode() As String
            Get
                Return _POSFileWriterCode
            End Get
            Set(ByVal value As String)
                _POSFileWriterCode = value
            End Set
        End Property

        Public Property POSFileWriterClass() As String
            Get
                Return _POSFileWriterClass
            End Get
            Set(ByVal value As String)
                _POSFileWriterClass = value
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

        Public Property LeadingDelimiter() As Boolean
            Get
                Return _leadingDelim
            End Get
            Set(ByVal value As Boolean)
                _leadingDelim = value
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

        Public Property TrailingDelimiter() As Boolean
            Get
                Return _trailingDelim
            End Get
            Set(ByVal value As Boolean)
                _trailingDelim = value
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

        Public Property TaxFlagTrueChar() As String
            Get
                Return _taxFlagTrueChar
            End Get
            Set(ByVal value As String)
                _taxFlagTrueChar = value
            End Set
        End Property

        Public Property TaxFlagFalseChar() As String
            Get
                Return _taxFlagFalseChar
            End Get
            Set(ByVal value As String)
                _taxFlagFalseChar = value
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

        Public Property Disabled() As Boolean
            Get
                Return _disabled
            End Get
            Set(ByVal value As Boolean)
                _disabled = value
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

        Public Property WriterType() As String
            Get
                Return _writerType
            End Get
            Set(ByVal value As String)
                _writerType = value
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

        Public Property ScaleWriterTypeDesc() As String
            Get
                Return _scaleWriterTypeDesc
            End Get
            Set(ByVal value As String)
                _scaleWriterTypeDesc = value
            End Set
        End Property

        Public Property BatchIdMin() As String
            Get
                Return _batchIdMin
            End Get
            Set(ByVal value As String)
                _batchIdMin = value
            End Set
        End Property

        Public Property BatchIdMax() As String
            Get
                Return _batchIdMax
            End Get
            Set(ByVal value As String)
                _batchIdMax = value
            End Set
        End Property
#End Region

#Region "Business rules"

        ''' <summary>
        ''' performs data validation and returns array of POSWriterBOStatus values
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidatePOSWriterData() As ArrayList
            Dim statusList As New ArrayList

            'file writer type is required
            Dim writerSelected As Boolean = True
            If Me.WriterType Is Nothing Or (Me.WriterType IsNot Nothing AndAlso Me.WriterType.Trim.Equals("")) Then
                statusList.Add(POSWriterBOStatus.Error_Required_FileWriterType)
                writerSelected = False
            End If

            'pos writer code is required
            If Me.POSFileWriterCode Is Nothing Or (Me.POSFileWriterCode IsNot Nothing AndAlso Me.POSFileWriterCode.Trim.Equals("")) Then
                statusList.Add(POSWriterBOStatus.Error_Required_FileWriterCode)
            End If

            'pos writer class is required
            If Me.POSFileWriterClass Is Nothing Or (Me.POSFileWriterClass IsNot Nothing AndAlso Me.POSFileWriterClass.Trim.Equals("")) Then
                statusList.Add(POSWriterBOStatus.Error_Required_WriterClass)
            End If

            'scale writer type is required for scale writers
            If (writerSelected AndAlso Me.WriterType = WRITER_TYPE_SCALE) AndAlso (Me.ScaleWriterType Is Nothing Or (Me.ScaleWriterType IsNot Nothing AndAlso Me.ScaleWriterType.Trim.Equals(""))) Then
                statusList.Add(POSWriterBOStatus.Error_Required_ScaleWriterType)
            End If

            'if leading/trailing/field id delim is checked then delim char is required
            If (Me.LeadingDelimiter Or Me.TrailingDelimiter Or Me.FieldIdDelim) AndAlso Me.DelimChar.Equals("") Then
                statusList.Add(POSWriterBOStatus.Error_Required_Delimiter)
            End If

            ' validate the min and max batch id values are numeric
            Dim theMinbatchID As Integer = 0
            Dim theMaxbatchID As Integer = 0

            If Not Me.BatchIdMin.Equals("") Then
                If Not Integer.TryParse(Me.BatchIdMin, theMinbatchID) Then
                    statusList.Add(POSWriterBOStatus.Error_Integer_BatchIdMin)
                Else
                    If theMinbatchID < 500 Or theMinbatchID > 999 Then
                        statusList.Add(POSWriterBOStatus.Error_Range_BatchId)
                    End If
                End If
            End If

            If Not Me.BatchIdMax.Equals("") Then
                If Not Integer.TryParse(Me.BatchIdMax, theMaxbatchID) Then
                    statusList.Add(POSWriterBOStatus.Error_Integer_BatchIdMax)
                Else
                    If theMaxbatchID < 500 Or theMaxbatchID > 999 Then
                        statusList.Add(POSWriterBOStatus.Error_Range_BatchId)
                    End If

                    If theMaxbatchID < theMinbatchID Then
                        statusList.Add(POSWriterBOStatus.Error_Range_BatchId)
                    End If
                End If
            End If

            If statusList.Count = 0 Then
                'data is valid
                statusList.Add(POSWriterBOStatus.Valid)
            End If

            Return statusList
        End Function

#End Region

    End Class
End Namespace

