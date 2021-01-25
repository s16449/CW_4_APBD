ALTER PROCEDURE PromoteStudents @Name NVARCHAR(100), @Semester INT
AS
BEGIN
	SET XACT_ABORT ON;
	BEGIN TRAN

	DECLARE @IdStudy INT = (select IdStudy from Studies where Name=@Name);
	IF @IdStudy IS NULL
	BEGIN
		RAISERROR('brak studiow',16,1)
		RETURN
	END

	DECLARE @IdEnrollment INT = (select IdEnrollment from Enrollment WHERE IdStudy=@IdStudy);
	IF @IdEnrollment IS NULL
	BEGIN
		RAISERROR('brak rekrutacji',16,1)
		RETURN
	END

	DECLARE @newIdEnrollment INT = (select IdEnrollment from Enrollment WHERE Semester=@Semester+1 AND IdStudy=@IdStudy);
	IF @newIdEnrollment IS NULL		
	BEGIN
		INSERT INTO Enrollment VALUES(@IdEnrollment+1, @Semester+1, @IdStudy, GetDate());
	END

	UPDATE Student SET IdEnrollment=@IdEnrollment + 1

	COMMIT

	RETURN @IdEnrollment + 1;
END

