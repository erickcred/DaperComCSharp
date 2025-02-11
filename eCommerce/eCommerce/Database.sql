﻿CREATE DATABASE [eCommerce]
GO

USE [eCommerce]
GO

CREATE TABLE [Usuario] (
    [Id] INT NOT NULL IDENTITY,
    [Nome] VARCHAR(100) NOT NULL,
    [Email] VARCHAR(100) NOT NULL,
    [Sexo] CHAR(1) NULL,
    [RG] VARCHAR(10) NOT NULL,
    [CPF] VARCHAR(11) NOT NULL,
    [NomeMae] VARCHAR(100) NULL,
    [SituacaoCadastro] BIT NOT NULL DEFAULT(1),
    [DataCadastro] DATETIME,

    CONSTRAINT [PK_Usuario] PRIMARY KEY([Id]),
    CONSTRAINT [UQ_Usuario_Email] UNIQUE([Email]),
    CONSTRAINT [UQ_Usuario_CPF] UNIQUE([CPF])
);
GO

CREATE TABLE [Contato] (
    [Id] INT NOT NULL IDENTITY,
    [UsuarioId] INT NOT NULL,
    [Telefone] VARCHAR(12),
    [Celular] VARCHAR(13),

    CONSTRAINT [PK_Contato] PRIMARY KEY([Id]),
    CONSTRAINT [FK_Contato_Usuario] FOREIGN KEY([UsuarioId]) REFERENCES [Usuario]([Id])
);
GO

CREATE TABLE [EnderecoEntrega] (
    [Id] INT NOT NULL IDENTITY,
    [UsuarioId] INT NOT NULL,
    [NomeEndereco] VARCHAR(50) NULL,
    [CEP] VARCHAR(10) NOT NULL,
    [Estado] VARCHAR(2) NOT NULL,
    [Cidade] VARCHAR(100) NOT NULL,
    [Bairro] VARCHAR(100) NOT NULL,
    [Endereco] VARCHAR(100) NOT NULL,
    [Numero] VARCHAR(20) NOT NULL,
    [Complemento] VARCHAR(200) NULL,

    CONSTRAINT [PK_EnderecoEntrega] PRIMARY KEY([Id]), 
    CONSTRAINT [FK_Usuario] FOREIGN KEY([UsuarioId]) REFERENCES [Usuario]([Id])
);
GO

CREATE TABLE [Departamento] (
    [Id] INT NOT NULL IDENTITY,
    [Nome] VARCHAR(200)

    CONSTRAINT [PK_Departamento] PRIMARY KEY([Id])
);
GO

CREATE TABLE [UsuarioDepartamento] (
    [Id] INT NOT NULL IDENTITY,
    [UsuarioId] INT NOT NULL,
    [DepartamentoId] INT NOT NULL,

    CONSTRAINT [PK_UsuarioDepartamento] PRIMARY KEY([Id]),
    CONSTRAINT [FK_UsuarioDepartamento_Usuario] FOREIGN KEY([UsuarioId]) REFERENCES [Usuario]([Id]),
    CONSTRAINT [FK_UsuarioDepartamento_Departamento] FOREIGN KEY([DepartamentoId]) REFERENCES [Departamento]([Id])
);
GO

 -- Inserts
INSERT INTO [Usuario]
    ([Nome], [Email], [RG], [CPF], [NomeMae], [SituacaoCadastro], [DataCadastro])
VALUES
('Fulano da Silva', 'fulano@teste.com', '91231231', '01212312311', 'Nome da Mae', 1, GETDATE()),
('Siclano de Oliveira', 'siclano@teste.com', '91231232', '01212312312', 'Nome da Mae', 1, GETDATE()),
('Astoudo Petra', 'astoufo@teste.com', '91231233', '01212312313', 'Nome da Mae', 1, GETDATE())
GO

INSERT INTO [Contato]
VALUES
    (1, '4132431234', '4198123451'),
    (2, '4132431235', '4198123452'),
    (3, '4132431236', '4198123452')
GO

 -- Procedures
CREATE OR ALTER PROCEDURE spUsuarios
AS
    SELECT
        [Usuario].*,
        [Contato].*
    FROM
        [Usuario]
        LEFT JOIN [Contato] ON [Contato].[UsuarioId] = [Usuario].[Id]
GO;

CREATE OR ALTER PROCEDURE spUsuario (
    @Id INT 
) AS
    SELECT
        *
    FROM
        [Usuario]
    WHERE
        [Id] = @Id
GO