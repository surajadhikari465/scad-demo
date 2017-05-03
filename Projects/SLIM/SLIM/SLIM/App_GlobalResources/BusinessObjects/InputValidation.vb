Imports Microsoft.VisualBasic
Imports SLIM.IdentifierCheck

Public Class InputValidation
    Inherits Base1
    Implements IValidate

    Public Shared Function IdentifierCheck(ByVal iid As String) As Boolean
        If IdentifierExists(iid) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function GetVendorKey(ByVal ItemRequest_ID As Integer) As String
        Dim da As New IrmaVendorTableAdapters.VendorTableAdapter
        Dim VendorKey As String = Nothing
        Try
            VendorKey = da.GetVendorKey(ItemRequest_ID)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        Return VendorKey
    End Function
End Class
