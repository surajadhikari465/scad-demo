Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common
Imports System.Configuration
Imports System.Web.Services

Namespace IRMA

    <DataContract()>
    Public Class ConversionCalculator

#Region "Constants"

        'Volume
        Private Const Gallons As String = "Gallons"
        Private Const Liters As String = "Liters"
        Private Const Milliliters As String = "Milliliters"
        Private Const Ounces As String = "Ounces"
        Private Const Pints As String = "Pints"
        Private Const Quarts As String = "Quarts"
        Private Const GallonsToLiters As Decimal = 3.78541
        Private Const GallonsToMilliliters As Decimal = 3785.41
        Private Const GallonsToOunces As Decimal = 128
        Private Const GallonsToPints As Decimal = 8
        Private Const GallonsToQuarts As Decimal = 4
        Private Const LitersToGallons As Decimal = 0.26417
        Private Const LitersToMilliliters As Decimal = 1000
        Private Const LitersToOunces As Decimal = 33.814
        Private Const LitersToPints As Decimal = 2.11338
        Private Const LitersToQuarts As Decimal = 1.05669
        Private Const MillilitersToGallons As Decimal = 0.00026
        Private Const MillilitersToLiters As Decimal = 0.001
        Private Const MillilitersToOunces As Decimal = 0.03381
        Private Const MillilitersToPints As Decimal = 0.00211
        Private Const MillilitersToQuarts As Decimal = 0.00106
        Private Const OuncesToGallons As Decimal = 0.00781
        Private Const OuncesToLiters As Decimal = 0.02957
        Private Const OuncesToMilliliters As Decimal = 29.5735
        Private Const OuncesToPints As Decimal = 0.0625
        Private Const OuncesToQuarts As Decimal = 0.03125
        Private Const PintsToGallons As Decimal = 0.125
        Private Const PintsToLiters As Decimal = 0.47318
        Private Const PintsToMilliliters As Decimal = 473.177
        Private Const PintsToOunces As Decimal = 16
        Private Const PintsToQuarts As Decimal = 0.5
        Private Const QuartsToGallons As Decimal = 0.25
        Private Const QuartsToLiters As Decimal = 0.94635
        Private Const QuartsToMilliliters As Decimal = 946.353
        Private Const QuartsToOunces As Decimal = 32
        Private Const QuartsToPints As Decimal = 2
        Private Const QuartsToQuarts As Decimal = 1

        'Mass
        Private Const Grams As String = "Grams"
        Private Const Kilograms As String = "Kilograms"
        Private Const Milligrams As String = "Milligrams"
        Private Const Pounds As String = "Pounds"
        Private Const GramToKilograms As Decimal = 0.001
        Private Const GramToPounds As Decimal = 0.00220462262184878
        Private Const GramToOunces As Decimal = 0.0352739619495804
        Private Const GramToMilligrams As Decimal = 1000
        Private Const KilogramToPounds As Decimal = 2.20462262184878
        Private Const KilogramToOunces As Decimal = 35.2739619495804
        Private Const KilogramToGrams As Decimal = 1000
        Private Const KilogramToMilligrams As Decimal = 1000000
        Private Const MilligramToKilograms As Decimal = 0.000001
        Private Const MilligramToPounds As Decimal = 0.00000220462262184878
        Private Const MilligramToOunces As Decimal = 0.0000352739619495804
        Private Const MilligramToGrams As Decimal = 0.001
        Private Const OunceToKilograms As Decimal = 0.028349523125
        Private Const OunceToPounds As Decimal = 0.0625
        Private Const OunceToGrams As Decimal = 28.349523125
        Private Const OunceToMilligrams As Decimal = 28349.523125
        Private Const PoundToKilograms As Decimal = 0.45359237
        Private Const PoundToOunces As Decimal = 16
        Private Const PoundToGrams As Decimal = 453.59237
        Private Const PoundToMilligrams As Decimal = 453592.37

#End Region

#Region "Exposed Methods"
        <WebMethod()>
        Public Function CalculateConversion(ByVal InUnit As String, ByVal OutUnit As String, ByVal Amount As Decimal) As Decimal
            Dim result As Decimal
            logger.Info("CalculateConversion() - Enter")

            Try
                Select Case InUnit
                    Case Is = Gallons
                        result = GLConversion(OutUnit, Amount)
                    Case Is = Liters
                        result = LTConversion(OutUnit, Amount)
                    Case Is = Milliliters
                        result = MLConversion(OutUnit, Amount)
                    Case Is = Ounces
                        result = OZConversion(OutUnit, Amount)
                    Case Is = Pints
                        result = PTConversion(OutUnit, Amount)
                    Case Is = Quarts
                        result = QTConversion(OutUnit, Amount)
                    Case Is = Grams
                        result = GConversion(OutUnit, Amount)
                    Case Is = Kilograms
                        result = KGConversion(OutUnit, Amount)
                    Case Is = Milligrams
                        result = MGConversion(OutUnit, Amount)
                    Case Is = Pounds
                        result = LBConversion(OutUnit, Amount)
                End Select

                Return result

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        <WebMethod()>
        Public Function GetInUnits() As String()
            Dim Results() As String = {Gallons,
                                       Liters,
                                       Milliliters,
                                       Ounces,
                                       Pints,
                                       Quarts,
                                       Grams,
                                       Kilograms,
                                       Milligrams,
                                       Pounds}

            Return Results
        End Function

        <WebMethod()>
        Public Function GetOutUnits(ByVal InUnit As String) As String()
            Dim Results As String()
            Dim OutUnits As String()
            Select Case InUnit
                Case Ounces
                    OutUnits = {Gallons,
                                Liters,
                                Milliliters,
                                Ounces,
                                Pints,
                                Quarts,
                                Grams,
                                Kilograms,
                                Milligrams,
                                Pounds}
                Case Gallons, Liters, Milliliters, Pints, Quarts
                    OutUnits = {Gallons,
                                Liters,
                                Milliliters,
                                Ounces,
                                Pints,
                                Quarts}
                Case Grams, Kilograms, Milligrams, Pounds
                    OutUnits = {Ounces,
                                Grams,
                                Kilograms,
                                Milligrams,
                                Pounds}
                Case Else
                    OutUnits = Nothing
            End Select
            Results = StripInUnitFromOutUnits(InUnit, OutUnits)
            Return Results
        End Function

#End Region

#Region "Private Stuff"

        Public Sub New()

        End Sub

        Private Function StripInUnitFromOutUnits(ByVal InUnit As String, ByVal OutUnits As String()) As String()
            Dim Results As String() = Nothing
            Dim index As Integer = Array.IndexOf(OutUnits, InUnit)

            If index <> -1 Then
                Dim copyStrArr As String() = New String(OutUnits.Length - 2) {}
                ' copy the elements before the found index
                For i As Integer = 0 To index - 1
                    copyStrArr(i) = OutUnits(i)
                Next

                ' copy the elements after the found index
                For i As Integer = index To copyStrArr.Length - 1
                    copyStrArr(i) = OutUnits(i + 1)

                Next
                Results = copyStrArr
            End If
            Return Results
        End Function

#Region "Unit Conversions"
        Private Function GLConversion(ByVal UnitOut As String, ByVal Amount As Decimal) As Decimal
            Dim Result As Decimal
            Select Case UnitOut
                Case Is = Liters
                    Result = GallonsToLiters * Amount
                Case Is = Milliliters
                    Result = GallonsToMilliliters * Amount
                Case Is = Ounces
                    Result = GallonsToOunces * Amount
                Case Is = Pints
                    Result = GallonsToPints * Amount
                Case Is = Quarts
                    Result = GallonsToQuarts * Amount
            End Select
            Return Result
        End Function

        Private Function LTConversion(ByVal UnitOut As String, ByVal Amount As Decimal) As Decimal
            Dim Result As Decimal
            Select Case UnitOut
                Case Is = Gallons
                    Result = LitersToGallons * Amount
                Case Is = Milliliters
                    Result = LitersToMilliliters * Amount
                Case Is = Ounces
                    Result = LitersToOunces * Amount
                Case Is = Pints
                    Result = LitersToPints * Amount
                Case Is = Quarts
                    Result = LitersToQuarts * Amount
            End Select
            Return Result
        End Function

        Private Function MLConversion(ByVal UnitOut As String, ByVal Amount As Decimal) As Decimal
            Dim Result As Decimal
            Select Case UnitOut
                Case Is = Gallons
                    Result = MillilitersToGallons * Amount
                Case Is = Liters
                    Result = MillilitersToLiters * Amount
                Case Is = Ounces
                    Result = MillilitersToOunces * Amount
                Case Is = Pints
                    Result = MillilitersToPints * Amount
                Case Is = Quarts
                    Result = MillilitersToQuarts * Amount
            End Select
            Return Result
        End Function

        Private Function OZConversion(ByVal UnitOut As String, ByVal Amount As Decimal) As Decimal
            Dim Result As Decimal
            Select Case UnitOut
                Case Is = Gallons
                    Result = OuncesToGallons * Amount
                Case Is = Liters
                    Result = OuncesToLiters * Amount
                Case Is = Milliliters
                    Result = OuncesToMilliliters * Amount
                Case Is = Pints
                    Result = OuncesToPints * Amount
                Case Is = Quarts
                    Result = OuncesToQuarts * Amount
                Case Is = Kilograms
                    Result = OunceToKilograms * Amount
                Case Is = Pounds
                    Result = OunceToPounds * Amount
                Case Is = Grams
                    Result = OunceToGrams * Amount
                Case Is = Milligrams
                    Result = OunceToMilligrams * Amount
            End Select
            Return Result
        End Function

        Private Function PTConversion(ByVal UnitOut As String, ByVal Amount As Decimal) As Decimal
            Dim Result As Decimal
            Select Case UnitOut
                Case Is = Gallons
                    Result = PintsToGallons * Amount
                Case Is = Liters
                    Result = PintsToLiters * Amount
                Case Is = Milliliters
                    Result = PintsToMilliliters * Amount
                Case Is = Ounces
                    Result = PintsToOunces * Amount
                Case Is = Quarts
                    Result = PintsToQuarts * Amount
            End Select
            Return Result
        End Function

        Private Function QTConversion(ByVal UnitOut As String, ByVal Amount As Decimal) As Decimal
            Dim Result As Decimal
            Select Case UnitOut
                Case Is = Gallons
                    Result = QuartsToGallons * Amount
                Case Is = Liters
                    Result = QuartsToLiters * Amount
                Case Is = Milliliters
                    Result = QuartsToMilliliters * Amount
                Case Is = Ounces
                    Result = QuartsToOunces * Amount
                Case Is = Pints
                    Result = QuartsToPints * Amount
                Case Is = Quarts
                    Result = QuartsToQuarts * Amount
            End Select
            Return Result
        End Function

        Private Function GConversion(ByVal UnitOut As String, ByVal Amount As Decimal) As Decimal
            Dim Result As Decimal
            Select Case UnitOut
                Case Is = Kilograms
                    Result = GramToKilograms * Amount
                Case Is = Ounces
                    Result = GramToOunces * Amount
                Case Is = Milligrams
                    Result = GramToMilligrams * Amount
                Case Is = Pounds
                    Result = GramToPounds * Amount
            End Select
            Return Result
        End Function

        Private Function KGConversion(ByVal UnitOut As String, ByVal Amount As Decimal) As Decimal
            Dim Result As Decimal
            Select Case UnitOut
                Case Is = Grams
                    Result = KilogramToGrams * Amount
                Case Is = Milligrams
                    Result = KilogramToMilligrams * Amount
                Case Is = Ounces
                    Result = KilogramToOunces * Amount
                Case Is = Pounds
                    Result = KilogramToPounds * Amount
            End Select
            Return Result
        End Function

        Private Function MGConversion(ByVal UnitOut As String, ByVal Amount As Decimal) As Decimal
            Dim Result As Decimal
            Select Case UnitOut
                Case Is = Grams
                    Result = MilligramToGrams * Amount
                Case Is = Kilograms
                    Result = MilligramToKilograms * Amount
                Case Is = Ounces
                    Result = MilligramToOunces * Amount
                Case Is = Pounds
                    Result = MilligramToPounds * Amount
            End Select
            Return Result
        End Function

        Private Function LBConversion(ByVal UnitOut As String, ByVal Amount As Decimal) As Decimal
            Dim Result As Decimal
            Select Case UnitOut
                Case Is = Grams
                    Result = PoundToGrams * Amount
                Case Is = Kilograms
                    Result = PoundToKilograms * Amount
                Case Is = Milligrams
                    Result = PoundToMilligrams * Amount
                Case Is = Ounces
                    Result = PoundToOunces * Amount
            End Select
            Return Result
        End Function

#End Region

#End Region

    End Class

End Namespace