-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:3306
-- Generation Time: 03-Jan-2019 às 02:16
-- Versão do servidor: 5.7.23
-- versão do PHP: 7.2.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `u911430744_estg`
--

-- --------------------------------------------------------

--
-- Estrutura da tabela `curso`
--

DROP TABLE IF EXISTS `curso`;
CREATE TABLE IF NOT EXISTS `curso` (
  `idCurso` int(11) NOT NULL,
  `nomeCurso` varchar(150) COLLATE utf8_unicode_ci DEFAULT 'Curso Padrão',
  `descricaoCurso` text COLLATE utf8_unicode_ci,
  `coordenadorCurso` varchar(150) COLLATE utf8_unicode_ci DEFAULT NULL,
  `faculdadeCurso` varchar(150) COLLATE utf8_unicode_ci DEFAULT 'FacisaBH',
  `Documento_idDocumento` int(11) NOT NULL,
  PRIMARY KEY (`idCurso`),
  KEY `fk_Curso_Documento_idx` (`Documento_idDocumento`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Extraindo dados da tabela `curso`
--

INSERT INTO `curso` (`idCurso`, `nomeCurso`, `descricaoCurso`, `coordenadorCurso`, `faculdadeCurso`, `Documento_idDocumento`) VALUES
(1, 'Análise e Desenvolvimento de Sistemas', 'Curso de técnicas de Analise, Engeharia e Desenvolvimento de Sistemas', 'André Rodrigues', 'FacisaBH', 1);

-- --------------------------------------------------------

--
-- Estrutura da tabela `documento`
--

DROP TABLE IF EXISTS `documento`;
CREATE TABLE IF NOT EXISTS `documento` (
  `idDocumento` int(11) NOT NULL,
  `Curso_idCurso` int(11) NOT NULL,
  `tituloDocumento` varchar(150) COLLATE utf8_unicode_ci NOT NULL,
  `descricaoDocumento` varchar(300) COLLATE utf8_unicode_ci NOT NULL,
  `requisitoDocumento` varchar(150) COLLATE utf8_unicode_ci DEFAULT NULL,
  `preenchimentoDocumento` text COLLATE utf8_unicode_ci NOT NULL,
  `posicaoDocumento` int(11) NOT NULL,
  `autorDocumento` varchar(45) COLLATE utf8_unicode_ci NOT NULL DEFAULT 'FacisaBH',
  `caminhoDocumento` varchar(150) COLLATE utf8_unicode_ci NOT NULL,
  `tiposRequisitos` varchar(150) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`idDocumento`,`descricaoDocumento`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Extraindo dados da tabela `documento`
--

INSERT INTO `documento` (`idDocumento`, `Curso_idCurso`, `tituloDocumento`, `descricaoDocumento`, `requisitoDocumento`, `preenchimentoDocumento`, `posicaoDocumento`, `autorDocumento`, `caminhoDocumento`, `tiposRequisitos`) VALUES
(1, 1, 'Declaração de Opção pela Área de Estágio', 'No Description Available', 'Nome-Do-Aluno,Registro-Academico,Periodo,Turno,Telefone-Fixo,Celular,Email,Empresa,Data,DDD-Telefone,DDD-Celular', '<h4 class=\'h-requisito\'>Aluno (a):</h4> <p class=\'auto-fill\'>{{Nome-Do-Aluno}} </p><h4 class=\'h-requisito\'>RA:</h4> <p class=\'auto-fill\'>{{Registro-Academico}}</p><h4 class=\'h-requisito\'>Período: </h4><p class=\'auto-fill\'>{{Periodo}}</p> <h4 class=\'h-requisito\'>Turno:</h4><p class=\'auto-fill\'>{{Turno}}</p><h4 class=\'h-requisito\'>Telefone Fixo:</h4> <p class=\'auto-fill\'>({{DDD-Telefone}}) {{Telefone}}</p> <h4 class=\'h-requisito\'>Telefone Celular:</h4> <p class=\'auto-fill\'>({{DDD-Celular}}) {{Celular}}</p><h4 class=\'h-requisito\'>E-mail:</h4> <p class=\'auto-fill\'>{{Email}}</p><h4 class=\'h-requisito\'>Empresa:</h4> <p class=\'auto-fill\'>{{Empresa}}</p>CARGA HORÁRIA OBRIGATÓRIA DE ESTÁGIO – 100 HORAS.', 1, 'FacisaBH', '/_documents/ADS/declaracao.pdf', '');

-- --------------------------------------------------------

--
-- Estrutura da tabela `requisito`
--

DROP TABLE IF EXISTS `requisito`;
CREATE TABLE IF NOT EXISTS `requisito` (
  `idRequisito` int(11) NOT NULL AUTO_INCREMENT,
  `Usuario_idUsuario` int(11) NOT NULL,
  `NomeAluno` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `RA` varchar(10) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Turno` varchar(24) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Turma` varchar(24) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Periodo` varchar(24) COLLATE utf8_unicode_ci DEFAULT NULL,
  `EscolhaArea` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `NomeSupervisor` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Telefone` varchar(24) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Celular` varchar(24) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Email` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `NomeEmpresa` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `CursoMatriculado` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `CPF` varchar(24) COLLATE utf8_unicode_ci DEFAULT NULL,
  `RG` varchar(24) COLLATE utf8_unicode_ci DEFAULT NULL,
  `DDDTel` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  `DDDCel` varchar(16) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`idRequisito`),
  KEY `Usuario_idUsuario` (`Usuario_idUsuario`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Extraindo dados da tabela `requisito`
--

INSERT INTO `requisito` (`idRequisito`, `Usuario_idUsuario`, `NomeAluno`, `RA`, `Turno`, `Turma`, `Periodo`, `EscolhaArea`, `NomeSupervisor`, `Telefone`, `Celular`, `Email`, `NomeEmpresa`, `CursoMatriculado`, `CPF`, `RG`, `DDDTel`, `DDDCel`) VALUES
(1, 1, 'Jackson Pires Ramalho', '15566', 'Noite', 'ADS3', '3°Periodo', 'Desenvolvimento de Software', 'Guilherme Neubaner', NULL, '99239-4685', 'jack-ten@hotmail.com', 'BlackDev', 'Análise e Desenvolvimento de Sistemas', '125.123.586-74', '15-735872', NULL, '31');

-- --------------------------------------------------------

--
-- Estrutura da tabela `requisitodedocumento`
--

DROP TABLE IF EXISTS `requisitodedocumento`;
CREATE TABLE IF NOT EXISTS `requisitodedocumento` (
  `documento_idDocumento` int(11) DEFAULT NULL,
  `requisitos_idRequisito` int(11) DEFAULT NULL,
  `ordemRequisito` tinyint(4) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Extraindo dados da tabela `requisitodedocumento`
--

INSERT INTO `requisitodedocumento` (`documento_idDocumento`, `requisitos_idRequisito`, `ordemRequisito`) VALUES
(1, 1, NULL),
(1, 2, NULL),
(1, 3, NULL),
(1, 6, NULL),
(1, 7, NULL),
(1, 8, NULL),
(1, 9, NULL),
(1, 10, NULL),
(1, 12, NULL),
(1, 13, NULL);

-- --------------------------------------------------------

--
-- Estrutura da tabela `requisitodeusuario`
--

DROP TABLE IF EXISTS `requisitodeusuario`;
CREATE TABLE IF NOT EXISTS `requisitodeusuario` (
  `usuario_idUsuario` int(11) DEFAULT NULL,
  `requisitos_idRequisito` int(11) DEFAULT NULL,
  `ordemRequisito` tinyint(4) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Estrutura da tabela `requisitos`
--

DROP TABLE IF EXISTS `requisitos`;
CREATE TABLE IF NOT EXISTS `requisitos` (
  `idRequisito` int(11) NOT NULL AUTO_INCREMENT,
  `NomeRequisito` varchar(150) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Dados` varchar(500) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Descricao` varchar(150) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Opcoes` text COLLATE utf8_unicode_ci,
  `Tipo` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `tag` enum('input','select','textarea') COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`idRequisito`)
) ENGINE=MyISAM AUTO_INCREMENT=16 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Extraindo dados da tabela `requisitos`
--

INSERT INTO `requisitos` (`idRequisito`, `NomeRequisito`, `Dados`, `Descricao`, `Opcoes`, `Tipo`, `tag`) VALUES
(1, 'Nome-Do-Aluno', NULL, NULL, NULL, 'text', 'input'),
(2, 'Registro-Academico', NULL, NULL, NULL, 'number', 'input'),
(3, 'Email', NULL, NULL, NULL, 'text', 'input'),
(4, 'Turma', NULL, NULL, NULL, 'text', 'input'),
(5, 'Escolha-de-Area', NULL, 'Escolha da área de estágio', 'Análise e Desenvolvimento de Software,Gerenciamento e desenvolvimento de banco de dados,Adiministração de redes de computadores, Gestão de recursos de informática,\r\nOutra/Especificar:', 'checkbox', 'input'),
(6, 'Turno-do-Curso', '', 'Turno em que realiza o curso.', 'Manhã,Tarde,Noite', 'checkbox', 'select'),
(7, 'Telefone', '', 'Telefone do aluno', '', 'text', 'input'),
(8, 'DDD-do-Telefone', '', 'Insira o DDD para o Telefone', '', 'number', 'input'),
(9, 'Celular', '', 'Insira o numero de Celular do aluno', '', 'text', 'input'),
(10, 'DDD-do-Celular', '', 'Insira o DDD do Celular', '', 'number', 'input'),
(11, 'Nome-do-Supervisor', '', 'Insira o nome do Supervisor de Estágio do aluno', '', 'text', 'input'),
(12, 'Nome-da-Empresa', '', 'Insira o nome da Empresa em que o aluno realizará o estágio assistido.', '', 'text', 'input'),
(13, 'Período-do-Curso', '', 'Escolha o Período em que o aluno realliza o curso.', '1°,2°,3°,4°,5°,6°,7°,8°', 'checkbox', 'select'),
(14, 'RG-do-Aluno', '', 'Insira o numero de Registro Geral (identidade) do aluno', '', 'text', 'input'),
(15, 'CPF', '', 'Insira o CPF do aluno', '', 'text', 'input');

-- --------------------------------------------------------

--
-- Estrutura da tabela `usuario`
--

DROP TABLE IF EXISTS `usuario`;
CREATE TABLE IF NOT EXISTS `usuario` (
  `idUsuario` int(11) NOT NULL,
  `nomeUsuario` varchar(45) COLLATE utf8_unicode_ci DEFAULT NULL,
  `emailUsuario` varchar(45) COLLATE utf8_unicode_ci DEFAULT NULL,
  `senhaUsuario` varchar(45) COLLATE utf8_unicode_ci DEFAULT NULL,
  `dataCriacaoUltimoDocumento` date DEFAULT NULL,
  `Documento_idDocumento` int(11) NOT NULL,
  PRIMARY KEY (`idUsuario`),
  KEY `fk_Usuario_Documento1_idx` (`Documento_idDocumento`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Extraindo dados da tabela `usuario`
--

INSERT INTO `usuario` (`idUsuario`, `nomeUsuario`, `emailUsuario`, `senhaUsuario`, `dataCriacaoUltimoDocumento`, `Documento_idDocumento`) VALUES
(1, 'Metalefs', 'jack-ten@hotmail.com', 'jp123321', NULL, 1);

--
-- Constraints for dumped tables
--

--
-- Limitadores para a tabela `curso`
--
ALTER TABLE `curso`
  ADD CONSTRAINT `fk_Curso_Documento` FOREIGN KEY (`Documento_idDocumento`) REFERENCES `documento` (`idDocumento`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Limitadores para a tabela `requisito`
--
ALTER TABLE `requisito`
  ADD CONSTRAINT `Requisito_ibfk_1` FOREIGN KEY (`Usuario_idUsuario`) REFERENCES `usuario` (`idUsuario`);

--
-- Limitadores para a tabela `usuario`
--
ALTER TABLE `usuario`
  ADD CONSTRAINT `fk_Usuario_Documento1` FOREIGN KEY (`Documento_idDocumento`) REFERENCES `documento` (`idDocumento`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
