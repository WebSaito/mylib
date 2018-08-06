<?php
/*
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);
*/

//Configurações
require_once("../config/constantes.php");
require_once("../config/db.php");

//Namespaces / Classes
include '../classes/websaito-crypto.php';


//Instanciando objetos da página
$oCri = new wsCripto();

//Variaveis da página
$formErros = "";

//LOAD

	//Verificação se sessão
	
	
	//Instanciando DB
	$cx = new PDO("mysql:host=". DB1[0] .";dbname=". DB1[1] .";charset=utf8", DB1[2], DB1[3]);
	
	if($_REQUEST["acao"] == "enviar" ){
		
		//Validação
		if(trim($_REQUEST["cmpNome"]) == ""){
			$formErros .= "|Nome:";
		}
		
		if(trim($_REQUEST["cmpEmail"]) == ""){
			$formErros .= "|Email:";
		}
		
		if(trim($_REQUEST["cmpCelDDD"]) == ""){
			$formErros .= "|DDD:";
		}
		
		if(trim($_REQUEST["cmpCelular"]) == ""){
			$formErros .= "|Celular:";
		}
		
		if(trim($_REQUEST["cmpSenha"]) == ""){
			$formErros .= "|Senha:";
		}else{
			if(trim($_REQUEST["cmpSenha"]) != trim($_REQUEST["cmpSenha2"])){
				$formErros .= "|Senha:Senha e confirmação de senha estão diferentes";
			}
		}
		
		
		
		if($formErros == ""){
			
			//Normal 
			//$cx->exec("insert into pr_clientes() values()");
			
			//Show de bola
			$campos = "email, nome, celddd, cel, secret, dataCadastro, ultimoAcesso";
			
			$stm = $cx->prepare("insert into pr_clientes(". $campos .") values (:". str_replace(", "," ,:",$campos) .")");
			
			$stm->execute(array(
				":email" => $_REQUEST["cmpEmail"],
				":nome" => $_REQUEST["cmpNome"],
				":celddd" => $_REQUEST["cmpCelDDD"],
				":cel" => $_REQUEST["cmpCelular"],
				":secret" => $oCri->cenha($_REQUEST["cmpSenha"]),
				":dataCadastro" => date("Y-m-d H:i:s"),
				":ultimoAcesso" => date("Y-m-d H:i:s")
			));
			
			$id = $cx->lastInsertId();
			//Autenticar usuario
			
			
		}
		
	}

//FIM LOAD

?>
<!DOCTYPE HTML>
<html lang="en-US">
<head>
	<?php include '../includes/metas.php';?>
	<?php include '../includes/bootstrap.php'; ?>
	
	<title>Área do Anunciante | Portal Relax</title>
	
	<?php include '../includes/head.php';?>
	
	<?php include '../includes/js.php'; ?>
	<?php include '../includes/css.php'; ?>
</head>
<body>
	
	<!--?php include '../includes/topo.php'; ?-->
	
	<!-- Begin page content -->
	<div class="container">
		
		<form class="form-signin" action="?acao=enviar" method="POST">
			
			<h2 class="form-signin-heading">Cadastro de Anunciante</h2>
			
			<div class="form-group">
				<label for="cmpEmail">E-mail</label>
				<input type="email" id="cmpEmail" name="cmpEmail" class="form-control" placeholder="E-mail" required autofocus>
			</div>
			
			<div class="form-group">
				<label for="cmpNome">Nome</label>
				<input type="text" id="cmpNome" name="cmpNome" class="form-control" placeholder="Nome" required>
			</div>
			
			<div class="form-group">
				<label for="cmpCelular">Celular</label>
				<div class="row">
					<div class="col-xs-3">
						<input type="text" id="cmpCelDDD" name="cmpCelDDD" class="form-control" placeholder="DDD" maxlength="2" required>
					</div>
					<div class="col-xs-6">
						<input type="text" id="cmpCelular" name="cmpCelular" class="form-control" placeholder="Celular" maxlength="15" required>
					</div>
					<div class="col-xs-3">
					</div>
				</div>
			</div>
			
			<div class="form-group">
				<label for="cmpSenha">Senha</label>
				<input type="password" id="cmpSenha" name="cmpSenha" class="form-control" placeholder="Senha" maxlength="20" required>
			</div>
			
			
			<div class="form-group">
				<label for="cmpSenha2">Confirmação de Senha</label>
				<input type="password" id="cmpSenha2" name="cmpSenha2" class="form-control" placeholder="Confirmação de Senha" maxlength="20" required>
			</div>
			
			
			<button class="btn btn-lg btn-primary btn-block" type="submit">Enviar</button>
			
		</form>
		
	</div>

	<!--?php include '../includes/rodape.php';?-->
</body>
</html>