Option Strict Off
Option Explicit On
Friend Class frmSelStore
	Inherits System.Windows.Forms.Form

	Private m_bGoodCreate As Boolean
	Private m_lStore_No As Integer

    Public Sub New(ByVal bAllowAllStores As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        m_lStore_No = 0

        CenterForm(Me)

        LoadStores(Me.cmbStore)
        If bAllowAllStores Then
            Me.cmbStore.Items.Add(New VB6.ListBoxItem(ResourcesIRMA.GetString("AllStores"), -1))
            Call SetCombo((Me.cmbStore), IIf(IsDBNull(glStore_Limit), -1, glStore_Limit))
        Else
            Call SetCombo((Me.cmbStore), glStore_Limit)
        End If
    End Sub

    Public ReadOnly Property Store_No() As Integer
        Get
            Store_No = m_lStore_No
        End Get
    End Property
	
	

	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		Me.Hide()
	End Sub
	
	Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click
		
		If cmbStore.SelectedIndex <> -1 Then
			m_lStore_No = VB6.GetItemData(cmbStore, cmbStore.SelectedIndex)
			Me.Hide()
		Else
            MsgBox(ResourcesIRMA.GetString("SelectStore"), MsgBoxStyle.Critical, Me.Text)
		End If
		
	End Sub
	

	
	
	Private Sub frmSelStore_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		
		If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
			Me.Hide()
			Cancel = True
		End If
		
		eventArgs.Cancel = Cancel
	End Sub
	

End Class