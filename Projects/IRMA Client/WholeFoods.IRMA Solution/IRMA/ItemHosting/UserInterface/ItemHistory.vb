Option Strict Off
Option Explicit On
Friend Class frmItemHistory
	Inherits System.Windows.Forms.Form
	
    Private m_sIdentifier As String
    Private mdt As DataTable
    Private mdv As DataView

    Private Sub frmItemHistory_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Call SetupDataTable()
        Call LoadDataTable()

    End Sub

    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New DataTable("ItemHistory")

        'All Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Order", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Date", GetType(Date)))
        mdt.Columns.Add(New DataColumn("Vendor", GetType(String)))
        mdt.Columns.Add(New DataColumn("Recv Loc", GetType(String)))
        mdt.Columns.Add(New DataColumn("Qty Recv", GetType(String)))
        mdt.Columns.Add(New DataColumn("Date Recv", GetType(Date)))
        mdt.Columns.Add(New DataColumn("Cost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Discount", GetType(String)))
        mdt.Columns.Add(New DataColumn("Freight", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Adjusted", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Landed", GetType(Decimal)))

    End Sub

    Private Sub LoadDataTable()
        Dim rsItemHistory As DAO.Recordset = Nothing
        Dim row As DataRow

        Try
            rsItemHistory = SQLOpenRecordSet("EXEC GetItemHistory '" & m_sIdentifier & "', null, null, 0, null, null", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            'Load the data set.
            mdt.Rows.Clear()

            While (Not rsItemHistory.EOF)

                row = mdt.NewRow
                row("Order") = Trim(Str(rsItemHistory.Fields("OrderHeader_ID").Value))
                row("Date") = rsItemHistory.Fields("OrderDate").Value
                row("Vendor") = rsItemHistory.Fields("CompanyName").Value
                row("Recv Loc") = rsItemHistory.Fields("ReceiveLocation").Value
                row("Qty Recv") = VB6.Format(rsItemHistory.Fields("QuantityReceived").Value, "####0.0#") & " " & rsItemHistory.Fields("Unit_Name").Value
                row("Date Recv") = rsItemHistory.Fields("DateReceived").Value
                row("Cost") = rsItemHistory.Fields("Cost").Value
                row("Discount") = IIf(rsItemHistory.Fields("DiscountType").Value = 0, "None", VB6.Format(rsItemHistory.Fields("QuantityDiscount").Value, "####0.0#") & " " & sDiscountType(rsItemHistory.Fields("DiscountType").Value))
                row("Freight") = rsItemHistory.Fields("Freight").Value
                row("Adjusted") = rsItemHistory.Fields("AdjustedCost").Value
                row("Landed") = rsItemHistory.Fields("LandedCost").Value

                mdt.Rows.Add(row)

                rsItemHistory.MoveNext()
            End While

            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)
            mdv.Sort = "Order DESC"
            ugrdItemHistory.DataSource = mdv

            If rsItemHistory.RecordCount > 0 Then
                'Set the first item to selected.
                ugrdItemHistory.Rows(0).Selected = True
            End If

        Finally
            If rsItemHistory IsNot Nothing Then
                rsItemHistory.Close()
                rsItemHistory = Nothing
            End If
        End Try

ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub

    Public WriteOnly Property Identifier() As String
        Set(ByVal Value As String)
            m_sIdentifier = Value
        End Set
    End Property
	
    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        '-- Close the form
        Me.Hide()

    End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        Dim fItemOrderHist As New ItemOrderHistory
        fItemOrderHist.Identifier = m_sIdentifier
        fItemOrderHist.LockIdentifier = True
        fItemOrderHist.ShowDialog()
        fItemOrderHist.Close()
        fItemOrderHist.Dispose()
	End Sub

End Class