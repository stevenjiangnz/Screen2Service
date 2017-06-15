CREATE TABLE [dbo].[Scans] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [RuleId]        INT            NOT NULL,
    [IsActive]      BIT            DEFAULT ((0)) NOT NULL,
    [IsSystem]      BIT            DEFAULT ((0)) NOT NULL,
    [Owner]         NVARCHAR (MAX) NULL,
    [StartDate]     INT            DEFAULT ((0)) NOT NULL,
    [EndDate]       INT            NULL,
    [Name]          NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [Description]   NVARCHAR (MAX) NULL,
    [IsScheduled]   BIT            DEFAULT ((0)) NOT NULL,
    [ShareString]   NVARCHAR (MAX) NULL,
    [WatchString]   NVARCHAR (MAX) NULL,
    [ScopeType]     NVARCHAR (MAX) NULL,
    [ProfitFormula] NVARCHAR (MAX) NULL,
    [StopFormula]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Scans] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Scans_dbo.Rules_RuleId] FOREIGN KEY ([RuleId]) REFERENCES [dbo].[Rules] ([Id]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_RuleId]
    ON [dbo].[Scans]([RuleId] ASC);

