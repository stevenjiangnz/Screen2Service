CREATE TABLE [dbo].[WatchLists] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX) NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [Owner]        NVARCHAR (MAX) NULL,
    [IsSystem]     BIT            DEFAULT ((0)) NOT NULL,
    [DisplayOrder] INT            DEFAULT ((0)) NOT NULL,
    [Status]       NVARCHAR (MAX) NULL,
    [ZoneId]       INT            NULL,
    CONSTRAINT [PK_dbo.WatchLists] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.WatchLists_dbo.Zones_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zones] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_ZoneId]
    ON [dbo].[WatchLists]([ZoneId] ASC);

