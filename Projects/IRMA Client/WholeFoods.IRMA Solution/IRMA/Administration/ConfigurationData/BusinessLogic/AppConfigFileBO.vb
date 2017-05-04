Imports System.XML.Serialization
Imports System.Xml
Imports System.IO
Imports WholeFoods.IRMA.Administration.ConfigurationData.DataAccess

Public Class AppConfigFileBO

    Public Shared Sub Write(ByVal AppID As Guid, ByVal EnvID As Guid)

        ' Instantiate the data access class
        Dim daoConfig As ConfigurationDataDAO = New ConfigurationDataDAO

        Dim _doc As New Xml.XmlDocument

        _doc.LoadXml(daoConfig.GetConfigDocument(AppID, EnvID))
        _doc.Save(My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\AppConfig.config")

    End Sub

End Class
