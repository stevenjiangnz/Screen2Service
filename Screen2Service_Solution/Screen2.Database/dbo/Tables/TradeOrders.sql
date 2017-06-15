CREATE TABLE [dbo].[TradeOrders] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [LatestPrice]          FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [OrderPrice]           FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [Size]                 INT            DEFAULT ((0)) NOT NULL,
    [Direction]            NVARCHAR (MAX) NULL,
    [OrderType]            NVARCHAR (MAX) NULL,
    [Stop]                 FLOAT (53)     DEFAULT ((0)) NULL,
    [Limit]                FLOAT (53)     DEFAULT ((0)) NULL,
    [Note]                 NVARCHAR (MAX) NULL,
    [ShareId]              INT            DEFAULT ((0)) NOT NULL,
    [UpdateDate]           DATETIME       DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [UpdatedBy]            NVARCHAR (MAX) NULL,
    [Status]               NVARCHAR (MAX) NULL,
    [LatestTradingDate]    INT            NOT NULL,
    [AccountId]            INT            DEFAULT ((0)) NOT NULL,
    [ProcessedTradingDate] INT            NULL,
    [Fee]                  FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [OutstandingSize]      INT            DEFAULT ((0)) NOT NULL,
    [FulfiledSize]         INT            DEFAULT ((0)) NOT NULL,
    [TradingOrderDate]     INT            DEFAULT ((0)) NOT NULL,
    [Reserve]              FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [OrderValue]           FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [IsFlip]               BIT            DEFAULT ((0)) NOT NULL,
    [Source]               NVARCHAR (MAX) NULL,
    [Reason]               NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.TradeOrders] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TradeOrders_dbo.Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TradeOrders_dbo.Shares_ShareId] FOREIGN KEY ([ShareId]) REFERENCES [dbo].[Shares] ([Id]) ON DELETE CASCADE
);


















GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[TradeOrders]([ShareId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AccountId]
    ON [dbo].[TradeOrders]([AccountId] ASC);

