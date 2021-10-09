CREATE PROCEDURE [dbo].[sp_ChuyenChiNhanh] 
	@MANV INT, 
	@MACN nchar(10)
AS
DECLARE @LGNAME VARCHAR(50)
DECLARE @USERNAME VARCHAR(50)
SET XACT_ABORT ON;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRY
	BEGIN TRAN
		-- Kiểm tra xem bên Site chuyển tới đã có dữ liệu nhân viên đó chưa. Nếu có rồi thì đổi trạng thái, chưa thì thêm vào
		IF EXISTS(SELECT * FROM LINK1.QLVT.dbo.NhanVien WHERE MANV = @MANV)
		BEGIN
			UPDATE LINK1.QLVT.dbo.NhanVien
			SET TrangThaiXoa = 0
			WHERE MANV = @MANV;
		END
		ELSE
		-- nếu chưa tồn tại thì thêm mới hoàn toàn vào chi nhánh mới
		BEGIN
			INSERT INTO LINK1.QLVT.dbo.NhanVien (MANV, HO, TEN, DIACHI, NGAYSINH, LUONG, MACN, TRANGTHAIXOA)
			SELECT MANV, HO, TEN, DIACHI, NGAYSINH, LUONG, MACN = @MACN, TRANGTHAIXOA
			FROM dbo.NhanVien
			WHERE MANV = @MANV;
		END
		--Kiểm tra tại phân mảnh hiện tại xem Nhân viên được chuyển có lập bất kì đơn nào chưa. Nếu chưa xóa luôn nhân viên đó
		IF EXISTS(SELECT 1 FROM NhanVien
				WHERE NhanVien.MANV = @MANV AND				
				(EXISTS(SELECT 1 FROM PhieuNhap WHERE PhieuNhap.MANV = NhanVien.MANV) 
					OR EXISTS(SELECT MAPX FROM PhieuXuat WHERE PhieuXuat.MANV = NhanVien.MANV) 
						OR EXISTS(SELECT MasoDDH FROM DatHang WHERE DatHang.MANV = NhanVien.MANV)))
		-- nếu có thì đổi trạng thái xóa
		BEGIN 
			UPDATE dbo.NhanVien
			SET TrangThaiXoa = 1
			WHERE MANV = @MANV;
		END
		ELSE		
		BEGIN
			DELETE FROM NhanVien Where NhanVien.MANV = @MANV
		END
		COMMIT TRAN;
		-- Kiểm tra xem Nhân viên đã có login chưa. Có thì xóa
		IF EXISTS(SELECT SUSER_SNAME(sid) FROM sys.sysusers WHERE name = CAST(@MANV AS NVARCHAR))
		BEGIN
		SET @LGNAME = CAST((SELECT SUSER_SNAME(sid) FROM sys.sysusers WHERE name = CAST(@MANV AS NVARCHAR)) AS VARCHAR(50))
		SET @USERNAME = CAST(@MANV AS VARCHAR(50))
		EXEC SP_DROPUSER @USERNAME;
		EXEC SP_DROPLOGIN  @LGNAME;
		END
END TRY
-- bắt trường hợp nếu đã có trasaction mở rồi mà mở thêm
BEGIN CATCH
	--@@TRANCOUNT trả về số lượng transaction đang hoạt động
	IF (@@TRANCOUNT > 0)
	BEGIN
		ROLLBACK TRAN;
	END;
	THROW;
END CATCH