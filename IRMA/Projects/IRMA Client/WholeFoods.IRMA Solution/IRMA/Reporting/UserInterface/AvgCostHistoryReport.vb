Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.Utility
Imports log4net

Public Class frmAvgCostHistoryReport

    Private _subteam As Integer = -1
    Private _bu As Integer = -1

    Public WriteOnly Property BusinessUnit() As Integer
        Set(ByVal value As Integer)
            Me._bu = value
        End Set
    End Property

    Public WriteOnly Property SubTeam() As Integer
        Set(ByVal value As Integer)
            Me._subteam = value
        End Set
    End Property

    Public WriteOnly Property Identifier() As String
        Set(ByVal value As String)
            Me.txtIdentifier.Text = value
        End Set
    End Property

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub AvgCostHistoryReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim StoreList As ArrayList = StoreListDAO.GetStoresListByStoreName()
        With cmbStore
            .DataSource = StoreList
            .DisplayMember = "StoreName"
            .ValueMember = "StoreNo"
            .SelectedIndex = 0
        End With

        LoadAllSubTeams(cmbSubTeam)

        cmbSubTeam.Items.Insert(0, "--ALL--")

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            cmbStore.SelectedValue = glStore_Limit
        Else
            cmbStore.SelectedIndex = -1
        End If

        If cmbSubTeam.Items.Count > 0 Then
            cmbSubTeam.SelectedIndex = Me._subteam + 1 ' this is because the selector from the avg cost panel does not have an "all" option and we are passing over a position index to set the subteam
        End If

        ' set the current tolerance value
        Me.txtTolerance.Text = ConfigurationServices.AppSettings("AvgCostTolerance")

        dtpStartDate.Value = Date.Today
        dtpEndDate.Value = Date.Today

    End Sub

    Private Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click

        '  Validation

        If Not IsNumeric(Trim(Me.txtTolerance.Text)) Then
            MsgBox(String.Format(ResourcesCommon.GetString("msg_validation_notNumeric"), lblTolerance.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            Me.txtTolerance.Focus()
            Exit Sub
        ElseIf Trim(Me.txtTolerance.Text) = String.Empty Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblTolerance.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            Me.txtTolerance.Focus()
            Exit Sub
        End If

        If cmbStore.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblBusinessUnit.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbStore.Focus()
            Exit Sub
        End If

        If cmbSubTeam.SelectedIndex = -1 Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblSubTeam.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
            cmbStore.Focus()
            Exit Sub
        End If

        '--------------------------
        ' Setup Report URL
        ' for Reporting Services
        '--------------------------

        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("AvgCostVariance")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        '--------------------------
        ' Add Report Parameters
        '--------------------------

        If Trim(txtIdentifier.Text) = String.Empty Then
            sReportURL.Append("&Identifier:isnull=true")
        Else
            sReportURL.Append("&Identifier=" & Trim(txtIdentifier.Text))
        End If

        sReportURL.Append("&Store_No=" & cmbStore.SelectedValue.ToString)

        If cmbSubTeam.SelectedIndex = 0 Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
        End If

        If Len(Trim(dtpStartDate.Text)) <> 0 Then
            sReportURL.Append("&StartDate=" & Trim(dtpStartDate.Text))
        End If

        If Len(Trim(dtpEndDate.Text)) <> 0 Then
            sReportURL.Append("&EndDate=" & Trim(dtpEndDate.Text))
        End If

        sReportURL.Append("&Tolerance=" & Trim(txtTolerance.Text))

        sReportURL.Append("&LimitToOutOfTolerance=" & chkLimitOutput.CheckState)

        '--------------------------
        ' Display Report
        '--------------------------

        Call ReportingServicesReport(sReportURL.ToString)

        Me.Close()

    End Sub
End Class