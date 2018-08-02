Imports System
Imports SAPI

Public Class pc
	Inherits System.Web.UI.Page
	
	Public clienteAPI as new CLI1()
	
	Sub Page_Load(byval src as object, byval e as eventArgs)
		
		if request("acao") <> "" then
			enviar()
		end if
		
	End Sub
	
	
	Public Sub enviar()
		
		
		clienteAPI.headers.add(new objCliHeader() with { .chave="Cliente" , .valor= request("cliente") }) 
		clienteAPI.headers.add(new objCliHeader() with { .chave="Segredo" , .valor= request("segredo") }) 
		
		'Fix metodo
		
		
		response.write(clienteAPI.SEND( "http://localhost/api.aspx" , "abc=123" , request("metodo") ))
		
	End Sub
	
End Class