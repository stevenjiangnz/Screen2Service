CREATE TABLE [dbo].[AlertResults] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TradingDate] INT            NOT NULL,
    [IsMatch]     BIT            NOT NULL,
    [Message]     NVARCHAR (MAX) NULL,
    [ProcessDT]   DATETIME       NOT NULL,
    [AlertId]     INT            NOT NULL,
    [ShareId]     INT            DEFAULT ((0)) NOT NULL,
    [ZoneId]      INT            NULL,
    CONSTRAINT [PK_dbo.AlertResults] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[AlertResults]([ShareId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AlertId]
    ON [dbo].[AlertResults]([AlertId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TradingDate]
    ON [dbo].[AlertResults]([TradingDate] ASC);

