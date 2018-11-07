Option Explicit On
Option Strict On

Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.IRMA.EPromotions.DataAccess
Imports WholeFoods.Utility
Imports System.ComponentModel   ' Need for BindingList 

Public Class Form_PromotionOffer_AddConidtion

    Private Sub Form_PromotionOffer_AddGroup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CheckBox_Required.Checked = _IsRequired
        LoadGroupsList()
    End Sub

    Private Sub LoadGroupsList()
        Dim PromotionDAO As PromotionOfferDAO = New PromotionOfferDAO
        Dim GroupList As BindingList(Of ItemGroupBO) = New BindingList(Of ItemGroupBO)
        Dim removeList As ArrayList = New ArrayList

        GroupList = PromotionDAO.GetPromotionalGroupList()

        Dim CurrentPromotionMembers As PromotionOfferMemberBOList = New PromotionOfferMemberBOList
        CurrentPromotionMembers = PromotionDAO.GetPromotionalOfferMembersList(_PromotionId, CInt(IIf(CheckBox_Required.Checked, 0, 1)), CBool(IIf(_Purpose = 1, True, False)))

        For Each Promotion As PromotionOfferMemberBO In CurrentPromotionMembers
            For Each Group As ItemGroupBO In GroupList
                If Promotion.GroupID = Group.GroupID Then
                    removeList.Add(Group)
                End If
            Next
        Next

        For Each Group As ItemGroupBO In removeList
            GroupList.Remove(Group)
        Next

        ListBox_Groups.DataSource = GroupList
        ListBox_Groups.DisplayMember = "GroupName"
        ListBox_Groups.ValueMember = "GroupId"


    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Public Enum PurposeType
        PurchaseRequirement = 0
        Reward = 1
    End Enum
    Private Sub Button_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Add.Click
        Dim PromotionMember As PromotionOfferMemberBO = New PromotionOfferMemberBO
        Dim PromotionDAO As PromotionOfferDAO = New PromotionOfferDAO()




        'Check for a valid Quantity.
        Try
            If Int32.Parse(TextBox_Qty.Text) < 1 Then
                'error: Qty must be 1 or greater
                Debug.WriteLine("error: Qty must be 1 or greater")
            End If
        Catch ex As Exception
            'error: Qty must be an integer value
            Debug.WriteLine("error: Qty must be an integer value")
        End Try
        'Check for a selected Group
        If ListBox_Groups.SelectedItems.Count < 1 Then
            'error: Must Select a Group.
            Debug.WriteLine("error: Must Select a Group.")
        End If



        With PromotionMember
            .JoinLogic = CType(IIf(CheckBox_Required.Checked, 0, 1), Byte)
            .Quantity = Int32.Parse(TextBox_Qty.Text)
            .UserID = 1
            .Modified = DateTime.Now
            .Purpose = CBool(Purpose)
            .GroupID = CType(ListBox_Groups.SelectedItem, ItemGroupBO).GroupID
            .OfferID = _PromotionId
        End With

        'Insert Group/Qty Into Current Promotion
        Try
            PromotionDAO.InsertPromotionalOfferMember(PromotionMember)
        Catch ex As Exception
            MsgBox(ex.InnerException.Message)

        End Try

        Me.Close()



    End Sub

    Sub New(ByVal PromotionId As Integer, Optional ByVal IsRequired As Boolean = False, Optional ByVal Purpose As PurposeType = PurposeType.PurchaseRequirement)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _IsRequired = IsRequired
        _PromotionId = PromotionId
        _Purpose = Purpose

    End Sub


    Private _PromotionId As Integer
    Public Property PromotionId() As Integer
        Get
            Return _PromotionId
        End Get
        Set(ByVal value As Integer)
            _PromotionId = value
        End Set
    End Property


    Private _IsRequired As Boolean
    Public Property IsRequired() As Boolean
        Get
            Return _IsRequired
        End Get
        Set(ByVal value As Boolean)
            _IsRequired = value
        End Set
    End Property


    Private _Purpose As Integer
    Public Property Purpose() As Integer
        Get
            Return _Purpose
        End Get
        Set(ByVal value As Integer)
            _Purpose = value
        End Set
    End Property


    Private Sub CheckBox_Required_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_Required.CheckedChanged
        _IsRequired = CheckBox_Required.Checked
        LoadGroupsList()

    End Sub
End Class