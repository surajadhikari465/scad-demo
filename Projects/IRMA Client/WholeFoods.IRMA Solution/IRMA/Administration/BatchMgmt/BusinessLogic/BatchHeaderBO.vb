Imports log4net
Imports Infragistics.Win.UltraWinGrid
Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.Administration.BatchMgmt.DataAccess
    Public Class BatchHeaderBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private _priceBatchHeaderID As Integer
        Private _batchDesc As String
        Private _storeNo As Integer
        Private _storeName As String
        Private _subTeamNo As Integer
        Private _subTeamName As String
        Private _batchStatusID As Integer
        Private _batchStatusDesc As String
        Private _startDate As String
        Private _applyDate As String
        Private _printedDate As String
        Private _sentDate As String
        Private _itemChgTypeID As Integer
        Private _itemChgTypeDesc As String
        Private _priceChgTypeID As Integer
        Private _priceChgTypeDesc As String
        Private _totalItemCount As Integer
        Private _autoApplyFlag As Boolean
        Private _posBatchID As Integer
        Private _bypassPrintShelfTags As Boolean
        Private _bypassApplyBatches As Boolean

#Region "Property Accessor Methods"
        Public Property PriceBatchHeaderID() As Integer
            Get
                Return _priceBatchHeaderID
            End Get
            Set(ByVal value As Integer)
                _priceBatchHeaderID = value
            End Set
        End Property
        Public Property BatchDesc() As String
            Get
                Return _batchDesc
            End Get
            Set(ByVal value As String)
                _batchDesc = value
            End Set
        End Property
        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property
        Public Property StoreName() As String
            Get
                Return _storeName
            End Get
            Set(ByVal value As String)
                _storeName = value
            End Set
        End Property
        Public Property SubTeamNo() As Integer
            Get
                Return _subTeamNo
            End Get
            Set(ByVal value As Integer)
                _subTeamNo = value
            End Set
        End Property
        Public Property SubTeamName() As String
            Get
                Return _subTeamName
            End Get
            Set(ByVal value As String)
                _subTeamName = value
            End Set
        End Property
        Public Property BatchStatusID() As Integer
            Get
                Return _batchStatusID
            End Get
            Set(ByVal value As Integer)
                _batchStatusID = value
            End Set
        End Property
        Public Property BatchStatusDesc() As String
            Get
                Return _batchStatusDesc
            End Get
            Set(ByVal value As String)
                _batchStatusDesc = value
            End Set
        End Property
        Public Property StartDate() As String
            Get
                Return _startDate
            End Get
            Set(ByVal value As String)
                _startDate = value
            End Set
        End Property
        Public Property ApplyDate() As String
            Get
                Return _applyDate
            End Get
            Set(ByVal value As String)
                _applyDate = value
            End Set
        End Property
        Public Property PrintedDate() As String
            Get
                Return _printedDate
            End Get
            Set(ByVal value As String)
                _printedDate = value
            End Set
        End Property
        Public Property SentDate() As String
            Get
                Return _sentDate
            End Get
            Set(ByVal value As String)
                _sentDate = value
            End Set
        End Property
        Public Property ItemChgTypeID() As Integer
            Get
                Return _itemChgTypeID
            End Get
            Set(ByVal value As Integer)
                _itemChgTypeID = value
            End Set
        End Property
        Public Property ItemChgTypeDesc() As String
            Get
                Return _itemChgTypeDesc
            End Get
            Set(ByVal value As String)
                _itemChgTypeDesc = value
            End Set
        End Property
        Public Property PriceChgTypeID() As Integer
            Get
                Return _priceChgTypeID
            End Get
            Set(ByVal value As Integer)
                _priceChgTypeID = value
            End Set
        End Property
        Public Property PriceChgTypeDesc() As String
            Get
                Return _priceChgTypeDesc
            End Get
            Set(ByVal value As String)
                _priceChgTypeDesc = value
            End Set
        End Property
        Public Property TotalItemCount() As Integer
            Get
                Return _totalItemCount
            End Get
            Set(ByVal value As Integer)
                _totalItemCount = value
            End Set
        End Property
        Public Property AutoApplyFlag() As Boolean
            Get
                Return _autoApplyFlag
            End Get
            Set(ByVal value As Boolean)
                _autoApplyFlag = value
            End Set
        End Property
        Public Property POSBatchID() As Integer
            Get
                Return _posBatchID
            End Get
            Set(ByVal value As Integer)
                _posBatchID = value
            End Set
        End Property
        Public Property BypassPrintShelfTags() As Boolean
            Get
                Return _bypassPrintShelfTags
            End Get
            Set(ByVal value As Boolean)
                _bypassPrintShelfTags = value
            End Set
        End Property
        Public Property BypassApplyBatches() As Boolean
            Get
                Return _bypassApplyBatches
            End Get
            Set(ByVal value As Boolean)
                _bypassApplyBatches = value
            End Set
        End Property

#End Region

#Region "constructors and helper methods to initialize the data"
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Create an instance of the data object, populating it with results from a SQL query.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef results As SqlDataReader)
            logger.Debug("New entry with results")

            If (Not results.IsDBNull(results.GetOrdinal("PriceBatchHeaderID"))) Then
                _priceBatchHeaderID = results.GetInt32(results.GetOrdinal("PriceBatchHeaderID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BatchDescription"))) Then
                _batchDesc = results.GetString(results.GetOrdinal("BatchDescription"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("Store_No"))) Then
                _storeNo = results.GetInt32(results.GetOrdinal("Store_No"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("Store_Name"))) Then
                _storeName = results.GetString(results.GetOrdinal("Store_Name"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SubTeam_No"))) Then
                _subTeamNo = results.GetInt32(results.GetOrdinal("SubTeam_No"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SubTeam_Name"))) Then
                _subTeamName = results.GetString(results.GetOrdinal("SubTeam_Name"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PriceBatchStatusID"))) Then
                _batchStatusID = Integer.Parse(results.GetValue(results.GetOrdinal("PriceBatchStatusID")).ToString())
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PriceBatchStatusDesc"))) Then
                _batchStatusDesc = results.GetString(results.GetOrdinal("PriceBatchStatusDesc"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("StartDate"))) Then
                _startDate = results.GetDateTime(results.GetOrdinal("StartDate")).ToString(gsUG_DateMask)
            Else
                _startDate = Nothing
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ApplyDate"))) Then
                _applyDate = results.GetDateTime(results.GetOrdinal("ApplyDate")).ToString(gsUG_DateMask)
            Else
                _applyDate = Nothing
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PrintedDate"))) Then
                _printedDate = results.GetDateTime(results.GetOrdinal("PrintedDate")).ToString(gsUG_DateMask)
            Else
                _printedDate = Nothing
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SentDate"))) Then
                _sentDate = results.GetDateTime(results.GetOrdinal("SentDate")).ToString(gsUG_DateMask)
            Else
                _sentDate = Nothing
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ItemChgTypeID"))) Then
                _itemChgTypeID = Integer.Parse(results.GetValue(results.GetOrdinal("ItemChgTypeID")).ToString())
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ItemChgTypeDesc"))) Then
                _itemChgTypeDesc = results.GetString(results.GetOrdinal("ItemChgTypeDesc"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PriceChgTypeID"))) Then
                _priceChgTypeID = Integer.Parse(results.GetValue(results.GetOrdinal("PriceChgTypeID")).ToString())
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PriceChgTypeDesc"))) Then
                _priceChgTypeDesc = results.GetString(results.GetOrdinal("PriceChgTypeDesc"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("TotalItems"))) Then
                _totalItemCount = results.GetInt32(results.GetOrdinal("TotalItems"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("AutoApplyFlag"))) Then
                _autoApplyFlag = results.GetBoolean(results.GetOrdinal("AutoApplyFlag"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("POSBatchID"))) Then
                _posBatchID = results.GetInt32(results.GetOrdinal("POSBatchID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BypassPrintShelfTags"))) Then
                _bypassPrintShelfTags = results.GetBoolean(results.GetOrdinal("BypassPrintShelfTags"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BypassApplyBatches"))) Then
                _bypassApplyBatches = results.GetBoolean(results.GetOrdinal("BypassApplyBatches"))
            End If

            logger.Debug("New exit")
        End Sub

        ''' <summary>
        ''' Create an instance of the data object, populating it with data from a selected row in a grid.
        ''' </summary>
        ''' <param name="currentRow"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal currentRow As UltraGridRow)
            logger.Debug("New entry with UltraGridRow")

            _priceBatchHeaderID = CInt(currentRow.Cells("PriceBatchHeaderID").Value)
            If currentRow.Cells("BatchDesc").Value IsNot Nothing Then
                _batchDesc = currentRow.Cells("BatchDesc").Value.ToString
            End If
            _storeNo = CInt(currentRow.Cells("StoreNo").Value)
            If currentRow.Cells("StoreName").Value IsNot Nothing Then
                _storeName = currentRow.Cells("StoreName").Value.ToString
            End If
            _subTeamNo = CInt(currentRow.Cells("SubTeamNo").Value)
            If currentRow.Cells("SubTeamName").Value IsNot Nothing Then
                _subTeamName = currentRow.Cells("SubTeamName").Value.ToString
            End If
            _batchStatusID = CInt(currentRow.Cells("BatchStatusID").Value)
            If currentRow.Cells("BatchStatusDesc").Value IsNot Nothing Then
                _batchStatusDesc = currentRow.Cells("BatchStatusDesc").Value.ToString
            End If
            _startDate = currentRow.Cells("StartDate").Value.ToString
            If currentRow.Cells("ApplyDate").Value IsNot Nothing Then
                _applyDate = currentRow.Cells("ApplyDate").Value.ToString
            End If
            If currentRow.Cells("PrintedDate").Value IsNot Nothing Then
                _printedDate = currentRow.Cells("PrintedDate").Value.ToString
            End If
            If currentRow.Cells("SentDate").Value IsNot Nothing Then
                _sentDate = currentRow.Cells("SentDate").Value.ToString
            End If
            _itemChgTypeID = CInt(currentRow.Cells("ItemChgTypeID").Value)
            If currentRow.Cells("ItemChgTypeDesc").Value IsNot Nothing Then
                _itemChgTypeDesc = currentRow.Cells("ItemChgTypeDesc").Value.ToString
            End If
            _priceChgTypeID = CInt(currentRow.Cells("PriceChgTypeID").Value)
            If currentRow.Cells("PriceChgTypeDesc").Value IsNot Nothing Then
                _priceChgTypeDesc = currentRow.Cells("PriceChgTypeDesc").Value.ToString
            End If
            _totalItemCount = CInt(currentRow.Cells("TotalItemCount").Value)
            _autoApplyFlag = CBool(currentRow.Cells("AutoApplyFlag").Value)
            _posBatchID = CInt(currentRow.Cells("POSBatchID").Value)
            _bypassPrintShelfTags = CBool(currentRow.Cells("BypassPrintShelfTags").Value)
            _bypassApplyBatches = CBool(currentRow.Cells("BypassApplyBatches").Value)

            logger.Debug("New exit")
        End Sub
#End Region

    End Class
End Namespace
