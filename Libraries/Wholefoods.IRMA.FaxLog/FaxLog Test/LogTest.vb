Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports System.Xml.Linq

Imports WholeFoods.IRMA.FaxLog

'''<summary>
'''This is a test class for LogEntryTest and is intended
'''to contain all LogEntryTest Unit Tests
'''</summary>
<TestClass()> _
Public Class LogTest

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
    <ClassInitialize()> _
    Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)

        Log.FaxLogLocation = "TransmissionLog.xml"

        Log.RetentionPolicy = 30
        Log.ExpirationThreshold = 30

    End Sub
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
    '''A test for CreateEntry
    '''</summary>
    <TestMethod()> _
    Public Sub CreateEntryTest()
        Dim PO As Integer = 11114
        Dim Destination As String = "5104287479"
        Log.CreateEntry(PO, Destination)
        Dim actual As Log.LogEntry = Log.GetEntry(PO)
        Assert.AreEqual(PO, actual.PO)
        Assert.AreEqual(Destination, actual.Destination)
    End Sub

    '''<summary>
    '''A test for CreateEntry to be used by other tests methods to setup scenarios.
    '''</summary>
    Private Sub CreateEntryTest(ByVal POID As Integer, ByVal Destination As String)
        Dim PO As Integer = POID
        Log.CreateEntry(PO, Destination)
        Dim actual As Log.LogEntry = Log.GetEntry(PO)
        Assert.AreEqual(PO, actual.PO)
        Assert.AreEqual(Destination, actual.Destination)
    End Sub

    '''<summary>
    '''A test for GetEntry
    '''</summary>
    <TestMethod()> _
    Public Sub GetEntryTest()
        Dim PO As Integer = 11114
        Dim actual As Log.LogEntry
        actual = Log.GetEntry(PO)
        Assert.IsNotNull(actual.PO)
        Assert.IsNotNull(actual.Attempts)
        Assert.IsNotNull(actual.Destination)
        Assert.IsNotNull(actual.ResentAttempt)
        Assert.IsNotNull(actual.Response)
        Assert.IsNotNull(actual.Status)
        Assert.IsNotNull(actual.Timestamp)
    End Sub

    '''<summary>
    '''A test for UpdateStatus
    '''</summary>
    <TestMethod()> _
    Public Sub UpdateStatusTestFail()
        Dim PO As Integer = 11114
        Dim Response As String = "Number Busy"
        Dim Destination As String = "5104287579"
        Dim Status As FaxStatus = FaxStatus.TransmissionFailure
        Log.UpdateStatus(PO, Destination, Response, Status)
        Dim actual As Log.LogEntry = Log.GetEntry(PO)
        Assert.AreEqual(PO, actual.PO)
        Assert.AreEqual(Response, actual.Response)
        Assert.AreEqual(Status.ToString, actual.Status.ToString)
    End Sub

    '''<summary>
    '''A test for UpdateStatus
    '''</summary>
    <TestMethod()> _
    Public Sub UpdateStatusTestSuccess()
        Dim PO As Integer = 11116
        Dim Response As String = "Success"
        Dim Status As FaxStatus = FaxStatus.TransmissionSuccess
        Dim Destination As String = "5104287579"
        Log.UpdateStatus(PO, Destination, Response, Status)
        Dim actual As Log.LogEntry = Log.GetEntry(PO)
        Assert.AreEqual(PO, actual.PO)
        Assert.AreEqual(Response, actual.Response)
        Assert.AreEqual(Status.ToString, actual.Status.ToString)
    End Sub

    '''<summary>
    '''A test for UpdateStatus to test other conditions.
    '''</summary>
    Private Sub UpdateStatusTestSent(ByVal POID)
        Dim PO As Integer = POID
        Dim Response As String = ""
        Dim Status As FaxStatus = FaxStatus.TransmissionSent
        Dim Destination As String = "xxx-xxx-xxxx"
        Log.UpdateStatus(PO, Destination, Response, Status)
        Dim actual As Log.LogEntry = Log.GetEntry(PO)
        Assert.AreEqual(PO, actual.PO)
        Assert.AreEqual(Response, actual.Response)
        Assert.AreEqual(Status.ToString, actual.Status.ToString)
    End Sub

    '''<summary>
    '''A test for GetUnconfirmedTransmissions
    '''</summary>
    <TestMethod()> _
    Public Sub GetUnconfirmedTransmissionsTest()

        Log.RetentionPolicy = 0
        Log.ExpirationThreshold = 0

        Log.PurgeTransmissionHistory()

        CreateEntryTest(1000, "xxx-xxx-xxxx")
        UpdateStatusTestSent(1000)
        CreateEntryTest(1001, "xxx-xxx-xxxx")
        UpdateStatusTestSent(1001)
        CreateEntryTest(1002, "xxx-xxx-xxxx")
        UpdateStatusTestSent(1002)

        Dim expected As Integer = 3
        Dim doc As Data.DataTable
        doc = Log.GetUnconfirmedTransmissions()
        Dim actual As Integer = doc.Rows.Count
        Assert.AreEqual(expected, actual)

    End Sub

End Class
