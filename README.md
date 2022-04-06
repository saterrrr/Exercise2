# Exercise2
Exercise2 C# and SQL


To get a list of users use the GET at /api/Users

To modify a user use the PUT at /api/Users with the parameters: id of the user You want to modify, and the new username

To validate a username use the GET at /api/Users/validateusername with the parameters: username You want to check

To add a user use the POST at /api/users/adduser with the parameters: username of the new user (has to pass validation)

to delete a user use the DELETE at /ap/users/{id} with the parameters: id of user you want to delete


notes:  
1.ID is the unique GUID  
2.Make sure to change the connection too the database accordingly and the database name, i used sql server 2019 for the development
3.I tested every functionality from the PDF and it worked, if something is missing I might have not concluded that from the instruction  
4.Below is the SQL query used to create a database used during tests  


CREATE TABLE Users
(
ID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
UserName CHAR(30) NOT NULL,
)
GO
INSERT INTO Users (ID, UserName)
VALUES ('8BE8D278-CCC1-46AE-99CF-06514EAD59BF', 'user1723'),
('FE4BC159-B7BC-43E4-9F9C-0A7654850840', 'user7419'),
('3FA85F64-5717-4562-B3FC-2C963F66AFA6', 'user8412');
GO

