Arquivo do Banco de dados - Nome - ArquivoLivrariaPremium:

create database legitimoLivros;
-- drop database legitimoLivros;
use legitimoLivros;
 select * from tbLivro_historico;

create table tbEmail(
codEmail int primary key auto_increment,
email varchar(100) not null unique
);


create table tbLogradouro(
codLogradouro int primary key auto_increment,
logradouro varchar(100) not null unique
);
create table tbEstado(
codEstado int primary key auto_increment,
estado char(2) not null unique
);
create table tbCidade(
codCidade int primary key auto_increment,
cidade varchar(100) not null unique
);
create table tbBairro(
codBairro int primary key auto_increment,
bairro varchar(100) not null unique
);

create table tbEndereco(
cep varchar(8) primary key,
codLogradouro int not null,
codEstado int not null,
codCidade int not null,
codBairro int not null,
foreign key(codLogradouro) references tbLogradouro(codLogradouro),
foreign key(codEstado) references tbEstado(codEstado),
foreign key(codCidade) references tbCidade(codCidade),
foreign key(codBairro) references tbBairro(codBairro)
);

create table tbTel(
idTel int primary key auto_increment,
numTel bigint not null unique
);

create table tbCliente(
idCli int primary key auto_increment,
nome varchar(100) not null,
codEmail int not null,
codTel int not null unique,
cep varchar(8) not null, -- vamos normalizar ele
senha varchar(100) not null,
numeroEndereco int not null,
ComplementoEndereco varchar(100),
foreign key(cep) references tbEndereco(cep),
foreign key(codEmail) references tbEmail(codEmail),
foreign key(codTel) references tbTel(idTel)
);


create table tbClientePF(
cpf bigint not null unique,
datanasc date not null,
idCli int primary key,
foreign key(idCli) references tbcliente(idCli),
check (cpf REGEXP '^[0-9]{11}$')
);

create table tbClientePJ(
cnpj bigint not null unique,
idCli int primary key,
foreign key(idCli) references tbcliente(idCli),
check (cnpj REGEXP '^[0-9]{14}$')
);

create table tbFuncionario(
idFuncionario int auto_increment primary key,
codEmail int not null,
codTel int not null unique,
cpf bigint not null unique,
nome varchar(100) not null,
cargo enum("administrador total", "gerente de compras", "gerente de contratação", "gerente comercial", "caixa") not null,
dataAdmissao date not null,
cep varchar(8) not null, 
senha varchar(100) not null,
numeroEndereco int not null,
ComplementoEndereco varchar(100),
fimContrato date null ,
foreign key(cep) references tbEndereco(cep),
foreign key(codTel) references tbTel(idTel),
foreign key(codEmail) references tbEmail(codEmail)
);

create table tbAutor(
codAutor int primary key auto_increment,
nomeAutor varchar(100) not null unique
);
create table tbEditora(
codEditora int primary key auto_increment,
nomeEditora varchar(100) not null unique
);


create table tbLivro(
codBarras decimal(13,0) primary key,
ISBN decimal(13,0) not null unique,
nome varchar(100) not null, -- vai repetir algumas vezes, mas não a ponto de valer a pena normalizar
preco decimal(6,2) not null,
sinopse varchar(850) not null,
codAutor int not null,
codEditora int not null,
promocaoPorcentagem int default(0),
idadeMinRecomendada enum("+3", "+5", "+10", "+14", "+18") not null,
dataPublicacao date not null,
tipo enum("capa dura", "capa mole", "ebook") not null,
idioma enum("português", "inglês", "espanhol") not null,
literatura enum("internacional", "nacional") not null,
tamanho varchar(100) not null,
imagemPri varchar(100) not null,
ebookfile varchar(100),
check(promocaoPorcentagem >= 0 and promocaoPorcentagem < 90),
check(ISBN >= 1000000000000 and ISBN <= 9999999999999),
check(codBarras >=1000000000000 and codBarras <= 9999999999999),
foreign key(codAutor) references tbAutor(codAutor),
foreign key(codEditora) references tbEditora(codEditora)
);

create table tbEstoque(
codBarras decimal(13,0) primary key,
qtd int not null default(0),
capacidadeEstoque int not null,
foreign key(codBarras) references tbLivro(codBarras),
check(codBarras >=1000000000000 and codBarras <= 9999999999999),
check(capacidadeEstoque = 0 OR (capacidadeEstoque >= 10 AND capacidadeEstoque <= 100))
);


create table tbGeneros(
IdGeneros smallint auto_increment primary key,
Genero varchar(50) not null unique
);

insert into tbGeneros(Genero) values ("Romance"), ("Aventura");
insert into tbGeneros(Genero) values ("Fantasia"), ("Ficção Científica"), ("História"), ("Biografia"),
("Autoajuda"), ("Terror"), ("Mistério"), ("Filosofia"), ("Humor"), ("Drama"), ("Suspense"), ("Religião"), ("Literatura Clássica"),
("Literatura Infantil"), ("Literatura Juvenil"), ("Contos"), ("Poesia"), ("Mangás");
select * from tbGeneros order by IdGeneros asc; -- verificando gêneros

create table tbGenerosLivros(
IdGenero smallint,
codBarras decimal(13,0),
primary key(IdGenero, CodBarras),
foreign key (IdGenero) references tbGeneros(IdGeneros),
foreign key (codBarras) references tbLivro(codBarras)
);

create table tbAvaliacao(
codBarras decimal(13,0),
idCli int,
notaLivro smallint not null,
check(notaLivro >= 0 and notaLivro <= 5),
comentario varchar(200) not null,
tituloComentario varchar(50) not null,
dataAvaliacao date not null,
primary key(codBarras, idCli),
foreign key(codBarras) references tbLivro(codBarras),
foreign key(idCli) references tbCliente(idCli),
check(codBarras >=1000000000000 and codBarras <= 9999999999999)
);
select * from tbAvaliacao;


create table tbVenda(
idCli int not null,
numeroVenda int primary key auto_increment,
notaFiscal int not null unique,
idFuncionario int null,
dataVenda date not null,
formaPagamento enum("débito", "crédito", "pix", "Outro") not null,
numeroCartao bigint null,
nomeCartao varchar(100) null,
validade date null,
cvv int null,
numeroParcelamentos smallint not null,
totalVenda decimal(10, 2) not null default(0.00),
qtdTotal int not null default(0),
foreign key(idCli) references tbCliente(idCli),
foreign key(idFuncionario) references tbFuncionario(idFuncionario)
);


create table tbItemVenda(
numeroVenda int,
valorItem decimal(10,2) not null,
qtdItem int not null,
codBarras decimal(13,0),
primary key(numeroVenda, CodBarras),
foreign key(numeroVenda) references tbVenda(numeroVenda),
foreign key(codBarras) references tbLivro(codBarras),
check(codBarras >=1000000000000 and codBarras <= 9999999999999)
);

DELIMITER $$
create procedure spAdicionarLivroGenero(
vgenero varchar(50),
vcodBarras decimal(13,0)
)
begin
   declare vIdGenero smallint;
   select IdGeneros into vIdGenero from tbGeneros where (Genero = vgenero);
   if not exists(select 1 from tbGenerosLivros where IdGenero = vIdGenero and codBarras = vcodBarras)
   then
       insert into tbGenerosLivros(IdGenero, CodBarras) values(vIdGenero, vCodBarras);
   end if;
end
$$







-- Acabando tabelas, iniciamos a View do Livro

CREATE OR REPLACE VIEW ViewLivro AS
SELECT
    li.codBarras,
    li.ISBN,
    li.Nome,
    li.Preco,
    li.Sinopse,
    li.PromocaoPorcentagem,
    au.NomeAutor AS Autor,
    ed.NomeEditora AS Editora,
    li.IdadeMinRecomendada,
    li.dataPublicacao,
    li.Tipo,
    li.Idioma,
    li.Literatura,
    li.Tamanho,
    li.imagemPri AS Imagem,
    li.ebookfile AS ArquivoEbook,
    es.qtd AS Quantidade,
    es.capacidadeEstoque,
    GROUP_CONCAT(DISTINCT g.Genero ORDER BY g.Genero SEPARATOR ', ') AS Generos
FROM tbLivro li
INNER JOIN tbAutor au
    ON li.codAutor = au.codAutor
INNER JOIN tbEditora ed
    ON li.codEditora = ed.codEditora
INNER JOIN tbEstoque es
    ON li.codBarras = es.codBarras
LEFT JOIN tbGenerosLivros gl
    ON li.codBarras = gl.codBarras
LEFT JOIN tbGeneros g
    ON gl.IdGenero = g.IdGeneros
GROUP BY li.CodBarras
ORDER BY CodBarras ASC;

select * from ViewLivro; -- Verificando ViewLivro


-- procedure para adicionar um livro
DELIMITER $$
create procedure spAdicionarLivro(
vcodBarras decimal(13,0),
vISBN decimal(13,0),
vnome varchar(100),
vpreco decimal(10,2),
vsinopse varchar(850),
vAutor varchar(100),
vEditora varchar(100),
vidadeMinRecomendada enum("+3", "+5", "+10", "+14", "+18"),
vdataPublicacao date,
vtipo enum("capa dura", "capa mole", "ebook"),
vidioma enum("português", "inglês", "espanhol"),
vliteratura enum("internacional", "nacional"),
vtamanho varchar(100),
vimagemPri varchar(100),
vebookfile varchar(100),
vcapacidadeEstoque int
)
begin
   declare vcodEditora int;
   declare vcodAutor int;
   if not exists(select 1 from tbLivro where(vcodBarras = codBarras))then
    if not exists(select 1 from tbAutor where(vAutor = nomeAutor))then
     insert into tbAutor (nomeAutor) values (vAutor);
    end if;
    if not exists(select 1 from tbEditora where(vEditora = nomeEditora))then
     insert into tbEditora (nomeEditora) values (vEditora);
    end if;
    select codEditora into vcodEditora from tbEditora where (nomeEditora = vEditora);
    select codAutor into vcodAutor from tbAutor where(nomeAutor = vAutor);
    if(vtipo = "ebook") then
    insert into tbLivro(codBarras, ISBN, nome, preco, sinopse, codAutor, codEditora, idadeMinRecomendada, dataPublicacao, tipo, idioma, literatura, tamanho, imagemPri, ebookfile) values(vcodbarras, vISBN, vnome, vpreco, vsinopse, vcodAutor, vcodEditora, vidadeMinRecomendada, vdataPublicacao, vtipo, vidioma, vliteratura, vtamanho, vimagemPri, vebookfile);
    else
    insert into tbLivro(codBarras, ISBN, nome, preco, sinopse, codAutor, codEditora, idadeMinRecomendada, dataPublicacao, tipo, idioma, literatura, tamanho, imagemPri) values(vcodbarras, vISBN, vnome, vpreco, vsinopse, vcodAutor, vcodEditora, vidadeMinRecomendada, vdataPublicacao, vtipo, vidioma, vliteratura, vtamanho, vimagemPri);
    end if;
    -- vou ter que melhorar a lógica da View para o Estoque receber valor 0
    insert into tbEstoque(codBarras, CapacidadeEstoque) values (vcodBarras, vcapacidadeEstoque);
   end if;
end
$$
call spAdicionarLivro(1234567891234, 1234567891234,"Yukiguni", 140.55, "Yukiguni se passa em uma terra totalmente presente pelo gelo, enfrentando conflitos dolorosos", "Yasunari Kawabata", "Editora Clássicos", "+10", "2000-10-08", "capa dura", "português", "internacional", "50x90cm", "images/imagem1.png", null, 80  );
-- Teste para a procedure (trocar dados depois para melhor integridade ABNT
call spAdicionarLivroGenero("Romance",1234567891234);
call spAdicionarLivroGenero("Ficção Científica", 1234567891234);

-- View que mostra a promoção e preço diminuido
create or replace View ViewLivroPromotion as
select 
preco as precoOriginal,
concat(promocaoPorcentagem,"%") as Promocao,
round(preco * promocaoPorcentagem/100, 2) as precoDiminuido,
round(preco - preco * promocaoPorcentagem/100, 2) as precoFinal
from tbLivro;


-- criando tabela histórica
create table tbLivro_historico like tbLivro;
select * from tbLivro_historico;
alter table tbLivro_historico drop column Nome,
 drop column Sinopse,
 drop column codAutor, 
 drop column codEditora,
 drop column promocaoPorcentagem,
 drop column idadeMinRecomendada,
 drop column dataPublicacao,
 drop column tipo,
 drop column idioma,
 drop column literatura,
 drop column tamanho,
 drop column imagemPri,
 drop column ebookfile;

-- Fazendo uma tabela histórica para usar em uma das ações do funcionário
select * from tbFuncionario;
 alter table tbLivro_historico add IdFuncionario int not null;
 alter table tbLivro_historico add foreign key (IdFuncionario) references tbFuncionario(IdFuncionario);
 alter table tbLivro_historico add dataMudanca datetime not null;
 select * from tbLivro_historico;
 alter table tbLivro_historico drop primary key;
 alter table tbLivro_historico add Idhistorico int auto_increment primary key;
 describe tbLivro_historico;
select * from tbLivro_historico; 
 -- Fazendo procedure de armazenar histórico
 DELIMITER $$
 create procedure spArmazenarHistorico(
 vCodBarras decimal(13,0),
 vISBN decimal(13,0),
 vPreco decimal(10,2),
 vIdFunc int
 )
 begin
  declare vDataMudanca datetime;
  set vDataMudanca = CURRENT_TIMESTAMP();
  insert into tbLivro_historico (codBarras, ISBN, Preco, idFuncionario, dataMudanca) values (vCodBarras, vISBN, vPreco, vIdFunc, vDataMudanca);
 end
 $$


 -- Fazendo procedure de mudar preço
DELIMITER $$
create procedure spChangePrice(
vCodBarras decimal(13,0),
vNome varchar(100),
vPreco decimal(10,2),
vPromocaoPorcentagem int,
vIdFunc int
)
begin
   declare vISBN decimal(13,0);
   if exists(select 1 from tbLivro where codBarras = vCodBarras) then
     update tbLivro set promocaoPorcentagem = vPromocaoPorcentagem, preco = vpreco where (codBarras = vCodbarras and nome = vNome);
     select ISBN into vISBN from tbLivro where codBarras = vCodBarras;
     call spArmazenarHistorico(vCodBarras, vISBN, vPreco, vIdFunc);
   end if;
end
$$

select * from ViewLivroPromotion

-- Fazendo Views para os clientes
create or replace View tbClientePFG as
select
 pf.idCli,
 pf.cpf,
 pf.datanasc,
 cli.nome,
 cli.cep,
 cli.senha,
 cli.numeroEndereco,
 cli.ComplementoEndereco,
 tel.NumTel,
 lo.logradouro,
 ba.bairro,
 ci.cidade,
 es.estado,
 em.email
 from tbClientePF pf
 inner join tbCliente cli
 on pf.idCli = cli.idCli
 inner join tbTel tel
 on cli.codTel = tel.idTel
 inner join tbEndereco en
 on cli.CEP = en.CEP
 inner join tbLogradouro lo
 on en.codLogradouro = lo.codLogradouro
 inner join tbBairro ba
 on en.codBairro = ba.codBairro
 inner join tbCidade ci
 on en.codCidade = ci.codCidade
 inner join tbEstado es
 on en.codEstado = es.codEstado
 inner join tbEmail em
 on cli.codEmail = em.codEmail
 order by cli.idCli asc;
 -- Selecionnando cliente geral pf
select * from tbClientePFG;

-- View Geral Cliente PJ
create or replace View tbClientePJG as
select
 pj.idCli,
 pj.cnpj,
 cli.nome,
 cli.cep,
 cli.senha,
 cli.numeroEndereco,
 cli.ComplementoEndereco,
 tel.NumTel,
 lo.logradouro,
 ba.bairro,
 ci.cidade,
 es.estado,
 em.email
 from tbClientePJ pj
 inner join tbCliente cli
 on pj.idCli = cli.idCli
 inner join tbTel tel
 on cli.codTel = tel.idTel
 inner join tbEndereco en
 on cli.CEP = en.CEP
 inner join tbLogradouro lo
 on en.codLogradouro = lo.codLogradouro
 inner join tbBairro ba
 on en.codBairro = ba.codBairro
 inner join tbCidade ci
 on en.codCidade = ci.codCidade
 inner join tbEstado es
 on en.codEstado = es.codEstado
 inner join tbEmail em
 on cli.codEmail = em.codEmail
 order by cli.idCli asc;
 -- Selecionado View Geral PJ
select * from tbClientePJG;

-- View do Cliente geral
create or replace View ViewCliente as
select
 cli.idCli,
 cli.nome,
 cli.senha,
 tel.NumTel,
 em.Email,
 cli.Cep,
 cli.numeroEndereco,
 cli.ComplementoEndereco,
 lo.logradouro,
 ba.bairro,
 ci.cidade,
 es.estado
 from tbCliente cli
 inner join tbTel tel
 on cli.codTel = tel.idTel
 inner join tbEmail em
 on cli.codEmail = em.codEmail
 inner join tbEndereco en
 on cli.CEP = en.CEP
 inner join tbLogradouro lo
 on en.codLogradouro = lo.codLogradouro
 inner join tbBairro ba
 on en.codBairro = ba.codBairro
 inner join tbCidade ci
 on en.codCidade = ci.codCidade
 inner join tbEstado es
 on en.codEstado = es.codEstado
 order by cli.idCli asc;
select * from ViewCliente;

-- View do Funcionaria
create or replace View ViewFuncionario as
select 
func.idFuncionario,
func.cpf,
func.cargo,
func.nome,
func.dataAdmissao,
func.cep,
func.senha,
em.Email,
tel.NumTel,
func.numeroEndereco,
func.complementoEndereco,
func.fimContrato,
uf.Estado,
ci.Cidade,
ba.Bairro,
lo.Logradouro
from tbFuncionario func
inner join tbEndereco e
on func.CEP = e.CEP
inner join tbEstado uf
on e.codEstado = uf.codEstado
inner join tbCidade ci
on e.codCidade = ci.codCidade
inner join tbBairro ba
on e.codBairro = ba.codBairro
inner join tbLogradouro lo
on e.codLogradouro = lo.codLogradouro
inner join tbEmail em
on func.codEmail = em.codEmail
inner join tbTel tel
on func.codTel = tel.IdTel
order by func.IdFuncionario asc;
-- selecionando View do funcionário
select * from ViewFuncionario;

-- Procedure para criar endereço

DELIMITER $$
create procedure spCriarEndereco(
vCEP varchar(8),
vLogradourob varchar(100),
vBairrob varchar(100),
vCidadeb varchar(100),
vEstadob char(2)
)
begin
declare vcodLogradouro int;
declare vcodBairro int;
declare vcodCidade int;
declare vcodEstado int;
declare vLogradouro varchar(100);
declare vBairro varchar(100);
declare vCidade varchar(100);
declare vEstado char(2);
set vLogradouro = Upper(vLogradourob);
set vBairro = Upper(vBairrob);
set vCidade = Upper(vCidadeb);
set vEstado = Upper(vEstadob);
   if not exists(select 1 from tbEndereco where (cep = vCEP)) then
      if not exists(select 1 from tbLogradouro where Logradouro = vLogradouro)then
      insert into tbLogradouro (Logradouro) values(vLogradouro);
      end if;
      if not exists(select 1 from tbBairro where Bairro = vBairro) then
      insert into tbBairro (Bairro) values(vBairro);
      end if;
      if not exists(select 1 from tbCidade where Cidade = vCidade) then
      insert into tbCidade(Cidade) values(vCidade);
      end if;
      if not exists(select 1 from tbEstado where Estado = vEstado) then
      insert into tbEstado(Estado) values(vEstado);
      end if;
      select codLogradouro into vcodLogradouro from tbLogradouro where(Logradouro = vLogradouro);
      select codBairro into vcodBairro from tbBairro where(Bairro = vBairro);
      select codCidade into vcodCidade from tbCidade where(Cidade = vCidade);
      select codEstado into vcodEstado from tbEstado where(Estado = vEstado);
      insert into tbEndereco(cep, codLogradouro, codBairro, codCidade, codEstado) values (vCEP, vcodLogradouro, vcodBairro, vcodCidade, vcodEstado);
   end if;
end
$$


-- procedure para cadastrar Cliente PF
-- drop procedure comentado de precaução


-- drop procedure spCadastroClientePF;
DELIMITER $$
create procedure spCadastroClientePF(
vNome varchar(100),
vCPF bigint,
vEmail varchar(100),
vNumTel bigint,
vSenha varchar(100),
vCEP varchar(8),
vNumeroEndereco int,
vComplementoEndereco varchar(100),
vLogradouro varchar(100),
vBairro varchar(100),
vCidade varchar(100),
vEstado char(2),
vDatanasc date
)
begin
declare vcodEmail int;
declare vcodTel int;
declare vidCli int;
   if not exists(select 1 from tbClientePF where CPF = vCPF) then
     if not exists(select 1 from ViewCliente where Email = vEmail or numTel = vnumTel)
     then
          select codEmail into vcodEmail from tbEmail where(email = vEmail);
          select idTel into vcodTel from tbTel where(numTel = vNumTel);
          if vcodEmail is null then
             insert into tbEmail (email) values(vEmail);
             set vcodEmail = last_insert_id();
          end if;
          if vcodTel is null then
             insert into tbTel (numTel) values(vNumTel);
             set vcodTel = last_insert_id();
          end if;
		call spCriarEndereco(vCEP, vLogradouro, vBairro, vCidade, vEstado);
        insert into tbCliente(nome, codEmail, codTel, cep, senha, numeroEndereco, complementoEndereco) value(vNome, vcodEmail, vcodTel, vCEP, vSenha, vNumeroEndereco, vComplementoEndereco);
        set vidCli = last_insert_id();
        insert into tbClientePF(cpf, datanasc, idCli) values(vCPF, vDatanasc, vidCli);
	  end if;
   end if;
end
$$
-- chamando procedure
call spCadastroClientePF('Carlos', 11111111111,'Carlos@gmail.com', 55119234567,'enildo123','05088901', 105, 'Deixar entregas no porão', 'Rua Guaipá', 'Vila Leopoldina', 'São Paulo', 'SP', '1975-06-26');
select * from tbClientePFG;


-- procedure do cliente pj
-- drop procedure spCadastroClientePJ
DELIMITER $$
create procedure spCadastroClientePJ(
vNome varchar(100),
vCNPJ bigint,
vEmail varchar(100),
vNumTel bigint,
vSenha varchar(100),
vCEP varchar(8),
vNumeroEndereco int,
vComplementoEndereco varchar(100),
vLogradouro varchar(100),
vBairro varchar(100),
vCidade varchar(100),
vEstado char(2)
)
begin
declare vcodEmail int;
declare vcodTel int;
declare vidCli int;
   if not exists(select 1 from tbClientePJ where CNPJ = vCNPJ) then
      if not exists(select 1 from ViewCliente where Email = vEmail or numTel = vnumTel)
      then
          select codEmail into vcodEmail from tbEmail where(email = vEmail);
          select idTel into vcodTel from tbTel where(numTel = vNumTel);
          if vcodEmail is null then
             insert into tbEmail (email) values(vEmail);
             set vcodEmail = last_insert_id();
          end if;
          if vcodTel is null then
             insert into tbTel (numTel) values(vNumTel);
             set vcodTel = last_insert_id();
          end if;
		  call spCriarEndereco(vCEP, vLogradouro, vBairro, vCidade, vEstado);
          insert into tbCliente(nome, codEmail, codTel, cep, senha, numeroEndereco, complementoEndereco) value(vNome, vcodEmail, vcodTel, vCEP, vSenha, vNumeroEndereco, vComplementoEndereco);
          set vidCli = last_insert_id(); -- retorna id do cliente pelo último insert
          insert into tbClientePJ(cnpj, idCli) values(vCNPJ, vidCli);
	   end if;
   end if;
end
$$
-- camando procedure
call spCadastroClientePJ('NilsonsCompany', 11111111111111,'NCom@gmail.com', 55119234566,'1980nilson','05088901', 105, 'Telefonar caso eu não estiver na porta', 'Rua Guaipá', 'Vila Leopoldina', 'São Paulo', 'SP');
call spCadastroClientePJ('Livraria SATMO', 11111111111111,'SatmoLiv@gmail.com', 55119237566,'50satmo','05088901', 105, 'Telefonar caso eu não estiver na porta', 'Rua Guaipá', 'Vila Leopoldina', 'São Paulo', 'SP');

select * from ViewCliente;


-- procedure do funcionário

DELIMITER $$
create procedure spCadastroFuncionario(
vNome varchar(100),
vCPF bigint,
vNumTel bigint,
vEmail varchar(100),
vCargo enum("administrador total", "gerente de compras", "gerente de contratação", "gerente comercial", "caixa"),
vDataAdmissao date,
vFimContrato date,
vCEP varchar(8),
vEstado char(2),
vCidade varchar(100),
vBairro varchar(100),
vLogradouro varchar(100),
vSenha varchar(100),
vNumeroEndereco int,
vComplementoEndereco varchar(100)
)
begin
   declare vcodEmail int;
   declare vcodTel int;
   if not exists(select 1 from ViewFuncionario where CPF = vCPF or Email = vEmail or NumTel = vNumTel)
   then
       select codEmail into vcodEmail from tbEmail where Email = vEmail;
       select IdTel into vcodTel from tbTel where numTel = vNumTel;
       if vcodEmail is null
       then
          insert into tbEmail(Email) values(vEmail);
          select codEmail into vcodEmail from tbEmail where Email = vEmail;
       end if;
       if vcodTel is null
       then
          insert into tbTel(numTel) values (vNumTel);
          select IdTel into vcodTel from tbTel where numTel = vNumTel;
       end if;
       call spCriarEndereco(vCEP, vLogradouro, vBairro, vCidade, vEstado);
       insert into tbFuncionario(nome, cpf, codTel, codEmail, cargo, dataAdmissao, fimContrato, CEP, senha, numeroEndereco, complementoEndereco) values(vNome, vCPF, vcodTel, vcodEmail, vCargo, vDataAdmissao, vFimContrato, vCEP, vSenha, vNumeroEndereco, vComplementoEndereco);

   end if;
end 
$$
-- chamando procedure
call spcadastrofuncionario('Maria oliveira', 98765432100, 11999998888, 'maria@email.com', 'gerente de compras', '2026-08-02', '2027-08-02', '01234567', 'SP', 'São Paulo', 'Centro', 'Avenida paulista', 'Bh18659298Getkap', 200, 'apto 45');
call spcadastrofuncionario('joão da silva', 12345678901, 11987654321, 'joao@email.com', 'gerente comercial', '2026-08-02', '2027-08-02', '12345678', 'SP', 'São Paulo', 'Centro', 'Rua das flores', 'kL96449287huYEws', 100, 'apto 12');
call spcadastrofuncionario('Nícolas Schinagel', 57053591875, 11909090900, 'schinagel@gmail.com', 'administrador total', '2022-10-08', null, '12345678', 'SP', 'São Paulo', 'Centro', 'Rua das flores',  'Cg99881234Soruke', 157, 'apto 18'); 
-- Chamando mudança de preço
call spChangePrice(1234567891234, "Yukiguni", 140.55, 30, 1);



-- procedure para adicionar no estoque

DELIMITER $$
create procedure spAdicionarEstoque(
vCodBarras decimal(13,0),
vQtdAdd int
)
begin
   declare vQtdAtual int;
   declare vCapacidade int;
   if exists(select 1 from tbEstoque where codBarras = vCodBarras) then
       select qtd into vQtdAtual from tbEstoque where codBarras = vCodBarras;
       select capacidadeEstoque into vCapacidade from tbEstoque where codBarras = vCodBarras;
       if((vQtdAdd + vQtdAtual) <= vCapacidade) then
           update tbEstoque set qtd = qtd + vQtdAdd where codBarras = vCodBarras;
       end if; -- pensando em adicionar condicional else com volta de erro
   end if;
end
$$
-- chamando procedure 
call spAdicionarEstoque(1234567891234, 20);


select * from tbVenda;
-- procedure da venda

DELIMITER $$
create procedure spCadastroVenda(
vNotaFiscal int,
vCPFcliente bigint,
vCNPJcliente bigint,
vIdFunc int,
vFormaPagamento enum("débito", "crédito", "pix", "Outro"),
vNumeroCartao bigint,
vNomeCartao varchar(100),
vValidade date,
vCVV int,
vNumeroParcelamentos smallint
)
begin
   declare vdataVenda date;
    declare vIdCli int;
    set vdataVenda = CURRENT_TIMESTAMP();
      if exists(select 1 from tbClientePF where CPF = vCPFcliente) then
         select IdCli into vIdCli from tbClientePF where (CPF = vCPFCliente);
         insert into tbVenda (notaFiscal, idFuncionario, idCli, formaPagamento, dataVenda, numeroCartao, nomeCartao, validade, cvv, numeroParcelamentos) values(vNotaFiscal, vIdFunc, vIdCli, vFormaPagamento, vdataVenda, vNumeroCartao, vNomeCartao, vValidade, vCVV, vNumeroParcelamentos);
      end if;
      if exists(select 1 from tbClientePJ where CNPJ = vCNPJcliente) then
         select IdCli into vIdCli from tbClientePJ where (CNPJ = vCNPJcliente);
         insert into tbVenda (notaFiscal, idFuncionario, idCli, formaPagamento, dataVenda, numero_cartao, nomeCartao, validade, cvv, numeroParcelamentos) values(vNotaFiscal, vIdFunc, vIdCli, vFormaPagamento, vdataVenda, vNumeroCartao, vNomeCartao, vValidade, vCVV, vNumeroParcelamentos);
      end if;
end
$$
call spCadastroVenda(987654321, 11111111111, null, 5, "débito", 4539148803436467, "Felipe Camargo S.", "2029-07-21", 182, 1);




-- procedure do item da venda
-- drop procedure spCadastroItemVenda;
-- pegar tipo capa
DELIMITER $$

create procedure spCadastroItemVenda(
vNumeroVenda int,
vCodBarras decimal(13,0),
vQtdItem int
)
begin
   declare vValorItem decimal(10,2);
   declare vTipoLivro varchar(20);
   declare vPromocao decimal(5,2);
   if exists(select 1 from tbLivro where codBarras = vCodBarras)
   then
      if exists( select 1 from tbVenda where numeroVenda = vNumeroVenda)
      then
      select preco, Tipo, PromocaoPorcentagem into vValorItem, vTipoLivro, vPromocao from tbLivro where codBarras = vCodBarras;
      set vValorItem = round(vValorItem * (1 - vPromocao / 100), 2);
      -- pegar tipo da capa para anular perda no estoque
      if vTipoLivro <> 'ebook' then
         update tbEstoque set qtd = qtd - vQtdItem where codBarras = vCodBarras;
      end if;
      insert into tbItemVenda(numeroVenda, codBarras, valorItem, qtdItem) values(vNumeroVenda, vcodBarras, vValorItem, vQtdItem);
      update tbVenda set qtdTotal = qtdTotal + vQtdItem, TotalVenda = TotalVenda + (vValorItem * vQtdItem) where NumeroVenda = vNumeroVenda;
      end if;
   end if;
end 
$$
select * from tbVenda;
call spCadastroItemVenda(1, 9788581862538, 2);
select * from tbItemVenda;
select * from tbLivro;



-- procedure de fazer avaliação

DELIMITER $$
create procedure spFazerAvaliacao(
vCodBarras decimal(13,0),
vIdCli int,
vNotaLivro smallint,
vComentario varchar(200),
vTituloComentario varchar(50)
)
begin 
    if exists(select 1 from tbCliente where IdCli = vIdCli) then
        if exists(select 1 from tbLivro where codBarras = vCodBarras) then
           if not exists(select 1 from tbAvaliacao where codBarras = vCodBarras and IdCli = vIdCli) then
              insert into tbAvaliacao(codBarras, idCli, notaLivro, comentario, tituloComentario, dataAvaliacao) values(vCodBarras, vIdCli, vNotaLivro, vComentario, vTituloComentario, current_date());
           end if;
        end if;
    end if;
end
$$
select * from tbLivro;
select * from tbCliente;
select * from tbAvaliacao;
call spFazerAvaliacao(1234567891234, 1, 4, "Achei o conteúdo do livro muito bom, material é resistente e duraduouro", "Produto Maravilhoso" );
call spFazerAvaliacao(1234567891234, 2, 4, "É uma história meio triste, mas seus aspectos literários são dignos de uma boa leitura", "Boa escolha para ler" );

-- procedure de remover avaliação, talvez usarei mas sem certeza
DELIMITER $$
create procedure spRemoverAvaliacao(
vCodBarras decimal(13,0),
vIdCli int
)
begin
    if exists(select 1 from tbAvaliacao where codBarras = vCodBarras and IdCli = vIdCli) then
       delete from tbAvaliacao where codBarras = vCodBarras and IdCli = vIdCli;
    end if;
end
$$
-- call spRemoverAvaliacao(1234567891234, 2); Não será usado
select * from tbAvaliacao;


-- Fazendo Views para mostrar Vendas por Livro e outro para fazer Média por Livro
create or replace View ViewLivroVendas as
select
    li.codBarras,
    li.nome,
    COUNT(DISTINCT v.numeroVenda) as "NumeroVendas" -- Número de Vendas por Livro nos últimos 30 dias
from tbLivro li
left join tbItemVenda iv
    on li.codBarras = iv.codBarras
left join tbVenda v
    on iv.numeroVenda = v.numeroVenda
    and v.dataVenda >= DATE_SUB(CURDATE(), INTERVAL 1 MONTH)
group by li.codBarras, li.nome
order by NumeroVendas desc;

select * from ViewLivroVendas;


create or replace View ViewLivroAvaliacoes as
select
    li.codBarras,
    li.nome,
    COALESCE(ROUND(AVG(av.notaLivro), 1), 0) as "MediaAvaliacao" -- Média do Livro
from tbLivro li
left join tbAvaliacao av
    on li.codBarras = av.codBarras
group by li.codBarras, li.nome
order by MediaAvaliacao desc;
select * from ViewLivroAvaliacoes;


select * from ViewLivro;
select * from tbCliente;
select * from tbAvaliacao;

create or replace View ViewAvaliacao as 
select 
a.codBarras,
a.Idcli,
a.notaLivro as "nota",
a.comentario,
a.dataAvaliacao,
a.tituloComentario as "titulo",
cli.nome
from tbAvaliacao a
inner join tbCliente cli
on a.IdCli = cli.IdCli
;
select * from ViewAvaliacao;

create or replace view ViewLivroQuantidadeAvaliacoes as
select
    codBarras,

    SUM(case when notaLivro= 0 then 1 else 0 end) AS Nota0,
    SUM(case when notaLivro = 1 then 1 else 0 end) AS Nota1,
    SUM(case when notaLivro = 2 then 1 else 0 end) AS Nota2,
    SUM(case when notaLivro = 3 then 1 else 0 end) AS Nota3,
    SUM(case when notaLivro = 4 then 1 else 0 end) AS Nota4,
    SUM(case when notaLivro = 5 then 1 else 0 end) AS Nota5,

    COUNT(*) as QuantidadeTotal

from tbAvaliacao group by codBarras;

select * from ViewLivroQuantidadeAvaliacoes;

select * from ViewLivro;

DELIMITER $$

create procedure spAtualizarCliente(
vIdCli int,
vSenha varchar(100),
vNumTel bigint,
vEmail varchar(100),
vCEP varchar(8),
vNumeroEndereco int,
vComplementoEndereco varchar(100),
vLogradouro varchar(100),
vBairro varchar(100),
vCidade varchar(100),
vEstado char(2)
)
begin
declare vcodEmail int;
declare vcodTel int;
   if exists(select 1 from tbCliente where idCli = vIdCli) then
      if not exists(select 1 from ViewCliente where (Email = vEmail or numTel = vNumTel) and idCli <> vIdCli)
      then
          select codEmail into vcodEmail from tbEmail where(email = vEmail);
          select idTel into vcodTel from tbTel where(numTel = vNumTel);
          if vcodEmail is null then
             insert into tbEmail (email) values(vEmail);
             set vcodEmail = last_insert_id();
          end if;
          if vcodTel is null then
             insert into tbTel (numTel) values(vNumTel);
             set vcodTel = last_insert_id();
          end if;
          call spCriarEndereco(vCEP, vLogradouro, vBairro, vCidade, vEstado);
          update tbCliente set nome = nome, codEmail = vcodEmail, codTel = vcodTel, CEP = vCEP, senha = ifnull(vSenha, senha), numeroEndereco = vNumeroEndereco, complementoEndereco = vComplementoEndereco where idCli = vIdCli;
      end if;
   end if;
end
$$
select * from ViewCliente;

call spAtualizarCliente(1, 'enildo123', 55119234567, 'Carlosboneco@gmail.com', '05088901', 105, 'Deixar entregas no porão', 'RUA GUAIPÁ', 'VILA LEOPOLDINA', 'SÃO PAULO', 'SP');

DELIMITER $$

create procedure spEditarEndereco(
vIdCli int,
vCEP varchar(8),
vLogradouro varchar(100),
vBairro varchar(100),
vCidade varchar(100),
vEstado char(2),
vNumeroEndereco int,
vComplementoEndereco varchar(100)
)
begin
   if exists(select 1 from tbCliente where idCli = vIdCli) then
      call spCriarEndereco(vCEP, vLogradouro, vBairro, vCidade, vEstado);

      update tbCliente 
      set CEP = vCEP,
          numeroEndereco = vNumeroEndereco,
          complementoEndereco = vComplementoEndereco
      where idCli = vIdCli;
   end if;
end
$$

call spEditarEndereco(1, "04567000", "Rua Augusta", "Consolação", "São Paulo", "SP", 850, "Apartamento 32");
select * from ViewCliente;



DELIMITER $$

CREATE PROCEDURE spAtualizarFuncionario(
    vIdFuncionario int,
    vSenha varchar(100),
    vNumTel bigint,
    vEmail varchar(100),
    vCargo enum("administrador total", "gerente de compras", "gerente de contratação", "gerente comercial", "caixa"),
    vFimContrato date,
    vCEP varchar(8),
    vEstado char(2),
    vCidade varchar(100),
    vBairro varchar(100),
    vLogradouro varchar(100),
    vNumeroEndereco int,
    vComplementoEndereco varchar(100)
)
begin
    declare vcodEmail int default null;
    declare vcodTel int default null;

    if exists(select 1 from tbFuncionario where idFuncionario = vIdFuncionario) then

        if not exists(select 1 from ViewFuncionario where (Email = vEmail or numTel = vNumTel) and idFuncionario <> vIdFuncionario) then

            set vcodEmail = (select codEmail from tbEmail where Email = vEmail limit 1);
            set vcodTel = (select idTel from tbTel where numTel = vNumTel limit 1);

            if vcodEmail is null then
                insert into tbEmail(Email) values(vEmail);
                set vcodEmail = last_insert_id();
            end if;

            if vcodTel is null then
                insert into tbTel(numTel) values(vNumTel);
                set vcodTel = last_insert_id();
            end if;

            call spCriarEndereco(vCEP, vLogradouro, vBairro, vCidade, vEstado);

            update tbFuncionario
            set senha = IFNULL(vSenha, senha),
                codTel = vcodTel,
                codEmail = vcodEmail,
                cargo = vCargo,
                fimContrato = vFimContrato,
                CEP = vCEP,
                numeroEndereco = vNumeroEndereco,
                complementoEndereco = vComplementoEndereco
            where idFuncionario = vIdFuncionario;

        end if;
    end if;
end
$$

-- Fazendo View que mostra livros comprados e seu respecitivo cliente (Útil para saber se cliente comprou um livro e agora tem acesso a avaliação ou ao 
create or replace View ViewLivrosComprados as
select 
cli.Nome as "NomeCli",
cli.idCli,
v.NumeroVenda,
i.codBarras,
li.Nome as "NomeLivro",
li.Tipo
from tbCliente cli
inner join tbVenda v
on cli.idCli = v.idCli
inner join tbItemVenda i
on v.numeroVenda = i.numeroVenda
inner join tbLivro li
on i.codBarras = li.codBarras
order by cli.idCli asc;
select * from ViewLivrosComprados;

create or replace View MinhaCompra as
select
    v.idCli,
    li.nome as Nome,
    au.nomeAutor as Autor,
    ed.nomeEditora as Editora,
    li.ISBN,
    li.imagemPri,
    v.notaFiscal,
    iv.valorItem as PrecoPago,
    v.dataVenda as DataCompra,
    iv.qtdItem as Quantidade
from tbVenda v
inner join tbItemVenda iv
on v.numeroVenda = iv.numeroVenda
inner join tbLivro li
on iv.codBarras = li.codBarras
inner join tbAutor au
on li.codAutor = au.codAutor
inner join tbEditora ed
on li.codEditora = ed.codEditora
order by v.dataVenda desc;
select * from MinhaCompra;



select * from tbLivro;
