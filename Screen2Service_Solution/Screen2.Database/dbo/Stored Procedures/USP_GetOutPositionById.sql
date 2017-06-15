CREATE PROCEDURE [dbo].[USP_GetOutPositionById]
	@Id int = 0
AS
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
		where p.Id = @Id
RETURN 0
