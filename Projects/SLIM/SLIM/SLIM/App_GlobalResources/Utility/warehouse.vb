Imports Microsoft.VisualBasic

Public Class warehouse

    Public Shared Function FormatToUPC(ByVal upc As String) As String
        Dim warehouse As String = ""
        Dim i As Integer
        If Len(upc) > 12 Then
            warehouse = upc.Substring(0, 11)
        ElseIf Len(upc) = 12 Then
            warehouse = upc
        Else
            For i = 0 To (11 - Len(upc))
                If i = 0 Then
                    warehouse = upc & "0"
                Else
                    warehouse = "0" & warehouse
                End If
            Next
        End If
        Return warehouse
    End Function
End Class
