IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Cidade] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(max) NOT NULL,
    [CodigoIBGE] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Cidade] PRIMARY KEY ([Id])
);

CREATE TABLE [Pessoa] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Senha] nvarchar(max) NOT NULL,
    [Telefone] nvarchar(max) NULL,
    CONSTRAINT [PK_Pessoa] PRIMARY KEY ([Id])
);

CREATE TABLE [Servico] (
    [Id] int NOT NULL IDENTITY,
    [Codigo] int NOT NULL,
    [Nome] nvarchar(max) NOT NULL,
    [Preco] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Servico] PRIMARY KEY ([Id])
);

CREATE TABLE [Cep] (
    [Id] int NOT NULL IDENTITY,
    [Codigo] int NOT NULL,
    [Numero] nvarchar(max) NOT NULL,
    [CidadeId] int NOT NULL,
    CONSTRAINT [PK_Cep] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cep_Cidade_CidadeId] FOREIGN KEY ([CidadeId]) REFERENCES [Cidade] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Cliente] (
    [Id] int NOT NULL,
    CONSTRAINT [PK_Cliente] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cliente_Pessoa_Id] FOREIGN KEY ([Id]) REFERENCES [Pessoa] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Barbeiro] (
    [Id] int NOT NULL,
    [Logradouro] nvarchar(max) NULL,
    [Nr_Logradouro] int NOT NULL,
    [Bairro] nvarchar(max) NULL,
    [CepId] int NULL,
    CONSTRAINT [PK_Barbeiro] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Barbeiro_Cep_CepId] FOREIGN KEY ([CepId]) REFERENCES [Cep] ([Id]),
    CONSTRAINT [FK_Barbeiro_Pessoa_Id] FOREIGN KEY ([Id]) REFERENCES [Pessoa] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Atendimento] (
    [Id] int NOT NULL IDENTITY,
    [DataHora] datetime2 NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [ClienteId] int NOT NULL,
    [BarbeiroId] int NOT NULL,
    CONSTRAINT [PK_Atendimento] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Atendimento_Barbeiro_BarbeiroId] FOREIGN KEY ([BarbeiroId]) REFERENCES [Barbeiro] ([Id]),
    CONSTRAINT [FK_Atendimento_Cliente_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Cliente] ([Id])
);

CREATE TABLE [BarbeiroServico] (
    [BarbeirosId] int NOT NULL,
    [ServicosId] int NOT NULL,
    CONSTRAINT [PK_BarbeiroServico] PRIMARY KEY ([BarbeirosId], [ServicosId]),
    CONSTRAINT [FK_BarbeiroServico_Barbeiro_BarbeirosId] FOREIGN KEY ([BarbeirosId]) REFERENCES [Barbeiro] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BarbeiroServico_Servico_ServicosId] FOREIGN KEY ([ServicosId]) REFERENCES [Servico] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AtendimentoServico] (
    [AtendimentosId] int NOT NULL,
    [ServicosId] int NOT NULL,
    CONSTRAINT [PK_AtendimentoServico] PRIMARY KEY ([AtendimentosId], [ServicosId]),
    CONSTRAINT [FK_AtendimentoServico_Atendimento_AtendimentosId] FOREIGN KEY ([AtendimentosId]) REFERENCES [Atendimento] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AtendimentoServico_Servico_ServicosId] FOREIGN KEY ([ServicosId]) REFERENCES [Servico] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Atendimento_BarbeiroId] ON [Atendimento] ([BarbeiroId]);

CREATE INDEX [IX_Atendimento_ClienteId] ON [Atendimento] ([ClienteId]);

CREATE INDEX [IX_AtendimentoServico_ServicosId] ON [AtendimentoServico] ([ServicosId]);

CREATE INDEX [IX_Barbeiro_CepId] ON [Barbeiro] ([CepId]);

CREATE INDEX [IX_BarbeiroServico_ServicosId] ON [BarbeiroServico] ([ServicosId]);

CREATE INDEX [IX_Cep_CidadeId] ON [Cep] ([CidadeId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260513133357_InitialCreate', N'10.0.5');

COMMIT;
GO

