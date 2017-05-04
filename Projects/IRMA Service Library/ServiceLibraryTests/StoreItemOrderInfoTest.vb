Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports WholeFoods.ServiceLibrary.IRMA



'''<summary>
'''This is a test class for StoreItemOrderInfoTest and is intended
'''to contain all StoreItemOrderInfoTest Unit Tests
'''</summary>
<TestClass()> _
Public Class StoreItemOrderInfoTest


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
    '''A test for GetStoreItemOrderInfo
    '''</summary>
    <TestMethod()> _
    Public Sub GetStoreItemOrderInfoTest()
        Dim target As StoreItemOrderInfo = New StoreItemOrderInfo() ' TODO: Initialize to an appropriate value
        Dim iStoreNo As Integer = 12
        Dim iTransferToSubTeamNo As Integer = 10
        Dim iItemKey As Integer = 10666
        Dim expected As StoreItemOrderInfo = Nothing
        Dim actual As StoreItemOrderInfo
        actual = target.GetStoreItemOrderInfo(iStoreNo, iTransferToSubTeamNo, iItemKey)
        Assert.IsTrue(actual.QtyOnOrder >= 0, "Fail")
    End Sub
End Class
