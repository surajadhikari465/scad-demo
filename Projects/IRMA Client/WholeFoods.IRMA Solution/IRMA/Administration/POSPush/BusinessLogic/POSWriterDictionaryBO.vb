Imports WholeFoods.Utility

Public Enum POSWriterDictionaryStatus
    Valid
    Error_ColumnsAssociated
End Enum

Namespace WholeFoods.IRMA.Administration.POSPush.BusinessLogic
    Public Class POSWriterDictionaryBO
#Region "Property Definitions"
        Private _POSFileWriterKey As Integer
        Private _fieldId As String
        Private _fieldIdCount As Integer
        Private _dataType As String
#End Region

#Region "constructors"
        ''' <summary>
        ''' Create a new instance of the object
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Create a new instance of the object from a current DataGridViewRow
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal currentRow As DataGridViewRow)
            If currentRow.Cells("POSFileWriterKey").Value IsNot DBNull.Value Then
                _POSFileWriterKey = CType(currentRow.Cells("POSFileWriterKey").Value, Integer)
            End If

            If currentRow.Cells("FieldId").Value IsNot DBNull.Value Then
                _fieldId = currentRow.Cells("FieldId").Value.ToString
            End If

            If currentRow.Cells("FieldIdCount").Value IsNot DBNull.Value Then
                _fieldIdCount = CType(currentRow.Cells("FieldIdCount").Value, Integer)
            End If

            If currentRow.Cells("DataType").Value IsNot DBNull.Value Then
                _dataType = currentRow.Cells("DataType").Value.ToString
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

        Public Property FieldId() As String
            Get
                Return _fieldId
            End Get
            Set(ByVal value As String)
                _fieldId = value
            End Set
        End Property

        Public Property FieldIdCount() As Integer
            Get
                Return _fieldIdCount
            End Get
            Set(ByVal value As Integer)
                _fieldIdCount = value
            End Set
        End Property

        Public Property DataType() As String
            Get
                Return _dataType
            End Get
            Set(ByVal value As String)
                _dataType = value
            End Set
        End Property
#End Region

        ''' <summary>
        ''' Can delete a data dictionary only when no POSWriterFileConfig definitions include the field id.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateDelete() As POSWriterDictionaryStatus
            Dim status As POSWriterDictionaryStatus

            ' Make sure that there are not any columns defined with this field id for the writer and POSDataType
            If FieldIdCount > 0 Then
                status = POSWriterDictionaryStatus.Error_ColumnsAssociated
            Else
                status = POSWriterDictionaryStatus.Valid
            End If

            Return status
        End Function
    End Class
End Namespace
