USE [InstinctDRC]
GO

DECLARE	@return_value int

EXEC	@return_value = [dbo].[PIndex_ReorganizeRebuild]
		@DB_Name = N'InstinctDRC'

SELECT	'Return Value' = @return_value

GO
