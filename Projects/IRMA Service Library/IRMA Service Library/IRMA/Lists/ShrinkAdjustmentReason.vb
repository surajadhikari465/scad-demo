Namespace IRMA

    <DataContract()> _
    Public Class ShrinkAdjustmentReason

        Private _inventoryAdjustmentCodeID As Integer
        Private _abbreviation As String
        Private _adjustmentDescription As String
        Private _adjustmentID As Integer

        <DataMember()> _
        Public Property InventoryAdjustmentCodeID() As Integer
            Get
                Return _inventoryAdjustmentCodeID
            End Get
            Set(ByVal value As Integer)
                _inventoryAdjustmentCodeID = value
            End Set
        End Property

        <DataMember()> _
        Public Property Abbreviation() As String
            Get
                Return _abbreviation
            End Get
            Set(ByVal value As String)
                _abbreviation = value
            End Set
        End Property

        <DataMember()> _
        Public Property AdjustmentDescription() As String
            Get
                Return _adjustmentDescription
            End Get
            Set(ByVal value As String)
                _adjustmentDescription = value
            End Set
        End Property

        <DataMember()> _
        Public Property AdjustmentID() As Integer
            Get
                Return _adjustmentID
            End Get
            Set(ByVal value As Integer)
                _adjustmentID = value
            End Set
        End Property


    End Class

End Namespace
