Imports System.Security.Principal
Imports System.Runtime.InteropServices
Public Class ImpersonationUtil


    Public Shared Function Impersonate(logon As String, password As String, domain As String) As Boolean

        Dim tempWindowsIdentity As WindowsIdentity
        Dim token As IntPtr = IntPtr.Zero
        Dim tokenDuplicate As IntPtr = IntPtr.Zero

        If (LogonUser(logon, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, token) <> 0) Then

            If (DuplicateToken(token, 2, tokenDuplicate) <> 0) Then

                tempWindowsIdentity = New WindowsIdentity(tokenDuplicate)
                impersonationContext = tempWindowsIdentity.Impersonate()
                If (Not impersonationContext Is Nothing) Then
                    Return True
                End If
            End If
        End If
        Return False

    End Function


    Public Shared Sub UnImpersonate()

        impersonationContext.Undo()
    End Sub

    <DllImport("advapi32.dll", CharSet:=CharSet.Auto)>
    Public Shared Function LogonUser( _
        lpszUserName As String, _
        lpszDomain As String, _
        lpszPassword As String, _
        dwLogonType As Integer, _
        dwLogonProvider As Integer, _
        ByRef phToken As IntPtr) As IntPtr

    End Function

    <DllImport("advapi32.dll", _
    CharSet:=System.Runtime.InteropServices.CharSet.Auto, _
    SetLastError:=True)>
    Public Shared Function DuplicateToken(
        hToken As IntPtr, _
        impersonationLevel As Integer, _
        ByRef hNewToken As IntPtr) As Integer
    End Function

    Private Const LOGON32_LOGON_INTERACTIVE As Integer = 2
    Private Const LOGON32_LOGON_NETWORK_CLEARTEXT As Integer = 4
    Private Const LOGON32_PROVIDER_DEFAULT As Integer = 0
    Private Shared impersonationContext As WindowsImpersonationContext

End Class
