Imports System

Imports WholeFoods.ServiceLibrary.IRMA

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports WholeFoods.ServiceLibrary



'''<summary>
'''This is a test class for OrderTest and is intended
'''to contain all OrderTest Unit Tests
'''</summary>
<TestClass()> _
Public Class POSPushTest


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
            testContextInstance = value
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
    '''A test for the POS Push
    '''</summary>
    <TestMethod()> _
    Public Sub RunPOSPushTest()
        Dim push As New POSPush
        Dim retval = push.RunPOSPush("C:\POSPush\POSPush.exe", "FL", "Data Source=idt-fl\flt;Initial Catalog=ItemCatalog_Test;User Id=IRSUser;Password=Sup#rm@n!;", "enteremailaddresshere@wholefoods.com")

        Assert.IsTrue(retval, "FAIL")
    End Sub
End Class
