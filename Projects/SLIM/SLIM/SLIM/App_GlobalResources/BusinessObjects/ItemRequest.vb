Imports Microsoft.VisualBasic

Public Class ItemRequest

    Public Function InsertNewItemRequest(ByVal dr As NewItemRequest.ItemRequestRow) As Integer

        Dim da As New NewItemRequestTableAdapters.ItemRequestTableAdapter
        Dim ItemRequest_ID As Integer
        Try
            'ItemRequest_ID = da.InsertItemRequest(dr.Identifier, dr.ItemStatus_ID, dr.ItemType_ID, dr.ItemTemplate, _
            'dr.User_ID, dr.User_Store, dr.UserAccessLevel_ID, dr.VendorRequest_ID, dr.Item_Description, _
            'dr.POS_Description, dr.ItemUnit, dr.ItemSize, dr.PackSize, dr.VendorNumber, dr.SubTeam_No, _
            'dr.Price, dr.PriceMultiple, dr.CaseCost, dr.CaseSize, dr.Warehouse, dr.Brand_ID, _
            'dr.BrandName, dr.Category_ID, dr.Insert_Date, dr.ClassID, dr.TaxClass_ID, dr.CRV, _
            'dr.AgeCode, dr.FoodStamp, dr.Ready_To_Apply)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Error_Log.throwException(ex.Message, ex)
        End Try
        Return ItemRequest_ID
    End Function

End Class
