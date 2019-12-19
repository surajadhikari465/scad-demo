Public Class SetScannerSettingService
    Private Shared _service As SetScannerSettingService

    Private _
        scanner As PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver = _
            New PsionTeklogix.Barcode.ScannerServices.ScannerServicesDriver()

    Private _undo As New Dictionary(Of String, Integer)

    Private Const UPCA_SUFFIX_KEY As String = "Barcode\UPCA\Scs\Suffix Char"
    Private Const UPCE_SUFFIX_KEY As String = "Barcode\UPCE\Scs\Suffix Char"
    Private Const EAN8_SUFFIX_KEY As String = "Barcode\EAN8\Scs\Suffix Char"
    Private Const EAN13_SUFFIX_KEY As String = "Barcode\EAN13\Scs\Suffix Char"

    Private Const UPCA_TRANSMIT_CHECK_DIGIT_KEY = "Barcode\UPCA\ICSP\Transmit Check Digit"
    Private Const UPCE_TRANSMIT_CHECK_DIGIT_KEY = "Barcode\UPCE\ICSP\Transmit Check Digit"
    Private Const EAN8_TRANSMIT_CHECK_DIGIT_KEY = "Barcode\EAN8\ICSP\Transmit Check Digit"
    Private Const EAN13_TRANSMIT_CHECK_DIGIT_KEY = "Barcode\EAN13\ICSP\Transmit Check Digit"


    Private Sub New()
    End Sub

    Public Shared Function Singleton() As SetScannerSettingService
        If (_service Is Nothing) Then
            _service = New SetScannerSettingService()
        End If
        Return _service
    End Function


    Public Sub SetScannerSettings()
        SuffixEnterKeys()
        TurnOffCheckDigits()
        scanner.ApplySettingChanges()
    End Sub


    Private Sub SuffixEnterKeys()
        AddMomento(UPCA_SUFFIX_KEY, scanner.GetProperty(UPCA_SUFFIX_KEY))
        scanner.SetProperty(UPCA_SUFFIX_KEY, CInt(&HD))

        AddMomento(UPCE_SUFFIX_KEY, scanner.GetProperty(UPCE_SUFFIX_KEY))
        scanner.SetProperty(UPCE_SUFFIX_KEY, CInt(&HD))

        AddMomento(EAN8_SUFFIX_KEY, scanner.GetProperty(EAN8_SUFFIX_KEY))
        scanner.SetProperty(EAN8_SUFFIX_KEY, CInt(&HD))

        AddMomento(EAN13_SUFFIX_KEY, scanner.GetProperty(EAN13_SUFFIX_KEY))
        scanner.SetProperty(EAN13_SUFFIX_KEY, CInt(&HD))
    End Sub

    Private Sub AddMomento(ByVal key As String, ByVal value As Integer)
        _undo.Add(key, value)
    End Sub


    Private Sub TurnOffCheckDigits()
        AddMomento(UPCA_TRANSMIT_CHECK_DIGIT_KEY, scanner.GetProperty(UPCA_TRANSMIT_CHECK_DIGIT_KEY))
        scanner.SetProperty(UPCA_TRANSMIT_CHECK_DIGIT_KEY, 0)

        AddMomento(UPCE_TRANSMIT_CHECK_DIGIT_KEY, scanner.GetProperty(UPCE_TRANSMIT_CHECK_DIGIT_KEY))
        scanner.SetProperty(UPCE_TRANSMIT_CHECK_DIGIT_KEY, 0)

        AddMomento(EAN8_TRANSMIT_CHECK_DIGIT_KEY, scanner.GetProperty(EAN8_TRANSMIT_CHECK_DIGIT_KEY))
        scanner.SetProperty(EAN8_TRANSMIT_CHECK_DIGIT_KEY, 0)

        AddMomento(EAN13_TRANSMIT_CHECK_DIGIT_KEY, scanner.GetProperty(EAN13_TRANSMIT_CHECK_DIGIT_KEY))
        scanner.SetProperty(EAN13_TRANSMIT_CHECK_DIGIT_KEY, 0)
    End Sub


    Public Sub Undo()
        Dim pair As KeyValuePair(Of String, Integer)
        For Each pair In _undo
            scanner.SetProperty(pair.Key, pair.Value)
        Next
        scanner.ApplySettingChanges()
    End Sub
End Class
