CREATE TABLE [dbo].[DailyScanResults] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TradingDate] INT            DEFAULT ((0)) NOT NULL,
    [IsMatch]     BIT            DEFAULT ((0)) NOT NULL,
    [Message]     NVARCHAR (MAX) NULL,
    [ProcessDT]   DATETIME       DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [ShareId]     INT            DEFAULT ((0)) NOT NULL,
    [DailyScanID] INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.DailyScanResults] PRIMARY KEY CLUSTERED ([Id] ASC)
);








GO
CREATE NONCLUSTERED INDEX [IX_TradingDate]
    ON [dbo].[DailyScanResults]([TradingDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[DailyScanResults]([ShareId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DailyScanID]
    ON [dbo].[DailyScanResults]([DailyScanID] ASC);

