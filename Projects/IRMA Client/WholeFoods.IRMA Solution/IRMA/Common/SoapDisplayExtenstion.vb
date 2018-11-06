Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Text
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

Public Class SoapDisplayExtension
    Inherits SoapExtension

    ' Variables to hold the original stream and the stream that we return
    Public originalStream As Stream
    Public internalStream As Stream

    ' Called the first time the Web service is used version called if configured with an attribute
    Public Overloads Overrides Function GetInitializer(ByVal methodInfo As LogicalMethodInfo, ByVal attribute As SoapExtensionAttribute) As Object
        ' Not used in this example, but it's declared
        ' MustOverride in the base class
        Return Nothing
    End Function
    ' Version called if configured with a config file
    Public Overloads Overrides Function GetInitializer(ByVal WebServiceType As Type) As Object
        ' Not used in this example, but it's declared 
        ' MustOverride in the base class
        Return Nothing
    End Function

    ' Called each time the Web service is used and gets passed the data from GetInitializer
    Public Overrides Sub Initialize(ByVal initializer As Object)
        ' Not used in this example, but it's declared 
        ' MustOverride in the base class
    End Sub

    ' The Chainstream method gives us a chance
    ' to grab the SOAP messages as they go by
    Public Overrides Function ChainStream(ByVal stream As Stream) As Stream
        ' Save the original stream
        originalStream = stream

        ' Create and return our own in its place
        internalStream = New MemoryStream()
        ChainStream = internalStream
    End Function

    ' The ProcessMessage method is where we do our work
    Public Overrides Sub ProcessMessage(ByVal message As SoapMessage)
        Dim myResultRegExp As System.Text.RegularExpressions.Regex = Nothing
        Dim soptions As System.Text.RegularExpressions.RegexOptions = (System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim myMatch As System.Text.RegularExpressions.Match
        Select Case message.MethodInfo.Name
            Case "saveItemData"
                myResultRegExp = New System.Text.RegularExpressions.Regex("\|CODE:(.+)\|DESC:(.+)\|ID:(.+)\|", soptions)
            Case "isCreditVendor"
                myResultRegExp = New System.Text.RegularExpressions.Regex("<return xsi:type=""xsd:string"">(.*)</return>", soptions)
        End Select



        '<return xsi:type="xsd:string">

        ' Determine the stage and take appropriate action
        Select Case message.Stage
            Case SoapMessageStage.BeforeSerialize
                ' About to prepare a SOAP Response
            Case SoapMessageStage.AfterSerialize
                ' SOAP response is all prepared
                ' Open a log file and write a status line
                'Dim fs As FileStream = _
                '  New FileStream("c:\temp\Tracker.log", _
                '  FileMode.Append, FileAccess.Write)
                'Dim sw As New StreamWriter(fs)
                'sw.WriteLine("AfterSerialize")
                'sw.Flush()
                ' Copy the passed message to the file
                'internalStream.Position = 0
                'CopyStream(internalStream, fs)
                'fs.Close()
                ' Copy the passed message to the other stream
                internalStream.Position = 0
                CopyStream(internalStream, originalStream)
                internalStream.Position = 0
            Case SoapMessageStage.BeforeDeserialize
                ' About to handle a SOAP request
                ' Copy the passed message to the other stream
                CopyStream(originalStream, internalStream)
                internalStream.Position = 0
                ' Open a log file and write a status line
                'Dim fs As FileStream = _
                'New FileStream("c:\temp\Tracker.log", _
                '  FileMode.Append, FileAccess.Write)
                'Dim sw As New StreamWriter(fs)
                'sw.WriteLine("BeforeDeserialize")
                'sw.Flush()
                ' Copy the passed message to the file
                'clone stream first

                Dim sr As MemoryStream
                sr = ObjectCopy(internalStream)
                Dim srCpy As New StreamReader(sr)

                'CopyStream(internalStream, fs)
                'fs.Close()
                'Save results to static global
                ElectronicOrderWebService.SoapResponseString = srCpy.ReadToEnd
                myMatch = myResultRegExp.Match(ElectronicOrderWebService.SoapResponseString)

                If (myMatch.Groups.Count > 0) Then
                    Select Case message.MethodInfo.Name
                        Case "isCreditVendor"
                            ElectronicOrderWebService.ReturnString = myMatch.Groups(1).Value
                        Case "saveItemData"
                            ElectronicOrderWebService.ErrorCode = myMatch.Groups(1).Value
                            ElectronicOrderWebService.ErrorDescription = myMatch.Groups(2).Value
                            ElectronicOrderWebService.DVOPurchaseOrder = myMatch.Groups(3).Value
                    End Select
                End If

                internalStream.Position = 0
            Case SoapMessageStage.AfterDeserialize
                ' SOAP request has been deserialized
        End Select
    End Sub

    ' Helper function to copy one stream to another
    Private Sub CopyStream(ByVal fromStream As Stream, ByVal toStream As Stream)
        Try
            Dim sr As New StreamReader(fromStream)
            Dim sw As New StreamWriter(toStream)
            sw.WriteLine(sr.ReadToEnd())
            sw.Flush()
        Catch ex As Exception
        End Try
    End Sub

    Function ObjectCopy(ByVal obj As Object) As Object
        'copies original object to stream then 
        'deserializes that stream and returns the output
        'to create clone (copy) of object

        Dim objMemStream As New MemoryStream(5000)
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, _
            New StreamingContext(StreamingContextStates.Clone))

        objBinaryFormatter.Serialize(objMemStream, obj)

        objMemStream.Seek(0, SeekOrigin.Begin)

        ObjectCopy = objBinaryFormatter.Deserialize(objMemStream)

        objMemStream.Close()
    End Function
End Class
