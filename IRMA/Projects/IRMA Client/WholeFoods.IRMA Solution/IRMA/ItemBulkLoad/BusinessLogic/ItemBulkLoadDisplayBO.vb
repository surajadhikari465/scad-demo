Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Namespace WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
    Public Class ItemBulkLoadDisplayBO

        Private _validItems As Collection
        Private _identifierInvalidItems As Collection
        Private _invalidSubTeamItems As Collection
        Private _itemUploadHeaderID As Integer


        Property ValidItems() As Collection
            Get
                Return _validItems
            End Get
            Set(ByVal value As Collection)
                _validItems = value
            End Set
        End Property

        Property IdentifierInvalidItems() As Collection
            Get
                Return _identifierInvalidItems
            End Get
            Set(ByVal value As Collection)
                _identifierInvalidItems = value
            End Set
        End Property

        Property InvalidSubTeamItems() As Collection
            Get
                Return _invalidSubTeamItems
            End Get
            Set(ByVal value As Collection)
                _invalidSubTeamItems = value
            End Set
        End Property

        Property ItemUploadHeaderID() As Integer
            Get
                Return _itemUploadHeaderID
            End Get
            Set(ByVal value As Integer)
                _itemUploadHeaderID = value
            End Set
        End Property
    End Class
End Namespace
