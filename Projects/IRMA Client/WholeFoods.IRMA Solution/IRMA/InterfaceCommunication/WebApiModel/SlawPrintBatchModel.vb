Imports System.Web.Script.Serialization

Namespace WholeFoods.IRMA.InterfaceCommunication.WebApiModel

    Public Class SlawPrintBatchModel
        Private _batchId As Integer
        Private _businessUnitId As Integer
        Private _batchName As String
        Private _effectiveDate As String
        Private _items As List(Of SlawPrintBatchItemModel)
        Private _batchEventType As String
        Private _application As String
        Private _batchType As String
        Private _isAdHoc As Boolean
        Private _hasPriceChange As Integer
        Private _itemCount As Integer
        Private _batchChangeType As String


        Property BatchId() As Integer
            Get
                Return _batchId
            End Get

            Set(ByVal Value As Integer)
                _batchId = Value
            End Set
        End Property
        Property BatchName() As String
            Get
                Return _batchName
            End Get

            Set(ByVal Value As String)
                _batchName = Value
            End Set
        End Property
        Property BusinessUnitId() As Integer
            Get
                Return _businessUnitId
            End Get

            Set(ByVal Value As Integer)
                _businessUnitId = Value
            End Set
        End Property

        Property BatchItems() As List(Of SlawPrintBatchItemModel)
            Get
                Return _items
            End Get

            Set(ByVal Value As List(Of SlawPrintBatchItemModel))
                _items = Value
            End Set
        End Property
        Property EffectiveDate() As String
            Get
                Return _effectiveDate
            End Get

            Set(ByVal Value As String)
                _effectiveDate = Value
            End Set
        End Property

        Property BatchEvent() As String
            Get
                Return _batchEventType
            End Get

            Set(ByVal Value As String)
                _batchEventType = Value
            End Set
        End Property
        Property BatchType() As String
            Get
                Return _batchType
            End Get

            Set(ByVal Value As String)
                _batchType = Value
            End Set
        End Property
        Property Application() As String
            Get
                Return _application
            End Get

            Set(ByVal Value As String)
                _application = Value
            End Set
        End Property


        Property IsAdHoc() As Boolean
            Get
                Return _isAdHoc
            End Get

            Set(ByVal Value As Boolean)
                _isAdHoc = Value
            End Set
        End Property

        Property HasPriceChange() As Integer
            Get
                Return _hasPriceChange
            End Get

            Set(ByVal Value As Integer)
                _hasPriceChange = Value
            End Set
        End Property

        Property ItemCount() As Integer
            Get
                Return _itemCount
            End Get

            Set(ByVal Value As Integer)
                _itemCount = Value
            End Set
        End Property


        Public Property BatchChangeType() As String
            Get
                Return _batchChangeType
            End Get
            Set(ByVal value As String)
                _batchChangeType = value
            End Set
        End Property
    End Class
End Namespace
