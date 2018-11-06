Imports System.Data.SqlClient


Namespace WholeFoods.Utility.DataAccess

    'data types to be used in DataAccess objects
    Public Enum DBParamType
        Binary
        Bit
        [Char]
        DateTime
        [Decimal]
        Float
        Image
        Int
        Money
        SmallInt
        [String]
        Timestamp
        UnicodeString
        Xml
    End Enum

    Public Class DBParam

        Private _name As String
        Private _value As Object
        Private _type As DBParamType

        Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Property Value() As Object
            Get
                Return _value
            End Get
            Set(ByVal value As Object)
                _value = value
            End Set
        End Property

        Property Type() As DBParamType
            Get
                Return _type
            End Get
            Set(ByVal value As DBParamType)
                _type = value
            End Set
        End Property

        ReadOnly Property SqlDbType() As SqlDbType
            Get
                Return GetParamType(SupportedDatabaseType.SqlServer, Me.Type)
            End Get
        End Property

        Private Function GetParamType(ByVal databaseType As SupportedDatabaseType, ByVal paramType As DBParamType) As SqlDbType
            Dim returnType As SqlDbType

            'determine database type
            Select Case databaseType
                Case SupportedDatabaseType.SqlServer
                    'match DBParamType to SqlServer specific database parameter
                    Select Case paramType
                        Case DBParamType.Binary
                            returnType = SqlDbType.Binary
                        Case DBParamType.Bit
                            returnType = SqlDbType.Bit
                        Case DBParamType.Char
                            returnType = SqlDbType.Char
                        Case DBParamType.DateTime
                            returnType = SqlDbType.DateTime
                        Case DBParamType.Decimal
                            returnType = SqlDbType.Decimal
                        Case DBParamType.Float
                            returnType = SqlDbType.Float
                        Case DBParamType.Image
                            returnType = SqlDbType.Image
                        Case DBParamType.Int
                            returnType = SqlDbType.Int
                        Case DBParamType.Money
                            returnType = SqlDbType.Money
                        Case DBParamType.SmallInt
                            returnType = SqlDbType.SmallInt
                        Case DBParamType.String
                            returnType = SqlDbType.VarChar
                        Case DBParamType.Timestamp
                            returnType = SqlDbType.Timestamp
                        Case DBParamType.UnicodeString
                            returnType = SqlDbType.NVarChar
                        Case DBParamType.Xml
                            returnType = SqlDbType.Xml
                    End Select
            End Select

            Return returnType
        End Function

    End Class

End Namespace
