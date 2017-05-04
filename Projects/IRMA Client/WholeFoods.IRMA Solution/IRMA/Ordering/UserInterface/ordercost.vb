Option Strict Off
Option Explicit On
Imports log4net

Friend Class frmOrderCost
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private mdt As DataTable

    Private Sub RefreshGrid()

        logger.Debug("RefreshGrid Entry")

        Dim rsItemOrder As DAO.Recordset = Nothing
        Dim row As DataRow

        mdt.Clear()
        Try
            rsItemOrder = SQLOpenRecordSet("EXEC GetItemOrder " & glOrderItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If gbAccountant Or gbCoordinator Or gbBuyer Then
                While Not rsItemOrder.EOF

                    row = mdt.NewRow
                    row("Store_Name") = rsItemOrder.Fields("Store_Name").Value
                    row("Retail") = IIf(rsItemOrder.Fields("Multiple").Value > 1, rsItemOrder.Fields("Multiple").Value & "@" & CDec(rsItemOrder.Fields("Price").Value).ToString("####0.00##"), CDec(rsItemOrder.Fields("Price").Value).ToString("####0.00##"))
                    row("OrdCost") = rsItemOrder.Fields("UnitExtCost").Value
                    row("GIG") = (((rsItemOrder.Fields("Price").Value / rsItemOrder.Fields("Multiple").Value) - rsItemOrder.Fields("UnitExtCost").Value) / (rsItemOrder.Fields("Price")).Value / rsItemOrder.Fields("Multiple").Value) * 100
                    row("DistCost") = (rsItemOrder.Fields("UnitExtCost").Value * (100 + rsItemOrder.Fields("DistributionMarkup").Value)) / 100
                    row("DistGIG") = (((rsItemOrder.Fields("Price").Value / rsItemOrder.Fields("Multiple").Value) - ((rsItemOrder.Fields("UnitExtCost").Value * (100 + rsItemOrder.Fields("DistributionMarkup").Value)) / 100)) / (rsItemOrder.Fields("Price")).Value / rsItemOrder.Fields("Multiple").Value) * 100
                    mdt.Rows.Add(row)

                    rsItemOrder.MoveNext()
                End While

                rsItemOrder.Close()
                rsItemOrder = Nothing

                mdt.AcceptChanges()
                Me.ugrdItemList.DataSource = mdt

                If ugrdItemList.Rows.Count > 0 Then
                    ugrdItemList.DisplayLayout.Bands(0).Columns("Store_Name").PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand)
                Else
                    MessageBox.Show("GIG calculation cannot be performed due to a Price of 0.00 or the receiving store is not a retail location.")
                End If

            Else
                MessageBox.Show("You lack sufficient permissions to review this data.")
            End If
        Finally
            If rsItemOrder IsNot Nothing Then
                rsItemOrder.Close()
                rsItemOrder = Nothing
            End If
        End Try

        logger.Debug("RefreshGrid Exit")

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        '-- Close the form
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub frmOrderCost_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmOrderCost_Load Entry")
        '-- Center the form
        CenterForm(Me)
        SetupDataTable()
        RefreshGrid()
        logger.Debug("frmOrderCost_Load Exit")

    End Sub

    Private Sub SetupDataTable()

        logger.Debug("SetupDataTable Entry")
        mdt = New DataTable("OrderCost")

        mdt.Columns.Add(New DataColumn("Store_Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("Retail", GetType(String)))
        mdt.Columns.Add(New DataColumn("OrdCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("GIG", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("DistCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("DistGIG", GetType(Decimal)))

        logger.Debug("SetupDataTable Exit")
    End Sub
End Class