CREATE TABLE [dbo].[ShareInfoes] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [CompanyUrl]     NVARCHAR (150) NULL,
    [YSector]        NVARCHAR (100) NULL,
    [YIndustry]      NVARCHAR (100) NULL,
    [CompanySummary] NVARCHAR (MAX) NULL,
    [EmployeeNumber] INT            NULL,
    [MarketCapM]     FLOAT (53)     NULL,
    [IsTop50]        BIT            NOT NULL,
    [IsTop100]       BIT            NOT NULL,
    [IsTop200]       BIT            NOT NULL,
    [IsTop300]       BIT            NOT NULL,
    [LastProcessed]  DATETIME       NULL,
    [ShareId]        INT            NOT NULL,
    CONSTRAINT [PK_dbo.ShareInfoes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ShareInfoes_dbo.Shares_ShareId] FOREIGN KEY ([ShareId]) REFERENCES [dbo].[Shares] ([Id]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[ShareInfoes]([ShareId] ASC);

