Public Class UploadMissingAttributesErrorForm

    Private _missingAttributes As New ArrayList

    Public Property MissingAttributes() As ArrayList
        Get
            Return _missingAttributes
        End Get
        Set(ByVal value As ArrayList)
            _missingAttributes = value

            If Not IsNothing(value) Then

                For Each theAttributeName As String In Me._missingAttributes
                    Me.ListBoxMissingColumns.Items.Add(theAttributeName)
                Next

                If Me._missingAttributes.Count > 0 Then
                    Me.ListBoxMissingColumns.SelectedIndex = 0
                End If
            End If
        End Set
    End Property

    Private Sub ButtonCopyToClipboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCopyToClipboard.Click

        Dim theAttributeNames As String = ""

        For Each theAttributeName As String In Me.ListBoxMissingColumns.Items
            theAttributeNames = theAttributeNames + theAttributeName + Environment.NewLine
        Next

        Clipboard.SetText(theAttributeNames)

        ' give the user come visual feedback
        Me.Enabled = False
        Thread.Sleep(500)
        Me.Enabled = True

    End Sub
End Class