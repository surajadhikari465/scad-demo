Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net

Public Class SubTeam
	Private _dataset As DataSet

	' Define the log4net logger for this class.
	Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


	Private Sub SubTeam_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		LoadSubTeams()
	End Sub
	Private Sub LoadSubTeams()
		ComboBox_SubTeams.DataSource = SubTeamDAO.GetSubteams()
		SetFieldsStates()
	End Sub

	Private Sub Button_AddSubTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddSubTeam.Click
		logger.Debug("Button_AddSubTeam_Click")
		' Load form to create a new team.
		Dim frm As SubTeamEdit = New SubTeamEdit(-1)
		frm.ShowDialog()
		frm.Dispose()
		LoadSubTeams()
		logger.Debug("Button_AddSubTeam_Click")
	End Sub

	Private Sub Button_EditSubTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_EditSubTeam.Click
		Dim frm As SubTeamEdit

		If ComboBox_SubTeams.SelectedItem Is Nothing Then
			'Blank entry selected. Create a new Item.
			frm = New SubTeamEdit(-1)
		Else
			' Load form to edit an existing team.
			frm = New SubTeamEdit(ComboBox_SubTeams.SelectedItem.SubTeamNo)  '.SelectedValue)
		End If

		frm.ShowDialog()
		frm.Dispose()
		LoadSubTeams()
	End Sub

	Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
		logger.Debug("Button_Close_Click Entry")
		Me.Close()
		logger.Debug("Button_Close_Click Exit")
	End Sub
	Private Sub SetFieldsStates()
		If InstanceDataDAO.IsFlagActive("UKIPS", glStoreID) Then
			Me.Button_AddSubTeam.Enabled = False
		Else
			Me.Button_AddSubTeam.Enabled = True
		End If
	End Sub
End Class