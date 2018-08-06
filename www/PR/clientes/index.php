<?php
//Configurações
require_once("../config/constantes.php");
require_once("../config/db.php");

//Namespaces / Classes
include '../classes/websaito-crypto-2.php';


//Instanciando objetos da pÃ¡gina
$oCri = new wsCripto();


//LOAD
$prosseguir = false;
	
	//Verificando se ja esta logado
	if(isset($_COOKIE[CKKY_N])){
		header("Location:http://portalrelax.com/clientes/anuncios.php");
		exit();
	}
	
	//Login
	if($_REQUEST["acao"] == "entrar" ){
		
		$pdo = new PDO(DB1[4], DB1[2], DB1[3], $DBopt);
		
		$consulta = $pdo->query("select * from pr_clientes where email='". $_REQUEST["cmpEmail"] ."' and excluido=0 and disponivel=1");
		$linha = $consulta->fetch();
		//
		if( $linha["secret"] == $oCri->cenha($_REQUEST["cmpSenha"])){
			$prosseguir = true;
			
			setcookie(CKKY_N, $oCri->CID6($linha["id"] ,CKKY));
		}
		
		
		if( $prosseguir == true){
			
			//Gravando log de acesso
			$sistema = get_browser(null, true);
			$pdo->exec("insert into pr_clientes_logs(idCliente, ip, dataAcesso, navegador, navegador_v, os) values(". $linha["id"] .", '". get_client_ip() ."', '". date("Y-m-d H:i:s") ."', '". $sistema["browser"] ."', '". $sistema["version"] ."', '". $sistema["platform"] ."')");
			
			header("Location: http://portalrelax.com/clientes/anuncios.php"); /* Redirect browser */
			exit();
		}else{
			
			echo "S ". $linha["secret"] ." / SENHA ".  $oCri->cenha($_REQUEST["cmpSenha"]);
			
		}
		
	}
	//Fim login
	
	
	//------------------------
	//Métodos auxiliares
	
	//Método get_client_ip
	function get_client_ip() {
		$ipaddress = '';
		if (getenv('HTTP_CLIENT_IP'))
			$ipaddress = getenv('HTTP_CLIENT_IP');
		else if(getenv('HTTP_X_FORWARDED_FOR'))
			$ipaddress = getenv('HTTP_X_FORWARDED_FOR');
		else if(getenv('HTTP_X_FORWARDED'))
			$ipaddress = getenv('HTTP_X_FORWARDED');
		else if(getenv('HTTP_FORWARDED_FOR'))
			$ipaddress = getenv('HTTP_FORWARDED_FOR');
		else if(getenv('HTTP_FORWARDED'))
		   $ipaddress = getenv('HTTP_FORWARDED');
		else if(getenv('REMOTE_ADDR'))
			$ipaddress = getenv('REMOTE_ADDR');
		else
			$ipaddress = 'UNKNOWN';
		return $ipaddress;
	}
	
//FIM LOAD

?>
<!DOCTYPE HTML>
<html lang="pt-BR">
<head>
	<?php include '../includes/metas.php';?>
	<?php include '../includes/bootstrap.php'; ?>
	
	<title>Área do Anunciante | <?php echo WEBTITLE;?></title>
	
	<?php include '../includes/head.php';?>
	
	<?php include '../includes/js.php'; ?>
	<?php include '../includes/css.php'; ?>
</head>
<body class="body-login">
	
	<!--?php include '../includes/topo.php'; ?-->
	
	<!-- Begin page content -->
	
	<img src="/_img/bg-clean-2.jpg" class="bg"/>
	
	<div class="container container-table container-fluid">
		
		<div class="row vertical-center-row">
		
			<div class="col-md-6 col-sm-12 col-xs-12 col-md-offset-3 col-sm-offset-0 col-xs-offset-0">
				
				<form class=" quadro-login" action="?acao=entrar" method="POST">
					
					<div class="row">
						<div class="col-md-6 col-sm-12 text-muted">
							<img src="/_img/logo-2-200x40.png" border="0"/>
						</div>
						<div class="col-md-6 col-sm-12">
							<h4 class="pull-right hidden-sm hidden-xs">Área do Anunciante</h4>
							<h4 class="pull-left visible-sm visible-xs">Área do Anunciante</h5>
						</div>
					</div>
					
					<div style="height:35px;display:block;">
					</div>
					
					<label for="cmpEmail" class="sr-only">E-mail</label>
					<input type="email" id="cmpEmail" name="cmpEmail" class="form-control input-so-bottom" placeholder="E-mail" required autofocus>
					
					<div style="height:15px;display:block;">
					</div>
					
					<label for="cmpSenha" class="sr-only">Senha</label>
					<input type="password" id="cmpSenha" name="cmpSenha" class="form-control input-so-bottom" placeholder="Senha" required>
					
					
					<div class="checkbox pull-right">
					  <label>
						<input type="checkbox" value="lembrar-me"> Lembrar-me
					  </label>
					</div>
					
					<div style="height:15px;display:block;clear:both;">
					</div>
					
					<div class="row">
						<div class="col-md-9 col-sm-12">
							Ainda não tem cadastro? <a href="novo-cadastro.php">Clique Aqui</a>
						</div>
						<div class="col-md-3 col-sm-12">
							<button class="btn btn-lg btn-primary btn-block" type="submit">Entrar</button>
						</div>
					</div>
				</form>
				
			</div>
			
		</div>
		
	</div><!--/end container-->

	<!--?php include '../includes/rodape.php';?-->
</body>
</html>