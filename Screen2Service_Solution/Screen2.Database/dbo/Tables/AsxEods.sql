CREATE TABLE [dbo].[AsxEods] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Symbol]        NVARCHAR (MAX) NULL,
    [TradingDate]   INT            NOT NULL,
    [Open]          FLOAT (53)     NOT NULL,
    [Close]         FLOAT (53)     NOT NULL,
    [High]          FLOAT (53)     NOT NULL,
    [Low]           FLOAT (53)     NOT NULL,
    [Volumn]        BIGINT         NOT NULL,
    [AdjustedClose] FLOAT (53)     NULL,
    [ShareId]       INT            NOT NULL,
    CONSTRAINT [PK_dbo.AsxEods] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AsxEods_dbo.Shares_ShareId] FOREIGN KEY ([ShareId]) REFERENCES [dbo].[Shares] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[AsxEods]([ShareId] ASC);

