CREATE TABLE [dbo].[AsxEods] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Symbol]        VARCHAR (8000) NOT NULL,
    [TradingDate]   INT            NOT NULL,
    [Open]          FLOAT (53)     NOT NULL,
    [Close]         FLOAT (53)     NOT NULL,
    [High]          FLOAT (53)     NOT NULL,
    [Low]           FLOAT (53)     NOT NULL,
    [Volumn]        BIGINT         NOT NULL,
    [AdjustedClose] FLOAT (53)     NULL,
    CONSTRAINT [PK_dbo.AsxEods] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_TradingDate]
    ON [dbo].[AsxEods]([TradingDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Symbol]
    ON [dbo].[AsxEods]([Symbol] ASC);

