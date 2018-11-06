Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
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
        logger.Debug("LoadSubTeams Entry")
        'Load Existing teams

        Dim DAO As New SubTeamDAO

        ComboBox_SubTeams.DataSource = SubTeamDAO.GetSubteams()
        ComboBox_SubTeams.DisplayMember = "SubTeam_Name"
        ComboBox_SubTeams.ValueMember = "SubTeam_No"

        SetFieldsStates()

        logger.Debug("LoadSubTeams Exit")
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

        logger.Debug("Button_EditSubTeam_Click Entry")
        Dim frm As SubTeamEdit

        If ComboBox_SubTeams.SelectedItem Is Nothing Then
            'Blank entry selected. Create a new Item.
            frm = New SubTeamEdit(-1)
        Else
            ' Load form to edit an existing team.
            'frm = New SubTeamEdit(CType(ComboBox_SubTeams.SelectedItem, SubTeamBO).SubTeamNo)
            frm = New SubTeamEdit(ComboBox_SubTeams.SelectedValue)
        End If

        frm.ShowDialog()
        frm.Dispose()
        LoadSubTeams()

        logger.Debug("Button_EditSubTeam_Click Exit")

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