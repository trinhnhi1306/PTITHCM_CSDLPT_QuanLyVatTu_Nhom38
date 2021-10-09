CREATE PROCEDURE [dbo].[sp_TraCuu_NhanVien_Kho]
@Code NVARCHAR(30), @Type NVARCHAR(15)
AS
BEGIN
	-- Nhân viên
	IF(@Type = 'MANV')
	BEGIN
		IF EXISTS(SELECT * FROM LINK2.QLVT.dbo.NhanVien AS NV WHERE NV.MANV = @Code)
			RETURN 1; -- Mã NV đã tồn tại
	END

	-- Kho
	IF(@Type = 'MAKHO')
	BEGIN
		IF EXISTS(SELECT * FROM LINK2.QLVT.dbo.Kho AS KHO WHERE KHO.MAKHO = @Code)
			RETURN 1; -- Mã Kho đã tồn tại
	END

	IF(@Type = 'TENKHO')
	BEGIN
		IF EXISTS(SELECT * FROM LINK2.QLVT.dbo.Kho AS KHO WHERE KHO.TENKHO = @Code)
			RETURN 1; -- Tên Kho đã tồn tại
	END

	RETURN 0 -- Không bị trùng
END
