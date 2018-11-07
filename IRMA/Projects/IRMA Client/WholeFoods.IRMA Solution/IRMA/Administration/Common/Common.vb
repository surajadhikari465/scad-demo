Module Common

    Public Sub UnbindBindingSource( _
ByVal source As BindingSource, ByVal apply As Boolean, ByVal isRoot As Boolean)
        Dim current As System.ComponentModel.IEditableObject = _
        TryCast(source.Current, System.ComponentModel.IEditableObject)
        If isRoot Then
            source.DataSource = Nothing
        End If
        If current IsNot Nothing Then
            If apply Then
                current.EndEdit()
            Else
                current.CancelEdit()
            End If
        End If
    End Sub

End Module
