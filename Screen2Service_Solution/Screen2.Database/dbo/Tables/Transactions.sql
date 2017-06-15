CREATE TABLE [dbo].[Transactions] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [TradeOrderId] INT            NOT NULL,
    [Message]      NVARCHAR (MAX) NULL,
    [ModifiedBy]   NVARCHAR (MAX) NULL,
    [ModifiedDate] DATETIME       DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [Direction]    NVARCHAR (MAX) NULL,
    [Price]        FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [Size]         INT            DEFAULT ((0)) NOT NULL,
    [Fee]          FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [TradingDate]  INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Transactions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Transactions_dbo.TradeOrders_TradeOrderId] FOREIGN KEY ([TradeOrderId]) REFERENCES [dbo].[TradeOrders] ([Id]) ON DELETE CASCADE
);












GO
CREATE NONCLUSTERED INDEX [IX_TradeOrderId]
    ON [dbo].[Transactions]([TradeOrderId] ASC);


GO



GO


