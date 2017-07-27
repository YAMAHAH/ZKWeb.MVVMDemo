/*
SQLyog Ultimate v10.00 Beta1
MySQL - 5.7.17-log : Database - zkwebdb
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`zkwebdb` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `zkwebdb`;

/* Procedure structure for procedure `getManyTreeNodes` */

/*!50003 DROP PROCEDURE IF EXISTS  `getManyTreeNodes` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `getManyTreeNodes`(pTable VARCHAR(36),pkey VARCHAR(36),pNodeId longtext,pRootId longtext)
BEGIN
SET @exeSql=CONCAT("SELECT node.* FROM ",pTable ," AS node,", pTable," AS parent" ,
		" WHERE node.lft BETWEEN parent.lft AND parent.rgt", " AND parent." , pkey ," in ( ",pNodeId, " ) And node.RootId in ( ",pRootId,
		" ) ORDER BY node.lft ");    
    PREPARE stmt FROM @exeSql;  
    EXECUTE stmt;  
    DEALLOCATE PREPARE stmt;
    END */$$
DELIMITER ;

/* Procedure structure for procedure `getTreeNodePath` */

/*!50003 DROP PROCEDURE IF EXISTS  `getTreeNodePath` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `getTreeNodePath`(pTable varchar(36),pKey varchar(36),pNodeId varchar(36))
BEGIN
    SET @exeSql=CONCAT("SELECT parent.* ", 
		       " FROM ",pTable ," AS node,", pTable," AS parent", 
		       " WHERE node.lft BETWEEN parent.lft AND parent.rgt AND node.",pKey, "= ",pNodeId,
		       " ORDER BY parent.lft ");              
    PREPARE stmt FROM @exeSql;  
    EXECUTE stmt;  
    DEALLOCATE PREPARE stmt; 
END */$$
DELIMITER ;

/* Procedure structure for procedure `getTreeNodes` */

/*!50003 DROP PROCEDURE IF EXISTS  `getTreeNodes` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `getTreeNodes`(pTable varchar(36),pkey varchar(36),pNodeId varchar(36),pRootId varchar(36))
BEGIN
    SET @exeSql=CONCAT("SELECT node.* FROM ",pTable ," AS node,", pTable," AS parent" ,
		" WHERE node.lft BETWEEN parent.lft AND parent.rgt", " AND parent." , pkey ,"=",pNodeId, " And node.RootId=",pRootId,
		" ORDER BY node.lft ");    
    PREPARE stmt FROM @exeSql;  
    EXECUTE stmt;  
    DEALLOCATE PREPARE stmt;
END */$$
DELIMITER ;

/* Procedure structure for procedure `getTreeNodesByLevel` */

/*!50003 DROP PROCEDURE IF EXISTS  `getTreeNodesByLevel` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `getTreeNodesByLevel`(pTable varchar(36),pNodeId varchar(36),pRootId varchar(36),pLevel int)
BEGIN
    SET @exeSql=CONCAT("SELECT node.* , parent.Level AS plevel",
			" FROM " ,pTable," AS node,",PTable ," AS parent", 
			" WHERE node.lft BETWEEN parent.lft AND parent.rgt AND ",
			" parent.Id = ",pNodeId ," AND node.RootId = ",pRootId ,"AND ",
		        " node.Level < parent.Level + ",pLevel,
			" ORDER BY node.lft");   
    PREPARE stmt FROM @exeSql;  
    EXECUTE stmt;  
    DEALLOCATE PREPARE stmt; 	
END */$$
DELIMITER ;

/* Procedure structure for procedure `getTreeNodesByVersion` */

/*!50003 DROP PROCEDURE IF EXISTS  `getTreeNodesByVersion` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `getTreeNodesByVersion`(pNodeVersionId varchar(36))
BEGIN
	DECLARE nodeIds longtext;
	DECLARE rootIds longtext;
	SELECT GROUP_CONCAT(a.Id) into nodeIds
				FROM zkweb_bom AS a 
				WHERE a.NodeVersionId = pNodeVersionId AND a.ParentId IS NOT NULL;
				
	SELECT GROUP_CONCAT(b.RootId) INTO rootIds FROM (
	SELECT DISTINCT a.`RootId` AS RootId 
	FROM zkweb_bom AS a 
	WHERE a.NodeVersionId = pNodeVersionId AND a.ParentId IS NOT NULL) AS b;
	
	SELECT DISTINCT node.* 
	FROM zkweb_bom AS node, zkweb_bom AS parent 
	WHERE node.lft BETWEEN parent.lft AND parent.rgt AND 
		find_in_set(parent.Id,nodeIds) AND  
		find_in_set(node.RootId,rootIds)
	ORDER BY node.lft;
    END */$$
DELIMITER ;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
