-- 1. Tipos de Material
CREATE TABLE TiposMaterial (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(50) NOT NULL
);

-- 2. Usuários
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Senha NVARCHAR(255) NOT NULL,
    Pontos INT DEFAULT 0,
    DataCadastro DATETIME DEFAULT GETDATE()
);

-- 3. Locais de Coleta
CREATE TABLE Locais (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Endereco NVARCHAR(255),
    TipoMaterialId INT,

    FOREIGN KEY (TipoMaterialId) REFERENCES TiposMaterial(Id)
);

-- 4. Registros de Reciclagem
CREATE TABLE Reciclagens (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UsuarioId INT NOT NULL,
    LocalId INT NOT NULL,
    TipoMaterialId INT NOT NULL,
    Quantidade DECIMAL(10,2),
    PontosGerados INT,
    Data DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (LocalId) REFERENCES Locais(Id),
    FOREIGN KEY (TipoMaterialId) REFERENCES TiposMaterial(Id)
);

-- 5. Recompensas
CREATE TABLE Recompensas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome NVARCHAR(100) NOT NULL,
    Descricao NVARCHAR(255),
    PontosNecessarios INT NOT NULL,
    Validade DATETIME
);

-- 6. Resgates
CREATE TABLE Resgates (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UsuarioId INT NOT NULL,
    RecompensaId INT NOT NULL,
    Data DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50),

    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (RecompensaId) REFERENCES Recompensas(Id)
);