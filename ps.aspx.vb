Imports System
Imports PagSeguro

Public Class pc
	Inherits System.Web.UI.Page
	
	Public oPS as new PagSeguro2()
	
	Sub Page_Load(byval src as object, byval e as eventArgs)
		
		oPS.modo = "poducao"
		
		oPS.psEmail = "websaito07@gmail.com"
		oPS.psToken = "6D87FE76DFB04BD1AA18DBF5D637663B"
		
		oPS.getSession() 
		
		response.write(oPS.resultado)
		
		response.write("<hr/>")
		
		response.write(oPS.psSessionID)
		
	End Sub
	
End Class