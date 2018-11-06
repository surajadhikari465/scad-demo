Imports WholeFoods.IRMA.ItemChaining.DataAccess

Namespace WholeFoods.IRMA.ItemChaining.UserInterface
    Public Class ItemSearchItemOptions

#Region "Properties"

        Property ShowWFM() As Boolean
            Get
                ShowWFM = chkWFMItems.Visible
            End Get
            Set(ByVal Value As Boolean)
                chkWFMItems.Visible = Value
            End Set
        End Property

        Property ShowHFM() As Boolean
            Get
                ShowHFM = chkHFM.Visible
            End Get
            Set(ByVal Value As Boolean)
                chkHFM.Visible = Value
            End Set
        End Property

#End Region

#Region "Public Methods"

        Public Sub Clear()
            chkDiscontinued.Checked = False
            chkHFM.Checked = False
            chkIncludeDeletedItems.Checked = False
            chkNotAvailable.Checked = False
            chkWFMItems.Checked = False
        End Sub

        Public Sub AddSearchCriteria(ByVal itemSearchHelper As Item.ItemSearchHelper)
            With itemSearchHelper
                .Discontinue_Item = chkDiscontinued.Checked
                .NotAvailable = chkNotAvailable.Checked
                .WFMItems = chkWFMItems.Checked
                .HFM = chkHFM.Checked
                .IncludeDeletedItems = chkIncludeDeletedItems.Checked
            End With
        End Sub

        Public Sub AddNonDefaultSearchCriteria(ByVal itemSearchHelper As Item.ItemSearchHelper)
            With itemSearchHelper
                If chkDiscontinued.Visible Then
                    .Discontinue_Item = chkDiscontinued.Checked
                End If

                If chkNotAvailable.Visible Then
                    .NotAvailable = chkNotAvailable.Checked
                End If

                If chkWFMItems.Visible Then
                    .WFMItems = chkWFMItems.Checked
                End If

                If chkHFM.Visible Then
                    .HFM = chkHFM.Checked
                End If

                If chkIncludeDeletedItems.Visible Then
                    .IncludeDeletedItems = chkIncludeDeletedItems.Checked
                End If
            End With
        End Sub

#End Region

        Private Sub ItemSearchItemOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Set transparency
            Me.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, False)
            Me.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, True)
            Me.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, True)
            Me.BackColor = System.Drawing.Color.Transparent
        End Sub
    End Class
End Namespace