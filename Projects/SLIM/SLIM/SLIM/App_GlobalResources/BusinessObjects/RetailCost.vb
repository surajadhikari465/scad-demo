Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols


Public Class RetailCost


#Region "Private Members"
    Private VendorID As Integer
    Private ItemID As Integer
    Private Price As Single
    Private Cost As Single
    Private Store As Integer = 0
    Private VendorName As String
    Private Multiple As Integer
    Private CaseSize As Integer
#End Region
#Region "Constructors"
    Sub New()

    End Sub

    Sub New(ByVal Item_Key As Integer)
        ItemID = Item_Key
    End Sub

#End Region
#Region "Properties"
    Public Property ItemKey() As Integer
        Get
            Return ItemID
        End Get
        Set(ByVal value As Integer)
            ItemID = value
        End Set
    End Property

    Public Property NewPrice() As Single
        Get
            Return Price
        End Get
        Set(ByVal value As Single)
            Price = value
        End Set
    End Property

    Public Property NewCost() As Single
        Get
            Return Cost
        End Get
        Set(ByVal value As Single)
            Cost = value
        End Set
    End Property

    Public Property StoreNo() As Integer
        Get
            Return Store
        End Get
        Set(ByVal value As Integer)
            Store = value
        End Set
    End Property

    Public Property VendorNameID() As String
        Get
            Return VendorName
        End Get
        Set(ByVal value As String)
            VendorName = value
        End Set
    End Property

    Public Property NewMultiple() As Integer
        Get
            Return Multiple
        End Get
        Set(ByVal value As Integer)
            Multiple = value
        End Set
    End Property

    Public Property CasePack() As Integer
        Get
            Return CaseSize
        End Get
        Set(ByVal value As Integer)
            CaseSize = value
        End Set
    End Property

#End Region
#Region "Private Methods"
    Private Function InsertRegPrice() As Integer
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        Dim result As Integer
        result = da.InsertPrice(ItemID, Nothing, Nothing, Store, Date.Now, Multiple, _
                    Price, Price, Nothing, "SLIM")
        Return result
    End Function

    Private Sub InsertRegCost()
        Dim da As New IrmaItemTableAdapters.ItemTableAdapter
        Dim uom As Integer
        Try
            uom = da.GetItemUOM(ItemID)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        da.InsertCost(Store, "|", ItemID, VendorID, Cost, Nothing, CaseSize, _
                    Date.Today, Nothing, False, Nothing, False, uom, uom)
    End Sub

    Private Function GetVendorID() As Integer
        Dim venID As Integer
        Dim da As New IrmaVendorTableAdapters.VendorTableAdapter
        venID = da.GetVendorID(VendorName)
        Return venID
    End Function
#End Region
#Region "Public Methods"
    Public Sub ChangePrice(ByVal NewPrice As Single, ByVal NewMultiple As Integer)
        Dim az As Integer
        Price = NewPrice
        Multiple = NewMultiple
        If Store = 0 Then
            Throw New MissingMemberException
        End If
        az = InsertRegPrice()
    End Sub

    Public Sub ChangeCost(ByVal NewCost As Single)
        Cost = NewCost
        If Store = 0 Then
            Throw New MissingMemberException
        End If
        If VendorName = "" Then
            Throw New MissingMemberException
        End If
        VendorID = GetVendorID()
        InsertRegCost()
    End Sub
#End Region

    


    
End Class
