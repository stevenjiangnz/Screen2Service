CREATE PROCEDURE [dbo].[USP_RemoveSharesFromWatchList]
		@WatchID int = 0,
	@UserID varchar(128),
 	@ShareListString varchar(max)
AS
	Delete dbo.watchlistDetails
	Where WatchListID = @WatchID and
			UserID =  @UserID and
			ShareID  in (select ShareId from dbo.split(@ShareListString,','))
	option (maxrecursion 0)
RETURN 0

