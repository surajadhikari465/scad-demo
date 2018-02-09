Namespace IRMA

    <DataContract()>
    Public Class ShrinkSubType

        <DataMember()>
        Public Property ShrinkSubTypeID As Integer
        <DataMember()>
        Public Property InventoryAdjustmentCodeID As Integer
        <DataMember()>
        Public Property ShrinkType As String
        <DataMember()>
        Public Property ShrinkSubType As String
        <DataMember()>
        Public Property ReasonCode As String
        <DataMember()>
        Public Property LastUpdateUserId As Integer
        <DataMember()>
        Public Property LastUpdateDateTime As DateTime
        <DataMember()>
        Public Property Abbreviation As String
        Sub New()

        End Sub
    End Class
End Namespace