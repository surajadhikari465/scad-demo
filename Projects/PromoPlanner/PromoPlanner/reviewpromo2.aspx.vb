
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration


Namespace pp_irma


Partial Class reviewpromo2


    Inherits System.Web.UI.Page
    Protected WithEvents loadStor As System.Web.UI.WebControls.Button
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
                'Dim delOrrders As New SqlClient.SqlCommand("delete from PromoPreOrders where Start_Date =" & "'" & Session("startdate") & "'" & "and Sale_End_Date =" & "'" & Session("enddate") & "'" & "and Dept_No in" & Session("deptno") & " and Item_Key not in (select Item_Key from Price where ptype='EDV')", adoCON)
                'Dim loadOrders As New SqlClient.SqlCommand("insert into PromoPreOrders (PriceBatchPromoID, Item_Key, Identifier, Store_No, ProjUnits) select PriceBatchPromoID, Item_Key, Identifier, Store_No, ProjUnits from PriceBatchPromo where Start_Date = '" & Session("startdate") & "'" & "and Sale_End_Date =" & "'" & Session("enddate") & "'" & "and Dept_No in (" & Session("deptno") & ")", adoCON)
                'adocon.Open()

                'loadOrders.ExecuteNonQuery()

                'adocon.Close()

            End If
        End Sub
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
            Dim Item_Key As Integer
            Item_Key = CType(e.Item.FindControl("lbl_Item_Key"), Label).Text

            Dim ID As Integer
            ID = CType(e.Item.FindControl("lbl_ID"), Label).Text

            adoCON.Close()
            Dim deleteItem As New SqlClient.SqlCommand("delete from PriceBatchPromo where PriceBatchPromoID = " & ID, adoCON)
            Dim deleteItem2 As New SqlClient.SqlCommand("delete from PriceBatchDetail where Item_Key = '" & Item_Key & "' and StartDate = '" & Session("startdate") & "' and Sale_End_Date = '" & Session("enddate") & "'", adoCON)
            adoCON.Open()
            deleteItem.ExecuteNonQuery()
            deleteItem2.ExecuteNonQuery()
            adoCON.Close()

            loadpromo()
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
        Dim ID As Integer
        ID = CType(e.Item.FindControl("lbl_ID2"), Label).Text
        Dim Item_Key As Integer
        Item_Key = CType(e.Item.FindControl("lbl_Item_Key2"), Label).Text
        Dim salPr As Double
        salPr = CType(e.Item.FindControl("txt_price"), TextBox).Text
        Dim salCo As Double
        salCo = CType(e.Item.FindControl("txt_cost"), TextBox).Text
        Dim comment As String
        comment = CType(e.Item.FindControl("txt_com1"), TextBox).Text
        Dim store As String
        store = CType(e.Item.FindControl("lbl_store2"), Label).Text
        Dim qty As Integer
        qty = CType(e.Item.FindControl("qty"), TextBox).Text

        Dim UpdatePromoItemRev As New SqlCommand("UpdatePromoItemRev", adoCON)


        adoCON.Open()

        UpdatePromoItemRev.CommandTimeout = 1200
        UpdatePromoItemRev.CommandType = Data.CommandType.StoredProcedure

        UpdatePromoItemRev.Parameters.Add(New SqlParameter("@ID", Data.SqlDbType.Int))
            UpdatePromoItemRev.Parameters.Add(New SqlParameter("@salPr", Data.SqlDbType.SmallMoney))
            UpdatePromoItemRev.Parameters.Add(New SqlParameter("@salCo", Data.SqlDbType.SmallMoney))
        UpdatePromoItemRev.Parameters.Add(New SqlParameter("@comment", Data.SqlDbType.VarChar, 50))
        UpdatePromoItemRev.Parameters.Add(New SqlParameter("@qty", Data.SqlDbType.Int))
        UpdatePromoItemRev.Parameters.Add(New SqlParameter("@key", Data.SqlDbType.Int))
        UpdatePromoItemRev.Parameters.Add(New SqlParameter("@store", Data.SqlDbType.Int))
        UpdatePromoItemRev.Parameters.Add(New SqlParameter("@strtDt", Data.SqlDbType.SmallDateTime))
        UpdatePromoItemRev.Parameters.Add(New SqlParameter("@endDt", Data.SqlDbType.SmallDateTime))

        UpdatePromoItemRev.Parameters("@salPr").Value = salPr
        UpdatePromoItemRev.Parameters("@salCo").Value = salCo
        UpdatePromoItemRev.Parameters("@ID").Value = ID
        UpdatePromoItemRev.Parameters("@comment").Value = comment
        UpdatePromoItemRev.Parameters("@qty").Value = qty
        UpdatePromoItemRev.Parameters("@store").Value = store
        UpdatePromoItemRev.Parameters("@key").Value = Item_Key
        UpdatePromoItemRev.Parameters("@endDt").Value = Session("enddate")
        UpdatePromoItemRev.Parameters("@strtDt").Value = Session("startdate")


        UpdatePromoItemRev.ExecuteNonQuery()
        adoCON.Close()
        promoItems.EditItemIndex = -1
        loadpromo()

       
    End Sub




        Private Sub main_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles main.Click
            Response.Redirect("index.aspx")
        End Sub


End Class

End Namespace
