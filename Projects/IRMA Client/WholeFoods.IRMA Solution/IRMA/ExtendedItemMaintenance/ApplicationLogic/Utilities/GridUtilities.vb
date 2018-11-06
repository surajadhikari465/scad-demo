Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win

Imports WholeFoods.IRMA.ModelLayer.BusinessLogic

Namespace WholeFoods.IRMA.ExtendedItemMaintenance.Logic.Utilites

    ''' <summary>
    ''' Contains various shared utility functions
    ''' for working with the UltraWinGid and its components.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GridUtilities


        Public Shared Sub SetUploadExclusionAppearance(ByRef inGridCell As UltraGridCell, _
        ByVal inValidationMessage As String, ByVal inValidationLevel As EIM_Constants.ValidationLevels)

            If inValidationLevel = EIM_Constants.ValidationLevels.Valid Then

                inGridCell.Value = False

            ElseIf inValidationLevel = EIM_Constants.ValidationLevels.Invalid Then

                inGridCell.Appearance.BackColor = EIM_Constants.GRID_CELL_BACKGROUND_COLOR_VALIDATION_ERROR

                inGridCell.Activation = Activation.AllowEdit


            End If

        End Sub


        Public Shared Sub SetCellStyle(ByRef inGridCell As UltraGridCell, _
                ByVal inValidationMessage As String, ByVal inValidationLevel As EIM_Constants.ValidationLevels)

            SetCellStyle(inGridCell, inValidationMessage, inValidationLevel, True)

        End Sub

        Public Shared Sub SetCellStyle(ByRef inGridCell As UltraGridCell, _
                ByVal inValidationMessage As String, ByVal inValidationLevel As EIM_Constants.ValidationLevels, _
                ByVal inReset As Boolean)

            If inReset Or (Not inReset And inGridCell.Appearance.BackColor = Color.Empty And (IsNothing(inGridCell.ToolTipText) OrElse _
                String.IsNullOrEmpty(inGridCell.ToolTipText))) Then

                inGridCell.ToolTipText = inValidationMessage

                If inValidationLevel = EIM_Constants.ValidationLevels.Valid Then
                    inGridCell.Appearance.FontData.Italic = DefaultableBoolean.False
                    inGridCell.Appearance.FontData.Bold = DefaultableBoolean.False
                    inGridCell.Appearance.BackColor = Color.Empty
                    inGridCell.Appearance.ForeColor = Color.Black
                ElseIf inValidationLevel = EIM_Constants.ValidationLevels.Warning Then
                    inGridCell.Appearance.FontData.Italic = DefaultableBoolean.True
                    inGridCell.Appearance.BackColor = EIM_Constants.GRID_CELL_BACKGROUND_COLOR_VALIDATION_WARNING
                    inGridCell.Appearance.ForeColor = Color.Black
                ElseIf inValidationLevel = EIM_Constants.ValidationLevels.Invalid Then
                    inGridCell.Appearance.FontData.Bold = DefaultableBoolean.True
                    inGridCell.Appearance.BackColor = EIM_Constants.GRID_CELL_BACKGROUND_COLOR_VALIDATION_ERROR
                    inGridCell.Appearance.ForeColor = Color.Black
                End If
            End If

        End Sub

        Private Shared _gridCellCacheByRow As New Hashtable()

        Public Shared Function TryGetGridCell(ByVal inCloumnKey As String, ByRef inGridRow As UltraGridRow, _
                ByRef inOutGridCell As UltraGridCell) As Boolean
            Dim theGridCellsForRowCache As Hashtable

            ' look for the cell cache for the grid row
            theGridCellsForRowCache = CType(GridUtilities._gridCellCacheByRow.Item(inGridRow), Hashtable)

            If IsNothing(theGridCellsForRowCache) Then

                ' can't find it so create it
                theGridCellsForRowCache = New Hashtable()

                ' add it to the master cell cache keyed by grid row
                GridUtilities._gridCellCacheByRow.Add(inGridRow, theGridCellsForRowCache)

                ' add the cells to it
                For Each theUltraGridCell As UltraGridCell In inGridRow.Cells
                    theGridCellsForRowCache.Add(theUltraGridCell.Column.Key.ToLower(), theUltraGridCell)
                Next
            End If

            ' try to get the cell by the column key
            inOutGridCell = CType(theGridCellsForRowCache.Item(inCloumnKey.ToLower()), UltraGridCell)

            ' return true if found
            Return Not IsNothing(inOutGridCell)

        End Function

        Public Shared Sub EnableCell(ByRef inGridRow As UltraGridRow, _
                ByVal inAttributeKey As String)

            Dim theGridCell As UltraGridCell = Nothing

            If GridUtilities.TryGetGridCell(inAttributeKey, inGridRow, theGridCell) Then
                If theGridCell.Appearance.BackColor = EIM_Constants.GRID_CELL_BACKGROUND_COLOR_DISABLED Then
                    theGridCell.Appearance.BackColor = Color.Empty
                End If
                theGridCell.Appearance.AlphaLevel = 0
                theGridCell.Activation = Activation.AllowEdit
            End If
        End Sub

        Public Shared Sub DisableCellWithToolTip(ByRef inGridRow As UltraGridRow, _
                ByVal inAttributeKey As String, ByVal inToolTipText As String)

            Dim theGridCell As UltraGridCell = Nothing

            If GridUtilities.TryGetGridCell(inAttributeKey, inGridRow, theGridCell) Then
                theGridCell.Appearance.BackColor = EIM_Constants.GRID_CELL_BACKGROUND_COLOR_DISABLED
                theGridCell.Appearance.AlphaLevel = 75
                theGridCell.Activation = Activation.NoEdit
                theGridCell.Appearance.Cursor = Cursors.Default

                theGridCell.ToolTipText = inToolTipText
            End If
        End Sub

        Public Shared Sub DisableCell(ByRef inGridRow As UltraGridRow, _
               ByVal inAttributeKey As String)

            DisableCellWithToolTip(inGridRow, inAttributeKey, Nothing)

        End Sub

        Public Shared Function GetGridCellStringValue(ByVal inKey As String, ByRef inGridRow As UltraGridRow) As String

            Dim theGridCell As UltraGridCell = Nothing
            Dim theValue As String = Nothing

            If GridUtilities.TryGetGridCell(inKey, inGridRow, theGridCell) Then

                theValue = GetGridCellStringValue(theGridCell)
            End If

            Return theValue
        End Function

        Public Shared Function GetGridCellStringValue(ByRef inGridCell As UltraGridCell) As String

            Dim theValue As String
            If IsNothing(inGridCell.Value) Or inGridCell.Value Is DBNull.Value Then
                theValue = Nothing
            Else
                theValue = inGridCell.Value.ToString()
            End If
            Return theValue

        End Function

        Public Shared Function GetGridCellIntegerValue(ByRef inGridCell As UltraGridCell) As Integer

            Dim theValue As Integer = 0
            Dim theStringValue As String = GetGridCellStringValue(inGridCell)

            If Not (IsNothing(theStringValue) Or String.IsNullOrEmpty(theStringValue)) Then
                Integer.TryParse(theStringValue, theValue)
            End If

            Return theValue

        End Function

        Public Shared Function GetGridCellIntegerValue(ByVal inKey As String, ByRef inGridRow As UltraGridRow) As Integer

            Dim theValue As Integer = 0
            Dim theStringValue As String = GridUtilities.GetGridCellStringValue(inKey, inGridRow)

            If Not (IsNothing(theStringValue) Or String.IsNullOrEmpty(theStringValue)) Then
                Integer.TryParse(theStringValue, theValue)
            End If

            Return theValue

        End Function
        Public Shared Function GetGridCellDecimalValue(ByRef inGridCell As UltraGridCell) As Decimal

            Dim theValue As Decimal = 0
            Dim theStringValue As String = GetGridCellStringValue(inGridCell)

            If Not (IsNothing(theStringValue) Or String.IsNullOrEmpty(theStringValue)) Then
                Decimal.TryParse(theStringValue, theValue)
            End If

            Return theValue

        End Function

        Public Shared Function GetGridCellSingleValue(ByVal inKey As String, ByRef inGridRow As UltraGridRow) As Single

            Dim theValue As Single = 0
            Dim theStringValue As String = GridUtilities.GetGridCellStringValue(inKey, inGridRow)

            If Not (IsNothing(theStringValue) Or String.IsNullOrEmpty(theStringValue)) Then
                Single.TryParse(theStringValue, theValue)
                'Integer.TryParse(theStringValue, theValue)
            End If

            Return theValue

        End Function

        Public Shared Function GetGridCellBooleanValue(ByRef inGridCell As UltraGridCell) As Boolean

            Dim theValue As Boolean = False
            Dim theStringValue As String = GridUtilities.GetGridCellStringValue(inGridCell)

            If Not (IsNothing(theStringValue) Or String.IsNullOrEmpty(theStringValue)) Then
                Boolean.TryParse(theStringValue, theValue)
            End If

            Return theValue

        End Function

        Public Shared Function GetGridCellBooleanValue(ByVal inKey As String, ByRef inGridRow As UltraGridRow) As Boolean

            Dim theValue As Boolean = False
            Dim theStringValue As String = GridUtilities.GetGridCellStringValue(inKey, inGridRow)

            If Not (IsNothing(theStringValue) Or String.IsNullOrEmpty(theStringValue)) Then
                Boolean.TryParse(theStringValue, theValue)
            End If

            Return theValue

        End Function

    End Class

End Namespace