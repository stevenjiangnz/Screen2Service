CREATE PROCEDURE [dbo].[USP_GetPositionListByAccount]
	@AccountId int,
	@Take int = 50
AS
IF(@Take < 0)
    BEGIN
		SELECT p.id,
				p.ShareId,
				p.Size,
				p.EntryPrice,
				p.EntryFee,
				t1.TradingDate as EntryDate,
				p.EntryTransactionId,
				p.ExistPrice as ExitPrice,
				p.ExistFee as ExitFee,
				t2.TradingDate as ExitDate,
				p.ExitTransactionId,
				p.[Days] as DaySpan
		 from TradePositions p 
		 inner join Transactions t1 on p.EntryTransactionId = t1.Id
		 left join Transactions t2 on p.ExitTransactionId = t2.Id
		where AccountId = @AccountId 
		order by p.Id desc
	END
	ELSE
	BEGIN
		SELECT TOP (@Take) 
				p.id,
				p.ShareId,
				p.Size,
				p.EntryPrice,
				p.EntryFee,
				t1.TradingDate as EntryDate,
				p.EntryTransactionId,
				p.ExistPrice as ExitPrice,
				p.ExistFee as ExitFee,
				t2.TradingDate as ExitDate,
				p.ExitTransactionId,
				p.[Days] as DaySpan
		 from TradePositions p 
		 inner join Transactions t1 on p.EntryTransactionId = t1.Id
		 left join Transactions t2 on p.ExitTransactionId = t2.Id
		where AccountId = @AccountId 
		order by p.Id desc
	END

RETURN 0
