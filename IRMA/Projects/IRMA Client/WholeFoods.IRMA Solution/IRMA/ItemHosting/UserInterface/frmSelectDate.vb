Option Strict Off
Option Explicit On

Friend Class frmSelectDate
    Inherits System.Windows.Forms.Form
    Implements _iNewObj
    Private m_bGoodCreate As Boolean
    Private m_bUserAccepted As Boolean

    Public Sub New(ByVal Message As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.lblMessage.Text = Message
        'Me.txtDate.Text = Now.ToString(ResourcesIRMA.GetString("DateStringFormat"))
        dtpStartDate.Value = System.DateTime.Today
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ByVal Message As String, ByVal DefaultDate As Date)

        InitializeComponent()
        Me.lblMessage.Text = Message
        dtpStartDate.Value = DefaultDate

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        m_bUserAccepted = False
        Me.Hide()

    End Sub

    Public ReadOnly Property ReturnDate() As String
        Get
            ReturnDate = IIf(m_bUserAccepted, dtpStartDate.Value, String.Empty)
        End Get
    End Property

    Private WriteOnly Property iNewObj_GoodCreate() As Boolean Implements _iNewObj.GoodCreate
        Set(ByVal Value As Boolean)
            m_bGoodCreate = Value
        End Set
    End Property

    Private Sub cmdSubmit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSubmit.Click

        m_bUserAccepted = True
        Me.Hide()

    End Sub

    Private Sub frmSelectDate_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

    End Sub

End Class