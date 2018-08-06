<%@ Application Language="VB" %>
<%@ import namespace="system"%>
<%@ import namespace="system.web"%>
<%@ import namespace="system.data.sqlClient"%>
<%@ import namespace="emails"%>
<%@ import namespace="nsUrl"%>
<%@ import namespace="Spacelab"%>
<%@ import namespace="System.Globalization"%>
<%@ import namespace="System.Threading"%>
<script runat="server">

Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

End Sub

Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)

End Sub

Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
 		
End Sub

Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)

End Sub

Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)

End Sub


Protected Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
	'-------------------------------------
	'OBJETOS---------------------------
	
	Dim oURL as new sURL()
	'FIM OBJETOS-----------------------
	
	
	
	'##########################################################################################
	'LÍNGUA
	
	Dim linguaAtual as String = "pt-BR"
	
	if instr(request.rawurl.tostring(),"pt-BR") or instr(request.rawurl.tostring(),"pt-br")then
		linguaAtual = "pt-BR"
	end if
	
	'if instr(request.rawurl.tostring(),"en-US") or instr(request.rawurl.tostring(),"en-us")then
		'linguaAtual = "en-US"
	'end if
	
	if instr(request.rawurl.tostring(),"fr-FR") or instr(request.rawurl.tostring(),"fr-fr")then
		linguaAtual = "fr-FR"
	end if
	
	'Setando a lingua manualmente
	Thread.CurrentThread.CurrentCulture = new CultureInfo(linguaAtual)
	Thread.CurrentThread.CurrentUICulture = new CultureInfo(linguaAtual)
	
	'FIM LÍNGUA
	'##########################################################################################
	
	
	
	
	'##########################################################################################
	'URL
	
	Dim paginas() as String = {
			"readymake:/website/v1/sobre.aspx:tmp"
	}
							  
	oURL.adicionarPaginas(paginas)

	dim urlDestino as String = ""

	'[FILTROS - Inicio]
	if check_filtros() = True then 
		return
	end if
	'[FILTROS - Fim]

	'[301]
	dim urlDestino_301 = check_URL301()
	if urlDestino_301 <> "" then
		Response.Status = "301 Moved Permanently"
		Response.AddHeader("Location", urlDestino_301)
		Response.End()
		return
	end if
	'[301 - Fim]
	
	'[URL - Módulo URL]	
	urlDestino = oURL.destino(Request.URL)
	if urlDestino <> "" then
		context.rewritePath("~"& urlDestino, false)
		return
	end if
	'[URL - Módulo URL]

	'[URL - Módulo Geral]	
	urlDestino = url_geral()
	if urlDestino <> "" then
		context.rewritePath("~"& urlDestino, false)
		return
	end if
	'[URL - Módulo Geral]
	
End Sub

'// ================================================================
'//	URL - GERAL
'// ================================================================
private function url_geral()
	dim rawURL as String = Request.RawUrl

	'Quando publicar o site utilizar o IF abaixo
	'if rawURL = "/" or Regex.IsMatch(rawURL, "^/pt-br/?$") or Regex.IsMatch(rawURL, "^/en-us/?$") then
	'if Regex.IsMatch(rawURL, "^/website/v1/?$") or Regex.IsMatch(rawURL, "^/pt-br/?$") or Regex.IsMatch(rawURL, "/fr-fr/?$") then
	'	return "/website/v1/default.aspx"
	'end if

	return ""
end function

'// ================================================================
'//	URL - 301
'// ================================================================
private function check_URL301()
	
	dim rawURL as String = Request.RawUrl
	
	if Regex.IsMatch(rawURL, "^/gds/?$") then
		return "http://readymake.gerenciadordesite.com"
	end if
	
	return ""

end function

'// ================================================================
'//	URL - FILTROS
'// Inclua aqui filtros pelo qual a URL deve ser ignorada.
'// Retorne True para parar a URL amigável
'// ================================================================
private function check_filtros()
	return False
end function

</script> 