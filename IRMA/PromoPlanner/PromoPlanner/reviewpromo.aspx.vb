Imports System.Web.Configuration
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration


Namespace pp_irma

Partial Class reviewpromo
    Inherits System.Web.UI.Page
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
            If Not Page.IsPostBack Then
                'loads promo into datagrid
                loadpromo()

            End If
        End Sub
    Sub loadpromo()
        'load existing promo item already entered with item just added for dates entered
        'Dim getPromo As New SqlClient.SqlCommand("select p.PriceBatchPromoID, p.Store_No, p.Item_Key, id.Identifier, v.Vendor_ID, v.CompanyName, u.Brand_ID, u.Item_Description, u.Package_Desc2, u.Package_Unit_ID, p.Sale_Price, p.Sale_Multiple, p.Dept_No, p.Comment1, p.Comment2, p.ProjUnits,f.Price, x.Unit_Name, p.Sale_Cost, br.Brand_Name from Vendor v,   PriceBatchPromo p, Item u, ItemIdentifier id, Price f, StoreItemVendor s, ItemUnit x, ItemBrand br with (nolock) where u.Brand_ID = br.Brand_ID and u.Package_Unit_ID = x.Unit_ID and p.Sale_End_Date ='" & Session("enddate") & "' and p.Start_Date ='" & Session("startdate") & "'and p.Dept_No in (" & Session("deptno") & ") and u.Item_Key = id.Item_Key and p.Item_Key = u.Item_Key and p.Item_Key = f.Item_Key  and p.Vendor_ID = v.Vendor_ID group by p.PriceBatchPromoID, p.Store_No, p.Item_Key, id.Identifier, v.Vendor_ID, u.Brand_ID, u.Item_Description, u.Package_Desc2, u.Package_Unit_ID,p.Sale_Cost, p.Sale_Price, p.Sale_Multiple, p.Dept_No, p.Comment1, p.Comment2, p.ProjUnits,f.Price, x.Unit_Name, br.Brand_Name, v.CompanyName order by id.Identifier desc, p.Store_No ", adoCON)

        adoCON.Open()
        Dim GetPromo As New SqlCommand("GetPromo", adoCON)
        GetPromo.CommandTimeout = 1200
        GetPromo.CommandType = Data.CommandType.StoredProcedure


        GetPromo.Parameters.Add(New SqlParameter("@strtDt", Data.SqlDbType.SmallDateTime))
        GetPromo.Parameters.Add(New SqlParameter("@endDt", Data.SqlDbType.SmallDateTime))
        GetPromo.Parameters.Add(New SqlParameter("@deptno", Data.SqlDbType.Int))

        GetPromo.Parameters("@deptno").Value = Session("deptno")
        GetPromo.Parameters("@strtDt").Value = Session("startdate")
        GetPromo.Parameters("@endDt").Value = Session("enddate")

        GetPromo.ExecuteNonQuery()

        Dim readPromo As Data.SqlClient.SqlDataReader

        GetPromo.CommandTimeout = 2400
        readPromo = GetPromo.ExecuteReader
        promoItems.DataSource = readPromo
        promoItems.DataBind()
        readPromo.Close()
        adoCON.Close()


    End Sub




    Sub promoItems_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Dim ID As String
        ID = CType(e.Item.FindControl("lbl_ID2"), Label).Text
        Dim salPr As Decimal
        salPr = CType(e.Item.FindControl("txt_price"), TextBox).Text
        Dim salCo As Decimal
        salCo = CType(e.Item.FindControl("txt_cost"), TextBox).Text
        Dim comment As String
        comment = CType(e.Item.FindControl("txt_com1"), TextBox).Text
        Dim qty As Integer
        qty = CType(e.Item.FindControl("qty"), TextBox).Text
        Dim UpdatePromoItem As New SqlCommand("UpdatePromoItem", adoCON)


        adoCON.Open()

        UpdatePromoItem.CommandTimeout = 1200
        UpdatePromoItem.CommandType = Data.CommandType.StoredProcedure

        UpdatePromoItem.Parameters.Add(New SqlParameter("@ID", Data.SqlDbType.Int))
            UpdatePromoItem.Parameters.Add(New SqlParameter("@salPr", Data.SqlDbType.SmallMoney))
            UpdatePromoItem.Parameters.Add(New SqlParameter("@salCo", Data.SqlDbType.SmallMoney))
        UpdatePromoItem.Parameters.Add(New SqlParameter("@comment", Data.SqlDbType.VarChar, 50))
        UpdatePromoItem.Parameters.Add(New SqlParameter("@qty", Data.SqlDbType.Int))

        UpdatePromoItem.Parameters("@salPr").Value = salPr
        UpdatePromoItem.Parameters("@salCo").Value = salCo
        UpdatePromoItem.Parameters("@ID").Value = ID
        UpdatePromoItem.Parameters("@comment").Value = comment
        UpdatePromoItem.Parameters("@qty").Value = qty
        UpdatePromoItem.ExecuteNonQuery()
        adoCON.Close()
        promoItems.EditItemIndex = -1
        loadpromo()

    End Sub

    'cencels item selected for edit and reloads promo to datagrid
    Sub promoItems_Cancel(ByVal Source As Object, ByVal e As DataGridCommandEventArgs)
        promoItems.EditItemIndex = -1
        loadpromo()
    End Sub


    'sets selected to edit mode and reloads promo to datagrid
    Sub promoItems_Edit(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        promoItems.EditItemIndex = CInt(e.Item.ItemIndex)
        loadpromo()
    End Sub


    'deletes selected item from table and reload rpomo to datagrid
    Sub promoItems_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        Dim ID As String
            ID = CType(e.Item.FindControl("lbl_ID"), Label).Text
        'Dim storeno As Integer
        'storeno = CType(e.Item.FindControl("lbl_storeno2"), Label).Text

        Dim deleteItem As New SqlClient.SqlCommand("delete from PriceBatchPromo where PriceBatchPromo_ID = " & ID, adoCON)
        adoCON.Open()
        deleteItem.ExecuteNonQuery()
        adoCON.Close()

        loadpromo()
    End Sub

    'loads promo data to PriceBatchDetail and PromoPreorders tables
    Private Sub loadStor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles loadStor.Click

        'load PromoPreOrders table for Store_No orders
            Dim loadOrders As New SqlClient.SqlCommand("insert into PromoPreOrders (PriceBatchPromoID, Item_Key, Identifier, Store_No, OrderQty) select p.PriceBatchPromoID, p.Item_Key, i.Identifier, p.Store_No, p.ProjUnits from PriceBatchPromo p, ItemIdentifier i where p.Start_Date =" & "'" & Session("startdate") & "'" & "and p.Sale_End_Date =" & "'" & Session("enddate") & "'" & "and p.Dept_No in (" & Session("deptno") & ")  and p.Item_Key =i.Item_Key and i.item_key not in (select item_key from PromoPreOrders where Start_Date =" & "'" & Session("startdate") & "'" & "and Sale_End_Date =" & "'" & Session("enddate") & "'" & "and Dept_No in (" & Session("deptno") & ")) group by p.PriceBatchPromoID, p.Item_Key, i.Identifier, p.Store_No, p.ProjUnits", adoCON)
        'eliminate EDVs not changing effprice
            Dim elimEDV As New SqlClient.SqlCommand("delete FROM PriceBatchPromo WHERE Item_Key in (SELECT a.Item_Key FROM PriceBatchPromo a, Price b WHERE a.Item_Key = b.Item_Key AND b.PriceChgTypeID = 3 AND a.Dept_No IN (" & Session("deptno") & ") and a.Sale_End_Date ='" & Session("enddate") & "' and a.Start_Date ='" & Session("startdate") & "' group by a.Item_Key) ", adoCON)

            'loads PriceBatchDetail table
        Dim InsertPriceBatchDetail As New SqlCommand("UpdatePriceBatchDetailPromoPlanner", adoCON)
        InsertPriceBatchDetail.CommandTimeout = 1200
        InsertPriceBatchDetail.CommandType = Data.CommandType.StoredProcedure

        InsertPriceBatchDetail.Parameters.Add(New SqlParameter("@deptno", Data.SqlDbType.Int))
        InsertPriceBatchDetail.Parameters.Add(New SqlParameter("@strtDt", Data.SqlDbType.SmallDateTime))
        InsertPriceBatchDetail.Parameters.Add(New SqlParameter("@endDt", Data.SqlDbType.SmallDateTime))

        InsertPriceBatchDetail.Parameters("@strtDt").Value = Session("startdate")
        InsertPriceBatchDetail.Parameters("@endDt").Value = Session("enddate")
        InsertPriceBatchDetail.Parameters("@deptno").Value = Session("deptno")

        adoCON.Open()
        loadOrders.ExecuteNonQuery()
        elimEDV.ExecuteNonQuery()
        InsertPriceBatchDetail.ExecuteNonQuery()
        adoCON.Close()

        Response.Redirect("index.aspx")

    End Sub

    Private Sub main_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles main.Click
        Response.Redirect("index.aspx")
    End Sub


End Class

End Namespace
