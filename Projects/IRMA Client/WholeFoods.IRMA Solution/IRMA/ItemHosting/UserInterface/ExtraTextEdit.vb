Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class ExtraTextEdit

    Private _scaleBO As ScaleExtraTextBO
    Private _itemKey As Integer
    Private _hasChanged As Boolean
    Private _isInitializing As Boolean
    Private _enableReturnsInExtraText As Boolean

    Public Property ItemKey() As Integer
        Get
            Return Me._itemKey
        End Get
        Set(ByVal value As Integer)
            Me._itemKey = value
        End Set
    End Property

    Private Property ExtraTextBox() As String
        Get
            Return Me.txtExtraText.Text
        End Get
        Set(ByVal value As String)
            ' set the current object and the text box to the same values so we keep the object always updated with the current text
            Me.CurrentExtraTextBO.ExtraText = value
            Me.txtExtraText.Text = value
        End Set
    End Property

    Private Property ExtraTextDescription() As String
        Get
            Return Me.txtExtraTextDescription.Text
        End Get
        Set(ByVal value As String)
            ' set the current object and the text box to the same values so we keep the object always updated with the current text
            Me.CurrentExtraTextBO.Description = value
            Me.txtExtraTextDescription.Text = value
        End Set
    End Property

    Public Property HasChanged() As Boolean
        Get
            Return Me._hasChanged
        End Get
        Set(ByVal value As Boolean)
            Select Case value
                Case False
                    Me.cmdSave.Enabled = False
                Case True
                    Me.cmdSave.Enabled = True
            End Select
        End Set
    End Property

    Public ReadOnly Property CurrentExtraTextBOChanged() As Boolean
        Get
            Return Me._hasChanged
        End Get
    End Property

    Public Property CurrentExtraTextBO() As ScaleExtraTextBO
        Get
            Return Me._scaleBO
        End Get
        Set(ByVal value As ScaleExtraTextBO)

            Me._scaleBO = value

            Dim dt As DataTable
            dt = New DataTable
            dt = ItemDAO.GetItemName(Me.ItemKey)

            ' clear out editable values
            Me.txtExtraText.Text = String.Empty
            Me.cmbLabelType.SelectedIndex = -1

            ' load the label type drop-down
            Me.cmbLabelType.DataSource = ScaleLabelTypeDAO.GetComboList()

            ' first set the scale object values
            Me.ExtraTextBox = value.ExtraText
            Me.ExtraTextDescription = value.Description

            If Me.cmbLabelType.Items.Count > 0 Then
                Me.cmbLabelType.DisplayMember = "Description"
                Me.cmbLabelType.ValueMember = "ID"
                ' assign it the value attached to this extra text record
                Me.cmbLabelType.SelectedValue = value.Scale_LabelType_ID
            End If

            ' now the linked item values
            If Not value Is Nothing Then

                Me.lblExtraTextItemDesc.Text = dt.Rows(0).Item("Item_Description").ToString
                Me.lblExtraTextItemIdentifier.Text = dt.Rows(0).Item("Identifier").ToString

                dt = Nothing

            End If

            If value.ID > 0 Then
                Me.grpCurrentRecordOptions.Enabled = True
                Me.grpDetails.Enabled = True
            Else
                Me.grpCurrentRecordOptions.Enabled = False
                Me.grpDetails.Enabled = False
            End If

        End Set

    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me._isInitializing = True

    End Sub

    Private Sub ExtraTextEdit_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me._scaleBO.ID > 0 Then
            Me.grpCurrentRecordOptions.Enabled = True
            Me.grpDetails.Enabled = True
        Else
            Me.grpCurrentRecordOptions.Enabled = False
            Me.grpDetails.Enabled = False
        End If
        '20110210 - DBS Set Permissions logic to allow non editors to view ingredients
        SetPermissions()

        cmdClose.Enabled = True
        Me._isInitializing = False
        Me._hasChanged = False

    End Sub

    Private Sub SetPermissions()
        Dim IsEditable As Boolean = False
        IsEditable = (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser

        Dim IsReadOnly As Boolean = False
        If IsEditable = False Then
            IsReadOnly = True
        End If
        Me.txtExtraTextDescription.ReadOnly = IsReadOnly
        Me.cmbLabelType.Enabled = IsEditable
        Me.btnExtraTxtLookup.Enabled = IsEditable
        Me.txtExtraText.ReadOnly = IsReadOnly
        Me.btnExtraTxtNew.Enabled = IsEditable
        Me.btnExtraTxtLink.Enabled = IsEditable
        Me.cmdSave.Enabled = IsEditable

        Me._enableReturnsInExtraText = (Not IsReadOnly) AndAlso InstanceDataDAO.IsFlagActive("EnableReturnsInExtraTextAndStorageData")
        Me.txtExtraText.AcceptsReturn = Me._enableReturnsInExtraText
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Dim extraTextCount As Integer
        ValidateOnSave()

        ' set the currently selected extra text record details and save them
        Me.CurrentExtraTextBO.Description = Me.txtExtraTextDescription.Text
        Me.CurrentExtraTextBO.ExtraText = Me.txtExtraText.Text
        Me.CurrentExtraTextBO.Scale_LabelType_ID = CInt(Me.cmbLabelType.SelectedValue)

        extraTextCount = ScaleExtraTextDAO.GetItemScaleCountWithExtraText(CurrentExtraTextBO.ID)
        If extraTextCount <= 1 OrElse
        MessageBox.Show(String.Format(ResourcesCommon.GetString("msg_confirmUpdateMultiple"), extraTextCount.ToString(), "items"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

            ScaleExtraTextDAO.Save(Me.CurrentExtraTextBO)

            Me.HasChanged = False

            Windows.Forms.Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub ValidateOnSave()

        Me.frmErrorProvider.Clear()

        If Me.cmbLabelType.SelectedIndex = -1 Then Me.frmErrorProvider.SetError(Me.cmbLabelType, "Label Type selection required.")
        If Me.txtExtraText.Text.Length = 0 Then Me.frmErrorProvider.SetError(Me.txtExtraText, "Enter the Extra Text in the box provided.")
        If Me.txtExtraTextDescription.Text.Length = 0 Then Me.frmErrorProvider.SetError(Me.txtExtraTextDescription, "Enter the Extra Text Description in the box provided.")

    End Sub

    Private Sub btnExtraTxtLink_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtraTxtLink.Click

        ValidateOnSave()
        Me.Close()

    End Sub

    Private Sub btnExtraTxtLookup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtraTxtLookup.Click
        Dim frmChooseExtraText As New ExtraTextLookup
        frmChooseExtraText.CurrentExtraTextRecord = Me.CurrentExtraTextBO
        If frmChooseExtraText.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            If Not Me.CurrentExtraTextBO.ID.Equals(frmChooseExtraText.NewExtraTextBO.ID) Then
                Me.CurrentExtraTextBO = frmChooseExtraText.NewExtraTextBO
                Me.btnExtraTxtLink.Enabled = True
                Me.cmdSave.Enabled = False
                If frmChooseExtraText.LinkBackToIdentifier Then
                    btnExtraTxtLink_Click(sender, e)
                End If
                frmChooseExtraText.Dispose()
            End If
        End If
    End Sub

    Private Sub txtExtraText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtExtraText.TextChanged
        If Not Me._isInitializing Then
            Me.HasChanged = True

            If Not _enableReturnsInExtraText Then
                'Prevent users from pasting in TABs and CR/LF.
                txtExtraText.Text = txtExtraText.Text.Replace(Convert.ToChar(9), "")
                txtExtraText.Text = txtExtraText.Text.Replace(Convert.ToChar(10), "")
                txtExtraText.Text = txtExtraText.Text.Replace(Convert.ToChar(13), "")
            End If
        End If
    End Sub

    Private Sub cmbLabelType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbLabelType.SelectedIndexChanged
        If Not Me._isInitializing Then
            Me.HasChanged = True
        End If
    End Sub

    Private Sub txtExtraTextDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtExtraTextDescription.TextChanged
        If Not Me._isInitializing Then
            Me.HasChanged = True
        End If
    End Sub

    Private Sub btnExtraTxtNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExtraTxtNew.Click
        Dim frmNewExtraText As New ExtraTextNew
        frmNewExtraText.LinkBackToIdentifier = True
        frmNewExtraText.ShowLinkCheckBox = True
        If frmNewExtraText.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Me.CurrentExtraTextBO = frmNewExtraText.NewExtraTextBO
            Me.btnExtraTxtLink.Enabled = True
            Me.cmdSave.Enabled = False
            If frmNewExtraText.LinkBackToIdentifier Then
                btnExtraTxtLink_Click(sender, e)
            End If
            frmNewExtraText.Dispose()
        End If
    End Sub

End Class