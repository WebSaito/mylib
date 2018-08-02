<%@ Page Language="VB" CodeFile="api-cliente.aspx.vb" Inherits="pc" %>
<%@ OutputCache Duration="1" Location="none"%>
<!DOCTYPE HTML>
<html lang="en-US">
<head>
	<meta charset="UTF-8">
	<title></title>
</head>
<script type="text/javascript">
	function enviar(v){
		document.forms["form1"].action = "api-cliente.aspx?acao=enviar&metodo="+v;
		document.forms["form1"].submit();
	}
</script>
<body>
	<form id="form1" name="form1" method="POST">
		
		<input type="text" name="cliente" id="cliente" value="<%=request("cliente")%>" />
		<input type="text" name="segredo" id="segredo" value="<%=request("segredo")%>" />
		
		<hr/>
		<button type="button" onclick="enviar('GET')">GET</button>
		<button type="button" onclick="enviar('POST')">POST</button>
		<button type="button" onclick="enviar('PUT')">PUT</button>
		<button type="button" onclick="enviar('DELETE')">DELETE</button>
	</form>
</body>
</html>