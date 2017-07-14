CREATE PROCEDURE [dbo].[USP_GetAsxEodByDate]
	@TradingDate int
AS
	SELECT * from AsxEods a
	WHERE a.TradingDate = @TradingDate
RETURN 0
