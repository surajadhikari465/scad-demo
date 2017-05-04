Imports Infragistics.Win.UltraWinGrid

Namespace WholeFoods.IRMA.Administration.POSPush.BusinessLogic
    Public Enum BatchIdDefaultType
        WriterChange
        PriceChange
        ItemChange
    End Enum

    Public Enum BatchIdDefaultBOStatus
        Valid
        Error_Integer_BatchIdDefault
    End Enum

    Public Class BatchIdDefaultBO
        Private _batchIdType As BatchIdDefaultType
        Private _posFileWriterKey As Integer
        Private _changeTypeId As Integer
        Private _changeTypeDesc As String
        Private _batchIdDefault As String

#Region "Property Accessors"
        Public Property BatchIdType() As BatchIdDefaultType
            Get
                Return _batchIdType
            End Get
            Set(ByVal value As BatchIdDefaultType)
                _batchIdType = value
            End Set
        End Property

        Public Property POSFileWriterKey() As Integer
            Get
                Return _posFileWriterKey
            End Get
            Set(ByVal value As Integer)
                _posFileWriterKey = value
            End Set
        End Property

        Public Property ChangeTypeId() As Integer
            Get
                Return _changeTypeId
            End Get
            Set(ByVal value As Integer)
                _changeTypeId = value
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

        Public Property BatchIdDefault() As String
            Get
                Return _batchIdDefault
            End Get
            Set(ByVal value As String)
                _batchIdDefault = value
            End Set
        End Property
#End Region

#Region "Constructors"
        ''' <summary>
        ''' Create a new business object.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal type As BatchIdDefaultType)
            _batchIdType = type
        End Sub
#End Region

#Region "Methods to populate from a data grid row"
        ''' <summary>
        ''' Populate the business object from the selected price change type row.
        ''' </summary>
        ''' <param name="currentRow"></param>
        ''' <param name="currentWriter"></param>
        ''' <remarks></remarks>
        Public Sub PopulateFromSelectedPriceChangeType(ByRef currentRow As UltraGridRow, ByRef currentWriter As POSWriterBO)
            If Not currentRow.Cells("POSFileWriterKey").Value.Equals(DBNull.Value) Then
                _posFileWriterKey = CInt(currentRow.Cells("POSFileWriterKey").Value)
            Else
                _posFileWriterKey = currentWriter.POSFileWriterKey
            End If
            _changeTypeId = CInt(currentRow.Cells("PriceChgTypeId").Value)
            _changeTypeDesc = currentRow.Cells("PriceChgTypeDesc").Value.ToString
            If Not currentRow.Cells("POSBatchIdDefault").Value.Equals(DBNull.Value) Then
                _batchIdDefault = currentRow.Cells("POSBatchIdDefault").Value.ToString
            End If
        End Sub

        ''' <summary>
        ''' Populate the business object from the selected item change type row.
        ''' </summary>
        ''' <param name="currentRow"></param>
        ''' <param name="currentWriter"></param>
        ''' <remarks></remarks>
        Public Sub PopulateFromSelectedItemChangeType(ByRef currentRow As UltraGridRow, ByRef currentWriter As POSWriterBO)
            If Not currentRow.Cells("POSFileWriterKey").Value.Equals(DBNull.Value) Then
                _posFileWriterKey = CInt(currentRow.Cells("POSFileWriterKey").Value)
            Else
                _posFileWriterKey = currentWriter.POSFileWriterKey
            End If
            _changeTypeId = CInt(currentRow.Cells("ItemChgTypeId").Value)
            _changeTypeDesc = currentRow.Cells("ItemChgTypeDesc").Value.ToString
            If Not currentRow.Cells("POSBatchIdDefault").Value.Equals(DBNull.Value) Then
                _batchIdDefault = currentRow.Cells("POSBatchIdDefault").Value.ToString
            End If
        End Sub

        ''' <summary>
        ''' Populate the business object from the selected writer change type row.
        ''' </summary>
        ''' <param name="currentRow"></param>
        ''' <param name="currentWriter"></param>
        ''' <remarks></remarks>
        Public Sub PopulateFromSelectedWriterChangeType(ByRef currentRow As UltraGridRow, ByRef currentWriter As POSWriterBO)
            If Not currentRow.Cells("POSFileWriterKey").Value.Equals(DBNull.Value) Then
                _posFileWriterKey = CInt(currentRow.Cells("POSFileWriterKey").Value)
            Else
                _posFileWriterKey = currentWriter.POSFileWriterKey
            End If
            _changeTypeId = CInt(currentRow.Cells("POSChangeTypeKey").Value)
            _changeTypeDesc = currentRow.Cells("ChangeTypeDesc").Value.ToString
            If Not currentRow.Cells("POSBatchIdDefault").Value.Equals(DBNull.Value) Then
                _batchIdDefault = currentRow.Cells("POSBatchIdDefault").Value.ToString
            End If
        End Sub

#End Region

#Region "Business rules"
        ''' <summary>
        ''' performs data validation and returns array of BatchIdDefaultBOStatus values
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateData() As ArrayList
            Dim statusList As New ArrayList

            ' validate the batch id default value is numeric
            If Not Me.BatchIdDefault.Equals("") Then
                If Not DataValidationBO.IsIntegerString(Me.BatchIdDefault) Then
                    statusList.Add(BatchIdDefaultBOStatus.Error_Integer_BatchIdDefault)
                End If
            End If

            If statusList.Count = 0 Then
                'data is valid
                statusList.Add(BatchIdDefaultBOStatus.Valid)
            End If

            Return statusList
        End Function
#End Region
    End Class
End Namespace
