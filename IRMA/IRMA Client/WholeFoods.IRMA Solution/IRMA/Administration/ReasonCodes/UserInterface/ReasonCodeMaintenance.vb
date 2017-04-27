
Imports log4net
Imports System.Configuration
Imports WholeFoods.Utility
Imports System.Data
Imports System.Text
Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.Administration.ReasonCodes.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic

Public Class ReasonCodeMaintenance

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub New()

        InitializeComponent()

    End Sub

    Private Sub ReasonCodeMaintenance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CenterForm(Me)
        LoadData()

    End Sub
    Private Sub LoadData()

        logger.Debug("ReasonCodeMaintenance.LoadData entry")

        Dim bo As ReasonCodeMaintenanceBO = New ReasonCodeMaintenanceBO
        Dim dt As DataTable = bo.getReasonCodeType

        Dim dr As DataRow = dt.NewRow()
        dr("ReasonCodeTypeAbbr") = -1
        dr("ReasonCodeTypeDesc") = ""
        dt.Rows.InsertAt(dr, 0)

        cmbReasonCodeTypes.DataSource = dt
        cmbReasonCodeTypes.DisplayMember = "ReasonCodeTypeDesc"
        cmbReasonCodeTypes.ValueMember = "ReasonCodeTypeAbbr"

        logger.Debug("ReasonCodeMaintenance.LoadData exit")

    End Sub

    Private Sub cmbReasonCodeTypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbReasonCodeTypes.SelectedIndexChanged

        logger.Debug("ReasonCodeMaintenance.cmbReasonCodeTypes_SelectedIndexChanged exit")

        loadDetails()

        logger.Debug("ReasonCodeMaintenance.cmbReasonCodeTypes_SelectedIndexChanged exit")

    End Sub
    Private Sub loadDetails()

        logger.Debug("ReasonCodeMaintenance.loadDetails entry")

        Dim bo As ReasonCodeMaintenanceBO = New ReasonCodeMaintenanceBO
        Dim dt As DataTable = bo.getReasonCodeDetailsForType(cmbReasonCodeTypes.SelectedValue.ToString)

        UltraGrid_ReasonCodeMaintenance.DataSource = dt
        UltraGrid_ReasonCodeMaintenance.DataBind()

        logger.Debug("ReasonCodeMaintenance.loadDetails exit")

    End Sub

    Private Sub formatData()

        logger.Debug("ReasonCodeMaintenance.formatData entry")

        With UltraGrid_ReasonCodeMaintenance

            .DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn
            .DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False
            .DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect

            .Selected.Rows.Clear()

            .DisplayLayout.Override.ActiveRowAppearance.Reset()
            .DisplayLayout.Appearance.FontData.SizeInPoints = 9
            .DisplayLayout.Appearance.FontData.Name = "Calibri"

        End With

        UltraGrid_ReasonCodeMaintenance.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns

        logger.Debug("ReasonCodeMaintenance.formatData exit")

    End Sub

    Private Sub AddReasonCodeType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddReasonCodeType.Click

        logger.Debug("ReasonCodeMaintenance.AddReasonCodeType_Click entry")

        ReasonCodeTypeAdd.ShowDialog()
        ReasonCodeTypeAdd.Dispose()

        LoadData()

        logger.Debug("ReasonCodeMaintenance.AddReasonCodeType_Click exit")

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        logger.Debug("ReasonCodeMaintenance.cmdAdd_Click entry")

        ReasonCodeMappings.ShowDialog()
        ReasonCodeMappings.Dispose()

        logger.Debug("ReasonCodeMaintenance.cmdAdd_Click exit")

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        logger.Debug("ReasonCodeMaintenance.cmdExit_Click Entry")

        Me.Close()

        logger.Debug("ReasonCodeMaintenance.cmdExit_Click Exit")

    End Sub

    Private Sub UltraGrid_ReasonCodeMaintenance_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles UltraGrid_ReasonCodeMaintenance.AfterCellUpdate

        logger.Debug("ReasonCodeMaintenance.UltraGrid_ReasonCodeMaintenance_AfterCellUpdate Entry")

        If MsgBox("Are you sure you want to update the Reason Code Description for '" + UltraGrid_ReasonCodeMaintenance.ActiveRow.Cells("ReasonCode").Value + "'?", MsgBoxStyle.OkCancel, "Update Description?") = MsgBoxResult.Ok Then

            Dim ReasonCode, ReasonCodeDesc, ReasonCodeExtDesc As String

            ReasonCode = UltraGrid_ReasonCodeMaintenance.ActiveRow.Cells("ReasonCode").Value
            ReasonCodeDesc = UltraGrid_ReasonCodeMaintenance.ActiveRow.Cells("ReasonCodeDesc").Value
            ReasonCodeExtDesc = UltraGrid_ReasonCodeMaintenance.ActiveRow.Cells("ReasonCodeExtDesc").Value

            Dim bo As ReasonCodeMaintenanceBO = New ReasonCodeMaintenanceBO
            bo.updateReasonCodeDetail(ReasonCode, ReasonCodeDesc, ReasonCodeExtDesc)
        Else
            loadDetails()
        End If

        logger.Debug("ReasonCodeMaintenance.UltraGrid_ReasonCodeMaintenance_AfterCellUpdate Exit")

    End Sub

    Private Sub AddReasonCodeDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddReasonCodeDetail.Click

        logger.Debug("ReasonCodeMaintenance.AddReasonCodeDetail_Click Entry")

        ReasonCodeDetailAdd.ShowDialog()
        ReasonCodeDetailAdd.Dispose()

        logger.Debug("ReasonCodeMaintenance.AddReasonCodeDetail_Click Exit")

    End Sub

    Private Sub EditMappings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditMappings.Click

        logger.Debug("ReasonCodeMaintenance.cmdAdd_Click entry")

        ReasonCodeMappings.ShowDialog()
        ReasonCodeMappings.Dispose()
        loadDetails()

        logger.Debug("ReasonCodeMaintenance.cmdAdd_Click exit")

    End Sub
End Class