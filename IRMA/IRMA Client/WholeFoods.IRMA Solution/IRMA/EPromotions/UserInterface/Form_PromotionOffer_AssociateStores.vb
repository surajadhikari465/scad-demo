Imports WholeFoods.Utility
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.IRMA.EPromotions.DataAccess


Public Class Form_PromotionOffer_AssociateStores

    Private _PromotionOfferId As Integer
    Private _PricingMethodId As Integer
    Private _VerifyingSelectedItem As Integer
    Private _RemoveList As ArrayList = New ArrayList
    Private _MarkDeletedList As ArrayList = New ArrayList
    Private _AddList As ArrayList = New ArrayList
    Private _WarningList As ArrayList = New ArrayList
    Private _OriginalList As ArrayList = New ArrayList

#Region "Constructors"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
#End Region

#Region "Properties"

    Public ReadOnly Property LoadedStoresCount() As Integer
        Get
            Return ListBox_Stores.Items.Count
        End Get
    End Property
#End Region

#Region "Public Methods"

    Public Sub AssignStores(ByVal PromotionOfferId As Integer, ByVal PricingMethodId As Integer, Optional ByVal Display As Boolean = True)
        _PromotionOfferId = PromotionOfferId
        _PricingMethodId = PricingMethodId

        If Display Then
            Me.ShowDialog(MyBase.ParentForm)
        Else
            LoadStores(_PromotionOfferId, _PricingMethodId)
        End If
    End Sub

    Public Sub SaveChanges(ByRef OfferId As Integer)
        Dim DAO As PromotionOfferDAO = New PromotionOfferDAO

        Try

            For Each item As PromotionalOfferStoreBO In _RemoveList
                DAO.RemoveStoreFromPromotion(OfferId, CInt(item.StoreNo))
            Next
            For Each item As PromotionalOfferStoreBO In _WarningList
                DAO.RemoveStoreFromPromotion(OfferId, CInt(item.StoreNo))
            Next
            For Each item As PromotionalOfferStoreBO In _AddList
                DAO.AddStoreToPromotion(OfferId, CInt(item.StoreNo))
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Form_PromotionOffer_AssociateStores:SaveChanges")

        End Try

    End Sub

    ''' <summary>
    ''' Checks to see if all the currently selected stores are valid for the submitted Pricing Method
    ''' </summary>
    ''' <returns>a string value; either an empty string for valid or the warning mesage if invalid stores were detected.</returns>
    ''' <remarks></remarks>
    Public Function ValidateSelectedStoresForPricingMethod(ByVal pricingMethodID As Integer) As String
        Dim result As String = ""
        Dim PromotionDAO As New PromotionOfferDAO
        Dim PriceMethods As DataTable = PromotionDAO.GetStoresByPricingMethod(pricingMethodID)
        Dim valid As Boolean

        Try
            For Each storeBO As PromotionalOfferStoreBO In ListBox_Stores.Items
                valid = False

                ' check to see if the selected store is in the list of valid stores for this Pricing Method
                For Each dr As DataRow In PriceMethods.Rows
                    If storeBO.StoreNo = dr("Store_No").ToString Then
                        valid = True
                        Exit For
                    End If
                Next

                ' if the selected store is not found, add it's name to the message
                If Not valid Then
                    result = result & ", " & storeBO.StoreName
                End If

            Next

            ' if a string has been stored, remove leading comma
            If result.Length > 2 Then
                result = result.Substring(2)
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Form_PromotionOffer_AssociateStores:ValidateSelectedStoresForPricingMethod")
        End Try

        Return result

    End Function

    Public Sub RefreshStoresForPricingMethod(ByVal PricingMethodID As Integer)
        Dim PromotionDAO As PromotionOfferDAO = New PromotionOfferDAO
        Dim PriceMethods As DataTable = PromotionDAO.GetStoresByPricingMethod(PricingMethodID)
        Dim storesSupported As New ArrayList
        Dim StoreItem As PromotionalOfferStoreBO
        Dim ItemIndex As Integer
        Dim found As Boolean

        ' build array of  supprted StoreIDs
        For Each dr As DataRow In PriceMethods.Rows
            storesSupported.Add(CInt(dr("Store_No")))
        Next

        ' remove unsupported items from list
        For Each item As PromotionalOfferStoreBO In ListBox_Stores.Items
            If Not storesSupported.Contains(CInt(item.StoreNo)) Then
                ListBox_Stores.Items.Remove(item)
            End If
        Next

        ' check to see if the store already exists in  the list.
        ' if not, add store
        For Each dr As DataRow In PriceMethods.Rows

            found = False
            For Each item As PromotionalOfferStoreBO In ListBox_Stores.Items
                If item.StoreNo = dr("Store_No").ToString Then
                    found = True
                    Exit For
                End If
            Next

            If Not found Then
                StoreItem = New PromotionalOfferStoreBO
                StoreItem.StoreNo = dr("Store_No").ToString
                StoreItem.StoreName = dr("Store_Name").ToString
                StoreItem.IsAssigned = False
                StoreItem.IsActive = False
                ItemIndex = ListBox_Stores.Items.Add(StoreItem)
            End If

        Next

    End Sub

#End Region

#Region "Private Methods"
    Private Sub LoadStores(ByVal OfferId As Integer, ByVal PricingMethodID As Integer)
        Dim PromotionDAO As PromotionOfferDAO = New PromotionOfferDAO
        Dim StoreData As DataTable = PromotionDAO.GetStoresByPromotionId(OfferId)
        Dim StoreItem As PromotionalOfferStoreBO
        Dim ItemIndex As Integer

        ' Process store records
        If StoreData.Rows.Count > 0 Then

            ListBox_Stores.DisplayMember = "Store_Name"
            ListBox_Stores.ValueMember = "Store_No"

            ' Load list with PromotionalOfferStoreBO's for every supported Store
            For Each dr As DataRow In StoreData.Rows
                StoreItem = New PromotionalOfferStoreBO
                StoreItem.StoreNo = dr("Store_No").ToString
                StoreItem.StoreName = dr("Store_Name").ToString
                StoreItem.IsAssigned = CBool(dr("IsAssigned"))
                StoreItem.IsActive = CBool(dr("IsActive"))

                ItemIndex = ListBox_Stores.Items.Add(StoreItem)

                Label_NoStoresMsg.Visible = False
                ListBox_Stores.Visible = True

                If StoreItem.IsAssigned = True Then
                    ListBox_Stores.SetSelected(ItemIndex, True)

                    ' cache original store list so we can revert to it on cancel
                    _OriginalList.Add(StoreItem)
                End If
            Next
        Else
            ' No Store/Offer association records; populate list with stores that are valid for 
            ' current Pricing Method
            Dim PriceMethods As DataTable = PromotionDAO.GetStoresByPricingMethod(PricingMethodID)

            If PriceMethods.Rows.Count > 0 Then
                ListBox_Stores.DisplayMember = "Store_Name"
                ListBox_Stores.ValueMember = "Store_No"
                For Each dr As DataRow In PriceMethods.Rows
                    StoreItem = New PromotionalOfferStoreBO
                    StoreItem.StoreNo = dr("Store_No").ToString
                    StoreItem.StoreName = dr("Store_Name").ToString
                    StoreItem.IsAssigned = False
                    StoreItem.IsActive = False

                    ItemIndex = ListBox_Stores.Items.Add(StoreItem)
                Next

                Label_NoStoresMsg.Visible = False
                ListBox_Stores.Visible = True

            Else
                Label_NoStoresMsg.Visible = True
                ListBox_Stores.Visible = False
            End If
        End If
        StoreData.Dispose()


    End Sub

#End Region

#Region "Form Events"
    Private Sub Form_PromotionOffer_AssociateStores_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Me.LoadedStoresCount = 0 Then
            LoadStores(_PromotionOfferId, _PricingMethodId)
        End If

    End Sub
    Private Sub SelectionStatus_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton_All.CheckedChanged, RadioButton_Manual.CheckedChanged

        With ListBox_Stores
            If RadioButton_All.Checked Then
                For cnt As Integer = 0 To .Items.Count - 1
                    .SetSelected(cnt, True)
                Next
                .Enabled = False
            ElseIf RadioButton_Manual.Checked Then
                .Enabled = True
            End If
        End With
    End Sub

    Private Sub RadioButton_Clear_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButton_Clear.CheckedChanged

        If RadioButton_Clear.Checked And ListBox_Stores.SelectedItems.Count > 0 Then
            If MessageBox.Show(String.Format(ResourcesEPromotions.GetString("msg_confirm_ClearStores")), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) _
            = Windows.Forms.DialogResult.Yes Then

                With ListBox_Stores
                    For cnt As Integer = 0 To .Items.Count - 1
                        .SetSelected(cnt, False)
                    Next

                    ' disable list - only re-enabled by going to manual
                    .Enabled = False
                End With
                RadioButton_Clear.Checked = True
            Else
                RadioButton_Clear.Checked = False
                RadioButton_Manual.Checked = True
            End If

        End If

    End Sub

    Private Sub ListBox_Stores_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox_Stores.SelectedIndexChanged
        Dim item As PromotionalOfferStoreBO
        For i As Integer = 0 To ListBox_Stores.Items.Count - 1
            item = CType(ListBox_Stores.Items.Item(i), PromotionalOfferStoreBO)
            If ListBox_Stores.SelectedIndices.Contains(i) Then
                item.IsSelected = True
            Else
                item.IsSelected = False
            End If
        Next
        If ListBox_Stores.SelectedIndices.Count > 0 And RadioButton_Clear.Checked Then
            RadioButton_Manual.Checked = True
        End If
    End Sub

    Private Sub Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        Dim item As PromotionalOfferStoreBO
        Dim WarningMessage As String = String.Empty
        Dim result As DialogResult

        If RadioButton_Manual.Checked And ListBox_Stores.SelectedItems.Count = 0 Then
            MsgBox("You must select at least one store.")
            Exit Sub
        End If

        For i As Integer = 0 To ListBox_Stores.Items.Count - 1
            item = CType(ListBox_Stores.Items.Item(i), PromotionalOfferStoreBO)

            If Not item.IsSelected Then
                If item.IsActive Then
                    'Item was Unselected, but Promotion is active and already contains a reference for this store. warning.
                    If Not _WarningList.Contains(item) Then
                        _WarningList.Add(item)
                    End If
                ElseIf item.IsAssigned Then
                    'Unselected, but assigned. remove from  PromotionalOfferStore
                    If Not _RemoveList.Contains(item) Then
                        _RemoveList.Add(item)
                    End If
                Else
                    'Unselected, but not assigned, do nothing.
                    While _AddList.Contains(item)
                        _AddList.Remove(item)
                    End While
                End If
            Else
                If item.IsActive Then
                    'selected, but active, do nothing.
                ElseIf item.IsAssigned Then
                    'selected, but already assigned, do nothing
                Else
                    '        'selected, and not assigned, add record to PromotionalOfferStore
                    If Not _AddList.Contains(item) Then
                        _AddList.Add(item)
                    End If

                End If
            End If
        Next

        ' are there any items in the warning list? If so, create warning message and exit sub.
        If _WarningList.Count > 0 Then
            WarningMessage = "You have unselected stores that are currently assigned to active Promotions. This will cause this promotion to be removed from the following stores:" & vbCrLf
            For Each store As PromotionalOfferStoreBO In _WarningList
                WarningMessage += store.StoreName & vbCrLf
            Next
            WarningMessage += "Do you want to apply the changes?"
        End If

        If WarningMessage.Length > 0 Then
            result = MessageBox.Show(WarningMessage, "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation)

            If result = Windows.Forms.DialogResult.Yes Then
                ' do nothing - changes are ready to applied by executing SaveChanges method - should probably only be called
                ' from Form_PromotionalOffer
                Me.Hide()
            ElseIf result = Windows.Forms.DialogResult.No Then
                ' erase Add, remove and warning listsfs
                _AddList.Clear()
                _RemoveList.Clear()
                _WarningList.Clear()
                Exit Sub
            ElseIf result = Windows.Forms.DialogResult.Cancel Then
                Me.Close()
            End If
        Else
            ' do nothing - changes are ready to applied by executing SaveChanges method - should probably only be called
            ' from Form_PromotionalOffer
            Me.Hide()

        End If
    End Sub


    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Hide()

        Dim i As Integer
        For i = 0 To ListBox_Stores.Items.Count - 1
            If _OriginalList.Contains(ListBox_Stores.Items(i)) Then
                ListBox_Stores.SetSelected(i, True)
            Else
                ListBox_Stores.SetSelected(i, False)
            End If
        Next

    End Sub

#End Region

End Class


