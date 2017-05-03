Imports System.Text
Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic
Imports WholeFoods.IRMA.CompetitorStore.DataAccess
Imports WholeFoods.IRMA.ItemChaining.DataAccess

Namespace WholeFoods.IRMA.Reporting.UserInterface
    Public Class frmCompetitorMarginImpact

#Region "Member Variables"

        Private _npvDataSet As New NPVDataSet
        Private _csDataSet As New CompetitorStoreDataSet

#End Region

#Region "Helper Methods"

        Private Function GetNullableIDFromComboBox(ByVal comboBox As ComboBox) As Nullable(Of Integer)
            Return CType(IIf(comboBox.SelectedIndex > 0, CInt(comboBox.SelectedValue), Nothing), Nullable(Of Integer))
        End Function

        Private Sub LoadWFMStores()
            Dim table As DataTable = ItemHosting.DataAccess.StoreDAO.CreateStoreDataTable()
            Dim row As DataRow = table.NewRow()

            row("Store_No") = -1
            row("Store_Name") = My.Resources.CompetitorStore.NoItems

            table.Rows.Add(row)

            ItemHosting.DataAccess.StoreDAO.GetStoreList(table)

            With cmbWFMStore
                .DisplayMember = "Store_Name"
                .ValueMember = "Store_No"
                .DataSource = table
            End With
        End Sub

        Private Sub LoadFiscalWeeks()
            Dim currentWeek As CompetitorStoreDataSet.FiscalWeekRow

            FiscalWeek.List(_csDataSet)

            cmbFiscalWeek.DisplayMember = _csDataSet.FiscalWeek.FiscalWeekDescriptionColumn.ColumnName
            cmbFiscalWeek.DataSource = _csDataSet.FiscalWeek

            currentWeek = FiscalWeek.FindCurrent(_csDataSet)

            If currentWeek IsNot Nothing Then
                cmbFiscalWeek.SelectedIndex = cmbFiscalWeek.FindStringExact(currentWeek.FiscalWeekDescription)
            End If
        End Sub

        Private Sub LoadCompetitivePriceTypes()
            _npvDataSet.CompetitivePriceType.AddCompetitivePriceTypeRow(-1, My.Resources.CompetitorStore.AllItems)

            CompetitivePriceType.List(_npvDataSet.CompetitivePriceType)

            With cmbCompetitivePriceType
                .DisplayMember = _npvDataSet.CompetitivePriceType.DescriptionColumn.ColumnName
                .ValueMember = _npvDataSet.CompetitivePriceType.CompetitivePriceTypeIDColumn.ColumnName
                .DataSource = _npvDataSet.CompetitivePriceType
            End With
        End Sub

#End Region

#Region "Constructors"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            LoadWFMStores()
            LoadCompetitivePriceTypes()
            LoadFiscalWeeks()

            _npvDataSet.AcceptChanges()

            lsCompetitorStore.DataSet = _csDataSet
        End Sub

#End Region

#Region "Form Event Handlers"

        Private Sub frmCompetitorMarginImpact_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            ' Attach event handlers that cause issues before everything is set up
            AddHandler cmbWFMStore.SelectedIndexChanged, New EventHandler(AddressOf cmbWFMStore_SelectedIndexChanged)
            AddHandler lsCompetitorStore.ButtonStateChanged, New ItemChaining.UserInterface.ListSelect.ButtonStateChangedEventHandler(AddressOf StoreSelectStateChanged)
            AddHandler cmbWFMStore.SelectedIndexChanged, New EventHandler(AddressOf StoreSelectStateChanged)
        End Sub

#End Region

#Region "List Select Event Handlers"

        Private Sub StoreSelectStateChanged(ByVal sender As Object, ByVal e As EventArgs)
            btnViewReport.Enabled = cmbWFMStore.SelectedIndex > 0 OrElse lsCompetitorStore.SelectedListCount > 0
        End Sub

        Private Sub cmbWFMStore_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
            lsCompetitorStore.WFMStore_No = CType(IIf(cmbWFMStore.SelectedIndex = 0, Nothing, CInt(cmbWFMStore.SelectedValue)), Nullable(Of Integer))
        End Sub

#End Region

#Region "Button Event Handlers"

        Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
            Me.Close()
        End Sub

        Private Sub btnViewReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewReport.Click
            Dim reportURLBuilder As New System.Text.StringBuilder
            Dim selectedFiscalWeek As CompetitorStoreDataSet.FiscalWeekRow = CType(CType(cmbFiscalWeek.SelectedValue, DataRowView).Row, CompetitorStoreDataSet.FiscalWeekRow)
            Dim itemSearchHelper As New Item.ItemSearchHelper

            If lsCompetitorStore.SelectedListCount > 0 OrElse cmbWFMStore.SelectedIndex > 0 Then
                lsCompetitorStore.SaveSelections()

                With reportURLBuilder
                    .Append("CompetitorMarginImpact&rs:Command=Render&rc:Parameters=False")

                    ' Item search control parameters
                    iscItemSearch.AddNonDefaultSearchCriteria(itemSearchHelper)
                    itemSearchHelper.AddOnlyNonDefaultParameters(reportURLBuilder)

                    If cmbWFMStore.SelectedIndex > 0 Then
                        .AppendFormat("&Store_No={0}", CInt(cmbWFMStore.SelectedValue))
                    End If

                    If cmbCompetitivePriceType.SelectedIndex > 0 Then
                        .AppendFormat("&CompetitivePriceTypeID={0}", CInt(cmbCompetitivePriceType.SelectedValue))
                    End If

                    If lsCompetitorStore.SelectedListCount > 0 Then
                        .Append("&CompetitorStoreIDs=")

                        For Each store As CompetitorStoreDataSet.CompetitorStoreRow In lsCompetitorStore.SelectedCompetitorStores
                            .AppendFormat("{0},", store.CompetitorStoreID)
                        Next

                        ' Remove the trailing comma
                        .Remove(.Length - 1, 1)
                    End If

                    .AppendFormat("&FiscalYear={0}&FiscalPeriod={1}&PeriodWeek={2}", _
                        selectedFiscalWeek.FiscalYear, _
                        selectedFiscalWeek.FiscalPeriod, _
                        selectedFiscalWeek.PeriodWeek)

                End With

                ReportingServicesReport(reportURLBuilder.ToString())
            Else
                MessageBox.Show(My.Resources.CompetitorStore.SelectStore, My.Resources.CompetitorStore.SelectStoreTitle)
            End If
        End Sub

#End Region

    End Class
End Namespace