CREATE TABLE [dbo].[AccountBalanceJourneys] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [TotalBalance]      FLOAT (53)     NOT NULL,
    [AvailableFund]     FLOAT (53)     NOT NULL,
    [Margin]            FLOAT (53)     NOT NULL,
    [PositionValue]     FLOAT (53)     NOT NULL,
    [TradingDate]       INT            NULL,
    [UpdateDT]          DATETIME       NOT NULL,
    [TransactionId]     INT            NULL,
    [Action]            NVARCHAR (MAX) NULL,
    [AccountId]         INT            DEFAULT ((0)) NOT NULL,
    [AccountSummaryXML] NVARCHAR (MAX) NULL,
    [OrderId]           INT            NULL,
    [FundAmount]        FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [FeeSum]            FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [Reserve]           FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [TradeSetId]        INT            NULL,
    CONSTRAINT [PK_dbo.AccountBalanceJourneys] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AccountBalanceJourneys_dbo.Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([Id]) ON DELETE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_TransactionId]
    ON [dbo].[AccountBalanceJourneys]([TransactionId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AccountId]
    ON [dbo].[AccountBalanceJourneys]([AccountId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_OrderId]
    ON [dbo].[AccountBalanceJourneys]([OrderId] ASC);

