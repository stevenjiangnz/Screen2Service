CREATE TABLE [dbo].[TradeReviews] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [IsDirectionCorrect] BIT            NULL,
    [Notes]              NVARCHAR (MAX) NULL,
    [TradePositionId]    INT            NOT NULL,
    [IsEntryLong]        BIT            NULL,
    [BBEntryPercent]     FLOAT (53)     NULL,
    [BBExitPercent]      FLOAT (53)     NULL,
    [Volatility]         INT            NULL,
    [DaysSpan]           INT            NULL,
    [Diff]               FLOAT (53)     NULL,
    [Diff_Per]           FLOAT (53)     NULL,
    [UpdatedBy]          NVARCHAR (MAX) NULL,
    [UpdatedDT]          DATETIME       DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    [IsLimitTriggered]   BIT            NULL,
    [EntryPercent]       FLOAT (53)     NULL,
    [ExitPercent]        FLOAT (53)     NULL,
    [ProfitRating]       INT            NULL,
    [OverallRating]      INT            NULL,
    [EntryTiming]        INT            NULL,
    [IsStopTriggered]    BIT            NULL,
    [ExitTiming]         INT            NULL,
    [TrendRating]        INT            NULL,
    [IsReverse]          BIT            NULL,
    [IsExitBBTriggered]  BIT            NULL,
    [IsAddSize]          BIT            NULL,
    CONSTRAINT [PK_dbo.TradeReviews] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.TradeReviews_dbo.TradePositions_TradePositionId] FOREIGN KEY ([TradePositionId]) REFERENCES [dbo].[TradePositions] ([Id]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_TradePositionId]
    ON [dbo].[TradeReviews]([TradePositionId] ASC);

