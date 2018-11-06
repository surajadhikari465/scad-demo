Public Class Response

    Public Enum ServiceResponse
        Success = 0
        FTPError = 1
        ADOError = 2
        FileBuilderError = 3
        MailError = 4
        ShareAccessError = 5
        ValidationError = 6
        FileNotFound = 7
        RecipientError = 8
        NotSupported = 9
        UnknownError = 100
    End Enum

End Class
