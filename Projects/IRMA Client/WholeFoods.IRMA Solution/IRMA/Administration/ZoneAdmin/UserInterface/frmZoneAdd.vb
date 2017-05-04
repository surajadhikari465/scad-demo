Imports log4net
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class ZoneAdd

    ' ---------------------------------------------------------------------------
    ' Revision History
    ' ---------------------------------------------------------------------------
    ' 8/19/2010             Tom Lux               TFS 13258        Fixed event method for cancel button (missing 'handles' piece).
    ' 8/19/2010             Tom Lux               TFS 13258        Added validation so that when user clicks 'OK', form/app won't bomb due to null references.
    ' 8/19/2010             Tom Lux               TFS 13258        Added logger and logging to help track was happens in this form.


#Region "Private Members"

    ''' <summary>
    ''' Log4Net logger for this class.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    '  Private mZone As Zone

#End Region

    Private Sub frmZoneAdd_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cboZoneRegion.DataSource = RegionDAO.GetRegionList
        cboZoneRegion.DisplayMember = "RegionName"
        cboZoneRegion.ValueMember = "Region_ID"
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        ' Dim i As Integer

        If String.IsNullOrEmpty(Me.txtZoneDesc.Text) Or Me.txtZoneDesc.Text.Trim = "" Then
            MessageBox.Show("A zone description is required.", "Add Zone", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If String.IsNullOrEmpty(Me.txtZoneGLMarketingExpenseAcct.Text) Or Me.txtZoneGLMarketingExpenseAcct.Text.Trim = "" Then
            MessageBox.Show("A GL Marketing Expense Acct is required.", "Add Zone", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Not IsNumeric(txtZoneGLMarketingExpenseAcct.Text) Then
            MessageBox.Show("The GL Marketing Expense Acct must be a number.", "Add Zone", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If String.IsNullOrEmpty(Me.cboZoneRegion.SelectedValue) Then
            MessageBox.Show("A region is required.", "Add Zone", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        ZoneDAO.InsertZone(txtZoneDesc.Text, txtZoneGLMarketingExpenseAcct.Text, cboZoneRegion.SelectedValue, giUserID)

        Me.Close()
    End Sub
End Class