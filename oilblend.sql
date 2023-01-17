-- MySQL dump 10.13  Distrib 8.0.30, for Win64 (x86_64)
--
-- Host: localhost    Database: oilblend
-- ------------------------------------------------------
-- Server version	8.0.30

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `compoilconfig`
--

DROP TABLE IF EXISTS `compoilconfig`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compoilconfig` (
  `id` int NOT NULL AUTO_INCREMENT,
  `comOilName` varchar(128) DEFAULT NULL,
  `cet` float DEFAULT NULL,
  `d50` float DEFAULT NULL,
  `pol` float DEFAULT NULL,
  `den` float DEFAULT NULL,
  `price` float DEFAULT NULL,
  `autoLow1` float DEFAULT NULL,
  `autoHigh1` float DEFAULT NULL,
  `expLow1` float DEFAULT NULL,
  `expHigh1` float DEFAULT NULL,
  `autoLow2` float DEFAULT NULL,
  `autoHigh2` float DEFAULT NULL,
  `expLow2` float DEFAULT NULL,
  `expHigh2` float DEFAULT NULL,
  `iniVolume` float DEFAULT NULL,
  `highVolume` float DEFAULT NULL,
  `lowVolume` float DEFAULT NULL,
  `planProduct1` float DEFAULT NULL,
  `planProduct2` float DEFAULT NULL,
  `planProduct3` float DEFAULT NULL,
  `planProduct4` float DEFAULT NULL,
  `planProduct5` float DEFAULT NULL,
  `planProduct6` float DEFAULT NULL,
  `planProduct7` float DEFAULT NULL,
  `prod1Low1` float DEFAULT NULL,
  `prod1High1` float DEFAULT NULL,
  `prod2Low1` float DEFAULT NULL,
  `prod2High1` float DEFAULT NULL,
  `prod1Low2` float DEFAULT NULL,
  `prod1High2` float DEFAULT NULL,
  `prod2Low2` float DEFAULT NULL,
  `prod2High2` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `compoilconfig`
--

LOCK TABLES `compoilconfig` WRITE;
/*!40000 ALTER TABLE `compoilconfig` DISABLE KEYS */;
INSERT INTO `compoilconfig` VALUES (1,'6#加氢柴油',42,280,1,842,5000,0,5000,0,5000,0,5000,0,5000,200,8000,200,2000,2000,2000,2000,2000,2000,2000,0,5000,0,5000,0,5000,0,5000),(2,'7#加氢柴油',47,300,4,842,5000,0,5000,0,5000,0,5000,0,5000,200,8000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000),(3,'3#加氢柴油',44,290,3,842,5500,0,5000,0,5000,0,5000,0,5000,200,8000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000),(4,'VI柴油加氢',45,280,4,837,6550,0,5000,0,5000,0,5000,0,5000,200,8000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000),(5,'航空煤油',47,300,5,847,1000,0,6000,0,5000,0,5000,0,5000,200,8000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000),(6,'1#加氢减一线',40,300,1,842,1000,0,1000,0,5000,0,5000,0,5000,200,8000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000),(7,'1#加氢减二线',49,300,7,842,1000,0,5000,0,5000,0,5000,0,5000,200,8000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000);
/*!40000 ALTER TABLE `compoilconfig` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `compoilconfig_gas`
--

DROP TABLE IF EXISTS `compoilconfig_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `compoilconfig_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `comOilName` varchar(128) DEFAULT NULL,
  `ron` float DEFAULT NULL,
  `t50` float DEFAULT NULL,
  `suf` float DEFAULT NULL,
  `den` float DEFAULT NULL,
  `price` float DEFAULT NULL,
  `gas92Low1` float DEFAULT NULL,
  `gas92High1` float DEFAULT NULL,
  `gas95Low1` float DEFAULT NULL,
  `gas95High1` float DEFAULT NULL,
  `gas92Low2` float DEFAULT NULL,
  `gas92High2` float DEFAULT NULL,
  `gas95Low2` float DEFAULT NULL,
  `gas95High2` float DEFAULT NULL,
  `iniVolume` float DEFAULT NULL,
  `highVolume` float DEFAULT NULL,
  `lowVolume` float DEFAULT NULL,
  `planProduct1` float DEFAULT NULL,
  `planProduct2` float DEFAULT NULL,
  `planProduct3` float DEFAULT NULL,
  `planProduct4` float DEFAULT NULL,
  `planProduct5` float DEFAULT NULL,
  `planProduct6` float DEFAULT NULL,
  `planProduct7` float DEFAULT NULL,
  `gas98Low1` float DEFAULT NULL,
  `gas98High1` float DEFAULT NULL,
  `gasSelfLow1` float DEFAULT NULL,
  `gasSelfHigh1` float DEFAULT NULL,
  `gas98Low2` float DEFAULT NULL,
  `gas98High2` float DEFAULT NULL,
  `gasSelfLow2` float DEFAULT NULL,
  `gasSelfHigh2` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `compoilconfig_gas`
--

LOCK TABLES `compoilconfig_gas` WRITE;
/*!40000 ALTER TABLE `compoilconfig_gas` DISABLE KEYS */;
INSERT INTO `compoilconfig_gas` VALUES (1,'重整汽油',100,121,0.1,760,5000,0,5000,0,5000,0,5000,0,5000,200,10000,200,2000,2000,2000,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000),(2,'MTBE',116,50,3,727,5000,0,5000,0,5000,0,5000,0,5000,200,10000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000),(3,'非芳汽油',84,100,0.2,689,5500,0,5000,0,5000,0,5000,0,5000,200,10000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000),(4,'催化汽油',92,82,4,736,6550,0,5000,0,5000,0,5000,0,5000,200,10000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000),(5,'C5',86,68,0.3,732,2000,0,6000,0,5000,0,5000,0,5000,200,10000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000),(6,'烷基化油',96,104,1.2,692,5200,0,5000,0,5000,0,5000,0,5000,200,10000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000),(7,'加氢裂化石脑油',85.6,54,1,661,4800,0,5000,0,5000,0,5000,0,5000,200,10000,200,2500,2500,2500,2500,2500,2500,2500,0,5000,0,5000,0,5000,0,5000);
/*!40000 ALTER TABLE `compoilconfig_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dispatchweight`
--

DROP TABLE IF EXISTS `dispatchweight`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dispatchweight` (
  `id` int NOT NULL AUTO_INCREMENT,
  `weightName` varchar(128) DEFAULT NULL,
  `weight` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dispatchweight`
--

LOCK TABLES `dispatchweight` WRITE;
/*!40000 ALTER TABLE `dispatchweight` DISABLE KEYS */;
INSERT INTO `dispatchweight` VALUES (1,'调度周期',7),(2,'车用柴油产量最大权值',1),(3,'出口柴油十六烷值卡边权值',1),(4,'出口柴油产量最大权值',0),(5,'调度方案切换权值',1),(6,'成品油个数',2);
/*!40000 ALTER TABLE `dispatchweight` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dispatchweight_gas`
--

DROP TABLE IF EXISTS `dispatchweight_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dispatchweight_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `weightName` varchar(128) DEFAULT NULL,
  `weight` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dispatchweight_gas`
--

LOCK TABLES `dispatchweight_gas` WRITE;
/*!40000 ALTER TABLE `dispatchweight_gas` DISABLE KEYS */;
INSERT INTO `dispatchweight_gas` VALUES (1,'调度周期',7),(2,'92#汽油产量最大权值',1),(3,'95#汽油辛烷值卡边权值',1),(4,'95#汽油产量最大权值',0),(5,'调度方案切换权值',1),(6,'成品油个数',2);
/*!40000 ALTER TABLE `dispatchweight_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menulist`
--

DROP TABLE IF EXISTS `menulist`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menulist` (
  `id` int NOT NULL AUTO_INCREMENT,
  `menuName` varchar(128) DEFAULT NULL,
  `icon` varchar(128) DEFAULT NULL,
  `path` varchar(128) DEFAULT NULL,
  `component` varchar(128) DEFAULT NULL,
  `childID` varchar(128) DEFAULT NULL,
  `parentID` varchar(128) DEFAULT NULL,
  `menuState` varchar(128) DEFAULT NULL,
  `menuCode` varchar(128) DEFAULT NULL,
  `menuType` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menulist`
--

LOCK TABLES `menulist` WRITE;
/*!40000 ALTER TABLE `menulist` DISABLE KEYS */;
INSERT INTO `menulist` VALUES (1,'方案配置','CreditCard','/schemeconfig/schemeTab','0','0','0','0','0','2'),(5,'方案验证','Histogram','/schemetest/blendcalc','0','0','0','0','0','2'),(8,'配方优化','List','/formulaopti/formulacalc','0','0','0','0','0','2'),(12,'智能决策','Guide','/intelligentdec','0','0','0','0','0','1'),(13,'参数设置','None','/intelligentdec/parameter','/intelligentdec/parameter','12','104','1','0','1'),(14,'决策方案','None','/intelligentdec/decisionsche','/intelligentdec/decisionsche','12','104','1','0','1'),(15,'系统管理','Stamp','/system','0','0','0','0','0','1'),(16,'用户管理','Apple','/system/decisionsche','/schemeconfig/user','15','105','1','0','1'),(17,'角色管理','Apple','/system/role','/schemeconfig/user','15','105','1','0','1'),(18,'菜单权限','Apple','/system/log','/schemeconfig/user','15','105','1','0','1'),(19,'日志管理','Apple','/system/menupermi','/system/menupermi','15','105','1','0','1'),(20,'测试','Apple','/system/test','/system/test','15','105','1','0','1'),(21,'首页','House','/welcome','0','0','0','0','0','2');
/*!40000 ALTER TABLE `menulist` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menulist_gas`
--

DROP TABLE IF EXISTS `menulist_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menulist_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `menuName` varchar(128) DEFAULT NULL,
  `icon` varchar(128) DEFAULT NULL,
  `path` varchar(128) DEFAULT NULL,
  `component` varchar(128) DEFAULT NULL,
  `childID` varchar(128) DEFAULT NULL,
  `parentID` varchar(128) DEFAULT NULL,
  `menuState` varchar(128) DEFAULT NULL,
  `menuCode` varchar(128) DEFAULT NULL,
  `menuType` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menulist_gas`
--

LOCK TABLES `menulist_gas` WRITE;
/*!40000 ALTER TABLE `menulist_gas` DISABLE KEYS */;
INSERT INTO `menulist_gas` VALUES (1,'首页','House','/patrolWelcome','0','0','0','0','0','2'),(2,'方案配置','CreditCard','/schemeconfigp/schemeTabp','0','0','0','0','0','2'),(5,'方案验证','Histogram','/schemetestp/blendcalcp','0','0','0','0','0','2'),(6,'配方优化','List','/formulaoptip/formulacalcp','0','0','0','0','0','2'),(12,'智能决策','Guide','/intelligentdecp','0','0','0','0','0','1'),(13,'参数设置','None','/intelligentdecp/parameterp','/intelligentdecp/parameterp','12','104','1','0','1'),(14,'决策方案','None','/intelligentdecp/decisioncalcp','/intelligentdecp/decisioncalcp','12','104','1','0','1'),(15,'系统管理','Stamp','/system','0','0','0','0','0','1'),(16,'用户管理','Apple','/system/decisionsche','/schemeconfig/user','15','105','1','0','1'),(17,'角色管理','Apple','/system/role','/schemeconfig/user','15','105','1','0','1'),(18,'菜单权限','Apple','/system/log','/schemeconfig/user','15','105','1','0','1'),(19,'日志管理','Apple','/system/menupermi','/system/menupermi','15','105','1','0','1'),(20,'测试','Apple','/system/test','/system/test','15','105','1','0','1');
/*!40000 ALTER TABLE `menulist_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `prodoilconfig`
--

DROP TABLE IF EXISTS `prodoilconfig`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prodoilconfig` (
  `id` int NOT NULL AUTO_INCREMENT,
  `prodOilName` varchar(128) DEFAULT NULL,
  `cetLowLimit` float DEFAULT NULL,
  `cetHighLimit` float DEFAULT NULL,
  `d50LowLimit` float DEFAULT NULL,
  `d50HighLimit` float DEFAULT NULL,
  `polLowLimit` float DEFAULT NULL,
  `polHighLimit` float DEFAULT NULL,
  `denLowLimit` float DEFAULT NULL,
  `denHighLimit` float DEFAULT NULL,
  `iniVolume` float DEFAULT NULL,
  `prodVolumeLowLimit` float DEFAULT NULL,
  `prodVolumeHighLimit` float DEFAULT NULL,
  `demand1LowLimit` float DEFAULT NULL,
  `demand1HighLimit` float DEFAULT NULL,
  `demand2LowLimit` float DEFAULT NULL,
  `demand2HighLimit` float DEFAULT NULL,
  `demand3LowLimit` float DEFAULT NULL,
  `demand3HighLimit` float DEFAULT NULL,
  `demand4LowLimit` float DEFAULT NULL,
  `demand4HighLimit` float DEFAULT NULL,
  `demand5LowLimit` float DEFAULT NULL,
  `demand5HighLimit` float DEFAULT NULL,
  `demand6LowLimit` float DEFAULT NULL,
  `demand6HighLimit` float DEFAULT NULL,
  `demand7LowLimit` float DEFAULT NULL,
  `demand7HighLimit` float DEFAULT NULL,
  `Apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prodoilconfig`
--

LOCK TABLES `prodoilconfig` WRITE;
/*!40000 ALTER TABLE `prodoilconfig` DISABLE KEYS */;
INSERT INTO `prodoilconfig` VALUES (1,'车用柴油',47,60,255,300,1,7,830,870,200,200,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,1),(2,'出口柴油',43,60,250,300,1,5,830,870,200,200,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,1),(3,'备用成品油1',43,60,250,300,1,7,830,870,200,200,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,1),(4,'备用成品油2',43,60,250,300,1,7,830,870,200,200,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,2000,10000,0);
/*!40000 ALTER TABLE `prodoilconfig` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `prodoilconfig_gas`
--

DROP TABLE IF EXISTS `prodoilconfig_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prodoilconfig_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `prodOilName` varchar(128) DEFAULT NULL,
  `ronLowLimit` float DEFAULT NULL,
  `ronHighLimit` float DEFAULT NULL,
  `t50LowLimit` float DEFAULT NULL,
  `t50HighLimit` float DEFAULT NULL,
  `sufLowLimit` float DEFAULT NULL,
  `sufHighLimit` float DEFAULT NULL,
  `denLowLimit` float DEFAULT NULL,
  `denHighLimit` float DEFAULT NULL,
  `iniVolume` float DEFAULT NULL,
  `prodVolumeLowLimit` float DEFAULT NULL,
  `prodVolumeHighLimit` float DEFAULT NULL,
  `demand1LowLimit` float DEFAULT NULL,
  `demand1HighLimit` float DEFAULT NULL,
  `demand2LowLimit` float DEFAULT NULL,
  `demand2HighLimit` float DEFAULT NULL,
  `demand3LowLimit` float DEFAULT NULL,
  `demand3HighLimit` float DEFAULT NULL,
  `demand4LowLimit` float DEFAULT NULL,
  `demand4HighLimit` float DEFAULT NULL,
  `demand5LowLimit` float DEFAULT NULL,
  `demand5HighLimit` float DEFAULT NULL,
  `demand6LowLimit` float DEFAULT NULL,
  `demand6HighLimit` float DEFAULT NULL,
  `demand7LowLimit` float DEFAULT NULL,
  `demand7HighLimit` float DEFAULT NULL,
  `Apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prodoilconfig_gas`
--

LOCK TABLES `prodoilconfig_gas` WRITE;
/*!40000 ALTER TABLE `prodoilconfig_gas` DISABLE KEYS */;
INSERT INTO `prodoilconfig_gas` VALUES (1,'92#汽油',92,93,50,110,0,5,700,775,200,200,12000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,1),(2,'95#汽油',95,96,50,110,0,5,700,775,200,200,12000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,1),(3,'98#汽油',98,99,50,110,0,5,700,775,200,200,10000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,0),(4,'自定义牌号汽油',90,100,50,110,0,5,700,775,200,200,10000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,8000,10000,0);
/*!40000 ALTER TABLE `prodoilconfig_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `property`
--

DROP TABLE IF EXISTS `property`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `property` (
  `id` int NOT NULL AUTO_INCREMENT,
  `propertyName` varchar(128) DEFAULT NULL,
  `apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `property`
--

LOCK TABLES `property` WRITE;
/*!40000 ALTER TABLE `property` DISABLE KEYS */;
INSERT INTO `property` VALUES (1,'十六烷值指数',1),(2,'50%回收温度（℃）',1),(3,'多环芳烃含量（wt%）',1),(4,'密度（kg/m³）',1);
/*!40000 ALTER TABLE `property` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `property_gas`
--

DROP TABLE IF EXISTS `property_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `property_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `propertyName` varchar(128) DEFAULT NULL,
  `apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `property_gas`
--

LOCK TABLES `property_gas` WRITE;
/*!40000 ALTER TABLE `property_gas` DISABLE KEYS */;
INSERT INTO `property_gas` VALUES (1,'研究法辛烷值（RON）',1),(2,'50%蒸发温度（℃）',1),(3,'硫含量（mg/kg）',1),(4,'密度（kg/m³）',1);
/*!40000 ALTER TABLE `property_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipecalc1`
--

DROP TABLE IF EXISTS `recipecalc1`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipecalc1` (
  `id` int NOT NULL AUTO_INCREMENT,
  `comOilName` varchar(128) DEFAULT NULL,
  `comOilProductHigh` float DEFAULT NULL,
  `comOilProductLow` float DEFAULT NULL,
  `autoFlowHigh` float DEFAULT NULL,
  `autoFlowLow` float DEFAULT NULL,
  `expFlowHigh` float DEFAULT NULL,
  `expFlowLow` float DEFAULT NULL,
  `prod1FlowHigh` float DEFAULT NULL,
  `prod1FlowLow` float DEFAULT NULL,
  `prod2FlowHigh` float DEFAULT NULL,
  `prod2FlowLow` float DEFAULT NULL,
  `iniVolume` float DEFAULT NULL,
  `volumeHigh` float DEFAULT NULL,
  `volumeLow` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipecalc1`
--

LOCK TABLES `recipecalc1` WRITE;
/*!40000 ALTER TABLE `recipecalc1` DISABLE KEYS */;
INSERT INTO `recipecalc1` VALUES (1,'6#加氢柴油',5000,5000,50,0,50,0,50,0,50,0,200,8000,100),(2,'7#加氢柴油',4000,4000,45,0,45,0,45,0,45,0,200,8000,100),(3,'3#加氢柴油',5000,5000,45,0,45,0,45,0,45,0,200,8000,100),(4,'VI柴油加氢',5000,5000,40,0,40,0,40,0,40,0,200,8000,100),(5,'航空煤油',3000,3000,40,0,40,0,40,0,40,0,200,8000,100),(6,'1#加氢减一线',6000,6000,40,0,40,0,40,0,40,0,200,8000,100),(7,'1#加氢减二线',5000,5000,35,0,35,0,35,0,35,0,200,8000,100);
/*!40000 ALTER TABLE `recipecalc1` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipecalc1_gas`
--

DROP TABLE IF EXISTS `recipecalc1_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipecalc1_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `comOilName` varchar(128) DEFAULT NULL,
  `comOilProductHigh` float DEFAULT NULL,
  `comOilProductLow` float DEFAULT NULL,
  `gas92FlowHigh` float DEFAULT NULL,
  `gas92FlowLow` float DEFAULT NULL,
  `gas95FlowHigh` float DEFAULT NULL,
  `gas95FlowLow` float DEFAULT NULL,
  `gas98FlowHigh` float DEFAULT NULL,
  `gas98FlowLow` float DEFAULT NULL,
  `gasSelfFlowHigh` float DEFAULT NULL,
  `gasSelfFlowLow` float DEFAULT NULL,
  `iniVolume` float DEFAULT NULL,
  `volumeHigh` float DEFAULT NULL,
  `volumeLow` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipecalc1_gas`
--

LOCK TABLES `recipecalc1_gas` WRITE;
/*!40000 ALTER TABLE `recipecalc1_gas` DISABLE KEYS */;
INSERT INTO `recipecalc1_gas` VALUES (1,'重整汽油',5000,1000,45,0,45,0,44,0,46,0,200,8000,100),(2,'MTBE',4000,2000,46,0,46,0,42,0,45,0,200,8000,100),(3,'非芳汽油',3000,1000,48,0,49,0,46,0,48,0,200,8000,100),(4,'催化汽油',3000,1000,50,0,52,0,48,0,47,0,200,8000,100),(5,'C5',3000,1000,43,0,51,0,36,0,52,0,200,8000,100),(6,'烷基化油',6000,1000,40,0,48,0,38,0,30,0,200,8000,100),(7,'加氢裂化石脑油',2000,1000,42,0,39,0,39,0,60,0,200,8000,100);
/*!40000 ALTER TABLE `recipecalc1_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipecalc2`
--

DROP TABLE IF EXISTS `recipecalc2`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipecalc2` (
  `id` int NOT NULL AUTO_INCREMENT,
  `weightName` varchar(128) DEFAULT NULL,
  `weight` float DEFAULT NULL,
  `ProdOilStatus` int DEFAULT NULL,
  `Apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipecalc2`
--

LOCK TABLES `recipecalc2` WRITE;
/*!40000 ALTER TABLE `recipecalc2` DISABLE KEYS */;
INSERT INTO `recipecalc2` VALUES (1,'车柴产量最大权值',1,1,1),(2,'出柴产量最大权值',0,2,1),(3,'备用成品油1产量最大权值',0,3,1),(4,'备用成品油2产量最大权值',1,4,0),(5,'车柴十六烷值权值',0,1,1),(6,'出柴十六烷值权值',1,2,1),(7,'备用成品油1十六烷值权值',0,3,1),(8,'备用成品油2十六烷值权值',0,4,0),(9,'车柴多芳烃权值',0,1,1),(10,'出柴多芳烃权值',1,2,1),(11,'备用成品油1多芳烃权值',0,3,1),(12,'备用成品油2多芳烃权值',0,4,0);
/*!40000 ALTER TABLE `recipecalc2` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipecalc2_2`
--

DROP TABLE IF EXISTS `recipecalc2_2`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipecalc2_2` (
  `id` int NOT NULL AUTO_INCREMENT,
  `weightName` varchar(128) DEFAULT NULL,
  `weight` float DEFAULT NULL,
  `ProdOilStatus` int DEFAULT NULL,
  `Apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipecalc2_2`
--

LOCK TABLES `recipecalc2_2` WRITE;
/*!40000 ALTER TABLE `recipecalc2_2` DISABLE KEYS */;
INSERT INTO `recipecalc2_2` VALUES (1,'车柴产量最大权值',1,1,1),(2,'出柴产量最大权值',1,2,1),(3,'备用成品油1产量最大权值',0,3,1),(4,'备用成品油2产量最大权值',1,4,0),(5,'车柴十六烷值权值',0,1,1),(6,'出柴十六烷值权值',1,2,1),(7,'备用成品油1十六烷值权值',0,3,1),(8,'备用成品油2十六烷值权值',0,4,0),(9,'车柴多芳烃权值',0,1,1),(10,'出柴多芳烃权值',1,2,1),(11,'备用成品油1多芳烃权值',1,3,1),(12,'备用成品油2多芳烃权值',0,4,0);
/*!40000 ALTER TABLE `recipecalc2_2` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipecalc2_2_gas`
--

DROP TABLE IF EXISTS `recipecalc2_2_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipecalc2_2_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `weightName` varchar(128) DEFAULT NULL,
  `weight` float DEFAULT NULL,
  `ProdOilStatus` int DEFAULT NULL,
  `Apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipecalc2_2_gas`
--

LOCK TABLES `recipecalc2_2_gas` WRITE;
/*!40000 ALTER TABLE `recipecalc2_2_gas` DISABLE KEYS */;
INSERT INTO `recipecalc2_2_gas` VALUES (1,'92#汽油产量最大权值',1,1,1),(2,'95#汽油产量最大权值',0,2,1),(3,'98#汽油产量最大权值',0,3,0),(4,'自定义牌号汽油产量最大权值',0,4,0),(5,'92#汽油辛烷值卡边权值',0,1,1),(6,'95#汽油辛烷值卡边权值',1,2,1),(7,'98#汽油辛烷值卡边权值',0,3,0),(8,'自定义牌号汽油辛烷值卡边权值',0,4,0),(9,'92#汽油硫含量最小权值',0,1,1),(10,'95#汽油硫含量最小权值',1,2,1),(11,'98#汽油硫含量最小权值',0,3,0),(12,'自定义牌号汽油硫含量最小权值',0,4,0);
/*!40000 ALTER TABLE `recipecalc2_2_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipecalc2_3`
--

DROP TABLE IF EXISTS `recipecalc2_3`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipecalc2_3` (
  `id` int NOT NULL AUTO_INCREMENT,
  `weightName` varchar(128) DEFAULT NULL,
  `weight` float DEFAULT NULL,
  `ProdOilStatus` int DEFAULT NULL,
  `Apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipecalc2_3`
--

LOCK TABLES `recipecalc2_3` WRITE;
/*!40000 ALTER TABLE `recipecalc2_3` DISABLE KEYS */;
INSERT INTO `recipecalc2_3` VALUES (1,'车柴产量最大权值',1,1,1),(2,'出柴产量最大权值',0,2,1),(3,'备用成品油1产量最大权值',0,3,1),(4,'备用成品油2产量最大权值',0,4,0),(5,'车柴十六烷值权值',0,1,1),(6,'出柴十六烷值权值',0,2,1),(7,'备用成品油1十六烷值权值',0,3,1),(8,'备用成品油2十六烷值权值',1,4,0),(9,'车柴多芳烃权值',0,1,1),(10,'出柴多芳烃权值',0,2,1),(11,'备用成品油1多芳烃权值',0,3,1),(12,'备用成品油2多芳烃权值',0,4,0);
/*!40000 ALTER TABLE `recipecalc2_3` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipecalc2_3_gas`
--

DROP TABLE IF EXISTS `recipecalc2_3_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipecalc2_3_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `weightName` varchar(128) DEFAULT NULL,
  `weight` float DEFAULT NULL,
  `ProdOilStatus` int DEFAULT NULL,
  `Apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipecalc2_3_gas`
--

LOCK TABLES `recipecalc2_3_gas` WRITE;
/*!40000 ALTER TABLE `recipecalc2_3_gas` DISABLE KEYS */;
INSERT INTO `recipecalc2_3_gas` VALUES (1,'92#汽油产量最大权值',1,1,1),(2,'95#汽油产量最大权值',0,2,1),(3,'98#汽油产量最大权值',0,3,0),(4,'自定义牌号汽油产量最大权值',0,4,0),(5,'92#汽油辛烷值卡边权值',0,1,1),(6,'95#汽油辛烷值卡边权值',1,2,1),(7,'98#汽油辛烷值卡边权值',0,3,0),(8,'自定义牌号汽油辛烷值卡边权值',0,4,0),(9,'92#汽油硫含量最小权值',0,1,1),(10,'95#汽油硫含量最小权值',1,2,1),(11,'98#汽油硫含量最小权值',0,3,0),(12,'自定义牌号汽油硫含量最小权值',0,4,0);
/*!40000 ALTER TABLE `recipecalc2_3_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipecalc2_gas`
--

DROP TABLE IF EXISTS `recipecalc2_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipecalc2_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `weightName` varchar(128) DEFAULT NULL,
  `weight` float DEFAULT NULL,
  `ProdOilStatus` int DEFAULT NULL,
  `Apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipecalc2_gas`
--

LOCK TABLES `recipecalc2_gas` WRITE;
/*!40000 ALTER TABLE `recipecalc2_gas` DISABLE KEYS */;
INSERT INTO `recipecalc2_gas` VALUES (1,'92#汽油产量最大权值',1,1,1),(2,'95#汽油产量最大权值',1,2,1),(3,'98#汽油产量最大权值',0,3,0),(4,'自定义牌号汽油产量最大权值',0,4,0),(5,'92#汽油辛烷值卡边权值',1,1,1),(6,'95#汽油辛烷值卡边权值',1,2,1),(7,'98#汽油辛烷值卡边权值',0,3,0),(8,'自定义牌号汽油辛烷值卡边权值',0,4,0),(9,'92#汽油硫含量最小权值',1,1,1),(10,'95#汽油硫含量最小权值',1,2,1),(11,'98#汽油硫含量最小权值',0,3,0),(12,'自定义牌号汽油硫含量最小权值',0,4,0);
/*!40000 ALTER TABLE `recipecalc2_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipecalc3`
--

DROP TABLE IF EXISTS `recipecalc3`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipecalc3` (
  `id` int NOT NULL AUTO_INCREMENT,
  `prodOilName` varchar(128) DEFAULT NULL,
  `prodOilProduct` float DEFAULT NULL,
  `prodOilProductLow` float DEFAULT NULL,
  `totalFlow` float DEFAULT NULL,
  `totalFlow2` float DEFAULT NULL,
  `Apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipecalc3`
--

LOCK TABLES `recipecalc3` WRITE;
/*!40000 ALTER TABLE `recipecalc3` DISABLE KEYS */;
INSERT INTO `recipecalc3` VALUES (1,'车用柴油',20000,2000,20000,20000,1),(2,'出口柴油',12000,2000,16000,16000,1),(3,'备用成品油1',12000,2000,16000,16000,1),(4,'备用成品油2',10000,2000,16000,10000,0);
/*!40000 ALTER TABLE `recipecalc3` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipecalc3_gas`
--

DROP TABLE IF EXISTS `recipecalc3_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipecalc3_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `prodOilName` varchar(128) DEFAULT NULL,
  `prodOilProduct` float DEFAULT NULL,
  `totalFlow` float DEFAULT NULL,
  `totalFlow2` float DEFAULT NULL,
  `Apply` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipecalc3_gas`
--

LOCK TABLES `recipecalc3_gas` WRITE;
/*!40000 ALTER TABLE `recipecalc3_gas` DISABLE KEYS */;
INSERT INTO `recipecalc3_gas` VALUES (1,'92#汽油',5000,20000,20000,1),(2,'95#汽油',8000,16000,16000,1),(3,'98#汽油',10000,16000,16000,0),(4,'自定义牌号汽油',10000,16000,16000,0);
/*!40000 ALTER TABLE `recipecalc3_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `schemeverify1`
--

DROP TABLE IF EXISTS `schemeverify1`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `schemeverify1` (
  `id` int NOT NULL AUTO_INCREMENT,
  `comOilName` varchar(128) DEFAULT NULL,
  `autoQualityProduct` float DEFAULT NULL,
  `expQualityProduct` float DEFAULT NULL,
  `prod1QualityProduct` float DEFAULT NULL,
  `prod2QualityProduct` float DEFAULT NULL,
  `autoFlowPercentMass` float DEFAULT NULL,
  `expFlowPercentMass` float DEFAULT NULL,
  `prod1FlowPercentMass` float DEFAULT NULL,
  `prod2FlowPercentMass` float DEFAULT NULL,
  `autoFlowMass` float DEFAULT NULL,
  `expFlowMass` float DEFAULT NULL,
  `prod1FlowMass` float DEFAULT NULL,
  `prod2FlowMass` float DEFAULT NULL,
  `autoVolumeProduct` float DEFAULT NULL,
  `expVolumeProduct` float DEFAULT NULL,
  `prod1VolumeProduct` float DEFAULT NULL,
  `prod2VolumeProduct` float DEFAULT NULL,
  `autoFlowPercentVol` float DEFAULT NULL,
  `expFlowPercentVol` float DEFAULT NULL,
  `prod1FlowPercentVol` float DEFAULT NULL,
  `prod2FlowPercentVol` float DEFAULT NULL,
  `autoFlowVol` float DEFAULT NULL,
  `expFlowVol` float DEFAULT NULL,
  `prod1FlowVol` float DEFAULT NULL,
  `prod2FlowVol` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `schemeverify1`
--

LOCK TABLES `schemeverify1` WRITE;
/*!40000 ALTER TABLE `schemeverify1` DISABLE KEYS */;
INSERT INTO `schemeverify1` VALUES (1,'6#加氢柴油',22,25,22,25,15,10,15,10,30,70,30,40,15,15,15,15,16,16,16,16,10,20,10,20),(2,'7#加氢柴油',30,12,30,12,20,15,20,15,30,60,70,70,21,11,21,11,21,11,21,11,10,10,10,10),(3,'3#加氢柴油',10,15,10,15,10,20,10,20,40,50,50,50,12,21,12,21,12,21,12,21,10,20,10,20),(4,'VI柴油加氢',8,6,8,6,13,18,13,18,50,40,60,60,10,13,10,13,10,13,10,13,10,10,10,10),(5,'航空煤油',7,0,7,0,18,13,18,13,60,30,40,30,9,14,9,14,9,14,9,14,10,20,10,20),(6,'1#加氢减一线',0,0,0,0,7,9,7,9,70,30,30,30,8,5,8,5,8,5,8,5,10,10,10,10),(7,'1#加氢减二线',0,25,0,25,9,7,9,7,30,80,30,80,8,6,8,6,8,6,8,6,10,20,10,20);
/*!40000 ALTER TABLE `schemeverify1` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `schemeverify1_gas`
--

DROP TABLE IF EXISTS `schemeverify1_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `schemeverify1_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `comOilName` varchar(128) DEFAULT NULL,
  `gas92QualityProduct` float DEFAULT NULL,
  `gas95QualityProduct` float DEFAULT NULL,
  `gas98QualityProduct` float DEFAULT NULL,
  `gasSelfQualityProduct` float DEFAULT NULL,
  `gas92FlowPercentMass` float DEFAULT NULL,
  `gas95FlowPercentMass` float DEFAULT NULL,
  `gas98FlowPercentMass` float DEFAULT NULL,
  `gasSelfFlowPercentMass` float DEFAULT NULL,
  `gas92FlowMass` float DEFAULT NULL,
  `gas95FlowMass` float DEFAULT NULL,
  `gas98FlowMass` float DEFAULT NULL,
  `gasSelfFlowMass` float DEFAULT NULL,
  `gas92VolumeProduct` float DEFAULT NULL,
  `gas95VolumeProduct` float DEFAULT NULL,
  `gas98VolumeProduct` float DEFAULT NULL,
  `gasSelfVolumeProduct` float DEFAULT NULL,
  `gas92FlowPercentVol` float DEFAULT NULL,
  `gas95FlowPercentVol` float DEFAULT NULL,
  `gas98FlowPercentVol` float DEFAULT NULL,
  `gasSelfFlowPercentVol` float DEFAULT NULL,
  `gas92FlowVol` float DEFAULT NULL,
  `gas95FlowVol` float DEFAULT NULL,
  `gas98FlowVol` float DEFAULT NULL,
  `gasSelfFlowVol` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `schemeverify1_gas`
--

LOCK TABLES `schemeverify1_gas` WRITE;
/*!40000 ALTER TABLE `schemeverify1_gas` DISABLE KEYS */;
INSERT INTO `schemeverify1_gas` VALUES (1,'重整汽油',22,25,22,25,15,10,15,10,30,70,30,40,15,15,15,15,16,16,16,16,10,20,10,20),(2,'MTBE',30,12,30,12,20,15,20,15,30,60,70,70,21,11,21,11,21,11,21,11,10,10,10,10),(3,'非芳汽油',10,15,10,15,10,20,10,20,40,50,50,50,12,21,12,21,12,21,12,21,10,20,10,20),(4,'催化汽油',8,6,8,6,13,18,13,18,50,40,60,60,10,13,10,13,10,13,10,13,10,10,10,10),(5,'C5',7,0,7,0,18,13,18,13,60,30,40,30,9,14,9,14,9,14,9,14,10,20,10,20),(6,'烷基化油',0,0,0,0,7,9,7,9,70,30,30,30,8,5,8,5,8,5,8,5,10,10,10,10),(7,'加氢裂化石脑油',0,25,0,25,9,7,9,7,30,80,30,80,8,6,8,6,8,6,8,6,10,20,10,20);
/*!40000 ALTER TABLE `schemeverify1_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `schemeverify2`
--

DROP TABLE IF EXISTS `schemeverify2`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `schemeverify2` (
  `id` int NOT NULL AUTO_INCREMENT,
  `prodOilName` varchar(128) DEFAULT NULL,
  `bottomVolume` float DEFAULT NULL,
  `cetVol` float DEFAULT NULL,
  `d50Vol` float DEFAULT NULL,
  `polVol` float DEFAULT NULL,
  `denVol` float DEFAULT NULL,
  `totalBlendVol` float DEFAULT NULL,
  `totalBlendVol2` float DEFAULT NULL,
  `totalBlendVol3` float DEFAULT NULL,
  `bottomMass` float DEFAULT NULL,
  `cetMass` float DEFAULT NULL,
  `d50Mass` float DEFAULT NULL,
  `polMass` float DEFAULT NULL,
  `denMass` float DEFAULT NULL,
  `totalBlendMass` float DEFAULT NULL,
  `totalBlendMass2` float DEFAULT NULL,
  `totalBlendMass3` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `schemeverify2`
--

LOCK TABLES `schemeverify2` WRITE;
/*!40000 ALTER TABLE `schemeverify2` DISABLE KEYS */;
INSERT INTO `schemeverify2` VALUES (1,'车用柴油',10,46,280,7,820,2000,1000,4000,50,46,280,1,822,2200,1000,4000),(2,'出口柴油',20,48,285,8,825,2000,500,2000,60,48,275,7,825,2500,2000,5000),(3,'备用成品油1',20,48,285,8,825,2000,500,2000,60,48,275,7,825,2500,2000,5000),(4,'备用成品油2',20,48,285,8,825,2000,500,2000,60,48,275,7,825,2500,2000,5000);
/*!40000 ALTER TABLE `schemeverify2` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `schemeverify2_gas`
--

DROP TABLE IF EXISTS `schemeverify2_gas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `schemeverify2_gas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `prodOilName` varchar(128) DEFAULT NULL,
  `bottomVolume` float DEFAULT NULL,
  `ronVol` float DEFAULT NULL,
  `t50Vol` float DEFAULT NULL,
  `sufVol` float DEFAULT NULL,
  `denVol` float DEFAULT NULL,
  `totalBlendVol` float DEFAULT NULL,
  `totalBlendVol2` float DEFAULT NULL,
  `totalBlendVol3` float DEFAULT NULL,
  `bottomMass` float DEFAULT NULL,
  `ronMass` float DEFAULT NULL,
  `t50Mass` float DEFAULT NULL,
  `sufMass` float DEFAULT NULL,
  `denMass` float DEFAULT NULL,
  `totalBlendMass` float DEFAULT NULL,
  `totalBlendMass2` float DEFAULT NULL,
  `totalBlendMass3` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `schemeverify2_gas`
--

LOCK TABLES `schemeverify2_gas` WRITE;
/*!40000 ALTER TABLE `schemeverify2_gas` DISABLE KEYS */;
INSERT INTO `schemeverify2_gas` VALUES (1,'92#汽油',10,92,105,3,730,2000,1000,4000,50,92,106,1,725,2200,1000,4000),(2,'95#汽油',20,95,108,2,745,2000,500,2000,60,95,107,4,740,2500,2000,5000),(3,'98#汽油',20,98,110,4,738,2000,500,2000,60,98,108,3,748,2500,2000,5000),(4,'自定义牌号汽油',20,90,110,1,750,2000,500,2000,60,90,109,2,765,2500,2000,5000);
/*!40000 ALTER TABLE `schemeverify2_gas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'oilblend'
--

--
-- Dumping routines for database 'oilblend'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-01-14 20:24:39
