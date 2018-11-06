Imports Microsoft.Build.Utilities
Imports System.IO
Imports System.Xml
Imports WholeFoods.Utility.Encryption

Namespace WholeFoods.Build.BuildTask
    ''' <summary>
    ''' The SetApplicationConfigProperty defines a custom build task to set the value
    ''' for a key in the app.config file.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SetApplicationConfigProperty
        Inherits Microsoft.Build.Utilities.Task

#Region "Property definitons"
        Private _configFile As String
        Private _node As String
        Private _key As String
        Private _value As String
        Private _encrypted As Boolean = False
        Private _comment As String = Nothing
#End Region

#Region "Task definitions"
        ''' <summary>
        ''' The Execute method opens the application configuration file and sets the
        ''' value for Key equal to Value.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Execute() As Boolean
            Log().LogMessage("Entering SetApplicationConfigProperty.Execute method: " & _configFile)
            Dim returnVal As Boolean = True
            Dim xmlDoc As New XmlDocument()
            Dim xmlNode As XmlNode
            Dim xmlElement As XmlElement
            Dim xmlComment As XmlComment

            Try
                ' Open the app.config XML document for reading and initialize the XmlDocument with the contents
                xmlDoc.Load(_configFile)

                ' Get the node being updated
                Log().LogMessage("Node is " & _node)
                xmlNode = xmlDoc.DocumentElement.SelectSingleNode(_node)

                If xmlNode IsNot Nothing Then
                    ' Get the add element that contains the key
                    Log().LogMessage("Key is " & _key)
                    xmlElement = CType(xmlNode.SelectSingleNode(String.Format("//add[@key='{0}']", _key)), XmlElement)

                    If xmlElement IsNot Nothing Then
                        ' Update the value for the key
                        Log().LogMessage("Updating value " & _value)
                        xmlElement.SetAttribute("value", GetValue())
                    Else
                        ' Key was not found so create the add element
                        Log().LogMessage("Creating value " & _value)
                        xmlElement = xmlDoc.CreateElement("add")
                        xmlElement.SetAttribute("key", _key)
                        xmlElement.SetAttribute("value", GetValue())
                        xmlNode.AppendChild(xmlElement)
                    End If

                    ' Add a comment above the node, if one is specified
                    If (_comment IsNot Nothing) AndAlso (_comment <> "") Then
                        xmlComment = xmlDoc.CreateComment(_comment)
                        xmlNode.InsertBefore(xmlComment, xmlElement)
                    End If

                    ' Save the changes to the file
                    xmlDoc.Save(_configFile)
                Else
                    Log().LogError("xmlNode was Nothing")
                End If
            Catch ex As Exception
                Log().LogErrorFromException(ex, True)
                returnVal = False
            End Try
            Log().LogMessage("Exiting SetApplicationConfigProperty.Execute method: " + returnVal.ToString())
            Return returnVal
        End Function

        ''' <summary>
        ''' Returns the value to set in the config file for the key.
        ''' If the _encrypted flag is true, this will be the encrytped value.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetValue() As String
            Dim returnValue As String = _value
            If _encrypted Then
                ' Encrypt the value before returning it
                Dim encryptor As New Encryptor()
                returnValue = encryptor.Encrypt(_value)
            End If
            Return returnValue
        End Function
#End Region

#Region "Property access methods"
        ''' <summary>
        ''' Node that contains the property being updated.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Node() As String
            Get
                Return _node
            End Get
            Set(ByVal value As String)
                _node = value
            End Set
        End Property

        ''' <summary>
        ''' Key for the property.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Key() As String
            Get
                Return _key
            End Get
            Set(ByVal value As String)
                _key = value
            End Set
        End Property

        ''' <summary>
        ''' Value for the property.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Value() As String
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
            End Set
        End Property

        ''' <summary>
        ''' Path to the configuration file being updated.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConfigFile() As String
            Get
                Return _configFile
            End Get
            Set(ByVal value As String)
                _configFile = value
            End Set
        End Property

        ''' <summary>
        ''' Flag set to True if the Value should be encrypted.  Defaults to False. (optional)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Encrypted() As Boolean
            Get
                Return _encrypted
            End Get
            Set(ByVal value As Boolean)
                _encrypted = value
            End Set
        End Property

        ''' <summary>
        ''' Comment to appear above the property. (optional)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Comment() As String
            Get
                Return _comment
            End Get
            Set(ByVal value As String)
                _comment = value
            End Set
        End Property
#End Region

    End Class
End Namespace
