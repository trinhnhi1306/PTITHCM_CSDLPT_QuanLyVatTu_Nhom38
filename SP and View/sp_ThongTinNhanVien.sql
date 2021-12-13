USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[sp_ThongTinNhanVien]    Script Date: 13/12/2021 09:46:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_ThongTinNhanVien]
	@MANV int
AS
BEGIN
	select HO+ ' '+TEN AS HOTEN,
	DIACHI,
	NGAYSINH,
	LUONG
	from NhanVien
	WHERE MANV = @MANV 
END
