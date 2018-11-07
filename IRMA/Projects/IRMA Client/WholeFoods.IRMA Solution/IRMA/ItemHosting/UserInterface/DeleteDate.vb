Option Strict Off
Option Explicit On
Friend Class frmDeleteDate
	Inherits System.Windows.Forms.Form
	
	Private mbCancelled As Boolean
	
	Public ReadOnly Property Cancelled() As Boolean
		Get
			
			Cancelled = mbCancelled
			
		End Get
	End Property
	
	Public Property DeleteDate() As Date
		Get
            DeleteDate = dtpStartDate.Value
        End Get

        Set(ByVal Value As Date)
            dtpStartDate.Value = Value
        End Set
    End Property

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        mbCancelled = True

        Me.Hide()

    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click

        Me.Hide()

    End Sub

    Private Sub frmDeleteDate_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        CenterForm(Me)

        dtpStartDate.Value = System.DateTime.Today

    End Sub
	
	Private Sub frmDeleteDate_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		
		If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
			mbCancelled = True
			Cancel = True
			Me.Hide()
		End If
		
		eventArgs.Cancel = Cancel
	End Sub
	
	Private Sub frmDeleteDate_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		
		mbCancelled = False

    End Sub
	
End Class