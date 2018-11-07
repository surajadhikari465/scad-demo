Imports System.Configuration

Public Class ConfigurationIO

    Public Shared ReadOnly Property ConfigurationDocument() As String
        Get
            Return ConfigurationManager.AppSettings("ConfigurationURI")
        End Get
    End Property

    Public Shared Function IsRegionConfigured(ByVal RegionCode As String) As Boolean

        ' load up the configuration file
        Dim _xDoc = XDocument.Load(ConfigurationDocument)

        Dim _region = From sg In _xDoc...<Region> _
                      Where sg.@id = RegionCode.ToString

        If _region.Count = 0 Then Return False Else Return True

    End Function

    Public Shared Function PluginCount(ByVal RegionCode As String) As Integer

        ' load up the configuration file
        Dim _xDoc = XDocument.Load(ConfigurationDocument)

        Dim _region = From sg In _xDoc...<Region> _
                      Where sg.@id = RegionCode

        Return _region.<Application>.Count

    End Function

    Public Shared Function GetSecurityGroupName(ByVal RegionCode As String) As String

        ' load up the configuration file
        Dim _xDoc = XDocument.Load(ConfigurationDocument)

        Dim _groupName = From sg In _xDoc...<Region> _
                         Where sg.@id = RegionCode _
                         Select sg.<SecurityGroup>.Value

        If _groupName.Count = 0 Then
            Return String.Empty
        Else
            Return _groupName.First
        End If

    End Function

    Public Shared Function GetSecurityGroupName(ByVal PluginName As String, ByVal RegionCode As String) As String

        ' load up the configuration file
        Dim _xDoc = XDocument.Load(ConfigurationDocument)

        Dim _groupName = From sg In _xDoc...<Region> _
                         Where sg.@id = RegionCode _
                         And sg.<Application>.@name = PluginName _
                         Select sg.<SecurityGroup>.Value

        If _groupName.Count = 0 Then
            Return String.Empty
        Else
            Return _groupName.First
        End If

    End Function

End Class
