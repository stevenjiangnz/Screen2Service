CREATE TABLE [dbo].[TradePositions] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Size]               INT            NOT NULL,
    [CurrentPrice]       FLOAT (53)     NOT NULL,
    [CurrentTradingDate] INT            NOT NULL,
    [ShareId]            INT            NOT NULL,
    [AccountId]          INT            NOT NULL,
    [EntryPrice]         FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [UpdateDT]           DATETIME       DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [Stop]               FLOAT (53)     NULL,
    [Limit]              FLOAT (53)     NULL,
    [Source]             NVARCHAR (MAX) NULL,
    [EntryFee]           FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [ExistFee]           FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [ExistPrice]         FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [EntryTransactionId] INT            DEFAULT ((0)) NOT NULL,
    [ExitTransactionId]  INT            NULL,
    [LastProcessedDate]  INT            NULL,
    [days]               INT            NULL,
    [Margin]             FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [OtherCost]          FLOAT (53)     NULL,
    CONSTRAINT [PK_dbo.TradePositions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TradePositions_dbo.Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TradePositions_dbo.Shares_ShareId] FOREIGN KEY ([ShareId]) REFERENCES [dbo].[Shares] ([Id]) ON DELETE CASCADE
);
















GO
CREATE NONCLUSTERED INDEX [IX_AccountId]
    ON [dbo].[TradePositions]([AccountId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[TradePositions]([ShareId] ASC);


GO


