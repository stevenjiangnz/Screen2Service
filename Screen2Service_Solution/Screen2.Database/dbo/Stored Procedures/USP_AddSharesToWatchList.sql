CREATE PROCEDURE [dbo].[USP_AddSharesToWatchList]
	@WatchID int = 0,
	@UserID varchar(128),
 	@ShareListString varchar(max)
AS
	insert into dbo.watchlistDetails
	SELECT @WatchID, @UserID, s.ShareID 
	from dbo.split(@ShareListString,',') s
	where not exists (
		Select * from dbo.watchlistDetails w
		where w.ShareId = s.ShareID
		and w.WatchListId = @WatchID
	)
	option (maxrecursion 0)

RETURN 0
