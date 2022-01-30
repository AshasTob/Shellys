CREATE TABLE [dbo].[Order](
	[Id] [int] NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[TotalPrice] float NOT NULL,
	[Status] [int] NOT NULL
)

CREATE TABLE [dbo].[Menu](
	[Name] [varchar](255) NOT NULL,
	[Price] float NOT NULL
) ON [PRIMARY]
