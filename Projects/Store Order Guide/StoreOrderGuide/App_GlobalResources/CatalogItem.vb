Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class CatalogItem

#Region "Private Properties"
    Private _CatalogItemID As Integer
    Private _Catalog As Catalog
    Private _InsertDate As DateTime
    Private _InsertUser As String
    Private _Item As Item
    Private _ItemNote As String
#End Region

#Region "Public Properties"
    Public Property CatalogItemID() As Integer
        Get
            Return _CatalogItemID
        End Get
        Set(ByVal value As Integer)
            _CatalogItemID = value
        End Set
    End Property

    Public Property Catalog() As Catalog
        Get
            Return _Catalog
        End Get
        Set(ByVal value As Catalog)
            _Catalog = value
        End Set
    End Property

    Public ReadOnly Property InsertDate() As DateTime
        Get
            Return _InsertDate
        End Get
    End Property

    Public Property InsertUser() As String
        Get
            Return _InsertUser
        End Get
        Set(ByVal value As String)
            _InsertUser = Trim(value)
        End Set
    End Property

    Public Property Item() As Item
        Get
            Return _Item
        End Get
        Set(ByVal value As Item)
            _Item = value
        End Set
    End Property

    Public Property ItemNote() As String
        Get
            Return _ItemNote
        End Get
        Set(ByVal value As String)
            _ItemNote = Trim(value)
        End Set
    End Property
#End Region

#Region "Public Methods"
    Public Function GetCatalogItems(ByVal CatalogID As Integer, ByVal StoreNo As Integer, ByVal Order As Boolean, ByVal Identifier As String, ByVal Description As String, ByVal SubTeamID As Integer, ByVal ClassID As Integer, ByVal Level3ID As Integer, ByVal BrandID As Integer) As Data.DataSet
        Dim Dal As New Dal

        Return Dal.GetCatalogItems(CatalogID, StoreNo, Order, Identifier, Description, SubTeamID, ClassID, Level3ID, BrandID)
    End Function

    Public Function DelCatalogItem(ByVal CatalogItemID As Integer) As Boolean
        Dim Dal As New Dal

        Return Dal.DelCatalogItem(CatalogItemID)
    End Function

    Public Function GetItemList(ByVal CatalogID As Integer, ByVal Identifier As String, ByVal Description As String, ByVal SubTeamID As Integer, ByVal ClassID As Integer, ByVal Level3ID As Integer, ByVal BrandID As Integer) As Data.DataSet
        Dim Dal As New Dal

        Return Dal.GetItemList(CatalogID, Identifier, Description, SubTeamID, ClassID, Level3ID, BrandID)
    End Function

    Public Function AddCatalogItem(ByVal CatalogID As Integer, ByVal ItemKey As Integer, ByVal UserName As String) As Boolean
        Dim Dal As New Dal

        Return Dal.AddCatalogItem(CatalogID, ItemKey, UserName)
    End Function

    Public Function SetCatalogItem(ByVal CatalogItemID As Integer, ByVal ItemNote As String) As Boolean
        Dim Dal As New Dal

        Me.CatalogItemID = CatalogItemID
        Me.ItemNote = ItemNote

        Dal.SetCatalogItem(Me, CatalogItemID)
    End Function
#End Region
End Class