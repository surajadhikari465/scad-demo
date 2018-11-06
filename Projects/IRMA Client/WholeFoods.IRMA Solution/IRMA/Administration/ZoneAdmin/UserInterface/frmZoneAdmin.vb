Imports log4net
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class ZoneAdmin

    ' ---------------------------------------------------------------------------
    ' Revision History
    ' ---------------------------------------------------------------------------
    ' 8/19/2010             Tom Lux               TFS 13258        Added logger and logging to help track was happens in this form.


#Region "Private Members"

    ''' <summary>
    ''' Log4Net logger for this class.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    '  Private mZones As Zones

#End Region

    Private Sub frmZoneAdmin_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text = "Manage Zones"

        ugrdZoneList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single
        PopulateGrid()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim frm As New ZoneAdd

        frm.ShowDialog()
        frm.Close()
        frm.Dispose()

        PopulateGrid()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If ugrdZoneList.Selected.Rows.Count > 0 Then
            ugrdZoneList.ActiveRow.Selected = True

            If MsgBox("Remove the selected Zone?", MsgBoxStyle.OkCancel, "Delete Zone") = MsgBoxResult.Ok Then
                ZoneDAO.DeleteZone(ugrdZoneList.Selected.Rows(0).Cells(0).Value)
                PopulateGrid()
            End If
        Else
            MsgBox("You must select a zone to delete", MsgBoxStyle.Information, "Delete Zone")
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub ugrdZoneList_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles ugrdZoneList.AfterRowUpdate

        ZoneDAO.UpdateZone(e.Row.Cells("Zone_Id").Value, e.Row.Cells("Zone_Name").Value, e.Row.Cells("GLMarketingExpenseAcct").Value, e.Row.Cells("Region_Id").Value, giUserID)
        PopulateGrid()
    End Sub

    Private Sub PopulateGrid()
        ugrdZoneList.DataSource = ZoneDAO.GetZones

        ugrdZoneList.DisplayLayout.Bands(0).Columns("Zone_Id").Hidden = True
        ugrdZoneList.DisplayLayout.Bands(0).Columns("Region_Id").Hidden = True
        ugrdZoneList.DisplayLayout.Bands(0).Columns("LastUpdate").Hidden = True
        ugrdZoneList.DisplayLayout.Bands(0).Columns("StoreCount").Hidden = True
    End Sub
End Class
