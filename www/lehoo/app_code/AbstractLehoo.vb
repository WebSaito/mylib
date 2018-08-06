Imports System
Imports System.Web.UI.Page

'// ====================================================================================
'//	Todas os arquivos que são do FRONT, que exibem conteúdo (exceto requisições) 
'// devem extender esta classe
'//	
'//	Herança direta: 
'//	<%@ Page Language="VB" Inherits="abstract.AbstractPage" debug="true" %>
'//
'//	Herança via classe:
'//	<%@ Page Language="VB" CodeFile="arquivo.vb" Inherits="[CLASSE]" debug="true" %>
'//	
'//	Public Class [CLASSE]
'//	Inherits abstract.AbstractPage
'//	
'//	Sub Page_Load(Src As Object, E As EventArgs) 
'//		
'//	End Sub
'//	
'//	End Class
'// ====================================================================================

Namespace abstract

	Public Class AbstractLehoo
	    Inherits Abstract
		
		'Objetos comuns a todas as páginas
		Public oDB as new DB()
		
		'Propriedades
	    Public leUID as String = ""
		Public leNOME as String = ""
		Public leEMAIL as String = ""
		'-
		'Propriedades da página
		Public pgTitulo as String = System.Configuration.ConfigurationManager.AppSettings("tituloComplemento")
		
		
	    Protected Sub Page_Load(byVal E As EventArgs)
			
			MyBase.OnLoad(E)
				
	    End Sub

	    
		
	End Class

End Namespace