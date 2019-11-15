Option Strict Off
Option Explicit On

Imports log4net
Imports System.Data
Imports WholeFoods.Utility.DataAccess

Friend Class frmOrdersAllocateOrderList
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private mdt As DataTable
    Public mbOrderWindowDisplayed As Boolean
    Public WhereClause As String
    Private mFactory As New DataFactory(DataFactory.ItemCatalog)

    Private Sub frmOrdersAllocateOrderList_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
    End Sub

    Private Sub frmOrdersAllocateOrderList_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        logger.Debug("frmOrdersAllocateOrderList_Shown Entry")

        ' Task 6450 (Rick) begin
        Dim sSQL As String
        Dim dr As SqlClient.SqlDataReader
        Dim row As DataRow

        sSQL = "SELECT DISTINCT OrderHeader_ID, CompanyName FROM tmpOrdersAllocateOrderItems oi " _
                & "INNER JOIN tmpOrdersAllocateItems i ON oi.Item_Key = i.Item_Key " _
                & WhereClause & " ORDER BY CompanyName, OrderHeader_ID"
        dr = mFactory.GetDataReader(sSQL)

        mdt.Clear()
        While dr.Read
            row = mdt.NewRow
            row("OrderHeader_ID") = dr.Item("OrderHeader_ID")
            row("CompanyName") = dr.Item("CompanyName")
            mdt.Rows.Add(row)
        End While

        mdt.AcceptChanges()
        ugrdOrders.DataSource = mdt

        logger.Debug("frmOrdersAllocateOrderList_Shown Exit")
    End Sub

    Private Sub frmOrdersAllocateOrderList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrdersAllocateOrderList_Load Entry")

        mdt = New DataTable("OrdersAllocOrderList")
        mdt.Columns.Add(New DataColumn("OrderHeader_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("CompanyName", GetType(String)))
        Global.SetUltraGridSelectionStyle(ugrdOrders)
        logger.Debug("frmOrdersAllocateOrderList_Load Exit")
    End Sub
	
	Private Sub frmOrdersAllocateOrderList_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmOrdersAllocateOrderList_FormClosing Entry")
        Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		
		If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
			Cancel = True
			Me.Hide()
		End If
		
        eventArgs.Cancel = Cancel
        logger.Debug("frmOrdersAllocateOrderList_FormClosing Exit")
    End Sub

    Private Sub cmdSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
        logger.Debug("cmdSelect_Click Entry")

        If ugrdOrders.Selected.Rows.Count = 0 Then
            glOrderHeaderID = CInt(ugrdOrders.ActiveRow.Cells("OrderHeader_ID").Value)
        Else
            glOrderHeaderID = CInt(ugrdOrders.Selected.Rows(0).Cells("OrderHeader_ID").Value)
        End If

        bSpecificOrder = True

        frmOrders.ShowDialog()
        frmOrders.Dispose()
        bSpecificOrder = False

        mbOrderWindowDisplayed = True

        logger.Debug("cmdSelect_Click Exit")
    End Sub

    Private Sub ugrdOrders_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdOrders.DoubleClickRow
        logger.Debug("ugrdOrders_DoubleClickRow Entry")

        Call cmdSelect_Click(cmdSelect, New System.EventArgs())

        logger.Debug("ugrdOrders_DoubleClickRow Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Hide()
    End Sub
End Class