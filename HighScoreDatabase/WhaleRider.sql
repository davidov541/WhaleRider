CREATE TABLE [dbo].[WhaleRider] (
    [Date]       DATETIME       NOT NULL,
    [PlayerName] NVARCHAR (MAX) NULL,
    [Score]      INT            NULL,
    [Platform]   INT            NULL,
    [Mode]       INT            NOT NULL,
    [ID]         INT            NOT NULL,
    CONSTRAINT [PrimaryKey_84b1c822-3aca-42c8-8d9e-12ada5983a5b] PRIMARY KEY CLUSTERED ([ID] ASC)
);


