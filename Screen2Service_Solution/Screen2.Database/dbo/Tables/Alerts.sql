CREATE TABLE [dbo].[Alerts] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Owner]         NVARCHAR (MAX) NULL,
    [ModifiedDate]  DATETIME       NOT NULL,
    [Formula]       NVARCHAR (MAX) NULL,
    [ZoneId]        INT            NULL,
    [ShareId]       INT            NOT NULL,
    [Message]       NVARCHAR (MAX) NULL,
    [LastProcessed] DATETIME       DEFAULT ('1900-01-01T00:00:00.000') NULL,
    [IsActive]      BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Alerts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Alerts_dbo.Shares_ShareId] FOREIGN KEY ([ShareId]) REFERENCES [dbo].[Shares] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Alerts_dbo.Zones_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zones] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[Alerts]([ShareId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ZoneId]
    ON [dbo].[Alerts]([ZoneId] ASC);

