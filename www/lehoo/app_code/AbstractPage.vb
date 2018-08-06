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

	Public Class AbstractPage

	    Inherits Abstract

	    Public meta_description As String
		
	    Protected Sub Page_Load(byVal E As EventArgs)
			
			MyBase.OnLoad(E)
			
	    End Sub

	    Public Function share_default(ByVal title As String, ByVal description As String) As String

		Dim metaList As String

		metaList += "<meta property='og:title' content='[TITLE]' />"
		metaList += "<meta property='og:type' content='website' />"
		metaList += "<meta property='og:description' content='[DESCRIPTION]' />"
		metaList += "<meta property='og:image' content='http://[DOMINIO]/_share/share-lemier-website.jpg' />"
		metaList += "<meta property='og:url' content='http://[DOMINIO][RAW_URL]' />"

		metaList = metaList.replace("[TITLE]", title)
		metaList = metaList.replace("[DESCRIPTION]", description)
		metaList = Regex.Replace(metaList, "\[DOMINIO\]", System.Configuration.ConfigurationManager.AppSettings("dom"))
		metaList = metaList.replace("[RAW_URL]", Request.RawURL)

		return metaList

	End Function
		
	End Class

End Namespace