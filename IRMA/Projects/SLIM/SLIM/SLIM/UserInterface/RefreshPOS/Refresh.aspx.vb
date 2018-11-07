Imports Infragistics.WebUI

Partial Class UserInterface_RefreshPOS_Refresh
    Inherits RefreshPOSPage

    Private lockAuthTable As New DataTable
    Private lockAuthJurisdictionIds As New List(Of Integer)
    Private lockAuthJurisdictionDescriptions As New List(Of String)
    Private authLockedForAllStores As Boolean = True

#Region "Helper Methods"

    Private Function RefreshCheckbox(ByVal row As GridViewRow) As CheckBox
        Return CType(row.Cells(1).FindControl("cbRefresh"), CheckBox)
    End Function

    Private Function GetDataSource(ByRef Item_Key As Integer, ByRef Store_No As Integer) As DataTable
        Dim rp As New RefreshPOS()
        Dim lockedJurisdictions As String = Nothing

        Item_Key = Request.QueryString("i")

        If Session("AccessLevel") = 3 Then
            Store_No = Dropdown_Store_List.SelectedValue
        Else
            Store_No = Session("Store_No")
        End If

        lockAuthTable = LockAuthCheck.IsAuthLockedByJurisdiction(Item_Key)

        For Each row As DataRow In lockAuthTable.Rows
            If row.Item("LockAuth") = True Then
                lockAuthJurisdictionDescriptions.Add(row.Item("JurisdictionDescription"))
                lockAuthJurisdictionIds.Add(row.Item("JurisdictionID"))
            End If
        Next

        For Each item As String In lockAuthJurisdictionDescriptions
            lockedJurisdictions = lockedJurisdictions + " " + item.ToString()
        Next

        If Not IsPostBack And lockAuthJurisdictionDescriptions.Count > 0 Then
            lblRefreshMessage.Visible = True
            lblRefreshMessage.ForeColor = Drawing.Color.Red
            lblRefreshMessage.Text = String.Format("Refresh is locked for the following jurisdictions: {0}", lockedJurisdictions)
        End If

        Return rp.GetStoreItemRefresh(Item_Key, Store_No)
    End Function

    Private Sub LoadAndBindMobileRefresh()
        Dim itemKey As Integer
        Dim storeNo As Integer
        gvRefresh.DataSource = GetDataSource(itemKey, storeNo)
        gvRefresh.DataBind()
    End Sub

    Private Sub UpdateStoreItemRefresh(ByVal Item_Key As Integer, ByVal rp As RefreshPOS, ByVal Store_No As Integer, ByVal Refresh As Boolean, ByVal Reason As String, ByVal UserID As Integer)
        Try
            rp.UpdateStoreItemRefresh(Item_Key, Store_No, Refresh, Reason, UserID)
        Catch ex As Exception
            Label_Error.Visible = True
            Label_Error.Text = "An error has occured when attempting to update the Item"
        End Try
    End Sub

    Private Sub UpdateRefreshFromGridView(ByVal Item_Key As Integer, ByVal rp As RefreshPOS)
        For Each row As GridViewRow In gvRefresh.Rows
            UpdateStoreItemRefresh(Item_Key, rp, gvRefresh.DataKeys(row.RowIndex).Value, RefreshCheckbox(row).Checked, ddlReason.SelectedValue, Session("UserID"))
        Next
    End Sub

    Private Sub UpdateRefreshFromUltraGrid(ByVal Item_Key As Integer, ByVal rp As RefreshPOS)
        For Each iRow As UltraWebGrid.UltraGridRow In UltraWebGrid2.Rows
            UpdateStoreItemRefresh(Item_Key, rp, iRow.Cells.FromKey("Store_No").Value, iRow.Cells.FromKey("Refresh").Value, ddlReason.SelectedValue, Session("UserID"))
        Next
    End Sub

#End Region

#Region "UltraWebGrid Event Handlers"
    Protected Sub UltraWebGrid2_InitializeDataSource(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.UltraGridEventArgs) Handles UltraWebGrid2.InitializeDataSource
        Dim Item_Key As Integer
        Dim Store_No As Integer

        UltraWebGrid2.DataSource = GetDataSource(Item_Key, Store_No)
        UltraWebGrid2.DataBind()

        If authLockedForAllStores Then
            Button_Submit.Enabled = False
        End If
    End Sub

    Protected Sub UltraWebGrid2_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles UltraWebGrid2.InitializeLayout
        Dim backColor As System.Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#004000")
        Dim AltColor As System.Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#C0FFC0")

        UltraWebGrid2.Font.Size = 12
        UltraWebGrid2.Font.Name = "Tahoma"

        With UltraWebGrid2.DisplayLayout
            .Pager.AllowPaging = True
            .Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.PrevNext
            .Pager.PageSize = 50
            .Bands(0).AllowAdd = UltraWebGrid.AllowAddNew.No
            .Bands(0).AllowDelete = UltraWebGrid.AllowDelete.No
            .ColWidthDefault = Nothing
            .HeaderStyleDefault.BackColor = backColor
            .HeaderStyleDefault.ForeColor = Drawing.Color.White
            .RowAlternateStyleDefault.BackColor = AltColor
        End With

        UltraWebGrid2.Columns(0).Hidden = True
        UltraWebGrid2.Columns(2).AllowUpdate = UltraWebGrid.AllowUpdate.Yes
        UltraWebGrid2.Columns(3).Hidden = True
        UltraWebGrid2.Columns(4).Hidden = True
        UltraWebGrid2.Columns(5).Hidden = True
    End Sub

    Protected Sub UltraWebGrid2_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles UltraWebGrid2.InitializeRow
        ' Disable authorized checkboxes for stores that do not have vendors for this item

        ' RefreshPOS.vb:  GetStoreItemRefresh function and SP,  bug 1666
        ' if item is deauth for store or no vendor for the item or item is set in SLIM to be refreshed 
        ' and is wating for the push to be picked up then disable the checkbox.
        ' cells(0) = Store_No 
        ' cells(1) = Store_Name 
        ' cells(2) = Refresh 
        ' cells(3) = StoreItemVendorID 
        ' cells(4) = Authorized 

        If e.Row.Cells(3).Value Is Nothing Or e.Row.Cells(2).Value = True Or e.Row.Cells(4).Value = False Then
            e.Row.Cells(2).AllowEditing = UltraWebGrid.AllowEditing.No
        End If

        If lockAuthJurisdictionIds.Contains(e.Row.GetCellValue(UltraWebGrid2.Columns.FromKey("StoreJurisdictionID"))) Then
            For Each cell As Infragistics.WebUI.UltraWebGrid.UltraGridCell In e.Row.Cells
                cell.AllowEditing = UltraWebGrid.AllowEditing.No
            Next
        Else
            authLockedForAllStores = False
        End If

    End Sub

    Protected Sub UltraWebGrid2_UpdateCell(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.CellEventArgs) Handles UltraWebGrid2.UpdateCell
        If e.Cell.DataChanged = True Then
            e.Cell.AllowEditing = UltraWebGrid.AllowEditing.No

            If Application.Get("RefreshMessageOn") = "True" Then
                lblRefreshMessage.Visible = True
                lblRefreshMessage.Text = Application.Get("RefreshMessage")
            End If
        End If
    End Sub
#End Region

    Protected Sub gvAuthorizations_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvRefresh.RowDataBound
        ' Disable authorized checkboxes for stores that do not have vendors for this item
        If e.Row.RowType = DataControlRowType.DataRow AndAlso gvRefresh.DataKeys(e.Row.RowIndex).Item("StoreItemVendorID").Equals(DBNull.Value) Then
            RefreshCheckbox(e.Row).Enabled = False
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session("AccessLevel") = 3 Then
                Dropdown_Store_List.AppendDataBoundItems = True
                Dropdown_Store_List.Items.Insert(0, New ListItem("All Stores", "0"))
            Else
                Dropdown_Store_List.Enabled = False
                Dropdown_Store_List.SelectedValue = Session("Store_No")
            End If

            If _isMobileDevice Then

                pnlDesktop.Visible = False
                Button_Submit.Visible = False
                pnlMobile.Visible = True

                gvRefresh.Visible = True
                LoadAndBindMobileRefresh()
            End If
        End If

        Label9.Text = Nothing
        Label10.Text = Nothing
        Label3.Text = Request.QueryString("u")
        Label4.Text = Request.QueryString("d")

        If Page.IsPostBack Then
            SqlDataSource3.FilterExpression = "Store_No = {0}"
            SqlDataSource3.FilterParameters.Clear()
            If Dropdown_Store_List.SelectedValue <> 0 Then
                SqlDataSource3.FilterParameters.Add("Store_No", Dropdown_Store_List.SelectedValue)
            Else
                SqlDataSource3.FilterExpression = String.Empty
            End If
        End If
    End Sub

    Protected Sub SqlDataSource3_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SqlDataSource3.Selecting
        Debug.Print("SqlDataSource3_Selecting")

        SqlDataSource3.FilterExpression = "Store_No = {0}"
        SqlDataSource3.FilterParameters.Clear()

        If Dropdown_Store_List.SelectedValue <> 0 Then
            SqlDataSource3.FilterParameters.Add("Store_No", Dropdown_Store_List.SelectedValue)
        Else
            SqlDataSource3.FilterExpression = String.Empty
        End If
    End Sub

    Protected Sub Dropdown_Store_List_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Dropdown_Store_List.SelectedIndexChanged
        If _isMobileDevice Then
            LoadAndBindMobileRefresh()
        End If
    End Sub

    Protected Sub Button_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Submit.Click, btnMobileSubmit.Click
        Dim Item_Key As Integer = Request.QueryString("i")
        Dim rp As New RefreshPOS()

        If _isMobileDevice Then
            UpdateRefreshFromGridView(Item_Key, rp)
        Else
            UpdateRefreshFromUltraGrid(Item_Key, rp)
        End If

        lblRefreshMessage.Visible = True
        lblRefreshMessage.ForeColor = Drawing.Color.Green
    End Sub
End Class
