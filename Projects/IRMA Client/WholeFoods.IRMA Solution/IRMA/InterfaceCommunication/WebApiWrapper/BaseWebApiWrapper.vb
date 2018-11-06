Imports System.Linq
Imports System.Net
Imports System.Web.Script.Serialization
Imports log4net

Namespace WholeFoods.IRMA.InterfaceCommunication.WebApiWrapper
    Public MustInherit Class BaseWebApiWarapper
        Implements IDisposable

        Protected _disposed As Boolean = False
        Private ReadOnly _baseUrl As String
        Private _webClient As ExtendedWebClient
        Protected Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Sub New(baseUrl As String)
            _baseUrl = baseUrl.Trim("/")

            _webClient = New ExtendedWebClient
        End Sub

        Public Class ExtendedWebClient
            Inherits WebClient
            Dim _timeout As Int32

            'Set a default timeout value
            Public Sub New()
                Me._timeout = 60000
            End Sub

            Public Sub New(timeout As Integer)
                Me._timeout = timeout
            End Sub

            Public Property Timeout() As Int32
                Get
                    Return _timeout
                End Get
                Set(value As Int32)
                    _timeout = value
                End Set
            End Property

            Protected Overrides Function GetWebRequest(ByVal address As System.Uri) As System.Net.WebRequest
                Dim w As WebRequest = MyBase.GetWebRequest(address)
                w.Timeout = _timeout
                Return w
            End Function
        End Class

        Protected Function Client() As ExtendedWebClient
            If (_webClient Is Nothing) Then
                Throw New ObjectDisposedException("WebClient has been disposed")
            End If

            Return _webClient
        End Function

        Protected Sub PostAsJson(Of T)(apiUrl As String, ByVal timeout As Int32, ByVal objectToPost As T, apiVerion As String)
            If String.IsNullOrEmpty(apiUrl) Then
                logger.Error("BaseWebApiWrapper is called but apiurl is null: check to make sure api url is set in the config")
                Exit Sub
            End If

            If objectToPost Is Nothing Then
                logger.Error("BaseWebApiWrapper is called with empty data: API call will not be made to " + apiUrl)
                Exit Sub
            End If

            Dim serializer As JavaScriptSerializer = New JavaScriptSerializer()
            Dim jsonString = serializer.Serialize(objectToPost)
            Dim client As ExtendedWebClient = Me.Client()

            client.UseDefaultCredentials = True
            client.Headers(HttpRequestHeader.ContentType) = "application/json"
            client.Headers.Add("api-version", apiVerion)
            client.Timeout = timeout

            Try
                logger.Info("Posting message to " + apiUrl + "| Version Header: api-version: " + client.Headers.GetValues("api-version").FirstOrDefault() + ": Message: " + jsonString)
                Dim result As String = client.UploadString(New Uri(apiUrl), "POST", jsonString)
            Catch exception As Exception
                logger.Error("API POST to " + apiUrl + "failed with exception: " + exception.Message)
                Throw
            End Try
        End Sub

        Protected Sub PostAsJson(Of T)(apiUrl As String, ByVal timeout As Int32, ByVal objectToPost As T, userId As String, password As String, apiVersion As String)
            If String.IsNullOrEmpty(apiUrl) Or String.IsNullOrEmpty(userId) Or String.IsNullOrEmpty(password) Then
                logger.Error("BaseWebApiWrapper is called with integrated security, but configurations are not set right: check to make sure api url, user ID and password are set")
                Exit Sub
            End If

            If objectToPost Is Nothing Then
                logger.Error("BaseWebApiWrapper is called with empty data: API call will not be made to " + apiUrl)
                Exit Sub
            End If

            Dim serializer As JavaScriptSerializer = New JavaScriptSerializer()
            Dim jsonString = serializer.Serialize(objectToPost)
            Dim client As ExtendedWebClient = Me.Client()
            Dim userCredentials = userId & ":" & password

            client.Credentials = New NetworkCredential(userId, password)
            client.Headers(HttpRequestHeader.ContentType) = "application/json"
            client.Headers.Add("api-version", apiVersion)
            client.Headers(HttpRequestHeader.Authorization) = "NTLM " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(userCredentials))
            client.Timeout = timeout

            Dim result As String
            Try
                logger.Info("Posting message to " + apiUrl + "| Version Header: api-version: " + client.Headers.GetValues("api-version").FirstOrDefault() + "| Message: " + jsonString)
                result = client.UploadString(New Uri(apiUrl), "POST", jsonString)
            Catch exception As Exception
                logger.Error("API POST to " + apiUrl + "failed with exception: " + exception.ToString())
                Throw
            End Try
        End Sub

        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not Me._disposed Then
                If disposing Then
                    If (Not (_webClient Is Nothing)) Then
                        _webClient.Dispose()
                        _webClient = Nothing
                    End If
                End If
            End If

            Me._disposed = True
        End Sub

        Public Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
    End Class
End Namespace