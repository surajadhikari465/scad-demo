Imports log4net
Imports System.Configuration
Imports WholeFoods.Utility
Imports System.Data
Imports System.Text
Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.Administration.ReasonCodes.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic

Public Class ReasonCodeMappings
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub New()

        InitializeComponent()

    End Sub

    Private Sub ReasonCodeMappings_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CenterForm(Me)
        LoadData()

    End Sub
    Private Sub LoadData()

        logger.Debug("ReasonCodeMappings.LoadData entry")

        Dim bo As ReasonCodeMaintenanceBO = New ReasonCodeMaintenanceBO

        Dim dtType As DataTable = bo.getReasonCodeType

        Dim drType As DataRow = dtType.NewRow()
        drType("ReasonCodeTypeDesc") = ""
        drType("ReasonCodeTypeAbbr") = -1
        dtType.Rows.InsertAt(drType, 0)

        cmbType.DataSource = dtType
        cmbType.DisplayMember = "ReasonCodeTypeDesc"
        cmbType.ValueMember = "ReasonCodeTypeAbbr"

        Dim dtDetail As DataTable = bo.getReasonCodeDetail

        Dim drDetail As DataRow = dtDetail.NewRow()
        drDetail("ReasonCodeDesc") = ""
        drDetail("ReasonCode") = -1
        dtDetail.Rows.InsertAt(drDetail, 0)

        cmbDescription.DataSource = dtDetail
        cmbDescription.DisplayMember = "ReasonCodeDesc"
        cmbDescription.ValueMember = "ReasonCode"

        logger.Debug("ReasonCodeMappings.LoadData exit")

    End Sub


    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        logger.Debug("ReasonCodeMappings.cmdExit_Click Entry")
        Me.Close()
        logger.Debug("ReasonCodeMappings.cmdExit_Click Exit")
    End Sub

    Private Sub cmdDisableMapping_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDisableMapping.Click
        logger.Debug("ReasonCodeMappings.cmdAddMapping_Click Entry")

        If (cmbType.SelectedValue.ToString = "-1" Or cmbDescription.SelectedValue.ToString = "-1") Then
            MsgBox("Please select a Type and Detail record!", MsgBoxStyle.Exclamation, Me.Text)
        Else
            Dim bo As ReasonCodeMaintenanceBO = New ReasonCodeMaintenanceBO
            bo.disableReasonCodeMapping(cmbType.SelectedValue.ToString, cmbDescription.SelectedValue.ToString)
            MsgBox("Mapping successfully disabled!", MsgBoxStyle.Exclamation, Me.Text)
            Me.Close()
        End If

        logger.Debug("ReasonCodeMappings.cmdAddMapping_Click Exit")
    End Sub

    Private Sub cmdAddMapping_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddMapping.Click
        logger.Debug("ReasonCodeMappings.cmdAddMapping_Click Entry")

        If (cmbType.SelectedValue.ToString = "-1" Or cmbDescription.SelectedValue.ToString = "-1") Then
            MsgBox("Please select a Type and Detail record!", MsgBoxStyle.Exclamation, Me.Text)
        Else
            Dim bo As ReasonCodeMaintenanceBO = New ReasonCodeMaintenanceBO
            bo.addReasonCodeMapping(cmbType.SelectedValue.ToString, cmbDescription.SelectedValue.ToString)
            MsgBox("Mapping successfully added!", MsgBoxStyle.Exclamation, Me.Text)
            Me.Close()
        End If

        logger.Debug("ReasonCodeMappings.cmdAddMapping_Click Exit")
    End Sub
End Class