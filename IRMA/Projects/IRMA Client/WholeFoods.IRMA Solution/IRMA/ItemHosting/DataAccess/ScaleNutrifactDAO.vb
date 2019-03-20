Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class ScaleNutrifactDAO
        Public Shared Function GetNutriFact(ByVal ID As Integer) As ScaleNutrifactBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim scaleNutrifact As New ScaleNutrifactBO
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "NutriFactsID"
                currentParam.Value = ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetNutrifact", paramList)

                While results.Read
                    scaleNutrifact.ID = ID
                    scaleNutrifact.Description = results.GetString(results.GetOrdinal("Description"))
                    scaleNutrifact.Scale_LabelFormat_ID = results.GetInt32(results.GetOrdinal("Scale_LabelFormat_ID"))
                    scaleNutrifact.ServingUnits = GetIntegerValue(results.GetValue(results.GetOrdinal("ServingUnits")))
                    scaleNutrifact.ServingsPerPortion = Convert.ToDouble(results.GetValue(results.GetOrdinal("ServingsPerPortion")))
                    scaleNutrifact.SizeWeight = GetIntegerValue(results.GetValue(results.GetOrdinal("SizeWeight")))
                    scaleNutrifact.Calories = GetIntegerValue(results.GetValue(results.GetOrdinal("Calories")))
                    scaleNutrifact.CaloriesFat = GetIntegerValue(results.GetValue(results.GetOrdinal("CaloriesFat")))
                    scaleNutrifact.CaloriesSaturatedFat = GetIntegerValue(results.GetValue(results.GetOrdinal("CaloriesSaturatedFat")))
                    scaleNutrifact.ServingPerContainer = IIf(IsDBNull(results.GetOrdinal("ServingPerContainer")), "VARIED", results.GetString(results.GetOrdinal("ServingPerContainer")))
                    scaleNutrifact.TotalFatWeight = GetDecimalValue(results.GetValue(results.GetOrdinal("TotalFatWeight")))
                    scaleNutrifact.TotalFatPercentage = GetIntegerValue(results.GetValue(results.GetOrdinal("TotalFatPercentage")))
                    scaleNutrifact.SaturatedFatWeight = GetDecimalValue(results.GetValue(results.GetOrdinal("SaturatedFatWeight")))
                    scaleNutrifact.SaturatedFatPercent = GetIntegerValue(results.GetValue(results.GetOrdinal("SaturatedFatPercent")))
                    scaleNutrifact.PolyunsaturatedFat = GetDecimalValue(results.GetValue(results.GetOrdinal("PolyunsaturatedFat")))
                    scaleNutrifact.MonounsaturatedFat = GetDecimalValue(results.GetValue(results.GetOrdinal("MonounsaturatedFat")))
                    scaleNutrifact.CholesterolWeight = GetDecimalValue(results.GetValue(results.GetOrdinal("CholesterolWeight")))
                    scaleNutrifact.CholesterolPercent = GetIntegerValue(results.GetValue(results.GetOrdinal("CholesterolPercent")))
                    scaleNutrifact.SodiumWeight = GetDecimalValue(results.GetValue(results.GetOrdinal("SodiumWeight")))
                    scaleNutrifact.SodiumPercent = GetIntegerValue(results.GetValue(results.GetOrdinal("SodiumPercent")))
                    scaleNutrifact.PotassiumWeight = GetDecimalValue(results.GetValue(results.GetOrdinal("PotassiumWeight")))
                    scaleNutrifact.PotassiumPercent = GetIntegerValue(results.GetValue(results.GetOrdinal("PotassiumPercent")))
                    scaleNutrifact.TotalCarbohydrateWeight = GetDecimalValue(results.GetValue(results.GetOrdinal("TotalCarbohydrateWeight")))
                    scaleNutrifact.TotalCarbohydratePercent = GetIntegerValue(results.GetValue(results.GetOrdinal("TotalCarbohydratePercent")))
                    scaleNutrifact.DietaryFiberWeight = GetDecimalValue(results.GetValue(results.GetOrdinal("DietaryFiberWeight")))
                    scaleNutrifact.DietaryFiberPercent = GetIntegerValue(results.GetValue(results.GetOrdinal("DietaryFiberPercent")))
                    scaleNutrifact.SolubleFiber = GetDecimalValue(results.GetValue(results.GetOrdinal("SolubleFiber")))
                    scaleNutrifact.InsolubleFiber = GetDecimalValue(results.GetValue(results.GetOrdinal("InsolubleFiber")))
                    scaleNutrifact.Sugar = GetDecimalValue(results.GetValue(results.GetOrdinal("Sugar")))
                    scaleNutrifact.SugarAlcohol = GetDecimalValue(results.GetValue(results.GetOrdinal("SugarAlcohol")))
                    scaleNutrifact.OtherCarbohydrates = GetDecimalValue(results.GetValue(results.GetOrdinal("OtherCarbohydrates")))
                    scaleNutrifact.ProteinWeight = GetDecimalValue(results.GetValue(results.GetOrdinal("ProteinWeight")))
                    scaleNutrifact.ProteinPercent = GetIntegerValue(results.GetValue(results.GetOrdinal("ProteinPercent")))
                    scaleNutrifact.VitaminA = GetIntegerValue(results.GetValue(results.GetOrdinal("VitaminA")))
                    scaleNutrifact.Betacarotene = GetIntegerValue(results.GetValue(results.GetOrdinal("Betacarotene")))
                    scaleNutrifact.VitaminC = GetIntegerValue(results.GetValue(results.GetOrdinal("VitaminC")))
                    scaleNutrifact.Calcium = GetIntegerValue(results.GetValue(results.GetOrdinal("Calcium")))
                    scaleNutrifact.Iron = GetIntegerValue(results.GetValue(results.GetOrdinal("Iron")))
                    scaleNutrifact.VitaminD = GetIntegerValue(results.GetValue(results.GetOrdinal("VitaminD")))
                    scaleNutrifact.VitaminE = GetIntegerValue(results.GetValue(results.GetOrdinal("VitaminE")))
                    scaleNutrifact.Thiamin = GetIntegerValue(results.GetValue(results.GetOrdinal("Thiamin")))
                    scaleNutrifact.Riboflavin = GetIntegerValue(results.GetValue(results.GetOrdinal("Riboflavin")))
                    scaleNutrifact.Niacin = GetIntegerValue(results.GetValue(results.GetOrdinal("Niacin")))
                    scaleNutrifact.VitaminB6 = GetIntegerValue(results.GetValue(results.GetOrdinal("VitaminB6")))
                    scaleNutrifact.Folate = GetIntegerValue(results.GetValue(results.GetOrdinal("Folate")))
                    scaleNutrifact.VitaminB12 = GetIntegerValue(results.GetValue(results.GetOrdinal("VitaminB12")))
                    scaleNutrifact.Biotin = GetIntegerValue(results.GetValue(results.GetOrdinal("Biotin")))
                    scaleNutrifact.PantothenicAcid = GetIntegerValue(results.GetValue(results.GetOrdinal("PantothenicAcid")))
                    scaleNutrifact.Phosphorous = GetIntegerValue(results.GetValue(results.GetOrdinal("Phosphorous")))
                    scaleNutrifact.Iodine = GetIntegerValue(results.GetValue(results.GetOrdinal("Iodine")))
                    scaleNutrifact.Magnesium = GetIntegerValue(results.GetValue(results.GetOrdinal("Magnesium")))
                    scaleNutrifact.Zinc = GetIntegerValue(results.GetValue(results.GetOrdinal("Zinc")))
                    scaleNutrifact.Copper = GetIntegerValue(results.GetValue(results.GetOrdinal("Copper")))
                    scaleNutrifact.Transfat = GetIntegerValue(results.GetValue(results.GetOrdinal("Transfat")))
                    scaleNutrifact.TransfatWeight = GetDecimalValue(results.GetValue(results.GetOrdinal("TransfatWeight")))
                    scaleNutrifact.CaloriesFromTransFat = GetIntegerValue(results.GetValue(results.GetOrdinal("CaloriesFromTransFat")))
                    scaleNutrifact.Om6Fatty = GetDecimalValue(results.GetValue(results.GetOrdinal("Om6Fatty")))
                    scaleNutrifact.Om3Fatty = GetDecimalValue(results.GetValue(results.GetOrdinal("Om3Fatty")))
                    scaleNutrifact.Starch = GetDecimalValue(results.GetValue(results.GetOrdinal("Starch")))
                    scaleNutrifact.Chloride = GetIntegerValue(results.GetValue(results.GetOrdinal("Chloride")))
                    scaleNutrifact.Chromium = GetIntegerValue(results.GetValue(results.GetOrdinal("Chromium")))
                    scaleNutrifact.VitaminK = GetIntegerValue(results.GetValue(results.GetOrdinal("VitaminK")))
                    scaleNutrifact.Manganese = GetIntegerValue(results.GetValue(results.GetOrdinal("Manganese")))
                    scaleNutrifact.Molybdenum = GetIntegerValue(results.GetValue(results.GetOrdinal("Molybdenum")))
                    scaleNutrifact.Selenium = GetIntegerValue(results.GetValue(results.GetOrdinal("Selenium")))
                    scaleNutrifact.ServingSizeDesc = results.GetString(results.GetOrdinal("ServingSizeDesc"))

                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return scaleNutrifact

        End Function

        Private Shared Function GetDecimalValue(ByRef inValue As Object) As Decimal

            Dim theDecValue As Decimal = 0.0

            ' return zero if the incoming value is nothing, DBNull,
            ' or cannot be parsed to a valid int
            If Not IsNothing(inValue) AndAlso Not inValue.Equals(DBNull.Value) Then
                If Not Decimal.TryParse(inValue.ToString(), theDecValue) Then
                    theDecValue = 0.0
                End If
            End If

            Return theDecValue

        End Function

        Private Shared Function GetIntegerValue(ByRef inValue As Object) As Integer

            Dim theIntValue As Integer = 0

            ' return zero if the incoming value is nothing, DBNull,
            ' or cannot be parsed to a valid int
            If Not IsNothing(inValue) AndAlso Not inValue.Equals(DBNull.Value) Then
                If Not Integer.TryParse(inValue.ToString(), theIntValue) Then
                    theIntValue = 0
                End If
            End If

            Return theIntValue

        End Function

        Public Shared Function GetNutrifactComboList() As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim scaleNutrifact As ScaleNutrifactBO
            Dim scaleNutrifactList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetNutrifacts")

                While results.Read
                    scaleNutrifact = New ScaleNutrifactBO()
                    scaleNutrifact.ID = results.GetInt32(results.GetOrdinal("NutrifactsID"))
                    scaleNutrifact.Description = results.GetString(results.GetOrdinal("Description"))

                    scaleNutrifactList.Add(scaleNutrifact)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return scaleNutrifactList
        End Function

        Public Shared Function GetNutriFactByItem(ByVal itemKey As Integer, ByVal jurisdiction As Integer, Optional ByVal isScaleItem As Boolean = True) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim nutrifactID As Integer = -1

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Jurisdiction"
                currentParam.Value = jurisdiction
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "IsScaleItem"
                currentParam.Value = isScaleItem
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetNutriFactByItem", paramList)

                While results.Read
                    If (Not results.IsDBNull(results.GetOrdinal("Nutrifact_ID"))) Then
                        nutrifactID = results.GetInt32(results.GetOrdinal("Nutrifact_ID"))
                    Else
                        nutrifactID = 0
                    End If
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return nutrifactID
        End Function

        Public Shared Function Save(ByVal scaleNutrifact As ScaleNutrifactBO, ByVal AlternateJurisdiction As Boolean) As Boolean

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim isSuccess As Boolean = True

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ID"
                currentParam.Value = scaleNutrifact.ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = scaleNutrifact.Description
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                If AlternateJurisdiction Then
                    results = factory.GetStoredProcedureDataReader("Scale_CheckForDuplicateNutrifact", paramList)
                    While results.Read
                        If results.GetInt32(results.GetOrdinal("DuplicateCount")) > 0 Then
                            ' this is a duplicate
                            isSuccess = False
                            Exit While
                        End If
                    End While
                    results.Close()
                End If           

                If isSuccess Then
                    currentParam = New DBParam
                    currentParam.Name = "Scale_LabelFormat_ID"
                    currentParam.Value = scaleNutrifact.Scale_LabelFormat_ID
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "ServingUnits"
                    currentParam.Value = scaleNutrifact.ServingUnits
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "ServingsPerPortion"
                    currentParam.Value = scaleNutrifact.ServingsPerPortion
                    currentParam.Type = DBParamType.Float
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "SizeWeight"
                    currentParam.Value = scaleNutrifact.SizeWeight
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Calories"
                    currentParam.Value = scaleNutrifact.Calories
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "CaloriesFat"
                    currentParam.Value = scaleNutrifact.CaloriesFat
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "CaloriesSaturatedFat"
                    currentParam.Value = scaleNutrifact.CaloriesSaturatedFat
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "ServingPerContainer"
                    currentParam.Value = scaleNutrifact.ServingPerContainer
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "TotalFatWeight"
                    currentParam.Value = scaleNutrifact.TotalFatWeight
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "TotalFatPercentage"
                    currentParam.Value = scaleNutrifact.TotalFatPercentage
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "SaturatedFatWeight"
                    currentParam.Value = scaleNutrifact.SaturatedFatWeight
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "SaturatedFatPercent"
                    currentParam.Value = scaleNutrifact.SaturatedFatPercent
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "PolyunsaturatedFat"
                    currentParam.Value = scaleNutrifact.PolyunsaturatedFat
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "MonounsaturatedFat"
                    currentParam.Value = scaleNutrifact.MonounsaturatedFat
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "CholesterolWeight"
                    currentParam.Value = scaleNutrifact.CholesterolWeight
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "CholesterolPercent"
                    currentParam.Value = scaleNutrifact.CholesterolPercent
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "SodiumWeight"
                    currentParam.Value = scaleNutrifact.SodiumWeight
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "SodiumPercent"
                    currentParam.Value = scaleNutrifact.SodiumPercent
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "PotassiumWeight"
                    currentParam.Value = scaleNutrifact.PotassiumWeight
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "PotassiumPercent"
                    currentParam.Value = scaleNutrifact.PotassiumPercent
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "TotalCarbohydrateWeight"
                    currentParam.Value = scaleNutrifact.TotalCarbohydrateWeight
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "TotalCarbohydratePercent"
                    currentParam.Value = scaleNutrifact.TotalCarbohydratePercent
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "DietaryFiberWeight"
                    currentParam.Value = scaleNutrifact.DietaryFiberWeight
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "DietaryFiberPercent"
                    currentParam.Value = scaleNutrifact.DietaryFiberPercent
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "SolubleFiber"
                    currentParam.Value = scaleNutrifact.SolubleFiber
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "InsolubleFiber"
                    currentParam.Value = scaleNutrifact.InsolubleFiber
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Sugar"
                    currentParam.Value = scaleNutrifact.Sugar
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "SugarAlcohol"
                    currentParam.Value = scaleNutrifact.SugarAlcohol
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "OtherCarbohydrates"
                    currentParam.Value = scaleNutrifact.OtherCarbohydrates
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "ProteinWeight"
                    currentParam.Value = scaleNutrifact.ProteinWeight
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "ProteinPercent"
                    currentParam.Value = scaleNutrifact.ProteinPercent
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "VitaminA"
                    currentParam.Value = scaleNutrifact.VitaminA
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Betacarotene"
                    currentParam.Value = scaleNutrifact.Betacarotene
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "VitaminC"
                    currentParam.Value = scaleNutrifact.VitaminC
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Calcium"
                    currentParam.Value = scaleNutrifact.Calcium
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Iron"
                    currentParam.Value = scaleNutrifact.Iron
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "VitaminD"
                    currentParam.Value = scaleNutrifact.VitaminD
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "VitaminE"
                    currentParam.Value = scaleNutrifact.VitaminE
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Thiamin"
                    currentParam.Value = scaleNutrifact.Thiamin
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Riboflavin"
                    currentParam.Value = scaleNutrifact.Riboflavin
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Niacin"
                    currentParam.Value = scaleNutrifact.Niacin
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "VitaminB6"
                    currentParam.Value = scaleNutrifact.VitaminB6
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Folate"
                    currentParam.Value = scaleNutrifact.Folate
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "VitaminB12"
                    currentParam.Value = scaleNutrifact.VitaminB12
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Biotin"
                    currentParam.Value = scaleNutrifact.Biotin
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "PantothenicAcid"
                    currentParam.Value = scaleNutrifact.PantothenicAcid
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Phosphorous"
                    currentParam.Value = scaleNutrifact.Phosphorous
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Iodine"
                    currentParam.Value = scaleNutrifact.Iodine
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Magnesium"
                    currentParam.Value = scaleNutrifact.Magnesium
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zinc"
                    currentParam.Value = scaleNutrifact.Zinc
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Copper"
                    currentParam.Value = scaleNutrifact.Copper
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Transfat"
                    currentParam.Value = scaleNutrifact.Transfat
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "TransfatWeight"
                    currentParam.Value = scaleNutrifact.TransfatWeight
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "CaloriesFromTransFat"
                    currentParam.Value = scaleNutrifact.CaloriesFromTransFat
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Om6Fatty"
                    currentParam.Value = scaleNutrifact.Om6Fatty
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Om3Fatty"
                    currentParam.Value = scaleNutrifact.Om3Fatty
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Starch"
                    currentParam.Value = scaleNutrifact.Starch
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Chloride"
                    currentParam.Value = scaleNutrifact.Chloride
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Chromium"
                    currentParam.Value = scaleNutrifact.Chromium
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "VitaminK"
                    currentParam.Value = scaleNutrifact.VitaminK
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Manganese"
                    currentParam.Value = scaleNutrifact.Manganese
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Molybdenum"
                    currentParam.Value = scaleNutrifact.Molybdenum
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Selenium"
                    currentParam.Value = scaleNutrifact.Selenium
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "ServingSizeDesc"
                    currentParam.Value = scaleNutrifact.ServingSizeDesc
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)


                    ' Execute the stored procedure 
                    factory.ExecuteStoredProcedure("Scale_InsertUpdateNutrifact", paramList)
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return isSuccess
        End Function

        Public Shared Function InsertOrUpdateItemNutrifact(ByVal itemKey As Integer, ByVal nutrifactId As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            currentParam = New DBParam
            currentParam.Name = "ItemKey"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "NutrifactId"
            currentParam.Value = nutrifactId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("InsertOrUpdateItemNutrifact", paramList)
        End Function

    Public Shared Function InsertOrUpdateItemExtraText(ByVal itemKey As Integer, ByVal extraTextId As Integer, ByVal scaleLabelTypeId As Integer, ByVal extraText As String) As Boolean
      Dim factory As New DataFactory(DataFactory.ItemCatalog)
      Dim paramList As New ArrayList(New DBParam() {
        New DBParam("ExtraTextId", DBParamType.Int, extraTextId),
        New DBParam("ItemKey", DBParamType.Int, itemKey),
        New DBParam("Description", DBParamType.String, String.Empty),
        New DBParam("Scale_LabelType_ID", DBParamType.Int, scaleLabelTypeId),
        New DBParam("ExtraText", DBParamType.String, extraText)})

      factory.ExecuteStoredProcedure("InsertOrUpdateItemExtraText", paramList)
    End Function

    Public Shared Function InsertOrUpdateItemIngredient(ByVal itemKey As Integer, ByVal scaleIngredientId As Integer, ByVal ingredients As String) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            currentParam = New DBParam
            currentParam.Name = "Scale_Ingredient_Id"
            currentParam.Value = scaleIngredientId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemKey"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Description"
            currentParam.Value = String.Empty
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Ingredients"
            currentParam.Value = ingredients
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("InsertOrUpdateItemIngredient", paramList)
        End Function
        Public Shared Function InsertOrUpdateItemAllergen(ByVal itemKey As Integer, ByVal scaleAllergenId As Integer, ByVal allergens As String) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            currentParam = New DBParam
            currentParam.Name = "Scale_Allergen_Id"
            currentParam.Value = scaleAllergenId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemKey"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Description"
            currentParam.Value = String.Empty
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Allergens"
            currentParam.Value = allergens
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("InsertOrUpdateItemAllergen", paramList)
        End Function

        Public Shared Function DeleteItemNutrifact(ByVal itemKey As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            currentParam = New DBParam
            currentParam.Name = "ItemKey"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("DeleteItemNutrifact", paramList)
        End Function

        Public Shared Function DeleteItemExtraText(ByVal itemKey As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            currentParam = New DBParam
            currentParam.Name = "ItemKey"
            currentParam.Value = itemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("DeleteItemExtraText", paramList)
        End Function

    End Class
End Namespace
