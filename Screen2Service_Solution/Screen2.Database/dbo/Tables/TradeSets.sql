CREATE TABLE [dbo].[TradeSets] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [CreatedDT]          DATETIME       NOT NULL,
    [ClosedDT]           DATETIME       NULL,
    [Status]             NVARCHAR (MAX) NULL,
    [CreatedTradingDate] INT            DEFAULT ((0)) NOT NULL,
    [ClosedTradingDate]  INT            DEFAULT ((0)) NULL,
    CONSTRAINT [PK_dbo.TradeSets] PRIMARY KEY CLUSTERED ([Id] ASC)
);



