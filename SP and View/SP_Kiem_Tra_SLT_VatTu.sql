CREATE PROC [dbo].[SP_Kiem_Tra_SLT_VatTu]
	@MAVT nchar(4),
	@soluong int
AS
BEGIN
	DECLARE @SLT int
	select @SLT = (select SOLUONGTON from Vattu where MAVT = @MAVT)
	if(@soluong <= @SLT)
		return 1
	else
		return 0
END
