Imports System

Public Class pc
	Inherits System.Web.UI.Page
	
	Dim oDB as new DB()
	
	Sub Page_Load(byval src as object, byval e as eventArgs)
		
		if request("acao") = "login" then
			
			Dim r as String = ""
			Dim dados as new Hashtable()
			dados = oDB.consultaLinha("select * from  lh_auth where email='"& oDB.SS( request("email") ) &"' and senha='"& oDB.SS( request("senha") ) &"' ")
			if( dados.containsKey("id") )then
				
				r = "{"& _
				"	'status':'sucesso',"& _
				"	'cliente':'"& dados("cliente") &"',"& _
				"	'segredo':'"& dados("segredo") &"'"& _
				"}"
				
			else
				
				r ="{ 'status':'erro', 'email':'"& request("email") &"', 'senha':'"& request("senha") &"' }"
				
			end if
			
			response.write(r)
			
		end if
		
	End Sub
	
End Class