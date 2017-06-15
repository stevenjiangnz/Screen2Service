CREATE PROCEDURE [dbo].[USP_GetNoteByShareDate]
	@ShareId int = 0,
	@TradingDate int =0,
	@UserID varchar(128) =''
AS
	SELECT n.[Id]
      ,n.[Comment]
      ,n.[Type]
      ,n.[ShareId]
      ,n.[CreatedBy]
      ,n.[Create]
      ,n.[TradingDate],
	  u.FirstName,
	  u.LastName,
		u.UserName
  FROM [Notes] n
	inner join AspNetUsers u
	on CreatedBy = u.Id
  where (@ShareId =0 Or ShareId = @ShareId) And
		(@TradingDate =0 OR tradingDate is null OR tradingDate <= @TradingDate) and
		(@UserID = '' or CreatedBy = @UserID)
	order by tradingDate desc, [Create] desc
RETURN 0
