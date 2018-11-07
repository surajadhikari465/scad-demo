Public Class WindowsAuthentication
    Private Declare Auto Function LogonUser Lib "advapi32.dll" (ByVal lpszUsername As [String], ByVal lpszDomain As [String], ByVal lpszPassword As [String], ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, ByRef phToken As IntPtr) As Boolean
    Private Declare Ansi Function FormatMessage Lib "kernel32" Alias "FormatMessageA" ( _
        ByVal dwFlags As Int32, _
        ByVal lpSource As Int32, _
        ByVal dwMessageId As Int32, _
        ByVal dwLanguageId As Int32, _
        ByVal lpBuffer As System.Text.StringBuilder, _
        ByVal nSize As Int32, _
        ByVal Arguments As Int32) As Int32
    Private Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Boolean
    Public Shared Function GetErrorMessage(ByVal errorCode As Integer) As String
        Dim sBuffer As New System.Text.StringBuilder(512)
        Dim iReturn As Int32
        iReturn = FormatMessage(&H1000, 0, errorCode, &H0, sBuffer, sBuffer.Capacity, 0)
        Return sBuffer.ToString
    End Function
    Public Shared Function ValidUser(ByVal UserName As String, ByVal Password As String) As String
        Try
            Const LOGON32_PROVIDER_DEFAULT As Integer = 0
            'Const LOGON32_PROVIDER_WINNT40 As Integer = 2
            Const LOGON32_LOGON_NETWORK As Integer = 3
            Dim tokenHandle As New IntPtr(0)
            Dim errmsg As String = String.Empty
            tokenHandle = IntPtr.Zero
            'Call the LogonUser function to obtain a handle to an access token.
            Dim returnValue As Boolean = LogonUser(UserName, "WFM", Password, LOGON32_LOGON_NETWORK, LOGON32_PROVIDER_DEFAULT, tokenHandle)

            If returnValue = False Then
                'This function returns the error code that the last unmanaged function returned.
                Dim ret As Integer = System.Runtime.InteropServices.Marshal.GetLastWin32Error()
                errmsg = GetErrorMessage(ret)
            End If

            'Free the access token.
            If Not System.IntPtr.op_Equality(tokenHandle, IntPtr.Zero) Then
                CloseHandle(tokenHandle)
            End If

            Return errmsg
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
