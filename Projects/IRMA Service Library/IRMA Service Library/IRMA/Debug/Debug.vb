Namespace IRMA.Debug

    <DataContract()> _
    Public Class Debug
        Private _sConnectionString As String = String.Empty

        <DataMember()> _
        Public Property ConnectionString() As Integer
            Get
                Return _sConnectionString
            End Get
            Set(ByVal value As Integer)
                _sConnectionString = value
            End Set
        End Property
    End Class
End Namespace