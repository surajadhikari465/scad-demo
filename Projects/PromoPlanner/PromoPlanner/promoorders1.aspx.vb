Imports System.Web.Configuration
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Namespace pp_irma

Partial Class promoorders1
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

            'initial page for Store_Nos tp place orders they select Store_No, dept and promo dates 

            Session.Timeout = 2400

            'get available dates from PromoPreOrders table going back 30 days & populate dropdown
            Dim thirty2 As Date
            thirty2 = System.DateTime.Today.Subtract(System.TimeSpan.FromDays(30))
            Dim getOrdDates As New SqlClient.SqlCommand("select Start_Date, Sale_End_Date from PriceBatchPromo where Start_Date >'" & thirty2 & "' group by Start_Date, Sale_End_Date order by Start_Date", adoCON)
            Dim readOrdDates As Data.SqlClient.SqlDataReader
            adoCON.Open()

            readOrdDates = getOrdDates.ExecuteReader
            Do While readOrdDates.Read
                Dim dateList As New ListItem
                dateList.Text = readOrdDates("Start_Date") & " - " & readOrdDates("Sale_End_Date")
                dateList.Value = readOrdDates("Start_Date") & "," & readOrdDates("Sale_End_Date")
                orddates.Items.Add(dateList)
            Loop
            adoCON.Close()

            'get dept from promdepts tables and populate dropdown
            Dim getDepts As New SqlClient.SqlCommand("select Dept_No from PriceBatchPromo where Start_Date >'" & thirty2 & "' group by Dept_No", adoCON)
            Dim readDepts As Data.SqlClient.SqlDataReader
            adoCON.Open()
            readDepts = getDepts.ExecuteReader

            Do While readDepts.Read
                Dim deptList As New ListItem
                deptList.Text = readDepts.Item("Dept_No")
                deptList.Value = readDepts.Item("Dept_No")
                dept.Items.Add(deptList)
            Loop

            adoCON.Close()

            'get Store_No from promStore_Nos table and populate dropdown
            Dim getStore_Nos As New SqlClient.SqlCommand("select Store_No from Store order by Store_No", adoCON)
            Dim readStore_Nos As Data.SqlClient.SqlDataReader
            adoCON.Open()
            readStore_Nos = getStore_Nos.ExecuteReader

            Do While readStore_Nos.Read

                Dim Store_NoList As New ListItem
                Store_NoList.Text = readStore_Nos.Item("Store_No")
                Store_NoList.Value = readStore_Nos.Item("Store_No")
                Store_Nos.Items.Add(Store_NoList)

            Loop

            If Not Session("storeno") = Nothing Then
                Store_Nos.SelectedValue = Session("storeno")
            End If

            adoCON.Close()

        End Sub

        Private Sub getOrders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles getOrders.Click

            'set session variables from selected values in dropdowns
            Session("storeno") = Store_Nos.SelectedItem.Text
            Session("deptno") = dept.SelectedItem.Value
            Session("dept") = dept.SelectedItem.Text
            'split dates from dropdown into two variables and set default session variable for price zone to 1
            Dim strDate As String
            strDate = orddates.SelectedItem.Value
            Dim arrDate As Array
            arrDate = Split(strDate, ",")
            Session("startdate") = arrDate(0)
            Session("enddate") = arrDate(1)


            'go to order page
            Response.Redirect("promoorders2.aspx")
        End Sub
End Class

End Namespace
