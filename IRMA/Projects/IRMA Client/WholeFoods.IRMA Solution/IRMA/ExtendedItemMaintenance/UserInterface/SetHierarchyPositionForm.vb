Imports System.Linq
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ModelLayer.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class SetHierarchyPositionForm

    Private itemHasAlignedSubteam As Boolean
    Private enforceSubteamLocking As Boolean

    Public Property ItemIdentifier() As String
        Get
            Return Me.HierarchySelectorForItem.ItemIdentifier
        End Get
        Set(ByVal value As String)
            Me.HierarchySelectorForItem.ItemIdentifier = value
            Me.LabelItemIdentifier.Text = value
        End Set
    End Property

    Public Property SubTeamNo() As Integer
        Get
            Return Me.HierarchySelectorForItem.SelectedSubTeamId
        End Get
        Set(ByVal value As Integer)
            Me.HierarchySelectorForItem.SelectedSubTeamId = value
        End Set
    End Property

    Public Property CategoryId() As Integer
        Get
            Return Me.HierarchySelectorForItem.SelectedCategoryId
        End Get
        Set(ByVal value As Integer)
            Me.HierarchySelectorForItem.SelectedCategoryId = value
        End Set
    End Property

    Public Property Level3() As Integer
        Get
            Return Me.HierarchySelectorForItem.SelectedLevel3Id
        End Get
        Set(ByVal value As Integer)
            Me.HierarchySelectorForItem.SelectedLevel3Id = value
        End Set
    End Property

    Public Property Level4() As Integer
        Get
            Return Me.HierarchySelectorForItem.SelectedLevel4Id
        End Get
        Set(ByVal value As Integer)
            Me.HierarchySelectorForItem.SelectedLevel4Id = value
        End Set
    End Property

    Private _retailSale As Boolean
    Public Property IsRetailSale() As Boolean
        Get
            Return _retailSale
        End Get
        Set(ByVal value As Boolean)
            _retailSale = value
        End Set
    End Property

    Public Sub Initialize()
        Me.HierarchySelectorForItem.Initialize()
    End Sub

    Private Sub SetHierarchyPositionForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        enforceSubteamLocking = InstanceDataDAO.IsFlagActive("UKIPS")
        itemHasAlignedSubteam = ItemIdentifierDAO.Instance.HasAlignedSubteam(UploadRowDAO.Instance.GetItemKeyByIdentifier(Me.ItemIdentifier))

        ' No subteam updates are allowed if the item is associated to an aligned subteam (unless it's not retail sale).
        If enforceSubteamLocking And itemHasAlignedSubteam And IsRetailSale Then
            HierarchySelectorForItem.cmbSubTeam.Enabled = False
        End If
    End Sub

    Private Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click
        ' If the item is associated to a non-aligned subteam, then only another non-aligned subteam can be chosen.
        If enforceSubteamLocking And Not itemHasAlignedSubteam Then
            Dim nonAlignedSubteams As List(Of String) = SubTeamDAO.GetNonAlignedSubteamNames().OrderBy(Function(subteam) subteam).ToList()
            Dim selectedSubteam As String = HierarchySelectorForItem.cmbSubTeam.SelectedItem.ToString().Trim()

            If Not nonAlignedSubteams.Contains(selectedSubteam) Then
                MessageBox.Show(String.Format("{0} is assigned to a non-aligned subteam, so only another non-aligned subteam may be chosen.  Please choose one of the following:" + _
                                          Environment.NewLine + Environment.NewLine + "{1}", Me.ItemIdentifier, String.Join(", ", nonAlignedSubteams)), "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If
        End If

        ' If the item is non-retail sale and associated to an aligned subteam, then it can only be moved to another aligned subteam.
        If enforceSubteamLocking And itemHasAlignedSubteam And Not IsRetailSale Then
            Dim nonAlignedSubteams As List(Of String) = SubTeamDAO.GetNonAlignedSubteamNames().OrderBy(Function(subteam) subteam).ToList()
            Dim selectedSubteam As String = HierarchySelectorForItem.cmbSubTeam.SelectedItem.ToString().Trim()

            If nonAlignedSubteams.Contains(selectedSubteam) Then
                MessageBox.Show(String.Format("{0} is assigned to an aligned subteam, so only another aligned subteam may be chosen.  Please choose any subteam except one of the following:" + _
                                          Environment.NewLine + Environment.NewLine + "{1}", Me.ItemIdentifier, String.Join(", ", nonAlignedSubteams)), "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.DialogResult = Windows.Forms.DialogResult.None
                Exit Sub
            End If
        End If
    End Sub
End Class
