CREATE TABLE [dbo].[Plans] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TradingDate] INT            NOT NULL,
    [Modified]    DATETIME       NOT NULL,
    [Content]     NVARCHAR (MAX) NULL,
    [Status]      NVARCHAR (MAX) NULL,
    [Owner]       NVARCHAR (MAX) NULL,
    [Created]     DATETIME       DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [ZoneId]      INT            NULL,
    CONSTRAINT [PK_dbo.Plans] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Plans_dbo.Zones_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zones] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_ZoneId]
    ON [dbo].[Plans]([ZoneId] ASC);

