Imports WholeFoods.IRMA.Ordering.DataAccess
Imports log4net
Imports System.Text.RegularExpressions

Namespace WholeFoods.IRMA.Ordering.BusinessLogic
    Public Class OrderValidationBO

        ' ------------------------------------------------------------------------------------------------
        ' Revision History
        ' Date, TFS, Dev, Desc
        ' ------------------------------------------------------------------------------------------------
        ' 8/27/10             TFS 13319                Tom Lux       Fixed PO unlocking issues and saving-while-locked-by-another-user issues.
        ' Added GetPOLockStatus(), Lock(), and Unlock() methods to help support locking functionality and get logic out of form/screen class.
        '
        ' ------------------------------------------------------------------------------------------------

#Region "Public Members"

        Public vdrInfoHashtable As Hashtable = New Hashtable

#End Region

#Region "Private Members"

        ''' <summary>
        ''' Log4Net logger for this class.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Inner Classes, Structures, Enums, etc."

        Public Enum POLockStatus
            CurrentUserLock
            DifferentUserLock
            NoLock
        End Enum

#End Region

#Region "Constructors"
        ''' <summary>
        ''' Blank constructor
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()

        End Sub

#End Region

#Region "Instance Methods"

        Public Function isVendorConfigComplete(ByVal orderId As Integer) As String

            logger.Debug("isVendorConfigComplete Entry for orderid=" & orderId.ToString)

            Dim orderValDao As OrderValidationDAO = New OrderValidationDAO
            Dim message As String = Nothing

            vdrInfoHashtable = orderValDao.GetVendorConfigDataset(orderId)

            If (String.IsNullOrEmpty(CStr(vdrInfoHashtable.Item(OrderValidationDAO.ELECTRONIC_TRANSFER))) Or CStr(vdrInfoHashtable.Item(OrderValidationDAO.ELECTRONIC_TRANSFER)).ToLower.Equals("false")) Then
                'now check fax info as it is NOT a FTPed order
                If (String.IsNullOrEmpty(CStr(vdrInfoHashtable.Item(OrderValidationDAO.FAX))) And vdrInfoHashtable.Item(OrderValidationDAO.FAX_ORDER).Equals("True")) Then
                    'since NO fax number is assigned, return error message
                    Return ("Please contact support, vendor does not have an assigned fax number.")
                ElseIf (String.IsNullOrEmpty(CStr(vdrInfoHashtable.Item(OrderValidationDAO.EMAIL))) And vdrInfoHashtable.Item(OrderValidationDAO.EMAIL_ORDER).Equals("True")) Then
                    'since NO email address is assigned, return error message
                    Return ("Please contact support, vendor does not have an assigned email address.")
                End If
            Else
                If (String.IsNullOrEmpty(CStr(vdrInfoHashtable.Item(OrderValidationDAO.FTP_HOST)))) Then
                    'since NO fax number is assigned, return error message
                    Return ("Please contact support, vendor does not have an assigned FTP server.")
                End If
                If (String.IsNullOrEmpty(CStr(vdrInfoHashtable.Item(OrderValidationDAO.FTP_USER)))) Then
                    'since NO fax number is assigned, return error message
                    Return ("Please contact support, vendor does not have an assigned FTP user account.")
                End If
                If (String.IsNullOrEmpty(CStr(vdrInfoHashtable.Item(OrderValidationDAO.FTP_PASSWORD)))) Then
                    'since NO fax number is assigned, return error message
                    Return ("Please contact support, vendor does not have an assigned FTP account password.")
                End If
            End If

            Return message
            logger.Debug("isVendorConfigComplete Exit for orderid=" & orderId.ToString & message)

        End Function

        Public Function isElectronicTransfer(ByVal orderId As Integer) As Boolean

            logger.Debug("isElectronicTransfer Entry")

            'If the hashtable is empty because the isVendorConfigComplete() was not called earlier, 
            'pull the information from the database to populate it TFS 8316
            If (vdrInfoHashtable.Count() = 0) Then
                Dim orderValDao As OrderValidationDAO = New OrderValidationDAO
                vdrInfoHashtable = orderValDao.GetVendorConfigDataset(orderId)
            End If


            If (String.IsNullOrEmpty(CStr(vdrInfoHashtable.Item(OrderValidationDAO.ELECTRONIC_TRANSFER))) Or CStr(vdrInfoHashtable.Item(OrderValidationDAO.ELECTRONIC_TRANSFER)).ToLower.Equals("false")) Then

                Return False
                logger.Debug("isElectronicTransfer Exit- False")

            Else
                Return True
                logger.Debug("isElectronicTransfer Exit- True")
            End If



        End Function

        'Task 8316 - Add functionality to validate email address entered on OrderSend form
        Public Function isEmailValid(ByVal emailAddress As String) As String
            logger.Debug("isEmailValid Entry")
            Dim message As String = Nothing
            Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
            Dim emailAddressMatch As Match

            If String.IsNullOrEmpty(emailAddress) Then
                Return ("Please enter a valid email address.")
            End If


            emailAddressMatch = Regex.Match(emailAddress, pattern)

            If emailAddressMatch.Success Then
                Return message

            Else
                Return ("Please enter a valid email address.")

            End If

            logger.Debug("isEmailValid Exit")
        End Function

        'Task 8316 - Add functionality to validate fax number entered on OrderSend form
        Public Function isFaxValid(ByVal faxNumber As String) As String
            logger.Debug("isFaxValid Entry")

            Dim pattern As String = "^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$"
            Dim message As String = Nothing

            Dim faxNumberMatch As Match

            If String.IsNullOrEmpty(faxNumber) Then
                Return ("Please enter a valid fax number.")
            End If


            faxNumberMatch = Regex.Match(faxNumber, pattern)

            If faxNumberMatch.Success Then
                Return message

            Else
                Return ("Please enter a valid fax number.")

            End If
            logger.Debug("isFaxValid Exit")
        End Function

#End Region

#Region "Shared Methods"

        ''' <summary>
        ''' Determines if the specified user has a lock on the specified PO.
        ''' </summary>
        ''' <param name="ponum">PO to check for user lock.</param>
        ''' <param name="userId">Integer user ID to compare to user ID locking the PO.</param>
        ''' <returns></returns>
        ''' <remarks>This should be used by UI before updating POs, meaning we should not update a PO for a user unless we know that user owns the lock on that PO.</remarks>
        Public Shared Function GetPOLockStatus(ByVal poNum As Integer, ByVal userId As Integer) As POLockStatus
            Dim lockingUserId As Integer = OrderingDAO.GetUserLockingPO(poNum)
            logger.InfoFormat("PO {0} current user is {1}, locking user is {2}.", poNum, userId, lockingUserId)
            If lockingUserId < 0 Then
                Return POLockStatus.NoLock
            ElseIf userId = lockingUserId Then
                Return POLockStatus.CurrentUserLock
            Else
                Return POLockStatus.DifferentUserLock
            End If
        End Function

        ''' <summary>
        ''' Validates state of PO to ensure a user lock can be set.
        ''' Attempts to set lock for specified PO to the specified user.
        ''' If the userIdInPO arg is provided, we do not try to retrieve the locking user from the PO; we assume the caller has already done this
        ''' and is passing that user in here for validation.
        ''' </summary>
        ''' <param name="poNum">PO to lock.</param>
        ''' <param name="currentUserId">User to acquire PO lock.</param>
        ''' <param name="userIdInPO">Object that contains a user ID value from the PO (such as one retrieved from a query, which is why this params type is 'Object').</param>
        ''' <remarks>Throws an exception if PO is already locked by a different user.</remarks>
        Public Shared Sub Lock(ByVal poNum As Integer, ByVal currentUserId As Integer, Optional ByVal userIdInPO As Object = Nothing)
            ' Validation...
            ' Only lock if no lock exists, meaning if another user has a lock, we cannot override it here.  The process for this is to first be unlocked, but 
            ' we don't know where this 'lock' call is coming from so do not override.
            Dim lockUserId As Integer
            ' If caller did not pass this arg, we assume we need to go get the locking user so we can validate the lock request for the current user.
            If userIdInPO Is Nothing Then
                logger.InfoFormat("Retrieving lock owner for PO '{0}'...", poNum)
                lockUserId = OrderingDAO.GetUserLockingPO(poNum)
            Else
                ' If user ID passed in for locking user is NULL, that means there is no lock on the PO, so we set locking ID to negative value (valid user IDs are >= 0).
                If IsDBNull(userIdInPO) Then
                    lockUserId = -1
                Else
                    lockUserId = CInt(userIdInPO)
                End If
            End If
            ' Negative user ID means no lock on PO, so attempt lock.
            If lockUserId < 0 Then
                logger.InfoFormat("PO '{0}' not locked, so locking for user '{1}'...", poNum, currentUserId)
                OrderingDAO.LockPO(poNum, currentUserId)
            ElseIf lockUserId = currentUserId Then
                ' Why are we trying to lock this PO again?
                logger.WarnFormat("No lock-PO action taken because user '{0}' already has PO '{1}' locked.", currentUserId, poNum)
                Exit Sub
            Else
                ' This PO is locked by another user and we need to stop whatever process is trying to acquire the lock, so we throw exception.
                Dim msg = String.Format("User '{0}' already has PO '{1}' locked, so current user '{2}' cannot acquire lock.", lockUserId, poNum, currentUserId)
                logger.Error(msg)
                Throw New Exception(msg)
            End If
        End Sub

        ''' <summary>
        ''' Validates state of PO to ensure a user lock can be removed.
        ''' Attempts to remove lock for specified PO for the specified user.
        ''' </summary>
        ''' <param name="poNum">PO to unlock.</param>
        ''' <param name="userId">User holding lock on PO.</param>
        ''' <remarks></remarks>
        Public Shared Sub Unlock(ByVal poNum As Integer, ByVal userId As Integer)
            ' Validation...
            ' Make sure the current user owns the lock.  If not, we do not unlock.

            Dim lockUserId As Integer = OrderingDAO.GetUserLockingPO(poNum)
            logger.InfoFormat("Unlocking PO if current user '{0}' is the locking user '{1}'.", userId, lockUserId)
            If lockUserId = userId Then
                ' ## Bug 9276  if not locked by another user, make sure its not locked by you before you leave.
                OrderingDAO.UnlockPO(poNum)
            ElseIf lockUserId >= 0 Then
                logger.WarnFormat("PO '{0}' not unlocked because it is locked by a different user.", poNum)
            Else
                logger.InfoFormat("PO '{0}' not locked.", poNum)
            End If
        End Sub

#End Region

    End Class
End Namespace
