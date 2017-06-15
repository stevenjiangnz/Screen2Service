CREATE TABLE [dbo].[Notes] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Comment]     NVARCHAR (MAX) NULL,
    [Type]        NVARCHAR (128) NULL,
    [ShareId]     INT            NULL,
    [Create]      DATETIME       NOT NULL,
    [tradingDate] INT            NULL,
    [CreatedBy]   NVARCHAR (128) NULL,
    [ZoneId]      INT            NULL,
    CONSTRAINT [PK_dbo.Notes] PRIMARY KEY CLUSTERED ([Id] ASC)
);








GO



GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[Notes]([ShareId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_tradingDate]
    ON [dbo].[Notes]([tradingDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [dbo].[Notes]([CreatedBy] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ZoneId]
    ON [dbo].[Notes]([ZoneId] ASC);

