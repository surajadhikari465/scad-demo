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
    Public SmoreAppPath As String
    Public regionID As String
    Public MaxItems As Integer
    Public WcfAddress As String
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





    






    'Public Function CheckSmoreConfigFile()

    '    Dim mySettingsConfigFile As String = SmoreAppPath & "\MySettings.xml"


    '    If (File.Exists(mySettingsConfigFile)) Then
    '        mySettingsFileExists = True
    '    Else
    '        mySettingsFileExists = False
    '    End If


    '    Return mySettingsFileExists


    'End Function




    Public Function AppPath()

        SmoreAppPath = System.IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString())

        Return SmoreAppPath

    End Function




End Module
