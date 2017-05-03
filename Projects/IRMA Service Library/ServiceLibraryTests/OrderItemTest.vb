Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports WholeFoods.ServiceLibrary.IRMA



'''<summary>
'''This is a test class for OrderItemTest and is intended
'''to contain all OrderItemTest Unit Tests
'''</summary>
<TestClass()> _
Public Class OrderItemTest


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
    '#End Region


    '    '''<summary>
    '    '''A test for Receive
    '    '''</summary>
    '    <TestMethod()> _
    '    Public Sub ReceiveTest()
    '        Dim OrderHeader_ID As Object = 1833960
    '        Dim targetOrder As Order = New Order(OrderHeader_ID)
    '        Dim target As OrderItem = targetOrder.OrderItems.First
    '        Dim dQuantity As [Decimal] = New [Decimal](1)
    '        Dim dWeight As [Decimal] = New [Decimal](0)
    '        Dim dtDate As DateTime = DateTime.Now
    '        Dim bCorrection As Boolean = False
    '        Dim dPackSize As [Decimal] = New [Decimal](48.0)
    '        Dim UserID As Long = 0 '
    '        'target.Receive(dQuantity, dWeight, dtDate, bCorrection, dPackSize, UserID)
    '        Assert.IsTrue(target.Cost > 0, "FAIL")
    '    End Sub
End Class
