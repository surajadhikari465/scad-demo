Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.Utility
Imports System.Text.RegularExpressions
Imports log4net

Public Class SubTeamEdit
	Private _dataset As DataSet
	Private isDisabled As Boolean = False
	' Define the log4net logger for this class.
	Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	Public Property IsNewSubTeam() As Boolean
	Public Property SubTeamNo() As Integer

	Sub New(ByVal SubTeam_NO As Integer)

		logger.Debug("New Entry with SubTeam_NO=" + SubTeam_NO.ToString)

		' This call is required by the Windows Form Designer.
		InitializeComponent()

		Me.SubTeamNo = SubTeam_NO
		Me.IsNewSubTeam = (SubTeam_NO = -1)

		logger.Debug("New Exit")

	End Sub

	Private Sub SubTeamEdit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		logger.Debug("SubTeamEdit_Load Entry")

		LoadTeams()
		LoadSubTeamTypes()

		If Not Me.IsNewSubTeam Then
			Me.Text = "Edit A SubTeam"
			' Load Existing team data.
			LoadSubteamInfo()
			TextBox_SubTeamNo.Enabled = False
			'the user should only be able to check the "create dist subteam" box
			' ...it should not be available to be un-checked
			If Me.CheckBox_Distribution.Checked = True Then
				Me.CheckBox_Distribution.Enabled = False
			Else : Me.CheckBox_Distribution.Enabled = True
			End If
		Else
			Me.Text = "Create A New SubTeam"
			TextBox_SubTeamNo.Enabled = True
			Me.CheckBox_Distribution.Checked = True
		End If

		Me.SetFieldsStates()
		logger.Debug("SubTeamEdit_Load Exit")
	End Sub

	Private Sub LoadTeams()
		logger.Debug("LoadTeams Entry")

		Dim DAO As TeamDAO = New TeamDAO
		_dataset = DAO.GetTeams()
		ComboBox_Team.Items.Clear()
		For Each dr As DataRow In _dataset.Tables(0).Rows
			ComboBox_Team.Items.Add(New TeamBO(CInt(dr("Team_No").ToString), dr("Team_Name").ToString, dr("Team_Abbreviation").ToString))
		Next
		_dataset.Dispose()
		ComboBox_Team.DisplayMember = "TeamName"
		ComboBox_Team.ValueMember = "TeamNo"

		logger.Debug("LoadTeams Exit")
	End Sub

	Private Sub LoadSubTeamTypes()

		Dim DAO As SubTeamDAO = New SubTeamDAO

		ComboBox_SubTeamTypeId.DataSource = DAO.GetSubTeamTypes()
		ComboBox_SubTeamTypeId.DisplayMember = "Description"
		ComboBox_SubTeamTypeId.ValueMember = "ID"
		ComboBox_SubTeamTypeId.SelectedIndex = -1

	End Sub

	Private Sub LoadSubteamInfo()
		logger.Debug("LoadSubteamInfo Entry")

		Dim DAO As SubTeamDAO = New SubTeamDAO
		Dim dr As DataRow = Nothing
		Dim STASupportKey As String
		Dim STASupportGroups As String()
		Dim SearchPattern As String = "\|"
		Dim UserGroupFound As Boolean = False

		_dataset = DAO.GetSubTeam(Me.SubTeamNo)
		dr = _dataset.Tables(0).Rows(0)

		TextBox_SubTeamNo.Text = dr("SubTeam_No").ToString().Trim                   'required
		TextBox_SubteamName.Text = dr("SubTeam_Name").ToString().Trim
		TextBox_SubteamAbbrev.Text = dr("SubTeam_Abbreviation").ToString().Trim
		TextBox_DepartmentNo.Text = dr("Dept_No").ToString().Trim
		TextBox_SubDeptNo.Text = dr("SubDept_No").ToString().Trim
		TextBox_BuyerUserId.Text = dr("Buyer_User_ID").ToString().Trim
		TextBox_TargetMargin.Text = dr("Target_Margin").ToString().Trim             'required
		TextBox_JDA.Text = dr("jda").ToString().Trim
		TextBox_GLPurchaseAccount.Text = dr("GLPurchaseAcct").ToString().Trim
		TextBox_GLDistributionAccont.Text = dr("GLDistributionAcct").ToString().Trim
		TextBox_GLTransferAccount.Text = dr("GLTransferAcct").ToString().Trim
		TextBox_GLSalesAccount.Text = dr("GLSalesAcct").ToString().Trim
		TextBox_GLPackagingAccount.Text = dr("GLPackagingAcct").ToString().Trim
		TextBox_GLSuppliesAccount.Text = dr("GLSuppliesAcct").ToString().Trim
		TextBox_TransferToMarkup.Text = dr("Transfer_To_Markup").ToString().Trim
		TextBox_ScaleDept.Text = dr("ScaleDept").ToString().Trim
		ComboBox_SubTeamTypeId.SelectedValue = dr("SubTeamType_Id")
		CheckBox_EXEWarehouseSent.Checked = (dr("EXEWarehouseSent").ToString().Equals("True") Or dr("EXEWarehouseSent").ToString().Equals("1"))
		CheckBox_EXEDistributed.Checked = (dr("EXEDistributed").ToString().Equals("True") Or dr("EXEDistributed").ToString().Equals("1"))
		CheckBox_Retail.Checked = (dr("Retail").ToString().Equals("True") Or dr("Retail").ToString().Equals("1"))
		CheckBox_InventoryCountByCase.Checked = (dr("InventoryCountByCase").ToString().Equals("True") Or dr("InventoryCountByCase").ToString().Equals("1"))
		CheckBox_Distribution.Checked = (dr("Distribution").ToString.Equals("True") Or dr("Distribution").ToString().Equals("1"))
		CheckBox_FixedSpoilage.Checked = (dr("FixedSpoilage").ToString().Equals("True") Or dr("FixedSpoilage").ToString().Equals("1"))
		CheckBox_Beverage.Checked = (dr("Beverage").ToString().Equals("True") Or dr("Beverage").ToString().Equals("1"))
		CheckBox_AlignedSubTeam.Checked = If(IsDBNull(dr!AlignedSubTeam), False, CBool(dr!AlignedSubTeam))
		isDisabled = If(IsDBNull(dr!IsDisabled), False, CBool(dr!IsDisabled))
		chkDisable.Checked = isDisabled
		chkDisable.Enabled = Not CheckBox_AlignedSubTeam.Checked

		For Each team As TeamBO In ComboBox_Team.Items
			If team.TeamNo = CInt(dr("Team_No")) Then
				ComboBox_Team.SelectedItem = team
			End If
		Next

		If Not IsNothing(ConfigurationServices.AppSettings("STASupport")) Then
			STASupportKey = Trim(ConfigurationServices.AppSettings("STASupport").ToString)
			STASupportGroups = Regex.Split(STASupportKey, SearchPattern)

			Dim UserAccess As New UserAccessBO

			For Each SupportGroup As String In STASupportGroups
				If UserAccess.IsInGroup(SupportGroup) Then
					UserGroupFound = True
					Exit For
				End If
			Next

			If UserGroupFound Then
				CheckBox_AlignedSubTeam.Visible = True
			Else
				CheckBox_AlignedSubTeam.Visible = False
			End If
		Else
			CheckBox_AlignedSubTeam.Visible = False
		End If

		_dataset.Dispose()

		logger.Debug("LoadSubteamInfo Exit")
	End Sub

	Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
		Me.Close()
	End Sub

	Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click
		logger.Debug("Button_Save_Click Entry")

		' Validate and save.   
		Try
			If ValidateData() Then
				Dim SubTeam As SubTeamBO = CreateSubTeamObject()
				SaveChanges(SubTeam)
				Me.Close()
			End If
		Catch ex As Exception
			Dim msg As String = ex.Message
			If Not ex.InnerException Is Nothing Then
				msg = msg & vbCrLf & vbCrLf & ex.InnerException.Message
			End If
			MsgBox("Your Changes have not been saved. An error occurred." & vbCrLf & msg)
			logger.Info("Your Changes have not been saved. An error occurred.")

		End Try

		logger.Debug("Button_Save_Click Exit")
	End Sub

	Private Function ValidateData() As Boolean
		Dim intResult = 0
		Dim snglResult = 0
		Dim retval As Boolean = True
		Dim DAO As SubTeamDAO = New SubTeamDAO

		Try
			' --- SubTeam_No
			If Not (Integer.TryParse(TextBox_SubTeamNo.Text.Trim, intResult) OrElse intResult <= 0) Then Throw New Exception("SubTeam must be a valid Integer value")

			If Me.IsNewSubTeam Then
				_dataset = DAO.GetSubTeam(intResult)
				If _dataset.Tables(0).Rows.Count > 0 Then
					Throw New Exception("A SubTeam (" & _dataset.Tables(0).Rows(0)("SubTeam_Name").ToString.Trim & ") arleady exists with that SubTeam Number.")
				End If
			End If

			' --- Team
			If ComboBox_Team.SelectedItem Is Nothing Then Throw New Exception("You must choose a default Team for this SubTeam")
			' --- SubTeam Name
			If TextBox_SubteamName.Text.Trim.Equals("") Then Throw New Exception("SubTeam Name must not be blank")
			' --- SubTeam Abbreviation
			If TextBox_SubteamAbbrev.Text.Trim.Equals("") Then Throw New Exception("SubTeam Abbreviation must not be blank")

			' -- JDA
			If Not TextBox_JDA.Text.Trim.Equals("") And Not IsNumeric(TextBox_JDA.Text.Trim) Then
				Throw New Exception("JDA must be a valid Integer value greater than 0 or blank.")
			End If
			If Not TextBox_JDA.Text.Trim.Equals("") Then
				If Not CInt(TextBox_JDA.Text.Trim) > 0 Then
					Throw New Exception("JDA must be a valid Integer value greater than 0 or blank.")
				End If
			End If

			' -- Department No
			If Not TextBox_DepartmentNo.Text.Trim.Equals("") And Not IsNumeric(TextBox_DepartmentNo.Text.Trim) Then
				Throw New Exception("Department Number must be a valid Integer value greater than 0 or blank.")
			End If
			If Not TextBox_DepartmentNo.Text.Trim.Equals("") Then
				If Not CInt(TextBox_DepartmentNo.Text.Trim) > 0 Then
					Throw New Exception("Department Number must be a valid Integer value greater than 0 or blank.")
				End If
			End If

			' -- SubDepartment No
			If Not TextBox_SubDeptNo.Text.Trim.Equals("") And Not IsNumeric(TextBox_SubDeptNo.Text.Trim) Then
				Throw New Exception("Sub Department Number must be a valid Integer value greater than 0 or blank.")
			End If
			If Not TextBox_SubDeptNo.Text.Trim.Equals("") Then
				If Not CInt(TextBox_SubDeptNo.Text.Trim) > 0 Then
					Throw New Exception("Sub Department Number must be a valid Integer value greater than 0 or blank.")
				End If
			End If
			'-- BuyerUserId
			If Not TextBox_BuyerUserId.Text.Trim.Equals("") And Not IsNumeric(TextBox_BuyerUserId.Text.Trim) Then
				Throw New Exception("Buyer UserId must be a valid Integer value greater than 0 or blank.")
			End If
			If Not TextBox_BuyerUserId.Text.Trim.Equals("") Then
				If Not CInt(TextBox_BuyerUserId.Text.Trim) > 0 Then
					Throw New Exception("Buyer UserId must be a valid Integer value greater than 0 or blank.")
				End If
			End If
			'-- target margin
			If (Not Single.TryParse(TextBox_TargetMargin.Text.Trim, snglResult) OrElse snglResult <= 0) Then
				Throw New Exception("Target Margin must be a valid decimal value greater than 0.")
			End If

			' --  GLPurchase
			If Not TextBox_GLPurchaseAccount.Text.Trim.Equals("") And Not IsNumeric(TextBox_GLPurchaseAccount.Text.Trim) Then
				Throw New Exception("GLPurchaseAccount must be a valid Integer value greater than 0 or blank.")
			End If
			If Not TextBox_GLPurchaseAccount.Text.Trim.Equals("") Then
				If Not CInt(TextBox_GLPurchaseAccount.Text.Trim) > 0 Then
					Throw New Exception("GLPurchaseAccount must be a valid Integer value greater than 0 or blank.")
				End If
			End If

			' --  GLDistribution
			If Not TextBox_GLDistributionAccont.Text.Trim.Equals("") And Not IsNumeric(TextBox_GLDistributionAccont.Text.Trim) Then
				Throw New Exception("GLDistributionAccount must be a valid Integer value greater than 0 or blank.")
			End If
			If Not TextBox_GLDistributionAccont.Text.Trim.Equals("") Then
				If Not CInt(TextBox_GLDistributionAccont.Text.Trim) > 0 Then
					Throw New Exception("GLDistributionAccount must be a valid Integer value greater than 0 or blank.")
				End If
			End If

			' --  GLTransfer
			If Not TextBox_GLTransferAccount.Text.Trim.Equals("") And Not IsNumeric(TextBox_GLTransferAccount.Text.Trim) Then
				Throw New Exception("GLTransferAccount must be a valid Integer value greater than 0 or blank.")
			End If
			If Not TextBox_GLTransferAccount.Text.Trim.Equals("") Then
				If Not CInt(TextBox_GLTransferAccount.Text.Trim) > 0 Then
					Throw New Exception("GLTransferAccount must be a valid Integer value greater than 0 or blank.")
				End If
			End If

			' --  GLSales
			If Not TextBox_GLSalesAccount.Text.Trim.Equals("") And Not IsNumeric(TextBox_GLSalesAccount.Text.Trim) Then
				Throw New Exception("GLSalesAccount must be a valid Integer value greater than 0 or blank.")
			End If
			If Not TextBox_GLSalesAccount.Text.Trim.Equals("") Then
				If Not CInt(TextBox_GLSalesAccount.Text.Trim) > 0 Then
					Throw New Exception("GLSalesAccount must be a valid Integer value greater than 0 or blank.")
				End If
			End If
			' --  TransferToMarkup
			If Not TextBox_TransferToMarkup.Text.Trim.Equals("") And Not IsNumeric(TextBox_ScaleDept.Text.Trim) Then
				Throw New Exception("Transfer To Markup must be a valid decimal value greater than 0 or blank.")
			End If
			If Not TextBox_TransferToMarkup.Text.Trim.Equals("") Then
				If Not CSng(TextBox_TransferToMarkup.Text.Trim) > 0 Then
					Throw New Exception("Transfer To Markup must be a valid decimal value greater than 0 or blank.")
				End If
			End If
			' --  Scale Dept
			If Not TextBox_ScaleDept.Text.Trim.Equals("") And Not IsNumeric(TextBox_ScaleDept.Text.Trim) Then
				Throw New Exception("Scale Department must be a valid Integer value greater than 0 or blank.")
			End If
			If Not TextBox_ScaleDept.Text.Trim.Equals("") Then
				If Not CInt(TextBox_ScaleDept.Text.Trim) > 0 Then
					Throw New Exception("Scale Department must be a valid Integer value greater than 0 or blank.")
				End If
			End If

			If Me.ComboBox_SubTeamTypeId.SelectedIndex = -1 Then
				Throw New Exception("You must select a valid Sub Team Type.")
			End If

			If CheckBox_AlignedSubTeam.Checked AndAlso chkDisable.Checked Then Throw New Exception("Aligned Subteam can not be marked as Disabled/ Hidden. Please change Aligned Subteam and/or Disable Subteam options accordingly.")
		Catch ex As Exception
			MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
			logger.Error(ex.Message)
			retval = False
		Finally
			If Not _dataset Is Nothing Then _dataset.Dispose()
		End Try

		Return retval
	End Function

	Private Function CreateSubTeamObject() As SubTeamBO
		logger.Debug("CreateSubTeamObject Entry")

		Dim item As SubTeamBO = New SubTeamBO

		Try
			item.SubTeamNo = CInt(TextBox_SubTeamNo.Text.Trim)
			item.SubTeamName = TextBox_SubteamName.Text.Trim
			item.SubTeamAbbreviation = TextBox_SubteamAbbrev.Text.Trim
			item.DeptNo = ToNullableInt(TextBox_DepartmentNo.Text)
			item.TeamNo = CType(ComboBox_Team.SelectedItem, TeamBO).TeamNo
			item.BuyerUserId = ToNullableInt(TextBox_BuyerUserId.Text)
			item.SubDeptNo = ToNullableInt(TextBox_SubDeptNo.Text)
			item.TargetMargin = CSng(TextBox_TargetMargin.Text.Trim())
			item.JDA = ToNullableInt(TextBox_JDA.Text)
			item.TransferToMarkup = ToNullableSingle(TextBox_TransferToMarkup.Text)
			item.EXEDistributed = CheckBox_EXEDistributed.Checked
			item.EXEWarehouseSent = CheckBox_EXEWarehouseSent.Checked
			item.Retail = CheckBox_Retail.Checked
			item.FixedSpoilage = CheckBox_FixedSpoilage.Checked
			item.Beverage = CheckBox_Beverage.Checked
			item.AlignedSubTeam = CheckBox_AlignedSubTeam.Checked
			item.GLDistributionAcct = ToNullableInt(TextBox_GLDistributionAccont.Text)
			item.GLPurchaseAcct = ToNullableInt(TextBox_GLPurchaseAccount.Text)
			item.GLSalesAcct = ToNullableInt(TextBox_GLSalesAccount.Text)
			item.GLTransferAcct = ToNullableInt(TextBox_GLTransferAccount.Text)
			item.GLPackagingAcct = ToNullableInt(TextBox_GLPackagingAccount.Text)
			item.GLSuppliesAcct = ToNullableInt(TextBox_GLSuppliesAccount.Text)
			item.ScaleDept = ToNullableInt(TextBox_ScaleDept.Text)
			item.SubTeamTypeId = CInt(ComboBox_SubTeamTypeId.SelectedValue)
			item.Distribution = CheckBox_Distribution.Checked
			item.InventoryCountByCase = CheckBox_InventoryCountByCase.Checked
			item.IsDisabled = chkDisable.Checked AndAlso Not CheckBox_AlignedSubTeam.Checked
		Catch ex As Exception
			Dim msg As String = "SubTeam Object could not be created. Check the form data."
			Throw New Exception(msg, ex)
			logger.Info(msg)
		End Try

		logger.Debug("CreateSubTeamObject Exit")
		Return item
	End Function

	Private Sub SaveChanges(ByRef SubTeam As SubTeamBO)
		Dim DAO As SubTeamDAO = New SubTeamDAO
		DAO.SaveSubTeam(SubTeam, Me.IsNewSubTeam)
	End Sub
	Private Sub SetFieldsStates()
		If InstanceDataDAO.IsFlagActive("UKIPS", glStoreID) Then
			Me.ComboBox_Team.Enabled = False
			Me.TextBox_SubteamName.Enabled = False
			Me.TextBox_DepartmentNo.Enabled = False
		Else
			Me.ComboBox_Team.Enabled = True
			Me.TextBox_SubteamName.Enabled = True
			Me.TextBox_DepartmentNo.Enabled = True
		End If
	End Sub

	Private Sub CheckBox_AlignedSubTeam_Click(sender As Object, e As EventArgs) Handles CheckBox_AlignedSubTeam.Click
		chkDisable.Enabled = Not CheckBox_AlignedSubTeam.Checked
		If CheckBox_AlignedSubTeam.Checked Then chkDisable.Checked = False
	End Sub

	Private Function ToNullableInt(ByVal value As String) As Integer?
		Dim result As Integer

		If (Integer.TryParse(value.Trim(), result) AndAlso result > 0) Then
			Return result
		Else
			Return Nothing
		End If
	End Function

	Private Function ToNullableSingle(ByVal value As String) As Single?
		Dim result As Integer

		If (Single.TryParse(value.Trim(), result) AndAlso result > 0) Then
			Return result
		Else
			Return Nothing
		End If
	End Function
End Class