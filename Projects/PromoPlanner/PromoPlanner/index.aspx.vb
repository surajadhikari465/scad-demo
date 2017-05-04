Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient

Namespace pp_irma

    Partial Class index
        Inherits System.Web.UI.Page
        Dim adoCON As SqlConnection
        Dim strConnection As String

        'Dim adoCON As New OleDb.OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=ItemCatalog_SP;Data Source=CEWD6501;")

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

            'load dropdownlist the available promo dates going back 30 days from table PriceBatchPromo
            Session.Timeout = 840
            Dim thirty As Date
            thirty = System.DateTime.Today.Subtract(System.TimeSpan.FromDays(30))
            Dim getDates As New SqlClient.SqlCommand("select Start_Date, Sale_End_Date from PriceBatchPromo where Start_Date >'" & thirty & "' group by Start_Date, Sale_End_Date order by Start_Date", adoCON)
            Dim readDates As Data.SqlClient.SqlDataReader

            adoCON.Open()
            readDates = getDates.ExecuteReader
            Do While readDates.Read
                Dim dateList As New ListItem
                dateList.Text = readDates("Start_Date") & " - " & readDates("Sale_End_Date")
                dateList.Value = readDates("Start_Date") & "," & readDates("Sale_End_Date")
                dates.Items.Add(dateList)
                dates2.Items.Add(dateList)
            Loop
            readDates.Close()

            'load dropdownlist with depts from table promdepts
            Dim getDepts As New SqlClient.SqlCommand("select SubTeam_No, SubTeam_Name from SubTeam order by SubTeam_Name", adoCON)
            Dim readDepts As Data.SqlClient.SqlDataReader
            readDepts = getDepts.ExecuteReader
            Do While readDepts.Read
                Dim deptList As New ListItem
                deptList.Text = readDepts.Item("SubTeam_Name")
                deptList.Value = readDepts.Item("SubTeam_No")
                dept.Items.Add(deptList)
            Loop
            readDepts.Close()

            'load available dates from table PromoPreOrders going back 30 days
            Dim thirty2 As Date
            thirty2 = System.DateTime.Today.Subtract(System.TimeSpan.FromDays(30))
            Dim getOrdDates As New SqlClient.SqlCommand("select Start_Date, Sale_End_Date from PriceBatchPromo where Start_Date >'" & thirty2 & "' and PriceBatchPromoID in (select PriceBatchPromoID from PromoPreOrders) group by Start_Date, Sale_End_Date order by Start_Date", adoCON)
            Dim readOrdDates As Data.SqlClient.SqlDataReader
            'adoCON.Open()

            readOrdDates = getOrdDates.ExecuteReader
            Do While readOrdDates.Read
                Dim dateList As New ListItem
                dateList.Text = readOrdDates("Start_Date") & " - " & readOrdDates("Sale_End_Date")
                dateList.Value = readOrdDates("Start_Date") & "," & readOrdDates("Sale_End_Date")
                orddates.Items.Add(dateList)
            Loop
            readOrdDates.Close()
            adoCON.Close()


        End Sub

        Private Sub buildPromo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buildPromo.Click
            'move to build promo page for selected dept
            Session("deptno") = dept.SelectedItem.Value
            Session("dept") = dept.SelectedItem.Text

            Response.Redirect("buildpromo.aspx")
        End Sub

        Private Sub revPromos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles revPromos.Click
            'move to review promo page for the selected dates and dept
            Dim strDate As String
            strDate = dates.SelectedItem.Value
            Dim arrDate As Array
            arrDate = Split(strDate, ",")
            Session("startdate") = arrDate(0)
            Session("enddate") = arrDate(1)
            Session("deptno") = dept.SelectedItem.Value
            Session("dept") = dept.SelectedItem.Text
            Response.Redirect("reviewpromo.aspx")
        End Sub

        Private Sub getOrders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles getOrders.Click
            'move to pull Store_No orders page for the selected dates and dept
            Dim strDate As String
            strDate = orddates.SelectedItem.Value
            Session("strDate") = strDate
            Dim arrDate As Array
            arrDate = Split(strDate, ",")
            Dim startdate As String
            startdate = arrDate(0)
            Session("startdate") = startdate
            Dim enddate As String
            enddate = arrDate(1)
            Session("enddate") = enddate
            Dim deptno As String
            deptno = dept.SelectedItem.Value
            Session("deptno") = deptno

            Response.Redirect("pullorders.aspx")
        End Sub

        Private Sub revPromos2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles revPromos2.Click
            Dim strDate As String
            strDate = dates2.SelectedItem.Value
            Dim arrDate As Array
            arrDate = Split(strDate, ",")
            Session("startdate") = arrDate(0)
            Session("enddate") = arrDate(1)
            Session("deptno") = dept.SelectedItem.Value
            Session("dept") = dept.SelectedItem.Text
            Response.Redirect("reviewpromo2.aspx")
        End Sub

        Protected Sub lnkUploadPromos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkUploadPromos.Click
            Server.Transfer("UploadPromos.aspx")
        End Sub
    End Class

End Namespace
