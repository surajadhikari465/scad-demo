Imports Microsoft.Build.Utilities
Imports System.IO
Imports System.Xml
Imports WholeFoods.Utility.Encryption

Namespace WholeFoods.Build.BuildTask
    ''' <summary>
    ''' The RemoveApplicationConfigProperty defines a custom build task removes the connection string value
    ''' from the app.config file.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RemoveConnectionStringProperty
        Inherits Microsoft.Build.Utilities.Task

#Region "Property definitons"
        Private _configFile As String
        Private _node As String

#End Region

#Region "Task definitions"
        ''' <summary>
        ''' The Execute method opens the application configuration file and sets the
        ''' value for Key equal to Value.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Execute() As Boolean
            Log().LogMessage("Entering RemoveConnectionStringProperty.Execute method: " & _configFile)
            Dim returnVal As Boolean = True
            Dim xmlDoc As New XmlDocument()
            Dim xmlNode As XmlNode
            Dim xmlElement As XmlElement

            Try
                ' Open the app.config XML document for reading and initialize the XmlDocument with the contents
                xmlDoc.Load(_configFile)

                ' Get the node being updated
                Log().LogMessage("Node is " & _node)
                xmlNode = xmlDoc.DocumentElement.SelectSingleNode(_node)


                If xmlNode IsNot Nothing Then
                    Log().LogMessage("Remove name " & _node)
                    xmlnode.ParentNode.RemoveChild(xmlnode)
                    xmlDoc.Save(_configFile)
                Else
                    Log().LogMessage("xmlNode was Nothing" & _node)
                End If

            Catch ex As Exception
                Log().LogErrorFromException(ex, True)
                returnVal = False
            End Try
            Log().LogMessage("Exiting SetConnectionStringProperty.Execute method: " + returnVal.ToString())
            Return returnVal
        End Function

        ''' <summary>
        ''' Returns the value to set in the config file for the key.
        ''' If the _encrypted flag is true, this will be the encrytped value.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        'Private Function GetValue(ByVal inValue As String) As String
        'Dim returnValue As String = inValue
        'If _encrypted Then
        ' Encrypt the value before returning it
        ' Dim encryptor As New Encryptor()
        'eturnValue = encryptor.Encrypt(inValue)
        ' End If
        'Return returnValue
        'End Function
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

#End Region

    End Class
End Namespace

