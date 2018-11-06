Option Explicit On
Option Strict On

Imports System.Data

Public Class UI_Utils

#Region "Public Functions"


    ''' <summary>
    ''' provide multiple interfaces for Generic "FillComboBox" to handle various
    ''' data sources
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function FillComboBox(ByRef controlComboBox As ComboBox, ByVal List As ArrayList, ByVal listValue As String, _
        ByVal listDisplay As String, Optional ByVal MoveFirst As Boolean = True, Optional ByVal AddBlank As Boolean = True) As Boolean

        ' Calls routine to fill ComboBox From ArrayList
        FillComboBox = FillComboBoxFromArrayList(controlComboBox, List, listValue, listDisplay, MoveFirst, AddBlank)

    End Function

    Public Shared Function FillComboBox(ByRef controlComboBox As ComboBox, ByVal bsList As BindingSource, ByVal listValue As String, _
        ByVal listDisplay As String, Optional ByVal MoveFirst As Boolean = True, Optional ByVal AddBlank As Boolean = True) As Boolean

        ' Calls routine to fill ComboBox From ArrayList
        FillComboBox = FillComboBoxFromBindingSource(controlComboBox, bsList, listValue, listDisplay, MoveFirst, AddBlank)

    End Function
#End Region

#Region "Private Functions"

    Private Shared Function FillComboBoxFromArrayList(ByRef controlComboBox As ComboBox, ByVal List As ArrayList, ByVal listValue As String, ByVal listDisplay As String, _
        Optional ByVal MoveFirst As Boolean = True, Optional ByVal AddBlank As Boolean = True) As Boolean

        ' add Blank line to combo box, if specified
        If AddBlank Then
            If List.Count > 0 Then
                'Create Blank version of ArrayList element
                Dim t As Type = List(0).GetType()
                Dim listBlank As Object = Activator.CreateInstance(t)

                'insert blank row at start of drop down, if specified. 
                List.Insert(0, listBlank)
            Else
                'insert blank row at start of drop down, if specified. 
                List.Insert(0, Nothing)
            End If
        End If

        'if no elements are in the array list, exit routine
        If List.Count = 0 Then Exit Function

        ' Bind combobox to array list
        With controlComboBox
            .DataSource = List
            .ValueMember = listValue
            .DisplayMember = listDisplay
        End With

        'Move Box to first row, if desired
        If MoveFirst Then
            controlComboBox.SelectedIndex = 0
        End If

    End Function

    Private Shared Function FillComboBoxFromBindingSource(ByRef controlComboBox As ComboBox, ByVal bsData As BindingSource, ByVal listValue As String, ByVal listDisplay As String, _
        Optional ByVal MoveFirst As Boolean = True, Optional ByVal AddBlank As Boolean = True) As Boolean

        ' add Blank line to combo box, if specified
        If AddBlank Then
            If bsData.List.Count > 0 Then
                'Create Blank version of ArrayList element
                Dim t As Type = bsData.List(0).GetType()
                Dim listBlank As Object = Activator.CreateInstance(t)

                'insert blank row at start of drop down, if specified. 
                bsData.List.Insert(0, listBlank)
            Else
                'insert blank row at start of drop down, if specified. 
                bsData.Insert(0, Nothing)
            End If
        End If

        'if no elements are in the array list, exit routine
        If bsData.Count = 0 Then Exit Function

        ' Bind combobox to BindingSource
        With controlComboBox
            .DataSource = bsData.List
            .ValueMember = listValue
            .DisplayMember = listDisplay
        End With

        'Move Box to first row, if desired
        If MoveFirst Then
            controlComboBox.SelectedIndex = 0
        End If

    End Function
#End Region

End Class
