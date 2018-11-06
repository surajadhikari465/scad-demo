

Imports System.Collections.Generic

Imports WholeFoods.ServiceLibrary

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports WholeFoods.ServiceLibrary.IRMA



'''<summary>
'''This is a test class for GatewayServiceTest and is intended
'''to contain all GatewayServiceTest Unit Tests
'''</summary>
<TestClass()> _
Public Class GatewayServiceTest


    '    Private testContextInstance As TestContext

    '    '''<summary>
    '    '''Gets or sets the test context which provides
    '    '''information about and functionality for the current test run.
    '    '''</summary>
    '    Public Property TestContext() As TestContext
    '        Get
    '            Return testContextInstance
    '        End Get
    '        Set(ByVal value As TestContext)
    '            testContextInstance = Value
    '        End Set
    '    End Property

    '#Region "Additional test attributes"
    '    '
    '    'You can use the following additional attributes as you write your tests:
    '    '
    '    'Use ClassInitialize to run code before running the first test in the class
    '    '<ClassInitialize()>  _
    '    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    '    'End Sub
    '    '
    '    'Use ClassCleanup to run code after all tests in a class have run
    '    '<ClassCleanup()>  _
    '    'Public Shared Sub MyClassCleanup()
    '    'End Sub
    '    '
    '    'Use TestInitialize to run code before running each test
    '    '<TestInitialize()>  _
    '    'Public Sub MyTestInitialize()
    '    'End Sub
    '    '
    '    'Use TestCleanup to run code after each test has run
    '    '<TestCleanup()>  _
    '    'Public Sub MyTestCleanup()
    '    'End Sub
    '    '
    '''<summary>
    '''A test for CreateOrder
    '''</summary>
    <TestMethod()> _
    Public Sub CreateTransferOrderTest()
        Dim targetOrder As New Order()
        Dim oiList As New List(Of OrderItem)
        Dim targetOrderItem As OrderItem
        Dim OrderHeader_ID As Integer = Nothing
        Dim gatewayService As New GatewayService()
        Dim testResult As New Result()

        targetOrder.CreatedBy = 3987
        targetOrder.ProductType_ID = 3
        targetOrder.OrderType_Id = 3
        targetOrder.Vendor_ID = 7545
        targetOrder.Transfer_SubTeam = 190
        targetOrder.ReceiveLocation_ID = 7545
        targetOrder.PurchaseLocation_ID = 7545
        targetOrder.Transfer_To_SubTeam = 645
        targetOrder.SupplyTransferToSubTeam = 110
        targetOrder.Fax_Order = False
        targetOrder.Expected_Date = Now()
        targetOrder.Return_Order = False
        targetOrder.FromQueue = False
        targetOrder.DSDOrder = False

        targetOrder.DSDOrder = False

        targetOrderItem = New OrderItem()
        targetOrderItem.QuantityOrdered = 1
        targetOrderItem.Item_Key = 279058
        targetOrderItem.QuantityUnit = 27
        targetOrderItem.AdjustedCost = 5.555
        targetOrderItem.ReasonCodeDetailID = 9

        oiList.Add(targetOrderItem)

        'targetOrderItem = New OrderItem()
        'targetOrderItem.Item_Key = 279058
        'targetOrderItem.QuantityUnit = 27
        'targetOrderItem.QuantityOrdered = 1
        'targetOrderItem.DiscountType = 0
        'targetOrderItem.QuantityDiscount = 0
        'targetOrderItem.ReasonCodeDetailID = 0

        'oiList.Add(targetOrderItem)

        'targetOrderItem = New OrderItem()
        'targetOrderItem.Item_Key = 113825
        'targetOrderItem.QuantityUnit = 25
        'targetOrderItem.QuantityOrdered = 1
        'targetOrderItem.DiscountType = 0
        'targetOrderItem.QuantityDiscount = 0
        'targetOrderItem.ReasonCodeDetailID = 0
        'oiList.Add(targetOrderItem)

        targetOrder.OrderItems = oiList

        testResult = gatewayService.CreateTransferOrder(targetOrder)

        If testResult.IRMA_PONumber = -1 Then 'CreateOrder() failed!
            Assert.IsTrue(testResult.IRMA_PONumber <> -1, "FAIL")
        End If

        OrderHeader_ID = testResult.IRMA_PONumber

        Assert.IsTrue(testResult.IRMA_PONumber <> -1, "FAIL")
    End Sub

    <TestMethod()> _
    Public Sub CreateDSDOrderTest()
        Dim targetOrder As New Order()
        Dim oiList As New List(Of OrderItem)
        Dim targetOrderItem As OrderItem
        Dim OrderHeader_ID As Integer = Nothing
        Dim gatewayService As New GatewayService()
        Dim testResult As New Result()

        targetOrder.CreatedBy = 5922
        targetOrder.ProductType_ID = 1
        'targetOrder.OrderType_Id = 3
        targetOrder.Vendor_ID = 2872
        'targetOrder.Transfer_SubTeam = 190
        targetOrder.ReceiveLocation_ID = 40038
        targetOrder.PurchaseLocation_ID = 40038
        targetOrder.Transfer_To_SubTeam = 9
        targetOrder.Fax_Order = 0
        targetOrder.Expected_Date = Now()
        targetOrder.Return_Order = 0
        targetOrder.FromQueue = 0
        targetOrder.DSDOrder = 1
        targetOrder.Electronic_Order = 1


        targetOrderItem = New OrderItem()
        targetOrderItem.QuantityOrdered = 3
        targetOrderItem.Item_Key = 369491
        targetOrderItem.QuantityUnit = 2
        targetOrderItem.DiscountType = 0
        targetOrderItem.QuantityDiscount = 0
        targetOrderItem.ReasonCodeDetailID = 0

        oiList.Add(targetOrderItem)

        targetOrderItem = New OrderItem()

        targetOrderItem.QuantityOrdered = 5
        targetOrderItem.Item_Key = 369490
        targetOrderItem.QuantityUnit = 2
        targetOrderItem.DiscountType = 0
        targetOrderItem.QuantityDiscount = 0
        targetOrderItem.ReasonCodeDetailID = 0

        oiList.Add(targetOrderItem)

        'targetOrderItem = New OrderItem()
        'targetOrderItem.Item_Key = 113825
        'targetOrderItem.QuantityUnit = 25
        'targetOrderItem.QuantityOrdered = 1
        'targetOrderItem.DiscountType = 0
        'targetOrderItem.QuantityDiscount = 0
        'targetOrderItem.ReasonCodeDetailID = 0
        'oiList.Add(targetOrderItem)

        targetOrder.OrderItems = oiList

        testResult = gatewayService.CreateDSDOrder(targetOrder)

        If testResult.IRMA_PONumber = -1 Then 'CreateOrder() failed!
            Assert.IsTrue(testResult.IRMA_PONumber <> -1, "FAIL")
        End If

        OrderHeader_ID = testResult.IRMA_PONumber

        Assert.IsTrue(testResult.IRMA_PONumber <> -1, "FAIL")
    End Sub
    '#End Region






    '    '''<summary>
    '    '''A test for GetStoreItem
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub GetStoreItemTest()
    '        '12,10,2978,NULL,94611
    '        Dim target As IGateway = New GatewayService()
    '        Dim iStoreNo As Integer = 204
    '        Dim iTransferToSubteam_To As Integer = 4020
    '        Dim iUser_ID As Integer = 2978
    '        Dim iItem_Key As Integer = Nothing
    '        Dim sIdentifier As String = "29731900000"
    '        Dim expected As StoreItem = Nothing
    '        Dim actual As StoreItem
    '        actual = target.GetStoreItem(iStoreNo, iTransferToSubteam_To, iUser_ID, iItem_Key, sIdentifier)
    '        Assert.IsTrue(actual.CostedByWeight <> False, "FAIL")
    '        ' Assert.Inconclusive("Verify the correctness of this test method.")
    '    End Sub

    '    '''<summary>
    '    '''A test for GetItemUnits
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub GetItemUnitsTest()
    '        Dim target As IGateway = New GatewayService()
    '        Dim actual As List(Of Lists.ItemUnit)
    '        actual = target.GetItemUnits
    '        Assert.IsTrue(actual.Count > 1, "Fail")

    '    End Sub

    '    '''<summary>
    '    '''A test for GetStoreItemCycleCountInfo
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub GetStoreItemCycleCountInfoTest()
    '        Dim iStoreNo As Integer = 1
    '        Dim iTransferToSubteam_To As Integer = 10
    '        Dim iUser_ID As Integer = 2978
    '        Dim iItem_Key As Integer = Nothing
    '        Dim sIdentifier As String = "94611"
    '        Dim target As IGateway = New GatewayService()
    '        Dim si As StoreItem = target.GetStoreItem(iStoreNo, iTransferToSubteam_To, iUser_ID, iItem_Key, sIdentifier)
    '        Dim lInventoryLocationID As Long = 2
    '        Dim expected As CycleCountInfo = Nothing
    '        Dim actual As CycleCountInfo
    '        actual = target.GetStoreItemCycleCountInfo(si, lInventoryLocationID)
    '        Assert.IsTrue(actual.Quantity = 0, "fail")

    '    End Sub

    '    '''<summary>
    '    '''A test for GetItem
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub GetItemTest()
    '        Dim target As IGateway = New GatewayService() ' TODO: Initialize to an appropriate value
    '        Dim Item_Key As Integer = Nothing ' TODO: Initialize to an appropriate value
    '        Dim Identifier As String = "89958700036" ' TODO: Initialize to an appropriate value
    '        'Dim expected As List(Of Lists.GetItem) = Nothing ' TODO: Initialize to an appropriate value
    '        Dim actual As List(Of Lists.GetItem)
    '        actual = target.GetItem(Nothing, Identifier)
    '        Assert.IsTrue(actual.Item(0).PackageDesc2 = 2.2, "Fail")
    '        'Assert.Inconclusive("Verify the correctness of this test method.")
    '    End Sub

    '    '''<summary>
    '    '''A test for GetOrder
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub GetOrderTest()
    '        Dim target As IGateway = New GatewayService() ' TODO: Initialize to an appropriate value
    '        'Dim expected As List(Of Lists.GetItem) = Nothing ' TODO: Initialize to an appropriate value
    '        Dim lOrderID As Long = 234
    '        Dim actual As IRMA.Order
    '        Actual = target.GetOrder(lOrderID)
    '        Assert.IsTrue(actual.OrderHeader_ID = 123344, "Fail")
    '        'Assert.Inconclusive("Verify the correctness of this test method.")
    '    End Sub

    '    '''<summary>
    '    '''A test for GetExternalOrder
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub GetExternalOrderTest()
    '        Dim target As IGateway = New GatewayService() ' TODO: Initialize to an appropriate value
    '        'Dim expected As List(Of Lists.GetItem) = Nothing ' TODO: Initialize to an appropriate value
    '        Dim lExternalOrderID As Long = 2375995
    '        Dim iStore_No = 402
    '        Dim actual As List(Of Lists.ExternalOrder)
    '        actual = target.GetExternalOrders(lExternalOrderID, iStore_No)
    '        Assert.IsTrue(actual(0).OrderHeader_ID = 234, "Fail")
    '        'Assert.Inconclusive("Verify the correctness of this test method.")
    '    End Sub


    '    '''<summary>
    '    '''A test for Get Item Movement
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub GetItemMovementTest()
    '        Dim target As IGateway = New GatewayService()
    '        Dim actual As List(Of Lists.ItemMovement)
    '        actual = target.GetItemMovement(10008, 2700, 3, "73898527730")
    '        Assert.IsTrue(actual.Count > 1, "Success")
    '    End Sub

    '    <TestMethod()> _
    '    Public Sub GetCycleCountMasterTest()
    '        Dim target As IGateway = New GatewayService()
    '        Dim actual As CycleCount
    '        actual = target.GetCycleCount(10215, 4900)
    '        Assert.IsTrue(actual Is Nothing, "Success")
    '    End Sub

    '<TestMethod()> _
    'Public Sub GetUserRolesTest()
    '    Dim target As IGateway = New GatewayService()
    '    Dim UserName As String = "Amudha.Sethuraman"
    '    Dim actual As New List(Of Security.UserRole)

    '    actual = target.GetUserRole(UserName)
    '    Assert.IsTrue(actual Is Nothing, "Success")
    'End Sub

    <TestMethod()> _
    Public Sub IsDSDVendorByUPCTest()
        Dim target As IGateway = New GatewayService()
        Dim UPC As String = "9948228363"
        Dim StoreNo As Integer = 101
        Dim actual As Boolean

        actual = target.IsDSDStoreVendorByUPC(UPC, StoreNo)
        Assert.IsTrue(actual, "Success")
    End Sub

End Class
