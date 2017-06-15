CREATE TABLE [dbo].[DailyScans] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (MAX) NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [Owner]           NVARCHAR (MAX) NULL,
    [Modified]        DATETIME       NULL,
    [ZoneId]          INT            NULL,
    [Formula]         NVARCHAR (MAX) NULL,
    [RuleId]          INT            NULL,
    [UseRule]         BIT            DEFAULT ((0)) NOT NULL,
    [Status]          NVARCHAR (MAX) NULL,
    [LastProcessed]   DATETIME       NULL,
    [WatchListString] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.DailyScans] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.DailyScans_dbo.Rules_RuleId] FOREIGN KEY ([RuleId]) REFERENCES [dbo].[Rules] ([Id]),
    CONSTRAINT [FK_dbo.DailyScans_dbo.Zones_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zones] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_RuleId]
    ON [dbo].[DailyScans]([RuleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ZoneId]
    ON [dbo].[DailyScans]([ZoneId] ASC);


GO


