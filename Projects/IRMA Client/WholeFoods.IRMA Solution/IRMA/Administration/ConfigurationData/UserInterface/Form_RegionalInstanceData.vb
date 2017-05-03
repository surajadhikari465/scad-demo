Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_RegionalInstanceData

    Private _hasChanges As Boolean
    Private _isInitializing As Boolean = True

    Private Sub Form_RegionalInstanceData_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToParent()

        'load form data
        BindData()
    End Sub

    Private Sub Form_RegionalInstanceData_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If _hasChanges Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                If Not ApplyChanges() Then
                    e.Cancel = True
                End If
            End If
        End If
    End Sub

    Private Sub BindData()
        Dim regionData As InstanceDataBO = InstanceDataDAO.GetInstanceData

        _isInitializing = True

        'set region info
        Me.TextBox_RegionName.Text = regionData.RegionName
        Me.TextBox_RegionAbbr.Text = regionData.RegionCode
        Me.TextBox_UGCulture.Text = regionData.UGCulture
        Me.TextBox_UGDateMask.Text = regionData.UGDateMask

        'set # digits to send to scales
        If regionData.PluDigitsSentToScale.Equals("ALWAYS 4") Then
            Me.RadioButton_Always4.Checked = True
        ElseIf regionData.PluDigitsSentToScale.Equals("ALWAYS 5") Then
            Me.RadioButton_Always5.Checked = True
        Else
            Me.RadioButton_VariableByItem.Checked = True
        End If

        _isInitializing = False
    End Sub

    Private Function ApplyChanges() As Boolean
        Dim success As Boolean
        Dim regionData As New InstanceDataBO
        Dim instanceDAO As New InstanceDataDAO

        Select Case True
            Case Me.RadioButton_Always4.Checked
                regionData.PluDigitsSentToScale = "ALWAYS 4"
            Case Me.RadioButton_Always5.Checked
                regionData.PluDigitsSentToScale = "ALWAYS 5"
            Case Else
                regionData.PluDigitsSentToScale = "VARIABLE BY ITEM"
        End Select

        regionData.UGCulture = TextBox_UGCulture.Text
        regionData.UGDateMask = TextBox_UGDateMask.Text

        Try
            'save changes
            instanceDAO.UpdateInstanceData(regionData)
            success = True
        Catch ex As Exception
            Logger.LogError("Exception: ", Me.GetType(), ex)
            'display a message to the user
            MessageBox.Show(Form_IRMABase.ERROR_DB, "IRMA Application Error", MessageBoxButtons.OK)
            'send message about exception
            Dim args(1) As String
            args(0) = "Form_RegionalInstanceData form: ApplyChanges sub"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)

            success = False
        End Try

        Return success
    End Function

#Region "button events"

    Private Sub Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        Me.Close()
    End Sub

    Private Sub RadioButton_Always4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_Always4.CheckedChanged
        If Not _isInitializing Then
            _hasChanges = True
        End If
    End Sub

    Private Sub RadioButton_Always5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_Always5.CheckedChanged
        If Not _isInitializing Then
            _hasChanges = True
        End If
    End Sub

    Private Sub RadioButton_VariableByItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_VariableByItem.CheckedChanged
        If Not _isInitializing Then
            _hasChanges = True
        End If
    End Sub

#End Region

End Class