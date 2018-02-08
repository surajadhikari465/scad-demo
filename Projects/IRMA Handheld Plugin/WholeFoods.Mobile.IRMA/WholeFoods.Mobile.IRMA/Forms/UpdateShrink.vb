Imports System.Windows.Forms
Imports System
Imports System.Linq
Imports System.Data
Imports WholeFoods.Mobile.IRMA.Common

Public Class UpdateShrink
    Inherits HandheldHardware.ScanForm

    Public subteam As String
    Public store As String
    Public formName As String

    Private mySession As Session
    Private _lastQtyRecorded As String = String.Empty
    Private _upc As String = String.Empty
    Private _uom As String = String.Empty
    Private _desc As String = String.Empty
    Private _cbw As Boolean = False
    Private _qty As String = String.Empty
    Private _vendorCost As String = String.Empty
    Private _shrinkSubType As String = String.Empty
    Private _shrinkSubTypeId As String = String.Empty

    Public ReadOnly Property ShrinkQuantity() As String
        Get
            Return _qty
        End Get

    End Property

    Public ReadOnly Property GetShrinkSubTypeId() As String
        Get
            Return _shrinkSubTypeId
        End Get
    End Property

    Public WriteOnly Property ShrinkSubTypeId() As String
        Set(ByVal value As String)
            _shrinkSubTypeId = value
        End Set
    End Property

    Public ReadOnly Property GetShrinkSubType() As String
        Get
            Return _shrinkSubType
        End Get
    End Property

    Public WriteOnly Property ShrinkSubType() As String
        Set(ByVal value As String)
            _shrinkSubType = value
        End Set
    End Property

    Public WriteOnly Property CostedByWeight() As Boolean
        Set(ByVal value As Boolean)
            _cbw = value
        End Set
    End Property

    Public WriteOnly Property UPC() As String
        Set(ByVal value As String)
            _upc = value
        End Set
    End Property

    Public WriteOnly Property UOM() As String
        Set(ByVal value As String)
            _uom = value
        End Set
    End Property

    Public WriteOnly Property LastQtyRecorded() As String
        Set(ByVal value As String)
            _lastQtyRecorded = value
        End Set
    End Property

    Public WriteOnly Property Desc() As String
        Set(ByVal value As String)
            _desc = value
        End Set
    End Property

    Public Property VendorCost() As String
        Get
            Return _vendorCost
        End Get
        Set(ByVal value As String)
            _vendorCost = value
        End Set
    End Property

#Region " Constructors"

    Public Sub New(ByVal frmName As String, Optional ByVal session As Session = Nothing)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.KeyPreview = True
        formName = frmName
        AlignText()
        Me.mySession = session

    End Sub

#End Region

    Private Sub txtQty_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.GotFocus
        Me.txtQty.SelectAll()
    End Sub

    Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
        _qty = Trim(txtQty.Text)

        If _qty > 999 Then
            MsgBox(Messages.QUANTITY_AMT_ERROR, MsgBoxStyle.OkOnly, Me.Text)
            Exit Sub
        End If

        If (Me.mySession IsNot Nothing) Then
            ShrinkSubTypeId = cmbSubType.SelectedValue
            ShrinkSubType = cmbSubType.Text
        End If

        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub txtQty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtQty.KeyPress

        If _cbw Then
            'allow numbers and 1 decimal to be entered
            If Char.IsDigit(e.KeyChar) _
                Or e.KeyChar = Chr(46) _
                Or e.KeyChar = Chr(8) Then

                'check for existing decimal point
                If e.KeyChar = Chr(46) And txtQty.Text.Contains(Chr(46)) Then
                    e.Handled = True
                End If

                'little erik doesn't want 6 chars for non decimals
                If e.KeyChar = Chr(46) Or txtQty.Text.Contains(Chr(46)) Then
                    txtQty.MaxLength = 6
                Else
                    txtQty.MaxLength = 5
                End If

                'check the positioning of the char being input
                If txtQty.Text.Contains(Chr(46)) And e.KeyChar <> Chr(8) Then
                    Dim iSelPos As Integer = txtQty.SelectionStart
                    Dim iDecPos As Integer = txtQty.Text.IndexOf(Chr(46)) + 1
                    If Len(Mid(txtQty.Text, iDecPos, Len(txtQty.Text))) > 2 And iSelPos = Len(txtQty.Text) Then
                        e.Handled = True
                    End If
                End If
            Else
                e.Handled = True
            End If
        Else
            'allow only numbers
            If Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = Chr(8)) Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        _qty = String.Empty
        Me.DialogResult = Windows.Forms.DialogResult.Abort
    End Sub

    Private Sub UpdateShrink_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lblName.Text = formName
        lblUPCVal.Text = _upc
        lblDescription.Text = _desc

        If _cbw Then
            lblOldQty.Text = "Weight Recorded: "
            lblQty.Text = "New Weight: "
        End If

        lblOldQtyValue.Text = _lastQtyRecorded
        txtQty.Text = _lastQtyRecorded

        lblUOMValue.Text = _uom

        If (Me.mySession Is Nothing) Then
            cmbSubType.Visible = False
            lblSubType.Visible = False
        Else
            Dim dtsTypes As DataTable
            Dim dr As DataRow

            dtsTypes = New DataTable
            dtsTypes.Columns.Add(New DataColumn("DisplayMember"))
            dtsTypes.Columns.Add(New DataColumn("ValueMember"))

            For Each shrinkType In mySession.ShrinkSubTypes
                dr = dtsTypes.NewRow()
                dr.Item("DisplayMember") = shrinkType.ShrinkSubType1
                dr.Item("ValueMember") = shrinkType.ShrinkSubTypeID
                dtsTypes.Rows.Add(dr)
            Next

            Me.cmbSubType.DataSource = dtsTypes
            Me.cmbSubType.DisplayMember = "DisplayMember"
            Me.cmbSubType.ValueMember = "ValueMember"

            Me.cmbSubType.SelectedValue = Me.GetShrinkSubTypeId
        End If

    End Sub

    Private Sub AlignText()
        lblUPC.TextAlign = ContentAlignment.TopRight
        lblDesc.TextAlign = ContentAlignment.TopRight
        lblOldQty.TextAlign = ContentAlignment.TopRight
        lblQty.TextAlign = ContentAlignment.TopRight
        lblUOM.TextAlign = ContentAlignment.TopRight
    End Sub

End Class