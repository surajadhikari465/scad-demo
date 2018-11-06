Imports System.Drawing
Imports Infragistics.WebUI
Imports Infragistics.Excel

Partial Class UserInterface_WebQuery_SearchItemDetail
    Inherits WebQueryPage

    Dim txbx As Integer
    Dim colorGrayBG As System.Drawing.Color
    Dim colorGrayFG As System.Drawing.Color


    Public Property ShowMovementInfo() As Boolean
        Get
            Dim info As Object = ViewState("ShowMovementInfo")

            If info Is Nothing Then
                Return False
            End If

            Return CBool(info)
        End Get
        Set(ByVal value As Boolean)
            ViewState("ShowMovementInfo") = value
        End Set
    End Property

    Protected Sub LoadItemDetails()
        Dim details As DataRow = ItemWebQuery.GetItemDetails(CInt(Session("ItemKey")), CInt(Session("StoreJurisdictionID")))

        Label_UPC.Text = details("Identifier")
        Label_Desc.Text = details("Item_Description")
        Label_Size.Text = details("Package_Desc2")
        Label_UnitOfMeasure.Text = details("Unit_Name")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Panel_Movement.Visible = (Session("LoggedIn") = 1)

        If Not Request.QueryString("i") = String.Empty Then
            Session("ItemKey") = Request.QueryString("i")
        End If

        If Not Request.QueryString("j") = String.Empty Then
            Session("StoreJurisdictionID") = Request.QueryString("j")
        End If

        If Application.Get("Allow_Unsecure_Excel_Export") = 0 Then
            LinkButton1.Visible = False
        End If

        LinkButton1.Visible = (Session("LoggedIn") = 1)

        ExcelLogo.Visible = (Session("LoggedIn") = 1)

        If Not IsPostBack Then
            If _isMobileDevice Then
                mvDetail.ActiveViewIndex = 1

                ' Toggle which calendar control to use
                StartDate.Visible = False
                EndDate.Visible = False
                calStartDate.Visible = True
                calEndDate.Visible = True
                calStartDate.VisibleDate = Date.Today
                calEndDate.VisibleDate = Date.Today
            End If

            LoadItemDetails()
        End If

        colorGrayBG = Color.FromArgb(255, 128, 128, 128)
        colorGrayFG = Color.FromArgb(255, 75, 75, 75)

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        'TODO: Get Movement when we have some data  *****
        Dim timeSpanWeeks As Integer
        Dim timeSpanDays As Integer
        Dim startVal As Date
        Dim endVal As Date

        timeSpanWeeks = CInt(Application.Get("maxTimeSpanMovement"))
        timeSpanDays = (timeSpanWeeks * 7)

        If _isMobileDevice Then
            startVal = calStartDate.SelectedDate
            endVal = calEndDate.SelectedDate
        Else
            startVal = CDate(StartDate.Text)
            endVal = CDate(EndDate.Text)
        End If

        ShowMovementInfo = False

        If IIf(_isMobileDevice, _
            calStartDate.SelectedDate.Equals(Date.MinValue) OrElse calEndDate.SelectedDate.Equals(Date.MinValue), _
            StartDate.Text = String.Empty Or EndDate.Text = String.Empty) Then
            Label5.Text = "Date(s) not supplied!"
        ElseIf startVal > Date.Today OrElse endVal > Date.Today Then
            Label5.Text = "Future Dates are not allowed!"
        ElseIf startVal > endVal Then
            Label5.Text = "StartDate cannot be larger than EndDate!"
        ElseIf DateDiff(DateInterval.Day, startVal, endVal) > timeSpanDays Then
            Label5.Text = "Maximun TimeSpan Allowed is " & timeSpanWeeks & " weeks!"
        Else
            Label5.Text = String.Empty
            Session("StartDate") = startVal
            Session("EndDate") = endVal
            UltraWebGrid1.DataSourceID = "SqlDataSource1"
            Try
                UltraWebGrid1.DataBind()
                ShowMovementInfo = True
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try

        End If
    End Sub

    Protected Sub UltraWebGrid1_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles UltraWebGrid1.InitializeLayout
        With UltraWebGrid1.DisplayLayout
            .Pager.AllowPaging = False
            .Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.PrevNext
            .Pager.PageSize = 60
            .Bands(0).AllowAdd = UltraWebGrid.AllowAddNew.No
            .Bands(0).AllowDelete = UltraWebGrid.AllowDelete.No
            .Bands(0).AllowUpdate = UltraWebGrid.AllowUpdate.No

            If Not Session("LoggedIn") = 1 Then
                .Bands(0).Columns.FromKey("Margin").Hidden = True
                .Bands(0).Columns.FromKey("SaleUnits").Hidden = True
                .Bands(0).Columns.FromKey("SaleDollars").Hidden = True
                .Bands(0).Columns.FromKey("UnitCost").Hidden = True
                .Bands(0).Columns.FromKey("NetMargin").Hidden = True
                .Bands(0).Columns.FromKey("NetCost").Hidden = True
            End If

            If CInt(Application.Get("WebQuery_Show_POSPrice")) = 0 Then
                .Bands(0).Columns.FromKey("POS_Price").Hidden = True
                .Bands(0).Columns.FromKey("POS_Price_Date").Hidden = True
            End If

            If CInt(Application.Get("WebQuery_Show_NetUnitCost")) = 0 Then
                .Bands(0).Columns.FromKey("NetMargin").Hidden = True
                .Bands(0).Columns.FromKey("NetCost").Hidden = True
            End If
        End With
    End Sub

    Protected Sub SqlDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SqlDataSource1.Selecting
        e.Command.CommandTimeout = 1100
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Dim book As Workbook = New Workbook()
        book.Worksheets.Add("WebQuery")

        If ShowMovementInfo Then
            UltraWebGrid1.DataSourceID = "SqlDataSource1"
            UltraWebGrid1.DataBind()
        End If

        UltraWebGridExcelExporter1.Export(UltraWebGrid1, book)
        'BIFF8Writer.WriteWorkbookToStream(book, Response.OutputStream)
        book.Save(Response.OutputStream)
    End Sub

    Protected Sub UltraWebGrid1_InitializeRow(sender As Object, e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles UltraWebGrid1.InitializeRow
        If (e.Row.Cells.FromKey("DiscontinueItem").Value = True) Then
            e.Row.Style.BackColor = colorGrayBG
            e.Row.Style.ForeColor = colorGrayFG
        End If
    End Sub
End Class
