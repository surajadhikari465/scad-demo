Imports System.Linq
Imports log4net
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class SubTeamDAO

        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Function GetSubTeam(ByVal SubTeam_No As Integer) As DataSet
            logger.Debug("GetSubTeam Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeam_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            logger.Debug("GetSubTeam Exit")

            Return factory.GetStoredProcedureDataSet("SubTeams_GetSubTeam", paramList)
        End Function

        Public Shared Function GetSubteams() As DataTable
            logger.Debug("GetSubTeams Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim paramList As New ArrayList

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("SubTeams_LoadSubTeams", paramList)
            Catch ex As Exception
                Throw ex
            End Try

            logger.Debug("GetSubTeams Exit")
            Return results
        End Function

        Public Function GetSubteamsSet() As DataSet

            logger.Debug("GetSubteams Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            logger.Debug("GetSubteams Exit")

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("SubTeams_LoadSubTeams")
        End Function

        Public Sub SaveSubTeam(ByVal SubTeam As SubTeamBO, ByVal IsNew As Boolean)

            logger.Debug("SaveSubTeam Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeam.SubTeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "Team_No"
            currentParam.Value = SubTeam.TeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_Name"
            currentParam.Value = IIf(SubTeam.SubTeamName.Equals(""), System.DBNull.Value, SubTeam.SubTeamName)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_Abbreviation"
            currentParam.Value = IIf(SubTeam.SubTeamAbbreviation.Equals(""), System.DBNull.Value, SubTeam.SubTeamAbbreviation)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Dept_No"
            currentParam.Value = IIf(SubTeam.DepartmentNo.Equals(-1), System.DBNull.Value, SubTeam.DepartmentNo)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubDept_No"
            currentParam.Value = IIf(SubTeam.SubDepartmentNo.Equals(-1), System.DBNull.Value, SubTeam.SubDepartmentNo)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Buyer_User_Id"
            currentParam.Value = IIf(SubTeam.BuyerUserId.Equals(-1), System.DBNull.Value, SubTeam.BuyerUserId)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Target_Margin"
            currentParam.Value = SubTeam.TargetMargin
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "JDA"
            currentParam.Value = IIf(SubTeam.JDA.Equals(-1), System.DBNull.Value, SubTeam.JDA)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GLPurchaseAcct"
            currentParam.Value = IIf(SubTeam.GLPurchaseAcct.Equals(-1), System.DBNull.Value, SubTeam.GLPurchaseAcct)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GLDistributionAcct"
            currentParam.Value = IIf(SubTeam.GLDistributionAcct.Equals(-1), System.DBNull.Value, SubTeam.GLDistributionAcct)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GLTransferAcct"
            currentParam.Value = IIf(SubTeam.GLTransferAcct.Equals(-1), System.DBNull.Value, SubTeam.GLTransferAcct)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GLSalesAcct"
            currentParam.Value = IIf(SubTeam.GLSalesAcct.Equals(-1), System.DBNull.Value, SubTeam.GLSalesAcct)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GLPackagingAcct"
            currentParam.Value = IIf(SubTeam.GLPackagingAcct.Equals(-1), System.DBNull.Value, SubTeam.GLPackagingAcct)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GLSuppliesAcct"
            currentParam.Value = IIf(SubTeam.GLSuppliesAcct.Equals(-1), System.DBNull.Value, SubTeam.GLSuppliesAcct)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Transfer_To_Markup"
            currentParam.Value = IIf(SubTeam.TransferToMarkup.Equals(-1), System.DBNull.Value, SubTeam.TransferToMarkup)
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EXEWarehouseSent"
            currentParam.Value = IIf(SubTeam.EXEWarehouseSent, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ScaleDept"
            currentParam.Value = IIf(SubTeam.ScaleDepartment.Equals(-1), System.DBNull.Value, SubTeam.ScaleDepartment)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Retail"
            currentParam.Value = IIf(SubTeam.Retail, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InventoryCountByCase"
            currentParam.Value = IIf(SubTeam.InventoryCountByCase, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EXEDistributed"
            currentParam.Value = IIf(SubTeam.EXEDistributed, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeamType_Id"
            currentParam.Value = IIf(SubTeam.SubTeamTypeId.Equals(-1), System.DBNull.Value, SubTeam.SubTeamTypeId)
            currentParam.Type = DBParamType.SmallInt
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Distribution"
            currentParam.Value = IIf(SubTeam.Distribution, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FixedSpoilage"
            currentParam.Value = IIf(SubTeam.FixedSpoilage, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            '  These lines needs to un-commented later when BeerFine field will be added to subteam table
            currentParam = New DBParam
            currentParam.Name = "Beverage"
            currentParam.Value = IIf(SubTeam.Beverage, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AlignedSubTeam"
            currentParam.Value = IIf(SubTeam.AlignedSubTeam, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            If IsNew Then
                factory.ExecuteStoredProcedure("SubTeams_CreateSubTeam", paramList)

            Else
                factory.ExecuteStoredProcedure("SubTeams_SaveSubTeam", paramList)
            End If
            logger.Debug("SaveSubTeam Exit")
        End Sub

        Public Function GetSubTeamTypes() As DataTable

            Dim dt As New DataTable
            Dim dc As DataColumn = Nothing
            Dim dr As DataRow = Nothing

            dc = New DataColumn("ID", System.Type.GetType("System.Int16"))
            dt.Columns.Add(dc)

            dc = New DataColumn("Description", System.Type.GetType("System.String"))
            dt.Columns.Add(dc)

            dr = dt.NewRow()
            With dr
                .Item("ID") = 1
                .Item("Description") = "Retail (unrestricted)"
            End With
            dt.Rows.Add(dr)

            dr = dt.NewRow()
            With dr
                .Item("ID") = 2
                .Item("Description") = "Manufacturing"
            End With
            dt.Rows.Add(dr)

            dr = dt.NewRow()
            With dr
                .Item("ID") = 3
                .Item("Description") = "Retail/Manufacturing"
            End With
            dt.Rows.Add(dr)

            dr = dt.NewRow()
            With dr
                .Item("ID") = 4
                .Item("Description") = "Expense"
            End With
            dt.Rows.Add(dr)

            dr = dt.NewRow()
            With dr
                .Item("ID") = 5
                .Item("Description") = "Packaging"
            End With
            dt.Rows.Add(dr)

            dr = dt.NewRow()
            With dr
                .Item("ID") = 6
                .Item("Description") = "Other Supplies"
            End With
            dt.Rows.Add(dr)

            dr = dt.NewRow()
            With dr
                .Item("ID") = 7
                .Item("Description") = "Front End"
            End With
            dt.Rows.Add(dr)

            Return dt

        End Function

        Public Shared Function IsSubTeamAligned(ByVal SubTeamNo As Integer) As Boolean

            logger.Debug("IsSubTeamAligned Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim result As DataTable


            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                result = factory.GetStoredProcedureDataTable("GetSubTeamBySubTeamNo", paramList)

                If result.Rows.Count > 0 Then
                    If CBool(result.Rows(0).Item("AlignedSubTeam").ToString) Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            End Try


            logger.Debug("GetSubTeamBySubTeamNo Exit")
        End Function

        Public Shared Function GetNonAlignedSubteamNames() As List(Of String)
            logger.Debug("GetNonAlignedSubteamNames Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results = factory.GetStoredProcedureDataTable("GetNonAlignedSubteamNames").AsEnumerable().ToList()
            Dim subteamNames As List(Of String) = New List(Of String)

            For Each dataRow As DataRow In results
                subteamNames.Add(dataRow(0).ToString().Trim())
            Next

            logger.Debug("GetNonAlignedSubteamNames Exit")

            Return subteamNames
        End Function

        Public Shared Function GetAlignedSubteams() As Dictionary(Of Integer, String)
            logger.Debug("GetAlignedSubteams Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results = factory.GetStoredProcedureDataTable("GetAlignedSubteams").AsEnumerable().ToList()
            Dim alignedSubteams As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)

            For Each dataRow As DataRow In results
                alignedSubteams.Add(dataRow("SubteamNumber"), dataRow("SubteamName"))
            Next

            logger.Debug("GetAlignedSubteams Exit")

            Return alignedSubteams
        End Function
    End Class
End Namespace
