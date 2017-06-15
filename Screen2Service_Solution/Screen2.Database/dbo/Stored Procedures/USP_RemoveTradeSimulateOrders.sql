CREATE PROCEDURE [dbo].[USP_RemoveTradeSimulateOrders]
	@StatScanSetId int
AS
	Delete TradeSimulateOrders 
	where StatScanSetId = @StatScanSetId
	--where  = @SetRef
RETURN 0
