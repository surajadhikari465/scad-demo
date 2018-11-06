Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class CatalogStore

#Region "Private Properties"
    Private _id As Integer
    Private _Catalog As Catalog
    Private _Store As Store
    Private _InsertDate As DateTime
    Private _InsertUser As String
#End Region

#Region "Public Properties"
    Public Property id() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property

    Public Property CatalogID() As Catalog
        Get
            Return _Catalog
        End Get
        Set(ByVal value As Catalog)
            _Catalog = value
        End Set
    End Property

    Public Property Store() As Store
        Get
            Return _Store
        End Get
        Set(ByVal value As Store)
            _Store = value
        End Set
    End Property

    Public Property InsertDate() As DateTime
        Get
            Return _InsertDate
        End Get
        Set(ByVal value As DateTime)
            _InsertDate = value
        End Set
    End Property

    Public Property InsertUser() As String
        Get
            Return _InsertUser
        End Get
        Set(ByVal value As String)
            _InsertUser = Trim(_InsertUser)
        End Set
    End Property
#End Region

#Region "Public Methods"
    Public Function GetCatalogStores(ByVal CatalogID As Integer) As Data.DataSet
        Dim Dal As New Dal

        Return Dal.GetCatalogStores(CatalogID)
    End Function

    Public Function GetStoreList() As Data.DataSet
        Dim Dal As New Dal

        Return Dal.GetStoreList()
    End Function

    Public Function AddCatalogStore(ByVal CatalogID As Integer, ByVal StoreID As Integer, ByVal UserName As String) As Boolean
        Dim Dal As New Dal

        Return Dal.AddCatalogStore(CatalogID, StoreID, UserName)
    End Function

    Public Function DelCatalogStore(ByVal CatalogStoreID As Integer) As Boolean
        Dim Dal As New Dal

        Return Dal.DelCatalogStore(CatalogStoreID)
    End Function
#End Region
End Class