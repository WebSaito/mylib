<?php

	if(isset($_COOKIE[CKKY_N])){

		$consulta = $pdo->query("select * from pr_clientes where id=". $oCri->RCID6($_COOKIE[CKKY_N], CKKY) );
		$cliente = $consulta->fetch();
		
	}
	
?>
<nav class="navbar navbar-default navbar-static-top" >
  <div class="container">
	<div class="navbar-header">
	  <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
		<span class="sr-only">Toggle navigation</span>
		<span class="icon-bar"></span>
		<span class="icon-bar"></span>
		<span class="icon-bar"></span>
	  </button>
	  <a class="navbar-brand" href="/">Portal Relax</a>
	</div><!--/.navbar-header-->
	<div id="navbar" class="navbar-collapse collapse">
	  <ul class="nav navbar-nav navbar-right">
		<li class="dropdown">
			<a href="#" class="dropdown-toggle" data-toggle="dropdown"><i class="glyphicon glyphicon-user"></i>&nbsp;<?php echo $cliente["nome"]?> <span class="caret"></span></a>
			<ul class="dropdown-menu">
				<li><a href="/clientes/meus-dados.php">Meus Dados</a></li>
				<li><a href="/clientes/sair.php">Sair</a></li>
			</ul>
		</li>
		<li><a href="/clientes/anuncios.php"><i></i> Meus Anúncios</a></li>
	  </ul>
	</div><!--/.nav-collapse -->
  </div><!--/.container-->
</nav>