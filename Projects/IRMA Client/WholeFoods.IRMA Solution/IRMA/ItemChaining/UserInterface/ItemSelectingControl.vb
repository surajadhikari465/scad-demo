Namespace WholeFoods.IRMA.ItemChaining.UserInterface
    Public Class ItemSelectingControl

#Region "Member Variables"
        Private _selectFromDataSet As DataSet = New DataSet()
        Private _icon As IconType
        Private _idField As String = ""
        Private _textField As String = ""
        Private _textField2 As String = ""
        Private _ImageField As String = ""
        Private _ItemText As String = ""
#End Region

#Region "Events"
        Public Event ExportClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
#End Region

#Region "Enumerations"
        Enum IconType
            [Boolean]
            [ByValue]
        End Enum
#End Region

#Region "Properties"
        ''' <summary>
        ''' Get or set the list view items in the selected items list
        ''' </summary>
        Property SelectedItems() As ListView.ListViewItemCollection
            Get
                SelectedItems = lstSelected.Items
            End Get
            Set(ByVal Value As ListView.ListViewItemCollection)
                lstSelected.Items.Clear()
                lstSelected.Items.AddRange(Value)
            End Set
        End Property

        ''' <summary>
        ''' Get or set the text of the list boxes title
        ''' </summary>
        Property ItemText() As String
            Get
                ItemText = _ItemText
            End Get
            Set(ByVal Value As String)
                _ItemText = Value
                lblFoundItems.Text = String.Format(My.Resources.ItemChaining.FoundItemsTitle, Value)
                lblSelectedItems.Text = String.Format(My.Resources.ItemChaining.SelectedItemsTitle, Value)
                lstFound.Columns(0).Text = Value
                lstSelected.Columns(0).Text = Value
            End Set
        End Property

        ''' <summary>
        ''' Get or set the height of the list boxes
        ''' </summary>
        Property ListHeight() As Integer
            Get
                ListHeight = lstSelected.Height
            End Get
            Set(ByVal Value As Integer)
                lstSelected.Height = Value
                lstFound.Height = Value
                btnClear.Top = lstSelected.Top + lstSelected.Height + 2
                btnExport.Top = btnClear.Top
            End Set
        End Property

        ''' <summary>
        ''' Get the found Itmes ListView control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property FoundItemsListView() As ListView
            Get
                FoundItemsListView = lstFound
            End Get
        End Property

        ''' <summary>
        ''' Get the selected Itmes ListView control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property SelectedItemsListView() As ListView
            Get
                SelectedItemsListView = lstSelected
            End Get
        End Property

        ''' <summary>
        ''' Get or Set the database field name that holds the ID of the record
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Field_ID() As String
            Get
                Field_ID = _idField
            End Get
            Set(ByVal Value As String)
                _idField = Value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the database field name that holds the Text of the record
        ''' </summary>
        Property Field_Text() As String
            Get
                Field_Text = _textField
            End Get
            Set(ByVal Value As String)
                _textField = Value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the database field name that holds the Text of the record
        ''' </summary>
        Property Field_Text2() As String
            Get
                Field_Text2 = _textField2
            End Get
            Set(ByVal Value As String)
                _textField2 = Value
                If Value = "" Then
                    lstFound.Columns(0).Width = 211
                    lstFound.Columns(1).Width = 0
                    lstSelected.Columns(0).Width = 211
                    lstSelected.Columns(1).Width = 0
                Else
                    lstFound.Columns(0).Width = 156
                    lstFound.Columns(1).Width = 60
                    lstSelected.Columns(0).Width = 156
                    lstSelected.Columns(1).Width = 60
                End If
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the database field name that holds the image index (icon left to the text) of the record. Take a look at the Icon property as well.
        ''' </summary>
        Property Field_Image() As String
            Get
                Field_Image = _ImageField
            End Get
            Set(ByVal Value As String)
                _ImageField = Value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the logic of showing the icon left to the text. In 
        ''' Boolean case, if image field column has value, image index 1 will be selected instead of image index 0.
        ''' In ByValue case, the image index will use the value from the field image column.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Icon() As IconType
            Get
                Icon = _icon
            End Get
            Set(ByVal Value As IconType)
                _icon = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets the Image List control to manipulate the icons.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property ImageList() As ImageList
            Get
                ImageList = ImageList1
            End Get
        End Property

        ''' <summary>
        ''' Get or Set the visibility of the "Search" button. You can perform search by calling the search method.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property ShowExportButton() As Boolean
            Get
                ShowExportButton = btnExport.Visible
            End Get
            Set(ByVal Value As Boolean)
                btnExport.Visible = Value
            End Set
        End Property

        ''' <summary>
        ''' Get or set the visibility of the "Clear" button.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property ShowClearButton() As Boolean
            Get
                Return btnClear.Visible
            End Get
            Set(ByVal value As Boolean)
                btnClear.Visible = value
            End Set
        End Property

        ''' <summary>
        ''' Get or Set the data set that will populate the found items list.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property SelectFromDataSet() As System.Data.DataSet
            Get
                Return _selectFromDataSet
            End Get
            Set(ByVal Value As System.Data.DataSet)
                _selectFromDataSet = Value

                LoadDataSet()
            End Set
        End Property
#End Region

#Region "Helper Methods"
        ''' <summary>
        ''' Performs a text find on the items in the list. 
        ''' </summary>
        ''' <param name="s"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function myFind(ByVal s As String) As Boolean
            For n As Integer = 0 To lstSelected.Items.Count - 1
                If lstSelected.Items(n).Text = s Then
                    Return True
                End If
            Next

            Return False
        End Function

        ''' <summary>
        ''' Loads the found item list with the dataset data
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadDataSet()
            lstFound.Items.Clear()

            For Each row As Data.DataRow In _selectFromDataSet.Tables(0).Rows
                Dim s As String = row(_textField).ToString.Trim
                If myFind(s) = False Then
                    Dim item As ListViewItem = New ListViewItem

                    If _textField2 <> "" Then
                        item.SubItems.Add(row(_textField2).ToString.Trim)
                    Else
                        item.SubItems.Add("")
                    End If

                    item.SubItems.Add(row(_idField).ToString.Trim)
                    item.Text = s

                    If _ImageField <> "" Then
                        If Icon = IconType.Boolean Then
                            If row(_ImageField) Is DBNull.Value = False Then
                                item.SubItems.Add(row(_ImageField).ToString)
                                item.StateImageIndex = 1
                            Else
                                item.SubItems.Add("")
                                item.StateImageIndex = 0
                            End If
                        Else
                            If row(_ImageField) Is DBNull.Value = False Then
                                item.SubItems.Add(row(_ImageField).ToString)
                                item.StateImageIndex = Convert.ToInt32(row(_ImageField).ToString)
                            Else
                                item.SubItems.Add("-1")
                                item.StateImageIndex = -1
                            End If
                        End If
                    End If

                    lstFound.Items.Add(item)
                End If
            Next

            SetButtons()
        End Sub

#End Region

#Region "Event Handlers"
        ''' <summary>
        ''' Control load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub ItemSelectingControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Sets transparency
            Me.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, False)
            Me.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, True)
            Me.BackColor = System.Drawing.Color.Transparent

            Me.Initialize(btnAdd, btnAddAll, btnRemove, btnRemoveAll, lstFound, lstSelected)

            SetButtons()
        End Sub

        Private Sub ItemSelectingControl_ButtonStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Me.ButtonStateChanged
            Dim areItemsSelected As Boolean = lstSelected.Items.Count > 0

            btnClear.Enabled = areItemsSelected
            btnExport.Enabled = areItemsSelected
        End Sub

#Region "Button clicks"

        ''' <summary>
        ''' User clicks the clear button
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
            BeforeMoveListItems()

            If MsgBox(String.Format(My.Resources.ItemChaining.ClearSelectedItems, vbNewLine), MsgBoxStyle.OkCancel, My.Resources.ItemChaining.ClearSelectedItemsTitle) = MsgBoxResult.Ok Then
                lstSelected.Items.Clear()
            End If

            AfterMoveListItems()
        End Sub

        ''' <summary>
        ''' Raise an Export Button pressed.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
            RaiseEvent ExportClick(Me, e)
            SetButtons()
        End Sub

#End Region

#End Region

    End Class
End Namespace