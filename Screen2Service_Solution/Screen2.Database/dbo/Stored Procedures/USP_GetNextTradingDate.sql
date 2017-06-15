CREATE PROCEDURE [dbo].[USP_GetNextTradingDate]
	@TradingDate int
	AS
	SELECT top 1 TradingDate as tradingDate
	From Indicators
	WHERE TradingDate > @TradingDate
	Order by tradingDate asc
RETURN 0
