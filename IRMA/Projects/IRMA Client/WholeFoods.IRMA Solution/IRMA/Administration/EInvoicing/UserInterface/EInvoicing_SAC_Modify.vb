Imports WholeFoods.IRMA.Replenishment.EInvoicing.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
Imports WholeFoods.IRMA.Administration.EInvoicing.BusinessLogic
Imports WholeFoods.IRMA.Administration.EInvoicing.DataAccess


Public Class EInvoicing_SAC_Modify


    Private _ConfigValue As EInvoicingConfigBO = Nothing
    Private _callingform As Form = Nothing
    Private _hasDataChanged As Boolean = False
    Private _isLoading As Boolean = True
    Private _isNew As Boolean = True




#Region " Constructors "

    Sub New(ByRef _parentform As Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _isNew = True
        _callingform = _parentform
    End Sub


    Sub New(ByVal configvalue As EInvoicingConfigBO, ByRef _parentform As Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _ConfigValue = configvalue
        _isNew = False
        _callingform = _parentform

    End Sub
#End Region

    Private Sub DataChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkAllowance.CheckedChanged, chkSACCode.CheckedChanged, _
                                                                                rdoAllocated.CheckedChanged, rdoNotAllocated.CheckedChanged, rdoLineItem.CheckedChanged, _
                                                                                TextBox_ElementName.TextChanged, TextBox_Label.TextChanged, rdoHeaderElement.CheckedChanged, rdoItemElement.CheckedChanged, rdoSummaryElement.CheckedChanged, CheckBox_Disabled.CheckedChanged, chkAllowance.CheckedChanged
        If Not _isLoading Then
            _hasDataChanged = True
        End If
    End Sub
    Private Sub EInvoiving_SAC_Add_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        LoadSubTeams()

        If Not _ConfigValue Is Nothing Then
            ' load info.

            TextBox_ElementName.Text = _ConfigValue.ElementName
            TextBox_ElementName.Enabled = False
            TextBox_Label.Text = _ConfigValue.Label
            chkAllowance.Checked = _ConfigValue.IsAllowance
            chkSACCode.Checked = _ConfigValue.IsSAC
            If Not _ConfigValue.SubTeam_NO = -1 Then
                ComboBox_SubTeam.SelectedValue = _ConfigValue.SubTeam_NO
            End If

            If _ConfigValue.IsHeaderElement Then
                rdoHeaderElement.Checked = True
            ElseIf _ConfigValue.IsItemElement Then
                rdoItemElement.Checked = True
            Else
                rdoSummaryElement.Checked = True
            End If

            chkAllowance.Checked = _ConfigValue.IsAllowance

            rdoAllocated.Checked = (_ConfigValue.SACType = 1)
            rdoNotAllocated.Checked = (_ConfigValue.SACType = 2)
            rdoLineItem.Checked = (_ConfigValue.SACType = 3)

            CheckBox_Disabled.Checked = _ConfigValue.Disabled



        End If

        SetControls()





        _isLoading = False

    End Sub

    Private Sub LoadSubTeams()
        Dim dao As EinvoicingConfigDAO = New EinvoicingConfigDAO
        Dim dt As DataTable = dao.getSubTeamsDataTable

        Dim dr As DataRow = dt.NewRow
        dr("Display") = String.Empty
        dr("SubTeam_No") = -1
        dt.Rows.InsertAt(dr, 0)

        ComboBox_SubTeam.DataSource = dt
        ComboBox_SubTeam.DisplayMember = "Display"
        ComboBox_SubTeam.ValueMember = "SubTeam_No"



    End Sub

    Private Sub Type_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoAllocated.CheckedChanged, rdoLineItem.CheckedChanged, rdoNotAllocated.CheckedChanged
        ' if "Not Allocated" is chosen
        If chkSACCode.Checked Then
            ComboBox_SubTeam.Enabled = rdoNotAllocated.Checked
        Else
            ComboBox_SubTeam.Enabled = False
        End If


    End Sub

    Private Sub SetControls()
        rdoAllocated.Enabled = chkSACCode.Checked
        rdoLineItem.Enabled = chkSACCode.Checked
        rdoNotAllocated.Enabled = chkSACCode.Checked
        ComboBox_SubTeam.Enabled = chkSACCode.Checked
        chkAllowance.Enabled = chkSACCode.Checked


        ComboBox_SubTeam.Enabled = rdoNotAllocated.Checked And chkSACCode.Checked
    End Sub



    Private Sub chkSACCode_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSACCode.CheckedChanged

        SetControls()


    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        If _hasDataChanged Then
            If MessageBox.Show("Are you sure you want to close this window without saving any changes that have been made?", "Cancel Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.Yes Then
                CType(_callingform, EInvoicing_SAC_Edit).RefreshData()
                Me.Close()
                Me.Dispose()
            End If
        Else
            Me.Close()
            Me.Dispose()
        End If
    End Sub


    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        If Not _isLoading Then
            If _hasDataChanged Then
                SaveChanges()
            End If
        End If
    End Sub


    Private Sub SaveChanges()

        Dim msg As String = String.Empty
        If _ConfigValue Is Nothing Then _ConfigValue = New EInvoicingConfigBO()
        With _ConfigValue
            .ElementName = TextBox_ElementName.Text.Trim
            .Disabled = CheckBox_Disabled.Checked
            If .ElementName = String.Empty Then
                MsgBox("You must enter a value for Element Name.", MsgBoxStyle.Exclamation, "Error")
                TextBox_ElementName.Focus()
                Exit Sub
            End If


            .IsAllowance = chkAllowance.Checked
            .IsSAC = chkSACCode.Checked
            .Label = TextBox_Label.Text.Trim
            If ComboBox_SubTeam.Enabled Then
                If Not ComboBox_SubTeam.SelectedValue Is Nothing Then
                    .SubTeam_NO = DirectCast(ComboBox_SubTeam.SelectedValue, Integer)
                Else
                    If chkSACCode.Checked AndAlso rdoNotAllocated.CheckAlign Then
                        If ComboBox_SubTeam.SelectedValue Is Nothing Then
                            MsgBox("You must select a Subteam/GLAccount for Non Allocated SAC Charges", MsgBoxStyle.Exclamation, "Error")
                            ComboBox_SubTeam.Focus()
                            Exit Sub
                        End If
                    End If
                End If
                If .SubTeam_NO = -1 Then
                    MsgBox("You must select a Subteam/GLAccount for Non Allocated SAC Charges", MsgBoxStyle.Exclamation, "Error")
                    ComboBox_SubTeam.Focus()
                    Exit Sub

                End If

            Else
                .SubTeam_NO = -1
            End If

            If rdoAllocated.Checked And rdoAllocated.Enabled Then
                .SACType = 1
            ElseIf rdoNotAllocated.Checked And rdoNotAllocated.Enabled Then
                .SACType = 2
            ElseIf rdoLineItem.Checked And rdoLineItem.Enabled Then
                .SACType = 3
            Else
                .SACType = -1
            End If

            If rdoHeaderElement.Checked Then
                .IsHeaderElement = True
            ElseIf rdoItemElement.Checked Then
                .IsItemElement = True
            ElseIf rdoSummaryElement.Checked Then
                .IsHeaderElement = False
                .IsItemElement = False
            End If



            .SaveChanges(msg, _isNew)


        End With

        If Not msg.Equals(String.Empty) Then
            MessageBox.Show(msg, "Save Changes", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            If _isNew = True Then _isNew = False
            CType(_callingform, EInvoicing_SAC_Edit).RefreshData()
            Me.Close()
            Me.Dispose()
        End If

    End Sub
End Class