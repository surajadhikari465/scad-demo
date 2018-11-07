Public Class UpdateManifest

    Public Shared ReadOnly Property ManifestFile() As String
        Get

            If Not IO.Directory.Exists("\Temp\WFMMobile") Then

                IO.Directory.CreateDirectory("\Temp\WFMMobile")

            End If

            Return "\Temp\WFMMobile\Updates.xml"
        End Get
    End Property

    Public Shared ReadOnly Property UpdatesPath() As String
        Get

            If Not IO.Directory.Exists("\Temp\WFMMobile") Then

                IO.Directory.CreateDirectory("\Temp\WFMMobile")

            End If

            Return "\Temp\WFMMobile\Updates"
        End Get
    End Property

    Public Shared Sub Clear()

        If IO.File.Exists(manifestFile) Then

            IO.File.Delete(manifestFile)

        End If

    End Sub

    Public Shared Sub Add(ByVal UpdateXML As XElement)

        If Not IO.File.Exists(manifestFile) Then

            CreateManifest()

        End If

        Dim xDoc As XDocument = XDocument.Load(manifestFile)

        xDoc.Root.Add(UpdateXML)

        xDoc.Save(manifestFile)

    End Sub

    Private Shared Sub CreateManifest()

        Dim xDoc = <Updates></Updates>

        xDoc.Save(manifestFile)

    End Sub

End Class
