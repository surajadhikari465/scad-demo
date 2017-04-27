Imports System

Imports System.Data

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports WholeFoods.ServiceLibrary.IRMA



'''<summary>
'''This is a test class for StoreItemTest and is intended
'''to contain all StoreItemTest Unit Tests
'''</summary>
<TestClass()> _
Public Class StoreItemTest


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = Value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region


    '''<summary>
    '''A test for StoreItem Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub StoreItemConstructorTest()
        Dim dt As DataTable = Nothing ' TODO: Initialize to an appropriate value
        Dim target As StoreItem = New StoreItem(dt)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for StoreItem Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub StoreItemConstructorTest1()
        Dim target As StoreItem = New StoreItem()
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for AddToOrderQueue
    '''</summary>
    <TestMethod()> _
    Public Sub AddToOrderQueueTest()
        Dim target As StoreItem = New StoreItem()
        target = target.GetStoreItem(12, 10, 2978, Nothing, "94611")
        Dim IsTransfer As Boolean = False
        Dim IsCredit As Boolean = True
        Dim Quantity As [Decimal] = New [Decimal](2)
        Dim UnitID As Integer = 2
        Dim iUser_ID As Integer = 2978
        Dim expected As Boolean = True
        Dim actual As Boolean
        'actual = target.AddToOrderQueue(IsTransfer, IsCredit, Quantity, UnitID, iUser_ID, target)
        Assert.IsTrue(expected = actual, "FAIL")

    End Sub

    ' '''<summary>
    ' '''A test for AddToReprintSignQueue
    ' '''</summary>
    '<TestMethod()> _
    'Public Sub AddToReprintSignQueueTest()
    '    Dim target As StoreItem = New StoreItem() ' TODO: Initialize to an appropriate value
    '    Dim lUser_ID As Long = 0 ' TODO: Initialize to an appropriate value
    '    Dim iSourceType As Integer = 0 ' TODO: Initialize to an appropriate value
    '    target.AddToReprintSignQueue(lUser_ID, iSourceType)
    '    Assert.Inconclusive("A method that does not return a value cannot be verified.")
    'End Sub

    '''<summary>
    '''A test for GetStoreItem
    '''</summary>
    <TestMethod()> _
    Public Sub GetStoreItemTest()
        Dim target As StoreItem = New StoreItem() ' TODO: Initialize to an appropriate value
        Dim iStoreNo As Integer = 10215
        Dim iTransferToSubteam_To As Integer = 10
        Dim iUser_ID As Integer = 2978
        Dim iItem_Key As Integer = 286839
        Dim sIdentifier As String = "29731900000"
        Dim expected As StoreItem = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As StoreItem
        actual = target.GetStoreItem(iStoreNo, iTransferToSubteam_To, iUser_ID, iItem_Key, sIdentifier)
         Assert.IsTrue(actual.ItemKey <> Nothing, "FAIL")
    End Sub



    '''<summary>
    '''A test for AddToReprintSignQueue
    '''</summary>
    <TestMethod()> _
    Public Sub AddToReprintSignQueueTest1()
        Dim target As StoreItem = New StoreItem() ' TODO: Initialize to an appropriate value
        Dim lUser_ID As Long = 1 ' TODO: Initialize to an appropriate value
        Dim iSourceType As Integer = 1 ' TODO: Initialize to an appropriate value
        Dim sItemList As String = "10666" ' TODO: Initialize to an appropriate value
        Dim cItemListSeperator As String = "|" ' TODO: Initialize to an appropriate value
        Dim iStoreNo As Integer = 12 ' TODO: Initialize to an appropriate value
        target.AddToReprintSignQueue(lUser_ID, iSourceType, sItemList, cItemListSeperator, iStoreNo)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for GetStoreItemCycleCountInfo
    '''</summary>
    <TestMethod()> _
    Public Sub GetStoreItemCycleCountInfoTest()
        Dim target As StoreItem = New StoreItem() ' TODO: Initialize to an appropriate value
        Dim lInventoryLocationID As Long = 0 ' TODO: Initialize to an appropriate value
        Dim expected As CycleCountInfo = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As CycleCountInfo
        actual = target.GetStoreItemCycleCountInfo(lInventoryLocationID)
        Assert.AreEqual(expected, actual)
        Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
