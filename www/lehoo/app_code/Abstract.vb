Imports System
Imports System.Web.UI.Page

'// ====================================================================================
'//	Todas as páginas devem extender a classe Abstract
'//	
'//	Herança direta: 
'//	<%@ Page Language="VB" Inherits="abstract.Abstract" debug="true" %>
'//
'//	Herança via classe:
'//	<%@ Page Language="VB" CodeFile="arquivo.vb" Inherits="[CLASSE]" debug="true" %>
'//	
'//	Public Class [CLASSE]
'//	Inherits abstract.Abstract
'//	
'//	Sub Page_Load(Src As Object, E As EventArgs) 
'//		MyBase.Page_Load(Src, E)
'//	End Sub
'//	
'//	End Class
'// ====================================================================================

Namespace abstract

	Public Class Abstract
	    Inherits System.Web.UI.Page
		
		Public lang As String = lcase(System.Globalization.CultureInfo.CurrentCulture.Name)
		
	    Protected Sub Page_Load(byVal E As EventArgs)

			MyBase.OnLoad(E)
			
	    End Sub
		

		'// ================================================================
		'//	Browser Detect
		'// ================================================================

		Public Function browser_name() As String

			if browser_isIE() then
				return "browser-ie"
			else if browser_isSafari()
				return "browser-safari"
			end if

			return ""

		End Function

		Public Function browser_isIE() As Boolean

			Dim bc As HttpBrowserCapabilities = Request.Browser

			if Regex.IsMatch(bc.Browser, "(IE|InternetExplorer)", RegexOptions.IgnoreCase) then
				return True
			else 
				return False
			end if

		End Function

		Public Function browser_isSafari() As Boolean

			Dim bc As HttpBrowserCapabilities = Request.Browser

			if Regex.IsMatch(bc.Browser, "Safari", RegexOptions.IgnoreCase) then
				return True
			else 
				return False
			end if

		End Function
				
	End Class

End Namespace