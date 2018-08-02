<%@ Page Language="VB" CodeFile="ps.aspx.vb" Inherits="pc"%>
<!DOCTYPE HTML>
<html lang="en-US">
<head>
	<meta charset="UTF-8">
	<script src="http://code.jquery.com/jquery-3.2.1.min.js" integrity="sha256-hwg4gsxgFZhOsEEamdOYGBf13FyQuiTwlAQgxVSNgt4=" crossorigin="anonymous"></script>
	<script type="text/javascript" src="<%=oPS.urlJS(1)%>"></script>
	
	 
	<title></title>
</head>
<body>
	<div id="meiosDePagamento">
	</div>
</body>
<script type="text/javascript">
window.onload = function(){ 
	PagSeguroDirectPayment.setSessionId('<%=oPS.psSessionID%>'); 
	
	
	
	//Ação de conclusão de pagamento
	//PagSeguroDirectPayment.getSenderHash();
	
	
	PagSeguroDirectPayment.getPaymentMethods({
	amount: 500.00, 
	success: function(response){
		/*
		$.getJSON( response, function( data ) {
			alert(data[0].name);
		});
		*/
		//var oJson = $.parseJSON(response.paymentMethods);
		var oJson = JSON.stringify(response);
		
		var obJ = response;
		
		//alert(oJson.error);
		$("#meiosDePagamento").append("<strong>Meios de Pagamento</strong> "+ oJson +"<br><br><br><br><hr/>");
		//destrinchando paymentMethods
		
		$("#meiosDePagamento").append("<br>"+ obJ.paymentMethods.BOLETO.name );
		$("#meiosDePagamento").append("<br>"+ obJ.paymentMethods.ONLINE_DEBIT.name );
		$("#meiosDePagamento").append("<br>"+ obJ.paymentMethods.CREDIT_CARD.name );
		
		$.each(obJ.paymentMethods.CREDIT_CARD.options, function(key,value){
			var cartao = value
			$("#meiosDePagamento").append("<br> "+ cartao.name +":"+ cartao.status);
		});
		
	},
	error: function(response){
		alert("Ocorreu um erro");
	}, 
	complete: function(response){
		
		var oJson = response;
		
	}
	}); 
}
	
	
</script>
</html>