Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports System.Text

Public Class ExtraTextLookup

    Private hasChanged As Boolean
    Private _extraTextID As Integer
    Private _extraText As String
    Private _itemKey As Integer
    Private _newScaleBO As ScaleExtraTextBO
    Private _currScaleBO As ScaleExtraTextBO
    Private _currScaleLabelTypeBO As ScaleLabelTypeBO
    Private _isInitializing As Boolean

    Public Property NewExtraTextBO() As ScaleExtraTextBO
        Get
            Return Me._newScaleBO
        End Get
        Set(ByVal value As ScaleExtraTextBO)
            Me._newScaleBO = value
        End Set
    End Property

    Public Property CurrentExtraTextRecord() As ScaleExtraTextBO
        Get
            Return Me._currScaleBO
        End Get
        Set(ByVal value As ScaleExtraTextBO)
            Me._currScaleBO = value
        End Set
    End Property

    Public Property CurrentScaleLabelType() As ScaleLabelTypeBO
        Get
            Return Me._currScaleLabelTypeBO
        End Get
        Set(ByVal value As ScaleLabelTypeBO)
            Me._currScaleLabelTypeBO = value
        End Set
    End Property

    Public Property LinkBackToIdentifier() As Boolean
        Get
            Return Me._chkLinkBack.Checked
        End Get
        Set(ByVal value As Boolean)
            Me._chkLinkBack.Checked = value
        End Set
    End Property

    Public WriteOnly Property ShowLinkCheckBox() As Boolean
        Set(ByVal value As Boolean)
            Me._chkLinkBack.Visible = value
        End Set
    End Property

    Private Sub Form_ExtraText_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me._isInitializing = True
        Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Me.LoadCombos()
        Windows.Forms.Cursor.Current = Cursors.Default
        Me._isInitializing = False

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Private Sub btnChooseRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseRecord.Click

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub DescriptionCombo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbDescription.SelectedIndexChanged

        If Not Me._isInitializing Then

            Dim _extraTextBO As New ScaleExtraTextBO
            Dim _extraTextLabelStyleBO As New ScaleLabelStyleBO

            _extraTextBO = ScaleExtraTextDAO.GetExtraText(CInt(Me.cmbDescription.SelectedValue))

            Me.cmbLabelType.SelectedValue = _extraTextBO.Scale_LabelType_ID

            _extraTextLabelStyleBO.ID = _extraTextBO.Scale_LabelType_ID
            _extraTextLabelStyleBO.Description = Me.cmbLabelType.Text

            Me.txtExtraText.Text = _extraTextBO.ExtraText
            Me.NewExtraTextBO = _extraTextBO

        End If

    End Sub

    Private Sub LoadCombos()

        ' scale extra text records
        Me.cmbDescription.DataSource = ScaleExtraTextDAO.GetComboListDataTable

        If Me.cmbDescription.Items.Count > 0 Then
            Me.cmbDescription.DisplayMember = "Description"
            Me.cmbDescription.ValueMember = "Scale_ExtraText_ID"
            Me.cmbDescription.SelectedIndex = -1
        End If

        ' load the label type drop-down
        Me.cmbLabelType.DataSource = ScaleLabelTypeDAO.GetComboList()

        If Me.cmbLabelType.Items.Count > 0 Then
            Me.cmbLabelType.DisplayMember = "Description"
            Me.cmbLabelType.ValueMember = "ID"
            Me.cmbLabelType.SelectedIndex = -1
        End If

    End Sub

    Private Sub Clear()

        ' clear out all the existing values
        Me.cmbDescription.SelectedIndex = -1
        Me.cmbLabelType.SelectedIndex = 1
        Me.txtExtraText.Text = String.Empty

    End Sub

    'Private Sub ApplyChanges()
    '    Dim requiredMessage As StringBuilder = New StringBuilder()

    '    If hasChanged Then

    '        If DescriptionTextbox.Text.Length = 0 Then
    '            requiredMessage.Append(String.Format(ResourcesIRMA.GetString("Required"), DescriptionLabel.Text.Replace(":", "")))
    '            requiredMessage.Append(vbLf)
    '        End If
    '        'If LabelTypeCombo.SelectedIndex = -1 Then
    '        '    requiredMessage.Append(String.Format(ResourcesIRMA.GetString("Required"), LabelTypeLabel.Text.Replace(":", "")))
    '        '    requiredMessage.Append(vbLf)
    '        'End If
    '        If ExtraTextBox.Text.Length = 0 Then
    '            requiredMessage.Append(String.Format(ResourcesIRMA.GetString("Required"), ExtraTextLabel.Text.Replace(":", "")))
    '            requiredMessage.Append(vbLf)
    '        End If
    '        If requiredMessage.Length > 0 Then
    '            MsgBox(requiredMessage.ToString(), MsgBoxStyle.Critical, Me.Text)
    '            Exit Sub
    '        End If

    '        Dim scaleExtraText As New ScaleExtraTextBO()
    '        scaleExtraText.Description = DescriptionTextbox.Text
    '        If cmbDescription.SelectedIndex > -1 Then scaleExtraText.ID = CInt(cmbDescription.SelectedValue.ToString())
    '        ' scaleExtraText.Scale_LabelType_ID = CInt(LabelTypeCombo.SelectedValue.ToString())
    '        scaleExtraText.ExtraText = ExtraTextBox.Text

    '        If ScaleExtraTextDAO.Save(scaleExtraText) Then
    '            ' reload the combo
    '            'If ExtraTextID = 0 Then
    '            '    LoadCombo(True)
    '            'Else
    '            '    LoadCombo(False)
    '            'End If

    '            ' select the most recently added/edited item
    '            If cmbDescription.Items.Count > 0 Then
    '                Dim index As Integer
    '                For Each item As ScaleExtraTextBO In cmbDescription.Items
    '                    If item.Description = scaleExtraText.Description Then
    '                        cmbDescription.SelectedIndex = index
    '                        ' set the ID so the calling screen can query it
    '                        _extraTextID = scaleExtraText.ID
    '                        _extraText = scaleExtraText.Description
    '                        Exit For
    '                    End If
    '                    index = index + 1
    '                Next
    '            End If
    '        Else
    '            ' Indicate that this is a duplicate
    '            MsgBox(String.Format(ResourcesIRMA.GetString("Duplicate"), DescriptionTextbox.Text), MsgBoxStyle.Critical, Me.Text)
    '            Clear()
    '        End If

    '        scaleExtraText = Nothing
    '        hasChanged = False
    '    End If

    'End Sub

End Class