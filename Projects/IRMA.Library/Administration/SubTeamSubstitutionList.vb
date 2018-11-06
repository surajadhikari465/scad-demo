Namespace Administration
    <Serializable()> _
    Public Class SubTeamSubstitutionList
        Inherits WfmReadOnlyListBase(Of SubTeamSubstitutionList, SubTeamSubstitutionInfo)

#Region " Factory Methods "

        Public Shared Function NewList() As SubTeamSubstitutionList
            Dim ReturnList As New SubTeamSubstitutionList
            Return ReturnList
        End Function


        Private Sub New()
            'Require use of factory methods
            Me.IsReadOnly = False

        End Sub

        Public Overrides Function ToString() As String
            Dim varSubTeamSubstitutionString As String = ""
            Dim RecordDelim As String
            RecordDelim = "|"
            For Each SubTeamSubstitutions As SubTeamSubstitutionInfo In Me
                varSubTeamSubstitutionString = varSubTeamSubstitutionString + SubTeamSubstitutions.ToString + RecordDelim
            Next
            Return varSubTeamSubstitutionString
        End Function
#End Region


    End Class
End Namespace

