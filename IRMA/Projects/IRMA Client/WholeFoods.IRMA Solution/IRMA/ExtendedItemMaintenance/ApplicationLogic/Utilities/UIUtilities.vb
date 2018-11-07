Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win

Imports WholeFoods.IRMA.ModelLayer.BusinessLogic

Namespace WholeFoods.IRMA.ExtendedItemMaintenance.Logic.Utilites

    ''' <summary>
    ''' Contains various shared UI utility functions.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UIUtilities

        Public Shared Function GetComboBoxIntegerValue(ByRef inComboBox As ComboBox) As Integer
            Dim theValue As Integer = -1
            If Not IsNothing(inComboBox.SelectedItem) Then
                theValue = CType(inComboBox.SelectedItem, VB6.ListBoxItem).ItemData
            End If
            Return theValue
        End Function

        Public Shared Function GetComboBoxStringValue(ByRef inComboBox As ComboBox) As String
            Dim theValue As String = Nothing
            If Not IsNothing(inComboBox.SelectedItem) Then
                theValue = inComboBox.SelectedItem.ToString()
            End If
            Return theValue
        End Function

        Public Shared Function MakeSimpleSingularOrPlural(ByVal inString As String, ByVal inCount As Integer) As String

            If inCount > 1 Then
                inString = inString + "s"
            End If

            Return inString

        End Function

        Public Shared Sub LoadDefautJuisdictionCombo(ByRef cmbComboBox As System.Windows.Forms.ComboBox)

            Dim NewIndex As Integer

            cmbComboBox.Items.Clear()

            NewIndex = cmbComboBox.Items.Add("- All -")
            VB6.SetItemData(cmbComboBox, NewIndex, -1)

            NewIndex = cmbComboBox.Items.Add("Yes")
            VB6.SetItemData(cmbComboBox, NewIndex, 1)

            NewIndex = cmbComboBox.Items.Add("No")
            VB6.SetItemData(cmbComboBox, NewIndex, 0)

            cmbComboBox.SelectedIndex = 0

        End Sub

    End Class

End Namespace