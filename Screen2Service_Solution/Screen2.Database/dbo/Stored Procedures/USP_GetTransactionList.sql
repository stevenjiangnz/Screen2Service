CREATE PROCEDURE [dbo].[USP_GetTransactionList]
	@AccountId int,
	@Take int = -1
AS

	IF(@take < 0)
    BEGIN
		Select 
		t.Id,
		t.Direction,
		t.Price,
		t.Size,
		t.[Message],
		t.Fee,
		t.TradingDate,
		t.ModifiedBy,
		t.ModifiedDate,
		o.ShareId,
		s.Symbol,
		t.TradeOrderId as OrderId
		from Transactions t
		inner join TradeOrders o on t.TradeOrderId = o.Id
		inner join Shares s on s.id = o.ShareId
		Where o.AccountId = @AccountId
		Order by t.Id desc
    END
	ELSE
	Begin
		Select top (@Take)
		t.Id,
		t.Direction,
		t.Price,
		t.Size,
		t.[Message],
		t.Fee,
		t.TradingDate,
		t.ModifiedBy,
		t.ModifiedDate,
		o.ShareId,
		s.Symbol,
		t.TradeOrderId as OrderId
		from Transactions t
		inner join TradeOrders o on t.TradeOrderId = o.Id
		inner join Shares s on s.id = o.ShareId
		Where o.AccountId = @AccountId
		order by t.id desc
	End
RETURN 0
