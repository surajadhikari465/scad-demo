Imports System.Reflection
Imports System.Text.RegularExpressions
Imports Csla.Validation

Namespace Validation

    ''' <summary>
    ''' Implements common business rules for the IRMA project (some of these we may want to move to a "higher" level 
    ''' I.E. WFM.Validation.CommonRules so that we can share them between different projects).
    ''' </summary>
    Public Module CommonRules

#Region "NoZero"
        ''' <summary>
        ''' Rule ensuring a property value is not zero.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name (as a string)
        ''' of the property to validate</param>
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
                  String.Format(My.Resources.NoZeroRule, Csla.Validation.RuleArgs.GetPropertyName(e))
                Return False

            Else
                Return True
            End If

        End Function
#End Region

#Region "DateMustBeFutureDate"
        ''' <summary>
        ''' Rule ensuring a property value of a Date Type must be in the future.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name (as a string)
        ''' of the property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        ''' <remarks>        
        ''' </remarks>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Public Function DateMustBeFutureDate(ByVal target As Object, ByVal e As RuleArgs) As Boolean

            Dim value As Date = CDate(CallByName(target, e.PropertyName, CallType.Get))
            If value < Now.Date Then
                e.Description = String.Format("{0} must be greater then today.", Csla.Validation.RuleArgs.GetPropertyName(e))
                Return False

            Else
                Return True
            End If

        End Function
#End Region

#Region "DateMustBePastDate"
        ''' <summary>
        ''' Rule ensuring a property value of a Date Type must be in the future.
        ''' </summary>
        ''' <param name="target">Object containing the data to validate</param>
        ''' <param name="e">Arguments parameter specifying the name (as a string)
        ''' of the property to validate</param>
        ''' <returns><see langword="false" /> if the rule is broken</returns>
        ''' <remarks>        
        ''' </remarks>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")> _
        Public Function DateMustBePastDate( _
          ByVal target As Object, ByVal e As RuleArgs) As Boolean

            Dim value As Date = CDate(CallByName(target, e.PropertyName, CallType.Get))
            If value > Now.Date Then
                e.Description = _
                  String.Format("{0} must be less then today.", Csla.Validation.RuleArgs.GetPropertyName(e))
                Return False
            Else
                Return True
            End If

        End Function
#End Region

#Region " DateNotOlderThen "

        ''' <summary>
        ''' Rule ensuring that a date value
        ''' is not older then a specified date.
        ''' </summary>
        ''' <param name="target">Object containing date to validate.</param>
        ''' <param name="e">Arguments variable specifying the
        ''' name of the property to validate, along with the min
        ''' allowed date.</param>
        Public Function DateNotOlderThen(ByVal target As Object, ByVal e As RuleArgs) As Boolean

            Dim args As DecoratedRuleArgs = DirectCast(e, DecoratedRuleArgs)
            Dim pi As PropertyInfo = target.GetType.GetProperty(e.PropertyName)
            Dim value As Date = DirectCast(pi.GetValue(target, Nothing), Date)
            Dim min As Date = CType(args("MinDateValue"), Date)

            Dim result As Integer = value.CompareTo(min)
            If result <= 1 Then
                Dim format As String = CStr(args("Format"))
                Dim outValue As String
                If String.IsNullOrEmpty(format) Then
                    outValue = min.ToString
                Else
                    outValue = String.Format(String.Format("{{0:{0}}}", format), min)
                End If
                e.Description = String.Format("{0} must be less then {1}.", Csla.Validation.RuleArgs.GetPropertyName(e), outValue)
                Return False

            Else
                Return True
            End If

        End Function

        ''' <summary>
        ''' Custom <see cref="RuleArgs" /> object required by the
        ''' <see cref="DateNotOlderThen" /> rule method.
        ''' </summary>
        Public Class DateNotOlderThenRuleArgs
            Inherits DecoratedRuleArgs

            ''' <summary>
            ''' Get the Min Date Value for the property.
            ''' </summary>
            Public ReadOnly Property MinDateValue() As Date
                Get
                    Return CType(Me("MinDateValue"), Date)
                End Get
            End Property

            ''' <summary>
            ''' Create a new object.
            ''' </summary>
            ''' <param name="propertyName">Name of the property.</param>
            ''' <param name="MinDateValue">Maximum allowed value for the property.</param>
            Public Sub New(ByVal propertyName As String, ByVal MinDateValue As Date)
                MyBase.New(propertyName)
                Me("MinDateValue") = MinDateValue
                Me("Format") = ""
            End Sub

            ''' <summary>
            ''' Create a new object.
            ''' </summary>
            ''' <param name="propertyName">Name of the property.</param>
            ''' <param name="friendlyName">A friendly name for the property, which
            ''' will be used in place of the property name when
            ''' creating the broken rule description string.</param>
            ''' <param name="MinDateValue">Maximum allowed value for the property.</param>
            Public Sub New(ByVal propertyName As String, ByVal friendlyName As String, ByVal MinDateValue As Date)
                MyBase.New(propertyName, friendlyName)
                Me("MinDateValue") = MinDateValue
                Me("Format") = ""
            End Sub

            ''' <summary>
            ''' Create a new object.
            ''' </summary>
            ''' <param name="propertyName">Name of the property.</param>
            ''' <param name="MinDateValue">Maximum allowed value for the property.</param>
            ''' <param name="format">Format string for the max value.</param>
            Public Sub New(ByVal propertyName As String, ByVal MinDateValue As Date, ByVal format As String)
                MyBase.New(propertyName)
                Me("MinDateValue") = MinDateValue
                Me("Format") = format
            End Sub

            ''' <summary>
            ''' Create a new object.
            ''' </summary>
            ''' <param name="propertyName">Name of the property.</param>
            ''' <param name="friendlyName">A friendly name for the property, which
            ''' will be used in place of the property name when
            ''' creating the broken rule description string.</param>
            ''' <param name="MinDateValue">Maximum allowed value for the property.</param>
            ''' <param name="format">Format string for the max value.</param>
            Public Sub New(ByVal propertyName As String, ByVal friendlyName As String, ByVal MinDateValue As Date, ByVal format As String)
                MyBase.New(propertyName, friendlyName)
                Me("MinDateValue") = MinDateValue
                Me("Format") = format
            End Sub

        End Class

#End Region


    End Module
End Namespace

