Imports System.IO

Module Module1





    Public storeBU As String
    Public itemsetname As String
    Public storenumber As String
    Public outputFileName As String
    Public upcCount As Integer
    Public outputFileStamp As String
    Public upcCode As String
    Public upcLength As Integer
    Public OOSAppPath As String
    Public regionID As String
    Public MaxItems As Integer
    Public WcfAddress As String
    Public WithinMax As Boolean
    Public storeName As String
    Public storeBUandName As String


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





    Sub UpdateScannerSettings()


        Try


     

            Dim scanner1 As PsionTeklogix.Barcode.Scanner = Nothing

            Dim scannerServicesDriver1 As PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver = Nothing


            If UpdateScanResult = True OrElse UpdateEnterKeys = True OrElse UpdatePrefixKeys = True OrElse UpdateUPCE = True OrElse UpdateUPCEStripTrailing = True OrElse UpdateUPCAStripTrailing = True OrElse UpdateEAN13StripTrailing = True OrElse UpdateEAN8StripTrailing = True OrElse UpdateUPCATransmitCheck = True OrElse UpdateUPCETransmitCheck = True OrElse UpdateEAN8TransmitCheck = True OrElse UpdateEAN13TransmitCheck = True OrElse UpdateGTCompliant = True Then

                Try

                    'added for Teklogix only

                    Try
                        scanner1 = New PsionTeklogix.Barcode.Scanner()
                    Catch ex As Exception

                        MessageBox.Show("Scanner instantiation failed with" & vbCr & vbLf & ex.Message, "*** ERROR ***")
                    End Try

                    Try
                        scannerServicesDriver1 = New PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver()
                    Catch ex As Exception

                        MessageBox.Show("Scanner Services Driver instantiation failed with" & vbCr & vbLf & ex.Message, "*** ERROR ***")
                    End Try


                    scanner1.Driver = scannerServicesDriver1


                    If UpdateEnterKeys = True Then
                        scannerServicesDriver1.SetProperty("Barcode\UPCA\Scs\Suffix Char", CInt(&HD))
                        scannerServicesDriver1.SetProperty("Barcode\UPCE\Scs\Suffix Char", CInt(&HD))
                        scannerServicesDriver1.SetProperty("Barcode\EAN8\Scs\Suffix Char", CInt(&HD))
                        scannerServicesDriver1.SetProperty("Barcode\EAN13\Scs\Suffix Char", CInt(&HD))
                        scannerServicesDriver1.SetProperty("Barcode\C39\Scs\Suffix Char", CInt(&HD))
                        scannerServicesDriver1.SetProperty("Barcode\C128\Scs\Suffix Char", CInt(&HD))
                    End If



                    'If UpdatePrefixKeys = True Then
                    ' Hm, not sure about this ... don't seem to need it.
                    'scannerServicesDriver1.SetProperty("Barcode\UPCA\Scs\Prefix Char", openSession.prefixKey)
                    'scannerServicesDriver1.SetProperty("Barcode\UPCE\Scs\Prefix Char", openSession.prefixKey)
                    'scannerServicesDriver1.SetProperty("Barcode\EAN8\Scs\Prefix Char", openSession.prefixKey)
                    'scannerServicesDriver1.SetProperty("Barcode\EAN13\Scs\Prefix Char", openSession.prefixKey)
                    'scannerServicesDriver1.SetProperty("Barcode\C39\Scs\Prefix Char", openSession.prefixKey)
                    'scannerServicesDriver1.SetProperty("Barcode\C128\Scs\Prefix Char", openSession.prefixKey)
                    'End If



                    If UpdateUPCE = True Then
                        scannerServicesDriver1.SetProperty("Barcode\UPCE\ICSP\Transmit as UPC-A", 0)
                    End If
                    If UpdateUPCAStripTrailing = True Then
                        scannerServicesDriver1.SetProperty("Barcode\UPCA\Scs\Strip Trailing", 1)
                    End If
                    If UpdateUPCEStripTrailing = True Then
                        scannerServicesDriver1.SetProperty("Barcode\UPCE\Scs\Strip Trailing", 1)
                    End If
                    If UpdateEAN8StripTrailing = True Then
                        scannerServicesDriver1.SetProperty("Barcode\EAN8\Scs\Strip Trailing", 1)
                    End If
                    If UpdateEAN13StripTrailing = True Then
                        scannerServicesDriver1.SetProperty("Barcode\EAN13\Scs\Strip Trailing", 1)
                    End If
                    If UpdateGTCompliant = True Then
                        scannerServicesDriver1.SetProperty("Barcode\UPC_EAN\ICSP\GTIN Compliant", 1)
                    End If
                    If UpdateUPCATransmitCheck = True Then
                        scannerServicesDriver1.SetProperty("Barcode\UPCA\ICSP\Transmit Check Digit", 1)
                    End If
                    If UpdateUPCETransmitCheck = True Then
                        scannerServicesDriver1.SetProperty("Barcode\UPCE\ICSP\Transmit Check Digit", 1)
                    End If
                    If UpdateEAN8TransmitCheck = True Then
                        scannerServicesDriver1.SetProperty("Barcode\EAN8\ICSP\Transmit Check Digit", 1)
                    End If
                    If UpdateEAN13TransmitCheck = True Then
                        scannerServicesDriver1.SetProperty("Barcode\EAN13\ICSP\Transmit Check Digit", 1)
                    End If

                    If UpdateScanResult = True Then
                        scannerServicesDriver1.SetProperty("Scs\Scan Result", 1)
                    End If

                    scannerServicesDriver1.ApplySettingChanges()

                Catch ex As Exception

                    MessageBox.Show("Unable to reset Suffix Char on scanner settings" & vbCr & vbLf & ex.Message, "")

                End Try

                scannerServicesDriver1.ApplySettingChanges()

            End If


        Catch ex As Exception

            MessageBox.Show(ex.Message)

        End Try


    End Sub






    




    'Public Function CheckSmoreConfigFile()

    '    Dim mySettingsConfigFile As String = OOSAppPath & "\MySettings.xml"


    '    If (File.Exists(mySettingsConfigFile)) Then
    '        mySettingsFileExists = True
    '    Else
    '        mySettingsFileExists = False
    '    End If


    '    Return mySettingsFileExists


    'End Function





    Public Function AppPath()

        OOSAppPath = System.IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString())

        Return OOSAppPath

    End Function




End Module
