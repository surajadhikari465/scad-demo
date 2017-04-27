
Imports System.Configuration.ConfigurationManager

Public Module Database

    Public ReadOnly Property IRMAConnection() As String
        Get
            Return ConnectionStrings("ItemCatalog").ConnectionString
        End Get
    End Property

    Public ReadOnly Property SecurityConnection() As String
        Get
            Return ConnectionStrings("Security").ConnectionString
        End Get
    End Property
    Public ReadOnly Property LocalDatabaseConnection() As String
        Get
            'For OLEdb connection string
            'Return String.Format(ConnectionStrings("LocalDB").ConnectionString, My.Application.Info.DirectoryPath, "INVENTORY.MDB")

            Return String.Format(ConnectionStrings("LocalDB").ConnectionString, My.Application.Info.DirectoryPath, "INVENTORY.MDB")
            'Return ConnectionStrings("LocalDB").ConnectionString
        End Get
    End Property
End Module

