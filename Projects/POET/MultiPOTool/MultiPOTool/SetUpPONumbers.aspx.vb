Imports Infragistics.WebUI.UltraWebGrid
Partial Public Class SetUpPONumbers
    Inherits System.Web.UI.Page

    Private Sub SetUpPONumbers_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim oCSS As HtmlGenericControl
        oCSS = New HtmlGenericControl
        oCSS.TagName = "style"
        oCSS.Attributes.Add("type", "text/css")
        oCSS.InnerHtml = "@import ""StyleSheet.css"";"
        Page.Header.Controls.Add(oCSS)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load, ddlRegion.DataBinding

        If Session("UserID") Is Nothing Then
            lblCreatePONumbers.Text = "There isn't anyone logged in"
        Else
            lblCreatePONumbers.Text = "Create PO Numbers for " + Session.Item("UserName").ToString
            lblExport.Text = "Export Selected PO Numbers for " + Session.Item("UserName").ToString
        End If
        If Not Page.IsPostBack Then

            btnDelete.Attributes.Add("onclick", _
                   "return confirm('Are you sure you want to delete?');")

            BindGrid()
        Else
            If ddlShowOption.SelectedValue <> ViewState("DisplayOption") Then
                BindGrid()
            End If
        End If

    End Sub

    Private Sub BindGrid()
        'Bind the GridView control to the data source.
        Dim br As New BOPONumbers
        Session("TaskTable") = br.GetUnusedPONumbersByUserID(Session("UserID")).Tables(0)
        Dim dt As DataTable = Session("TaskTable")

        If dt.Rows.Count = 0 Then
            trGrid.Visible = False
            trColmnExport.Visible = False
        End If

        gvPONumbers.DataSource = Session("TaskTable")
        gvPONumbers.PageSize = ddlShowOption.SelectedValue
        ViewState("DisplayOption") = ddlShowOption.SelectedValue
        gvPONumbers.DataBind()
    End Sub

    Protected Sub dsGetRegions_Selecting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles dsGetRegions.Selecting
        e.InputParameters("UserID") = Session("UserID")
    End Sub

    Protected Sub btnCreatePONumbers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreatePONumbers.Click

        Dim po As New BOPONumbers
        Dim regionID As Integer
        Dim poTypeID As Integer
        Dim userID As Integer
        Dim poCount As Integer
        Dim maxPOs As Integer

        maxPOs = CInt(ConfigurationManager.AppSettings("MaxPOsToCreate"))

        If Integer.TryParse(txtPOCount.Text.ToString, poCount) = False Or poCount > maxPOs Then
            lblCountError.Text = "enter a whole number; " & (maxPOs).ToString() & " or less"
            txtPOCount.Focus()
        Else
            trGrid.Visible = True
            trColmnExport.Visible = True
            lblGirdText.Text = "PO Numbers for " + Session.Item("UserName").ToString
            lblCountError.Text = Nothing
            regionID = CInt(ddlRegion.SelectedValue)
            poTypeID = CInt(ddlPOTypes.SelectedValue)
            userID = CInt(Session("UserID"))
            poCount = CInt(txtPOCount.Text)
            gvPONumbers.Visible = True
            po.AssignPONumbers(regionID, poTypeID, userID, poCount)
            BindGrid()
        End If
    End Sub

    Protected Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim columnCount As Integer

        GetChecked()
        If ViewState("SelectList") IsNot Nothing Then
            Dim exportList As New ArrayList
            exportList = CType(ViewState("SelectList"), ArrayList)
            If Integer.TryParse(txtColumnCount.Text, columnCount) Then
                columnCount = txtColumnCount.Text
                ExportToExcel(ExportRowFormat(exportList, columnCount))
            Else
                showMessage("Invalid column count, please try again.")
            End If
        Else
            showMessage("Please select POs to be exported.")
        End If

    End Sub

    Private Function ExportRowFormat(ByVal exportArray As ArrayList, ByVal columnCount As Integer) As DataTable
        Dim dt As New DataTable
        Dim iCount, iColumn, iRow As Integer

        'set up the datatable
        For iCount = 1 To columnCount
            dt.Columns.Add("Col" & iCount, System.Type.GetType("System.Int32"))
        Next

        'fill in data
        iColumn = 0
        iRow = 0
        dt.Rows.Add()
        For iCount = 0 To exportArray.Count - 1
            dt.Rows(iRow).Item(iColumn) = exportArray.Item(iCount).ToString
            If iColumn = columnCount - 1 Then
                iRow = iRow + 1
                iColumn = 0
                dt.Rows.Add()
            Else
                iColumn = iColumn + 1
            End If
        Next

        Return dt
    End Function

    Private Sub ExportToExcel(ByVal dtExport As DataTable)
        Dim attachment As String = "attachment; filename=POETPoNumbers.xls"
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"
        Dim tab As String = ""
        For Each dc As DataColumn In dtExport.Columns
            Response.Write(tab + dc.ColumnName)
            tab = vbTab
        Next
        Response.Write(vbLf)

        Dim i As Integer
        For Each dr As DataRow In dtExport.Rows
            tab = ""
            For i = 0 To dtExport.Columns.Count - 1
                Response.Write(tab + dr(i).ToString())
                tab = vbTab
            Next
            Response.Write(vbLf)
        Next

        Response.End()
    End Sub

    Private Sub GetChecked()
        Dim exportList As New ArrayList

        For Each dRow As GridViewRow In gvPONumbers.Rows
            If CType(dRow.Cells(0).FindControl("chkSelect"), CheckBox).Checked Then
                exportList.Add(dRow.Cells(0).Text)
            End If
        Next

        If exportList.Count <> 0 Then
            ViewState("SelectList") = exportList
        Else
            ViewState("SelectList") = Nothing
        End If

    End Sub

    Private Sub showMessage(ByVal textMessage As String)
        Dim javaScript As New System.Text.StringBuilder()

        javaScript.Append("<script language=JavaScript>" & vbLf)
        javaScript.Append("alert('" & textMessage & "');" & vbLf)
        javaScript.Append("</script>" & vbLf)

        ClientScript.RegisterStartupScript(Me.GetType, "ShowMessage", javaScript.ToString)
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim iDeleteCount As Integer

        GetChecked()
        If ViewState("SelectList") IsNot Nothing Then
            Dim deleteList As New ArrayList
            Dim br As New BOPONumbers
            deleteList = CType(ViewState("SelectList"), ArrayList)
            Try
                For iDeleteCount = 0 To deleteList.Count - 1
                    'call sp to delete by PO number
                    br.DeletePONumber(deleteList(iDeleteCount))
                Next
                showMessage("The PO Numbers have been successfully deleted.")
                BindGrid()
            Catch ex As Exception
                showMessage("An error has occured durring the deletion process, please try again.")
            End Try
        Else
            showMessage("Please select PO numbers to be deleted.")
        End If

    End Sub

    Private Sub gvPONumbers_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPONumbers.PageIndexChanging
        gvPONumbers.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Private Function GetSortDirection(ByVal column As String) As String

        ' By default, set the sort direction to ascending.
        Dim sortDirection = "ASC"

        ' Retrieve the last column that was sorted.
        Dim sortExpression = TryCast(ViewState("SortExpression"), String)

        If sortExpression IsNot Nothing Then
            ' Check if the same column is being sorted.
            ' Otherwise, the default value can be returned.
            If sortExpression = column Then
                Dim lastDirection = TryCast(ViewState("SortDirection"), String)
                If lastDirection IsNot Nothing _
                  AndAlso lastDirection = "ASC" Then

                    sortDirection = "DESC"

                End If
            End If
        End If

        ' Save new values in ViewState.
        ViewState("SortDirection") = sortDirection
        ViewState("SortExpression") = column

        Return sortDirection

    End Function

    Private Sub gvPONumbers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPONumbers.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            CType(e.Row.FindControl("chkAll"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & _
                    CType(e.Row.FindControl("chkAll"), CheckBox).ClientID & "')")
        End If
    End Sub

    Protected Sub gvPONumbers_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvPONumbers.Sorting

        'Retrieve the table from the session object.
        Dim dt = ctype(Session("TaskTable"), DataTable)

        If dt IsNot Nothing Then

            'Sort the data.
            dt.DefaultView.Sort = e.SortExpression & " " & GetSortDirection(e.SortExpression)
            gvPONumbers.DataSource = Session("TaskTable")
            gvPONumbers.DataBind()

        End If
    End Sub

End Class