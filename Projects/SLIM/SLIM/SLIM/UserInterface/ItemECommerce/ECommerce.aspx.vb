Imports Infragistics.WebUI

Partial Class UserInterface_ItemECommerce_ECommerce
    Inherits ItemECommercePage

    Private lockAuthTable As New DataTable
    Private lockAuthJurisdictionDescriptions As New List(Of String)
    Private lockAuthJurisdictionIds As New List(Of Integer)
    Private authLockedForAllStores As Boolean = True

#Region "Helper Methods"

    Private Function ECommerceCheckbox(ByVal row As GridViewRow) As CheckBox
        Return CType(row.Cells(1).FindControl("cbAuthorized"), CheckBox)
    End Function

    Private Function GetDataSource(ByRef Item_Key As Integer, ByRef Store_No As Integer) As DataTable
        Dim ia As New ItemAuth()
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
            Label_Error.Visible = True
            Label_Error.ForeColor = Drawing.Color.Red
            Label_Error.Text = String.Format("Authorization is locked for the following jurisdiction(s): {0}", lockedJurisdictions)
        End If

        Return ia.GetStoreItemECommerce(Item_Key, Store_No)
    End Function

    Private Sub LoadAndBindMobileAuthorizations()
        Dim itemKey As Integer
        Dim storeNo As Integer
        gvAuthorizations.DataSource = GetDataSource(itemKey, storeNo)
        gvAuthorizations.DataBind()
    End Sub

    Private Sub UpdateStoreItem(ByVal Item_Key As Integer, ByVal ia As ItemAuth, ByVal Store_No As Integer, ByVal Authorized As Boolean)
        Try
            'Call Stored Proc and pass in store_No, Item_Key, and CheckVal
            ia.UpdateStoreItem(Item_Key, Store_No, Authorized)
        Catch ex As Exception
            Label_Error.Visible = True
            Label_Error.ForeColor = Drawing.Color.Red
            Label_Error.Text = "An error has occured when attempting to update the item."
        End Try
    End Sub

    Private Sub UpdateStoreItemECommerce(ByVal Item_Key As Integer, ByVal ia As ItemAuth, ByVal Store_No As Integer, ByVal Ecommerce As Boolean)
        Try
            'Call Stored Proc and pass in store_No, Item_Key, and CheckVal
            ia.UpdateStoreItemECommerce(Item_Key, Store_No, Ecommerce)
        Catch ex As Exception
            Label_Error.Visible = True
            Label_Error.ForeColor = Drawing.Color.Red
            Label_Error.Text = "An error has occured when attempting to update the item."
        End Try
    End Sub

    Private Sub UpdateAuthorizationsFromGridView(ByVal Item_Key As Integer, ByVal ia As ItemAuth)
        For Each row As GridViewRow In gvAuthorizations.Rows
            UpdateStoreItem(Item_Key, ia, gvAuthorizations.DataKeys(row.RowIndex).Value, ECommerceCheckbox(row).Checked)
        Next
    End Sub

    Private Sub UpdateAuthorizationsFromUltraGrid(ByVal Item_Key As Integer, ByVal ia As ItemAuth)
        For Each iRow As UltraWebGrid.UltraGridRow In UltraWebGrid2.Rows
            If iRow.Cells.FromKey("ECommerce").IsEditable Then
                UpdateStoreItemECommerce(Item_Key, ia, iRow.Cells.FromKey("Store_No").Value, iRow.Cells.FromKey("ECommerce").Value)
            End If
        Next
    End Sub

#End Region

#Region "UltraWebGrid Event Handlers"

    Protected Sub UltraWebGrid1_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles UltraWebGrid1.InitializeLayout
        Dim backColor As System.Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#004000")
        Dim AltColor As System.Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#C0FFC0")

        UltraWebGrid1.Font.Size = 12
        UltraWebGrid1.Font.Name = "Tahoma"

        With UltraWebGrid1.DisplayLayout
            .Pager.AllowPaging = True
            .Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.PrevNext
            .Pager.PageSize = 50
            .Bands(0).Columns(0).CellStyle.HorizontalAlign = HorizontalAlign.Center
            .Bands(0).AllowAdd = UltraWebGrid.AllowAddNew.No
            .Bands(0).AllowDelete = UltraWebGrid.AllowDelete.No
            .Bands(0).AllowUpdate = UltraWebGrid.AllowUpdate.No
            .ColWidthDefault = Nothing
            .HeaderStyleDefault.BackColor = backColor
            .HeaderStyleDefault.ForeColor = Drawing.Color.White
            .RowAlternateStyleDefault.BackColor = AltColor
        End With
    End Sub

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
        Dim authLockedForAllStores As Boolean = True

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

    End Sub

    Protected Sub UltraWebGrid2_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles UltraWebGrid2.InitializeRow
        ' Disable ECommerce checkboxes for stores that do not have vendors for this item
        If e.Row.Cells(3).Value Is Nothing Then
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

#End Region

    Protected Sub gvAuthorizations_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvAuthorizations.RowDataBound
        ' Disable authorized checkboxes for stores that do not have vendors for this item
        If e.Row.RowType = DataControlRowType.DataRow AndAlso gvAuthorizations.DataKeys(e.Row.RowIndex).Item("StoreItemVendorID").Equals(DBNull.Value) Then
            ECommerceCheckbox(e.Row).Enabled = False
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
                ''pnlDesktop.Visible = False
                ''Button_Submit.Visible = False
                ''pnlMobile.Visible = True

                ''gvAuthorizations.Visible = True
                ''LoadAndBindMobileAuthorizations()
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
            'LoadAndBindMobileAuthorizations()
        End If
    End Sub

    Protected Sub Button_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Submit.Click, btnMobileSubmit.Click
        Dim Item_Key As Integer = Request.QueryString("i")
        Dim ia As New ItemAuth()

        If _isMobileDevice Then
            'UpdateAuthorizationsFromGridView(Item_Key, ia)
        Else
            UpdateAuthorizationsFromUltraGrid(Item_Key, ia)
        End If

        '********** Send E-Mail Notification ---  --- ******************
        Try
            If Application.Get("ItemAuthorizationEmail") = "1" Then
                Dim em As New EmailNotifications
                em.EmailType = "ECommerce"
                em.Identifier = Request.QueryString("u")
                em.ItemDescription = Request.QueryString("d")
                em.Store_Name = "Regional Office"
                em.Store_No = Session("Store_No")
                em.User_ID = Session("UserID")
                em.Reques_tor = Session("UserName")
                em.SentEmail()
            End If
            ' ***********************************************************

            Label_Error.Text = "ECommerce updates submitted successfully!"
            Label_Error.ForeColor = Drawing.Color.Green
            Label_Error.Visible = True
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub
End Class