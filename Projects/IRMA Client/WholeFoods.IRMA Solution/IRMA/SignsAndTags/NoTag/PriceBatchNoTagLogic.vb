Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports System.Linq
Imports log4net

Public Class PriceBatchNoTagLogic

    Private databaseAccess As NoTagDataAccess
    Private noTagRules As List(Of INoTagRule)
    Private candidatesForExclusion As List(Of Integer)
    Private batchHeader As PriceBatchHeaderBO

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _itemsExcluded As Boolean
    Public ReadOnly Property ItemsExcluded() As Boolean
        Get
            Return _itemsExcluded
        End Get
    End Property

    Private _excludedItems As List(Of Integer)
    Public ReadOnly Property ExcludedItems() As List(Of Integer)
        Get
            Return _excludedItems
        End Get
    End Property

    Public Sub New(noTagRules As List(Of INoTagRule), databaseAccess As NoTagDataAccess, candidatesForExclusion As Integer(), batchHeader As PriceBatchHeaderBO)
        Me.noTagRules = noTagRules
        Me.databaseAccess = databaseAccess
        Me.candidatesForExclusion = candidatesForExclusion.ToList()
        Me.batchHeader = batchHeader
        Me._excludedItems = New List(Of Integer)
    End Sub

    Public Sub ApplyNoTagLogic(Optional ByVal byPassNoTagRules As Boolean = False)
        ' Items with a LabelType of NONE are automatically excluded.  For other rules, an item can only be excluded if it is failed by all rules.

        Dim labelTypeExclusions As List(Of Integer) = databaseAccess.GetLabelTypeExclusions(candidatesForExclusion)

        If labelTypeExclusions.Count > 0 Then
            logger.Info(String.Format("The following items (Item_Key) have LabelType=NONE and will be excluded from the print request in batch {0}: {1}",
                                                batchHeader.PriceBatchHeaderId, String.Join(", ", labelTypeExclusions)))
            _itemsExcluded = True
            _excludedItems.AddRange(labelTypeExclusions)
            candidatesForExclusion = candidatesForExclusion.Except(labelTypeExclusions).ToList()

            databaseAccess.WriteToNoTagExclusion(labelTypeExclusions, batchHeader.PriceBatchHeaderId, batchHeader.StoreNumber)

            If candidatesForExclusion.Count = 0 Then
                logger.Info(String.Format("All items in batch {0} have LabelType=NONE and have been excluded from the print request.  No additional no-tag rules will be applied.",
                                          batchHeader.PriceBatchHeaderId))
                Exit Sub
            End If
        End If

        Dim offSaleExclusions As List(Of Integer) = databaseAccess.GetOffSaleExclusions(candidatesForExclusion, batchHeader.PriceBatchHeaderId)

        If offSaleExclusions.Count > 0 Then
            logger.Info(String.Format("The following items (Item_Key) in batch {0} are off-sale regular prices but have no price change.  They will be excluded from the print request: {1}",
                                                batchHeader.PriceBatchHeaderId, String.Join(", ", offSaleExclusions)))
            _itemsExcluded = True
            _excludedItems.AddRange(offSaleExclusions)
            candidatesForExclusion = candidatesForExclusion.Except(offSaleExclusions).ToList()

            databaseAccess.WriteToNoTagExclusion(offSaleExclusions, batchHeader.PriceBatchHeaderId, batchHeader.StoreNumber)

            If candidatesForExclusion.Count = 0 Then
                logger.Info(String.Format("All items in batch {0} have been excluded from the print request.  No additional no-tag rules will be applied.",
                                          batchHeader.PriceBatchHeaderId))
                Exit Sub
            End If
        End If

        If batchHeader.ItemChgTypeID = 1 Then
            logger.Info(String.Format("Batch {0} is type NEW.  No-tag logic will not be applied.", batchHeader.PriceBatchHeaderId))
            Exit Sub
        ElseIf batchHeader.ItemChgTypeID = 3 Then
            logger.Info(String.Format("Batch {0} is type DEL.  No-tag logic will not be applied.", batchHeader.PriceBatchHeaderId))
            Exit Sub
        ElseIf byPassNoTagRules = False Then
            logger.Info("Begin executing no-tag rules...")

            Dim historyThreshold As Integer

            For Each rule As INoTagRule In noTagRules
                historyThreshold = databaseAccess.GetSubteamOverride(batchHeader.SubteamNumber)

                If historyThreshold > 0 Then
                    logger.Info(String.Format("Executing no-tag rule: {0}.  Subteam override detected for {1}.  Looking back {2} days.",
                                              rule.GetType().Name, batchHeader.SubteamName, historyThreshold.ToString()))
                Else
                    historyThreshold = databaseAccess.GetRuleDefaultThreshold(rule.GetType().Name)
                    logger.Info(String.Format("Executing no-tag rule: {0}.  Looking back {1} days.", rule.GetType().Name, historyThreshold.ToString()))
                End If

                rule.ApplyRule(candidatesForExclusion, batchHeader.StoreNumber, historyThreshold)

                If rule.ExcludedItems.Count = 0 Then
                    logger.Info(String.Format("No items were excluded by rule: {0}.  Skipping all remaining rules.", rule.GetType().Name))
                    candidatesForExclusion.Clear()
                    Exit For
                Else
                    candidatesForExclusion = candidatesForExclusion.Where(Function(i) rule.ExcludedItems.Contains(i)).ToList()
                    logger.Info(String.Format("Rule {0} found the following possible candidates for exclusion: {1}", rule.GetType().Name, String.Join(", ", candidatesForExclusion)))
                End If
            Next

            If candidatesForExclusion.Count > 0 Then
                logger.Info(String.Format("The no-tag rules excluded the following item keys from batch {0}: {1}",
                                                batchHeader.PriceBatchHeaderId, String.Join(", ", candidatesForExclusion)))

                databaseAccess.WriteToNoTagExclusion(candidatesForExclusion, batchHeader.PriceBatchHeaderId, batchHeader.StoreNumber)
                _excludedItems.AddRange(candidatesForExclusion)
                _itemsExcluded = True
            End If
        Else
            logger.Info("Bypassing no-tag rules based on provided checkbox setting")
        End If

        logger.Info("All no-tag rules have executed successfully.")
    End Sub
End Class
