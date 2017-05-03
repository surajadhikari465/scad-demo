Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Namespace Encryption
    ''' <summary>
    ''' Class used to provide encryption functions to applications.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Encryptor
#Region "Property definitons"
        ''' <summary>
        ''' Phrase used to generate the secret key.
        ''' </summary>
        ''' <remarks></remarks>
        Private _phrase As String = "Default Phrase"
        ''' <summary>
        ''' Internal key value
        ''' </summary>
        ''' <remarks></remarks>
        Private _key(23) As Byte
        ''' <summary>
        ''' Internal initialization vector to encrypt/decrypt the first block
        ''' </summary>
        ''' <remarks></remarks>
        Private _IV(15) As Byte
#End Region

#Region "Constructors"
        ''' <summary>
        ''' Constructor - uses the default secret phrase.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="secretPhrase">Phrase to generate the key.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal secretPhrase As String)
            Me.Phrase = secretPhrase
        End Sub
#End Region

#Region "Public Methods"
        ''' <summary>
        ''' Encrypt the value using the Rijndael algorithm.
        ''' </summary>
        ''' <param name="encryptValue">Value to encrypt.</param>
        ''' <returns>Encrypted value.</returns>
        ''' <remarks></remarks>
        Public Function Encrypt(ByVal encryptValue As String) As String
            Dim encryptedString As String = ""
            Dim rijndael As New RijndaelManaged()
            Dim rijndaelEncrypt As ICryptoTransform = Nothing
            Dim encryptStream As CryptoStream = Nothing
            Dim memStream As New MemoryStream()

            Try
                If encryptValue.Length > 0 Then
                    ' Create the crypto objects
                    rijndael.Key = _key
                    rijndael.IV = _IV
                    rijndaelEncrypt = rijndael.CreateEncryptor()
                    encryptStream = New CryptoStream(memStream, rijndaelEncrypt, CryptoStreamMode.Write)

                    ' Write the encrytped value to memory
                    Dim input As Byte() = Encoding.UTF8.GetBytes(encryptValue)
                    encryptStream.Write(input, 0, input.Length)
                    encryptStream.FlushFinalBlock()

                    ' Retrieve the encrypted value to return
                    encryptedString = Convert.ToBase64String(memStream.ToArray())
                End If
            Finally
                If rijndael IsNot Nothing Then
                    rijndael.Clear()
                End If
                If rijndaelEncrypt IsNot Nothing Then
                    rijndaelEncrypt.Dispose()
                End If
                If memStream IsNot Nothing Then
                    memStream.Close()
                End If
            End Try

            Return encryptedString
        End Function

        ''' <summary>
        ''' Decrypt the value using the Rijndael algorithm.
        ''' </summary>
        ''' <param name="encryptedValue">Value to decrypt.</param>
        ''' <returns>Decrypted value.</returns>
        ''' <remarks></remarks>
        Public Function Decrypt(ByVal encryptedValue As String) As String
            Dim decryptedString As String = ""
            Dim rijndael As New RijndaelManaged()
            Dim rijndaelDecrypt As ICryptoTransform = Nothing
            Dim decryptStream As CryptoStream = Nothing
            Dim memStream As New MemoryStream()

            Try
                If encryptedValue.Length > 0 Then
                    ' Create the crypto objects
                    rijndaelDecrypt = rijndael.CreateDecryptor(_key, _IV)
                    Dim input As Byte() = Convert.FromBase64String(encryptedValue)
                    decryptStream = New CryptoStream(memStream, rijndaelDecrypt, CryptoStreamMode.Write)

                    ' Write the decrytped value to memory
                    decryptStream.Write(input, 0, input.Length)
                    decryptStream.FlushFinalBlock()
                    memStream.Position = 0
                    Dim result(CType(memStream.Length - 1, System.Int32)) As Byte
                    memStream.Read(result, 0, CType(result.Length, System.Int32))

                    ' Retrieve the decrypted value to return
                    Dim utf8 As New UTF8Encoding()
                    decryptedString = utf8.GetString(result)
                End If
            Finally
                If rijndael IsNot Nothing Then
                    rijndael.Clear()
                End If
                If rijndaelDecrypt IsNot Nothing Then
                    rijndaelDecrypt.Dispose()
                End If
                If memStream IsNot Nothing Then
                    memStream.Close()
                End If
            End Try

            Return decryptedString
        End Function
#End Region

#Region "Private Methods"
        ''' <summary>
        ''' Generate an encryption key for the given phrase.
        ''' The phrase is hashed to create a unique 32 character (256-bit) value, of which
        ''' 24 characters (192 bit) are used for the key and the remaining 8 are used for 
        ''' the initialization vector (IV).
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GenerateKey()
            ' Initialize the internal values
            _key(23) = New Byte
            _IV(15) = New Byte

            ' Perform a hash operation using the phrase.
            Dim bytePhrase() As Byte = Encoding.ASCII.GetBytes(_phrase)
            Dim sha384 As New SHA384Managed()
            sha384.ComputeHash(bytePhrase)
            Dim result() As Byte = sha384.Hash()

            ' Transfer the first 24 characters of the hashed value to the key
            ' and the remaining 8 characters to the initialization vector.
            For index As Integer = 0 To 23
                _key(index) = result(index)
            Next
            For index As Integer = 24 To 39
                _IV(index - 24) = result(index)
            Next
        End Sub
#End Region

#Region "Property access methods"
        Public Property Phrase() As String
            Get
                Return _phrase
            End Get
            Set(ByVal value As String)
                _phrase = value
                ' generate the key for the phrase
                GenerateKey()
            End Set
        End Property
#End Region
    End Class
End Namespace

