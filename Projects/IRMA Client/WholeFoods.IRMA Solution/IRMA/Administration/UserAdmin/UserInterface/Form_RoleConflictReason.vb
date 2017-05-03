Imports System.Text
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Net.Mail

Public Class Form_RoleConflictReasons
    Dim m_sRoleConflicts As String
    Dim m_sTitle As String
    Dim m_iTitleId As Integer
    Dim m_sUser As String
    Dim m_iUserId As Integer
    Dim m_bConflictRiskAccepted As Boolean
    Dim m_sConflictType As String

    Public Property ConflictType() As String
        Get
            ConflictType = m_sConflictType
        End Get
        Set(ByVal value As String)
            m_sConflictType = value
        End Set
    End Property

    Public Property RoleConflicts() As String
        Get
            RoleConflicts = m_sRoleConflicts
        End Get
        Set(ByVal value As String)
            m_sRoleConflicts = value
        End Set
    End Property

    Public Property ConflictRiskAccepted() As Boolean
        Get
            ConflictRiskAccepted = m_bConflictRiskAccepted
        End Get
        Set(ByVal value As Boolean)
            m_bConflictRiskAccepted = value
        End Set
    End Property

    Public Property User() As String
        Get
            User = m_sUser
        End Get
        Set(ByVal value As String)
            m_sUser = value
        End Set
    End Property

    Public Property UserId() As Integer
        Get
            UserId = m_iUserId
        End Get
        Set(ByVal value As Integer)
            m_iUserId = value
        End Set
    End Property

    Public Property Title() As String
        Get
            Title = m_sTitle
        End Get
        Set(ByVal value As String)
            m_sTitle = value
        End Set
    End Property

    Public Property TitleId() As Integer
        Get
            TitleId = m_iTitleId
        End Get
        Set(ByVal value As Integer)
            m_iTitleId = value
        End Set
    End Property

    Private Sub Form_ManageTitles_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PopulateForm()
    End Sub

    Private Sub PopulateForm()
        grdConflicts.Columns.Clear()
        Label_Notification.Text = "A notification e-mail will be sent to " & ConfigurationServices.AppSettings("IRMASOXRoleConflictEmail") & " upon acceptance of the risk."

        FormatGrid()
        PopulateGrid(Me.RoleConflicts)
    End Sub

    Private Sub PopulateGrid(ByVal sConflicts As String)
        Dim row As New DataGridViewRow
        Dim aryConflictPairs() As String
        Dim x As Integer = 0

        aryConflictPairs = Split(sConflicts, "|")

        For x = 0 To UBound(aryConflictPairs)
            If aryConflictPairs(x) <> "" Then grdConflicts.Rows.Add(Split(aryConflictPairs(x), "^"))
        Next
    End Sub

    Private Sub FormatGrid()
        Dim col1 As New DataGridViewTextBoxColumn
        Dim col2 As New DataGridViewTextBoxColumn
        Dim col3 As New DataGridViewTextBoxColumn
        Dim col4 As New DataGridViewTextBoxColumn

        col1.HeaderText = "Role 1"
        col1.Name = "Role1"
        grdConflicts.Columns.Add(col1)
        grdConflicts.Columns(0).DisplayIndex = 0
        grdConflicts.Columns(0).ReadOnly = True
        grdConflicts.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        grdConflicts.Columns(0).FillWeight = 50

        col2.HeaderText = "Role 2"
        col2.Name = "Role2"
        grdConflicts.Columns.Add(col2)
        grdConflicts.Columns(1).DisplayIndex = 1
        grdConflicts.Columns(1).ReadOnly = True
        grdConflicts.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
        grdConflicts.Columns(1).FillWeight = 50

        col3.HeaderText = "Reason"
        col3.Name = "Reason"
        col3.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        grdConflicts.Columns.Add(col3)
        grdConflicts.Columns(2).DisplayIndex = 2
        grdConflicts.Columns(2).ReadOnly = False
        grdConflicts.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        grdConflicts.Columns(2).FillWeight = 200

        grdConflicts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub Button_Close_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        Dim sEmail As String
        Dim row As DataGridViewRow

        For Each row In grdConflicts.Rows
            If row.Cells("Reason").Value = "" Then
                MsgBox("A conflict reason is required for all conflicts.  Please fill in a reason and try again.", MsgBoxStyle.Critical, "Title Role Conflicts")
                Exit Sub
            End If
        Next

        Cursor = Cursors.WaitCursor

        Try
            sEmail = ConfigurationServices.AppSettings("IRMASOXRoleConflictEmail")
        Catch ex As Exception
            sEmail = ""
        End Try

        If sEmail = "" Then
            MsgBox("Your changes will not be saved because there is no IRMA SOX Conflict e-mail address set up.  Please have the System Administrator add the IRMASOXRoleConflictEmail key to the IRMA Client and try again.", MsgBoxStyle.Critical, "IRMA SOX Role Conflict")
            Me.ConflictRiskAccepted = False
        Else
            LogRoleConflict(Me.ConflictType)
            SendRoleConflictEmail(sEmail)
            Me.ConflictRiskAccepted = True
        End If

        Cursor = Cursors.Arrow

        Me.Close()
    End Sub

    Private Sub SendRoleConflictEmail(ByVal sToEmail As String)
        Dim sFromEmail As String = UCase(gsRegionCode) & "_IRMASOXConflict@wholefoods.com"
        Dim sBody As String = ""
        Dim sSubject As String
        Dim SendMailClient As SmtpClient
        Dim msg As MailMessage
        Dim row As DataGridViewRow
        Dim sConflictPairs As String = ""

        For Each row In grdConflicts.Rows
            sConflictPairs = sConflictPairs & row.Cells("Role1").Value & " & " & row.Cells("Role2").Value & vbTab & "Reason: " & row.Cells("Reason").Value & vbCrLf & vbTab
        Next

        sSubject = UCase(gsRegionCode) & " IRMA SOX Conflict Notification"

        Select Case Me.ConflictType
            Case "T"
                sBody = "This e-mail is to inform you that the Security Administrator (" & gsUserName & ") has assigned the following conflicting SOX roles to the " & Me.Title & " title." & _
                        vbCrLf & vbCrLf & vbTab & sConflictPairs & vbCrLf & vbCrLf & _
                        "The conflicts are regionally configurable by the Application Configuration Administrator by going to Administration -> IRMA Configuration -> Application Configuration -> Manage Role Conflicts." & _
                        vbCrLf & vbCrLf & "All role conflicts can be viewed by running the IRMA SOX Role Conflict report in Report Manager."
            Case "U"
                sBody = "This e-mail is to inform you that the Security Administrator (" & gsUserName & ") has assigned the following conflicting SOX roles to the user " & Me.User & "." & _
                        vbCrLf & vbCrLf & vbTab & sConflictPairs & vbCrLf & vbCrLf & _
                        "The conflicts are regionally configurable by the Application Configuration Administrator by going to Administration -> IRMA Configuration -> Application Configuration -> Manage Role Conflicts." & _
                        vbCrLf & vbCrLf & "All role conflicts can be viewed by running the IRMA SOX Role Conflict report in Report Manager."
        End Select

        Try
            SendMailClient = New SmtpClient(ConfigurationServices.AppSettings("EmailSMTPServer"))
            msg = New MailMessage(sFromEmail, sToEmail, sSubject, sBody)
            msg.CC.Add(API.GetADUserInfo(Environment.UserName, "mail"))
            SendMailClient.Send(msg)
        Catch ex As Exception
            MsgBox("No e-mail could be sent because of the following error:" & vbCrLf & vbCrLf & ex.Message)
        Finally
            SendMailClient = Nothing
            msg = Nothing
        End Try
    End Sub

    Private Sub LogRoleConflict(ByVal sType As String)
        Dim row As DataGridViewRow

        For Each row In grdConflicts.Rows
            Select Case sType
                Case "T"
                    TitleDAO.InsertRoleConflictReason(sType, -1, Me.TitleId, row.Cells("Role1").Value, row.Cells("Role2").Value, row.Cells("Reason").Value, giUserID)
                Case "U"
                    TitleDAO.InsertRoleConflictReason(sType, Me.UserId, -1, row.Cells("Role1").Value, row.Cells("Role2").Value, row.Cells("Reason").Value, giUserID)
            End Select
        Next
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub
End Class
