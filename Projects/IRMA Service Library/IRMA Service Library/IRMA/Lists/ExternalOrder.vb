Namespace IRMA

    <DataContract()>
    Public Class ExternalOrder       ' type so user can decode the list
        <DataMember()> _
        Public Property OrderHeader_ID As Integer
        <DataMember()> _
        Public Property Source As String
        <DataMember()> _
        Public Property CompanyName As String
    End Class

End Namespace

