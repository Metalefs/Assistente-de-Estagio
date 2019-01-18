-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: u2019_estg
-- ------------------------------------------------------
-- Server version	8.0.13

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `curso`
--

DROP TABLE IF EXISTS `curso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `curso` (
  `idCurso` int(11) NOT NULL,
  `nomeCurso` varchar(150) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT 'Curso Padrão',
  `descricaoCurso` text CHARACTER SET utf8 COLLATE utf8_unicode_ci,
  `coordenadorCurso` varchar(150) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `faculdadeCurso` varchar(150) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT 'FacisaBH',
  `documento_idDocumento` int(11) NOT NULL,
  PRIMARY KEY (`idCurso`,`documento_idDocumento`),
  KEY `fk_curso_documento1_idx` (`documento_idDocumento`),
  CONSTRAINT `fk_curso_documento1` FOREIGN KEY (`documento_idDocumento`) REFERENCES `documento` (`iddocumento`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `curso`
--

LOCK TABLES `curso` WRITE;
/*!40000 ALTER TABLE `curso` DISABLE KEYS */;
INSERT INTO `curso` VALUES (1,'Análise e Desenvolvimento de Sistemas','Curso de técnicas de Analise, Engeharia e Desenvolvimento de Sistemas','André Rodrigues','FacisaBH',1);
/*!40000 ALTER TABLE `curso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `documento`
--

DROP TABLE IF EXISTS `documento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `documento` (
  `IdDocumento` int(11) NOT NULL,
  `Curso_idCurso` int(11) NOT NULL,
  `tituloDocumento` varchar(150) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `descricaoDocumento` varchar(300) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `requisitoDocumento` varchar(150) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `preenchimentoDocumento` text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `posicaoDocumento` int(11) NOT NULL,
  `autorDocumento` varchar(45) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL DEFAULT 'FacisaBH',
  `caminhoDocumento` varchar(150) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `tiposRequisitos` varchar(150) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`IdDocumento`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `documento`
--

LOCK TABLES `documento` WRITE;
/*!40000 ALTER TABLE `documento` DISABLE KEYS */;
INSERT INTO `documento` VALUES (1,1,'Declaração de Opção pela Área de Estágio','No Description Available','Nome-Do-Aluno,Registro-Academico,Periodo,Turno,Telefone-Fixo,Celular,Email,Empresa,Data,DDD-Telefone,DDD-Celular','<h4 class=\'h-requisito\'>Aluno (a):</h4> <p class=\'auto-fill\'>{{Nome-Do-Aluno}} </p><h4 class=\'h-requisito\'>RA:</h4> <p class=\'auto-fill\'>{{Registro-Academico}}</p><h4 class=\'h-requisito\'>Período: </h4><p class=\'auto-fill\'>{{Periodo}}</p> <h4 class=\'h-requisito\'>Turno:</h4><p class=\'auto-fill\'>{{Turno}}</p><h4 class=\'h-requisito\'>Telefone Fixo:</h4> <p class=\'auto-fill\'>({{DDD-Telefone}}) {{Telefone}}</p> <h4 class=\'h-requisito\'>Telefone Celular:</h4> <p class=\'auto-fill\'>({{DDD-Celular}}) {{Celular}}</p><h4 class=\'h-requisito\'>E-mail:</h4> <p class=\'auto-fill\'>{{Email}}</p><h4 class=\'h-requisito\'>Empresa:</h4> <p class=\'auto-fill\'>{{Empresa}}</p>CARGA HORÁRIA OBRIGATÓRIA DE ESTÁGIO – 100 HORAS.',1,'FacisaBH','/_documents/ADS/declaracao.pdf','');
/*!40000 ALTER TABLE `documento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `requisitodedocumento`
--

DROP TABLE IF EXISTS `requisitodedocumento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `requisitodedocumento` (
  `documento_idDocumento` int(11) DEFAULT NULL,
  `ordemRequisito` tinyint(4) DEFAULT NULL,
  `requisitos_idRequisito` int(11) NOT NULL,
  KEY `fk_requisitodedocumento_requisitos1_idx` (`requisitos_idRequisito`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `requisitodedocumento`
--

LOCK TABLES `requisitodedocumento` WRITE;
/*!40000 ALTER TABLE `requisitodedocumento` DISABLE KEYS */;
INSERT INTO `requisitodedocumento` VALUES (1,1,0),(1,2,0),(1,3,0),(1,6,0),(1,7,0),(1,8,0),(1,9,0),(1,10,0),(1,12,0),(1,13,0);
/*!40000 ALTER TABLE `requisitodedocumento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `requisitodeusuario`
--

DROP TABLE IF EXISTS `requisitodeusuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `requisitodeusuario` (
  `usuario_idUsuario` int(11) DEFAULT NULL,
  `ordemRequisito` tinyint(4) DEFAULT NULL,
  `requisitos_idRequisito` int(11) NOT NULL,
  KEY `fk_requisitodeusuario_requisitos1_idx` (`requisitos_idRequisito`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `requisitodeusuario`
--

LOCK TABLES `requisitodeusuario` WRITE;
/*!40000 ALTER TABLE `requisitodeusuario` DISABLE KEYS */;
/*!40000 ALTER TABLE `requisitodeusuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `requisitos`
--

DROP TABLE IF EXISTS `requisitos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `requisitos` (
  `idRequisito` int(11) NOT NULL AUTO_INCREMENT,
  `NomeRequisito` varchar(150) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `Dados` varchar(500) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `Descricao` varchar(150) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `Opcoes` text CHARACTER SET utf8 COLLATE utf8_unicode_ci,
  `Tipo` varchar(50) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL,
  `tag` enum('input','select','textarea') CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`idRequisito`)
) ENGINE=MyISAM AUTO_INCREMENT=16 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `requisitos`
--

LOCK TABLES `requisitos` WRITE;
/*!40000 ALTER TABLE `requisitos` DISABLE KEYS */;
INSERT INTO `requisitos` VALUES (1,'Nome-Do-Aluno',NULL,NULL,NULL,'text','input'),(2,'Registro-Academico',NULL,NULL,NULL,'number','input'),(3,'Email',NULL,NULL,NULL,'text','input'),(4,'Turma',NULL,NULL,NULL,'text','input'),(5,'Escolha-de-Area',NULL,'Escolha da área de estágio','Análise e Desenvolvimento de Software,Gerenciamento e desenvolvimento de banco de dados,Adiministração de redes de computadores, Gestão de recursos de informática,\r\nOutra/Especificar:','checkbox','input'),(6,'Turno-do-Curso','','Turno em que realiza o curso.','Manhã,Tarde,Noite','checkbox','input'),(7,'Telefone','','Telefone do aluno','','text','input'),(8,'DDD-do-Telefone','','Insira o DDD para o Telefone','','number','input'),(9,'Celular','','Insira o numero de Celular do aluno','','text','input'),(10,'DDD-do-Celular','','Insira o DDD do Celular','','number','input'),(11,'Nome-do-Supervisor','','Insira o nome do Supervisor de Estágio do aluno','','text','input'),(12,'Nome-da-Empresa','','Insira o nome da Empresa em que o aluno realizará o estágio assistido.','','text','input'),(13,'Período-do-Curso','','Escolha o Período em que o aluno realliza o curso.','1°,2°,3°,4°,5°,6°,7°,8°','checkbox','select'),(14,'RG-do-Aluno','','Insira o numero de Registro Geral (identidade) do aluno','','text','input'),(15,'CPF','','Insira o CPF do aluno','','text','input');
/*!40000 ALTER TABLE `requisitos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `usuario` (
  `idUsuario` int(11) NOT NULL,
  `nomeUsuario` varchar(45) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `emailUsuario` varchar(45) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `senhaUsuario` varchar(45) CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT NULL,
  `dataCriacaoUltimoDocumento` date DEFAULT NULL,
  `Documento_idDocumento` int(11) NOT NULL,
  PRIMARY KEY (`idUsuario`),
  KEY `fk_Usuario_Documento1_idx` (`Documento_idDocumento`),
  CONSTRAINT `fk_Usuario_Documento1` FOREIGN KEY (`Documento_idDocumento`) REFERENCES `documento` (`iddocumento`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (1,'Metalefs','jack-ten@hotmail.com','jp123321',NULL,1);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'u2019_estg'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-01-17 19:01:38
