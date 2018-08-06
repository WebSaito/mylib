<?php

//Configurações
require_once("../config/constantes.php");
require_once("../config/db.php");

//Namespaces / Classes
include '../classes/websaito-crypto-2.php';
include '../classes/websaito-forms.php';


//Instanciando objetos da página
$oCri = new wsCripto();
$oForm = new wsForms();

//Variaveis da pÃ¡gina
$formErros = "";

//LOAD
	
	//Verificação se sessÃ£o
	
	//Instanciando DB
	$pdo = new PDO(DB1[4], DB1[2], DB1[3], $DBopt);
	
	if($_REQUEST["acao"] == "enviar" ){
		
		//ValidaÃ§Ã£o
		if(trim($_REQUEST["cmpNome"]) == ""){
			$formErros .= "|Nome:";
		}
		
		if(trim($_REQUEST["cmpEmail"]) == ""){
			$formErros .= "|Email:";
		}else{
			//Verificar se existe alguma conta já cadastrada com este email
			
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
			//$pdo->exec("insert into pr_clientes() values()");
			
			//Show de bola
			$campos = "email, nome, celddd, cel, secret, dataCadastro, ultimoAcesso";
			
			$stm = $pdo->prepare("insert into pr_clientes(". $campos .") values (:". str_replace(", "," ,:",$campos) .")");
			
			$stm->execute(array(
				":email" => $_REQUEST["cmpEmail"],
				":nome" => $_REQUEST["cmpNome"],
				":celddd" => $_REQUEST["cmpCelDDD"],
				":cel" => $_REQUEST["cmpCelular"],
				":secret" => $oCri->cenha($_REQUEST["cmpSenha"]),
				":dataCadastro" => date("Y-m-d H:i:s"),
				":ultimoAcesso" => date("Y-m-d H:i:s")
			));
			
			$id = $pdo->lastInsertId();
			//Autenticar usuario
			
			setcookie(CKKY_N,  $oCri->CID6($id ,CKKY));
			header("Location: http://portalrelax.com/clientes/anuncios.php"); /* Redirect browser */
			exit();
		}
		
	}

//FIM LOAD

?>
<!DOCTYPE HTML>
<html lang="pt-BR">
<head>
	<?php include '../includes/metas.php';?>
	<?php include '../includes/bootstrap.php'; ?>
	
	<title>Área do Anunciante | Portal Relax</title>
	
	<?php include '../includes/head.php';?>
	
	<?php include '../includes/js.php'; ?>
	<?php include '../includes/css.php'; ?>
</head>
<body>
	
	<?php include '../includes/topo-clientes.php'; ?>
	
	<img src="/_img/bg-clean-2.jpg" class="bg"/>
	
	<!-- Begin page content -->
	<div class="container container-fluid container-table">
		
		
		<form class="quadro-login" action="?acao=enviar" method="POST">
			
			<div class="row">
				<div class="col-md-6 col-sm-12 col-xs-12 text-muted">
					<h2 class="text-muted">Cadastro de Anunciante</h2>
			
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
					
					<div class="form-group">
						<div class="row">
							<div class="pull-right col-md-4 col-sm-12 col-xs-12">
								<button class="btn btn-warning btn-block pull-right" type="submit">Enviar</button>
							</div>
						</div>
					</div>
					
				</div><!--/.col-md6-->
				
				
				<div class="col-md-6 col-sm-12 col-xs-12 text-muted">
					<h4 class="display-4">Portal Relax</h4>
					<p>	
						Ao realizar seu cadastro de anunciante, você poderá publicar anúncios.<br/>
						Mantenha sempre seus anúncios atualizados.
					</p>
				</div>
				
				
			</div><!--/.row-->
			
			<div style="clear:both;"></div>
		</form>
		
	</div>

	<!--?php include '../includes/rodape.php';?-->
</body>
</html>