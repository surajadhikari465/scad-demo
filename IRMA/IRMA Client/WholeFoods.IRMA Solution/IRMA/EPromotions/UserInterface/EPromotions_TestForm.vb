Imports WholeFoods.IRMA.EPromotions.BusinessLogic

Public Class Promotions_TestForm

    Private Sub Button_TestItemGroupEditor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TestItemGroupEditor.Click
        Dim item As PromotionOfferBO = New PromotionOfferBO()
        item.PromotionOfferID = 3
        Dim frm As Form_Promotion_AddRequirement = New Form_Promotion_AddRequirement(item, PromotionOfferMemberJoinLogic.Mandatory, PromotionOfferMemberPurpose.Requirement)
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub Button_TestPromotionOfferEditor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_TestPromotionOfferEditor.Click
        Dim frm As Form_PromotionOfferGrid
        frm = New Form_PromotionOfferGrid
        frm.ShowDialog()
        frm.Dispose()
    End Sub
End Class
