Imports System.Collections.Generic

Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports WholeFoods.ServiceLibrary.IRMA



'''<summary>
'''This is a test class for CycleCountTest and is intended
'''to contain all CycleCountTest Unit Tests
'''</summary>
<TestClass()> _
Public Class CycleCountTest


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
    '''A test for GetCycleCount
    '''</summary>
    <TestMethod()> _
    Public Sub GetCycleCountTest()
        Dim target As CycleCount = New CycleCount() ' TODO: Initialize to an appropriate value
        Dim lStoreNo As Long = 101 ' TODO: Initialize to an appropriate value
        Dim lSubTeamNo As Long = 2800 ' TODO: Initialize to an appropriate value
        Dim expected As CycleCount = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As CycleCount
        actual = target.GetCycleCount(lStoreNo, lSubTeamNo)

        Assert.IsTrue(actual.ID > 0, "FAIL")

    End Sub

    '''<summary>
    '''A test for CreateCycleCountHeader
    '''</summary>
    <TestMethod()> _
    Public Sub CreateCycleCountHeaderTest()
        Dim target As CycleCount = New CycleCount()
        Dim lMasterCountID As Long = 21058
        Dim dStartScan As DateTime = Today.Date
        Dim lInventoryLocationId As Long = 0 '
        Dim bExternal As Boolean = False
        Dim actual As Object
        actual = target.CreateCycleCountHeader(lMasterCountID, dStartScan, lInventoryLocationId, bExternal)

        Assert.IsTrue(CType(actual, InternalCycleCountHeader).ID = 0, "FAIL")
    End Sub

    '''<summary>
    '''A test for AddCycleCountItem
    '''</summary>
    <TestMethod()> _
    Public Sub AddCycleCountItemTest()
        Dim target As CycleCount = New CycleCount() ' TODO: Initialize to an appropriate value
        Dim lItemKey As Long = 0 ' TODO: Initialize to an appropriate value
        Dim dQuantity As [Decimal] = New [Decimal]() ' TODO: Initialize to an appropriate value
        Dim dWeight As [Decimal] = New [Decimal]() ' TODO: Initialize to an appropriate value
        Dim dPackSize As [Decimal] = New [Decimal]() ' TODO: Initialize to an appropriate value
        Dim bIsCaseCnt As Boolean = False ' TODO: Initialize to an appropriate value
        Dim lCycleCountID As Long = 0 ' TODO: Initialize to an appropriate value
        Dim lInvLocID As Long = 0 ' TODO: Initialize to an appropriate value
        target.AddCycleCountItem(lItemKey, dQuantity, dWeight, dPackSize, bIsCaseCnt, lCycleCountID, lInvLocID)
        Assert.Inconclusive("A method that does not return a value cannot be verified.")
    End Sub

    '''<summary>
    '''A test for GetInventorylocations
    '''</summary>
    <TestMethod()> _
    Public Sub GetInventorylocationsTest()
        Dim target As CycleCount = New CycleCount() ' TODO: Initialize to an appropriate value
        Dim lStoreNo As Long = 10008 ' TODO: Initialize to an appropriate value
        Dim lSubTeamNo As Long = 3700 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of InventoryLocation) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of InventoryLocation)
        actual = target.GetInventoryLocations(lStoreNo, lSubTeamNo)

        Assert.IsTrue(actual.Count > 0, "Fail")
    End Sub
End Class
