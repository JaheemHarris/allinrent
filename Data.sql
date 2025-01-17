INSERT INTO [dbo].[AuthUser] ([FirstName], [LastName], [EmailAddress], [PhoneNumber], [IdNumber], [Password], [Status])
    VALUES (N'Jaheem', N'Harris', N'namtetsuya@gmail.com', N'038 52 614 78', N'178484541841858', N'$2a$12$g6vaLdDS4KrvXXgxHQZlmu2vid/lATjQb3PW6GYVnO9y6cErBqM5W', 1)


-- Inserting without specifying Id (automatic identity)
INSERT INTO [dbo].[ItemType] ([Label]) 
VALUES (N'Ustensiles de cuisine');

INSERT INTO [dbo].[ItemType] ([Label]) 
VALUES (N'Meubles');

INSERT INTO [dbo].[ItemType] ([Label]) 
VALUES (N'Instruments de musique');

INSERT INTO [dbo].[ItemType] ([Label]) 
VALUES (N'Sonorisations');

INSERT INTO [dbo].[ItemType] ([Label]) 
VALUES (N'Groupes électrogènes');


INSERT INTO [dbo].[Item] ([ItemTypeId], [Name], [Description], [RentalFee]) SELECT (SELECT  [Id] FROM [dbo].[ItemType]  Where [Label] like N'Instruments de musique'), N'Guitare acoustique', N'Guitare acoustique', 70000;
INSERT INTO [dbo].[Item] ([ItemTypeId], [Name], [Description], [RentalFee]) SELECT (SELECT  [Id] FROM [dbo].[ItemType]  Where [Label] like N'Instruments de musique'), N'Guitare éléctrique', N'Guitare éléctrique', 85000;
INSERT INTO [dbo].[Item] ([ItemTypeId], [Name], [Description], [RentalFee]) SELECT (SELECT  [Id] FROM [dbo].[ItemType]  Where [Label] like N'Instruments de musique'), N'Clavier', N'Clavier', 60000;
INSERT INTO [dbo].[Item] ([ItemTypeId], [Name], [Description], [RentalFee]) SELECT (SELECT  [Id] FROM [dbo].[ItemType]  Where [Label] like N'Instruments de musique'), N'Batterie', N'Batterie', 85000;


INSERT INTO [dbo].[Item] ([ItemTypeId], [Name], [Description], [RentalFee]) SELECT (SELECT  [Id] FROM [dbo].[ItemType]  Where [Label] like N'Ustensiles de cuisine'), N'Paquet de 20 Couteaux', N'Paquet de 20 Couteaux', 25000;
INSERT INTO [dbo].[Item] ([ItemTypeId], [Name], [Description], [RentalFee]) SELECT (SELECT  [Id] FROM [dbo].[ItemType]  Where [Label] like N'Ustensiles de cuisine'), N'Paquet de 20 Fourchettes', N'Paquet de 20 Fourchettes', 20000;
INSERT INTO [dbo].[Item] ([ItemTypeId], [Name], [Description], [RentalFee]) SELECT (SELECT  [Id] FROM [dbo].[ItemType]  Where [Label] like N'Ustensiles de cuisine'), N'Paquet de 20 Cuillères', N'Paquet de 20 Cuillères', 20000;
INSERT INTO [dbo].[Item] ([ItemTypeId], [Name], [Description], [RentalFee]) SELECT (SELECT  [Id] FROM [dbo].[ItemType]  Where [Label] like N'Ustensiles de cuisine'), N'Paquet de 6 Verres', N'Paquet de 6 Verres', 12000;

INSERT INTO [dbo].[Item] ([ItemTypeId], [Name], [Description], [RentalFee]) SELECT (SELECT  [Id] FROM [dbo].[ItemType]  Where [Label] like N'Meubles'), N'Chaise', N'Chaise', 3000;
INSERT INTO [dbo].[Item] ([ItemTypeId], [Name], [Description], [RentalFee]) SELECT (SELECT  [Id] FROM [dbo].[ItemType]  Where [Label] like N'Meubles'), N'Table carrée', N'Table carrée', 6000;
INSERT INTO [dbo].[Item] ([ItemTypeId], [Name], [Description], [RentalFee]) SELECT (SELECT  [Id] FROM [dbo].[ItemType]  Where [Label] like N'Meubles'), N'Table ronde', N'Table ronde', 6000;

INSERT INTO [dbo].[Rental] ([UserId], [ItemId], [Quantity], [UnitPrice], [StartDate], [DurationDays], [Due])
    SELECT (SELECT  [Id] FROM [dbo].[AuthUser]  Where [EmailAddress] like N'namtetsuya@gmail.com'), (SELECT  [Id] FROM [dbo].[Item]  Where [Name] like N'Chaise'), 5, 3000, '2024-12-29', 3, 15000;
INSERT INTO [dbo].[Rental] ([UserId], [ItemId], [Quantity], [UnitPrice], [StartDate], [DurationDays], [Due])
    SELECT (SELECT  [Id] FROM [dbo].[AuthUser]  Where [EmailAddress] like N'namtetsuya@gmail.com'), (SELECT  [Id] FROM [dbo].[Item]  Where [Name] like N'Table ronde'), 3, 3000, '2024-12-29', 3, 9000;