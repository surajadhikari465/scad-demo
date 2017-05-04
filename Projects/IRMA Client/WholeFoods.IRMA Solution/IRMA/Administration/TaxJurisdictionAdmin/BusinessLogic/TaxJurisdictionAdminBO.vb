Public Class TaxJurisdictionAdminBO
    Inherits WholeFoods.IRMA.TaxHosting.BusinessLogic.TaxJurisdictionBO

#Region "Properties"
    Public Property OldTaxJurisdicitonID As Int16
    Public Property LastUpdateUserID As Int16
    Public Property LastUpdate As DateTime
#End Region

#Region "Subs and Functions"

    ''' <summary>
    ''' Calls TaxJurisdictionAdminDAO function for inserting a clone Tax Jurisdiction
    ''' </summary>
    ''' <remarks></remarks>
    Public Function InsertClone() As TaxJurisdictionAdminBO
        Dim taxJurDAO As New TaxJurisdictionAdminDAO

        Try
            Return taxJurDAO.InsertCloneTaxJurisdiction(Me)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Get list of jurisdictions for admin grid
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetList() As DataTable
        Dim taxJurDAO As New TaxJurisdictionAdminDAO

        Try
            Return taxJurDAO.GetJurisdictions.Tables(0)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Calls TaxJurisdictionAdminDAO function for deleting a Tax Jurisdiction
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Remove()
        Dim taxJurDAO As New TaxJurisdictionAdminDAO

        Try
            taxJurDAO.DeleteJurisdiction(Me)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Calls TaxJurisdictionAdminDAO function for updating a Tax Jurisdiction
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Update()
        Dim taxJurDAO As New TaxJurisdictionAdminDAO

        Try
            taxJurDAO.UpdateJurisdiction(Me)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub


#End Region
End Class
