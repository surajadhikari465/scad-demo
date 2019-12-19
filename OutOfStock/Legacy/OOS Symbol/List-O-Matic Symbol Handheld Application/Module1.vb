Imports System.IO

Module Module1

    Public storeBU As String
    Public itemsetname As String
    Public storenumber As String
    Public outputFileName As String
    Public upcCount As Integer
    Public outputFileStamp As String
    Public upcUpLoadCount As Integer = 0
    Public upcCode As String
    Public upcLength As Integer
    Public OOSAppPath As String
    Public regionID As String
    Public x As String
    Public MaxItems As Integer
    Public WcfAddress As String
    Public ScanAddress As String

    Public oosUrlSandbox As String

    Public WithinMax As Boolean
    Public storeName As String
    Public storeBUandName As String

    Public iam As String

    Public UpdateUPCATransmitCheck As Boolean
    Public UpdateUPCETransmitCheck As Boolean
    Public UpdateEAN8TransmitCheck As Boolean
    Public UpdateEAN13TransmitCheck As Boolean
    Public UpdateUPCAStripTrailing As Boolean
    Public UpdateUPCEStripTrailing As Boolean
    Public UpdateEAN8StripTrailing As Boolean
    Public UpdateEAN13StripTrailing As Boolean
    Public UpdateGTCompliant As Boolean
    Public UpdateEnterKeys As Boolean
    Public UpdatePrefixKeys As Boolean
    Public UpdateUPCE As Boolean
    Public UpdateScanResult As Boolean
    Public mySettingsFileExists As Boolean

    Public AppVersion As String
    Public Environment As String


    Public Function TcpSocketTest() As Boolean
        'check to see if we have internet connectivity.
        Try
            Dim client As System.Net.Sockets.TcpClient
            client = New Net.Sockets.TcpClient("www.google.com", 80)
            client.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function AppPath()

        OOSAppPath = System.IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString())

        Return OOSAppPath

    End Function

End Module
