CREATE TABLE [dbo].[ETFInfoes] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Category]        NVARCHAR (MAX) NULL,
    [FundFamily]      NVARCHAR (MAX) NULL,
    [NetAssetM]       FLOAT (53)     NULL,
    [YieldPercentage] FLOAT (53)     NULL,
    [InceptionDate]   NVARCHAR (MAX) NULL,
    [ShareId]         INT            NOT NULL,
    CONSTRAINT [PK_dbo.ETFInfoes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ETFInfoes_dbo.Shares_ShareId] FOREIGN KEY ([ShareId]) REFERENCES [dbo].[Shares] ([Id]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[ETFInfoes]([ShareId] ASC);

