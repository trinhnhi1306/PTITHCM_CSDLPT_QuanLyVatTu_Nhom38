USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[sp_TraCuu_NhanVien_Kho]    Script Date: 12/10/2021 11:15:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_TraCuu_NhanVien_Kho] @Code NVARCHAR(30), @Type NVARCHAR(15)
AS
BEGIN
	-- Nhân viên
	IF(@Type = 'MANV')
	BEGIN
		IF EXISTS(SELECT * FROM NhanVien AS NV WHERE NV.MANV = @Code)
			RETURN 1; -- Mã NV tồn tại đã tồn tại
		ELSE
			IF EXISTS(SELECT * FROM LINK2.QLVT.dbo.NhanVien AS NV WHERE NV.MANV = @Code)				
				RETURN 1;
	END

	-- Kho
	IF(@Type = 'MAKHO')
	BEGIN
		IF EXISTS(SELECT * FROM Kho AS KHO WHERE KHO.MAKHO = @Code)
			RETURN 1; -- Mã Kho đã tồn tại
		ELSE
			IF EXISTS(SELECT * FROM LINK2.QLVT.dbo.Kho AS KHO WHERE KHO.MAKHO = @Code)				
				RETURN 1;
	END

	IF(@Type = 'TENKHO')
	BEGIN
		IF EXISTS(SELECT * FROM Kho AS KHO WHERE KHO.TENKHO = @Code)
			RETURN 1; -- Tên Kho đã tồn tại
		ELSE
			IF EXISTS(SELECT * FROM LINK2.QLVT.dbo.Kho AS KHO WHERE KHO.TENKHO = @Code)				
				RETURN 1;
	END

	-- Vật tư
	IF(@Type = 'MAVT')
	BEGIN
		IF EXISTS(SELECT * FROM Vattu AS VT WHERE VT.MAVT = @Code)
			RETURN 1; -- Mã vật tư đã tồn tại
	END

	IF(@Type = 'TENVT')
	BEGIN
		IF EXISTS(SELECT * FROM Vattu AS VT WHERE VT.TENVT = @Code)
			RETURN 1; -- Tên vật tư đã tồn tại
	END

	RETURN 0 -- Không bị trùng
END

