CREATE PROCEDURE [dbo].[USP_GetDataStartDate]
	@shareId int = 0
AS
	SELECT min(TradingDate) as tradingDate
	From Tickers
	Where ShareId = @shareId
RETURN 0
