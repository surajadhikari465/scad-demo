Imports System.Web.Configuration
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration


Namespace pp_irma

    Partial Class pullorders
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
            adoCON.Open()
            Dim buildPivot As New SqlCommand("PromoPivotTable", adoCON)
            buildPivot.CommandTimeout = 2400
            buildPivot.CommandType = Data.CommandType.StoredProcedure

            'buildPivot.Parameters.Add("@tname", SqlDbType.VarChar)
            'buildPivot.Parameters.Add("@row_fields", SqlDbType.VarChar)
            'buildPivot.Parameters.Add("@col_field", SqlDbType.VarChar)
            'buildPivot.Parameters.Add("@agg_func_list", SqlDbType.VarChar)
            'buildPivot.Parameters.Add("@where_clause", SqlDbType.VarChar)
            'buildPivot.Parameters.Add("@dest_table", SqlDbType.VarChar)
            'buildPivot.Parameters.Add("@show_query", SqlDbType.Char)
            'buildPivot.Parameters("@where_clause").Value = "PriceBatchPromoID IN (select pbp.PriceBatchPromoID from PriceBatchPromo pbp, PromoPreOrders ppo where ppo.item_key = pbp.item_key and ppo.PriceBatchPromoID = pbp.PriceBatchPromoID and (pbp.Sale_End_Date ='" & Session("enddate") & "') AND (pbp.Start_Date ='" & Session("startdate") & "') AND ((pbp.Dept_No = '" & Session("deptno") & "' or pbp.Dept_no in (select dept_no from SubTeam where team_no = '" & Session("deptno") & "'))))"
            'buildPivot.Parameters("@tname").Value = "PromoPreOrders"
            'buildPivot.Parameters("@row_fields").Value = "Item_Key,Identifier"
            'buildPivot.Parameters("@tname").Value = "PromoPreOrders ppo,PriceBatchPromo pbp, Item i, ItemBrand ib, Vendor v, ItemVendor iv, StoreItemVendor siv, VendorCostHistory vch, ItemUnit iu"
            'buildPivot.Parameters("@row_fields").Value = "ppo.Item_Key, ib.Brand_Name, i.Item_Description, vch.Package_Desc1, i.Package_Desc2, iu.Unit_Name, pbp.Sale_Price, pbp.Sale_Cost, pbp.Price, pbp.Comment1, pbp.Comment2, ppo.Identifier, v.CompanyName, iv.item_id"

            'buildPivot.Parameters("@col_field").Value = "Store_No"
            'buildPivot.Parameters("@agg_func_list").Value = "sum(ppo.OrderQty)"
            'buildPivot.Parameters("@where_clause").Value = "ppo.Item_Key = pbp.Item_Key and i.Brand_ID = ib.Brand_ID and pbp.Vendor_Id = v.Vendor_ID and iv.item_key = i.item_key and iv.vendor_id = v.vendor_ID and siv.store_no = pbp.store_no and siv.item_key = i.item_key and vch.StoreItemVendorID = siv.StoreItemVendorID and i.Package_Unit_ID = iu.Unit_ID and pbp.Start_Date = '" & Session("startdate") & "' and pbp.Sale_End_Date = '" & Session("enddate") & "' and pbp.PriceBatchPromoID IN (select pbp.PriceBatchPromoID from PriceBatchPromo pbp, PromoPreOrders ppo where ppo.item_key = pbp.item_key and ppo.PriceBatchPromoID = pbp.PriceBatchPromoID and (pbp.Sale_End_Date ='" & Session("enddate") & "') AND (pbp.Start_Date ='" & Session("startdate") & "') AND ((pbp.Dept_No = '" & Session("deptno") & "' or pbp.Dept_no in (select dept_no from SubTeam where team_no = '" & Session("deptno") & "'))))"
            'buildPivot.Parameters("@dest_table").Value = "##PivotPreOrders"
            'buildPivot.Parameters("@show_query").Value = "0"
            buildPivot.Parameters.Add(New SqlParameter("@startdate", Data.SqlDbType.SmallDateTime))
            buildPivot.Parameters.Add(New SqlParameter("@enddate", Data.SqlDbType.SmallDateTime))
            buildPivot.Parameters.Add(New SqlParameter("@deptno", Data.SqlDbType.Int))
            buildPivot.Parameters("@startdate").Value = Session("startdate")
            buildPivot.Parameters("@enddate").Value = Session("enddate")
            buildPivot.Parameters("@deptno").Value = Session("deptno")
            buildPivot.ExecuteNonQuery()
            'getPromo.Parameters.Add(New SqlParameter("@startdt", Data.SqlDbType.SmallDateTime))
            'getPromo.Parameters.Add(New SqlParameter("@enddt", Data.SqlDbType.SmallDateTime))
            'getPromo.Parameters.Add(New SqlParameter("@deptno", Data.SqlDbType.Int))

            'getPromo.Parameters("@deptno").Value = Session("deptno")
            'getPromo.Parameters("@startdt").Value = Session("startdate")
            'getPromo.Parameters("@enddt").Value = Session("enddate")
            'getPromo.Parameters.Add("@team", SqlDbType.VarChar)
            'getPromo.Parameters("@team").Value = Session("teamname")

            'Dim getPromo As New SqlDataAdapter("GetPromoOrders2", adoCON)

            'Dim ds As New DataSet()
            'getPromo.Fill(ds, "PromoPreOrders")

            'Dim getPromo As SqlCommand = New SqlCommand("GetPromoOrders2", adoCON)
            '-'Dim getPromo As SqlCommand = New SqlCommand("select * from ##PivotPreOrders", adoCON)

            '-'getPromo.CommandTimeout = 2400
            'getPromo.CommandType = Data.CommandType.StoredProcedure



            '-'getPromo.ExecuteNonQuery()

            '-'Dim readPromo As Data.SqlClient.SqlDataReader

            '-'getPromo.CommandTimeout = 2400
            '-' readPromo = getPromo.ExecuteReader
            '-'promoItems.DataSource = readPromo


            'promoItems.DataSource = ds
            'promoItems.DataMember = "PromoPreOrders"
            '-'promoItems.DataBind()
            '-'readPromo.Close()


            Dim DS As DataSet
            Dim MyCommand As SqlDataAdapter

            MyCommand = New SqlDataAdapter("select * from ##PivotPreOrders", adoCON)

            DS = New DataSet()
            MyCommand.Fill(DS, "PromoPreOrders")

            promoItems.DataSource = DS.Tables("PromoPreOrders").DefaultView
            promoItems.DataBind()
            adoCON.Close()

            'exportExcel()
        End Sub

        Private Sub exportExcel()
            'exports datagrid to excel outside the browser
            ' Set the content type to Excel



            Response.AddHeader("Content-Disposition", "attachment; filename=promopreorders.xls")
            Response.ContentType = "application/vnd.ms-excel"

            'Turn off the view state
            Me.EnableViewState = False

            'Remove the charset from the Content-Type header
            Response.Charset = String.Empty

            Dim myTextWriter As New System.IO.StringWriter
            Dim myHtmlTextWriter As New System.Web.UI.HtmlTextWriter(myTextWriter)

            'Get the HTML for the control
            promoItems.RenderControl(myHtmlTextWriter)

            'Write the HTML to the browser
            Response.Write(myTextWriter.ToString())

            'End the response
            Response.End()
        End Sub

        Private Sub expExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles expExcel.Click
            'exports datagrid to excel outside the browser
            ' Set the content type to Excel



            Response.AddHeader("Content-Disposition", "attachment; filename=promopreorders.xls")
            Response.ContentType = "application/vnd.ms-excel"

            'Turn off the view state
            Me.EnableViewState = False

            'Remove the charset from the Content-Type header
            Response.Charset = String.Empty

            Dim myTextWriter As New System.IO.StringWriter
            Dim myHtmlTextWriter As New System.Web.UI.HtmlTextWriter(myTextWriter)

            'Get the HTML for the control
            promoItems.RenderControl(myHtmlTextWriter)

            'Write the HTML to the browser
            Response.Write(myTextWriter.ToString())

            'End the response
            Response.End()
        End Sub
    End Class

End Namespace
