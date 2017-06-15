CREATE PROCEDURE [dbo].[USP_GetShareListByTradingDate]
	@TradingDate int 
AS
	Select * 
	from shares 
	where id in (select shareid from tickers where tradingdate= @TradingDate)

RETURN 0
