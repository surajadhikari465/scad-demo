Imports System.Xml
Imports System.Text
Imports WholeFoods.Utility.ResourceServices

Public Class Form_ConfigurationData_View

    Private _xmlDocument As XmlDocument

    Public WriteOnly Property DisplayDocument() As XmlDocument
        Set(ByVal value As XmlDocument)
            Me._xmlDocument = value
            Dim d As New IO.StringReader(My.Resources.DisplayXML)
            Dim xr As XmlReader = XmlReader.Create(d)
            Dim xct As Xsl.XslCompiledTransform = New Xsl.XslCompiledTransform
            xct.Load(xr)
            Dim sb As StringBuilder = New StringBuilder
            Dim xw As XmlWriter = XmlWriter.Create(sb)
            xct.Transform(value, xw)
            Me._formWebBrowser.DocumentText = sb.ToString
        End Set
    End Property

    Public WriteOnly Property Title() As String
        Set(ByVal value As String)
            Me.Text = String.Format(Me.Text, value)
        End Set
    End Property

    Private Sub _buttonExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonExport.Click

        Me._dialogSaveFile.OverwritePrompt = True
        Me._dialogSaveFile.FileName = "appSettings"
        Me._dialogSaveFile.DefaultExt = "config"
        Me._dialogSaveFile.Filter = "Configuration files (*.config)|*.config"
        Me._dialogSaveFile.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.Desktop

        Dim result As DialogResult = Me._dialogSaveFile.ShowDialog()
        Dim _path As String = Me._dialogSaveFile.FileName

        If (result = DialogResult.OK) Then

            Me._dialogSaveFile.Dispose()
            Me._xmlDocument.Save(_path)
            MessageBox.Show("Configuration file exported.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If

    End Sub

    Private Sub _buttonCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _buttonCopy.Click

        Clipboard.SetText(Me._xmlDocument.InnerXml, TextDataFormat.Text)

    End Sub

End Class