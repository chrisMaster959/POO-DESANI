-- Remove a coluna Codigo se existir
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('dbo.Pessoa') AND name='Codigo')
  ALTER TABLE dbo.Pessoa DROP COLUMN Codigo;

-- Adiciona a coluna Telefone se não existir
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('dbo.Pessoa') AND name='Telefone')
  ALTER TABLE dbo.Pessoa ADD Telefone NVARCHAR(MAX) NULL;

-- Adiciona a coluna Discriminator se não existir (para TPH - Table Per Hierarchy)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID('dbo.Pessoa') AND name='Discriminator')
  ALTER TABLE dbo.Pessoa ADD Discriminator NVARCHAR(MAX) NOT NULL DEFAULT 'Pessoa';
