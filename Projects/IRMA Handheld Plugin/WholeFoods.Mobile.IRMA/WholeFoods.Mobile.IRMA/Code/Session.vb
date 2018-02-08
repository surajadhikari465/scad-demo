Imports System.Data

Public Class Session
    Inherits GatewayClient

    Public ServiceURI As String
    Public StoreName As String
    Public StoreNo As String
    Public UserName As String
    Public UserID As Integer
    Public UserStore As Integer
    Public Region As String
    Public SessionName As String
    Public Subteam As String
    Public SubteamKey As String
    Public Subteams As DataTable
    Public Stores As DataTable
    Public ShrinkTypes As DataTable
    Public ShrinkSubTypes As ShrinkSubType()
    Public ShrinkAdjustmentIds As DataTable
    Public ShrinkType As String
    Public ShrinkTypeId As String
    Public ShrinkAdjId As String
    Public ShrinkSubTypeID As Integer
    Public ShrinkSubType As String
    Public InventoryAdjustmentCodeID As Integer
    Public ShrinkList As List(Of XNode)
    Public MyScanner As HandheldHardware.HandheldScanner
    Public WebProxyClient As GatewayClient
    Public SupressInvalidSubteamWarning As Integer = -1
    Public ActionType As Enums.ActionType
    Public StartTime As DateTime
    Public IsLoadedSession As Boolean
    Public TransferFromStoreName As String
    Public TransferFromStoreNo As String
    Public TransferToStoreName As String
    Public TransferToStoreNo As String
    Public TransferFromSubteam As String
    Public TransferFromSubteamKey As String
    Public TransferToSubteam As String
    Public TransferToSubteamKey As String
    Public TransferExpectedDate As DateTime
    Public StoresWithVendorId As DataTable
    Public ProductTypes As DataTable
    Public SupplySubteams As DataTable
    Public SelectedProductType As Integer
    Public SelectedSupplySubteam As Integer
    Public transferVendorId As Integer
    Public UserAllStoresAccess As Boolean
    Public DSDVendorName As String
    Public DSDVendorID As Integer
    Public DSDInvoice As String
    Public UOMID As String
    Public StoreVendorID As Integer
    Public hasSubTypeUpdated As Boolean = False


    Public Enum CurrentScreenType
        MainForm = 0
        ReceiveOrder = 1
        ReceivingDocumentScan = 2
        ShrinkScan = 3
    End Enum

    Public CurrentScreen As CurrentScreenType
    Public DiscountReasonCodeList As DataTable

    Public Overloads Shared Function CreateDefaultBinding() As System.ServiceModel.Channels.Binding
        Dim httpBinding As System.ServiceModel.Channels.CustomBinding = New System.ServiceModel.Channels.CustomBinding
        Dim httpBindingElement As New System.ServiceModel.Channels.HttpTransportBindingElement

        httpBindingElement.MaxBufferPoolSize = 2147483647
        httpBindingElement.MaxBufferSize = 2147483647
        httpBindingElement.MaxReceivedMessageSize = 2147483647

        httpBinding.OpenTimeout = New TimeSpan(0, 10, 0)
        httpBinding.SendTimeout = New TimeSpan(0, 10, 0)
        httpBinding.ReceiveTimeout = New TimeSpan(0, 10, 0)
        httpBinding.CloseTimeout = New TimeSpan(0, 10, 0)

        httpBinding.Elements.Add(New System.ServiceModel.Channels.TextMessageEncodingBindingElement(System.ServiceModel.Channels.MessageVersion.Soap11, System.Text.Encoding.UTF8))
        httpBinding.Elements.Add(httpBindingElement)

        Return httpBinding
    End Function

    Public Sub New(ByVal webAddress As String)
        Dim address As System.ServiceModel.EndpointAddress
        Dim binding As System.ServiceModel.Channels.Binding = CreateDefaultBinding()

        If (webAddress <> Nothing) Then
            address = New System.ServiceModel.EndpointAddress(webAddress)
        Else
            address = GatewayClient.EndpointAddress
        End If

        Me.WebProxyClient = New GatewayClient(binding, address)
    End Sub

#Region " Public Methods"

    Function GetSessionName()
        Return Me.SessionName
    End Function

    Public Sub SetSubteamKey(ByVal sTeam As String)
        If (String.IsNullOrEmpty(sTeam) = False) Then

            'parse subteam
            Dim mylist As String() = sTeam.Split(",")

            If (mylist.Length > 1) Then
                Me.SubteamKey = mylist(0)
                Me.Subteam = mylist(1)
            Else
                Me.SubteamKey = sTeam
                Me.Subteam = sTeam
            End If

        End If
    End Sub

    Public Sub SetTransferFromSubteamKey(ByVal sTeam As String)
        If (String.IsNullOrEmpty(sTeam) = False) Then

            'parse subteam
            Dim mylist As String() = sTeam.Split(",")

            If (mylist.Length > 1) Then
                Me.TransferFromSubteamKey = mylist(0)
                Me.TransferFromSubteam = mylist(1)
            Else
                Me.TransferFromSubteamKey = sTeam
                Me.TransferFromSubteam = sTeam
            End If

        End If
    End Sub

    Public Sub SetTransferToSubteamKey(ByVal sTeam As String)
        If (String.IsNullOrEmpty(sTeam) = False) Then

            'parse subteam
            Dim mylist As String() = sTeam.Split(",")

            If (mylist.Length > 1) Then
                Me.TransferToSubteamKey = mylist(0)
                Me.TransferToSubteam = mylist(1)
            Else
                Me.TransferToSubteamKey = sTeam
                Me.TransferToSubteam = sTeam
            End If

        End If
    End Sub

    Public Sub SetStore(ByVal storeName As String)
        If (String.IsNullOrEmpty(storeName) = False) Then

            'parse subteam
            Dim mylist As String() = storeName.Split(",")

            If (mylist.Length > 1) Then
                Me.StoreNo = mylist(0)
                Me.StoreName = mylist(1)
            Else
                Me.StoreNo = storeName
                Me.StoreName = storeName
            End If

        End If
    End Sub

    Public Sub SetTransferFromStore(ByVal storeName As String)
        If (String.IsNullOrEmpty(storeName) = False) Then

            'parse subteam
            Dim mylist As String() = storeName.Split(",")

            If (mylist.Length > 1) Then
                Me.TransferFromStoreNo = mylist(0)
                Me.TransferFromStoreName = mylist(1)
            Else
                Me.TransferFromStoreNo = storeName
                Me.TransferFromStoreName = storeName
            End If

        End If
    End Sub

    Public Sub SetTransferToStore(ByVal storeName As String)
        If (String.IsNullOrEmpty(storeName) = False) Then

            'parse subteam
            Dim mylist As String() = storeName.Split(",")

            If (mylist.Length > 1) Then
                Me.TransferToStoreNo = mylist(0)
                Me.TransferToStoreName = mylist(1)
            Else
                Me.TransferToStoreNo = storeName
                Me.TransferToStoreName = storeName
            End If

        End If
    End Sub

    Public Sub SetItemType(ByVal sType As String)
        If (String.IsNullOrEmpty(sType) = False) Then

            'parse subteam
            Dim mylist As String() = sType.Split(",")

            If (mylist.Length > 1) Then
                Me.ShrinkTypeId = mylist(0)
                Me.ShrinkType = mylist(1)
            Else
                Me.ShrinkTypeId = sType
                Me.ShrinkType = sType
            End If

            'now look up Adjustment ID
            Dim expr As String = "DisplayMember='" & Me.ShrinkType & "'"
            Dim dr As DataRow() = ShrinkAdjustmentIds.Select(expr)

            Me.ShrinkAdjId = dr(0).Item("ValueMember")

        End If
    End Sub

    Public Sub SetHasSubTypeUpdated(ByVal sessionUpdated As Boolean)
        Me.hasSubTypeUpdated = sessionUpdated
    End Sub

    Public Sub SetShrinkSubTypeId(ByVal id As Integer)
        Me.ShrinkSubTypeID = id
    End Sub

    Public Sub SetShrinkSubType(ByVal shrinkSubType As String)
        Me.ShrinkSubType = shrinkSubType
    End Sub

    Public Sub SetInventoryAdjustmentCodeId(ByVal id As Integer)
        Me.InventoryAdjustmentCodeID = id
    End Sub

    Public Sub SetShrinkType(ByVal sType As String)
        If (String.IsNullOrEmpty(sType) = False) Then

            'parse subteam
            Dim mylist As String() = sType.Split(",")

            If (mylist.Length > 1) Then
                Me.ShrinkTypeId = mylist(0)
                Me.ShrinkType = mylist(1)
            Else
                Me.ShrinkTypeId = sType
                Me.ShrinkType = sType
            End If

            'now look up Adjustment ID
            Dim expr As String = "DisplayMember='" & Me.ShrinkType & "'"
            Dim dr As DataRow() = ShrinkAdjustmentIds.Select(expr)

            Me.ShrinkAdjId = dr(0).Item("ValueMember")

        End If
    End Sub

    Public Function GetUpdatedSession(ByVal filePath As String) As Session
        Return New Session(Me.ServiceURI)
    End Function

    Public Function IsSubTeamUnrestricted(ByVal SubteamNo As Integer) As Boolean
        Dim expr As String = "ValueMember='" & SubteamNo & "'"
        Dim dr As DataRow() = Subteams.Select(expr)
        Return dr(0).Item("IsUnrestricted")
    End Function

    Public Function IsSubTeamFixedSpoilage(ByVal SubteamNo As Integer) As Boolean
        Dim expr As String = "ValueMember='" & SubteamNo & "'"
        Dim dr As DataRow() = Subteams.Select(expr)
        Return dr(0).Item("IsFixedSpoilage")
    End Function

    Public Function IsExpenseSubteam(ByVal SubteamNo As Integer) As Boolean
        Dim expr As String = "ValueMember='" & SubteamNo & "'"
        Dim dr As DataRow() = Subteams.Select(expr)
        If dr(0).Item("SubTeamType_ID") = 4 Then Return True
        Return False
    End Function

    Public Function IsPackagingSubteam(ByVal SubteamNo As Integer) As Boolean
        Dim expr As String = "ValueMember='" & SubteamNo & "'"
        Dim dr As DataRow() = Subteams.Select(expr)
        If dr(0).Item("SubTeamType_ID") = 5 Then Return True
        Return False
    End Function

    Public Function IsOtherSuppliesSubteam(ByVal SubteamNo As Integer) As Boolean
        Dim expr As String = "ValueMember='" & SubteamNo & "'"
        Dim dr As DataRow() = Subteams.Select(expr)
        If dr(0).Item("SubTeamType_ID") = 6 Then Return True
        Return False
    End Function

    Public Function CanAcceptShrink(ByVal SubteamNo As Integer) As Boolean
        Dim expr As String = "ValueMember='" & SubteamNo & "'"
        Dim dr As DataRow() = Subteams.Select(expr)
        Dim typeID As Integer = CInt(dr(0).Item("SubTeamType_ID"))

        If typeID.Equals(Enums.SubTeamType.Expense) Or typeID.Equals(Enums.SubTeamType.Suplies) Or typeID.Equals(Enums.SubTeamType.Packaging) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Sub PopulateProductType()
        Dim dtProductTypes = New DataTable
        Dim ProductTypeId As Integer
        Dim dr As DataRow

        dtProductTypes.Columns.Add(New DataColumn("DisplayMember"))
        dtProductTypes.Columns.Add(New DataColumn("ValueMember"))

        ProductTypeId = Enums.ProductType.Product
        dr = dtProductTypes.NewRow()
        dr.Item("DisplayMember") = Enums.ProductType.Product
        dr.Item("ValueMember") = ProductTypeId
        dtProductTypes.Rows.Add(dr)

        ProductTypeId = Enums.ProductType.PackagingSupplies
        dr = dtProductTypes.NewRow()
        dr.Item("DisplayMember") = Enums.ProductType.PackagingSupplies
        dr.Item("ValueMember") = ProductTypeId
        dtProductTypes.Rows.Add(dr)

        ProductTypeId = Enums.ProductType.OtherSupplies
        dr = dtProductTypes.NewRow()
        dr.Item("DisplayMember") = Enums.ProductType.OtherSupplies
        dr.Item("ValueMember") = ProductTypeId
        dtProductTypes.Rows.Add(dr)

        ProductTypes = dtProductTypes
    End Sub

    Public Sub ClearTransferSession()
        TransferExpectedDate = Date.MinValue
        TransferFromStoreNo = String.Empty
        TransferFromStoreName = String.Empty
        TransferFromSubteam = String.Empty
        TransferFromSubteamKey = String.Empty
        TransferToStoreNo = String.Empty
        TransferToStoreName = String.Empty
        TransferToSubteam = String.Empty
        TransferToSubteamKey = String.Empty
        SelectedSupplySubteam = 0
        transferVendorId = 0
    End Sub
#End Region

End Class