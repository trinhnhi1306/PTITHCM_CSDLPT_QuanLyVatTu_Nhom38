CREATE PROC [dbo].[SP_Lay_Thong_Tin_NV_Tu_Login]
@TENLOGIN NVARCHAR( 100)
AS
DECLARE @UID INT
DECLARE @MANV NVARCHAR(100)
SELECT @UID= uid , @MANV= NAME FROM sys.sysusers 
  WHERE sid = SUSER_SID(@TENLOGIN)

SELECT MANV= @MANV, 
       HOTEN = (SELECT HO+ ' '+TEN FROM dbo.NHANVIEN WHERE MANV=@MANV ), 
       TENNHOM=NAME
  FROM sys.sysusers
    WHERE UID = (SELECT groupuid FROM sys.sysmembers WHERE memberuid=@uid)
