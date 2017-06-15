CREATE PROCEDURE [dbo].[USP_GetLatestTradingDateByShare]
	@ShareId int
	AS
	SELECT max(TradingDate) as tradingDate
	From Indicators
	WHERE ShareId = @ShareId 
RETURN 0
