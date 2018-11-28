Option Strict Off
Option Explicit On

Imports log4net
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic

''' <summary>
''' Displays the contents of an item within a Shipper and allows editing of select attributes.
''' </summary>
''' <remarks>
''' Form classes that use Shipper functionality (get/set data) should use the Shipper class.
''' 
''' [Constructor Note]
''' The default constructor for this form class should not be used because this class relies on a ShipperItem object,
''' which is provided by a different constructor.
''' </remarks>
Friend Class frmShipperEdit
    Inherits System.Windows.Forms.Form

#Region "Private Members"

    ''' <summary>
    ''' Log4Net logger for this class.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private IsInitializing As Boolean

    ''' <summary>
    ''' The item this screen represents and source of this screen's information.
    ''' </summary>
    ''' <remarks></remarks>
    Private _shipperItem As ShipperItem

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Creates the Shipper-Item screen, passing in a ShipperItem object from which this screen gets its information.
    ''' *WARNING* -- The default constructor should not be used because a ShipperItem object is the source of 
    ''' this screen's information.
    ''' </summary>
    ''' <param name="shpItem"></param>
    ''' <remarks></remarks>
    Public Sub New(ByRef shpItem As ShipperItem)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        _shipperItem = shpItem
    End Sub

#End Region

#Region "Form Event Handlers"

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub frmShipperEdit_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            _shipperItem.Qty = txtQuantity.Text
        Catch ex As Exception
            ' Prompt user to re-enter.
            If MessageBox.Show(ex.Message & vbCrLf & vbCrLf & ShipperMessages.CONFIRM_SHIPPERITEM_REENTER_QTY, ShipperMessages.CAPTION_SHIPPERITEM_SAVE, MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then
                e.Cancel = True
                txtQuantity.Focus()
            End If
            ' Since the qty was bad, we exit because we do not want to save.  If user said "YES" to prompt, they can retry,
            ' otherwise, the original value is preserved.
            Exit Sub
        End Try

        Try
            ' This ShipperItem object keeps track of changed information.
            If _shipperItem.HasChanged Then
                logger.InfoFormat("Updating ShipperItem, identifier='{0}'", _shipperItem.Identifier)
                _shipperItem.Save()
            End If
        Catch ex As Exception
            MessageBox.Show(ShipperMessages.ERROR_SHIPPERITEM_DURING_UPDATE_INFO, ShipperMessages.CAPTION_SHIPPERITEM_SAVE, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmShipperEdit_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        ' This form gets its data from a ShipperItem object, so we need to ensure we have a non-null reference to avoid null-ref errors.
        If _shipperItem Is Nothing Then
            Throw New Exception(ShipperMessages.ERROR_SHIPPERITEM_SCREEN_NO_SHIPPERITEM_OBJ)
        End If
        CenterForm(Me)
        txtItem.Text = _shipperItem.Desc
        txtQuantity.Text = _shipperItem.Qty
    End Sub

  Private Sub txtQuantity_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtQuantity.Enter
    txtQuantity.SelectAll()
  End Sub

  Private Sub txtQuantity_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtQuantity.KeyPress
    Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

    KeyAscii = ValidateKeyPressEvent(KeyAscii, "Number", txtQuantity, 0, 0, 0)

    eventArgs.KeyChar = Chr(KeyAscii)
    If KeyAscii = 0 Then
      eventArgs.Handled = True
    End If
  End Sub

#End Region

End Class
