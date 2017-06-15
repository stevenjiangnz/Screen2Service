CREATE PROCEDURE [dbo].[USP_DeleteAlertResultByAlert]
	@TradingDate int,
	@ZoneID int = -1
AS
	Delete AlertResults 
	Where TradingDate = @TradingDate and 
	((ZoneId is null and @ZoneID = -1) or (ZoneId = @ZoneID))
	 
	
RETURN 0
