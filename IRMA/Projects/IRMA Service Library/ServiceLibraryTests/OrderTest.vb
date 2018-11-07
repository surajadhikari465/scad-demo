Imports System

Imports WholeFoods.ServiceLibrary.IRMA

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports WholeFoods.ServiceLibrary



'''<summary>
'''This is a test class for OrderTest and is intended
'''to contain all OrderTest Unit Tests
'''</summary>
<TestClass()> _
Public Class OrderTest


    Private testContextInstance As TestContext

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
    '#End Region


    '    '''<summary>
    '    '''A test for Order Constructor
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub OrderConstructorTest()
    '        Dim OrderHeader_ID As Object = 1833960 ' TODO: Initialize to an appropriate value
    '        Dim target As IRMA.Order = New IRMA.Order(OrderHeader_ID)


    '        Assert.IsTrue(target.CompanyName <> Nothing, "FAIL")
    '    End Sub

    '    '''<summary>
    '    '''A test for OrderItes Constructor
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub OrderItemsConstructorTest()
    '        Dim OrderHeader_ID As Object = 1833960 ' TODO: Initialize to an appropriate value
    '        Dim target As IRMA.Order = New IRMA.Order(OrderHeader_ID)


    '        Assert.IsTrue(target.OrderItems.Count > 0, "FAIL")
    '    End Sub

    '    '''<summary>
    '    '''A test for GetOrderItem
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub GetOrderItemTest()
    '        Dim target As Order = New Order(1833960)
    '        Dim lItem_Key As Long = 141027

    '        Dim actual As OrderItem
    '        actual = target.GetOrderItem(lItem_Key)
    '        Assert.IsTrue(actual.OrderItem_ID <> Nothing, "FAIL")

    '    End Sub

    '    '''<summary>
    '    '''A test for GetOrderItem
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub GetOrderItemTest1()
    '        Dim target As Order = New Order(1833960)
    '        Dim sIdentifier As String = "48000012357"

    '        Dim actual As OrderItem
    '        actual = target.GetOrderItem(sIdentifier)
    '        Assert.IsTrue(actual.OrderItem_ID <> Nothing, "FAIL")
    '    End Sub

    '    '''<summary>
    '    '''A test for GetOrderItem
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub GetOrderItemTest2()
    '        Dim target As Order = New Order(1833960)
    '        Dim sIdentifier As String = "48000012357"
    '        Dim dPackage_Desc1 As [Decimal] = New [Decimal](48.0)

    '        Dim actual As OrderItem
    '        actual = target.GetOrderItem(sIdentifier, dPackage_Desc1)
    '        Assert.IsTrue(actual.OrderItem_ID <> Nothing, "FAIL")
    '    End Sub

    '''<summary>
    '''A test for CreateOrder
    '''</summary>
    <TestMethod()> _
    Public Sub CreateSendReceiveCloseOrderTest()
        Dim targetOrder As New Order()
        Dim oiList As New List(Of OrderItem)
        Dim targetOrderItem As OrderItem
        Dim OrderHeader_ID As Integer = Nothing

        targetOrder.Vendor_ID = 185 '5165 '5146 
        targetOrder.OrderType_Id = 1 '3
        targetOrder.ProductType_ID = 1 '2
        targetOrder.PurchaseLocation_ID = 419 '6950
        targetOrder.ReceiveLocation_ID = 419 '6950
        'targetOrder.Transfer_SubTeam = 2400 'DBNull.Value 
        targetOrder.Transfer_To_SubTeam = 1 '2400 '3500
        targetOrder.Fax_Order = False
        targetOrder.Expected_Date = Now()
        targetOrder.CreatedBy = 398
        targetOrder.Return_Order = True
        targetOrder.FromQueue = False
        'targetOrder.SupplyTransferToSubTeam = dbNull.value
        targetOrder.DSDOrder = True 'False 

        targetOrderItem = New OrderItem()
        targetOrderItem.Item_Key = 20240 '113822 '233637 
        targetOrderItem.QuantityUnit = 37 '25
        targetOrderItem.QuantityOrdered = 2
        targetOrderItem.DiscountType = 0
        targetOrderItem.QuantityDiscount = 0
        targetOrderItem.ReasonCodeDetailID = 0

        'targetOrder.Vendor_ID = 5058 '5165 '5146 
        'targetOrder.OrderType_Id = 1 '3
        'targetOrder.ProductType_ID = 1 '2
        'targetOrder.PurchaseLocation_ID = 5146 '6950
        'targetOrder.ReceiveLocation_ID = 5146 '6950
        ''targetOrder.Transfer_SubTeam = 2400 'DBNull.Value 
        'targetOrder.Transfer_To_SubTeam = 1400 '2400 '3500
        'targetOrder.Fax_Order = False
        'targetOrder.Expected_Date = Now()
        'targetOrder.CreatedBy = 6775
        'targetOrder.Return_Order = True
        'targetOrder.FromQueue = False
        ''targetOrder.SupplyTransferToSubTeam = dbNull.value
        'targetOrder.DSDOrder = True 'False 

        'targetOrderItem = New OrderItem()
        'targetOrderItem.Item_Key = 424290 '113822 '233637 
        'targetOrderItem.QuantityUnit = 1 '25
        'targetOrderItem.QuantityOrdered = 1
        'targetOrderItem.DiscountType = 0
        'targetOrderItem.QuantityDiscount = 0
        'targetOrderItem.ReasonCodeDetailID = 0

        oiList.Add(targetOrderItem)

        'targetOrderItem = New OrderItem()
        'targetOrderItem.Item_Key = 113823
        'targetOrderItem.QuantityUnit = 25
        'targetOrderItem.QuantityOrdered = 2
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

        targetOrder.ResultObject = targetOrder.CreateOrder()
        If targetOrder.ResultObject.IRMA_PONumber = -1 Then 'CreateOrder() failed!
            Assert.IsTrue(targetOrder.ResultObject.IRMA_PONumber <> -1, "FAIL")
        End If

        targetOrder.ResultObject = targetOrder.SendOrder()
        If targetOrder.ResultObject.IRMA_PONumber = -1 Then 'SendOrder() failed!
            Assert.IsTrue(targetOrder.ResultObject.IRMA_PONumber <> -1, "FAIL")
        End If

        targetOrder.ResultObject = targetOrder.SendElectronicOrder()
        If targetOrder.ResultObject.IRMA_PONumber = -1 Then 'SendElectronicOrder() failed!
            Assert.IsTrue(targetOrder.ResultObject.IRMA_PONumber <> -1, "FAIL")
        End If

        targetOrder.ResultObject = targetOrder.ReceiveOrder()
        If targetOrder.ResultObject.IRMA_PONumber = -1 Then 'ReceiveOrder() failed!
            Assert.IsTrue(targetOrder.ResultObject.IRMA_PONumber <> -1, "FAIL")
        End If

        targetOrder.ResultObject = targetOrder.CloseOrder(targetOrder.OrderHeader_ID, targetOrder.CreatedBy)
        If targetOrder.ResultObject.IRMA_PONumber = -1 Then 'CloseOrder() failed!
            Assert.IsTrue(targetOrder.ResultObject.IRMA_PONumber <> -1, "FAIL")
        End If

        OrderHeader_ID = targetOrder.OrderHeader_ID

        Assert.IsTrue(targetOrder.ResultObject.IRMA_PONumber <> -1, "FAIL")
    End Sub

    ' '''<summary>
    ' '''A test for SendReceiveCloseOrder
    ' '''</summary>
    '<TestMethod()> _
    'Public Sub SendReceiveCloseOrderTest()
    '    Dim targetOrder As New Order()
    '    Dim status As Boolean

    '    targetOrder.OrderHeader_ID = 4459314
    '    targetOrder.Fax_Order = False
    '    targetOrder.Email_Order = False
    '    targetOrder.Electronic_Order = True
    '    targetOrder.OverrideTransmissionMethod = False

    '    status = targetOrder.SendOrder()

    '    'TODO add receving line items and close order here

    '    Assert.IsTrue(status <> False, "FAIL")
    'End Sub
    <TestMethod()> _
    Public Sub TestLogger()
        Dim GS As GatewayService = New GatewayService()
        Dim strtest As String = GS.LoggerTest()

    End Sub
End Class
