create function [dbo].[fnGetUnknownAppID]()
returns int
as
begin
/*
Author: Tom Lux
Date: Sept, 2022

When this was written, it was checked into Azure repo here:
https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/dbo/Functions/fnGetUnknownAppID.sql&version=GBmaster

Main tech doc(s) here: https://dev.azure.com/wholefoods/Supply%20Chain%20Application%20Development/_git/SCAD?path=/OutOfStock/DEV/OOSDatabase/_documentation/
*/

	return (select coalesce(min(appid), 1) from app where appname like '%unknown%')
end;
go

