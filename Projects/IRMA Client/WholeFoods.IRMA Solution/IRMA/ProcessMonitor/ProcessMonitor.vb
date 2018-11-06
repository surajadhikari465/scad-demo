Option Strict Off
Imports WholeFoods.IRMA.ProcessMonitor.DataAccess
Imports log4net
Public Class ProcessMonitor

    Dim _dataSet As DataSet
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        GetJobStatusList()
        Me.UltraGrid_JobStatusList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False

    End Sub

    Private Sub GetJobStatusList()
        logger.Debug("GetJobStatusList Entry")
        Dim DAO As ProcessMDAO = New ProcessMDAO
        _dataSet = DAO.GetJobStatusList()

        Me.UltraGrid_JobStatusList.DataSource = _dataSet.Tables(0)
        If Me.UltraGrid_JobStatusList.Rows.Count > 0 Then
            'first row appears to be defaulted because it is highlighted, but it's not actually selected.
            'so if user tried to edit or delete upon entering the screen they must still click the highlighted row.
            'below code is actually selecting row that is already highlighted.
            Me.UltraGrid_JobStatusList.Rows(0).Selected = True
        End If

        logger.Debug("GetJobStatusList Exit")

    End Sub


    Private Sub Button_Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Refresh.Click
        Cursor = Cursors.WaitCursor
        GetJobStatusList()
        Cursor = Cursors.Default
    End Sub

    Private Sub rdoOFF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoOFF.CheckedChanged
        Cursor = Cursors.WaitCursor
        GetJobStatusList()
        Cursor = Cursors.Default
    End Sub

    Private Sub rdoON_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoON.CheckedChanged
        Cursor = Cursors.WaitCursor
        GetJobStatusList()
        Cursor = Cursors.Default
    End Sub

    Private Sub UltraGrid_JobStatusList_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UltraGrid_JobStatusList.InitializeLayout
        If rdoOFF.Checked = True Then
            e.Layout.Bands(0).Columns("details").Hidden = True
        Else
            e.Layout.Bands(0).Columns("details").Hidden = False
        End If

    End Sub


End Class