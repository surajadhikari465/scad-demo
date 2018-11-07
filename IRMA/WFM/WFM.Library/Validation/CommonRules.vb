Imports Csla.Validation
Namespace Validation
    Public Module CommonRules
#Region "NoZero"
        ''' <summary>
        ''' Rule ensuring a property value is not zero.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name of the string
        ''' property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        ''' <remarks>
        ''' This implementation uses late binding and can only be used on properties that can be cast as Decimal 
        ''' (we may want to investigate use of Generics for this validation rule to avoid late binding).
        ''' </remarks>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Public Function NoZero( _
          ByVal target As Object, ByVal e As RuleArgs) As Boolean

            Dim value As Decimal = _
              CDec(CallByName(target, e.PropertyName, CallType.Get))
            If value = 0 Then
                e.Description = _
                  String.Format(My.Resources.NoZeroRule, e.PropertyName)
                Return False

            Else
                Return True
            End If

        End Function
#End Region

    End Module


End Namespace

