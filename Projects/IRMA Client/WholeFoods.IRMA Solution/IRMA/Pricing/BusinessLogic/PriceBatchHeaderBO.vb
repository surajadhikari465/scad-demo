Imports WholeFoods.IRMA.Pricing.DataAccess

Public Enum PriceBatchHeaderStatus
    Valid
    Error_POSBatchId_Numeric
    Error_POSBatchId_MinRange
    Error_POSBatchId_MaxRange
End Enum

Namespace WholeFoods.IRMA.Pricing.BusinessLogic

    Public Class PriceBatchHeaderBO

        Private _storeNo As Integer
        Private _itemChgTypeID As Integer
        Private _priceChgTypeID As Integer
        Private _startDate As Date
        Private _priceBatchDetailIDList As String
        Private _detailIDListSeparator As Char
        Private _priceBatchHeaderID As Integer
        Private _priceBatchStatusID As Integer
        Private _batchdescription As String
        Private _autoApplyFlag As Boolean
        Private _autoApplyDate As Date
        Private _subteamNo As Integer
        Private _subteamName As String
        Private _posBatchId As String
        Private _minBatchIdAllowed As Integer
        Private _maxBatchIdAllowed As Integer

#Region "Constructors"
       
#End Region

        Public Sub PopulateFromSelectedRow(ByRef selectedRows As Infragistics.Win.UltraWinGrid.Selected, ByVal RowIndex As Integer)
            ' populate the PriceBatchHeaderBO for the selected batch and display the details form
            _priceBatchHeaderID = CInt(selectedRows.Rows(RowIndex).Cells("PriceBatchHeaderID").Value)
            _storeNo = CInt(selectedRows.Rows(RowIndex).Cells("Store_No").Value)
            _subteamNo = CInt(selectedRows.Rows(RowIndex).Cells("SubTeam_No").Value)
            _subteamName = selectedRows.Rows(RowIndex).Cells("SubTeam_Name").Value.ToString
            _itemChgTypeID = CInt(IIf(IsDBNull(selectedRows.Rows(RowIndex).Cells("ItemChgTypeID").Value), 0, selectedRows.Rows(RowIndex).Cells("ItemChgTypeID").Value))
            _priceChgTypeID = CInt(IIf(IsDBNull(selectedRows.Rows(RowIndex).Cells("PriceChgTypeID").Value), 0, selectedRows.Rows(RowIndex).Cells("PriceChgTypeID").Value))
            _priceBatchStatusID = CInt(selectedRows.Rows(RowIndex).Cells("PriceBatchStatusID").Value)
            _batchdescription = IIf(IsDBNull(selectedRows.Rows(RowIndex).Cells("BatchDescription").Value), "", selectedRows.Rows(RowIndex).Cells("BatchDescription").Value).ToString
            _posBatchId = IIf(IsDBNull(selectedRows.Rows(RowIndex).Cells("POSBatchId").Value), "", selectedRows.Rows(RowIndex).Cells("POSBatchId").Value).ToString
            If String.Equals(selectedRows.Rows(RowIndex).Cells("Auto").Value.ToString, "*") Then
                _autoApplyFlag = True
            Else
                _autoApplyFlag = False
            End If
            If IsValidDate(selectedRows.Rows(RowIndex).Cells("ApplyDate").Value.ToString()) Then
                _autoApplyDate = CDate(selectedRows.Rows(RowIndex).Cells("ApplyDate").Value)
            End If
        End Sub

        Public Function ValidatePOSBatchId(ByVal batchId As String, ByVal storeNo As Integer) As PriceBatchHeaderStatus
            Dim status As PriceBatchHeaderStatus = PriceBatchHeaderStatus.Valid

            If Not batchId Is Nothing AndAlso Not batchId.Trim.Equals("") Then
                ' the batch id is not required, but it must be numeric
                If Not IsNumeric(batchId) Then
                    status = PriceBatchHeaderStatus.Error_POSBatchId_Numeric
                Else
                    ' if the min and max batch ids were specified for the POS writer assigned to the store,
                    ' it must be validated that the batch id is in the specified range
                    Dim batchRange() As Integer = PriceBatchHeaderDAO.GetDefaultPOSBatchIdRangeByStore(storeNo)
                    _minBatchIdAllowed = batchRange(0)
                    _maxBatchIdAllowed = batchRange(1)
                    If _minBatchIdAllowed <> -1 AndAlso CInt(batchId) < _minBatchIdAllowed Then
                        status = PriceBatchHeaderStatus.Error_POSBatchId_MinRange
                    ElseIf _maxBatchIdAllowed <> -1 AndAlso CInt(batchId) > _maxBatchIdAllowed Then
                        status = PriceBatchHeaderStatus.Error_POSBatchId_MaxRange
                    End If
                End If
            End If

            Return status
        End Function

        Property ItemChgTypeID() As Integer
            Get
                Return _itemChgTypeID
            End Get
            Set(ByVal value As Integer)
                _itemChgTypeID = value
            End Set
        End Property

        Property PriceChgTypeID() As Integer
            Get
                Return _priceChgTypeID
            End Get
            Set(ByVal value As Integer)
                _priceChgTypeID = value
            End Set
        End Property

        Property StartDate() As Date
            Get
                Return _startDate
            End Get
            Set(ByVal value As Date)
                _startDate = value
            End Set
        End Property

        Property PriceBatchDetailIDList() As String
            Get
                Return _priceBatchDetailIDList
            End Get
            Set(ByVal value As String)
                _priceBatchDetailIDList = value
            End Set
        End Property

        Property DetailIDListSeparator() As Char
            Get
                Return _detailIDListSeparator
            End Get
            Set(ByVal value As Char)
                _detailIDListSeparator = value
            End Set
        End Property

        Property PriceBatchHeaderId() As Integer
            Get
                Return _priceBatchHeaderID
            End Get
            Set(ByVal value As Integer)
                _priceBatchHeaderID = value
            End Set
        End Property

        Property PriceBatchStatusID() As Integer
            Get
                Return _priceBatchStatusID
            End Get
            Set(ByVal value As Integer)
                _priceBatchStatusID = value
            End Set
        End Property

        Property BatchDescription() As String
            Get
                Return _batchdescription
            End Get
            Set(ByVal value As String)
                _batchdescription = value
            End Set
        End Property
        Property AutoApplyFlag() As Boolean
            Get
                Return _autoApplyFlag
            End Get
            Set(ByVal value As Boolean)
                _autoApplyFlag = value
            End Set
        End Property
        Property AutoApplyDate() As Date
            Get
                Return _autoApplyDate
            End Get
            Set(ByVal value As Date)
                _autoApplyDate = value
            End Set
        End Property

        Property StoreNumber() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property

        Property SubteamNumber() As Integer
            Get
                Return _subteamNo
            End Get
            Set(ByVal value As Integer)
                _subteamNo = value
            End Set
        End Property

        Property SubteamName() As String
            Get
                Return _subteamName
            End Get
            Set(ByVal value As String)
                _subteamName = value
            End Set
        End Property

        Property POSBatchId() As String
            Get
                Return _posBatchId
            End Get
            Set(ByVal value As String)
                _posBatchId = value
            End Set
        End Property

        Property MinBatchIdAllowed() As Integer
            Get
                Return _minBatchIdAllowed
            End Get
            Set(ByVal value As Integer)
                _minBatchIdAllowed = value
            End Set
        End Property

        Property MaxBatchIdAllowed() As Integer
            Get
                Return _maxBatchIdAllowed
            End Get
            Set(ByVal value As Integer)
                _maxBatchIdAllowed = value
            End Set
        End Property

        Public Sub New()
            _priceBatchHeaderID = 0
        End Sub
    End Class

End Namespace