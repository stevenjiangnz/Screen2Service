CREATE PROCEDURE [dbo].[USP_GetIdeaList]
	@Owner nvarchar(max)
	AS
Select 
	ID,
	Topic,
	[Type],
	Created,
	Modified,
	[Owner],
	[Status]
	from Ideas
	where [Owner] = @Owner
	order by id desc
RETURN 0
