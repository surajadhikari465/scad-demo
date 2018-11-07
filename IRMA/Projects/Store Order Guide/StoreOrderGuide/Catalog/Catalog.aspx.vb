Imports Microsoft.VisualBasic

Partial Public Class CatalogInterface
    Inherits System.Web.UI.Page
    Dim Common As StoreOrderGuide.Common
    Dim dvCatalogStoreList As DataTableCollection
    Dim dvCatalogItemList As DataTableCollection

#Region "Page Methods"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            '// Stylize DropdownList
            Common.SetDropDownStyle(ddlStoreFilter)
            Common.SetDropDownStyle(ddlSubTeamFilter)
            Common.SetDropDownStyle(ddlZoneFilter)
            Common.SetDropDownStyle(ddlSubTeamSearch)
            Common.SetDropDownStyle(ddlClassSearch)
            Common.SetDropDownStyle(ddlBrandSearch)
            Common.SetDropDownStyle(ddlLevel3Search)

            '// Populate DropdownList
            If Not Page.IsPostBack Then
                Dim Dal As New Dal

                ddlStoreFilter.DataSource = Dal.GetStoreList()
                ddlStoreFilter.DataBind()

                ddlSubTeamFilter.DataSource = Dal.GetSubTeamList(0)
                ddlSubTeamFilter.DataBind()

                ddlZoneFilter.DataSource = Dal.GetZoneList()
                ddlZoneFilter.DataBind()

                ''// ViewState
                If dvCatalogDetail.SelectedValue <= 0 Then
                    PageEditMode(False)
                End If
            End If

            '// ViewState
            If dvCatalogDetail.SelectedValue > 0 Then
                PageEditMode(True)
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub PageEditMode(ByVal EditMode As Boolean)
        Try
            If EditMode = True Then
                '// ViewState
                pnlFilter.Visible = False
                pnlCatalog.Visible = False
                pnlCatalogDetails.Visible = True

                If Not dvCatalogDetail.Rows.Count > 0 Then
                    dvCatalogDetail.DataBind()
                End If

                '// Stylize DropdownList
                Common.SetDropDownStyle(dvCatalogDetail.Rows(1).FindControl("ddlManagedByID"))
                Common.SetDropDownStyle(dvCatalogDetail.Rows(2).FindControl("ddlSubTeamID"))

                If CType(dvCatalogDetail.Rows(7).Cells(1).Controls(0), CheckBox).Checked = False Then
                    pnlCatalogItem.Visible = True
                    pnlCatalogStore.Visible = True

                    '// Populate ItemSearch DropdownLists
                    If Not ddlSubTeamSearch.SelectedIndex > 0 Then
                        ddlSubTeamSearch.DataSource = Dal.GetSubTeamList(0)
                        ddlSubTeamSearch.DataBind()
                    End If

                    If Not ddlBrandSearch.SelectedIndex > 0 Then
                        ddlBrandSearch.DataSource = Dal.GetBrandList(0)
                        ddlBrandSearch.DataBind()
                    End If

                    If Not ddlSubTeamSearch_Cat.SelectedIndex > 0 Then
                        ddlSubTeamSearch_Cat.DataSource = Dal.GetSubTeamList(0)
                        ddlSubTeamSearch_Cat.DataBind()
                    End If

                    If Not ddlBrandSearch_Cat.SelectedIndex > 0 Then
                        ddlBrandSearch_Cat.DataSource = Dal.GetBrandList(0)
                        ddlBrandSearch_Cat.DataBind()
                    End If
                Else
                    '// ViewState
                    pnlCatalogItem.Visible = False
                    pnlCatalogStore.Visible = False
                    Common.MessageToUser(Page, New Exception("Catalog must be unpublished to edit Store and Item data."), False)
                End If
            Else
                '// ViewState
                pnlFilter.Visible = True
                pnlCatalog.Visible = True
                pnlCatalogDetails.Visible = False
                pnlCatalogItem.Visible = False
                pnlCatalogStore.Visible = False

                '// Clear DataSource
                dsCatalog.SelectParameters("CatalogID").DefaultValue = 0

                '// Clear Grid
                gvCatalogList.SelectedIndex = -1
                gvCatalogList.DataBind()
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub ddlSubTeamSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlSubTeamSearch.SelectedIndexChanged
        Try
            ddlClassSearch.DataSource = Dal.GetClassList(0, ddlSubTeamSearch.SelectedValue)
            ddlClassSearch.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub ddlClassSearch_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlClassSearch.SelectedIndexChanged
        Try
            ddlLevel3Search.DataSource = Dal.GetLevel3List(0, ddlClassSearch.SelectedValue)
            ddlLevel3Search.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub btnAddCatalog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCatalog.Click
        Try
            pnlCatalogDetails.Visible = True
            pnlFilter.Visible = False
            pnlCatalog.Visible = False

            dvCatalogDetail.ChangeMode(DetailsViewMode.Insert)

            Dim txtCatalogID As TextBox = CType(dvCatalogDetail.Rows(0).Cells(1).Controls(0), TextBox)
            txtCatalogID.Visible = False

            Common.SetDropDownStyle(dvCatalogDetail.Rows(1).FindControl("ddlManagedByID"))
            Common.SetDropDownStyle(dvCatalogDetail.Rows(2).FindControl("ddlSubTeamID"))
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            'dsItems.Select()
            gvItemList.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Try
            PageEditMode(False)
            If chkPublishedFilter.Checked = False Then
                btnPublishAll.Text = "Publish All"
            Else
                btnPublishAll.Text = "Unpublish All"
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Dim sReportURL As New System.Text.StringBuilder
            Dim sReportServer As String
            sReportServer = HttpContext.Current.Application("reportingServicesURL")

            sReportURL.Append("SOG_PrintCatalogs")

            sReportURL.Append("&rs:Command=Render")
            sReportURL.Append("&rc:Parameters=False")

            Response.Redirect(sReportServer + sReportURL.ToString())
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    'Sub dsItems_Inserted(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs) Handles dsItems.Inserted
    '    Try
    '        gvCatalogItemList.DataBind()
    '    Catch ex As Exception
    '        Common.LogError(ex)
    '        Throw New Exception(ex.Message, ex.InnerException)
    '    End Try
    'End Sub

    'Sub dsStores_Inserted(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs) Handles dsStores.Inserted
    '    Try
    '        gvCatalogStoreList.DataBind()
    '    Catch ex As Exception
    '        Common.LogError(ex)
    '        Throw New Exception(ex.Message, ex.InnerException)
    '    End Try
    'End Sub

#End Region

#Region "Catalog Grid Methods"
    Sub gvCatalogList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCatalogList.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim db As LinkButton = CType(e.Row.Cells(2).Controls(0), LinkButton)

                If e.Row.Cells(11).Text = "True" Then
                    db.Enabled = False
                Else
                    db.Enabled = True
                    db.OnClientClick = "return confirm('Are you certain you want to delete the Catalog?')"
                End If
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub gvCatalogList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gvCatalogList.SelectedIndexChanged
        Try
            dsCatalog.SelectParameters("CatalogID").DefaultValue = gvCatalogList.SelectedValue

            PageEditMode(True)

            dvCatalogDetail.ChangeMode(DetailsViewMode.Edit)
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub gvCatalogList_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCatalogList.PageIndexChanged
        Try
            PageEditMode(False)
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub gvCatalogList_Sorted(ByVal sender As Object, ByVal a As EventArgs) Handles gvCatalogList.Sorted
        Try
            PageEditMode(False)
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub gvCatalogList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCatalogList.RowCommand
        Try
            If e.CommandName = "btnCopy" Then
                dsCatalog.InsertParameters("Copy").DefaultValue = True
                dsCatalog.InsertParameters("CatalogID").DefaultValue = gvCatalogList.Rows(Val(e.CommandArgument)).Cells(5).Text

                dsCatalog.Insert()
            End If

            If e.CommandName = "btnPrint" Then
                Dim sReportURL As New System.Text.StringBuilder
                Dim sReportServer As String
                sReportServer = HttpContext.Current.Application("reportingServicesURL")

                sReportURL.Append("SOG_PrintCatalog")

                sReportURL.Append("&rs:Command=Render")
                sReportURL.Append("&rc:Parameters=False")

                sReportURL.Append("&CatalogID=" & gvCatalogList.Rows(Val(e.CommandArgument)).Cells(5).Text)

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

    Sub dvCatalogDetail_ItemUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdateEventArgs) Handles dvCatalogDetail.ItemUpdating
        Try
            If e.NewValues.Contains("Published").ToString And gvCatalogStoreList.Rows.Count < 1 Then
                Common.MessageToUser(Page, New Exception("Catalog cannot be Published without at least one Store authorized."), False)

                Dim chkPublished As CheckBox = CType(dvCatalogDetail.Rows(7).Cells(1).Controls(0), CheckBox)
                chkPublished.Checked = False

                e.Cancel = True
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub dvCatalogDetail_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewUpdatedEventArgs) Handles dvCatalogDetail.ItemUpdated
        Try
            PageEditMode(False)
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub dvCatalogDetail_ItemInserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewInsertedEventArgs) Handles dvCatalogDetail.ItemInserted
        Try
            PageEditMode(False)
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub dvCatalogDetail_ModeChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewModeEventArgs) Handles dvCatalogDetail.ModeChanging
        Try
            PageEditMode(False)

            dvCatalogDetail.ChangeMode(e.NewMode)
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub
#End Region

#Region "Catalog Item Methods"

    'Sub gvItemList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvItemList.RowDataBound
    '    Try
    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            Dim db As LinkButton = CType(e.Row.Cells(0).Controls(0), LinkButton)

    '            db.Text = "Add"
    '        End If
    '    Catch ex As Exception
    '        Common.LogError(ex)
    '        Throw New Exception(ex.Message, ex.InnerException)
    '    End Try
    'End Sub

    'Sub gvCatalogItemList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCatalogItemList.RowDataBound
    '    Try
    '        If e.Row.RowType = DataControlRowType.DataRow And e.Row.RowState < 4 Then
    '            Dim db As LinkButton = CType(e.Row.Cells(1).Controls(0), LinkButton)

    '            db.OnClientClick = "return confirm('Are you certain you want to delete the CatalogItem?')"
    '        End If
    '    Catch ex As Exception
    '        Common.LogError(ex)
    '        Throw New Exception(ex.Message, ex.InnerException)
    '    End Try
    'End Sub

    'Sub gvItemList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvItemList.RowCommand
    '    Try
    '        If e.CommandName = "New" Then
    '            Dim ItemKey As Integer = gvItemList.Rows(e.CommandArgument).Cells(1).Text

    '            For Each CatalogItemRow As GridViewRow In gvCatalogItemList.Rows
    '                If CatalogItemRow.Cells(3).Text = ItemKey.ToString() Then
    '                    Common.MessageToUser(Page, New Exception("Item already exists in Catalog."), False)
    '                    Exit Sub
    '                End If
    '            Next

    '            dsItems.InsertParameters("ItemKey").DefaultValue = ItemKey

    '            dsItems.Insert()
    '        End If
    '    Catch ex As Exception
    '        Common.LogError(ex)
    '        Throw New Exception(ex.Message, ex.InnerException)
    '    End Try
    'End Sub



    Private Sub gvCatalogItemList_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCatalogItemList.DataBound
        Dim dal As New Dal
        Dim stClass, stLevel3 As String

        stClass = ddlClassSearch.SelectedValue
        stLevel3 = ddlLevel3Search.SelectedValue

        If ddlClassSearch.SelectedValue = "" Then
            stClass = "0"
        End If
        If ddlLevel3Search.SelectedValue = "" Then
            stLevel3 = "0"
        End If
        dvCatalogItemList = dal.GetCatalogItems(dvCatalogDetail.SelectedValue, ddlStoreFilter.SelectedValue, False, txtIdentifierSearch.Text, txtItemDescriptionSearch.Text, ddlSubTeamSearch.SelectedValue, stClass, stLevel3, ddlBrandSearch.SelectedValue).Tables

        Session("CatalogItemList") = dvCatalogItemList
    End Sub

#End Region

#Region "Catalog Store Methods"

    Private Sub gvCatalogStoreList_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCatalogStoreList.DataBound
        Dim dal As New Dal

        dvCatalogStoreList = dal.GetCatalogStores(dvCatalogDetail.SelectedValue).Tables
        Session("CatalogStoreList") = dvCatalogStoreList
    End Sub

    'Sub gvStoreList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvStoreList.RowDataBound
    '    Try
    '        If e.Row.RowType = DataControlRowType.DataRow And e.Row.RowState < 4 Then
    '            Dim db As LinkButton = CType(e.Row.Cells(0).Controls(0), LinkButton)

    '            db.Text = "Add"
    '        End If
    '    Catch ex As Exception
    '        Common.LogError(ex)
    '        Throw New Exception(ex.Message, ex.InnerException)
    '    End Try
    'End Sub

    'Sub gvCatalogStoreList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCatalogStoreList.RowDataBound
    '    Try
    '        If e.Row.RowType = DataControlRowType.DataRow And e.Row.RowState < 4 Then
    '            Dim db As LinkButton = CType(e.Row.Cells(0).Controls(0), LinkButton)

    '            db.OnClientClick = "return confirm('Are you certain you want to delete the CatalogStore?')"
    '        End If
    '    Catch ex As Exception
    '        Common.LogError(ex)
    '        Throw New Exception(ex.Message, ex.InnerException)
    '    End Try
    'End Sub

    'Sub gvStoreList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvStoreList.RowCommand
    '    Try
    '        If e.CommandName = "New" Then
    '            Dim StoreNo As Integer = gvStoreList.Rows(e.CommandArgument).Cells(1).Text

    '            For Each CatalogStoreRow As GridViewRow In gvCatalogStoreList.Rows
    '                If CatalogStoreRow.Cells(2).Text = StoreNo.ToString() Then
    '                    Common.MessageToUser(Page, New Exception("Store already authorized for Catalog."), False)
    '                    Exit Sub
    '                End If
    '            Next

    '            dsStores.InsertParameters("StoreID").DefaultValue = StoreNo

    '            dsStores.Insert()
    '        End If
    '    Catch ex As Exception
    '        Common.LogError(ex)
    '        Throw New Exception(ex.Message, ex.InnerException)
    '    End Try
    'End Sub
#End Region

    Public Sub ItemAddChecked(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checked As Boolean = sender.checked
        Dim gvr As GridViewRow = sender.parent.parent

        If checked Then
            Dim ItemKey As Integer = gvItemList.Rows(gvr.RowIndex).Cells(1).Text

            For Each CatalogItemRow As GridViewRow In gvCatalogItemList.Rows
                If CatalogItemRow.Cells(3).Text = ItemKey.ToString() Then
                    Common.MessageToUser(Page, New Exception("Item already exists in Catalog."), False)
                    Exit Sub
                End If
            Next

            dsItems.InsertParameters("ItemKey").DefaultValue = ItemKey

            dsItems.Insert()
        End If

    End Sub

    Public Sub ItemDeleteChecked(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checked As Boolean = sender.checked
        Dim gvr As GridViewRow = sender.parent.parent

        If checked Then
            dsCatalogItems.DeleteParameters("CatalogItemID").DefaultValue = gvCatalogItemList.DataKeys(gvr.RowIndex).Value
            dsCatalogItems.Delete()
        End If

    End Sub

    Public Sub DeleteStoreChecked(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checked As Boolean = sender.checked
        Dim gvr As GridViewRow = sender.parent.parent
        If checked Then
            Dim StoreNo As Integer = Integer.Parse(gvCatalogStoreList.DataKeys(gvr.RowIndex).Value)

            dsCatalogStores.DeleteParameters("CatalogStoreID").DefaultValue = StoreNo

            dsCatalogStores.Delete()
        End If
    End Sub

    Public Sub StoreChecked(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim checked As Boolean = sender.checked
        Dim gvr As GridViewRow = sender.parent.parent
        dvCatalogStoreList = Session("CatalogStoreList")

        If checked Then
            Dim StoreNo As Integer = gvStoreList.Rows(gvr.RowIndex).Cells(1).Text

            For Each CatalogStoreRow As DataRow In dvCatalogStoreList.Item(0).Rows
                If CatalogStoreRow.Item("StoreNo").ToString = StoreNo.ToString() Then
                    Common.MessageToUser(Page, New Exception("Store " + StoreNo.ToString() + " already authorized for Catalog."), False)
                    Exit Sub
                End If
            Next

            dsStores.InsertParameters("StoreID").DefaultValue = StoreNo

            dsStores.Insert()
        End If

    End Sub

    Private Sub btnSearch_Cat_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch_Cat.Click
        Try
            'dsCatalogItems.Select()
            gvCatalogItemList.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Private Sub ddlSubTeamSearch_Cat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubTeamSearch_Cat.SelectedIndexChanged
        Try
            ddlClassSearch_Cat.DataSource = Dal.GetClassList(0, ddlSubTeamSearch_Cat.SelectedValue)
            ddlClassSearch_Cat.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Private Sub ddlClassSearch_Cat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlClassSearch_Cat.SelectedIndexChanged
        Try
            ddlLevel3Search_Cat.DataSource = Dal.GetLevel3List(0, ddlClassSearch_Cat.SelectedValue)
            ddlLevel3Search_Cat.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Private Sub btnAddStores_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddStores.Click
        gvStoreList.DataBind()
        gvCatalogStoreList.DataBind()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        gvStoreList.DataBind()
        gvCatalogStoreList.DataBind()
    End Sub

    Private Sub btnAddItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddItem.Click
        gvCatalogItemList.DataBind()
        gvItemList.DataBind()
    End Sub

    Private Sub btnDeleteItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteItem.Click
        gvCatalogItemList.DataBind()
        gvItemList.DataBind()
    End Sub

    Protected Sub btnPublishAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPublishAll.Click
        If Page.IsValid Then
            Try
                Dim sbCatalogIDs As New StringBuilder
                Dim strCatalogIDs As String

                For Each row As GridViewRow In gvCatalogList.Rows
                    Dim chkToBeMassPublished As CheckBox = row.Cells(4).FindControl("chkMasskPublish")

                    If Not IsNothing(chkToBeMassPublished) Then
                        If chkToBeMassPublished.Checked Then
                            sbCatalogIDs.Append(row.Cells(5).Text + "|")
                        End If
                    End If
                Next

                If IsNothing(sbCatalogIDs) Then
                    Common.MessageToUser(Page, New Exception("No catalog is selecteed for mass update."), False)
                Else
                    strCatalogIDs = sbCatalogIDs.ToString()

                    dsMassPublishCatalogs.UpdateParameters("CatalogIDs").DefaultValue = strCatalogIDs
                    If btnPublishAll.Text = "Unpublish All" Then
                        dsMassPublishCatalogs.UpdateParameters("Published").DefaultValue = False
                    Else
                        dsMassPublishCatalogs.UpdateParameters("Published").DefaultValue = True
                    End If

                    If dsMassPublishCatalogs.Update() = True Then
                        gvCatalogList.DataBind()
                    End If
                End If
            Catch ex As Exception
                Common.LogError(ex)
                Throw
            End Try
        End If
    End Sub
End Class