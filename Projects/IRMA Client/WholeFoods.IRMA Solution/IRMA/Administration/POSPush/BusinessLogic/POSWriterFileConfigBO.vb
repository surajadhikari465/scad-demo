Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Administration.POSPush.BusinessLogic

    Public Enum POSWriterFileConfigStatus
        Valid
        Error_Integer_MaxWidth
        Error_Integer_MaxWidth_Decimal
        Error_Integer_MaxWidth_DecimalPrecision
        Error_Integer_MaxWidth_PackedDecimal
        Error_Integer_MaxWidth_PackedDecimalPrecision
        Error_Integer_PackLength
        Error_Required_DataElementType
        Error_Required_DynamicDataElement
        Error_Required_FieldId
        Error_Required_FillChar
        Error_Required_LiteralDataElement
        Error_Required_MaxWidth
        Error_Required_MaxWidth_Decimal
        Error_Required_MaxWidth_DecimalPrecision
        Error_Required_MaxWidth_PackedDecimal
        Error_Required_MaxWidth_PackedDecimalPrecision
        Error_Required_TaxFlagDataElement
        Error_Required_BooleanTrueChar
        Error_Required_BooleanFalseChar
        Error_Required_PackLength
    End Enum

    Public Enum DataElementType
        [Nothing] 'added because value defaults to "0" if nothing is selected;  item "0" is first value in the Enum list, which would be "Literal"
        Literal
        Dynamic
        TaxFlag
    End Enum

    Public Class POSWriterFileConfigBO

#Region "Property Definitions"
        Private _POSFileWriterKey As Integer
        Private _POSFileWriterCode As String
        Private _POSChangeTypeKey As Integer
        Private _changeTypeDesc As String
        Private _posDataTypeKey As Integer
        Private _rowOrder As Integer
        Private _columnOrder As Integer
        Private _bitOrder As Integer
        Private _enforceDictionary As Boolean
        Private _fixedWidth As Boolean      ' Flag set at the writer level - all fields for the writer are fixed width
        Private _appendToFile As Boolean
        Private _dataElementType As DataElementType
        Private _dataElement As String
        Private _fieldId As String
        Private _maxFieldWidth As String ' String instead of Integer so the UI doesn't see 0 for null
        Private _truncLeft As Boolean
        Private _defaultValue As String
        Private _isTaxFlag As Boolean
        Private _isLiteral As Boolean
        Private _isPackedDecimal As Boolean
        Private _packLength As String ' String instead of Integer so the UI doesn't see 0 for null
        Private _isBinaryInt As Boolean
        Private _isDecimalValue As Boolean
        Private _decimalPrecision As String ' String instead of Integer so the UI doesn't see 0 for null
        Private _includeDecimal As Boolean
        Private _isBoolean As Boolean
        Private _booleanTrueChar As String
        Private _booleanFalseChar As String
        Private _padLeft As Boolean
        Private _fillChar As String
        Private _leadingChars As String
        Private _trailingChars As String
        Private _fixedWidthField As Boolean     ' Flag set for writers that are not fixed width to indicate a particular field is fixed width
#End Region

#Region "constructors"
        ''' <summary>
        ''' Create a new instance of the object
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Create a new instance of the object, populated from the DataGridViewRow
        ''' for a selected row on the UI.
        ''' </summary>
        ''' <param name="selectedRow"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef selectedRow As DataGridViewRow)
            _POSFileWriterCode = CType(selectedRow.Cells("POSFileWriterCode").Value, String)
            _changeTypeDesc = CType(selectedRow.Cells("ChangeTypeDesc").Value, String)
            _rowOrder = CType(selectedRow.Cells("RowOrder").Value, Integer)
            _columnOrder = CType(selectedRow.Cells("ColumnOrder").Value, Integer)
            If selectedRow.Cells("BitOrder").Value.GetType IsNot GetType(DBNull) Then
                _bitOrder = CType(selectedRow.Cells("BitOrder").Value, Integer)
            Else
                _bitOrder = 0
            End If
            _isLiteral = CType(selectedRow.Cells("IsLiteral").Value, Boolean)
            _isTaxFlag = CType(selectedRow.Cells("IsTaxFlag").Value, Boolean)
            _dataElement = CType(selectedRow.Cells("DataElement").Value, String)
            _isBoolean = CType(selectedRow.Cells("IsBoolean").Value, Boolean)

            If selectedRow.Cells("IsBinaryInt").Value.GetType IsNot GetType(DBNull) Then
                _isBinaryInt = CType(selectedRow.Cells("IsBinaryInt").Value, Boolean)
            End If

            If selectedRow.Cells("IsPackedDecimal").Value.GetType IsNot GetType(DBNull) Then
                _isPackedDecimal = CType(selectedRow.Cells("IsPackedDecimal").Value, Boolean)
            End If

            If selectedRow.Cells("FieldId").Value.GetType IsNot GetType(DBNull) Then
                _fieldId = CType(selectedRow.Cells("FieldId").Value, String)
            Else
                _fieldId = Nothing
            End If

            If selectedRow.Cells("MaxFieldWidth").Value.GetType IsNot GetType(DBNull) Then
                _maxFieldWidth = CType(selectedRow.Cells("MaxFieldWidth").Value, String)
            Else
                _maxFieldWidth = Nothing
            End If

            If selectedRow.Cells("TruncLeft").Value.GetType IsNot GetType(DBNull) Then
                _truncLeft = CType(selectedRow.Cells("TruncLeft").Value, Boolean)
            Else
                _truncLeft = Nothing
            End If

            If selectedRow.Cells("DefaultValue").Value.GetType IsNot GetType(DBNull) Then
                _defaultValue = CType(selectedRow.Cells("DefaultValue").Value, String)
            Else
                _defaultValue = Nothing
            End If

            If selectedRow.Cells("IsDecimalValue").Value.GetType IsNot GetType(DBNull) Then
                _isDecimalValue = CType(selectedRow.Cells("IsDecimalValue").Value, Boolean)
            End If

            If selectedRow.Cells("DecimalPrecision").Value.GetType IsNot GetType(DBNull) Then
                _decimalPrecision = CType(selectedRow.Cells("DecimalPrecision").Value, String)
            Else
                _decimalPrecision = Nothing
            End If

            If selectedRow.Cells("IncludeDecimal").Value.GetType IsNot GetType(DBNull) Then
                _includeDecimal = CType(selectedRow.Cells("IncludeDecimal").Value, Boolean)
            End If

            If selectedRow.Cells("PadLeft").Value.GetType IsNot GetType(DBNull) Then
                _padLeft = CType(selectedRow.Cells("PadLeft").Value, Boolean)
            End If

            If selectedRow.Cells("FillChar").Value.GetType IsNot GetType(DBNull) Then
                _fillChar = CType(selectedRow.Cells("FillChar").Value, String)
            Else
                _fillChar = Nothing
            End If

            If selectedRow.Cells("LeadingChars").Value.GetType IsNot GetType(DBNull) Then
                _leadingChars = CType(selectedRow.Cells("LeadingChars").Value, String)
            Else
                _leadingChars = Nothing
            End If

            If selectedRow.Cells("TrailingChars").Value.GetType IsNot GetType(DBNull) Then
                _trailingChars = CType(selectedRow.Cells("TrailingChars").Value, String)
            Else
                _trailingChars = Nothing
            End If

            If selectedRow.Cells("BooleanTrueChar").Value.GetType IsNot GetType(DBNull) Then
                _booleanTrueChar = CType(selectedRow.Cells("BooleanTrueChar").Value, String)
            Else
                _booleanTrueChar = Nothing
            End If

            If selectedRow.Cells("BooleanFalseChar").Value.GetType IsNot GetType(DBNull) Then
                _booleanFalseChar = CType(selectedRow.Cells("BooleanFalseChar").Value, String)
            Else
                _booleanFalseChar = Nothing
            End If

            If selectedRow.Cells("FixedWidthField").Value.GetType IsNot GetType(DBNull) Then
                _fixedWidthField = CType(selectedRow.Cells("FixedWidthField").Value, Boolean)
            Else
                _fixedWidthField = Nothing
            End If

            If selectedRow.Cells("PackLength").Value.GetType IsNot GetType(DBNull) Then
                _packLength = CType(selectedRow.Cells("PackLength").Value, String)
            Else
                _packLength = Nothing
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

        Public Property POSChangeTypeKey() As Integer
            Get
                Return _POSChangeTypeKey
            End Get
            Set(ByVal value As Integer)
                _POSChangeTypeKey = value
            End Set
        End Property

        Public Property ChangeTypeDesc() As String
            Get
                Return _changeTypeDesc
            End Get
            Set(ByVal value As String)
                _changeTypeDesc = value
            End Set
        End Property

        Public Property POSDataTypeKey() As Integer
            Get
                Return _posDataTypeKey
            End Get
            Set(ByVal value As Integer)
                _posDataTypeKey = value
            End Set
        End Property

        Public Property RowOrder() As Integer
            Get
                Return _rowOrder
            End Get
            Set(ByVal value As Integer)
                _rowOrder = value
            End Set
        End Property

        Public Property ColumnOrder() As Integer
            Get
                Return _columnOrder
            End Get
            Set(ByVal value As Integer)
                _columnOrder = value
            End Set
        End Property

        Public Property BitOrder() As Integer
            Get
                Return _bitOrder
            End Get
            Set(ByVal value As Integer)
                _bitOrder = value
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

        Public Property FixedWidth() As Boolean
            Get
                Return _fixedWidth
            End Get
            Set(ByVal value As Boolean)
                _fixedWidth = value
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

        Public Property DataElementType() As DataElementType
            Get
                Return _dataElementType
            End Get
            Set(ByVal value As DataElementType)
                _dataElementType = value
            End Set
        End Property

        Public Property DataElement() As String
            Get
                Return _dataElement
            End Get
            Set(ByVal value As String)
                _dataElement = value
            End Set
        End Property

        Public Property FieldId() As String
            Get
                Return _fieldId
            End Get
            Set(ByVal value As String)
                _fieldId = value
            End Set
        End Property

        Public Property MaxFieldWidth() As String
            Get
                Return _maxFieldWidth
            End Get
            Set(ByVal value As String)
                _maxFieldWidth = value
            End Set
        End Property

        Public Property TruncLeft() As Boolean
            Get
                Return _truncLeft
            End Get
            Set(ByVal value As Boolean)
                _truncLeft = value
            End Set
        End Property

        Public Property DefaultValue() As String
            Get
                Return _defaultValue
            End Get
            Set(ByVal value As String)
                _defaultValue = value
            End Set
        End Property

        Public Property IsTaxFlag() As Boolean
            Get
                Return _isTaxFlag
            End Get
            Set(ByVal value As Boolean)
                _isTaxFlag = value
            End Set
        End Property

        Public Property IsLiteral() As Boolean
            Get
                Return _isLiteral
            End Get
            Set(ByVal value As Boolean)
                _isLiteral = value
            End Set
        End Property

        Public Property IsPackedDecimal() As Boolean
            Get
                Return _isPackedDecimal
            End Get
            Set(ByVal value As Boolean)
                _isPackedDecimal = value
            End Set
        End Property

        Public Property IsBinaryInt() As Boolean
            Get
                Return _isBinaryInt
            End Get
            Set(ByVal value As Boolean)
                _isBinaryInt = value
            End Set
        End Property

        Public Property IsDecimalValue() As Boolean
            Get
                Return _isDecimalValue
            End Get
            Set(ByVal value As Boolean)
                _isDecimalValue = value
            End Set
        End Property

        Public Property DecimalPrecision() As String
            Get
                Return _decimalPrecision
            End Get
            Set(ByVal value As String)
                _decimalPrecision = value
            End Set
        End Property

        Public Property IncludeDecimal() As Boolean
            Get
                Return _includeDecimal
            End Get
            Set(ByVal value As Boolean)
                _includeDecimal = value
            End Set
        End Property

        Public Property IsBoolean() As Boolean
            Get
                Return _isBoolean
            End Get
            Set(ByVal value As Boolean)
                _isBoolean = value
            End Set
        End Property

        Public Property BooleanTrueChar() As String
            Get
                Return _booleanTrueChar
            End Get
            Set(ByVal value As String)
                _booleanTrueChar = value
            End Set
        End Property

        Public Property BooleanFalseChar() As String
            Get
                Return _booleanFalseChar
            End Get
            Set(ByVal value As String)
                _booleanFalseChar = value
            End Set
        End Property

        Public Property PadLeft() As Boolean
            Get
                Return _padLeft
            End Get
            Set(ByVal value As Boolean)
                _padLeft = value
            End Set
        End Property

        Public Property FillChar() As String
            Get
                Return _fillChar
            End Get
            Set(ByVal value As String)
                _fillChar = value
            End Set
        End Property

        Public Property LeadingChars() As String
            Get
                Return _leadingChars
            End Get
            Set(ByVal value As String)
                _leadingChars = value
            End Set
        End Property

        Public Property TrailingChars() As String
            Get
                Return _trailingChars
            End Get
            Set(ByVal value As String)
                _trailingChars = value
            End Set
        End Property

        Public Property FixedWidthField() As Boolean
            Get
                Return _fixedWidthField
            End Get
            Set(ByVal value As Boolean)
                _fixedWidthField = value
            End Set
        End Property

        Public Property PackLength() As String
            Get
                Return _packLength
            End Get
            Set(ByVal value As String)
                _packLength = value
            End Set
        End Property

#End Region

#Region "Business rules"

        ''' <summary>
        ''' validates data elements of current instance of POSWriterFileConfigBO object
        ''' </summary>
        ''' <returns>ArrayList of POSWriterFileConfigStatus values</returns>
        ''' <remarks></remarks>
        Public Function ValidatePOSWriterData() As ArrayList
            Dim errorList As New ArrayList

            ' field is required for enforce dictionary = true
            If Me.EnforceDictionary AndAlso Me.FieldId.Equals("") Then
                errorList.Add(POSWriterFileConfigStatus.Error_Required_FieldId)
            End If

            ' validate based on the DataElementType
            Select Case Me.DataElementType
                Case BusinessLogic.DataElementType.Literal
                    If Me.DataElement.Equals("") Then
                        ' literal data element required
                        errorList.Add(POSWriterFileConfigStatus.Error_Required_LiteralDataElement)
                    End If
                Case BusinessLogic.DataElementType.Dynamic
                    ' dynamic data element required
                    If Me.DataElement.Equals("") Then
                        errorList.Add(POSWriterFileConfigStatus.Error_Required_DynamicDataElement)
                    End If
                Case BusinessLogic.DataElementType.TaxFlag
                    If Me.DataElement.Equals("") Then
                        ' tax flag required
                        errorList.Add(POSWriterFileConfigStatus.Error_Required_TaxFlagDataElement)
                    End If
                Case Else
                    'data element type is required
                    errorList.Add(POSWriterFileConfigStatus.Error_Required_DataElementType)
            End Select

            'perform data formatting value validation if needed
            If PerformDataFormattingValidation() Then
                ValidateDataFormattingValues(errorList)
            End If

            'no errors - return Valid status
            If errorList.Count = 0 Then
                errorList.Add(POSWriterFileConfigStatus.Valid)
            End If

            Return errorList
        End Function

        ''' <summary>
        ''' should data formatting values be obtained/validated?
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function PerformDataFormattingValidation() As Boolean
            Dim isGetValues As Boolean

            Select Case Me.DataElementType
                Case BusinessLogic.DataElementType.Literal
                    isGetValues = False
                Case BusinessLogic.DataElementType.Dynamic
                    isGetValues = True
                Case BusinessLogic.DataElementType.TaxFlag
                    If Me.DataElement.Equals(ResourcesAdministration.GetString("label_pirusActiveFlag")) Then
                        isGetValues = True
                    End If
                Case Else
                    isGetValues = False
            End Select

            Return isGetValues
        End Function

        Private Sub ValidateDataFormattingValues(ByRef errorList As ArrayList)
            'fill char required if fixed width = true
            If Me.FixedWidth AndAlso Me.FillChar.Equals("") Then
                errorList.Add(POSWriterFileConfigStatus.Error_Required_FillChar)
            End If

            If Me.IsDecimalValue Then
                'handle decimal value validation
                If Me.FixedWidth Then
                    ' max width is required for fixed width forms
                    If Me.MaxFieldWidth.Equals("") Then
                        errorList.Add(POSWriterFileConfigStatus.Error_Required_MaxWidth_Decimal)
                    End If

                    ' decimal precision max width required
                    If Me.DecimalPrecision.Equals("") Then
                        errorList.Add(POSWriterFileConfigStatus.Error_Required_MaxWidth_DecimalPrecision)
                    End If
                End If

                ' max width must be an integer
                If Not Me.MaxFieldWidth.Equals("") Then
                    If Not DataValidationBO.IsIntegerString(Me.MaxFieldWidth) Then
                        errorList.Add(POSWriterFileConfigStatus.Error_Integer_MaxWidth_Decimal)
                    End If
                End If

                ' decimal precison max width must be an integer
                If Not Me.DecimalPrecision.Equals("") Then
                    If Not DataValidationBO.IsIntegerString(Me.DecimalPrecision) Then
                        errorList.Add(POSWriterFileConfigStatus.Error_Integer_MaxWidth_DecimalPrecision)
                    End If
                End If
            ElseIf Me.IsBinaryInt Then
                'no additional validation
            ElseIf Me.IsPackedDecimal Then
                ' pack length is required
                If Me.PackLength.Equals("") Then
                    errorList.Add(POSWriterFileConfigStatus.Error_Required_PackLength)
                End If

                ' pack length must be an integer
                If Not DataValidationBO.IsIntegerString(Me.PackLength) Then
                    errorList.Add(POSWriterFileConfigStatus.Error_Integer_PackLength)
                End If

                'handle decimal value validation
                If Me.FixedWidth Then
                    ' max width is required for fixed width forms
                    If Me.MaxFieldWidth.Equals("") Then
                        errorList.Add(POSWriterFileConfigStatus.Error_Required_MaxWidth_PackedDecimal)
                    End If

                    ' decimal precision max width required
                    If Me.DecimalPrecision.Equals("") Then
                        errorList.Add(POSWriterFileConfigStatus.Error_Required_MaxWidth_PackedDecimalPrecision)
                    End If
                End If

                ' max width must be an integer
                If Not Me.MaxFieldWidth.Equals("") Then
                    If Not DataValidationBO.IsIntegerString(Me.MaxFieldWidth) Then
                        errorList.Add(POSWriterFileConfigStatus.Error_Integer_MaxWidth_PackedDecimal)
                    End If
                End If

                ' decimal precison max width must be an integer
                If Not Me.DecimalPrecision.Equals("") Then
                    If Not DataValidationBO.IsIntegerString(Me.DecimalPrecision) Then
                        errorList.Add(POSWriterFileConfigStatus.Error_Integer_MaxWidth_PackedDecimalPrecision)
                    End If
                End If

                ElseIf Me.IsBoolean Then
                    ' handle boolean validation
                    ' true and false characters must be specified
                    If Me.BooleanTrueChar Is Nothing Or Me.BooleanTrueChar.Equals("") Then
                        errorList.Add(POSWriterFileConfigStatus.Error_Required_BooleanTrueChar)
                    End If
                    If Me.BooleanFalseChar Is Nothing Or Me.BooleanFalseChar.Equals("") Then
                        errorList.Add(POSWriterFileConfigStatus.Error_Required_BooleanFalseChar)
                    End If
                ElseIf Not Me.IsDecimalValue AndAlso Not Me.IsBoolean Then
                    ' handle other value validation
                    If Me.FixedWidth Then
                        ' max width is required for fixed width forms
                        If Me.MaxFieldWidth.Equals("") Then
                            errorList.Add(POSWriterFileConfigStatus.Error_Required_MaxWidth)
                        End If

                        ' max width must be an integer
                        If Not DataValidationBO.IsIntegerString(Me.MaxFieldWidth) Then
                            errorList.Add(POSWriterFileConfigStatus.Error_Integer_MaxWidth)
                        End If
                    End If
                End If
        End Sub

#End Region

    End Class
End Namespace

