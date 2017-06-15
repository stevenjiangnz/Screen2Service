CREATE TABLE [dbo].[AccountBalances] (
    [Id]            INT        IDENTITY (1, 1) NOT NULL,
    [AvailableFund] FLOAT (53) NOT NULL,
    [TradingDate]   INT        NULL,
    [UpdateDT]      DATETIME   NOT NULL,
    [AccountId]     INT        NOT NULL,
    [FundAmount]    FLOAT (53) DEFAULT ((0)) NOT NULL,
    [FeeSum]        FLOAT (53) DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.AccountBalances] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AccountBalances_dbo.Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([Id]) ON DELETE CASCADE
);








GO
CREATE NONCLUSTERED INDEX [IX_AccountId]
    ON [dbo].[AccountBalances]([AccountId] ASC);

