CREATE PROCEDURE sp_ChuyenChiNhanh
	@MANV INT, 
	@MACN nchar(10)
AS
SET XACT_ABORT ON; -- ON: gặp lỗi thì dừng, OFF: bỏ qua lỗi chạy tiếp
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN
	BEGIN DISTRIBUTED TRAN
		DECLARE @HONV NVARCHAR(40)
		DECLARE @TENNV NVARCHAR(10)
		DECLARE @DIACHINV NVARCHAR(100)
		DECLARE @NGAYSINHNV DATETIME
		DECLARE @LUONGNV FLOAT						
		-- Lưu lại thông tin nhân viên cần chuyển chi nhánh để làm điều kiện kiểm tra
		SELECT @HONV = HO, @TENNV = TEN, @DIACHINV = DIACHI, @NGAYSINHNV = NGAYSINH, @LUONGNV = LUONG FROM NhanVien WHERE MANV = @MANV
		-- Kiểm tra xem bên Site chuyển tới đã có dữ liệu nhân viên đó chưa. Nếu có rồi thì đổi trạng thái, chưa thì thêm vào
		IF EXISTS(select MANV
				from LINK1.QLVT.dbo.NhanVien
				where HO = @HONV and TEN = @TENNV and DIACHI = @DIACHINV
				and NGAYSINH = @NGAYSINHNV and LUONG = @LUONGNV)
		BEGIN
				UPDATE LINK1.QLVT.dbo.NhanVien
				SET TrangThaiXoa = 0
				WHERE MANV = (	select MANV
								from LINK1.QLVT.dbo.NhanVien
								where HO = @HONV and TEN = @TENNV and DIACHI = @DIACHINV
										and NGAYSINH = @NGAYSINHNV and LUONG = @LUONGNV)
		END
		ELSE
		-- nếu chưa tồn tại thì thêm mới hoàn toàn vào chi nhánh mới với MANV sẽ là MANV lớn nhất hiện tại + 1
		BEGIN
			INSERT INTO LINK1.QLVT.dbo.NhanVien (MANV, HO, TEN, DIACHI, NGAYSINH, LUONG, MACN, TRANGTHAIXOA)
			VALUES ((SELECT MAX(MANV) FROM LINK0.QLVT.dbo.NhanVien) + 1, @HONV, @TENNV, @DIACHINV, @NGAYSINHNV, @LUONGNV, @MACN, 0)
		END
		-- Đổi trạng thái xóa đối với tài khoản cũ ở site hiện tại
		UPDATE dbo.NhanVien
		SET TrangThaiXoa = 1
		WHERE MANV = @MANV
	COMMIT TRAN;
		-- sp_droplogin và sp_dropuser không thể được thực thi trong một giao tác do người dùng định nghĩa
		-- Kiểm tra xem Nhân viên đã có login chưa. Có thì xóa
		IF EXISTS(SELECT SUSER_SNAME(sid) FROM sys.sysusers WHERE name = CAST(@MANV AS NVARCHAR))
		BEGIN
			DECLARE @LGNAME VARCHAR(50)
			DECLARE @USERNAME VARCHAR(50)
			SET @LGNAME = CAST((SELECT SUSER_SNAME(sid) FROM sys.sysusers WHERE name = CAST(@MANV AS NVARCHAR)) AS VARCHAR(50))
			SET @USERNAME = CAST(@MANV AS VARCHAR(50))
			EXEC SP_DROPUSER @USERNAME;
			EXEC SP_DROPLOGIN @LGNAME;
		END	
END