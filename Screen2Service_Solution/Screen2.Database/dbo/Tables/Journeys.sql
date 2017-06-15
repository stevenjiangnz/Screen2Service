CREATE TABLE [dbo].[Journeys] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [StartDay] NVARCHAR (MAX) NULL,
    [Status]   NVARCHAR (MAX) NULL,
    [Owner]    NVARCHAR (MAX) NULL,
    [Created]  DATETIME       NOT NULL,
    [Modified] DATETIME       NOT NULL,
    [ZoneId]   INT            NULL,
    [Content]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Journeys] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Journeys_dbo.Zones_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zones] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ZoneId]
    ON [dbo].[Journeys]([ZoneId] ASC);

