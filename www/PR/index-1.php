<?php
//Configurações
require_once("../config/constantes.php");
require_once("../config/db.php");

//Namespaces / Classes
include '../classes/websaito-crypto.php';


//Instanciando objetos da página
$oCri = new wsCripto();


//LOAD

	//Verificação se sessão
	

//FIM LOAD

?>
<!DOCTYPE HTML>
<html lang="en-US">
<head>
	<?php include 'includes/metas.php';?>
	
	<meta name="description" content="Encontre aqui sua massagem perfeita para relaxar em sua região. Divulgue seu serviço aqui para quem deseja anunciar." />
	<meta name="keywords" content="massagens, massagem relaxante, massagem profissional, massagem tantrica, relaxante, profissional, tantrica"/>
	
	<?php include 'includes/bootstrap.php'; ?>
	
	<title><?php echo WEBTITLE?> | Relaxe Aqui.</title>
	
	<?php include 'includes/head.php';?>
	
	<?php include 'includes/js.php'; ?>
	<?php include 'includes/css.php'; ?>
</head>
<body>
	
	<?php include 'includes/topo.php'; ?>
	
	<!-- Begin page content -->
	<div class="container">
		
		<!-- BUSCA FAST -->
		<?php include 'includes/busca-fast.php';?>
		<!-- FIM BUSCA FAST -->
		
		<!-- DESTAQUES -->
		<div class="row" style="text-align:center;margin-top:10px;">
		
				<div class="col-md-4" >
					<img class="img-circle" src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" alt="Generic placeholder image" width="140" height="140">
          <h2>Destaque 1</h2>
          <p>Donec sed odio dui. Etiam porta sem malesuada magna mollis euismod. Nullam id dolor id nibh ultricies vehicula ut id elit. Morbi leo risus, porta ac consectetur ac, vestibulum at eros. Praesent commodo cursus magna.</p>
          <p><a class="btn btn-default" href="#" role="button">View details &raquo;</a></p>
				</div>
				
				<div class="col-md-4" >
					<img class="img-circle" src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" alt="Generic placeholder image" width="140" height="140">
          <h2>Destaque 2</h2>
          <p>Donec sed odio dui. Etiam porta sem malesuada magna mollis euismod. Nullam id dolor id nibh ultricies vehicula ut id elit. Morbi leo risus, porta ac consectetur ac, vestibulum at eros. Praesent commodo cursus magna.</p>
          <p><a class="btn btn-default" href="#" role="button">View details &raquo;</a></p>
				</div>
				
				<div class="col-md-4" >
					<img class="img-circle" src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" alt="Generic placeholder image" width="140" height="140">
          <h2>Destaque 3</h2>
          <p>Donec sed odio dui. Etiam porta sem malesuada magna mollis euismod. Nullam id dolor id nibh ultricies vehicula ut id elit. Morbi leo risus, porta ac consectetur ac, vestibulum at eros. Praesent commodo cursus magna.</p>
          <p><a class="btn btn-default" href="#" role="button">View details &raquo;</a></p>
				</div>
			
		</div>
		<!-- FIM DESTAQUES -->
		
		<!-- LISTAGEM -->
		<div class="row">
			<div class="col-md-4" >
				<img class="img-responsive" src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" alt="[titulo]" />
				<h4>Titulo</h4>
				R$ 2,50
				Estado - Cidade - Bairro
			</div>
		</div>
		<!-- FIM LISTAGEM -->
		
	</div>

	<?php include 'includes/rodape.php';?>
</body>
</html>