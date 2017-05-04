Imports System.Linq
Imports log4net

Public Class AdHocReprintNoTagLogic

    Private databaseAccess As NoTagDataAccess
    Private noTagRules As List(Of INoTagRule)
    Private candidatesForExclusion As List(Of ItemKeyIdentifierModel)
    Private subteamNumber As Integer
    Private subteamName As String
    Private storeNumber As Integer

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private _excludedItems As List(Of Integer)
    Public ReadOnly Property ExcludedItems() As List(Of Integer)
        Get
            Return _excludedItems
        End Get
    End Property

    Public Sub New(
                  noTagRules As List(Of INoTagRule),
                  databaseAccess As NoTagDataAccess,
                  candidatesForExclusion As List(Of ItemKeyIdentifierModel),
                  subteamNumber As Integer,
                  subteamName As String,
                  storeNumber As Integer)

        Me.noTagRules = noTagRules
        Me.databaseAccess = databaseAccess
        Me.candidatesForExclusion = candidatesForExclusion
        Me.subteamNumber = subteamNumber
        Me.subteamName = subteamName
        Me.storeNumber = storeNumber
        Me._excludedItems = New List(Of Integer)
    End Sub

    Public Sub ApplyNoTagLogic()
        ' Items with a LabelType of NONE are automatically excluded.  For other rules, an item can only be excluded if it is failed by all rules.

        Dim labelTypeExclusions As List(Of Integer) = databaseAccess.GetLabelTypeExclusions(candidatesForExclusion.Select(Function(i) i.ItemKey).ToList())

        If labelTypeExclusions.Count > 0 Then
            logger.Info(String.Format("The following items (Item_Key) have LabelType=NONE and will be excluded from the ad hoc print request: {0}", String.Join(", ", labelTypeExclusions)))

            _excludedItems.AddRange(labelTypeExclusions)

            Dim excludedLabelTypeIdentifiers As List(Of String) = candidatesForExclusion.Where(Function(i) labelTypeExclusions.Contains(i.ItemKey)).Select(Function(i) i.Identifier).ToList()
            databaseAccess.WriteToNoTagExclusion(excludedLabelTypeIdentifiers, storeNumber)

            candidatesForExclusion = candidatesForExclusion.Where(Function(i) Not labelTypeExclusions.Contains(i.ItemKey)).ToList()

            If candidatesForExclusion.Count = 0 Then
                logger.Info("All items in the current ad hoc print request have LabelType=NONE and have been excluded.  No further no-tage rules will be applied.")
                Exit Sub
            End If
        End If

        logger.Info("Begin executing no-tag rules...")

        Dim historyThreshold As Integer

        For Each rule As INoTagRule In noTagRules
            historyThreshold = databaseAccess.GetSubteamOverride(subteamNumber)

            If historyThreshold > 0 Then
                logger.Info(String.Format("Executing no-tag rule: {0}.  Subteam override detected for {1}.  Looking back {2} days.",
                                          rule.GetType().Name, subteamName, historyThreshold.ToString()))
            Else
                historyThreshold = databaseAccess.GetRuleDefaultThreshold(rule.GetType().Name)
                logger.Info(String.Format("Executing no-tag rule: {0}.  Looking back {1} days.", rule.GetType().Name, historyThreshold.ToString()))
            End If

            rule.ApplyRule(candidatesForExclusion.Select(Function(i) i.ItemKey).ToList(), storeNumber, historyThreshold)

            If rule.ExcludedItems.Count = 0 Then
                logger.Info(String.Format("No items were excluded by rule: {0}.  Skipping all remaining rules.", rule.GetType().Name))
                candidatesForExclusion.Clear()
                Exit For
            Else
                candidatesForExclusion = candidatesForExclusion.Where(Function(i) rule.ExcludedItems.Contains(i.ItemKey)).ToList()
                logger.Info(String.Format("Rule {0} found the following possible candidates for exclusion: {1}",
                                          rule.GetType().Name, String.Join(", ", candidatesForExclusion.Select(Function(i) i.Identifier))))
            End If
        Next

        If candidatesForExclusion.Count > 0 Then
            logger.Info(String.Format("The no-tag rules excluded the following item keys from the current ad hoc print request: {0}",
                                      String.Join(", ", candidatesForExclusion.Select(Function(i) i.Identifier))))

            databaseAccess.WriteToNoTagExclusion(candidatesForExclusion.Select(Function(i) i.Identifier).ToList(), storeNumber)
            _excludedItems.AddRange(candidatesForExclusion.Select(Function(i) i.ItemKey).ToList())
        End If

        logger.Info("All no-tag rules have executed successfully.")
    End Sub
End Class
