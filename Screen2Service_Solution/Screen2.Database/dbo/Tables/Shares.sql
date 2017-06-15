CREATE TABLE [dbo].[Shares] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Symbol]          NVARCHAR (30)  NULL,
    [Name]            NVARCHAR (100) NULL,
    [Industry]        NVARCHAR (100) NULL,
    [IsActive]        BIT            NOT NULL,
    [IsYahooVerified] BIT            NOT NULL,
    [LastProcessed]   DATETIME       NULL,
    [ProcessComment]  NVARCHAR (MAX) NULL,
    [ShareType]       NVARCHAR (20)  NOT NULL,
    [MarketId]        INT            NOT NULL,
    [Sector]          NVARCHAR (100) NULL,
    [IsCFD]           BIT            NOT NULL,
    CONSTRAINT [PK_dbo.Shares] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Shares_dbo.Markets_MarketId] FOREIGN KEY ([MarketId]) REFERENCES [dbo].[Markets] ([Id]) ON DELETE CASCADE
);












GO
CREATE NONCLUSTERED INDEX [Symbol_Index]
    ON [dbo].[Shares]([Symbol] ASC);


GO



GO
CREATE NONCLUSTERED INDEX [IsActive_Index]
    ON [dbo].[Shares]([IsActive] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MarketId]
    ON [dbo].[Shares]([MarketId] ASC);

