Imports System
Imports System.Web.UI.Page

'// ====================================================================================
'//	Todas as classes que retornam JSON devem extender esta classe
'//	
'//	Herança direta: 
'//	<%@ Page Language="VB" Inherits="abstract.AbstractData" debug="true" %>
'//
'//	Herança via classe:
'//	<%@ Page Language="VB" CodeFile="arquivo.vb" Inherits="[CLASSE]" debug="true" %>
'//	
'//	Public Class [CLASSE]
'//	Inherits abstract.AbstractData
'//	
'//	Sub Page_Load(Src As Object, E As EventArgs) 
'//		MyBase.Page_Load(Src, E)
'//	End Sub
'//	
'//	End Class
'// ====================================================================================

Namespace abstract

	Public Class AbstractData
		
		public status As String = ""
		public mensagem As String = ""
		public id As String = ""
				
	End Class

End Namespace