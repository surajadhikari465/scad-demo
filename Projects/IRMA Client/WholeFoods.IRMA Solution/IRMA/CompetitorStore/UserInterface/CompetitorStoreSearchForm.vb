Imports WholeFoods.IRMA.CompetitorStore.BusinessLogic
Imports WholeFoods.IRMA.CompetitorStore.DataAccess

Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    Public Class CompetitorStoreSearchForm

#Region "Member Variables"

        Private _competitorStoreDataObject As DataAccess.CompetitorStore

#End Region

#Region "Properties"

        Public ReadOnly Property SelectedCompetitorStore() As CompetitorStoreDataSet.CompetitorStoreRow
            Get
                If lvResults.SelectedItems.Count = 0 Then
                    Return Nothing
                Else
                    Return ResultsDataSet.CompetitorStore.FindByCompetitorStoreID(Convert.ToInt32(lvResults.SelectedItems(0).SubItems(3).Text))
                End If
            End Get
        End Property

        Private ReadOnly Property CompetitorStoreDataObject() As DataAccess.CompetitorStore
            Get
                If _competitorStoreDataObject Is Nothing Then
                    _competitorStoreDataObject = New DataAccess.CompetitorStore
                End If

                Return _competitorStoreDataObject
            End Get
        End Property

#End Region

#Region "Helper Methods"

        Private Sub LoadCompetitorsAndLocations()
            ResultsDataSet.Competitor.AddCompetitorRow(My.Resources.CompetitorStore.AllItems)
            ResultsDataSet.CompetitorLocation.AddCompetitorLocationRow(My.Resources.CompetitorStore.AllItems)

            Competitor.List(Me.ResultsDataSet)
            CompetitorLocation.List(Me.ResultsDataSet)
        End Sub

        Private Sub DisplayResults()
            Dim item As ListViewItem

            For Each store As CompetitorStoreDataSet.CompetitorStoreRow In Me.ResultsDataSet.CompetitorStore
                item = New ListViewItem(store.CompetitorRow.Name)
                item.SubItems.Add(store.CompetitorLocationRow.Name)
                item.SubItems.Add(store.Name)
                item.SubItems.Add(store.CompetitorStoreID.ToString())

                lvResults.Items.Add(item)
            Next
        End Sub

#End Region

#Region "Constructor and Load Event Handler"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            LoadCompetitorsAndLocations()
        End Sub

        Private Sub CompetitorStoreSearchForm_Load(ByVal sender As Object, ByVal args As EventArgs) Handles Me.Load
            lvResults.Items.Clear()

            cbLocation.SelectedIndex = 0
            cbCompetitor.SelectedIndex = 0
        End Sub

#End Region

#Region "Button Click Event Handlers"

        Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            Dim competitorID As New Nullable(Of Integer)
            Dim competitorLocationID As New Nullable(Of Integer)

            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            ' Clear the list view
            lvResults.BeginUpdate()
            lvResults.Items.Clear()

            ' Clear the table of competitor stores
            Me.ResultsDataSet.CompetitorStore.Clear()

            ' Get the IDs of the criteria selected 
            If cbCompetitor.SelectedValue IsNot Nothing AndAlso cbCompetitor.SelectedIndex > 0 Then
                competitorID = CInt(cbCompetitor.SelectedValue)
            End If

            If cbLocation.SelectedValue IsNot Nothing AndAlso cbLocation.SelectedIndex > 0 Then
                competitorLocationID = CInt(cbLocation.SelectedValue)
            End If

            ' Perform the search
            CompetitorStoreDataObject.Search(Me.ResultsDataSet, competitorID, competitorLocationID)

            ' Display the results in the list view
            DisplayResults()

            lvResults.EndUpdate()

            Windows.Forms.Cursor.Current = Cursors.Default
        End Sub

        Private Sub btnSelectStore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectStore.Click
            If lvResults.SelectedItems.Count = 1 Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                MessageBox.Show(My.Resources.CompetitorStore.SelectCompetitorStore, My.Resources.CompetitorStore.SelectCompetitorStoreTitle)
            End If
        End Sub

        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End Sub

#End Region

    End Class
End Namespace