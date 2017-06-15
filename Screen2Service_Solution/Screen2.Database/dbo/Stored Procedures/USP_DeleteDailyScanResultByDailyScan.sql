CREATE PROCEDURE [dbo].[USP_DeleteDailyScanResultByDailyScan]
	@DailyScanId int,
	@TradingDate int
AS
	DELETE DailyScanResults
	Where DailyScanID = @DailyScanId and
	TradingDate = @TradingDate
RETURN 0
