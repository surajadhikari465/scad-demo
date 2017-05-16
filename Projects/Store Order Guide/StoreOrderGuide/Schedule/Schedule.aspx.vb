Imports Microsoft.VisualBasic

Partial Public Class ScheduleInterface
    Inherits System.Web.UI.Page
    Dim Common As StoreOrderGuide.Common

#Region "Page Methods"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Common.SetDropDownStyle(ddlSubTeamFilter)
            Common.SetDropDownStyle(ddlStoreFilter)
            Common.SetDropDownStyle(ddlManagedByFilter)

            '// Populate DropdownList
            If Not Page.IsPostBack Then
                Dim Dal As New Dal

                ddlStoreFilter.DataSource = Dal.GetStoreList()
                ddlStoreFilter.DataBind()

                ddlSubTeamFilter.DataSource = Dal.GetSubTeamList(0)
                ddlSubTeamFilter.DataBind()

                ddlManagedByFilter.DataSource = Dal.GetManagedByList()
                ddlManagedByFilter.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "DataSource Methods"
    Sub dsCatalogSchedule_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles dsCatalogSchedule.Inserted
        Try
            '// ViewState
            pnlCatalogScheduleDetails.Visible = False
            pnlCatalogScheduleList.Visible = True

            '// Rebind grid
            gvCatalogSchedule.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub
#End Region

#Region "Control Methods"
    Sub btnAddCatalogSchedule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCatalogSchedule.Click
        Try
            '// ViewState
            pnlCatalogScheduleDetails.Visible = True
            pnlCatalogScheduleList.Visible = False

            dvCatalogSchedule.ChangeMode(DetailsViewMode.Insert)

            Dim txtCatalogID As TextBox = CType(dvCatalogSchedule.Rows(0).Cells(1).Controls(0), TextBox)
            txtCatalogID.Visible = False

            '// Stylize Drop downs
            Common.SetDropDownStyle(dvCatalogSchedule.Rows(1).FindControl("ddlManagerID"))
            Common.SetDropDownStyle(dvCatalogSchedule.Rows(2).FindControl("ddlStoreNo"))
            Common.SetDropDownStyle(dvCatalogSchedule.Rows(3).FindControl("ddlSubTeamNo"))
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Protected Sub ddlManagedByFilter_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlManagedByFilter.DataBound
        Dim oListItemDefault As New System.Web.UI.WebControls.ListItem("All Facilities", CType(0, String))

        ddlManagedByFilter.Items.Insert(0, oListItemDefault)
    End Sub
#End Region

#Region "CatalogSchedule Grid Methods"
    Sub gvCatalogSchedule_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCatalogSchedule.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow And e.Row.RowState < 4 Then
                Dim db As LinkButton = CType(e.Row.Cells(1).Controls(0), LinkButton)

                db.OnClientClick = "return confirm('Are you certain you want to delete the CatalogSchedule?')"
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub
#End Region

#Region "CatalogSchedule Detail Methods"
    Sub dvCatalogSchedule_ModeChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewModeEventArgs) Handles dvCatalogSchedule.ModeChanging
        Try
            dvCatalogSchedule.ChangeMode(e.NewMode)

            '// ViewState
            pnlCatalogScheduleDetails.Visible = False
            pnlCatalogScheduleList.Visible = True

            gvCatalogSchedule.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub
#End Region
End Class