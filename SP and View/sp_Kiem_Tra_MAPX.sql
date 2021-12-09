USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[sp_Kiem_Tra_MAPX]    Script Date: 9/12/2021 8:02:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Kiem_Tra_MAPX]
	@MAPX nchar(8)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM PhieuXuat PX WHERE PX.MAPX = @MAPX)
		RETURN 1;
	ELSE
	IF EXISTS(SELECT 1 FROM LINK0.QLVT.dbo.PhieuXuat PX WHERE PX.MAPX = @MAPX)
		RETURN 1;
	RETURN 0;
END
