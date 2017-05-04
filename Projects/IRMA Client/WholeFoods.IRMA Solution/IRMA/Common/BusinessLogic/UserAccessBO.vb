Namespace WholeFoods.IRMA.Common.BusinessLogic
    Public Class UserAccessBO
        Private _UserID As Integer
        Private _UserName As String
        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal value As Integer)
                _UserID = value
            End Set
        End Property
        Public Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal value As String)
                _UserName = value
            End Set
        End Property

        Public Function IsInGroup(ByVal GroupName As String)
            Dim MyIdentity As System.Security.Principal.WindowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent()
            Dim MyPrincipal As System.Security.Principal.WindowsPrincipal = New System.Security.Principal.WindowsPrincipal(MyIdentity)
            If MyPrincipal.IsInRole(GroupName) Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class
End Namespace
