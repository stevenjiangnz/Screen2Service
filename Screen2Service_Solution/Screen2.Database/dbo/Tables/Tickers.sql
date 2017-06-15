CREATE TABLE [dbo].[Tickers] (
    [Id]            INT        IDENTITY (1, 1) NOT NULL,
    [TradingDate]   INT        NOT NULL,
    [Open]          FLOAT (53) NOT NULL,
    [Close]         FLOAT (53) NOT NULL,
    [High]          FLOAT (53) NOT NULL,
    [Low]           FLOAT (53) NOT NULL,
    [Volumn]        BIGINT     NOT NULL,
    [AdjustedClose] FLOAT (53) NOT NULL,
    [ShareId]       INT        DEFAULT ((0)) NOT NULL,
    [JSTicks]       BIGINT     DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Tickers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Tickers_dbo.Shares_ShareId] FOREIGN KEY ([ShareId]) REFERENCES [dbo].[Shares] ([Id]) ON DELETE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [TradingDate_Index]
    ON [dbo].[Tickers]([TradingDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[Tickers]([ShareId] ASC);

