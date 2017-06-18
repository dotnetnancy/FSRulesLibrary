CREATE FUNCTION [dbo].[uf_EscapeSearch] (@In nvarchar(max))
RETURNS nvarchar(max)
AS
BEGIN
	DECLARE @Out nvarchar(max);
	set @Out = replace(@In,'[','[[]');
	set @Out = replace(@Out,'_','[_]');
	RETURN @Out;
END

