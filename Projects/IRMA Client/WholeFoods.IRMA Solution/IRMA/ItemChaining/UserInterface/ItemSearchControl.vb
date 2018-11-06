Imports System.Text
Imports WholeFoods.IRMA.ItemChaining.DataAccess
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemChaining.UserInterface
    Public Class ItemSearchControl

#Region "Member Variables"

        Private _dataSet As ItemSearchResults = New DataAccess.ItemSearchResults()
        Private _dataSet2 As DataSet = New DataSet("SP")
        Private _brandID As String = ""
        Private _chainID As String = ""
        Private _vendorID As String = ""
        Private _subTeamID As String = ""
        Private _DistSubTeamID As String = ""
        Private _classID As String = ""
        Private _Level3ID As String = ""
        Private _Level4ID As String = ""
        Private lastTextBox As Object
        Private firstLetterLastString As String = ""

#End Region

#Region "Events"

        Public Event Searching()
        Public Event Searched()

#End Region

#Region "Properties"

        ''' <summary>
        ''' Get the search result dataset. See also Search funsion.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property DataSet() As ItemSearchResults
            Get
                Return _dataSet
            End Get
        End Property

        Property ShowWFM() As Boolean
            Get
                Return isoItemOptions.ShowWFM
            End Get
            Set(ByVal Value As Boolean)
                isoItemOptions.ShowWFM = Value
            End Set
        End Property

        Property ShowHFM() As Boolean
            Get
                Return isoItemOptions.ShowHFM
            End Get
            Set(ByVal Value As Boolean)
                isoItemOptions.ShowHFM = Value
            End Set
        End Property

        Property ShowSearchButton() As Boolean
            Get
                ShowSearchButton = btnSearch.Visible
            End Get
            Set(ByVal Value As Boolean)
                btnSearch.Visible = Value
            End Set
        End Property

        Property ShowClearButton() As Boolean
            Get
                Return btnClear.Visible
            End Get
            Set(ByVal value As Boolean)
                btnClear.Visible = value
            End Set
        End Property

        Property ShowItemCheckBoxes() As Boolean
            Get
                Return isoItemOptions.Visible
            End Get
            Set(ByVal value As Boolean)
                isoItemOptions.Visible = value
            End Set
        End Property

#End Region

#Region "Helper Methods"

        Private Function StringToIntOrZero(ByVal str As String) As Integer
            If (str.Trim().Length = 0) Then
                Return 0
            Else
                Return Convert.ToInt32(str)
            End If
        End Function

        Private Function StringToNullableInt(ByVal str As String) As Nullable(Of Integer)
            If (str.Trim().Length = 0) Then
                Return Nothing
            Else
                Return Convert.ToInt32(str)
            End If
        End Function

        Private Function StringOrNothing(ByVal value As String) As String
            Return CStr(IIf(String.IsNullOrEmpty(value), Nothing, value))
        End Function

        Private Sub LoadItems(ByVal StoredProcedure As String, ByVal text As String)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' Open the connection
            If text = "" Or text = "*" Then
                Return
            End If

            _dataSet2.Clear()

            factory.GetDataAdapter(StoredProcedure + " """ + text + """", Nothing).Fill(_dataSet2, "SP")
        End Sub

        ''' <summary>
        ''' Changes to the text fiels cause internal keeping of the relevant Id.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="ID"></param>
        ''' <remarks></remarks>
        Private Sub SetID(ByVal sender As TextBox, ByVal ID As String)
            If sender Is txtBrand Then _brandID = ID : Return
            If sender Is txtChain Then _chainID = ID : Return
            If sender Is txtVendor Then _vendorID = ID : Return
            If sender Is txtSubTeam Then _subTeamID = ID : Return
            If sender Is txtClass Then _classID = ID : Return
            If sender Is txtLevel3 Then _Level3ID = ID : Return
            If sender Is txtLevel4 Then _Level4ID = ID : Return
        End Sub

        Private Sub SetTextBoxesState()

            ' If the "parent" textbox is not empty and the ID exists
            If txtSubTeam.Text.Length > 0 AndAlso Not String.IsNullOrEmpty(_subTeamID) Then
                txtClass.Enabled = True
                If txtClass.Text = My.Resources.ItemChaining.SelectSubTeam Then
                    txtClass.Text = String.Empty
                End If
            Else
                txtClass.Enabled = False
                txtClass.Text = My.Resources.ItemChaining.SelectSubTeam
            End If

            If txtClass.Text.Length > 0 AndAlso txtClass.Enabled AndAlso Not String.IsNullOrEmpty(_classID) Then
                txtLevel3.Enabled = True
                If txtLevel3.Text = My.Resources.ItemChaining.SelectClass Then
                    txtLevel3.Text = String.Empty
                End If
            Else
                txtLevel3.Enabled = False
                txtLevel3.Text = My.Resources.ItemChaining.SelectClass
            End If

            If txtLevel3.Text.Length > 0 AndAlso txtLevel3.Enabled AndAlso Not String.IsNullOrEmpty(_Level3ID) Then
                txtLevel4.Enabled = True
                If txtLevel4.Text = My.Resources.ItemChaining.SelectLevel3 Then
                    txtLevel4.Text = String.Empty
                End If
            Else
                txtLevel4.Enabled = False
                txtLevel4.Text = My.Resources.ItemChaining.SelectLevel3
            End If
        End Sub

        Private Function BuildAutoCompleteStoredProcedureCall(ByVal sender As System.Object) As String
            ' Tag is set as the stored procedure to use for auto-complete
            Dim builder As StringBuilder = New StringBuilder(CType(sender, TextBox).Tag.ToString())

            If sender Is txtClass And txtClass.Enabled Then
                If Not String.IsNullOrEmpty(_subTeamID) Then
                    builder.AppendFormat(" {0},", _subTeamID)
                End If
            End If

            If sender Is txtLevel3 And txtLevel3.Enabled Then
                If Not String.IsNullOrEmpty(_classID) Then
                    builder.AppendFormat(" {0},", _classID)
                End If
            End If

            If sender Is txtLevel4 And txtLevel4.Enabled Then
                If Not String.IsNullOrEmpty(_Level3ID) Then
                    builder.AppendFormat(" {0},", _Level3ID)
                End If
            End If

            Return builder.ToString()
        End Function

#End Region

#Region "Public Methods"

        Public Sub Clear()
            isoItemOptions.Clear()

            txtBrand.Text = String.Empty
            txtChain.Text = String.Empty
            txtLevel4.Text = String.Empty
            txtLevel3.Text = String.Empty
            txtSubTeam.Text = String.Empty
            txtClass.Text = String.Empty
            txtDescription.Text = String.Empty
            txtDistSubTeam.Text = String.Empty
            txtIdentifier.Text = String.Empty
            txtVendor.Text = String.Empty
            txtVendorItemID.Text = String.Empty
            txtVendorPS.Text = String.Empty
            txtTextBox_TextChanged(txtBrand, EventArgs.Empty)
            _brandID = String.Empty
            _chainID = String.Empty
            _vendorID = String.Empty
            _subTeamID = String.Empty
            _classID = String.Empty
            _Level3ID = String.Empty
            _Level4ID = String.Empty
            txtBrand.Focus()
        End Sub

        Public Sub AddSearchCriteria(ByVal itemSearchHelper As Item.ItemSearchHelper)
            With itemSearchHelper
                .SubTeam_No = StringToIntOrZero(_subTeamID)
                .Category_ID = StringToIntOrZero(_classID)
                .Level3_ID = StringToIntOrZero(_Level3ID)
                .Level4_ID = StringToIntOrZero(_Level4ID)
                .Chain_ID = StringToIntOrZero(_chainID)
                .DistSubTeam_No = _DistSubTeamID
                .Brand_ID = _brandID

                '-- Make the where string for Identifier
                .Item_Description = ConvertQuotes(Trim(txtDescription.Text))
                .Identifier = Trim(txtIdentifier.Text)
                .Vendor = ConvertQuotes(Trim(txtVendor.Text))
                .Vendor_ID = Trim(txtVendorItemID.Text)
                .Vendor_PS = Trim(txtVendorPS.Text)
            End With

            isoItemOptions.AddSearchCriteria(itemSearchHelper)
        End Sub

        Public Sub AddNonDefaultSearchCriteria(ByVal itemSearchHelper As Item.ItemSearchHelper)
            With itemSearchHelper
                .SubTeam_No = StringToNullableInt(_subTeamID)
                .Category_ID = StringToNullableInt(_classID)
                .Level3_ID = StringToNullableInt(_Level3ID)
                .Level4_ID = StringToNullableInt(_Level4ID)
                .Chain_ID = StringToNullableInt(_chainID)
                .DistSubTeam_No = StringOrNothing(_DistSubTeamID)
                .Brand_ID = StringOrNothing(_brandID)

                '-- Make the where string for Identifier
                .Item_Description = StringOrNothing(ConvertQuotes(Trim(txtDescription.Text)))
                .Identifier = StringOrNothing(Trim(txtIdentifier.Text))
                .Vendor = StringOrNothing(ConvertQuotes(Trim(txtVendor.Text)))
                .Vendor_ID = StringOrNothing(Trim(txtVendorItemID.Text))
                .Vendor_PS = StringOrNothing(Trim(txtVendorPS.Text))
            End With

            If isoItemOptions.Visible Then
                isoItemOptions.AddNonDefaultSearchCriteria(itemSearchHelper)
            End If
        End Sub

        Public Function Search() As ItemSearchResults
            Dim item As New Item

            RaiseEvent Searching()

            AddSearchCriteria(item.SearchHelper)

            _dataSet = item.Search()

            RaiseEvent Searched()
            Return _dataSet
        End Function

#End Region

#Region "Form Event Handlers"

        Private Sub ItemSearchControl_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Set transparency
            Me.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, False)
            Me.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, True)
            Me.BackColor = System.Drawing.Color.Transparent
        End Sub

#End Region

#Region "TextBox Event Handlers"

        ''' <summary>
        ''' User is typing in the text boes that has change event. The relevant stored procedure is taken from the tag property.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub txtTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtVendor.TextChanged, txtBrand.TextChanged, txtChain.TextChanged, txtSubTeam.TextChanged, txtLevel4.TextChanged, txtLevel3.TextChanged, txtClass.TextChanged
            Dim id As String = String.Empty
            Dim currentTextbox As TextBox = CType(sender, TextBox)
            Dim firstLetterCurrentString As String

            SetTextBoxesState()

            If currentTextbox.Enabled = False Or currentTextbox.Text.Length = 0 Then
                SetID(currentTextbox, id)
                Return
            End If

            firstLetterCurrentString = currentTextbox.Text.Substring(0, 1)

            If sender.Equals(lastTextBox) = False Or firstLetterCurrentString <> firstLetterLastString Then
                Dim senderAsComponent As System.ComponentModel.IComponent = CType(sender, System.ComponentModel.IComponent)

                LoadItems(BuildAutoCompleteStoredProcedureCall(sender), firstLetterCurrentString)

                FormManager1.SetStringSource(senderAsComponent, _dataSet2, "SP.Value")
                FormManager1.UpdateStringSource(senderAsComponent)
            End If

            firstLetterLastString = firstLetterCurrentString
            lastTextBox = sender

            For Each row As System.Data.DataRow In _dataSet2.Tables(0).Rows
                If row("Value").ToString.ToLower = currentTextbox.Text.ToLower Then
                    id = row("ID").ToString
                    Exit For
                End If
            Next

            SetID(currentTextbox, id)

            SetTextBoxesState()

            Trace.Write(String.Format("text: {0} id: {1}", currentTextbox.Text, id))
        End Sub

#End Region

#Region "Button Event Handlers"

        Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            Search()
        End Sub

        Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
            Clear()
        End Sub

#End Region

        Private Sub txtSubTeam_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSubTeam.KeyPress
            e.GetType()
        End Sub
    End Class
End Namespace