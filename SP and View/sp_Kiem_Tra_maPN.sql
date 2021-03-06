USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[sp_Kiem_Tra_maPN]    Script Date: 13/11/2021 08:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Kiem_Tra_maPN] 
@MAPN nchar(8)
AS
BEGIN
	 IF EXISTS(SELECT * FROM dbo.PhieuNhap WHERE dbo.PhieuNhap.MAPN = @MAPN)
			RETURN 1; -- Mã PN tồn tại ở phân mảnh hiện tại
		ELSE IF EXISTS(SELECT * FROM LINK1.QLVT.dbo.PhieuNhap AS PN WHERE PN.MAPN = @MAPN)
			RETURN 2; -- Mã PN tồn tại ở phân mảnh khác
END
