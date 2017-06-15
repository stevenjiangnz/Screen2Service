CREATE TABLE [dbo].[ScanRuns] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [RunStart] DATETIME       NOT NULL,
    [RunEnd]   DATETIME       NULL,
    [Status]   NVARCHAR (MAX) NULL,
    [Result]   NVARCHAR (MAX) NULL,
    [ScanId]   INT            NOT NULL,
    [Owner]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.ScanRuns] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ScanRuns_dbo.Scans_ScanId] FOREIGN KEY ([ScanId]) REFERENCES [dbo].[Scans] ([Id]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_ScanId]
    ON [dbo].[ScanRuns]([ScanId] ASC);

