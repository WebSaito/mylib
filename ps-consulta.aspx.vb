Imports System
Imports PagSeguro

Public Class pc
	Inherits System.Web.UI.Page
	
	Public oPS as new PagSeguro2()
	
	Sub Page_Load(byval src as object, byval e as eventArgs)
		
		oPS.modo = "teste"
		
		oPS.psEmail = "websaito07@gmail.com"
		if oPS.modo = "producao" then
			oPS.psToken = "6D87FE76DFB04BD1AA18DBF5D637663B"
		else
			oPS.psToken = "F5C2BF82AAC84F60A5B060626BA353BE"
		end if
		
		'Consultando transaçao baseada na referencia(número do pedido) enviada na transaçao
		if request("acao") = "consultarRef" then
			oPS.getTransactionByRef(request("numeroPedido"), request("dataInicial"), request("dataFinal") )
		end if
		
		
		'Consultando transacao baseada no código
		if request("acao") = "consultarCodigo" then
			oPS.getTransactionByCode(request("codigoTransacao"))
		end if
		
		response.write(oPS.erros)
		
	End Sub
	
End Class


		