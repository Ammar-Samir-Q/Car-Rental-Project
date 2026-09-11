use [CarRental]
go
Insert Into People (FirstName,SecondName,ThirdName,LastName,Phone,NationalNo,Gender,DateOfBirth,Address) 
VALUES('AdminName','AdminName','AdminName','AdminName','7777777','N00',0,2005/1/1,'....');
declare @PersonID int ;
SELECT @PersonID=SCOPE_IDENTITY();
Insert Into Users (PersonID,Username,Password,IsActive,Role,CreationDate) VALUES (@PersonID,'Admin','a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3',1,1,GETDATE());
