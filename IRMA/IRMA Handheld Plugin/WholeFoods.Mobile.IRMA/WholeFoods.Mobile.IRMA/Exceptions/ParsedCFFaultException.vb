Imports System.Xml


Public Class ParsedCFFaultException
    Inherits System.ApplicationException

    Private _faultstring As String
    ReadOnly Property FaultString()
        Get
            Return _faultstring
        End Get
    End Property

    Private _faultdetail As String
    ReadOnly Property FaultDetail()
        Get
            Return _faultdetail
        End Get
    End Property

    Private _stacktrace As String
    Overrides ReadOnly Property StackTrace() As String
        Get
            Return _stacktrace
        End Get
    End Property

    Public Sub New(ByVal faultmessage As String)

        Dim xml As XmlDocument = New XmlDocument()

        xml.LoadXml(faultmessage)
        Dim nsmgr As New XmlNamespaceManager(xml.NameTable)
        nsmgr.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/")

        _stacktrace = xml.InnerText

        _faultstring = xml.DocumentElement.GetElementsByTagName("faultstring")(0).ChildNodes(0).Value

        Dim xNodes As XmlNodeList = xml.DocumentElement.SelectNodes("//*[local-name() = 'faultstring']")
        If xNodes.Count > 0 Then
            _faultstring = xNodes(0).FirstChild.Value
        End If

        xNodes = xml.DocumentElement.SelectNodes("//*[local-name() = 'Message']")
        If xNodes.Count > 0 Then
            _faultdetail = xNodes(0).InnerText & ""
        Else
            _faultdetail = "Unable to parse the ExceptionDetail properly. The entire XML returned by the webservice can be accessed through the .FaultString property."
        End If

    End Sub

End Class