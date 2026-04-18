----------- CRIANDO TABELAS DO SISTEMA DE BARBEIROS  -----------

Create table Pessoas 
(
	Codigo			int			not null	identity,
	Nome			varchar(50) not null,
	Senha			varchar(20) not null,
	Email			varchar(50) not null	unique,
	Cabeleleiro		bit			not null	default(0),

	-- Restricoes --
	constraint pk_pessoas primary key (Codigo)

)
GO
-----------------------------------------------------------------
/*
ALTER TABLE Pessoas
ADD CONSTRAINT uq_pessoas_email UNIQUE (Email);
*/
-----------------------------------------------------------------
Create table Telefones 
(
	Cod_Pessoa		int			not null,
	Telefone		varchar(20) not null,

	-- Restrições

	Constraint pk_telefone primary key (Telefone),
	Constraint fk_cod_pessoa foreign key (Cod_pessoa) references Pessoas(Codigo)

)
GO
-----------------------------------------------------------------
Create table UFs
(
	Sigla	varchar(2)	not null,
	Nome	varchar(50) not null,

	-- Restricoes 

	constraint pk_sigla primary key (Sigla)
)
GO
-----------------------------------------------------------------
Create table Cidades 
(
	Codigo_IBGE		varchar(20) not null,
	Nome			varchar(50) not null,
	Sigla_UF		varchar(2)  not null,

	-- Restricoes

	constraint pk_codigo_ibge  primary key (Codigo_IBGE),
	Constraint fk_sigla_uf foreign key (Sigla_UF) references UFs (Sigla)
)
GO
-----------------------------------------------------------------
Create table CEPs 
(
	Cep						varchar(8)  not null,
	Cidade_Codigo_Ibge		varchar(20) not null,

	-- Restricoes 

	constraint pk_cep primary key (Cep),
	Constraint fk_cidade_codigo_ibge foreign key  (Cidade_Codigo_Ibge) references Cidades (Codigo_IBGE)
)
GO
-----------------------------------------------------------------
Create table Cabeleleiros 
(
	Cod_Pessoa			int				not null,
	Logradouro			varchar(100)	not null,
	Nr_Logradouro		varchar(10)		not null,
	Bairro				varchar(100)	not null,
	Cep					varchar(8)		not null,

	-- Restrições

	constraint pk_cod_pessoa_cabeleleiro primary key (Cod_Pessoa),
	Constraint fk_cod_pessoa_cabeleleiro foreign key  (Cod_Pessoa) references Pessoas (Codigo),
	Constraint fk_Cep		 foreign key  (Cep) references CEPs (Cep)
)
GO
-----------------------------------------------------------------
Create table Categorias 
(
	Codigo		int				not null identity,
	Descricao	varchar(100)	not null,

	-- Restricoes 

	constraint pk_codigo_categoria primary key (Codigo),
)
GO
-----------------------------------------------------------------
Create table Servicos 
(
	Codigo				int				not null identity,
	Valor				decimal(5,2)	not null,
	Cod_Cabeleleiro		int				not null,
	Cod_Categoria		int				not null,

	-- Restricoes 

	constraint	 pk_codigo_servico		primary key (Codigo),
	Constraint fk_cod_cabeleleiro	 foreign key 	 (Cod_Cabeleleiro)	references	Cabeleleiros (Cod_Pessoa),
	Constraint fk_cod_categoria		 foreign key 	 (Cod_Categoria)		references	Categorias (Codigo)
)
GO
-----------------------------------------------------------------
Create table Clientes
(
	Cod_Pessoa int not null,
	Email varchar(50) not null,


	-- Restricoes 

	constraint pk_cod_pessoa_cliente primary key (Cod_Pessoa),
	Constraint fk_cod_pessoa_cliente foreign key  (Cod_Pessoa) references Pessoas (Codigo),
	Constraint fk_email_pessoa foreign key  (Email) references Pessoas (Email),
)
GO

-----------------------------------------------------------------
Create table Agenda 
(
	Nr_Atendimento int not null identity,
	Data_Hora dateTime null,
	Status bit not null default(0),
	Cod_Cabeleleiro int not null,

	-- Restricoes 

	Constraint pk_nr_atendimento primary key (Nr_Atendimento),
	Constraint fk_cod_cabeleleiro_Agenda foreign key (Cod_Cabeleleiro)  references Cabeleleiros (Cod_Pessoa)
)
GO
-----------------------------------------------------------------

Create table Agenda_cliente 
(
	Nr_Atendimento int not null,
	Cod_Cliente int not null,

	-- Restricoes 

	constraint pk_composta_atendimento_cliente primary key (Nr_Atendimento, Cod_Cliente),
	Constraint fk_nr_atendimento foreign key (Nr_Atendimento)  references Agenda (Nr_Atendimento),
	Constraint fk_cod_cliente foreign key (Cod_Cliente)   references Clientes (Cod_Pessoa),
)
GO
-----------------------------------------------------------------
Create table Agenda_Servicos
(
	Nr_atendimento int not null,
	Cod_Servico int not null,

	-- Restricoes 

	Constraint pk_composta_atendimento_servico primary key (Nr_Atendimento,Cod_Servico),
	Constraint fk_nr_atendimento_NpraN foreign key (Nr_Atendimento) references Agenda (Nr_Atendimento),
	Constraint fk_cod_servico_NpraN foreign key (Cod_Servico) references Servicos (Codigo)
)
GO
-----------------------------------------------------------------
-----------------------------------------------------------------
-----------------------------------------------------------------
-----------------------------------------------------------------
-----------------------------------------------------------------