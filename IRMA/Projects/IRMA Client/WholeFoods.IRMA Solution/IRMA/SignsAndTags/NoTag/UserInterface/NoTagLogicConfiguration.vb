Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class NoTagLogicConfiguration

    Private dataAccess As NoTagDataAccess
    Private defaultRuleConfigurations As Dictionary(Of String, Integer)
    Private subteamOverrides As Dictionary(Of Integer, Integer)

    Private Const ruleDefault As Integer = 30
    Private Const subteamOverrideDefault As Integer = 0

    Public Sub New(dataAccess As NoTagDataAccess)
        InitializeComponent()

        Me.dataAccess = dataAccess
    End Sub

    Private Sub NoTagLogicConfiguration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		Try
			RetrieveCurrentConfiguration()
			PopulateSubteamCombobox()
		Catch ex As Exception
			MessageBox.Show(String.Format("An error occurred while retrieving the current no-tag configuration: {0}.", ex.Message), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Me.Close()
        End Try

        Cursor = Cursors.Default
    End Sub

	Private Sub PopulateSubteamCombobox()
		cmbSubTeam.ValueMember = "SubTeamNo"
		cmbSubTeam.DisplayMember = "SubTeamName"
		cmbSubTeam.DataSource = SubTeamDAO.GetAlignedSubteams()
	End Sub

	Private Sub RetrieveCurrentConfiguration()
		SetMovementRuleDefaultThresholdValues()
		GetSubteamOverrides()
	End Sub

	Private Sub SetMovementRuleDefaultThresholdValues()
		defaultRuleConfigurations = dataAccess.GetRuleDefaultThresholds()

		If defaultRuleConfigurations.Count > 0 Then
			Dim movementConfig As Integer = defaultRuleConfigurations("MovementHistoryRule")
			Dim orderingConfig As Integer = defaultRuleConfigurations("OrderingHistoryRule")
			Dim receivingConfig As Integer = defaultRuleConfigurations("ReceivingHistoryRule")

			NumericUpDownMovementHistory.Value = movementConfig
			NumericUpDownOrderingHistory.Value = orderingConfig
			NumericUpDownReceivingHistory.Value = receivingConfig
		End If
	End Sub

	Private Sub GetSubteamOverrides()
		subteamOverrides = dataAccess.GetSubteamOverrides()
	End Sub

	Private Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click
		Try
			dataAccess.UpdateNoTagRuleThresholds(defaultRuleConfigurations)
			dataAccess.UpdateNoTagSubteamOverrides(subteamOverrides)

			MessageBox.Show("Configuration values have been saved successfully.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
			Me.DialogResult = DialogResult.OK
		Catch ex As Exception
			MessageBox.Show(String.Format("An error occurred while updating the no-tag configuration: {0}.", ex.Message), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Me.Close()
		End Try
	End Sub

	Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
		Me.DialogResult = DialogResult.Cancel
	End Sub

	Private Sub ComboBoxSubteams_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSubTeam.SelectedIndexChanged
		If subteamOverrides.ContainsKey(cmbSubTeam.SelectedItem.SubTeamNo) Then
			NumericUpDownSubteamOverride.Value = subteamOverrides(cmbSubTeam.SelectedItem.SubTeamNo)
		Else
			subteamOverrides.Add(cmbSubTeam.SelectedItem.SubTeamNo, 0)
			NumericUpDownSubteamOverride.Value = 0
		End If
	End Sub

	Private Sub NumericUpDownSubteamOverride_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownSubteamOverride.ValueChanged
		subteamOverrides(cmbSubTeam.SelectedItem.SubTeamNo) = NumericUpDownSubteamOverride.Value
	End Sub

    Private Sub NumericUpDownMovementHistory_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownMovementHistory.ValueChanged
        defaultRuleConfigurations("MovementHistoryRule") = NumericUpDownMovementHistory.Value
    End Sub

    Private Sub NumericUpDownOrderingHistory_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownOrderingHistory.ValueChanged
        defaultRuleConfigurations("OrderingHistoryRule") = NumericUpDownOrderingHistory.Value
    End Sub

    Private Sub NumericUpDownReceivingHistory_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDownReceivingHistory.ValueChanged
        defaultRuleConfigurations("ReceivingHistoryRule") = NumericUpDownReceivingHistory.Value
    End Sub

    Private Sub ButtonReset_Click(sender As Object, e As EventArgs) Handles ButtonReset.Click
        If MessageBox.Show("Rule thresholds and subteam overrides will be reset to default values.  Continue?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            NumericUpDownMovementHistory.Value = ruleDefault
            NumericUpDownOrderingHistory.Value = ruleDefault
            NumericUpDownReceivingHistory.Value = ruleDefault

            Dim keys As List(Of Integer) = New List(Of Integer)
            keys.AddRange(subteamOverrides.Keys)

            For Each key As Integer In keys
                subteamOverrides(key) = subteamOverrideDefault
            Next

            NumericUpDownSubteamOverride.Value = subteamOverrideDefault
        End If
    End Sub
End Class