Imports Microsoft.VisualBasic

Partial Public Class OrderInterface
    Inherits System.Web.UI.Page
    Dim Common As New StoreOrderGuide.Common
    Dim m_iRowIndex As Integer = 1

#Region "Page Methods"
    Dim dtCatalogSchedule As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            '// Stylize DropdownList
            Common.SetDropDownStyle(ddlSubTeamSearch)
            Common.SetDropDownStyle(ddlClassSearch)
            Common.SetDropDownStyle(ddlBrandSearch)
            Common.SetDropDownStyle(ddlLevel3Search)

            '// Populate DropdownList
            If Not Page.IsPostBack Then
                Dim Dal As New Dal

                '// Populate ItemSearch DropdownLists
                If Not ddlSubTeamSearch.SelectedIndex > 0 Then
                    ddlSubTeamSearch.DataSource = Dal.GetSubTeamList(0)
                    ddlSubTeamSearch.DataBind()
                End If

                If Not ddlBrandSearch.SelectedIndex > 0 Then
                    ddlBrandSearch.DataSource = Dal.GetBrandList(0)
                    ddlBrandSearch.DataBind()
                End If
            End If

            Common.SetDropDownStyle(ddlStoreFilter)

            btnPush.OnClientClick = "return submitButtonClick(this);"

            If Not Page.IsPostBack Then
                Dim Dal As New Dal

                ddlStoreFilter.DataSource = Dal.GetStoreList()
                ddlStoreFilter.DataBind()

                lblStore.Text = HttpContext.Current.Session("StoreNo")
            End If

            PageAdminMode()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub PageAdminMode()
        Try
            If HttpContext.Current.Session("Warehouse") = "True" Or HttpContext.Current.Session("StoreNo") = "0" Then
                '// ViewState
                lblStore.Visible = False
                ddlStoreFilter.Visible = True

                '// Set various ODS parameters
                dsCatalog.SelectParameters.Item("StoreID").DefaultValue = ddlStoreFilter.SelectedValue
                dsCatalogItems.SelectParameters.Item("StoreNo").DefaultValue = ddlStoreFilter.SelectedValue
                dsOrder.InsertParameters.Item("StoreID").DefaultValue = ddlStoreFilter.SelectedValue
                dsCatalogSchedule.SelectParameters.Item("StoreNo").DefaultValue = ddlStoreFilter.SelectedValue
            Else
                '// ViewState
                lblStore.Visible = True
                ddlStoreFilter.Visible = False

                '// Set various ODS parameters
                dsCatalog.SelectParameters.Item("StoreID").DefaultValue = HttpContext.Current.Session("StoreNo")
                dsCatalogItems.SelectParameters.Item("StoreNo").DefaultValue = HttpContext.Current.Session("StoreNo")
                dsOrder.InsertParameters.Item("StoreID").DefaultValue = HttpContext.Current.Session("StoreNo")
                dsCatalogSchedule.SelectParameters.Item("StoreNo").DefaultValue = HttpContext.Current.Session("StoreNo")
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub PageOrderMode(ByVal OrderMode As Boolean)
        Try
            If OrderMode = False Then
                '// ViewState
                pnlFilter.Visible = True
                pnlCatalog.Visible = True

                pnlOrderDetails.Visible = False

                '// Control clean-up
                gvCatalogList.SelectedIndex = -1

                gvCatalogItemList.Columns.Clear()

                dsCatalog.SelectParameters("CatalogID").DefaultValue = 0
            Else
                '// StoreNo (Session or DropDownList)
                If HttpContext.Current.Session("Warehouse") = "True" Or HttpContext.Current.Session("StoreNo") = "0" Then
                    dvCatalogDetail.Rows(4).Cells(1).Text = ddlStoreFilter.SelectedValue
                Else
                    dvCatalogDetail.Rows(4).Cells(1).Text = HttpContext.Current.Session("StoreNo")
                End If

                '// Set CalendarSchedule parameters and retrieve
                dsCatalogSchedule.SelectParameters.Item("SubTeamNo").DefaultValue = dvCatalogDetail.Rows(2).Cells(1).Text
                dsCatalogSchedule.SelectParameters.Item("ManagedByID").DefaultValue = dvCatalogDetail.Rows(1).Cells(1).Text

                dsCatalogSchedule.Select()

                '// Expected Date ViewState (Calendar or Label)
                If dvCatalogDetail.Rows(7).Cells(1).Text = "False" Then
                    calExpectedDate.VisibleDate = Now()
                    calExpectedDate.Visible = True
                    lblExpectedDate.Visible = False
                Else
                    lblExpectedDate.Visible = True
                    calExpectedDate.Visible = False
                End If

                '// ViewState
                pnlFilter.Visible = False
                pnlCatalog.Visible = False
                pnlOrderDetails.Visible = True

                '// Register client script for JS updating of Order totals
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "updateOrderTotals", "updateOrderTotals();", True)
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub
#End Region

#Region "DataSource Methods"
    Sub dsOrder_Inserted(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs) Handles dsOrder.Inserted
        Try
            If e.ReturnValue > 0 Then
                For Each row As GridViewRow In gvCatalogItemList.Rows
                    Dim txtQty As TextBox = row.Cells(0).FindControl("txtQuantity")

                    If CInt(txtQty.Text) > 0 Then
                        dsOrderItems.InsertParameters("CatalogOrderID").DefaultValue = e.ReturnValue
                        dsOrderItems.InsertParameters("CatalogItemID").DefaultValue = gvCatalogItemList.DataKeys(row.RowIndex).Value
                        dsOrderItems.InsertParameters("Quantity").DefaultValue = CInt(txtQty.Text)

                        dsOrderItems.Insert()
                    End If
                Next

                PageOrderMode(False)

                dsOrder.UpdateParameters("CatalogOrderID").DefaultValue = e.ReturnValue

                dsOrder.Update()
            Else
                Common.MessageToUser(Page, New Exception("There was a problem inserting the Order, please contact support."), False)
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub dsOrder_Updated(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs) Handles dsOrder.Updated
        Try
            Common.SuccessToUser(Page, e.ReturnValue)

            ' TFS 5524 send order email confirmation to user with item details in HTML format:
            Dim msgBody As String = "<strong>Orders created in IRMA: </strong><br /><br /><TABLE border=1 cellSpacing=1 cellPadding=1 ><TR><TD>PO #</TD><TD>Item Description</TD><TD>Identifier</TD><TD><P align = center>Qty</P></TD><TD><P align = center>Cost</P></TD></TR>"

            For Each row As DataRow In e.ReturnValue.Tables(0).Rows
                msgBody += "<TR><TD>" + row.Item(0).ToString + "</TD><TD><font size=2>" + row.Item(1).ToString + "</font></TD><TD>" + row.Item(2).ToString + "</TD><TD><P align=right>" + row.Item(3).ToString + "</P></TD><TD><P align=right>" + row.Item(4).ToString + "</P></TD></TR>"
            Next

            msgBody += "</TABLE><br /><b>Thank you for using SOG!</b>"

            'Common.SendEmail("StoreOrderGuide - Order Entry", msgBody, HttpContext.Current.Session("Email"))
            Common.SendEmailHTML("StoreOrderGuide - Order Entry", msgBody, HttpContext.Current.Session("Email"))

        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub dsCatalogSchedule_Selected(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs) Handles dsCatalogSchedule.Selected
        Dim Dal As New Dal
        Dim hrExpectedDate As Integer = Dal.GetAdminSetting("ExpectedDate")
        Dim dtExpectedDate As Date
        Dim iWeekDay As Integer
        Dim dr As DataRow

        '// If no expected date found, use 36 hour as the default
        If hrExpectedDate > 0 Then
            dtExpectedDate = FormatDateTime(DateAdd(DateInterval.Hour, hrExpectedDate, Now), DateFormat.ShortDate)
        Else
            dtExpectedDate = FormatDateTime(DateAdd(DateInterval.Hour, 36, Now), DateFormat.ShortDate)
        End If

        dtCatalogSchedule = e.ReturnValue.Tables(0)

        '// ToDo - use variables and methods to do this
        For Each dr In dtCatalogSchedule.Rows
            If dr("Sun") = True Then
                iWeekDay = 7
            End If

            If dr("Sat") = True Then
                iWeekDay = 6
            End If

            If dr("Fri") = True Then
                iWeekDay = 5
            End If

            If dr("Thu") = True Then
                iWeekDay = 4
            End If

            If dr("Wed") = True Then
                iWeekDay = 3
            End If

            If dr("Tue") = True Then
                iWeekDay = 2
            End If

            If dr("Mon") = True Then
                iWeekDay = 1
            End If
        Next

        '// If no schedule located, use ExpectedDate value, otherwise use CatalogSchedule logic
        Select Case iWeekDay
            Case 0
                '// Set label value to to the day with the expected date minimum applied
                lblExpectedDate.Text = FormatDateTime(dtExpectedDate, DateFormat.ShortDate)

            Case Is > Weekday(dtExpectedDate, FirstDayOfWeek.Monday)
                '// Set label value to to the next available day this week with the expected date minimum applied
                lblExpectedDate.Text = FormatDateTime(DateAdd(DateInterval.Day, (iWeekDay - Weekday(dtExpectedDate, FirstDayOfWeek.Monday)), dtExpectedDate), DateFormat.ShortDate)

            Case Else
                '// Set label value to to the next available day of next week with the expected date minimum applied
                lblExpectedDate.Text = FormatDateTime(DateAdd(DateInterval.Day, (7 - (Weekday(dtExpectedDate, FirstDayOfWeek.Monday) - iWeekDay)), dtExpectedDate), DateFormat.ShortDate)
        End Select
    End Sub
#End Region

#Region "Control Methods"
    Sub ddlStoreFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlStoreFilter.SelectedIndexChanged
        Try
            pnlOrderDetails.Visible = False
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            PageAdminMode()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub btnPush_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPush.Click
        If Page.IsValid Then
            Try
                Dim totQty As Integer = 0

                For Each row As GridViewRow In gvCatalogItemList.Rows
                    Dim txtQty As TextBox = row.Cells(0).FindControl("txtQuantity")

                    If txtQty.Text = "" Then
                        txtQty.Text = 0
                    End If

                    totQty += CInt(txtQty.Text)
                Next

                If totQty > 0 And (calExpectedDate.SelectedDate > "1/1/1900" Or lblExpectedDate.Text <> "0") Then
                    Common.ValidateSession()
                    PageAdminMode()

                    '// Set ExpectedDate InsertParameter based upon ViewState logic handled elsewhere
                    If calExpectedDate.SelectedDate > "1/1/1900" Then
                        dsOrder.InsertParameters("ExpectedDate").DefaultValue = FormatDateTime(calExpectedDate.SelectedDate, DateFormat.ShortDate)
                    Else
                        dsOrder.InsertParameters("ExpectedDate").DefaultValue = FormatDateTime(lblExpectedDate.Text, DateFormat.ShortDate)
                    End If

                    dsOrder.Insert()
                Else
                    Common.MessageToUser(Page, New Exception("There were no quantities on the Order or an Expected Date was not selected."), False)
                End If
            Catch ex As Exception
                Common.LogError(ex)
                Throw
            End Try
        End If
    End Sub

    Sub btnCancelOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            PageOrderMode(False)
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub calExpectedDate_DayRender(ByVal sender As Object, ByVal e As DayRenderEventArgs) Handles calExpectedDate.DayRender
        Dim dr As DataRow

        '// ToDo - use variables and methods to do this
        For Each dr In dtCatalogSchedule.Rows
            Select Case e.Day.Date.DayOfWeek
                Case DayOfWeek.Monday
                    e.Day.IsSelectable = dr("Mon")
                Case DayOfWeek.Tuesday
                    e.Day.IsSelectable = dr("Tue")
                Case DayOfWeek.Wednesday
                    e.Day.IsSelectable = dr("Wed")
                Case DayOfWeek.Thursday
                    e.Day.IsSelectable = dr("Thu")
                Case DayOfWeek.Friday
                    e.Day.IsSelectable = dr("Fri")
                Case DayOfWeek.Saturday
                    e.Day.IsSelectable = dr("Sat")
                Case DayOfWeek.Sunday
                    e.Day.IsSelectable = dr("Sun")
            End Select
        Next

        '// Validate future day
        If e.Day.Date < Now() Then
            e.Day.IsSelectable = False
        End If

        '// Stylize cell
        If e.Day.IsSelectable = False Then
            e.Cell.ForeColor = System.Drawing.Color.LightGray
        End If

        '// Clean-up
        dtCatalogSchedule.Dispose()
    End Sub

    Sub calExpectedDate_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles calExpectedDate.SelectionChanged
        PageOrderMode(True)
    End Sub

    Sub calExpectedDate_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles calExpectedDate.VisibleMonthChanged
        PageOrderMode(True)

        calExpectedDate.VisibleDate = e.NewDate
    End Sub
#End Region

#Region "Catalog Grid Methods"
    Sub gvCatalogList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gvCatalogList.SelectedIndexChanged
        dsCatalog.SelectParameters("CatalogID").DefaultValue = gvCatalogList.SelectedValue

        Try
            If ddlStoreFilter.SelectedValue = 0 And CInt(lblStore.Text) <= 0 Then
                dsCatalog.SelectParameters("CatalogID").DefaultValue = 0

                Common.MessageToUser(Page, New Exception("A Store must be selected to create an Order."), False)
            Else
                PageOrderMode(True)

                dvCatalogDetail.ChangeMode(DetailsViewMode.ReadOnly)
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub gvCatalogList_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCatalogList.PageIndexChanged
        Try
            PageOrderMode(False)
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub gvCatalogList_Sorted(ByVal sender As Object, ByVal a As EventArgs) Handles gvCatalogList.Sorted
        Try
            PageOrderMode(False)
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub gvCatalogList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCatalogList.RowCommand
        Try
            If e.CommandName = "btnPrint" Then
                Dim sReportURL As New System.Text.StringBuilder
                Dim sReportServer As String
                sReportServer = HttpContext.Current.Application("reportingServicesURL")

                sReportURL.Append("SOG_PrintCatalog")

                sReportURL.Append("&rs:Command=Render")
                sReportURL.Append("&rc:Parameters=False")

                sReportURL.Append("&CatalogID=" & gvCatalogList.Rows(Val(e.CommandArgument)).Cells(2).Text)

                If HttpContext.Current.Session("Warehouse") = "True" Or HttpContext.Current.Session("StoreNo") = "0" Then
                    sReportURL.Append("&StoreNo=" & ddlStoreFilter.SelectedValue)
                Else
                    sReportURL.Append("&StoreNo=" & HttpContext.Current.Session("StoreNo"))
                End If

                Response.Redirect(sReportServer + sReportURL.ToString())
            End If
        Catch ex As Exception
            If ex.Message <> "Thread was being aborted." Then
                Common.LogError(ex)
                Throw New Exception(ex.Message, ex.InnerException)
            End If
        End Try
    End Sub
#End Region

#Region "Catalog Detail Methods"
    Sub dvCatalogDetail_ModeChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewModeEventArgs) Handles dvCatalogDetail.ModeChanging
        Try
            dvCatalogDetail.ChangeMode(e.NewMode)
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub
#End Region

#Region "Catalog Item Methods"

    'Private Sub gvCatalogItemList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCatalogItemList.RowCreated
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Attributes.Add("onKeyDown", "set_focus('" + m_iRowIndex.ToString() + "')")
    '    End If

    '    m_iRowIndex = m_iRowIndex + 1

    'End Sub
    Sub gvCatalogItemList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCatalogItemList.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                For Each cell As TableCell In e.Row.Cells
                    Select Case cell.Text
                        Case "N/A"
                            cell.Attributes.Add("title", "A = Not Authorized" + vbCrLf + "C = No Cost Found" + vbCrLf + "V = Not Available")
                    End Select

                    For Each ctl As Control In cell.Controls
                        If ctl.GetType().ToString().Contains("DataControlLinkButton") Then
                            Dim lb As LinkButton = ctl

                            Select Case lb.Text
                                Case "N/A"
                                    cell.Attributes.Add("title", "A = Not Authorized" + vbCrLf + "C = No Cost Found" + vbCrLf + "V = Not Available")
                            End Select
                        End If
                    Next
                Next
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.Cells(12).Text = "False" Then
                    e.Row.Enabled = False
                End If
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub
#End Region

    Private Sub ddlSubTeamSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubTeamSearch.SelectedIndexChanged
        Try
            ddlClassSearch.DataSource = Dal.GetClassList(0, ddlSubTeamSearch.SelectedValue)
            ddlClassSearch.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Private Sub ddlClassSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlClassSearch.SelectedIndexChanged
        Try
            ddlLevel3Search.DataSource = Dal.GetLevel3List(0, ddlClassSearch.SelectedValue)
            ddlLevel3Search.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            dsCatalogItems.Select()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub
End Class