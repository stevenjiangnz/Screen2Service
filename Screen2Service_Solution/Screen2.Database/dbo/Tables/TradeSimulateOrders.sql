CREATE TABLE [dbo].[TradeSimulateOrders] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [ShareId]          INT            NOT NULL,
    [EntryTradingDate] INT            NOT NULL,
    [EntryPrice]       FLOAT (53)     NOT NULL,
    [ExitTradingDate]  INT            NULL,
    [ExitPrice]        FLOAT (53)     NULL,
    [Days]             INT            NOT NULL,
    [Flag]             INT            NOT NULL,
    [Diff_Per]         FLOAT (53)     NULL,
    [UpdatedDT]        DATETIME       NOT NULL,
    [Diff]             FLOAT (53)     NULL,
    [Day5Highest]      FLOAT (53)     NULL,
    [Day5Lowest]       FLOAT (53)     NULL,
    [Day5AboveDays]    INT            NULL,
    [ExitMode]         NVARCHAR (MAX) NULL,
    [StatScanSetId]    INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.TradeSimulateOrders] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TradeSimulateOrders_dbo.StatScanSets_StatScanSetId] FOREIGN KEY ([StatScanSetId]) REFERENCES [dbo].[StatScanSets] ([Id]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_StatScanSetId]
    ON [dbo].[TradeSimulateOrders]([StatScanSetId] ASC);

