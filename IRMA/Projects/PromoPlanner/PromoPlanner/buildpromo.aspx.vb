Imports System.Web.Configuration
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration


Namespace pp_irma

    Partial Class buildpromo
        Inherits System.Web.UI.Page
        Protected WithEvents Button1 As System.Web.UI.WebControls.Button
        Protected WithEvents zone As System.Web.UI.WebControls.DropDownList
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

            Session.Timeout = 2400
            strConnection = System.Configuration.ConfigurationManager.ConnectionStrings("PromoPlanner_Conn").ToString
            adoCON = New SqlConnection(strConnection)
            'clear session variables for promo dates if page is loaded for the first time and load existing promo for the dates entered 
            If Not Page.IsPostBack Then

                Session("startdate") = "01/01/1900"
                Session("enddate") = "01/01/1900"
                loadpromo()
                loadVendor_IDs()
            End If
            'call loadVendor_IDs sub

            'call javascript to format 13 digit Item_Key
            upc.Attributes("onblur") = "javascript:this.value=pad(this.value);"


        End Sub

        Sub loadVendor_IDs()
            'loads dropdownlist with Vendor_IDs from VendorCostHistory table to allow item pricing to be specific to that Vendor_ID when item is added
            Dim getVendors As New SqlClient.SqlCommand("select Vendor_ID, CompanyName from Vendor where Vendor_ID in (select Vendor_ID from ItemVendor where Item_Key in (select Item_Key from Item where SubTeam_No in (" & Session("deptno") & "))) group by Vendor_ID, CompanyName order by Vendor_ID", adoCON)
            Dim readVendors As Data.SqlClient.SqlDataReader
            adoCON.Open()
            readVendors = getVendors.ExecuteReader

            Do While readVendors.Read
                Dim Vendor_IDlist As New ListItem
                Vendor_IDlist.Text = readVendors("CompanyName")
                Vendor_IDlist.Value = readVendors("Vendor_ID")
                Vendor_ID.Items.Add(Vendor_IDlist)
            Loop
            readVendors.Close()

            adoCON.Close()
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

        Private Sub insertItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles insertItem.Click
            'enter item to promo from fields populated on screen

            adoCON.Open()
            Dim query As String
            Dim ckupcno As String
            ckupcno = upc.Text
            query = "select count(*) as thenumber from PriceBatchPromo where (SUBSTRING('00000000000000', 1, 13 - LEN(Identifier)) + Identifier + '0' =  '0' + '" & ckupcno & "') and Start_Date = '" & startdate.Text & "' and Sale_End_Date ='" & enddate.Text & "'"
            Dim getCount As New SqlClient.SqlCommand(query, adoCON)
            Dim query2 As String
            query2 = "select Discontinue_Item from Item where Item_Key in (select Item_Key from ItemIdentifier where (SUBSTRING('00000000000000', 1, 13 - LEN(Identifier)) + Identifier + '0' = '0' +  '" & ckupcno & "'))"
            Dim getDIS As New SqlClient.SqlCommand(query2, adoCON)
            Dim readDIS As Data.SqlClient.SqlDataReader
            Dim readCount As Data.SqlClient.SqlDataReader
            readCount = getCount.ExecuteReader()
            Dim thecount As Integer
            Dim dis As Boolean
            'Dim Multiple As String
            While readCount.Read

                thecount = readCount("thenumber")
            End While
            readCount.Close()
            readDIS = getDIS.ExecuteReader
            While readDIS.Read
                dis = readDIS("Discontinue_Item")
            End While
            readDIS.Close()
            If dis = True Then
                Response.Write("<table><tr><td><font color=red runat=server size=15>The item you are trying to add is a DIS item.<br>It cannot be added to the Promo Planner<br>Please use the browser's BACK button to return to Promo Planner Entry</font></td></tr></table>")
                Response.End()
            End If
            If thecount > 0 Then
                Response.Write("<table><tr><td><font color=red runat=server size=15>This upc has already been entered.<br> Use the browser's BACK button to return to the entry page and delete the current entry if needed<br> or enter a different upc!</font></td></tr></table>")
                Response.End()

            End If

            'variables
            Dim upcno As String
            upcno = upc.Text
            Dim salPrc As Decimal
            Session("salPrc") = Sale_Price.Text
            salPrc = Sale_Price.Text
            Dim salCst As Decimal
            Session("salCst") = salecost.Text
            salCst = salecost.Text
            Dim salPerc As Double
            salPerc = pricePerc.Text
            Dim cstPerc As Double
            cstPerc = costPerc.Text
            Dim com1 As String
            com1 = Replace(comments.Text, " ", "_")
            Dim vndr As String
            Session("vndr") = Vendor_ID.SelectedItem.Value
            vndr = Vendor_ID.SelectedItem.Value
            Dim bbk As Decimal
            Session("bbk") = billBack.Text
            bbk = billBack.Text
            Dim strtDt As String
            strtDt = startdate.Text
            Session("startdate") = strtDt
            Dim endDt As String
            endDt = enddate.Text
            Session("enddate") = endDt
            Dim pm As Integer
            pm = salPM.Text
            Dim com2 As String
            com2 = NorY.Text
            Dim qty As Integer
            qty = defQty.Text

            Dim InsertPromoPlanner As New SqlCommand("InsertPromoPlanner", adoCON)
            InsertPromoPlanner.CommandTimeout = 1200
            InsertPromoPlanner.CommandType = Data.CommandType.StoredProcedure

            InsertPromoPlanner.Parameters.Add(New SqlParameter("@upcno", Data.SqlDbType.Char, 13))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@salPrc", Data.SqlDbType.SmallMoney))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@salCst", Data.SqlDbType.SmallMoney))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@salPerc", Data.SqlDbType.Decimal, 9, 4))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@cstPerc", Data.SqlDbType.Decimal, 9, 4))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@com1", Data.SqlDbType.VarChar, 50))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@vndr", Data.SqlDbType.Int))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@bbk", Data.SqlDbType.Int))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@strtDt", Data.SqlDbType.SmallDateTime))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@endDt", Data.SqlDbType.SmallDateTime))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@pm", Data.SqlDbType.Int))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@com2", Data.SqlDbType.VarChar, 50))
            InsertPromoPlanner.Parameters.Add(New SqlParameter("@qty", Data.SqlDbType.Int))
            InsertPromoPlanner.Parameters("@upcno").Value = upcno

            InsertPromoPlanner.Parameters("@salPrc").Value = salPrc
            InsertPromoPlanner.Parameters("@salCst").Value = salCst
            InsertPromoPlanner.Parameters("@salPerc").Value = salPerc
            InsertPromoPlanner.Parameters("@cstPerc").Value = cstPerc
            InsertPromoPlanner.Parameters("@com1").Value = com1
            InsertPromoPlanner.Parameters("@vndr").Value = vndr
            InsertPromoPlanner.Parameters("@bbk").Value = bbk
            InsertPromoPlanner.Parameters("@strtDt").Value = strtDt
            InsertPromoPlanner.Parameters("@endDt").Value = endDt
            InsertPromoPlanner.Parameters("@pm").Value = pm
            InsertPromoPlanner.Parameters("@com2").Value = com2
            InsertPromoPlanner.Parameters("@qty").Value = qty

            InsertPromoPlanner.ExecuteNonQuery()


            adoCON.Close()
            loadpromo()
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
            'Dim storeno As String
            'storeno = CType(e.Item.FindControl("lbl_storeno"), Label).Text
            Dim qty As Integer
            qty = CType(e.Item.FindControl("qty"), TextBox).Text
            'Dim updateItem As New SqlClient.SqlCommand("update PriceBatchPromo  set comment1 = '" & comment & "', Sale_Price='" & salPr & "',  projunits = '" & qty & "' where PriceBatchPromoID = " & ID, adoCON)
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

        'if in edit mode puts item selected back into view mode and reloads promo into datagrid
        Sub promoItems_Cancel(ByVal Source As Object, ByVal e As DataGridCommandEventArgs)
            promoItems.EditItemIndex = -1
            loadpromo()
        End Sub

        'outs datagrid item selected into edit mode and reloads promo into datagrid
        Sub promoItems_Edit(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
            promoItems.EditItemIndex = CInt(e.Item.ItemIndex)
            loadpromo()
        End Sub

        'deletes an item from the promo being built and reloads promo into datagrid
        Sub promoItems_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

            Dim ID As String
            ID = CType(e.Item.FindControl("lbl_ID"), Label).Text

            Dim deleteItem As New SqlClient.SqlCommand("delete from PriceBatchPromo where PriceBatchPromo_ID = " & ID, adoCON)
            adoCON.Open()
            deleteItem.ExecuteNonQuery()
            adoCON.Close()

            loadpromo()
        End Sub

        Private Sub main_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles main.Click
            Response.Redirect("index.aspx")
        End Sub

    End Class

End Namespace
