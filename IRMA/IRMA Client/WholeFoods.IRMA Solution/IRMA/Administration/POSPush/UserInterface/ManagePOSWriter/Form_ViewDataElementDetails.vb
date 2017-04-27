Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_ViewDataElementDetails
#Region "Class Level Vars and Property Definitions"
    ''' <summary>
    ''' DataSet of data elements.
    ''' </summary>
    ''' <remarks></remarks>
    Dim _dataSet As DataSet
    ''' <summary>
    ''' Value of the POS Data Type being edited.
    ''' </summary>
    ''' <remarks></remarks>
    Private _posDataTypeKey As Integer
    ''' <summary>
    ''' Flag to return only elements which evaluate to True/False.
    ''' </summary>
    ''' <remarks></remarks>
    Private _booleanElementsOnly As Boolean
#End Region

#Region "Events handled by this form"
#Region "form load"
    ''' <summary>
    ''' Pre-populate the form with the existing data type values.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_ManagePOSWriterDictionary_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'load data to form control
        BindData()
        'format data control
        FormatDataGrid()
    End Sub

    ''' <summary>
    ''' bind data to form control
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindData()
        ' Read the data dictionary values
        _dataSet = POSDataElementDAO.GetPOSDataValues(_posDataTypeKey, _booleanElementsOnly)
        DataGridView_DataElements.DataSource = _dataSet.Tables(0)
    End Sub

    ''' <summary>
    ''' set formatting options - hide/display columns, set multi-select
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FormatDataGrid()
        DataGridView_DataElements.MultiSelect = False

        ' Format the view
        If (DataGridView_DataElements.Columns.Count > 0) Then
            DataGridView_DataElements.Columns("POSDataTypeKey").Visible = False
            DataGridView_DataElements.Columns("IsBoolean").Visible = False

            DataGridView_DataElements.Columns("DataElement").DisplayIndex = 0
            DataGridView_DataElements.Columns("DataElement").HeaderText = ResourcesAdministration.GetString("label_dataElement")
            DataGridView_DataElements.Columns("DataElement").ReadOnly = True
            DataGridView_DataElements.Columns("DataElement").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            DataGridView_DataElements.Columns("Description").DisplayIndex = 1
            DataGridView_DataElements.Columns("Description").HeaderText = ResourcesAdministration.GetString("label_description")
            DataGridView_DataElements.Columns("Description").ReadOnly = True

            DataGridView_DataElements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            DataGridView_DataElements.ScrollBars = ScrollBars.Both
        End If
    End Sub
#End Region

#Region "form buttons"
    ''' <summary>
    ''' Exit the form.  There are no changes to save.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Ok.Click
        Me.Close()
    End Sub
#End Region

#End Region

#Region "Property Definitions"
    Property POSDataTypeKey() As Integer
        Get
            Return _posDataTypeKey
        End Get
        Set(ByVal value As Integer)
            _posDataTypeKey = value
        End Set
    End Property

    Property BooleanElementsOnly() As Boolean
        Get
            Return _booleanElementsOnly
        End Get
        Set(ByVal value As Boolean)
            _booleanElementsOnly = value
        End Set
    End Property
#End Region

End Class