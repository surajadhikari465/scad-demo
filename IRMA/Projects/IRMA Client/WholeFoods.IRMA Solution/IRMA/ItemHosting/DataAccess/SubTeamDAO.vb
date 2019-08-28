Imports System.Linq
Imports log4net
Imports WholeFoods.Utility.DataAccess
Imports System.Reflection

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
	Public Class SubTeamDAO
		Private Shared logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

		Public Function GetSubTeam(ByVal SubTeam_No As Integer) As DataSet
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList From {New DBParam("SubTeam_No", DBParamType.Int, SubTeam_No)}

			Return factory.GetStoredProcedureDataSet("SubTeams_GetSubTeam", paramList)
		End Function

		Public Shared Function GetSubteams() As List(Of SubTeamBO)
			Return GetSubTeamsBOs("SubTeams_GetAllSubteams", Nothing)
		End Function

		Public Sub SaveSubTeam(ByVal SubTeam As SubTeamBO, ByVal IsNew As Boolean)
			logger.Debug("SaveSubTeam Entry")

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList From
			{
				New DBParam("SubTeam_No", DBParamType.Int, SubTeam.SubTeamNo),
				New DBParam("Team_No", DBParamType.Int, SubTeam.TeamNo),
				New DBParam("SubTeam_Name", DBParamType.String, If(String.IsNullOrWhiteSpace(SubTeam.SubTeamName), DBNull.Value, SubTeam.SubTeamName)),
				New DBParam("SubTeam_Abbreviation", DBParamType.String, If(String.IsNullOrWhiteSpace(SubTeam.SubTeamAbbreviation), DBNull.Value, SubTeam.SubTeamAbbreviation)),
				New DBParam("Dept_No", DBParamType.Int, If(SubTeam.DeptNo Is Nothing, DBNull.Value, SubTeam.DeptNo)),
				New DBParam("SubDept_No", DBParamType.Int, If(SubTeam.SubDeptNo Is Nothing, DBNull.Value, SubTeam.SubDeptNo)),
				New DBParam("Buyer_User_Id", DBParamType.Int, If(SubTeam.BuyerUserId Is Nothing, DBNull.Value, SubTeam.BuyerUserId)),
				New DBParam("Target_Margin", DBParamType.Decimal, SubTeam.TargetMargin),
				New DBParam("JDA", DBParamType.Int, If(SubTeam.JDA Is Nothing, DBNull.Value, SubTeam.JDA)),
				New DBParam("GLPurchaseAcct", DBParamType.Int, If(SubTeam.GLPurchaseAcct Is Nothing, DBNull.Value, SubTeam.GLPurchaseAcct)),
				New DBParam("GLDistributionAcct", DBParamType.Int, If(SubTeam.GLDistributionAcct Is Nothing, DBNull.Value, SubTeam.GLDistributionAcct)),
				New DBParam("GLTransferAcct", DBParamType.Int, If(SubTeam.GLTransferAcct Is Nothing, DBNull.Value, SubTeam.GLTransferAcct)),
				New DBParam("GLSalesAcct", DBParamType.Int, If(SubTeam.GLSalesAcct Is Nothing, DBNull.Value, SubTeam.GLSalesAcct)),
				New DBParam("GLPackagingAcct", DBParamType.Int, If(SubTeam.GLPackagingAcct Is Nothing, DBNull.Value, SubTeam.GLPackagingAcct)),
				New DBParam("GLSuppliesAcct", DBParamType.Int, If(SubTeam.GLSuppliesAcct Is Nothing, DBNull.Value, SubTeam.GLSuppliesAcct)),
				New DBParam("Transfer_To_Markup", DBParamType.Decimal, If(SubTeam.TransferToMarkup Is Nothing, DBNull.Value, SubTeam.TransferToMarkup)),
				New DBParam("EXEWarehouseSent", DBParamType.Bit, If(SubTeam.EXEWarehouseSent, 1, 0)),
				New DBParam("ScaleDept", DBParamType.Int, If(SubTeam.ScaleDept Is Nothing, DBNull.Value, SubTeam.ScaleDept)),
				New DBParam("Retail", DBParamType.Bit, If(SubTeam.Retail, 1, 0)),
				New DBParam("InventoryCountByCase", DBParamType.Bit, If(SubTeam.InventoryCountByCase, 1, 0)),
				New DBParam("EXEDistributed", DBParamType.Bit, If(SubTeam.EXEDistributed, 1, 0)),
				New DBParam("SubTeamType_Id", DBParamType.SmallInt, If(SubTeam.SubTeamTypeId Is Nothing, DBNull.Value, SubTeam.SubTeamTypeId)),
				New DBParam("Distribution", DBParamType.Bit, If(SubTeam.Distribution, 1, 0)),
				New DBParam("FixedSpoilage", DBParamType.Bit, If(SubTeam.FixedSpoilage, 1, 0)),
				New DBParam("Beverage", DBParamType.Bit, If(SubTeam.Beverage, 1, 0)),
				New DBParam("AlignedSubTeam", DBParamType.Bit, If(SubTeam.AlignedSubTeam, 1, 0)),
				New DBParam("IsDisabled", DBParamType.Bit, If(SubTeam.IsDisabled, 1, 0))
			}

			If IsNew Then
				factory.ExecuteStoredProcedure("SubTeams_CreateSubTeam", paramList)
			Else
				factory.ExecuteStoredProcedure("SubTeams_SaveSubTeam", paramList)
			End If
			logger.Debug("SaveSubTeam Exit")
		End Sub

		Public Function GetSubTeamTypes() As DataTable
			Dim dt As New DataTable
			dt.Columns.Add(New DataColumn("ID", System.Type.GetType("System.Int16")))
			dt.Columns.Add(New DataColumn("Description", System.Type.GetType("System.String")))

			dt.Rows.Add(1, "Retail (unrestricted)")
			dt.Rows.Add(2, "Manufacturing")
			dt.Rows.Add(3, "Retail/Manufacturing")
			dt.Rows.Add(4, "Expense")
			dt.Rows.Add(5, "Packaging")
			dt.Rows.Add(6, "Other Supplies")
			dt.Rows.Add(7, "Front End")

			Return dt
		End Function

		Public Shared Function IsSubTeamAligned(ByVal SubTeamNo As Integer) As Boolean
			logger.Debug("IsSubTeamAligned Entry")

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList From {New DBParam("SubTeam_No", DBParamType.Int, SubTeamNo)}

			Try
				Dim result As DataTable = factory.GetStoredProcedureDataTable("GetSubTeamBySubTeamNo", paramList)

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
			Try
				Dim factory As New DataFactory(DataFactory.ItemCatalog)
				Return factory.GetStoredProcedureDataTable("GetNonAlignedSubteamNames") _
					.Rows.Cast(Of DataRow) _
					.Where(Function(x) Not IsDBNull(x(0))) _
					.Select(Function(x) x(0).ToString().Trim()) _
					.ToList()
			Catch ex As Exception
				Throw New Exception($"SubTeamDAO.GetNonAlignedSubteamNames: {ex.Message}", ex.InnerException)
			End Try
		End Function

		Public Shared Function GetAlignedSubteams() As List(Of SubTeamBO)
			Try
				Dim factory As New DataFactory(DataFactory.ItemCatalog)
				Return factory.GetStoredProcedureDataTable("GetAlignedSubteams") _
					.Rows.Cast(Of DataRow) _
					.Select(Function(x) New SubTeamBO() With {.SubTeamNo = CInt(x!SubteamNumber), .SubTeamName = x!SubteamName.ToString()}) _
					.ToList()
			Catch ex As Exception
				Throw New Exception($"SubTeamDAO.GetAlignedSubteams: {ex.Message}", ex.InnerException)
			End Try
		End Function

		'TODO: switch all calls to get SubTeams here. Ref: Global.LoadSubTeamByType(); Global.LoadAllSubTeams()
		Shared Function GetSubTeamsBOs(ByVal spName As String, ByVal dbParams As DBParamList) As List(Of SubTeamBO)
			Try
				Dim table As DataTable = New DataFactory(DataFactory.ItemCatalog).GetStoredProcedureDataTable(spName, If(dbParams Is Nothing, New DBParamList(), dbParams))
				Dim names As Dictionary(Of String, Integer) = table.Columns.Cast(Of DataColumn) _
					.ToDictionary(Function(x) x.ColumnName, Function(x) x.Ordinal, StringComparer.InvariantCultureIgnoreCase)

				'Create annonymous type to hold PropertyInfo, PropertyType and Index in DataRow to avoid repeated Type creation when initilizing property in the loop.
				Dim pi = GetType(SubTeamBO).GetProperties().Where(Function(x) names.ContainsKey(x.Name)) _
				.Select(Function(x) New With
					{
						.PropertyInf = x,   'PropertyInfo
						.Index = names(x.Name), 'Index: DataRow(Index) to extract data from
						.PropertyType = If(Nullable.GetUnderlyingType(x.PropertyType), x.PropertyType) 'Property type to correctly Convert data
					}) _
				.ToArray()

				Return table.Rows.Cast(Of DataRow) _
					.Select(Function(x)
								Dim subTeam As New SubTeamBO()
								For Each p As Object In pi
									If Not IsDBNull(x(p.Index)) Then
										p.PropertyInf.SetValue(subTeam, Convert.ChangeType(x(p.Index), p.PropertyType), Nothing)
									End If
								Next
								Return subTeam
							End Function).OrderBy(Function(x) x.SubTeamName).ToList()
			Catch ex As Exception
				Throw New Exception($"SubTeam.DAO.GetSubteams(): {ex.Message}", ex.InnerException)
			End Try
		End Function
	End Class
End Namespace