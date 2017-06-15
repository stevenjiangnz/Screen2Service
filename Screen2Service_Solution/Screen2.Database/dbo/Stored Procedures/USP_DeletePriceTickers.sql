CREATE PROCEDURE [dbo].[USP_DeletePriceTickers]
	@shareId int
AS
	Delete Tickers 
	where ShareId = @shareId
RETURN 0
