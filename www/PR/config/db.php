<?php
	//
	define("DB1", ["localhost", "portalre_01", "portalre_admin", "relax0000", "mysql:host=localhost;dbname=portalre_01;charset=utf8" ]);
	
	$DBopt = [
		PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION,
		PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
		PDO::ATTR_EMULATE_PREPARES   => false,
	];
?>