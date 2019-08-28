Imports System.Linq

Public Class SubteamComboBox
	Dim subteamList As List(Of SubTeamBO) = Nothing
	Dim displayName As String
	Dim valueName As String

	Public Property IsShowAll As Boolean
		Get
			Return chk.Checked
		End Get
		Set(value As Boolean)
			chk.Checked = value
		End Set
	End Property

	Public Property CheckboxFont As Font
		Get
			Return chk.Font
		End Get
		Set(value As Font)
			chk.Font = value
		End Set
	End Property

	Public Property CheckboxForeColor As Color
		Get
			Return chk.ForeColor
		End Get
		Set(value As Color)
			chk.ForeColor = value
		End Set
	End Property

	Public Property CheckboxText As String
		Get
			Return chk.Text
		End Get
		Set(value As String)
			chk.Text = value.TrimEnd()
		End Set
	End Property

	Public Property DataSource As List(Of SubTeamBO)
		Get
			Return subteamList
		End Get
		Set(value As List(Of SubTeamBO))
			subteamList = value
			RefreshItems()
		End Set
	End Property

	Public Property DisplayMember As String
		Get
			Return displayName
		End Get
		Set(value As String)
			displayName = value
			RefreshItems()
		End Set
	End Property

	Public Property DropDownWidth As Integer
		Get
			Return cmb.DropDownWidth
		End Get
		Set(value As Integer)
			cmb.DropDownWidth = value
		End Set
	End Property

	Public Property ValueMember As String
		Get
			Return valueName
		End Get
		Set(value As String)
			valueName = value
			RefreshItems()
		End Set
	End Property

	Public Property HeaderText As String
		Get
			Return lbl.Text
		End Get
		Set(value As String)
			lbl.Text = value.TrimEnd()
		End Set
	End Property

	Public Property HeaderVisible As Boolean
		Get
			Return lbl.Visible
		End Get
		Set(value As Boolean)
			lbl.Visible = value
		End Set
	End Property

	Public Property HeaderFont As Font
		Get
			Return lbl.Font
		End Get
		Set(value As Font)
			lbl.Font = value
		End Set
	End Property

	Public Property HeaderForeColor As Color
		Get
			Return lbl.ForeColor
		End Get
		Set(value As Color)
			lbl.ForeColor = value
		End Set
	End Property

	Public ReadOnly Property Items As ComboBox.ObjectCollection
		Get
			Return cmb.Items
		End Get
	End Property

	Public Property SelectedIndex As Integer
		Get
			Return cmb.SelectedIndex
		End Get
		Set(value As Integer)
			cmb.SelectedIndex = value
		End Set
	End Property

	Public Property SelectedItem As SubTeamBO
		Get
			Return If(cmb.SelectedItem Is Nothing, Nothing, CType(cmb.SelectedItem, SubTeamBO))
		End Get
		Set(value As SubTeamBO)
			cmb.SelectedItem = If(cmb.DataSource Is Nothing, Nothing, If(CType(cmb.DataSource, List(Of SubTeamBO)).Contains(value), value, Nothing))
		End Set
	End Property

	Public Property SelectedValue As Object
		Get
			Return cmb.SelectedValue
		End Get
		Set(value As Object)
			cmb.SelectedValue = value
		End Set
	End Property

	Public Property SelectedText As String
		Get
			Return cmb.Text
		End Get
		Set(value As String)
			cmb.Text = value
		End Set
	End Property

	Public Property ClearSelectionVisisble As Boolean
		Get
			Return pic.Visible
		End Get
		Set(value As Boolean)
			pic.Visible = value
		End Set
	End Property

	Private Sub chk_Click(sender As Object, e As EventArgs) Handles chk.Click
		RefreshItems()
	End Sub

	Public Event DropDown(sender As Object, e As EventArgs)
	Public Event SelectedIndexChanged(sender As Object, e As EventArgs)

	Private Sub cmb_DropDown(sender As Object, e As EventArgs) Handles cmb.DropDown
		RaiseEvent DropDown(Me, e)
	End Sub

	Private Sub cmbData_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmb.SelectedIndexChanged
		RaiseEvent SelectedIndexChanged(Me, e)
	End Sub

	Private Sub pic_Click(sender As Object, e As EventArgs) Handles pic.Click
		cmb.SelectedItem = Nothing
	End Sub

	Private Sub RefreshItems()
		Dim item As SubTeamBO = cmb.SelectedItem

		If String.IsNullOrEmpty(displayName) Then displayName = "SubTeamName"
		If String.IsNullOrEmpty(valueName) Then valueName = "SubTeamNo"

		cmb.DataSource = If(chk.Checked OrElse subteamList Is Nothing, subteamList, subteamList.Where(Function(x) x.AlignedSubTeam OrElse Not x.IsDisabled).ToList())

		cmb.ValueMember = valueName
		cmb.DisplayMember = displayName
		If (item IsNot Nothing AndAlso cmb.DataSource IsNot Nothing) Then
			cmb.SelectedItem = If(CType(cmb.DataSource, List(Of SubTeamBO)).Contains(item), item, Nothing)
		End If
	End Sub

	Private Sub cmb_KeyUp(sender As Object, e As KeyEventArgs) Handles cmb.KeyUp
		If e.KeyCode = Keys.Escape Then cmb.SelectedItem = Nothing
	End Sub
End Class