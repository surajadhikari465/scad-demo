Imports System.Text
Public Class EInvoicing_CurrentInvoice

    Public Shared Filename As String
    Public Shared Regions As String
    Public Shared InvoiceNumber As String
    Public Shared StoreNumber As String
    Public Shared PONumber As String
    Public Shared VendorId As String
    Public Shared DuplicateInvoice As String
    Public Shared EInvoicingId As String
    Public Shared Validated As Boolean
    Public Shared LineItemColumns As String
    Public Shared HeaderItemColumns As String
    Public Shared ErrorCode As String
    Public Shared ErrorMessage As String
    Public Shared UnknownElements As List(Of String) = New List(Of String)
    Public Shared MissingStoreSubteamReferences As List(Of String) = New List(Of String)
    Public Shared ExceptionMessage As String
    Public Shared InnerExceptionMessage As String
    Public Shared StackTrace As String
    Private Shared GeneratedEmail As String = String.Empty




    Public Shared Sub Clear()
        'Filename = String.Empty -- file name should not be cleared because it needs to be kept until the next file is processed.
        'Regions = String.Empty
        InvoiceNumber = String.Empty
        StoreNumber = String.Empty
        PONumber = String.Empty
        VendorId = String.Empty
        DuplicateInvoice = String.Empty
        EInvoicingId = String.Empty
        Validated = False
        LineItemColumns = String.Empty
        HeaderItemColumns = String.Empty
        ErrorCode = String.Empty
        ErrorMessage = String.Empty
        ExceptionMessage = String.Empty
        InnerExceptionMessage = String.Empty
        StackTrace = String.Empty
        UnknownElements.Clear()
        MissingStoreSubteamReferences.Clear()

    End Sub

    Public Shared Function GenerateEmail() As String
        GeneratedEmail = My.Resources.EInvoicingEmailTemplate

        GeneratedEmail = UpdateTemplate(GeneratedEmail, "Filename", Filename)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "Regions", Regions)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "InvoiceNumber", InvoiceNumber)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "StoreNumber", StoreNumber)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "PONumber", PONumber)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "VendorId", VendorId)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "DuplicateInvoice", DuplicateInvoice)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "EInvoicingId", EInvoicingId)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "Validated", IIf(Validated, "True", "False").ToString())
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "LineItemColumns", LineItemColumns)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "HeaderItemColumns", HeaderItemColumns)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "ErrorCode", ErrorCode)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "ErrorMessage", ErrorMessage)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "ExceptionMessage", ExceptionMessage)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "InnerExceptionMessage", InnerExceptionMessage)
        GeneratedEmail = UpdateTemplate(GeneratedEmail, "StackTrace", StackTrace)

        If UnknownElements.Count > 0 Then
            Dim temp As StringBuilder = New StringBuilder()
            For Each item As String In UnknownElements
                temp.AppendFormat("{0},", item)
            Next
            temp.Remove(temp.Length - 1, 1)
            GeneratedEmail = UpdateTemplate(GeneratedEmail, "UnknownElements", temp.ToString)
        Else
            GeneratedEmail = UpdateTemplate(GeneratedEmail, "UnknownElements", "")
        End If

        If MissingStoreSubteamReferences.Count > 0 Then
            Dim temp As StringBuilder = New StringBuilder()
            For Each item As String In MissingStoreSubteamReferences
                temp.AppendFormat("{0},", item)
            Next
            temp.Remove(temp.Length - 1, 1)
            GeneratedEmail = UpdateTemplate(GeneratedEmail, "UnknownElements", temp.ToString)
        Else
            GeneratedEmail = UpdateTemplate(GeneratedEmail, "MissingStoreSubteamReferences", "")
        End If


        Return GeneratedEmail
    End Function

    Private Shared Function UpdateTemplate(ByVal template As String, ByVal key As String, ByVal value As String) As String
        key = String.Format("[# {0} #]", key)
        template = template.Replace(key, value)
        Return template
    End Function




End Class
