Create Function [WFM\anjana.maganti].fn_CSVToTable (@CSVList Varchar(3000))
Returns @Table Table (ColumnData Varchar(50))
As
Begin
If right(@CSVList, 1) <> ','
Select @CSVList = @CSVList + ','

Declare	@Pos	Smallint,
@OldPos	Smallint
Select	@Pos	= 1,
@OldPos = 1

While	@Pos < Len(@CSVList)
Begin
Select	@Pos = CharIndex(',', @CSVList, @OldPos)
Insert into @Table
Select	LTrim(RTrim(SubString(@CSVList, @OldPos, @Pos - @OldPos))) Col001
Select	@OldPos = @Pos + 1
End

Return
End