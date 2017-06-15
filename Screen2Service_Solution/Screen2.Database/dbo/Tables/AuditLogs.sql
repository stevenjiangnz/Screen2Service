CREATE TABLE [dbo].[AuditLogs] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ActionType]    NVARCHAR (50)  NULL,
    [ActionMessage] NVARCHAR (MAX) NULL,
    [ActionResult]  NVARCHAR (MAX) NULL,
    [ExtraData]     NVARCHAR (MAX) NULL,
    [ActionTime]    DATETIME       NOT NULL,
    [ShareId]       INT            NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_dbo.AuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_ShareId]
    ON [dbo].[AuditLogs]([ShareId] ASC);

