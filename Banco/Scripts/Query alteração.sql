USE PIM_2;
GO

/* =====================================================
   RENOMEAR Locais → Ecopontos
===================================================== */
EXEC sp_rename 'Locais', 'Ecopontos';
GO

/* =====================================================
   USUARIOS
===================================================== */
ALTER TABLE Usuarios
ADD CPF CHAR(11);
GO

/* =====================================================
   ECOPONTOS
===================================================== */
ALTER TABLE Ecopontos
ADD 
    Email NVARCHAR(100),
    Senha NVARCHAR(255),
    LinkMaps NVARCHAR(500);
GO

/* =====================================================
   TIPOS MATERIAL
===================================================== */
ALTER TABLE TiposMaterial
ADD Multiplicador INT DEFAULT 1;
GO

/* =====================================================
   RECICLAGENS
===================================================== */

/* Renomear Quantidade → Peso */
EXEC sp_rename 
    'Reciclagens.Quantidade', 
    'Peso', 
    'COLUMN';
GO

/* Adicionar fotos */
ALTER TABLE Reciclagens
ADD
    FotoMaterial NVARCHAR(255),
    FotoPeso NVARCHAR(255);
GO