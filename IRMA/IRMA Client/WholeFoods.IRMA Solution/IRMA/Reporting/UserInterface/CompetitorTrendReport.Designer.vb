Namespace WholeFoods.IRMA.Reporting.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class CompetitorTrendReport
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
            Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Store_Name")
            Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
            Me.ugData = New Infragistics.Win.UltraWinGrid.UltraGrid
            Me.ucTrend = New Infragistics.Win.UltraWinChart.UltraChart
            CType(Me.ugData, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ucTrend, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ugData
            '
            Me.ugData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Appearance1.BackColor = System.Drawing.Color.White
            Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
            Me.ugData.DisplayLayout.Appearance = Appearance1
            Me.ugData.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
            UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
            UltraGridColumn1.Header.Caption = "Store"
            UltraGridColumn1.Header.VisiblePosition = 0
            UltraGridColumn1.MaxWidth = 150
            UltraGridColumn1.MinWidth = 150
            UltraGridColumn1.Width = 150
            UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1})
            Me.ugData.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
            Me.ugData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
            Me.ugData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
            Me.ugData.DisplayLayout.MaxRowScrollRegions = 1
            Appearance2.BackColor = System.Drawing.Color.LightSteelBlue
            Appearance2.BackColor2 = System.Drawing.Color.White
            Me.ugData.DisplayLayout.Override.HeaderAppearance = Appearance2
            Me.ugData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
            Me.ugData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
            Me.ugData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
            Me.ugData.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
            Me.ugData.Location = New System.Drawing.Point(12, 339)
            Me.ugData.Name = "ugData"
            Me.ugData.Size = New System.Drawing.Size(768, 165)
            Me.ugData.TabIndex = 2
            Me.ugData.Text = "UltraGrid1"
            '
            ''UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
            ''ChartType' must be persisted ahead of any Axes change made in design time.
            '
            Me.ucTrend.ChartType = Infragistics.UltraChart.[Shared].Styles.ChartType.SplineChart
            '
            'ucTrend
            '
            Me.ucTrend.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ucTrend.Axis.X.Extent = 77
            Me.ucTrend.Axis.X.Labels.Flip = False
            Me.ucTrend.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near
            Me.ucTrend.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>"
            Me.ucTrend.Axis.X.Labels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.VerticalLeftFacing
            Me.ucTrend.Axis.X.Labels.OrientationAngle = 0
            Me.ucTrend.Axis.X.Labels.SeriesLabels.Flip = False
            Me.ucTrend.Axis.X.Labels.SeriesLabels.FormatString = ""
            Me.ucTrend.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.VerticalLeftFacing
            Me.ucTrend.Axis.X.Labels.SeriesLabels.OrientationAngle = 0
            Me.ucTrend.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.X.MajorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro
            Me.ucTrend.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.X.MajorGridLines.Thickness = 1
            Me.ucTrend.Axis.X.MajorGridLines.Visible = True
            Me.ucTrend.Axis.X.MinorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray
            Me.ucTrend.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.X.MinorGridLines.Thickness = 1
            Me.ucTrend.Axis.X.MinorGridLines.Visible = True
            Me.ucTrend.Axis.X.ScrollScale.Height = 10
            Me.ucTrend.Axis.X.ScrollScale.Visible = False
            Me.ucTrend.Axis.X.ScrollScale.Width = 15
            Me.ucTrend.Axis.X.TickmarkInterval = 0
            Me.ucTrend.Axis.X.Visible = True
            Me.ucTrend.Axis.X2.Labels.Flip = False
            Me.ucTrend.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far
            Me.ucTrend.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>"
            Me.ucTrend.Axis.X2.Labels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.VerticalLeftFacing
            Me.ucTrend.Axis.X2.Labels.OrientationAngle = 0
            Me.ucTrend.Axis.X2.Labels.SeriesLabels.Flip = False
            Me.ucTrend.Axis.X2.Labels.SeriesLabels.FormatString = ""
            Me.ucTrend.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far
            Me.ucTrend.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.VerticalLeftFacing
            Me.ucTrend.Axis.X2.Labels.SeriesLabels.OrientationAngle = 0
            Me.ucTrend.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.X2.MajorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro
            Me.ucTrend.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.X2.MajorGridLines.Thickness = 1
            Me.ucTrend.Axis.X2.MajorGridLines.Visible = True
            Me.ucTrend.Axis.X2.MinorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray
            Me.ucTrend.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.X2.MinorGridLines.Thickness = 1
            Me.ucTrend.Axis.X2.MinorGridLines.Visible = False
            Me.ucTrend.Axis.X2.ScrollScale.Height = 10
            Me.ucTrend.Axis.X2.ScrollScale.Visible = False
            Me.ucTrend.Axis.X2.ScrollScale.Width = 15
            Me.ucTrend.Axis.X2.TickmarkInterval = 0
            Me.ucTrend.Axis.X2.Visible = False
            Me.ucTrend.Axis.Y.Extent = 32
            Me.ucTrend.Axis.Y.Labels.Flip = False
            Me.ucTrend.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far
            Me.ucTrend.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:c>"
            Me.ucTrend.Axis.Y.Labels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.Horizontal
            Me.ucTrend.Axis.Y.Labels.OrientationAngle = 0
            Me.ucTrend.Axis.Y.Labels.SeriesLabels.Flip = False
            Me.ucTrend.Axis.Y.Labels.SeriesLabels.FormatString = ""
            Me.ucTrend.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far
            Me.ucTrend.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.Horizontal
            Me.ucTrend.Axis.Y.Labels.SeriesLabels.OrientationAngle = 0
            Me.ucTrend.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.Y.MajorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro
            Me.ucTrend.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.Y.MajorGridLines.Thickness = 1
            Me.ucTrend.Axis.Y.MajorGridLines.Visible = True
            Me.ucTrend.Axis.Y.Margin.Far.Value = 5
            Me.ucTrend.Axis.Y.Margin.Near.Value = 5
            Me.ucTrend.Axis.Y.MinorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray
            Me.ucTrend.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.Y.MinorGridLines.Thickness = 1
            Me.ucTrend.Axis.Y.MinorGridLines.Visible = False
            Me.ucTrend.Axis.Y.ScrollScale.Height = 10
            Me.ucTrend.Axis.Y.ScrollScale.Visible = False
            Me.ucTrend.Axis.Y.ScrollScale.Width = 15
            Me.ucTrend.Axis.Y.TickmarkInterval = 0
            Me.ucTrend.Axis.Y.Visible = True
            Me.ucTrend.Axis.Y2.Labels.Flip = False
            Me.ucTrend.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near
            Me.ucTrend.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>"
            Me.ucTrend.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.Horizontal
            Me.ucTrend.Axis.Y2.Labels.OrientationAngle = 0
            Me.ucTrend.Axis.Y2.Labels.SeriesLabels.Flip = False
            Me.ucTrend.Axis.Y2.Labels.SeriesLabels.FormatString = ""
            Me.ucTrend.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near
            Me.ucTrend.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.Horizontal
            Me.ucTrend.Axis.Y2.Labels.SeriesLabels.OrientationAngle = 0
            Me.ucTrend.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.Y2.MajorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro
            Me.ucTrend.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.Y2.MajorGridLines.Thickness = 1
            Me.ucTrend.Axis.Y2.MajorGridLines.Visible = True
            Me.ucTrend.Axis.Y2.Margin.Far.Value = 5
            Me.ucTrend.Axis.Y2.Margin.Near.Value = 5
            Me.ucTrend.Axis.Y2.MinorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray
            Me.ucTrend.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.Y2.MinorGridLines.Thickness = 1
            Me.ucTrend.Axis.Y2.MinorGridLines.Visible = False
            Me.ucTrend.Axis.Y2.ScrollScale.Height = 10
            Me.ucTrend.Axis.Y2.ScrollScale.Visible = False
            Me.ucTrend.Axis.Y2.ScrollScale.Width = 15
            Me.ucTrend.Axis.Y2.TickmarkInterval = 0
            Me.ucTrend.Axis.Y2.Visible = False
            Me.ucTrend.Axis.Z.Labels.Flip = False
            Me.ucTrend.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near
            Me.ucTrend.Axis.Z.Labels.ItemFormatString = "<DATA_VALUE:00.##>"
            Me.ucTrend.Axis.Z.Labels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.Horizontal
            Me.ucTrend.Axis.Z.Labels.OrientationAngle = 0
            Me.ucTrend.Axis.Z.Labels.SeriesLabels.Flip = False
            Me.ucTrend.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near
            Me.ucTrend.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.Horizontal
            Me.ucTrend.Axis.Z.Labels.SeriesLabels.OrientationAngle = 0
            Me.ucTrend.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.Z.MajorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro
            Me.ucTrend.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.Z.MajorGridLines.Thickness = 1
            Me.ucTrend.Axis.Z.MajorGridLines.Visible = True
            Me.ucTrend.Axis.Z.MinorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray
            Me.ucTrend.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.Z.MinorGridLines.Thickness = 1
            Me.ucTrend.Axis.Z.MinorGridLines.Visible = False
            Me.ucTrend.Axis.Z.ScrollScale.Height = 10
            Me.ucTrend.Axis.Z.ScrollScale.Visible = False
            Me.ucTrend.Axis.Z.ScrollScale.Width = 15
            Me.ucTrend.Axis.Z.TickmarkInterval = 0
            Me.ucTrend.Axis.Z.Visible = False
            Me.ucTrend.Axis.Z2.Labels.Flip = False
            Me.ucTrend.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near
            Me.ucTrend.Axis.Z2.Labels.ItemFormatString = ""
            Me.ucTrend.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.Horizontal
            Me.ucTrend.Axis.Z2.Labels.OrientationAngle = 0
            Me.ucTrend.Axis.Z2.Labels.SeriesLabels.Flip = False
            Me.ucTrend.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near
            Me.ucTrend.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.[Shared].Styles.TextOrientation.Horizontal
            Me.ucTrend.Axis.Z2.Labels.SeriesLabels.OrientationAngle = 0
            Me.ucTrend.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.Axis.Z2.MajorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro
            Me.ucTrend.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.Z2.MajorGridLines.Thickness = 1
            Me.ucTrend.Axis.Z2.MajorGridLines.Visible = True
            Me.ucTrend.Axis.Z2.MinorGridLines.AlphaLevel = CType(255, Byte)
            Me.ucTrend.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray
            Me.ucTrend.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dot
            Me.ucTrend.Axis.Z2.MinorGridLines.Thickness = 1
            Me.ucTrend.Axis.Z2.MinorGridLines.Visible = False
            Me.ucTrend.Axis.Z2.ScrollScale.Height = 10
            Me.ucTrend.Axis.Z2.ScrollScale.Visible = False
            Me.ucTrend.Axis.Z2.ScrollScale.Width = 15
            Me.ucTrend.Axis.Z2.TickmarkInterval = 0
            Me.ucTrend.Axis.Z2.Visible = False
            Me.ucTrend.Border.CornerRadius = 5
            Me.ucTrend.ColorModel.AlphaLevel = CType(150, Byte)
            Me.ucTrend.ColorModel.ModelStyle = Infragistics.UltraChart.[Shared].Styles.ColorModels.LinearRange
            Me.ucTrend.ColorModel.Scaling = Infragistics.UltraChart.[Shared].Styles.ColorScaling.Oscillating
            Me.ucTrend.Data.EmptyStyle.LineStyle.DrawStyle = Infragistics.UltraChart.[Shared].Styles.LineDrawStyle.Dash
            Me.ucTrend.Data.EmptyStyle.LineStyle.EndStyle = Infragistics.UltraChart.[Shared].Styles.LineCapStyle.NoAnchor
            Me.ucTrend.Data.EmptyStyle.LineStyle.MidPointAnchors = False
            Me.ucTrend.Data.EmptyStyle.LineStyle.StartStyle = Infragistics.UltraChart.[Shared].Styles.LineCapStyle.NoAnchor
            Me.ucTrend.Effects.Enabled = False
            Me.ucTrend.ForeColor = System.Drawing.SystemColors.ControlText
            Me.ucTrend.Legend.Location = Infragistics.UltraChart.[Shared].Styles.LegendLocation.Left
            Me.ucTrend.Legend.SpanPercentage = 20
            Me.ucTrend.Legend.Visible = True
            Me.ucTrend.Location = New System.Drawing.Point(12, 12)
            Me.ucTrend.Name = "ucTrend"
            Me.ucTrend.Size = New System.Drawing.Size(768, 321)
            Me.ucTrend.TabIndex = 3
            Me.ucTrend.TitleTop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold)
            Me.ucTrend.TitleTop.HorizontalAlign = System.Drawing.StringAlignment.Center
            Me.ucTrend.TitleTop.Text = "Top Title"
            Me.ucTrend.Tooltips.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!)
            Me.ucTrend.Tooltips.UseControl = False
            '
            'CompetitorTrendReport
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(792, 516)
            Me.Controls.Add(Me.ucTrend)
            Me.Controls.Add(Me.ugData)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(800, 550)
            Me.Name = "CompetitorTrendReport"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Competitor Trend Report"
            CType(Me.ugData, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ucTrend, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents ugData As Infragistics.Win.UltraWinGrid.UltraGrid
        Private WithEvents ucTrend As Infragistics.Win.UltraWinChart.UltraChart
    End Class
End Namespace