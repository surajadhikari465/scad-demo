Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess

Namespace WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic

    Public Class POSBatchHeaderBO

        Private _batchID As Integer
        Private _batchDate As Date
        Private _applyDate As Date
        Private _effectiveDate As Date
        Private _batchDesc As String
        Private _posBatchId As String
        Private _storeNo As Integer
        Private _autoApply As Boolean

        Public Property BatchID() As Integer
            Get
                Return _batchID
            End Get
            Set(ByVal Value As Integer)
                _batchID = Value
            End Set
        End Property

        Public Property BatchDate() As Date
            Get
                Return _batchDate
            End Get
            Set(ByVal Value As Date)
                _batchDate = Value
            End Set
        End Property

        Public Property ApplyDate() As Date
            Get
                Return _applyDate
            End Get
            Set(ByVal Value As Date)
                _applyDate = Value
            End Set
        End Property
        Public Property EffectiveDate() As Date
            Get
                Return _effectiveDate
            End Get
            Set(ByVal Value As Date)
                _effectiveDate = Value
            End Set
        End Property

        Public Property BatchDesc() As String
            Get
                Return _batchDesc
            End Get
            Set(ByVal Value As String)
                _batchDesc = Value
            End Set
        End Property

        Public Property POSBatchId() As String
            Get
                Return _posBatchId
            End Get
            Set(ByVal Value As String)
                _posBatchId = Value
            End Set
        End Property

        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal Value As Integer)
                _storeNo = Value
            End Set
        End Property

        Public Property AutoApply() As Boolean
            Get
                Return _autoApply
            End Get
            Set(ByVal value As Boolean)
                _autoApply = value
            End Set
        End Property

        ''' <summary>
        ''' The batch header sent to the POS system may contain a batch id value.  This reads the default
        ''' value from the database for the given file writer and change type.  
        ''' </summary>
        ''' <param name="posFileWriterKey"></param>
        ''' <param name="posChangeTypeKey"></param>
        ''' <remarks></remarks>
        Public Sub PopulateDefaultPOSBatchId(ByVal posFileWriterKey As Integer, ByVal posChangeTypeKey As Integer)
            ' If the POS Batch ID has not been defined, see if there is a default value for this writer type 
            If Not _posBatchId Is Nothing AndAlso Not POSBatchId.Equals("") Then
                ' does not change
            Else
                _posBatchId = POSWriterDAO.GetDefaultPOSWriterBatchId(posFileWriterKey, posChangeTypeKey)
            End If
        End Sub

    End Class

End Namespace
