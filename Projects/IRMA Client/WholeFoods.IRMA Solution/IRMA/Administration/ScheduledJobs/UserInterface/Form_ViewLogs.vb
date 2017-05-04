Option Strict Off
Option Explicit On
Imports WholeFoods.Utility
Imports Infragistics.Win.UltraWinDataSource
Imports Infragistics.Win.UltraWinGrid
Imports system.Data.SqlClient
Imports System.Configuration
Imports WholeFoods.IRMA.Administration.ConfigurationData.DataAccess
Imports log4net

Public Class Form_ViewLogs
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ' This enum is used to reference the columns of the grid that displays the log entries.
    Public Enum LogEntryGridColumns
        appName
        envShortName
        'logEntryId
        logDate
        appGUID
        hostName
        userName
        thread
        level
        logger
        message
        exception
        insertDate
    End Enum


    Private Sub btnViewLogEntries_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewLogEntries.Click
        popGrid()
    End Sub

    Private Sub popGrid()
        Dim logEntry As AppDBLogBO
        Dim rowIndex As Integer
        Dim results As SqlDataReader = Nothing

        ' Clear log msg detail text field.
        txtLogMsgDetail.Clear()
        lblLogSearchStatus.Text = "Log Entries Found: ?"
       

        Dim nCheckedRadioIdx As Integer = 0
        For Each ctrl As Control In GroupBoxSearchOptions.Controls
            If ctrl.GetType = GetType(RadioButton) Then
                If DirectCast(ctrl, RadioButton).Checked Then
                    Select Case DirectCast(ctrl, RadioButton).Name
                        Case "rbSearchAppLog"
                            nCheckedRadioIdx = 1
                        Case "rbSearchArchive"
                            nCheckedRadioIdx = 2
                        Case Else
                            nCheckedRadioIdx = 3
                    End Select
                End If
            End If
        Next
        udsAppLog.Rows.Clear()
        Try
            results = AppDBLogDAO.GetAppLogEntries(cmbAppName.Text, udteStartDate.Value, udteEndDate.Value, nCheckedRadioIdx)

            While results.Read
                rowIndex = rowIndex + 1
                udsAppLog.Rows.SetCount(rowIndex)
                logEntry = New AppDBLogBO(results)
                PopGridRow(logEntry, rowIndex)
            End While
        Catch ex As Exception
            logger.Error("Error retrieving log entries.", ex)
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try
        lblLogSearchStatus.Text = "Log Entries Found: " & CStr(rowIndex)

    End Sub
    Private Sub PopGridRow(ByVal logEntry As AppDBLogBO, ByVal index As Integer)
        Dim row As UltraDataRow

        row = Me.udsAppLog.Rows(index - 1)
        row(LogEntryGridColumns.appName) = logEntry.appName
        row(LogEntryGridColumns.envShortName) = logEntry.envShortName
        row(LogEntryGridColumns.logDate) = logEntry.logDate
        row(LogEntryGridColumns.appGUID) = logEntry.appGUID
        row(LogEntryGridColumns.hostName) = logEntry.hostName
        row(LogEntryGridColumns.userName) = logEntry.userName
        row(LogEntryGridColumns.thread) = logEntry.thread
        row(LogEntryGridColumns.level) = logEntry.level
        row(LogEntryGridColumns.logger) = logEntry.logger
        row(LogEntryGridColumns.message) = logEntry.message
        row(LogEntryGridColumns.exception) = logEntry.exception
        row(LogEntryGridColumns.insertDate) = logEntry.insertDate

    End Sub

    Private Sub Form_ViewLogs_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToParent()
        popAppListDropDown()
        udteStartDate.Value = Now.AddHours(-2)
        udteEndDate.Value = Now
    End Sub

    Private Sub popAppListDropDown()
        Dim envGUID As Guid = New Guid(ConfigurationManager.AppSettings("EnvironmentGUID"))

        Dim configDAO As ConfigurationDataDAO = New ConfigurationDataDAO
        Dim dtAppConfigAppList As DataTable = configDAO.GetApplicationList(envGUID)
        Me.cmbAppName.DataSource = dtAppConfigAppList
        Me.cmbAppName.ValueMember = "Name"
        Me.cmbAppName.DisplayMember = "Name"
        Me.cmbAppName.SelectedIndex = -1
    End Sub


    Private Sub ugridAppLogEntries_AfterRowActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugridAppLogEntries.AfterRowActivate
        ' tried: 
        ' AfterSelectChanged
        ' BeforeRowActivate
        ' AfterRowActivate
        txtLogMsgDetail.Clear()
        Try
            txtLogMsgDetail.Text = _
                    ugridAppLogEntries.ActiveRow.Cells.Item("AppName").Value & vbCrLf & _
                    "Logged: " & ugridAppLogEntries.ActiveRow.Cells.Item("LogDate").Value & vbCrLf & _
                    "Message: " & ugridAppLogEntries.ActiveRow.Cells.Item("Message").Value & vbCrLf & _
                    "Exception: " & ugridAppLogEntries.ActiveRow.Cells.Item("Exception").Value & vbCrLf & _
                    "Hostname: " & ugridAppLogEntries.ActiveRow.Cells.Item("HostName").Value & vbCrLf & _
                    "Username: " & ugridAppLogEntries.ActiveRow.Cells.Item("UserName").Value & vbCrLf & _
                    "Env: " & ugridAppLogEntries.ActiveRow.Cells.Item("EnvShortName").Value & vbCrLf & _
                    "Thread: " & ugridAppLogEntries.ActiveRow.Cells.Item("Thread").Value & vbCrLf & _
                    "Level: " & ugridAppLogEntries.ActiveRow.Cells.Item("Level").Value & vbCrLf & _
                    "Logger: " & ugridAppLogEntries.ActiveRow.Cells.Item("Logger").Value & vbCrLf & _
                    "Inserted: " & ugridAppLogEntries.ActiveRow.Cells.Item("InsertDate").Value & vbCrLf & _
                    "App GUID: " & ugridAppLogEntries.ActiveRow.Cells.Item("AppGUID").Value

        Catch ex As Exception
            txtLogMsgDetail.Text = "Error showing details: " & ex.ToString
        End Try

    End Sub

End Class