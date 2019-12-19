Imports Symbol.Barcode
Imports System.Collections.Generic
Imports System.Windows.Forms


Public Class SymbolScanner

    Private scanner As Symbol.Barcode.Reader = Nothing
    Private MyReaderData As Symbol.Barcode.ReaderData = Nothing
    Private MyReadNotifyHandler As System.EventHandler = Nothing
    Private MyReadParameterList As List(Of Object) = Nothing
    Private CurrentReadParamIndex As Integer = 0
    Private myForm As Form


    Private Sub InitializeDecodeComponent()
        Me.scanner = New Reader()
        Me.MyReaderData = New ReaderData(ReaderDataTypes.Text, ReaderDataLengths.MaximumLabel)
        scanner.Actions.Enable()

        InitReadParams()

        Me.MyReadNotifyHandler = New EventHandler(AddressOf OnScanCompleteEvent)

        If scanner Is Nothing Then
            Me.StartRead()
        End If

        AddHandler myForm.Activated, AddressOf ReaderForm_Activated
        AddHandler myForm.Deactivate, AddressOf ReaderForm_Deactivate
        AddHandler myForm.Closing, AddressOf ReaderForm_Activated



    End Sub

    Private Sub InitReadParams()
        MyReadParameterList = New List(Of Object)()
        MyReadParameterList.Add(scanner.Changes.Save())

        Me.scanner.Decoders.UPCE0.Enabled = True
        Me.scanner.Decoders.UPCE0.ConvertToUPCA = True
        Me.scanner.Decoders.UPCE0.Preamble = Symbol.Barcode.UPCE0.Preambles.CountryAndSystem

        Me.scanner.Decoders.UPCE1.Enabled = True
        Me.scanner.Decoders.UPCE1.ConvertToUPCA = True
        Me.scanner.Decoders.UPCE1.Preamble = Symbol.Barcode.UPCE1.Preambles.CountryAndSystem

        MyReadParameterList.Add(scanner.Changes.Save())

        CurrentReadParamIndex = 0


    End Sub

End Class



