CREATE TABLE [dbo].[StatScanSets] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [SetRef]     NVARCHAR (MAX) NULL,
    [EntryLogic] NVARCHAR (MAX) NULL,
    [Notes]      NVARCHAR (MAX) NULL,
    [StartDt]    DATETIME       NOT NULL,
    [CompleteDt] DATETIME       NULL,
    [WatchId]    INT            DEFAULT ((0)) NOT NULL,
    [StartDate]  INT            DEFAULT ((0)) NOT NULL,
    [EndDate]    INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.StatScanSets] PRIMARY KEY CLUSTERED ([Id] ASC)
);



