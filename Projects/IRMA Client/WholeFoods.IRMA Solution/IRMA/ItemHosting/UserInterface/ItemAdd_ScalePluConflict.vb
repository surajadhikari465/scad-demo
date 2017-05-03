Imports System.Text
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic

Public Class ItemAdd_ScalePluConflict

    Private _isShowOption_DoNotSendToScale As Boolean
    Private _pluConflicts As ArrayList
    Private _selectedOption As Integer
    Private _identifierListAlternateCancelMsg As Boolean

#Region "properties"

    Public Property IsShowOption_DoNotSendToScale() As Boolean
        Get
            Return _isShowOption_DoNotSendToScale
        End Get
        Set(ByVal value As Boolean)
            _isShowOption_DoNotSendToScale = value
        End Set
    End Property

    Public Property PluConflicts() As ArrayList
        Get
            Return _pluConflicts
        End Get
        Set(ByVal value As ArrayList)
            _pluConflicts = value
        End Set
    End Property

    Public Property SelectedOption() As Integer
        Get
            Return _selectedOption
        End Get
        Set(ByVal value As Integer)
            _selectedOption = value
        End Set
    End Property

    Public Property IdentifierListAlternateCancelMsg() As Boolean
        Get
            Return _identifierListAlternateCancelMsg
        End Get
        Set(ByVal value As Boolean)
            _identifierListAlternateCancelMsg = value
        End Set
    End Property

#End Region

    Private Sub ItemAdd_ScalePluConflict_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.CenterToParent()

        BindDataGrid()

        SetupMessageOptions()
    End Sub

    Private Sub ItemAdd_ScalePluConflict_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not IsSelectionValid() Then
            e.Cancel = True
        End If
    End Sub

    Private Sub BindDataGrid()
        'load plu conflict items to grid
        Me.UltraGrid_PluConflicts.DataSource = _pluConflicts
    End Sub

    Private Sub SetupMessageOptions()
        'msg options
        '* Cancel and enter a new PLU (alternate msg = 'Cancel changes to this Identifier')
        '* Keep this PLU but do not send it to the scales  (ONLY IF regional setting for "do not send to scales" is ON)
        '* Send 5 digits for this PLU                      (ONLY IF regional InstanceData is NOT "ALWAYS 4")
        If _identifierListAlternateCancelMsg Then
            Me.RadioButton_CancelSave.Text = ResourcesItemHosting.GetString("msg_optionText_PluConflictCancelSave")
        End If

        If Not _isShowOption_DoNotSendToScale Then
            Me.RadioButton_DoNotSendToScale.Visible = False
        End If

        If gsPluDigitsSentToScale.Equals("ALWAYS 4") Then
            Me.RadioButton_Send5Digits.Visible = False
        End If

        If Not _isShowOption_DoNotSendToScale AndAlso gsPluDigitsSentToScale.Equals("ALWAYS 4") Then
            'only option is option 1
            Me.RadioButton_CancelSave.Checked = True
        End If
    End Sub

    Private Sub Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        'verify that user has selected an option
        If IsSelectionValid() Then
            'note selected item
            Select Case True
                Case Me.RadioButton_CancelSave.Checked
                    _selectedOption = 1
                Case Me.RadioButton_DoNotSendToScale.Checked
                    _selectedOption = 2
                Case Me.RadioButton_Send5Digits.Checked
                    _selectedOption = 3
            End Select

            Me.Close()
        End If
    End Sub

    Private Function IsSelectionValid() As Boolean
        Dim valid As Boolean = True

        If Not Me.RadioButton_CancelSave.Checked AndAlso _
                   Not Me.RadioButton_DoNotSendToScale.Checked AndAlso _
                   Not Me.RadioButton_Send5Digits.Checked Then
            MessageBox.Show(ResourcesItemHosting.GetString("SelectItem"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)

            valid = False
        End If

        Return valid
    End Function
End Class