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
				<div class="col-md-6 col-sm-12 col-xs-12 text-muted">
					<h2 class="text-muted">Novo Anúncio</h2>
			
					<div class="form-group">
						<label for="cmpTitulo">Título do anúncio</label>
						<input type="text" id="cmpTitulo" name="cmpTitulo" class="form-control" placeholder="Digite aqui o título" required autofocus>
					</div>
				
			
					<div class="form-group">
						<label for="cmpDescricao">Descrição do anúncio</label>
						<textarea id="cmpDescricao" name="cmpDescricao" class="form-control" placeholder="Coloque aqui a descrição do seu anúncio" rows="5" required></textarea>
					</div>
					
					<div class="form-group">
						<label for="cmpBairro">Bairro (Opcional)</label>
						<input type="text" id="cmpBairro" name="cmpBairro" class="form-control" placeholder="Bairro" required>
					</div>
					
					<div class="form-group">
						<label for="cmpCelular">Cidade / Estado</label>
						<div class="row">
							<div class="col-xs-8">
								<input type="text" id="cmpCidade" name="cmpCidade" class="form-control" placeholder="Cidade" maxlength="2" required>
							</div>
							<div class="col-xs-4">
								<select id="cmpUF" name="cmpUF" class="form-control" required>
									<option value="">Estado</option>
									<option value="AC">AC</option>
									<option value="AL">AL</option>
									<option value="AM">AM</option>
									<option value="AP">AP</option>
									<option value="BA">BA</option>
									<option value="CE">CE</option>
									<option value="DF">DF</option>
									<option value="ES">ES</option>
									<option value="GO">GO</option>
									<option value="MA">MA</option>
									<option value="MG">MG</option>
									<option value="MT">MT</option>
									<option value="MS">MS</option>
									<option value="PA">PA</option>
									<option value="PB">PB</option>
									<option value="PE">PE</option>
									<option value="PI">PI</option>
									<option value="PR">PR</option>
									<option value="RJ">RJ</option>
									<option value="RN">RN</option>
									<option value="RO">RO</option>
									<option value="RR">RR</option>
									<option value="RS">RS</option>
									<option value="SC">SC</option>
									<option value="SE">SE</option>
									<option value="SP">SP</option>
									<option value="TO">TO</option>
								</select>
							</div>
						</div>
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
							<div class="form-check col-xs-3">
								<label class="form-check-label">
								<input type="checkbox" class="form-check-input" id="cmpWhatsapp" name="cmpWhatsapp">
								WhatsApp
								</label>
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