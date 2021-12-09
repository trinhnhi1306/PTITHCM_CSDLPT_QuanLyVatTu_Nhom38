USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[sp_Kiem_Tra_CTPX]    Script Date: 9/12/2021 8:02:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Kiem_Tra_CTPX]
	@MasoPX nchar(8),
	@MAVT nchar(4)
AS
BEGIN
	IF EXISTS(SELECT 1 FROM CTPX WHERE MAPX = @MasoPX AND MAVT = @MAVT)
		RETURN 1;
	ELSE
		RETURN 0;
END
