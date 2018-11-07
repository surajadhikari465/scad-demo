Imports Microsoft.VisualBasic

Partial Public Class AdminSettingsInterface
    Inherits System.Web.UI.Page
    Dim Common As StoreOrderGuide.Common

#Region "Page Methods"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

        Catch ex As Exception

        End Try
    End Sub

    Sub btnAddAdminSetting_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAdminSetting.Click
        Try
            pnlAdminSettingDetails.Visible = True
            pnlAdminSettingList.Visible = False

            dvAdminSettingDetail.ChangeMode(DetailsViewMode.Insert)

            Dim txtCatalogID As TextBox = CType(dvAdminSettingDetail.Rows(0).Cells(1).Controls(0), TextBox)
            txtCatalogID.Visible = False
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Sub dsAdminSettings_Inserted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs) Handles dsAdminSettings.Inserted
        Try
            pnlAdminSettingDetails.Visible = False
            pnlAdminSettingList.Visible = True

            gvAdminSettingsList.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub
#End Region

#Region "AdminSetting Grid Methods"
    Sub gvAdminSettingsList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAdminSettingsList.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow And e.Row.RowState < 4 Then
                Dim db As LinkButton = CType(e.Row.Cells(1).Controls(0), LinkButton)

                db.OnClientClick = "return confirm('Are you certain you want to delete the AdminSetting?')"
            End If
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub
#End Region

#Region "AdminSetting Grid Methods"
    Sub dvAdminSettingDetail_ModeChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DetailsViewModeEventArgs) Handles dvAdminSettingDetail.ModeChanging
        Try
            dvAdminSettingDetail.ChangeMode(e.NewMode)

            pnlAdminSettingDetails.Visible = False
            pnlAdminSettingList.Visible = True

            gvAdminSettingsList.DataBind()
        Catch ex As Exception
            Common.LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub
#End Region
End Class