CREATE TABLE [dbo].[Indicators] (
    [Id]               INT        IDENTITY (1, 1) NOT NULL,
    [TradingDate]      INT        NOT NULL,
    [ShareId]          INT        NOT NULL,
    [Close]            FLOAT (53) NULL,
    [SMA5]             FLOAT (53) NULL,
    [SMA10]            FLOAT (53) NULL,
    [SMA30]            FLOAT (53) NULL,
    [SMA50]            FLOAT (53) NULL,
    [SMA200]           FLOAT (53) NULL,
    [EMA10]            FLOAT (53) NULL,
    [EMA20]            FLOAT (53) NULL,
    [EMA50]            FLOAT (53) NULL,
    [BB_Middle]        FLOAT (53) NULL,
    [BB_Low]           FLOAT (53) NULL,
    [BB_High]          FLOAT (53) NULL,
    [ADX]              FLOAT (53) NULL,
    [ADX_Plus]         FLOAT (53) NULL,
    [ADX_Minus]        FLOAT (53) NULL,
    [MACD]             FLOAT (53) NULL,
    [MACD_Signal]      FLOAT (53) NULL,
    [MACD_Hist]        FLOAT (53) NULL,
    [Heikin_Open]      FLOAT (53) NULL,
    [Heikin_Close]     FLOAT (53) NULL,
    [Heikin_Low]       FLOAT (53) NULL,
    [Heikin_High]      FLOAT (53) NULL,
    [Stochastic_K]     FLOAT (53) NULL,
    [Stochastic_D]     FLOAT (53) NULL,
    [RSI]              FLOAT (53) NULL,
    [WR]               FLOAT (53) NULL,
    [Delt_Price]       FLOAT (53) NULL,
    [Delt_SMA5]        FLOAT (53) NULL,
    [Delt_SMA10]       FLOAT (53) NULL,
    [Delt_SMA50]       FLOAT (53) NULL,
    [Delt_EMA20]       FLOAT (53) NULL,
    [Delt_MACD_Hist]   FLOAT (53) NULL,
    [Delt_MACD]        FLOAT (53) NULL,
    [Delt_MACD_Signal] FLOAT (53) NULL,
    [Delt_K]           FLOAT (53) NULL,
    [Delt_D]           FLOAT (53) NULL,
    [Vol_AVG5]         BIGINT     NULL,
    [Vol_AVG10]        BIGINT     NULL,
    [Vol_AVG20]        BIGINT     NULL,
    [Delt_Diff_ADX]    FLOAT (53) NULL,
    [Prev_Heikin]      BIT        NULL,
    [RSI2]             FLOAT (53) NULL,
    CONSTRAINT [PK_dbo.Indicators] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Indicators_dbo.Shares_ShareId] FOREIGN KEY ([ShareId]) REFERENCES [dbo].[Shares] ([Id]) ON DELETE CASCADE
);
















GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[Indicators]([ShareId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TradingDate]
    ON [dbo].[Indicators]([TradingDate] ASC);

