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

//Variaveis da página
$formErros = "";

//LOAD
	
	//Verificação de sessao logada
	if(!isset($_COOKIE[CKKY_N])){
		header("Location: http://portalrelax.com/clientes/"); /* Redirect browser */
		exit();
	}
	
	
	//Instanciando DB
	$pdo = new PDO(DB1[4], DB1[2], DB1[3], $DBopt);
	
	//Carregando dados
	

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
				
				<h1>Meus anúncios</h1> <a href="anuncios-novo.php" class="btn btn-primary pull-right">Incluir Novo Anúncio</a>
				
				<div style="height:20px; clear:both; display:block;"></div>
				
				<table class="table">
					<thead class="thead-inverse">
						<th>Código</th>
						<th>Título</th>
						<th>Status</th>
						<th>Mensagens</th>
						<th>Validade</th>
						<th>&nbsp;</th>
					</thead>
					<?php 
					try{
					foreach($pdo->query("select * from pr_anuncios where idCliente="& $oCri->RCID6($_COOKIE[CKKY_N] ,CKKY)) as $linha){
					?>	
					<tr>
						<td><?php echo $linha["codigo"];?></td>
						<td><?php echo $linha["titulo"];?></td>
						<td><?php echo $linha["status"];?></td>
						<td><?php echo $linha["id"];?></td>
						<td><?php echo $linha["validade"];?></td>
						<td>
							<a href="anuncio-detalhe.php?k=<?php echo $oCri->CID6($linha["id"],CKKY);?>" class="btn btn-default btn-sm">detalhes</a>
							<a href="anuncio-editar.php?k=<?php echo $oCri->CID6($linha["id"],CKKY);?>" class="btn btn-default btn-sm">editar</a>
						</td>
					</tr>
					<?php
					}
					}
					catch(Exception $ex)
					{
					?>
					<tr>
						<td colspan="6">Você ainda não possui nenhum anúncio</td>
					</tr>
					<?php
					}
					?>
					
					
				</table>
				
			</div><!--/.row-->
			
			<div style="clear:both;"></div>
		</form>
		
	</div>

	<!--?php include '../includes/rodape.php';?-->
</body>
</html>