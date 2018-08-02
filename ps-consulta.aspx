<%@ Page Language="VB" CodeFile="ps-consulta.aspx.vb" Inherits="pc" %>
<%@ OutputCache Duration="1" Location="none"%>

<!DOCTYPE HTML>
<html lang="en-US">
<head>
	<meta charset="UTF-8">
	<title>PagSeguro2 - -=[ W3BS41T0 ]=-</title>
</head>
<body>
	<form id="f1" name="f1" method="POST" action="?acao=consultarRef">
		<br>
		N. Pedido: <input type="text" id="numeroPedido" name="numeroPedido" value="<%=request("numeroPedido")%>"/><br>
		Data Inicial: <input type="text" id="dataInicial" name="dataInicial" value="<%=request("dataInicial")%>"/><br>
		Data Final: <input type="text" id="dataFinal" name="dataFinal" value="<%=request("dataFinal")%>"/><br>
		<br>
		
		<button type="submit">Consultar</button>
		
		<br>
		<br>
		<!-- Resultado -->
		<br>
		Retorno:<br>
		<%=oPS.resultado%>
		
		<!-- Exibindo resultados caso tenha mais de uma transaction -->
		<%
			For each x as PagSeguro.Transaction in oPS.transactions
			%>	
			<div style="padding:20px; margin:20px 0px 20px 0px; border:solid 1px #EFEFEF;">
				
				Data: <%=x.data%><br>
				C&oacute;digo: <%=x.code%><br>
				Status: <%=oPS.rStatus(x.status)%>
				
			</div>
			<%	
			Next
		%>
		
		<br><br>
		Dados enviados:<br>
		<%=oPS.auxData%>
	</form>
	
	<hr/>
	
	<form id="f2" name="f2" method="POST" action="?acao=consultarCodigo">
		<br>
		C&oacute;digo: <input type="text" id="codigoTransacao" name="codigoTransacao" value="<%=request("codigoTransacao")%>"/><br>
		<br>
		
		<button type="submit">Consultar</button>
		
		<br>
		<br>
		<!-- Resultado -->
		<br>
		Retorno:<br>
		<%=oPS.resultado%>
		
		<!-- Exibindo resultados caso tenha mais de uma transaction -->
		<%
			For each x as PagSeguro.Transaction in oPS.transactions
			%>	
			<div style="padding:20px; margin:20px 0px 20px 0px; border:solid 1px #EFEFEF;">
				
				Data: <%=x.data%><br>
				C&oacute;digo: <%=x.code%><br>
				Status: <%=oPS.rStatus(x.status)%>
				
			</div>
			<%	
			Next
		%>
		
		<br><br>
		Dados enviados:<br>
		<%=oPS.auxData%>
		
	</form>
</body>
</html>