Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class BOAppSettings
    Public Function GetKeyValue(ByVal stKeyName As String) As String
        Dim ap As New DAOAppSettings

        Return ap.GetAppSetting(stKeyName)

    End Function
End Class
