Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net


Public Class StoreSubTeamEdit

    Private _IsNew As Boolean
    Private _Store_No As Integer
    Private _Team_No As Integer
    Private _SubTeam_No As Integer
    Private _PS_Team_No As Integer
    Private _PS_SubTeam_No As Integer
    Private _CostFactor As Decimal
    Private _ICVID As Integer

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Sub New(ByVal StoreNo As Integer)

        logger.Debug("New Entry with StoreNo=" + StoreNo.ToString)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.StoreNo = StoreNo
        Me.SubTeamNo = -1
        Me.TeamNo = -1
        Me.PS_SubTeamNo = -1
        Me.PS_TeamNo = -1
        Me.CostFactor = 0.0
        Me.ICVID = -1
        Me.IsNew = True
        logger.Debug("New exit")

    End Sub

    Sub New(ByVal StoreNo As Integer, ByVal SubTeam_No As Integer, ByVal Team_No As Integer, ByVal PS_SubTeam_No As Integer, ByVal PS_Team_No As Integer, ByVal CostFactor As Decimal, ByVal ICVID As Integer)

        logger.Debug("New Entry with StoreNo= " + StoreNo.ToString + ", SubTeam_No = " & SubTeam_No.ToString & ",Team_No= " + Team_No.ToString + ",PS_SubTeam_No = " + PS_SubTeam_No.ToString + ",PS_Team_No = " + PS_Team_No.ToString)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.StoreNo = StoreNo
        Me.SubTeamNo = SubTeam_No
        Me.TeamNo = Team_No
        Me.PS_SubTeamNo = PS_SubTeam_No
        Me.PS_TeamNo = PS_Team_No
        Me.IsNew = False
        Me.CostFactor = CostFactor
        Me.ICVID = ICVID

        logger.Debug("New Exit")
    End Sub


#Region "Property Accessors"
    Public Property IsNew() As Boolean
        Get
            Return _IsNew
        End Get
        Set(ByVal value As Boolean)
            _IsNew = value
        End Set
    End Property

    Public Property StoreNo() As Integer
        Get
            Return _Store_No
        End Get
        Set(ByVal value As Integer)
            _Store_No = value
        End Set
    End Property


    Public Property TeamNo() As Integer
        Get
            Return _Team_No
        End Get
        Set(ByVal value As Integer)
            _Team_No = value
        End Set
    End Property

    Public Property SubTeamNo() As Integer
        Get
            Return _SubTeam_No
        End Get
        Set(ByVal value As Integer)
            _SubTeam_No = value
        End Set
    End Property

    Public Property PS_TeamNo() As Integer
        Get
            Return _PS_Team_No
        End Get
        Set(ByVal value As Integer)
            _PS_Team_No = value
        End Set
    End Property

    Public Property PS_SubTeamNo() As Integer
        Get
            Return _PS_SubTeam_No
        End Get
        Set(ByVal value As Integer)
            _PS_SubTeam_No = value
        End Set
    End Property

    Public Property CostFactor() As Decimal
        Get
            Return _CostFactor
        End Get
        Set(value As Decimal)
            _CostFactor = value
        End Set
    End Property

    Public Property ICVID() As Integer
        Get
            Return _ICVID
        End Get
        Set(value As Integer)
            _ICVID = value
        End Set
    End Property
#End Region


    Private Sub StoreSubTeamEdit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        logger.Debug("StoreSubTeamEdit_Load Entry")

        If Me.IsNew Then
            Me.Text = "Add SubTeam / Team Relationship"
        Else
            Me.Text = "Edit SubTeam / Team Relationship"
        End If
        LoadStoreData()
        LoadSubTeamData()
        LoadTeamData()
        LoadICVData()
        SetValues()
        SetFieldsStates()

        logger.Debug("StoreSubTeamEdit_Load Exit")
    End Sub

    Private Sub SetValues()

        logger.Debug("SetValues Entry")
        For Each SubTeam As SubTeamBO In ComboBox_SubTeam.Items
            If SubTeam.SubTeamNo.ToString.Equals(Me.SubTeamNo.ToString) Then
                ComboBox_SubTeam.SelectedItem = SubTeam
            End If
        Next

        ComboBox_SubTeam.Enabled = (Me.SubTeamNo = -1)
        If Me.TeamNo <> -1 Then
            For Each Team As TeamBO In ComboBox_Team.Items
                If Team.TeamNo.ToString.Equals(Me.TeamNo.ToString) Then
                    ComboBox_Team.SelectedItem = Team
                End If
            Next
        End If

        If Me.PS_TeamNo <> -1 Then
            TextBox_PSTeamNo.Text = Me.PS_TeamNo.ToString
        End If
        If Me.PS_SubTeamNo <> -1 Then
            TextBox_PSSubTeamNo.Text = Me.PS_SubTeamNo.ToString
        End If

        TextBox_CostFactor.Text = Me.CostFactor.ToString

        For Each ICV As VendorBO In ComboBox_ICVID.Items
            If ICV.VendorID = Me.ICVID Then
                ComboBox_ICVID.SelectedItem = ICV
            End If
        Next

        logger.Debug("SetValues Exit")


    End Sub

    Private Sub LoadStoreData()
        logger.Debug("LoadStoreData Entry")

        Dim Dt As DataTable = StoreDAO.GetStoreList()
        For Each dr As DataRow In Dt.Rows
            If dr("Store_No").ToString = Me.StoreNo.ToString Then
                lblStoreName.Text = dr("Store_Name").ToString.Trim
            End If
        Next

        logger.Debug("LoadStoreData Exit")

    End Sub

    Private Sub LoadSubTeamData()

        logger.Debug("LoadSubTeamData Entry")

        Dim DAO As SubTeamDAO = New SubTeamDAO
        Dim _dataset As DataSet = DAO.GetSubteamsSet()
        ComboBox_SubTeam.Items.Clear()
        For Each dr As DataRow In _dataset.Tables(0).Rows
            ComboBox_SubTeam.Items.Add(New SubTeamBO(CInt(dr("SubTeam_No").ToString), dr("SubTeam_Name").ToString))
        Next

        ComboBox_SubTeam.DisplayMember = "SubTeamName"
        ComboBox_SubTeam.ValueMember = "SubTeamNo"

        logger.Debug("LoadSubTeamData Exit")

    End Sub

    Private Sub LoadTeamData()

        logger.Debug("LoadTeamData Entry")

        Dim DAO As TeamDAO = New TeamDAO
        Dim _dataset As DataSet = DAO.GetTeams()
        ComboBox_Team.Items.Clear()
        For Each dr As DataRow In _dataset.Tables(0).Rows
            ComboBox_Team.Items.Add(New TeamBO(CInt(dr("Team_No").ToString), dr("Team_Name").ToString, dr("Team_Abbreviation").ToString))
        Next
        _dataset.Dispose()
        ComboBox_Team.DisplayMember = "TeamName"
        ComboBox_Team.ValueMember = "TeamNo"

        logger.Debug("LoadTeamData Exit")
    End Sub

    Private Sub LoadICVData()
        logger.Debug("LoadICVData Entry")

        Dim _datatable As DataTable = VendorDAO.GetInventoryCountVendors()
        ComboBox_ICVID.Items.Clear()
        ComboBox_ICVID.Items.Add(New VendorBO(-1, "-None-", ""))
        For Each dr As DataRow In _datatable.Rows
            ComboBox_ICVID.Items.Add(New VendorBO(CInt(dr("ICVID").ToString), dr("ICVABBRV"), ""))
        Next
        _datatable.Dispose()
        ComboBox_ICVID.DisplayMember = "VendorName"
        ComboBox_ICVID.ValueMember = "VendorID"

        logger.Debug("LoadICVData Exit")
    End Sub

    Private Function ValidateData() As Boolean

        logger.Debug("ValidateData Entry")

        Dim retval As Boolean = True
        Dim _dataset As DataSet = Nothing
        Try
            If ComboBox_SubTeam.SelectedItem Is Nothing Then
                ComboBox_SubTeam.Focus()
                Throw New Exception("You must select a SubTeam.")
            End If
            If ComboBox_Team.SelectedItem Is Nothing Then
                ComboBox_Team.Focus()
                Throw New Exception("You must select a Team.")
            End If

            Me.SubTeamNo = CType(ComboBox_SubTeam.SelectedItem, SubTeamBO).SubTeamNo
            Me.TeamNo = CType(ComboBox_Team.SelectedItem, TeamBO).TeamNo

            ' Validate the PS subteam is numeric or blank
            If (TextBox_PSSubTeamNo.Text.Length > 0) AndAlso Not IsNumeric(TextBox_PSSubTeamNo.Text.Trim()) Then
                TextBox_PSSubTeamNo.Focus()
                Throw New Exception(String.Format(ResourcesIRMA.GetString("msg_NumericValue"), Label_PSSubTeam.Text.Replace(":", "")))
            End If

            ' Validate the PS team is numeric or blank
            If (TextBox_PSTeamNo.Text.Length > 0) AndAlso Not IsNumeric(TextBox_PSTeamNo.Text.Trim()) Then
                TextBox_PSTeamNo.Focus()
                Throw New Exception(String.Format(ResourcesIRMA.GetString("msg_NumericValue"), Label_PSTeam.Text.Replace(":", "")))
            End If

            If (TextBox_PSSubTeamNo.Text.Length > 0) Then
                Me.PS_SubTeamNo = CInt(TextBox_PSSubTeamNo.Text.Trim())
            Else
                Me.PS_SubTeamNo = -1
            End If

            If (TextBox_PSTeamNo.Text.Length > 0) Then
                Me.PS_TeamNo = CInt(TextBox_PSTeamNo.Text.Trim())
            Else
                Me.PS_TeamNo = -1
            End If

            If (TextBox_CostFactor.Text.Length > 0) And IsNumeric(TextBox_CostFactor.Text) Then
                Dim cfactor As Decimal = CDec(TextBox_CostFactor.Text.Trim())
                If cfactor >= 0.0 And cfactor <= 1.0 Then
                    Me.CostFactor = cfactor
                Else
                    TextBox_CostFactor.Focus()
                    Throw New Exception("Cost Factor must be between 0.000 and 1.000")
                End If
            Else
                TextBox_CostFactor.Focus()
                'LAR - TODO: Need to figure out how to manage resources so the message reads "The Cost Factor value must be populated"
                Throw New Exception(String.Format(ResourcesIRMA.GetString("msg_NumericValue"), Label_CostFactor.Text.Replace(":", "")))
            End If

            If (ComboBox_ICVID.SelectedItem Is Nothing) Then
                Me.ICVID = -1
            Else
                Me.ICVID = CType(ComboBox_ICVID.SelectedItem, VendorBO).VendorID
            End If

            If Me.IsNew Then
                Dim dao As StoreSubTeamDAO = New StoreSubTeamDAO
                _dataset = dao.ValidateSubTeamToTeamRelationships(Me.StoreNo, Me.SubTeamNo, Me.TeamNo)
                If _dataset.Tables(0).Rows.Count > 0 Then
                    ComboBox_SubTeam.Focus()
                    Throw New Exception("A Team relationship for that Store / Subteam combination already exists.")
                End If
            End If

        Catch Ex As Exception
            MsgBox(Ex.Message)
            logger.Error(Ex.Message)
            retval = False
        Finally
            If Not _dataset Is Nothing Then _dataset.Dispose()
        End Try

        logger.Debug("ValidateData Exit with retval= " + retval.ToString)
        Return retval
    End Function

    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        logger.Debug("Button_Save_Click Entry")
        If ValidateData() Then
            SaveChanges()
        End If
        logger.Debug("Button_Save_Click Exit")
    End Sub

    Private Sub SaveChanges()

        logger.Debug("SaveChanges Entry")

        Dim dao As StoreSubTeamDAO = New StoreSubTeamDAO
        If Me.IsNew Then
            dao.CreateSubTeamToTeamRelationship(Me.StoreNo, Me.SubTeamNo, Me.TeamNo, Me.PS_SubTeamNo, Me.PS_TeamNo, Me.CostFactor, Me.ICVID)
        Else
            dao.UpdateSubTeamToTeamRelationship(Me.StoreNo, Me.SubTeamNo, Me.TeamNo, Me.PS_SubTeamNo, Me.PS_TeamNo, Me.CostFactor, Me.ICVID)
        End If

        Me.Close()
        logger.Debug("SaveChanges Exit")

    End Sub

    Private Sub TextBox_PSSubTeamNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_PSSubTeamNo.TextChanged

    End Sub

    Private Sub TextBox_PSTeamNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_PSTeamNo.TextChanged

    End Sub

    Private Sub SetFieldsStates()
        If InstanceDataDAO.IsFlagActive("UKIPS", glStoreID) Then
            Me.ComboBox_Team.Enabled = False
            Me.TextBox_PSSubTeamNo.Enabled = False
            Me.TextBox_PSTeamNo.Enabled = False
        Else
            Me.ComboBox_Team.Enabled = True
            Me.TextBox_PSSubTeamNo.Enabled = True
            Me.TextBox_PSTeamNo.Enabled = True
        End If

    End Sub
End Class