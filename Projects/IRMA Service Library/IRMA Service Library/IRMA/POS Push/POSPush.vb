Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common
Imports System.Security

Namespace IRMA

    <DataContract()>
    Public Class POSPush

        Public Function RunPOSPush(ByVal sAppPath As String, ByVal sRegion As String, ByVal sConnectionString As String, ByVal sEmailAddress As String) As Boolean

            Dim proc As New ProcessStartInfo

            Try
                proc.FileName = sAppPath
                proc.Arguments = """" & sRegion & """" & " " & """" & sConnectionString & """" & " " & """" & sEmailAddress & """"
                Process.Start(proc)
            Catch ex As Exception
                Throw ex
            End Try

            Return True
        End Function

        Public Function GetSecureString(ByVal sPassword As String) As SecureString
            Dim secPassword As New SecureString

            secPassword.Clear()

            For Each c As Char In sPassword
                secPassword.AppendChar(c)
            Next

            Return secPassword
        End Function
    End Class
End Namespace