Namespace Common
    Public Module Functions
        Public Function StripLeadingZeros(ByVal value As String) As String
            Try
                If value.Length <> 0 Then
                    Do Until value.StartsWith("0") = False
                        value = value.Substring(2, value.Length - 2)
                    Loop
                End If
            Catch ex As Exception
                'I'll be damnded if this ever happens....
                MsgBox("Error Striping Leading Zeros?!?!")
            End Try

            Return value
        End Function

    End Module
End Namespace
