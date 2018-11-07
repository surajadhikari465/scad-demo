Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net

Public Class Team

    Private _dataset As DataSet

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub Team_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        logger.Debug("Team_Load Entry")
        LoadTeams()
        SetFieldsStates()
        logger.Debug("Team_Load Exit")
    End Sub

    Private Sub Button_AddTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddTeam.Click
        ' Load form to create a new team.
        logger.Debug("Button_AddTeam_Click Entry")
        Dim frm As TeamEdit = New TeamEdit(-1)
        frm.ShowDialog()
        frm.Dispose()
        LoadTeams()
        logger.Debug("Button_AddTeam_Click Exit")
    End Sub

    Private Sub Button_EditTeam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_EditTeam.Click


        logger.Debug("Button_EditTeam_Click Entry")

        Dim frm As TeamEdit

        If ComboBox_Teams.SelectedItem Is Nothing Then
            'Blank entry selected. Create a new Item.
            frm = New TeamEdit(-1)
        Else
            ' Load form to edit an existing team.
            frm = New TeamEdit(CType(ComboBox_Teams.SelectedItem, TeamBO).TeamNo)
        End If

        frm.ShowDialog()
        frm.Dispose()
        LoadTeams()

        logger.Debug("Button_EditTeam_Click Exit")
    End Sub

    Private Sub LoadTeams()

        logger.Debug("LoadTeams Entry")

        'Load Existing teams

        Dim DAO As TeamDAO = New TeamDAO
        _dataset = DAO.GetTeams()
        ComboBox_Teams.Items.Clear()
        For Each dr As DataRow In _dataset.Tables(0).Rows
            ComboBox_Teams.Items.Add(New TeamBO(CInt(dr("Team_No").ToString), dr("Team_Name").ToString, dr("Team_Abbreviation").ToString))
        Next
        _dataset.Dispose()
        ComboBox_Teams.DisplayMember = "TeamName"
        ComboBox_Teams.ValueMember = "TeamNo"

        logger.Debug("LoadTeams Exit")
    End Sub

    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
        logger.Debug("Button_Close_Click Entry")
        Me.Close()
        logger.Debug("Button_Close_Click Wxit")
    End Sub

    Private Sub SetFieldsStates()
        If InstanceDataDAO.IsFlagActive("UKIPS", glStoreID) Then
            Me.Button_AddTeam.Enabled = False
        Else
            Me.Button_AddTeam.Enabled = True
        End If
    End Sub

    
End Class

