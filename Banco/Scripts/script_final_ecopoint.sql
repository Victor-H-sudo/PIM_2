/* ============================================================
   SCRIPT FINAL - ECOPOINT / RECICLA+
   Banco: PIM_2
   Objetivo: recriar o banco do zero e inserir dados de demonstração.
   Uso: SSMS -> conectar no SQL Server -> abrir este arquivo -> Executar.
   ATENÇÃO: este script apaga o banco PIM_2 se ele já existir.
============================================================ */

USE [master];
GO

IF DB_ID(N'PIM_2') IS NOT NULL
BEGIN
    ALTER DATABASE [PIM_2] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [PIM_2];
END
GO

CREATE DATABASE [PIM_2];
GO

USE [PIM_2];
GO

/* =========================
   TABELA: Usuarios
========================= */
CREATE TABLE [dbo].[Usuarios] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Nome] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [Senha] NVARCHAR(255) NOT NULL,
    [Pontos] INT NULL CONSTRAINT [DF_Usuarios_Pontos] DEFAULT (0),
    [DataCadastro] DATETIME NULL CONSTRAINT [DF_Usuarios_DataCadastro] DEFAULT (GETDATE()),
    [CPF] NVARCHAR(20) NOT NULL,
    [TipoUsuario] NVARCHAR(50) NOT NULL CONSTRAINT [DF_Usuarios_TipoUsuario] DEFAULT ('Usuario'),

    CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Usuarios_Email] UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [UQ_Usuarios_CPF] UNIQUE NONCLUSTERED ([CPF] ASC)
);
GO

/* =========================
   TABELA: TiposMaterial
========================= */
CREATE TABLE [dbo].[TiposMaterial] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Nome] NVARCHAR(50) NOT NULL,
    [Multiplicador] INT NULL CONSTRAINT [DF_TiposMaterial_Multiplicador] DEFAULT (1),

    CONSTRAINT [PK_TiposMaterial] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

/* =========================
   TABELA: Ecopontos
========================= */
CREATE TABLE [dbo].[Ecopontos] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Nome] NVARCHAR(100) NOT NULL,
    [Endereco] NVARCHAR(255) NULL,
    [TipoMaterialId] INT NULL,
    [Email] NVARCHAR(100) NULL,
    [Senha] NVARCHAR(255) NULL,
    [LinkMaps] NVARCHAR(500) NULL,
    [CNPJ] NVARCHAR(20) NULL,

    CONSTRAINT [PK_Ecopontos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Ecopontos_TiposMaterial] FOREIGN KEY ([TipoMaterialId])
        REFERENCES [dbo].[TiposMaterial] ([Id])
);
GO

/* =========================
   TABELA: Recompensas
========================= */
CREATE TABLE [dbo].[Recompensas] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Nome] NVARCHAR(100) NOT NULL,
    [Descricao] NVARCHAR(255) NULL,
    [PontosNecessarios] INT NOT NULL,
    [Validade] DATETIME NULL,

    CONSTRAINT [PK_Recompensas] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

/* =========================
   TABELA: Reciclagens
========================= */
CREATE TABLE [dbo].[Reciclagens] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UsuarioId] INT NOT NULL,
    [TipoMaterialId] INT NOT NULL,
    [Peso] DECIMAL(10,2) NULL,
    [PontosGerados] INT NULL,
    [Data] DATETIME NULL CONSTRAINT [DF_Reciclagens_Data] DEFAULT (GETDATE()),
    [FotoMaterial] NVARCHAR(255) NULL,
    [FotoPeso] NVARCHAR(255) NULL,
    [EcopontoId] INT NULL,

    CONSTRAINT [PK_Reciclagens] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Reciclagens_Usuarios] FOREIGN KEY ([UsuarioId])
        REFERENCES [dbo].[Usuarios] ([Id]),
    CONSTRAINT [FK_Reciclagens_TiposMaterial] FOREIGN KEY ([TipoMaterialId])
        REFERENCES [dbo].[TiposMaterial] ([Id]),
    CONSTRAINT [FK_Reciclagens_Ecopontos] FOREIGN KEY ([EcopontoId])
        REFERENCES [dbo].[Ecopontos] ([Id])
);
GO

/* =========================
   TABELA: Resgates
========================= */
CREATE TABLE [dbo].[Resgates] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UsuarioId] INT NOT NULL,
    [RecompensaId] INT NOT NULL,
    [Data] DATETIME NULL CONSTRAINT [DF_Resgates_Data] DEFAULT (GETDATE()),
    [Status] NVARCHAR(50) NULL,
    [DataResgate] DATETIME NOT NULL CONSTRAINT [DF_Resgates_DataResgate] DEFAULT (GETDATE()),

    CONSTRAINT [PK_Resgates] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Resgates_Usuarios] FOREIGN KEY ([UsuarioId])
        REFERENCES [dbo].[Usuarios] ([Id]),
    CONSTRAINT [FK_Resgates_Recompensas] FOREIGN KEY ([RecompensaId])
        REFERENCES [dbo].[Recompensas] ([Id])
);
GO

/* ============================================================
   DADOS DE DEMONSTRAÇÃO
============================================================ */

/* Usuários de acesso */
INSERT INTO [dbo].[Usuarios] ([Nome], [CPF], [Email], [Senha], [Pontos], [DataCadastro], [TipoUsuario])
VALUES
('Admin EcoPoint', '00000000000', 'admin@ecopoint.com', '123', 0, GETDATE(), 'Admin'),
('Usuario Teste', '11111111111', 'user@ecopoint.com', '123', 1500, GETDATE(), 'Usuario'),
('Maria Recicla', '33333333333', 'maria@ecopoint.com', '123', 850, GETDATE(), 'Usuario'),
('João Sustentável', '44444444444', 'joao@ecopoint.com', '123', 430, GETDATE(), 'Usuario'),
('Empresa Teste', '22222222222222', 'empresa@ecopoint.com', '123', 0, GETDATE(), 'Empresa');
GO

/* Materiais */
INSERT INTO [dbo].[TiposMaterial] ([Nome], [Multiplicador])
VALUES
('Plástico', 1),
('Papel', 1),
('Vidro', 1),
('Metal', 2),
('Eletrônico', 3);
GO

/* Ecopontos */
INSERT INTO [dbo].[Ecopontos] ([Nome], [Endereco], [TipoMaterialId], [Email], [Senha], [LinkMaps], [CNPJ])
VALUES
('Ecoponto Central', 'Rua Exemplo, 100 - São Paulo/SP', 1, 'empresa@ecopoint.com', '123', '', '22222222222222'),
('Coleta Verde Tatuapé', 'Rua Sustentável, 250 - Tatuapé/SP', 4, 'tatuape@ecopoint.com', '123', '', '55555555000155');
GO

/* Recompensas */
INSERT INTO [dbo].[Recompensas] ([Nome], [Descricao], [PontosNecessarios], [Validade])
VALUES
('Chaveiro Gela', 'Chaveiro parceiro Gela disponível para resgate.', 1000, NULL),
('Cupom EcoParceiro', 'Cupom simbólico de desconto em parceiro sustentável.', 700, NULL),
('Ecobag RECICLA+', 'Ecobag reutilizável para incentivar consumo consciente.', 1200, NULL);
GO

/* Reciclagens de demonstração */
INSERT INTO [dbo].[Reciclagens] ([UsuarioId], [TipoMaterialId], [Peso], [PontosGerados], [Data], [FotoMaterial], [FotoPeso], [EcopontoId])
VALUES
(2, 1, 300.00, 300, DATEADD(DAY, -5, GETDATE()), NULL, NULL, 1),
(2, 4, 200.00, 400, DATEADD(DAY, -3, GETDATE()), NULL, NULL, 1),
(3, 5, 150.00, 450, DATEADD(DAY, -2, GETDATE()), NULL, NULL, 2),
(4, 2, 430.00, 430, DATEADD(DAY, -1, GETDATE()), NULL, NULL, 1);
GO

/* Resgate de demonstração */
INSERT INTO [dbo].[Resgates] ([UsuarioId], [RecompensaId], [Data], [Status], [DataResgate])
VALUES
(2, 1, GETDATE(), 'Concluído', GETDATE());
GO

/* Ajusta pontos do usuário após resgate de demonstração:
   Usuario Teste começou com 1500 e resgatou Chaveiro Gela de 1000.
*/
UPDATE [dbo].[Usuarios]
SET [Pontos] = 500
WHERE [Email] = 'user@ecopoint.com';
GO

/* ============================================================
   CONSULTAS DE CONFERÊNCIA
============================================================ */

SELECT * FROM [dbo].[Usuarios];
SELECT * FROM [dbo].[TiposMaterial];
SELECT * FROM [dbo].[Ecopontos];
SELECT * FROM [dbo].[Recompensas];
SELECT * FROM [dbo].[Reciclagens];
SELECT * FROM [dbo].[Resgates];
GO

/* ============================================================
   LOGINS DE TESTE

   Admin:
   admin@ecopoint.com / 123

   Usuário:
   user@ecopoint.com / 123

   Empresa:
   empresa@ecopoint.com / 123
============================================================ */
