-- Create database
CREATE DATABASE `driver_analytics`;
USE `driver_analytics`;

-- Users table matching Group5F25.API.Models.User
CREATE TABLE IF NOT EXISTS `Users` (
  `UserId` INT NOT NULL AUTO_INCREMENT,
  `FirstName` VARCHAR(100) NOT NULL DEFAULT '',
  `LastName` VARCHAR(100) NOT NULL DEFAULT '',
  `Email` VARCHAR(320) NOT NULL DEFAULT '',
  `PasswordHash` VARCHAR(512) NOT NULL DEFAULT '',
  `CreatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `UX_Users_Email` (`Email`)
) ENGINE=InnoDB
  DEFAULT CHARSET = utf8mb4
  COLLATE = utf8mb4_general_ci;

  