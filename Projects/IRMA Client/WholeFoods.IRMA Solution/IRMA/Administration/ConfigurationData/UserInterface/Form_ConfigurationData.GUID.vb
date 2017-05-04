Public Class Form_ConfigurationData_GUID

    Public WriteOnly Property IdentificationLabel() As String
        Set(ByVal value As String)
            Me._labelGUID.Text = String.Format(Me._labelGUID.Text, value)
        End Set
    End Property

    Public WriteOnly Property IdentificationGUID() As Guid
        Set(ByVal value As Guid)
            Me._textGUID.Text = value.ToString("D").ToUpper ' format as 32 digits separated by hyphens
        End Set
    End Property

End Class