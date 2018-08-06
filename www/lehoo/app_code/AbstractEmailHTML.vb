Imports System
Imports System.Web.UI.Page

'// ====================================================================================
'//	Todas as páginas que são templates de email devem extender esta classe
'//	
'//	Herança direta: 
'//	<%@ Page Language="VB" Inherits="abstract.AbstractEmailHTML" debug="true" %>
'//
'//	Herança via classe:
'//	<%@ Page Language="VB" CodeFile="arquivo.vb" Inherits="[CLASSE]" debug="true" %>
'//	
'//	Public Class [CLASSE]
'//	Inherits abstract.AbstractEmailHTML
'//	
'//	Sub Page_Load(Src As Object, E As EventArgs) 
'//		MyBase.Page_Load(Src, E)
'//	End Sub
'//	
'//	End Class
'// ====================================================================================

Namespace abstract

	Public Class AbstractEmailHTML

	    Inherits Abstract

	    Public dom As String = System.Configuration.ConfigurationManager.AppSettings("dom")

	    Protected Sub Page_Load(byVal E As EventArgs)

			MyBase.OnLoad(E)

	    End Sub

	End Class

End Namespace