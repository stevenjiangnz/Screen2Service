CREATE TABLE [dbo].[WatchListDetails] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [WatchListId] INT            NOT NULL,
    [UserId]      NVARCHAR (128) NOT NULL,
    [ShareId]     INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.WatchListDetails] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.WatchListDetails_dbo.Shares_ShareId] FOREIGN KEY ([ShareId]) REFERENCES [dbo].[Shares] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.WatchListDetails_dbo.WatchLists_WatchListId] FOREIGN KEY ([WatchListId]) REFERENCES [dbo].[WatchLists] ([Id]) ON DELETE CASCADE
);








GO
CREATE NONCLUSTERED INDEX [IX_WatchListId]
    ON [dbo].[WatchListDetails]([WatchListId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[WatchListDetails]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[WatchListDetails]([ShareId] ASC);

