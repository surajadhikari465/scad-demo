Imports System.Security.Principal
Imports WFM.UserAuthentication

Namespace Security

    <Serializable()> _
    Public Class IrmaPrincipal
        Inherits Csla.Security.BusinessPrincipalBase

        Private Sub New(ByVal identity As IIdentity)

            MyBase.New(identity)

        End Sub

        Public Shared Function Login(ByVal username As String, ByVal password As String, ByVal ResetRoles As Boolean, ByVal ThrowException As Boolean) As String
            Dim ReturnMsg As String = ""
            Dim identity As IrmaIdentity
            Dim principal As IrmaPrincipal


            'CSLA requires the current user object be inherited from csla.Security.BusinessPrincipalBase 
            'this will also clear the server side impersonation to avoid a "double impersonation" problem
            identity = IrmaIdentity.UnauthenticatedIdentity
            principal = New IrmaPrincipal(identity)
            Csla.ApplicationContext.User = principal
            ReturnMsg = WindowsAuthentication.ValidUser(username, password)
            If ReturnMsg.Length = 0 Then
                If identity Is IrmaIdentity.UnauthenticatedIdentity OrElse ResetRoles = True OrElse identity.Name <> username Then
                    Try
                        identity = IrmaIdentity.GetIdentity(username)
                    Catch ex As Exception
                        If ThrowException Then Throw
                        ReturnMsg = ex.Message
                    End Try
                End If
                principal = New IrmaPrincipal(identity)
                Csla.ApplicationContext.User = principal
            End If
            Return ReturnMsg

        End Function

#Region " Is In Role"
        Public Overrides Function IsInRole(ByVal role As String) As Boolean
            'me.RoleContext
            Return CType(Me.Identity, Security.IrmaIdentity).IsInRole(role)
        End Function
#End Region

    End Class

End Namespace

