USE [QuanikaDB]
GO
/****** Object:  Table [dbo].[Installed_Versions]    Script Date: 02/06/2020 12:56:27 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Installed_Versions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Hostname] [varchar](125) NULL,
	[IpAddress] [varchar](125) NULL,
	[Type] [varchar](125) NULL,
	[Version] [varchar](125) NULL,
 CONSTRAINT [PK_Installed_Versions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UpdateLogs]    Script Date: 02/06/2020 12:56:27 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UpdateLogs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Version] [nvarchar](50) NULL,
	[Command] [nvarchar](125) NULL,
	[Status] [bit] NULL,
	[TimeStamp] [datetime] NULL,
	[InstalledVersion] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_DBbackupUpdate]    Script Date: 02/06/2020 12:56:27 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[sp_DBbackupUpdate](    
@BackupFile VARCHAR(MAX),
@DatabaseName VARCHAR(MAX)
)    
AS    
Declare @Path varchar(100)
BEGIN   

     begin try 
	 set @Path = 'c:\QuanikaUpadate\DbBackups'
     EXEC master.dbo.xp_create_subdir @Path
	 IF NOT EXISTS (SELECT * FROM [dbo].[DatabaseBackupFiles] WHERE BackupFile = @BackupFile ) 
	 BEGIN
         BACKUP DATABASE @DatabaseName TO DISK = @BackupFile
		 insert into databasebackupfiles values(@BackupFile);
     END
	 ElSE
	 BEGIN
	  BACKUP DATABASE @DatabaseName TO DISK = @BackupFile
	 END

	 select 1 as flag

	 end try

     begin catch
        select 0 as flag
     end catch    
END

GO
