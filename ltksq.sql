/*
SQLyog 企业版 - MySQL GUI v8.14 
MySQL - 5.6.14 : Database - ltksq_db
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`ltksq_db` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `ltksq_db`;

/*Table structure for table `c_vote` */

DROP TABLE IF EXISTS `c_vote`;

CREATE TABLE `c_vote` (
  `voteid` int(11) NOT NULL AUTO_INCREMENT,
  `uid` int(11) DEFAULT NULL COMMENT '发起用户',
  `votetitle` varchar(100) DEFAULT NULL COMMENT '标题',
  `votetype` smallint(6) DEFAULT '0' COMMENT '0,单选,1多选',
  `votestatus` smallint(6) DEFAULT '0' COMMENT '0暂存,1发布,2关闭,3下线',
  `createtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `lasttime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `paywkcamount` double DEFAULT '0' COMMENT '支付的wkc',
  `enddate` date DEFAULT NULL COMMENT '结束日期',
  `remark` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`voteid`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM AUTO_INCREMENT=24 DEFAULT CHARSET=utf8;

/*Table structure for table `c_voteitem` */

DROP TABLE IF EXISTS `c_voteitem`;

CREATE TABLE `c_voteitem` (
  `svoteid` bigint(20) NOT NULL AUTO_INCREMENT,
  `voteid` int(11) DEFAULT NULL,
  `title` varchar(50) DEFAULT NULL COMMENT '投票项',
  `createtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `lasttime` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`svoteid`),
  KEY `voteid` (`voteid`)
) ENGINE=MyISAM AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

/*Table structure for table `c_voteuser` */

DROP TABLE IF EXISTS `c_voteuser`;

CREATE TABLE `c_voteuser` (
  `usvoteid` bigint(20) NOT NULL AUTO_INCREMENT,
  `voteid` int(11) DEFAULT NULL,
  `svoteid` bigint(20) DEFAULT NULL,
  `uid` bigint(20) DEFAULT NULL,
  `wkcaddress` varchar(100) DEFAULT NULL COMMENT '钱包地址',
  `wkcamount` double DEFAULT '0' COMMENT '钱包余额',
  `createtime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `lasttime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `onecloudnum` int(11) DEFAULT NULL COMMENT '钱包对应玩客云数量',
  `paywkcamount` double DEFAULT '0' COMMENT '支付的wkc',
  `getamountok` smallint(6) DEFAULT '0' COMMENT '0未获取余额,1已获取',
  PRIMARY KEY (`usvoteid`),
  KEY `voteid` (`voteid`,`svoteid`,`uid`)
) ENGINE=MyISAM AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

/*Table structure for table `jz_userdetail` */

DROP TABLE IF EXISTS `jz_userdetail`;

CREATE TABLE `jz_userdetail` (
  `zjuid` int(11) NOT NULL AUTO_INCREMENT,
  `uid` int(11) NOT NULL DEFAULT '0' COMMENT 'uid',
  `fmoney` double DEFAULT '0' COMMENT '金额',
  `remark` varchar(200) DEFAULT NULL COMMENT '备注',
  `createtime` datetime DEFAULT NULL COMMENT '时间',
  PRIMARY KEY (`zjuid`),
  KEY `uid` (`uid`)
) ENGINE=MyISAM AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

/*Table structure for table `m_msgnoc` */

DROP TABLE IF EXISTS `m_msgnoc`;

CREATE TABLE `m_msgnoc` (
  `msgid` int(11) NOT NULL AUTO_INCREMENT,
  `msgcode` varchar(6) DEFAULT NULL COMMENT '验证码',
  `stopdatetime` datetime DEFAULT NULL COMMENT '有效时间',
  `mobileno` varchar(20) DEFAULT NULL COMMENT '手机号',
  PRIMARY KEY (`msgid`)
) ENGINE=MyISAM AUTO_INCREMENT=7991 DEFAULT CHARSET=utf8;

/*Table structure for table `s_syspar` */

DROP TABLE IF EXISTS `s_syspar`;

CREATE TABLE `s_syspar` (
  `keyname` varchar(30) NOT NULL COMMENT 'par key',
  `keyvalue` varchar(1000) DEFAULT NULL COMMENT '参数值',
  `keydes` text COMMENT '描述',
  `datachange_lasttime` timestamp(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) ON UPDATE CURRENT_TIMESTAMP(3) COMMENT '更新时间',
  PRIMARY KEY (`keyname`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

/*Table structure for table `u_account` */

DROP TABLE IF EXISTS `u_account`;

CREATE TABLE `u_account` (
  `uid` int(11) NOT NULL DEFAULT '0' COMMENT '用户主键ID',
  `accountmony` double NOT NULL DEFAULT '0' COMMENT '余额',
  `stopmoney` double NOT NULL DEFAULT '0' COMMENT '冻结金额',
  `datachange_lasttime` timestamp(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) ON UPDATE CURRENT_TIMESTAMP(3) COMMENT '更新时间',
  PRIMARY KEY (`uid`),
  KEY `ix_DataChange_LastTime` (`datachange_lasttime`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户资产账户信息表';

/*Table structure for table `u_changedetail` */

DROP TABLE IF EXISTS `u_changedetail`;

CREATE TABLE `u_changedetail` (
  `uid` int(11) NOT NULL DEFAULT '0' COMMENT '用户id',
  `fmoney` double NOT NULL DEFAULT '0' COMMENT '金额',
  `ftype` smallint(6) NOT NULL DEFAULT '0' COMMENT '类型',
  `datachange_lasttime` timestamp(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) ON UPDATE CURRENT_TIMESTAMP(3) COMMENT '更新时间',
  `detid` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'pk',
  `remark` varchar(200) DEFAULT NULL COMMENT '备注',
  PRIMARY KEY (`detid`),
  KEY `ix_DataChange_LastTime` (`datachange_lasttime`),
  KEY `uid` (`uid`),
  KEY `ftype` (`ftype`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='账户明细';

/*Table structure for table `u_importmoney` */

DROP TABLE IF EXISTS `u_importmoney`;

CREATE TABLE `u_importmoney` (
  `inmid` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'pk',
  `wkcaddress` varchar(100) NOT NULL COMMENT '钱包地址',
  `thash` varchar(100) NOT NULL COMMENT '交易hash',
  `indatetime` datetime NOT NULL COMMENT '时间',
  `uid` int(11) NOT NULL DEFAULT '0' COMMENT '充值用户',
  `fmoney` double DEFAULT '0' COMMENT '充值金额',
  PRIMARY KEY (`inmid`),
  KEY `wkcaddress` (`wkcaddress`,`thash`)
) ENGINE=MyISAM AUTO_INCREMENT=11891 DEFAULT CHARSET=utf8;

/*Table structure for table `u_info` */

DROP TABLE IF EXISTS `u_info`;

CREATE TABLE `u_info` (
  `uid` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户主键',
  `nikename` varchar(20) DEFAULT NULL COMMENT '昵称',
  `userid` varchar(20) NOT NULL DEFAULT '' COMMENT '用户登录账号(手机号)',
  `lastcheckcode` varchar(4) DEFAULT NULL COMMENT '最后手机验证码',
  `lastchecktime` datetime DEFAULT '2018-01-01 00:00:00' COMMENT '验证码最后时间',
  `userpwd` varchar(64) NOT NULL DEFAULT '' COMMENT '登录密码',
  `changepwd` varchar(64) DEFAULT NULL COMMENT '交易密码',
  `wkcaddress` varchar(100) NOT NULL DEFAULT '' COMMENT '链克钱包地址',
  `datachange_lasttime` timestamp(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3) ON UPDATE CURRENT_TIMESTAMP(3) COMMENT '更新时间',
  `usertype` smallint(6) NOT NULL DEFAULT '0' COMMENT '类型,0是用户,1是系统,2是测试',
  `regdatetime` datetime DEFAULT NULL COMMENT '注册时间',
  `countrycode` varchar(8) DEFAULT '''86''' COMMENT '国家手机区号',
  `wkccheckpass` tinyint(1) DEFAULT '0' COMMENT '钱包验证通过',
  `wkccheckmoney` double DEFAULT '0.1' COMMENT '验证wkc数量',
  `puid` int(11) DEFAULT '0' COMMENT '推荐人id',
  `isvip` smallint(6) DEFAULT '0',
  `vipdate` date DEFAULT NULL,
  PRIMARY KEY (`uid`),
  KEY `ix_DataChange_LastTime` (`datachange_lasttime`)
) ENGINE=MyISAM AUTO_INCREMENT=4008 DEFAULT CHARSET=utf8 COMMENT='用户信息表';

/* Procedure structure for procedure `accountPay` */

/*!50003 DROP PROCEDURE IF EXISTS  `accountPay` */;

DELIMITER $$

/*!50003 CREATE DEFINER=`root`@`localhost` PROCEDURE `accountPay`(IN _uid INTEGER ,
    IN fmoney DOUBLE,IN aftype INTEGER)
BEGIN
    
     
     DECLARE price DOUBLE;
 
     
 SET price=0;
 
SELECT   accountmony INTO price FROM u_account  WHERE uid=_uid;
 
IF price>=fmoney  
THEN
UPDATE u_account SET accountmony=accountmony-fmoney WHERE  uid=_uid;
INSERT INTO u_changedetail (uid, fmoney, ftype) VALUES (_uid, -fmoney,aftype);
SELECT 1;
 
ELSE
SELECT 0;
END IF;
    END */$$
DELIMITER ;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
