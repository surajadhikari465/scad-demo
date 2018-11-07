Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net

Public Class TeamEdit
    Private _dataset As DataSet
    Private _IsNewteam As Boolean
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Property IsNewTeam() As Boolean
        Get
            Return _IsNewteam
        End Get
        Set(ByVal value As Boolean)
            _IsNewteam = value
        End Set
    End Property

    Private _TeamNO As Integer
    Public Property TeamNo() As Integer
        Get
            Return _TeamNO
        End Get
        Set(ByVal value As Integer)
            _TeamNO = value
        End Set
    End Property


    Sub New(ByVal TeamNo As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.IsNewTeam = (TeamNo = -1)
        Me.TeamNo = TeamNo


    End Sub

    Private Sub TeamEdit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        logger.Debug("TeamEdit_Load Entry")

        If Not Me.IsNewTeam Then
            Me.Text = "Edit A Team"
            ' Load Existing team data.
            LoadteamInfo()
        Else
            Me.Text = "Create A New Team"
        End If
        SetFieldsStates()

        logger.Debug("TeamEdit_Load Exit")
    End Sub
    Private Sub LoadteamInfo()

        logger.Debug("LoadteamInfo Entry")

        Dim DAO As TeamDAO = New TeamDAO
        _dataset = DAO.GetTeam(Me.TeamNo)
        TextBox_TeamNumber.Text = _dataset.Tables(0).Rows(0)("Team_No").ToString()
        TextBox_TeamName.Text = _dataset.Tables(0).Rows(0)("Team_Name").ToString()
        TextBox_TeamAbbrev.Text = _dataset.Tables(0).Rows(0)("Team_Abbreviation").ToString()
        TextBox_TeamNumber.Enabled = False
        _dataset.Dispose()

        logger.Debug("LoadteamInfo Exit")

    End Sub

    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click

        logger.Debug("Button_Save_Click Entry")

        Dim BO As New TeamBO
        BO.TeamNo = CInt(TextBox_TeamNumber.Text.Trim())
        BO.TeamName = TextBox_TeamName.Text.Trim()
        BO.TeamAbbr = TextBox_TeamAbbrev.Text.Trim()

        Dim DAO As TeamDAO = New TeamDAO

        Try
            If Not Me.IsNewTeam Then
                DAO.SaveChanges(BO)
            Else
                DAO.CreateNewTeam(BO)
            End If
            Me.Close()
        Catch ex As Exception
            MsgBox("Team Information could not be saved. " & vbCrLf & ex.Message)
            logger.Info("Team Information could not be saved. ")
        End Try

        logger.Debug("Button_Save_Click Exit")



    End Sub

    Private Sub TeamNumber_MaskRejected(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MaskInputRejectedEventArgs) Handles TextBox_TeamNumber.MaskInputRejected
        MsgBox("A Team Number can only be a 3 digit numeric value")
    End Sub

    Private Sub TeamName_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox_TeamName.Validating
        logger.Debug("TeamName_Validating Entry")

        ' Validate Team Name: Make sure one doesnt already exist with the same name.
        Dim DAO As TeamDAO = New TeamDAO
        _dataset = DAO.Validate_Teamname(Me.TeamNo, TextBox_TeamName.Text.Trim())
        If _dataset.Tables(0).Rows.Count > 0 Then
            MsgBox("A Team with the name " & _dataset.Tables(0).Rows(0)("Team_Name").ToString().Trim() & " already exists.")
            e.Cancel = True
        End If
        _dataset.Dispose()

        logger.Debug("TeamName_Validating Exit")


    End Sub

    Private Sub TeamAbbrev_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox_TeamAbbrev.Validating
        '' Validate Team Abbrev: Make sure one doesnt already exist with the same name.
        'Dim DAO As TeamDAO = New TeamDAO
        '_dataset = DAO.Validate_TeamAbbr(Me.TeamNo, TextBox_TeamAbbrev.Text.Trim())
        'If _dataset.Tables(0).Rows.Count > 0 Then
        '    MsgBox("A Team (" & _dataset.Tables(0).Rows(0)("Team_Name").ToString.Trim & ") with the abbreviation " & _dataset.Tables(0).Rows(0)("Team_Abbreviation").ToString().Trim() & " already exists.")
        '    e.Cancel = True
        'End If
        '_dataset.Dispose()

    End Sub

    Private Sub TeamNumber_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBox_TeamNumber.Validating
        ' Validate Team Number: Make sure one doesnt already exist with the same name.

        logger.Debug("TeamNumber_Validating Entry")

        If Me.IsNewTeam Then
            Dim DAO As TeamDAO = New TeamDAO
            _dataset = DAO.GetTeam(CInt(TextBox_TeamNumber.Text.Trim))
            If _dataset.Tables(0).Rows.Count > 0 Then
                MsgBox("A Team (" & _dataset.Tables(0).Rows(0)("Team_Name").ToString.Trim & ") with the Team_No " & _dataset.Tables(0).Rows(0)("Team_No").ToString().Trim() & " already exists.")
                e.Cancel = True
            End If
            _dataset.Dispose()
        End If

        logger.Debug("TeamNumber_Validating Exit")

    End Sub


    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub
    Private Sub SetFieldsStates()
        If InstanceDataDAO.IsFlagActive("UKIPS", glStoreID) Then
            Me.TextBox_TeamName.Enabled = False
            Me.TextBox_TeamAbbrev.Enabled = False
            Me.Button_Save.Visible = False
            Me.Button_Cancel.Text = "Close"
        Else
            Me.TextBox_TeamName.Enabled = True
            Me.TextBox_TeamAbbrev.Enabled = True
            Me.Button_Save.Visible = True
            Me.Button_Cancel.Text = "Cancel"
        End If
    End Sub
End Class