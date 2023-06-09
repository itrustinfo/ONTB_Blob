USE [CP02_Blob]
GO
/****** Object:  Table [dbo].[ActualDocumentBlob]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActualDocumentBlob](
	[Blob_UID] [uniqueidentifier] NOT NULL,
	[ActualDocumentUID] [uniqueidentifier] NULL,
	[Blob_Data] [varbinary](max) NULL,
	[Delete_Flag] [varchar](1) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_ActualDocumentBlob] PRIMARY KEY CLUSTERED 
(
	[Blob_UID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentsAttachments_Blob]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentsAttachments_Blob](
	[DocumentAttachmentBlob] [uniqueidentifier] NOT NULL,
	[AttachmentUID] [uniqueidentifier] NULL,
	[ActualDocumentUID] [uniqueidentifier] NULL,
	[StatusUID] [uniqueidentifier] NULL,
	[BlobData] [varbinary](max) NULL,
	[DeleteFlag] [varchar](1) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_DocumentsAttachments_Blob] PRIMARY KEY CLUSTERED 
(
	[DocumentAttachmentBlob] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentStatus_Blob]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentStatus_Blob](
	[StatusBlob_UID] [uniqueidentifier] NOT NULL,
	[StatusUID] [uniqueidentifier] NULL,
	[DocumentUID] [uniqueidentifier] NULL,
	[CoverFileBlob_Data] [varbinary](max) NULL,
	[ReviewFileBlob_Data] [varbinary](max) NULL,
	[Delete_Flag] [varchar](1) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_DocumentStatus_Blob] PRIMARY KEY CLUSTERED 
(
	[StatusBlob_UID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentVersionBlob]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentVersionBlob](
	[DocumentVersionBlob] [uniqueidentifier] NOT NULL,
	[DocVersion_UID] [uniqueidentifier] NULL,
	[DocumentUID] [uniqueidentifier] NULL,
	[CoverLetter_Blob] [varbinary](max) NULL,
	[ResubmitFile_Blob] [varbinary](max) NULL,
	[Delete_Flag] [varchar](1) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_DocumentVersionBlob] PRIMARY KEY CLUSTERED 
(
	[DocumentVersionBlob] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ActualDocumentBlob] ADD  CONSTRAINT [DF_ActualDocumentBlob_Delete_Flag]  DEFAULT ('N') FOR [Delete_Flag]
GO
ALTER TABLE [dbo].[ActualDocumentBlob] ADD  CONSTRAINT [DF_ActualDocumentBlob_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[DocumentsAttachments_Blob] ADD  CONSTRAINT [DF_DocumentsAttachments_Blob_DeleteFlag]  DEFAULT ('N') FOR [DeleteFlag]
GO
ALTER TABLE [dbo].[DocumentsAttachments_Blob] ADD  CONSTRAINT [DF_DocumentsAttachments_Blob_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[DocumentStatus_Blob] ADD  CONSTRAINT [DF_DocumentStatus_Blob_Delete_Flag]  DEFAULT ('N') FOR [Delete_Flag]
GO
ALTER TABLE [dbo].[DocumentStatus_Blob] ADD  CONSTRAINT [DF_DocumentStatus_Blob_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[DocumentVersionBlob] ADD  CONSTRAINT [DF_DocumentVersionBlob_Delete_Flag]  DEFAULT ('N') FOR [Delete_Flag]
GO
ALTER TABLE [dbo].[DocumentVersionBlob] ADD  CONSTRAINT [DF_DocumentVersionBlob_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  StoredProcedure [dbo].[usp_DocumentStatusBlob_InsertorUpdate]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_DocumentStatusBlob_InsertorUpdate]
@StatusBlob_UID uniqueidentifier,
@StatusUID uniqueidentifier,
@DocumentUID uniqueidentifier,
@CoverFileBlob_Data varbinary(max),
@ReviewFileBlob_Data varbinary(max)
as
begin
if not exists(select StatusBlob_UID from DocumentStatus_Blob where StatusUID=@StatusUID and Delete_Flag='N')
begin
if @ReviewFileBlob_Data= -1
begin
	insert into DocumentStatus_Blob(StatusBlob_UID,StatusUID,DocumentUID,CoverFileBlob_Data)
	values(@StatusBlob_UID,@StatusUID,@DocumentUID,@CoverFileBlob_Data)
end
else
begin
	insert into DocumentStatus_Blob(StatusBlob_UID,StatusUID,DocumentUID,CoverFileBlob_Data,ReviewFileBlob_Data)
	values(@StatusBlob_UID,@StatusUID,@DocumentUID,@CoverFileBlob_Data,@ReviewFileBlob_Data)
end
end
else
begin
	if @ReviewFileBlob_Data= -1
	begin
		update DocumentStatus_Blob set CoverFileBlob_Data=@CoverFileBlob_Data
		where StatusUID=@StatusUID and Delete_Flag='N';
	end
	else
	begin
		update DocumentStatus_Blob set CoverFileBlob_Data=@CoverFileBlob_Data,ReviewFileBlob_Data=@ReviewFileBlob_Data
		where StatusUID=@StatusUID and Delete_Flag='N';
	end
end
end
GO
/****** Object:  StoredProcedure [dbo].[usp_DocumentVersionBlob_insertorUpdate]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_DocumentVersionBlob_insertorUpdate]
@DocumentVersionBlob uniqueidentifier,
@DocVersion_UID uniqueidentifier,
@DocumentUID uniqueidentifier,
@CoverLetter_Blob varbinary(max),
@ResubmitFile_Blob varbinary(max)
as
begin
if not exists(select DocumentVersionBlob from DocumentVersionBlob where DocVersion_UID=@DocVersion_UID and Delete_Flag='N')
begin
if @CoverLetter_Blob= -1
begin
	insert into DocumentVersionBlob(DocumentVersionBlob,DocVersion_UID,DocumentUID,ResubmitFile_Blob)
	values(@DocumentVersionBlob,@DocVersion_UID,@DocumentUID,@ResubmitFile_Blob)
end
else
begin
	insert into DocumentVersionBlob(DocumentVersionBlob,DocVersion_UID,DocumentUID,CoverLetter_Blob,ResubmitFile_Blob)
	values(@DocumentVersionBlob,@DocVersion_UID,@DocumentUID,@CoverLetter_Blob,@ResubmitFile_Blob)
end
end
else
begin
	if @CoverLetter_Blob= -1
	begin
		update DocumentVersionBlob set ResubmitFile_Blob=@ResubmitFile_Blob
		where DocVersion_UID=@DocVersion_UID and Delete_Flag='N';
	end
	else
	begin
		update DocumentVersionBlob set CoverLetter_Blob=@CoverLetter_Blob,ResubmitFile_Blob=@ResubmitFile_Blob
		where DocVersion_UID=@DocVersion_UID and Delete_Flag='N';
	end
end
end
GO
/****** Object:  StoredProcedure [dbo].[usp_GetActualDocumentBlob_by_ActualDocumentUID]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_GetActualDocumentBlob_by_ActualDocumentUID]
@ActualDocumentUID uniqueidentifier
as
begin
set nocount on;
select Blob_UID,ActualDocumentUID,Blob_Data from ActualDocumentBlob where 
ActualDocumentUID=@ActualDocumentUID and Delete_Flag='N'
end
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAttachmentBlob_by_attachmentUID]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAttachmentBlob_by_attachmentUID] 
	-- Add the parameters for the stored procedure here
@AttachmentUID as uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * From DocumentsAttachments_Blob Where AttachmentUID=@AttachmentUID and DeleteFlag='N'
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetDocumentStatusBlob_by_StatusUID]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_GetDocumentStatusBlob_by_StatusUID]
@StatusUID uniqueidentifier
as
begin
set nocount on;
select StatusBlob_UID,StatusUID,CoverFileBlob_Data,ReviewFileBlob_Data
from DocumentStatus_Blob where StatusUID=@StatusUID and Delete_Flag='N'
end
GO
/****** Object:  StoredProcedure [dbo].[usp_GetDocumentVersionBlob_by_DocVersion_UID]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_GetDocumentVersionBlob_by_DocVersion_UID]
@DocVersion_UID uniqueidentifier
as
begin
set nocount on;
select DocumentVersionBlob,DocVersion_UID,CoverLetter_Blob,ResubmitFile_Blob
from DocumentVersionBlob where DocVersion_UID=@DocVersion_UID and Delete_Flag='N';
end
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertActualDocumentsBlob]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_InsertActualDocumentsBlob] 
	-- Add the parameters for the stored procedure here
@ActualDocumentUID as uniqueidentifier,
@Blob_Data varbinary(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	-- Insert into ActualDocumentBlob Table

if NOt Exists(select Blob_UID From ActualDocumentBlob Where ActualDocumentUID=@ActualDocumentUID)
Begin
	insert into ActualDocumentBlob(Blob_UID,ActualDocumentUID,Blob_Data)
	values(NEWID(),@ActualDocumentUID,@Blob_Data)
End
else
begin
	update ActualDocumentBlob set Blob_Data=@Blob_Data where ActualDocumentUID=@ActualDocumentUID
end

END
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertDocumentsAttachmentsBlob]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_InsertDocumentsAttachmentsBlob] 
	-- Add the parameters for the stored procedure here
@AttachmentUID as uniqueidentifier,
@ActualDocumentUID as uniqueidentifier,
@StatusUID as uniqueidentifier,
@BlobData as varbinary(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if NOt Exists(select DocumentAttachmentBlob From DocumentsAttachments_Blob Where AttachmentUID=@AttachmentUID)
Begin
	INSERT INTO [dbo].[DocumentsAttachments_Blob]
           ([DocumentAttachmentBlob]
           ,[AttachmentUID]
           ,[ActualDocumentUID]
		   ,[StatusUID]
           ,[BlobData]
           ,[DeleteFlag]
           )
     VALUES
           (NEWID()
           ,@AttachmentUID
           ,@ActualDocumentUID
		   ,@StatusUID
           ,@BlobData
           ,'N'
 )
 end
 else
 Update [dbo].[DocumentsAttachments_Blob]
 set BlobData=@BlobData
 Where AttachmentUID=@AttachmentUID
END
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertDocumentStatusBlob]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_InsertDocumentStatusBlob] 
	-- Add the parameters for the stored procedure here
@StatusUID as uniqueidentifier,
@ActualDocumentUID as uniqueidentifier,
@CoverLetterBlob as varbinary(max),
@ReviewFileBlob_Data varbinary(max)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if not exists(select * from DocumentStatus_Blob where StatusUID=@StatusUID and Delete_Flag='N')
begin
			insert into DocumentStatus_Blob(StatusBlob_UID,StatusUID,DocumentUID,CoverFileBlob_Data,ReviewFileBlob_Data)
			values(NEWID(),@StatusUID,@ActualDocumentUID,@CoverLetterBlob,@ReviewFileBlob_Data);
end
	else
	begin


	Update DocumentStatus_Blob set CoverFileBlob_Data=@CoverLetterBlob,ReviewFileBlob_Data=@ReviewFileBlob_Data
	where StatusUID=@StatusUID


	end
		

	  
END
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertDocumentStatusBlob_FirstTime]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[usp_InsertDocumentStatusBlob_FirstTime] 
	-- Add the parameters for the stored procedure here
@StatusUID as uniqueidentifier,
@ActualDocumentUID as uniqueidentifier,
@CoverLetterUID as uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @CoverLetterBlob varbinary(MAX);
	set @CoverLetterBlob=(select Blob_Data from ActualDocumentBlob where ActualDocumentUID=@CoverLetterUID and Delete_Flag='N');
	  --Insert into Document Status Blob table
		  insert into DocumentStatus_Blob(StatusBlob_UID,StatusUID,DocumentUID,CoverFileBlob_Data)
		  values(NEWID(),@StatusUID,@ActualDocumentUID,@CoverLetterBlob);
END
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertDocumentVersionBlob]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_InsertDocumentVersionBlob]
	-- Add the parameters for the stored procedure here
@DocVersionUID as uniqueidentifier,
@DocumentUID as uniqueidentifier,
@Blob_Data as varbinary(max),
@CoverLetter_Blob varbinary(max)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if not exists(select * from DocumentVersionBlob where DocVersion_UID=@DocVersionUID and Delete_Flag='N')
begin
	 insert into DocumentVersionBlob(DocumentVersionBlob,DocVersion_UID,DocumentUID,ResubmitFile_Blob,CoverLetter_Blob)
		  values(NEWID(),@DocVersionUID,@DocumentUID,@Blob_Data,@CoverLetter_Blob);
		  end
		  else
		  Begin
		  Update DocumentVersionBlob set ResubmitFile_Blob=@Blob_Data,CoverLetter_Blob=@CoverLetter_Blob
		  where DocVersion_UID=@DocVersionUID

		  End
END
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertorUpdateActualDocumentBlob]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[usp_InsertorUpdateActualDocumentBlob]
@Blob_UID uniqueidentifier,
@ActualDocumentUID uniqueidentifier,
@Blob_Data varbinary(max)
as
begin
if not exists(select Blob_UID from ActualDocumentBlob where ActualDocumentUID=@ActualDocumentUID and Delete_Flag='N')
begin
	insert into ActualDocumentBlob(Blob_UID,ActualDocumentUID,Blob_Data)
	values(@Blob_UID,@ActualDocumentUID,@Blob_Data)
end
else
begin
	update ActualDocumentBlob set Blob_Data=@Blob_Data where ActualDocumentUID=@ActualDocumentUID
end
end
GO
/****** Object:  StoredProcedure [dbo].[usp_ReplaceDocsFlow2old_Blob]    Script Date: 6/14/2023 12:00:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_ReplaceDocsFlow2old_Blob]
	-- Add the parameters for the stored procedure here
@ActualDocumentUID as uniqueidentifier,
@CoverLetterUID as uniqueidentifier,
@StatusUID as uniqueidentifier,
@DocVersionUID as uniqueidentifier,
@ActualDocument_Blob varbinary(max),
@CoverLetter_Blob as varbinary(max)
 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SET NOCOUNT ON;
	

    -- Insert statements for procedure here
	Update ActualDocumentBlob set Blob_Data=@ActualDocument_Blob
	Where ActualDocumentUID=@ActualDocumentUID


	Update ActualDocumentBlob set Blob_Data=@CoverLetter_Blob
	Where ActualDocumentUID=@CoverLetterUID

	--- update document status table
	Update DocumentStatus_Blob set CoverFileBlob_Data=@CoverLetter_Blob Where StatusUID = @StatusUID

	---- update document Version table
	Update  DocumentVersionBlob set ResubmitFile_Blob=@ActualDocument_Blob  Where DocVersion_UID=@DocVersionUID

END
GO
