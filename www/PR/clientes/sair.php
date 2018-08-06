<?php
//Configurações
require_once("../config/constantes.php");
require_once("../config/db.php");

//Namespaces / Classes
include '../classes/websaito-crypto-2.php';


//Instanciando objetos da pÃ¡gina
$oCri = new wsCripto();


//LOAD

	//Verificando se ja esta logado
	try{
		unset($_COOKIE[CKKY_N]);
		setcookie(CKKY_N,"",time()-1);
		header("Location:http://portalrelax.com/clientes/");
		exit();
	}catch(\Exception $ex){
		echo $ex;
	}
	
	
	
//FIM LOAD

?>
