Imports System.Windows.Forms

Public Class SessionUtility

#Region " Constructors"

    Public Sub New()

    End Sub

#End Region

#Region " Public Methods"

    Public Function IsEmpty(ByVal str As String) As Boolean

        'If ((str = Nothing) Or (str.Trim().Equals(""))) Then
        If [String].IsNullOrEmpty(str) = True Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function GetKeyValue(ByVal myList As Hashtable, ByVal key As String) As ShrinkItem

        Dim val As ShrinkItem = New ShrinkItem
        Dim myEnumerator As IDictionaryEnumerator = myList.GetEnumerator()
        While (myEnumerator.MoveNext())

            If (myEnumerator.Key.ToString().Equals(key)) Then

                val = CType(myEnumerator.Value, ShrinkItem)
                Exit While
            End If

        End While

        Return val

    End Function

    Public Function GetValidString(ByVal str As String) As String

        If (String.IsNullOrEmpty(str)) Then
            Return ""
        Else
            Return str
        End If

    End Function

    Public Function GetValidUpc(ByVal str As String) As String

        Dim badChar As Char() = New Char() {"\t", " ", "\n"}

        If (Not IsEmpty(str)) Then
            str = str.TrimStart(badChar)
            str = str.TrimEnd(badChar)
        End If

        Return str

    End Function

    Public Function CheckIfValidQty(ByVal str As String) As Boolean

        Dim stat As Boolean = True
        Dim qty As Integer = 0

        If (Not IsEmpty(str)) Then

            Try

                qty = Int16.Parse(str)

                If (qty > 100) Then
                    MessageBox.Show(Messages.QTY_WARNING_EXCEEDS_100, "Warning")
                End If

            Catch exp As Exception

                stat = False
                MessageBox.Show(Messages.QTY_ERROR, "Error")

            End Try

        Else

            stat = False

        End If

        Return stat

    End Function

#End Region

End Class
