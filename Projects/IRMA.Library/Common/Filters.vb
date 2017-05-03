Namespace Common
    Public Module Filters
        Public Function CaseInsensitveFilter(ByVal item As Object, ByVal FilterValue As Object) As Boolean
            Dim result As Boolean = False
            If Not item Is Nothing AndAlso Not FilterValue Is Nothing Then
                result = LCase(CStr(item)).Contains(LCase(CStr(FilterValue)))
            End If
            Return result
        End Function
    End Module
End Namespace
