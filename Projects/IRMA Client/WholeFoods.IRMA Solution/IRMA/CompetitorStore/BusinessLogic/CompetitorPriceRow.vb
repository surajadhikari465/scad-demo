Namespace WholeFoods.IRMA.CompetitorStore.BusinessLogic
    Partial Public Class CompetitorStoreDataSet
        Partial Public Class CompetitorPriceRow
            Implements ICompetitorPriceRow

#Region "Explicit Implementation of ICompetitorPriceRow"

            Private Property FiscalWeekRowParent1() As FiscalWeekRow Implements ICompetitorPriceRow.FiscalWeekRowParent
                Get
                    Return Me.FiscalWeekRowParent
                End Get
                Set(ByVal value As FiscalWeekRow)
                    Me.FiscalWeekRowParent = value
                End Set
            End Property

            Public ReadOnly Property IsExistingRow() As Boolean Implements ICompetitorPriceRow.IsExistingRow
                Get
                    Return Not (Me.RowState = DataRowState.Added OrElse Me.RowState = DataRowState.Detached)
                End Get
            End Property

            Private ReadOnly Property RowState1() As System.Data.DataRowState Implements ICompetitorPriceRow.RowState
                Get
                    Return Me.RowState
                End Get
            End Property

            Private Function IsCompetitorIDNull1() As Boolean Implements ICompetitorPriceRow.IsCompetitorIDNull
                Return Me.IsCompetitorIDNull()
            End Function

            Private Function IsCompetitorLocationIDNull1() As Boolean Implements ICompetitorPriceRow.IsCompetitorLocationIDNull
                Return Me.IsCompetitorLocationIDNull()
            End Function

            Private Function IsCompetitorStoreIDNull1() As Boolean Implements ICompetitorPriceRow.IsCompetitorStoreIDNull
                Return Me.IsCompetitorStoreIDNull()
            End Function

            Public Function IsCompetitorLocationNull1() As Boolean Implements ICompetitorPriceRow.IsCompetitorLocationNull
                Return Me.IsLocationNull()
            End Function

            Public Function IsCompetitorNull1() As Boolean Implements ICompetitorPriceRow.IsCompetitorNull
                Return Me.IsCompetitorNull()
            End Function

            Public Function IsCompetitorStoreNull1() As Boolean Implements ICompetitorPriceRow.IsCompetitorStoreNull
                Return Me.IsCompetitorStoreNull()
            End Function

            Private Function IsFiscalPeriodNull1() As Boolean Implements ICompetitorPriceRow.IsFiscalPeriodNull
                Return Me.IsFiscalPeriodNull()
            End Function

            Private Function IsFiscalYearNull1() As Boolean Implements ICompetitorPriceRow.IsFiscalYearNull
                Return Me.IsFiscalYearNull()
            End Function

            Private Function IsItem_KeyNull1() As Boolean Implements ICompetitorPriceRow.IsItem_KeyNull
                Return Me.IsItem_KeyNull()
            End Function

            Private Function IsPeriodWeekNull1() As Boolean Implements ICompetitorPriceRow.IsPeriodWeekNull
                Return Me.IsPeriodWeekNull()
            End Function

            Public Function IsPriceMultipleNull1() As Boolean Implements ICompetitorPriceRow.IsPriceMultipleNull
                Return Me.IsPriceMultipleNull()
            End Function

            Public Function IsPriceNull1() As Boolean Implements ICompetitorPriceRow.IsPriceNull
                Return Me.IsPriceNull()
            End Function

            Private Function IsSizeNull1() As Boolean Implements ICompetitorPriceRow.IsSizeNull
                Return Me.IsSizeNull()
            End Function

            Private Sub SetColumnError1(ByVal columnName As String, ByVal errorText As String) Implements ICompetitorPriceRow.SetColumnError
                Me.SetColumnError(columnName, errorText)
            End Sub

#End Region

        End Class
    End Class
End Namespace