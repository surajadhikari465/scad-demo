Imports System.Web.Configuration
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration


Namespace pp_irma

Partial Class promoorders3
    Inherits System.Web.UI.Page
    Protected WithEvents iQty As System.Web.UI.WebControls.TextBox
    Protected WithEvents iID As System.Web.UI.WebControls.Label
    Protected WithEvents promo2 As System.Web.UI.WebControls.Repeater
    Dim adoCON As SqlConnection
    Dim strConnection As String
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If Session("storeno") Is Nothing Then Response.Redirect("~/")

            strConnection = System.Configuration.ConfigurationManager.ConnectionStrings("PromoPlanner_Conn").ToString
            adoCON = New SqlConnection(strConnection)
            'order review/verification page
            If Not Page.IsPostBack Then
                'load repeater oblect promo1 with promo item including 13 week movement
                loader()

                'load repeater oblect promo2 with promo items that have zero movement in the last 13 weeks
                'loader2()
            End If


        End Sub

    Sub loader()

        'Dim getPromo As New SqlClient.SqlCommand("select ppo.PromoPreOrderID, ppo.PriceBatchPromoID, pbp.Item_Key, ib.Brand_Name, i.Item_Description, i.Package_Desc2, iu.Unit_Name, pbp.Sale_Price, pbp.Sale_Cost, round((pbp.Sale_Price - pbp.Sale_Cost) / pbp.Sale_Price * 100, 2) as mgn, pbp.Price, pbp.Comment1, pbp.Comment2, ppo.Identifier,  ppo.OrderQty, v.CompanyName from PromoPreOrders ppo, PriceBatchPromo pbp, Item i, Vendor v, ItemBrand ib, ItemUnit iu With (nolock) where pbp.Sale_End_Date ='" & Session("enddate") & "' and pbp.Start_Date ='" & Session("startdate") & "' and pbp.Dept_No in ('" & Session("deptno") & "') and ppo.Store_No ='" & Session("storeno") & "' and pbp.PriceBatchPromoID = ppo.PriceBatchPromoID and ppo.Item_Key = pbp.Item_Key and ppo.Item_Key = i.Item_Key and i.Brand_ID = ib.Brand_ID and pbp.Vendor_ID = v.Vendor_ID and i.Package_Unit_ID = iu.Unit_Id group by ppo.PromoPreOrderID, ppo.PriceBatchPromoID, pbp.Item_Key, ib.Brand_Name, i.Item_Description, i.Package_Desc2, iu.Unit_Name, pbp.Sale_Price, pbp.Sale_Cost, pbp.Price, pbp.Comment1, pbp.Comment2, ppo.Identifier, ppo.OrderQty, v.CompanyName order by v.CompanyName, ib.Brand_Name, pbp.Item_Key", adoCON)
        Dim getPromo As New SqlCommand("GetPromoPreOrder", adoCON)
        adoCON.Open()
        getPromo.CommandTimeout = 2400
        getPromo.CommandType = Data.CommandType.StoredProcedure
        getPromo.Parameters.Add(New SqlParameter("@strtDt", Data.SqlDbType.SmallDateTime))
        getPromo.Parameters.Add(New SqlParameter("@endDt", Data.SqlDbType.SmallDateTime))
        getPromo.Parameters.Add(New SqlParameter("@deptno", Data.SqlDbType.Int))
        getPromo.Parameters.Add(New SqlParameter("@storeno", Data.SqlDbType.Int))
        getPromo.Parameters("@strtDt").Value = Session("startdate")
        getPromo.Parameters("@endDt").Value = Session("enddate")
        getPromo.Parameters("@storeno").Value = Session("storeno")
        getPromo.Parameters("@deptno").Value = Session("deptno")


        getPromo.ExecuteNonQuery()
        Dim readPromo As SqlDataReader
        readPromo = getPromo.ExecuteReader
        promo.DataSource = readPromo
        promo.DataBind()
        readPromo.Close()
        adoCON.Close()
    End Sub

    '

    Private Sub orderIt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles orderIt.Click
        Response.Redirect("promoorders1.aspx")


    End Sub

End Class

End Namespace
