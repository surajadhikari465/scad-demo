'********************************************************************************************************************************************************************************
'Ripe Import Warning Summary
'Displays a warning window if tried to re-import already imported orders and reprints the invoices if selected "Yes"

'CREATED_BY         CREATED_DATE        FUNCTION_NAME                       FUNCTION_SUMMARY 
'--------------------------------------------------------------------------------------------

'UPDATED_BY         UPDATED_DATE        UPDATED_FUNCTION_NAME               UPDATION_SUMMARY
'--------------------------------------------------------------------------------------------
' vayals            12/14/09            cmdYes_Click                        Replaced old database function calls to DATAFACTORY
'********************************************************************************************************************************************************************************

Option Strict Off
Option Explicit On
Imports log4net
Imports System.Configuration
Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Common
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Tools.BusinessLogic



Friend Class frmRipeImportWarning
    Inherits System.Windows.Forms.Form
    Private m_bCancel As Boolean
    Private mdt As DataTable
    Private m_aryOrderdata(0, 0) As String
    Private Sub SetupDataTable()
        ' Create a data table
        mdt = New DataTable("DupOrders")
        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("IRS_PO", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("RIPEOrderName", GetType(String)))
        mdt.Columns.Add(New DataColumn("RipeCustomer", GetType(String)))
        mdt.Columns.Add(New DataColumn("ReturnOrderCount", GetType(Integer)))
    End Sub

    Private Sub LoadDataTable(ByRef aryData(,) As String)
        Dim row As DataRow
        Dim iLoop As Integer
        Dim MaxLoop As Short = 1000
        'Load the data set.
        mdt.Rows.Clear()
        m_aryOrderdata = aryData
        For iLoop = 1 To UBound(m_aryOrderdata, 2)
            row = mdt.NewRow
            row("IRS_PO") = m_aryOrderdata(0, iLoop)
            row("RIPEOrderName") = m_aryOrderdata(1, iLoop)
            row("RipeCustomer") = m_aryOrderdata(2, iLoop)
            row("ReturnOrderCount") = m_aryOrderdata(3, iLoop)
            mdt.Rows.Add(row)
        Next iLoop
        'Setup a column that you would like to sort on initially.
        mdt.AcceptChanges()
        ugrdAlreadyImported.DataSource = mdt
        'This may or may not be required.
        If ugrdAlreadyImported.Rows.Count > 0 Then
            'Set the first item to selected.
            ugrdAlreadyImported.Rows(0).Selected = True
        End If
ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub

    Public Sub New(ByRef aryData(,) As String, ByRef sDistDate As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        'Call WriteMasterLog("begin Set Up Data Table")
        Call SetupDataTable()
        'Call WriteMasterLog("end Set Up Data Table")
        'Call WriteMasterLog("begin Load Data Table")
        Call LoadDataTable(aryData)
        'Call WriteMasterLog("end Load Data Table")
        cmdYes.Tag = sDistDate
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        m_bCancel = True
        Me.Hide()
    End Sub

    Private Sub cmdNo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNo.Click
        m_bCancel = False
        Me.Hide()
    End Sub

    Private Sub cmdYes_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdYes.Click
        Dim dblTotTime As Double
        Dim iCnt As Short
        Dim lIgnoreErrNum(0) As Integer, lPOsWithROs(0) As Integer
        lIgnoreErrNum(0) = 50002
        m_bCancel = False
        'check for and display any selected POs that have Return Orders associated with them
        'Call WriteMasterLog("begin Check For Return Orders")
        For iCnt = 0 To ugrdAlreadyImported.Selected.Rows.Count - 1
            If ugrdAlreadyImported.Selected.Rows(iCnt).Cells("ReturnOrderCount").Value > 0 Then
                If Array.IndexOf(lPOsWithROs, ugrdAlreadyImported.Selected.Rows(iCnt).Cells("IRS_PO").Value) = -1 Then
                    ReDim Preserve lPOsWithROs(UBound(lPOsWithROs) + 1)
                    lPOsWithROs(UBound(lPOsWithROs)) = ugrdAlreadyImported.Selected.Rows(iCnt).Cells("IRS_PO").Value
                End If
            End If
        Next
        'Call WriteMasterLog("end Check For Return Orders")
        'if any of the select POs have Return orders inform the user and do not continue with the re-import process
        lPOsWithROs(0) = UBound(lPOsWithROs)
        If lPOsWithROs(0) > 0 Then
            Dim iPOs As Integer, sPOList As String = lPOsWithROs(1)
            For iPOs = 2 To UBound(lPOsWithROs)
                sPOList = sPOList & ", " & lPOsWithROs(iPOs)
            Next
            sPOList = sPOList & "."
            System.Windows.Forms.MessageBox.Show(lPOsWithROs(0) & " of the POs you have selected are associated with Return Orders.  " & _
                                                 "You must delete the Return Orders associated with the following POs before you can " & _
                                                 "re-import these orders:  " & sPOList, "Can Not Import Credits", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            m_bCancel = True
        Else
            On Error Resume Next
            'delete all orders contained in grid
            dblTotTime = Microsoft.VisualBasic.DateAndTime.Timer
            'Call WriteMasterLog("Begin Delte selected Orders")
            For iCnt = 0 To ugrdAlreadyImported.Selected.Rows.Count - 1
                'Using DATAFACTORY for all db method calls
                'Dim df As New DataFactory(DataFactory.dbRIPE)
                Dim df As New DataFactory(DataFactory.ItemCatalog)
                Dim paramList As New ArrayList
                Dim currentParam As DBParam
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = ugrdAlreadyImported.Selected.Rows(iCnt).Cells("IRS_PO").Value
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = giUserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)
                df.ExecuteStoredProcedure("DeleteOrderHeader", paramList)
                If Err.Number = 50002 Then
                    'ignore the "ordering window"
                    'nop
                End If
                Dim paramList1 As New ArrayList
                Dim currentParam1 As DBParam
                currentParam1 = New DBParam
                currentParam1.Name = "IRSOrderHeaderID"
                currentParam1.Value = ugrdAlreadyImported.Selected.Rows(iCnt).Cells("IRS_PO").Value
                currentParam1.Type = DBParamType.Int
                paramList1.Add(currentParam1)
                df.ExecuteStoredProcedure("RIPEDeleteIRSOrderHistory", paramList1)
            Next iCnt
            'Call WriteMasterLog("end Delte selected Orders (Total time: " & Microsoft.VisualBasic.DateAndTime.Timer - dblTotTime & ")")
        End If
        Me.Hide()
    End Sub

    Private Sub frmRipeImportWarning_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Call CenterForm(Me)
    End Sub

    Private Sub frmRipeImportWarning_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
        If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
            m_bCancel = True
            Me.Hide()
        End If
        eventArgs.Cancel = Cancel
    End Sub

    Public ReadOnly Property Cancel() As Boolean
        Get
            Cancel = m_bCancel
        End Get
    End Property

End Class