Imports System
Imports System.Web.UI.Page

'// ====================================================================================
'//	Todas as páginas do Website devem estender esta classe
'//	
'//	Herança direta: 
'//	<%@ Page Language="VB" Inherits="spacelab.Core" debug="true" %>
'//
'//	Herança via classe:
'//	<%@ Page Language="VB" CodeFile="arquivo.vb" Inherits="[CLASSE]" debug="true" %>
'//	
'//	Public Class [CLASSE]
'//	Inherits spacelab.Core
'//	
'//	Sub Page_Load(Src As Object, E As EventArgs) 
'//		MyBase.Page_Load(Src, E)
'//	End Sub
'//	
'//	End Class
'// ====================================================================================

'// ================================================================
'//	@version 1.0.6
'// ================================================================

Namespace spacelab

	Public Class Core

	    Inherits System.Web.UI.Page
		
	    Protected Sub Page_Load(byVal E As EventArgs)

			MyBase.OnLoad(E)
			
	    End Sub

	    '// ================================================================
	    '//	Inclui tags META HTML para compartilhamento
        '// no Facebook e Google Plus
        '// @param title Titulo da página
        '// @param description Descrição da página
        '// @param image Opcional. Se não especificado, recupera
        '// a imagem que está configurada no WEB.CONFIG
        '// @param fullImgURL Se true, indica que o parâmetro image
        '// foi passado com a URL completa da imagem, incluindo http://...
	    '// ================================================================
	    Public Function share_default(ByVal title As String, ByVal description As String, Optional ByVal image As String = "", Optional ByVal fullImgURL As Boolean = False) As String

			Dim metaList As String

			metaList &= "<meta property='og:title' content='[TITLE]' />"
			metaList &= "<meta property='og:type' content='website' />"
			metaList &= "<meta property='og:description' content='[DESCRIPTION]' />"

            if fullImgURL then 
                metaList &= "<meta property='og:image' content='[IMAGE]' />"
            else
                metaList &= "<meta property='og:image' content='[PROTOCOL]://[DOMINIO][IMAGE]' />"
            end if

			metaList &= "<meta property='og:url' content='[PROTOCOL]://[DOMINIO][RAW_URL]' />"
			metaList &= "<meta property='fb:app_id' content='[APP_ID]' />"

			metaList = metaList.replace("[TITLE]", title)
			metaList = metaList.replace("[DESCRIPTION]", util_removeHTML(description))
			metaList = Regex.Replace(metaList, "\[DOMINIO\]", Request.Url.Host)
			metaList = Regex.Replace(metaList, "\[PROTOCOL\]", Request.Url.Scheme)
			metaList = metaList.replace("[RAW_URL]", Request.RawURL)
			metaList = metaList.replace("[APP_ID]", System.Configuration.ConfigurationManager.AppSettings("facebook_appId"))

			if image = "" then
				metaList = metaList.replace("[IMAGE]", System.Configuration.ConfigurationManager.AppSettings("share_defaultImage"))
			else
				metaList = metaList.replace("[IMAGE]", image)
			end if

			return metaList

		End Function

	    '// ================================================================
	    '//	Inclui tags META HTML para compartilhamento de Video
        '// no Facebook
        '// @param videoURL URL para o video
        '// @param title Titulo da página
        '// @param description Descrição da página
        '// @param image Opcional. Se não especificado, recupera
        '// a imagem que está configurada no WEB.CONFIG
        '// @param fullImgURL Se true, indica que o parâmetro image
        '// foi passado com a URL completa da imagem, incluindo http://...
        '// @param fullVideoURL Se true, indica que o parâmetro video
        '// foi passado com a URL completa, incluindo http://...
	    '// ================================================================
        Public Function share_video(ByVal videoURL As String, ByVal title As String, ByVal description As String, ByVal width As Integer, ByVal height As Integer, Optional ByVal image As String = "", Optional ByVal fullImgURL As Boolean = False, Optional ByVal fullVideoURL As Boolean = False)

            dim metaList As String = ""
            dim extensionList() As String = { "m4v","flv","webm","ogv" }
            dim mimeTypeList() As String = { "video/mp4","video/x-flv","video/webm","video/ogg" }

			metaList &= "<meta property='fb:app_id' content='[APP_ID]' />"
            metaList &= "<meta property='og:type' content='video.other' />"
            metaList &= "<meta property='og:title' content='[TITLE]' />"
			metaList &= "<meta property='og:description' content='[DESCRIPTION]' />"
			metaList &= "<meta property='og:url' content='[PROTOCOL]://[DOMINIO][RAW_URL]' />"

            if fullImgURL then 
                metaList &= "<meta property='og:image' content='[IMAGE]' />"
            else
                metaList &= "<meta property='og:image' content='[PROTOCOL]://[DOMINIO][IMAGE]' />"
            end if

            For i As Integer = 0 To extensionList.length - 1
                if fullVideoURL then 
                    metaList &= "<meta property='og:video:url' content='[VIDEO].[VIDEO_EXT]' />"
                else
                    metaList &= "<meta property='og:video:url' content='[PROTOCOL]://[DOMINIO][VIDEO].[VIDEO_EXT]' />"

                    if System.Configuration.ConfigurationManager.AppSettings("ssl") <> "" then
                        metaList &= "<meta property='og:video:secure_url' content='[SSL][VIDEO].[VIDEO_EXT]' />"
                    end if
                end if
                metaList &= "<meta property='og:video:type' content='[VIDEO_MIME_TYPE]' />"
                metaList &= "<meta property='og:video:width' content='[WIDTH]' />"
                metaList &= "<meta property='og:video:height' content='[HEIGHT]' />"
                metaList = metaList.replace("[VIDEO_EXT]", extensionList(i))
                metaList = metaList.replace("[VIDEO_MIME_TYPE]", mimeTypeList(i))
            Next

			metaList = metaList.replace("[TITLE]", title)
			metaList = metaList.replace("[DESCRIPTION]", util_removeHTML(description))
			metaList = Regex.Replace(metaList, "\[DOMINIO\]", Request.Url.Host)
			metaList = Regex.Replace(metaList, "\[PROTOCOL\]", Request.Url.Scheme)
			metaList = metaList.replace("[APP_ID]", System.Configuration.ConfigurationManager.AppSettings("facebook_appId"))
			metaList = metaList.replace("[VIDEO]", videoURL)
			metaList = metaList.replace("[SSL]", System.Configuration.ConfigurationManager.AppSettings("ssl"))
			metaList = metaList.replace("[WIDTH]", width)
			metaList = metaList.replace("[HEIGHT]", height)
			metaList = metaList.replace("[RAW_URL]", Request.RawURL)

			if image = "" then
				metaList = metaList.replace("[IMAGE]", System.Configuration.ConfigurationManager.AppSettings("share_defaultImage"))
			else
				metaList = metaList.replace("[IMAGE]", image)
			end if

            return metaList

        End Function

	    '// ================================================================
	    '//	Retorna um link para compartilhamento no Facebook
        '// Necessita Meta Tags no Head do HTML, ver função share_default
        '// @param url URL a ser compartilhada
	    '// ================================================================
		Public function shareLink_facebook(Optional ByVal url As String = "", Optional ByVal autoEncode As Boolean = True)
			if url = "" then url = currentURL()
			'return "https://www.facebook.com/sharer/sharer.php?s=100&p[url]="+IIF(autoEncode, HttpUtility.URLEncode(url), url)
            return String.Format("https://facebook.com/dialog/feed?app_id={0}&link={1}", System.Configuration.ConfigurationManager.AppSettings("facebook_appId"), IIF(autoEncode, HttpUtility.URLEncode(url), url))
		End Function

        '// ================================================================
        '// Retorna um link para compartilhamento no Linkedin
        '// Necessita Meta Tags no Head do HTML, ver função share_default
        '// @param url URL a ser compartilhada
        '// ================================================================
        Public function shareLink_linkedin(Optional ByVal url As String = "", Optional ByVal autoEncode As Boolean = True)
            if url = "" then url = currentURL()
            return "https://www.linkedin.com/cws/share?url="+IIF(autoEncode, HttpUtility.URLEncode(url), url)
        End Function

	    '// ================================================================
	    '//	Retorna um link para compartilhamento no Twitter
        '// @param text Texto a ser tuítado, exemplo: Novo website
        '// @param url URL a ser compartilhada
        '// @param hashTags Hash tags separadas por virgula, exemplo:
        '// spacelab,agencia,frontend
	    '// ================================================================
		Public Function shareLink_twitter(ByVal text As String, Optional ByVal url As String = "", Optional ByVal hashTags As String = "", Optional ByVal maxLength As Integer = 140, Optional ByVal autoEncode As Boolean = True)
			if url = "" then url = currentURL()
			Dim sLength As Integer = (text & hashTags & url).Length
            Dim mathMin As Integer = Math.Min(maxLength, sLength)
            Dim mathMax As Integer = Math.Max(maxLength, sLength)
            text = util_removeHTML(text)
			text = textoResumo(text, IIF(sLength > maxLength, mathMax-(mathMax-mathMin), maxLength))
			Dim s As String = "https://twitter.com/share?url=[PAGE_URL]&text=[TEXT]"
			s = s.replace("[PAGE_URL]", IIF(autoEncode, HttpUtility.URLEncode(url), url))
			s = s.replace("[TEXT]", IIF(autoEncode, HttpUtility.URLEncode(text), text))
			if hashTags <> "" then s += "&hashtags="+IIF(autoEncode, HttpUtility.URLEncode(hashTags), hashTags)
			return s
		End Function

	    '// ================================================================
	    '//	Retorna um link para compartilhamento no Google Plus
        '// Necessita Meta Tags no Head do HTML, ver função share_default
        '// @param url URL a ser compartilhada
	    '// ================================================================
		Public Function shareLink_googlePlus(Optional ByVal url As String = "", Optional ByVal autoEncode As Boolean = True)
			if url = "" then url = currentURL()
			return "https://plus.google.com/share?url="+IIF(autoEncode, HttpUtility.URLEncode(url), url)
		End Function

	    '// ================================================================
	    '//	Retorna um link para compartilhamento no Pinterest
        '// @param url URL a ser compartilhada
	    '// ================================================================
		Public Function shareLink_pinterest(ByVal text As String, ByVal image As String, Optional ByVal url As String = "", Optional ByVal autoEncode As Boolean = True)
			text = textoResumo(text, 140)
			if url = "" then url = currentURL()
            text = util_removeHTML(text)
			Dim s As String = "https://pinterest.com/pin/create/button/?url=[PAGE_URL]&description=[TEXT]&media=[IMAGE]"
			s = s.replace("[PAGE_URL]", IIF(autoEncode, HttpUtility.URLEncode(url), url))
			s = s.replace("[TEXT]", IIF(autoEncode, HttpUtility.URLEncode(text), text))
			s = s.replace("[IMAGE]", IIF(autoEncode, HttpUtility.URLEncode(image), image))
			return s
		End Function

	    '// ================================================================
	    '//	Valida CEP em formato simples: 01228-000
	    '// ================================================================
		Public Function valida_cep(ByVal cep As String) As Boolean

			' Valida se o total de caracteres é menor que 8 ou maior que 8
			if len(Regex.Replace(cep, "[^\d]", "")) < 8 or len(Regex.Replace(cep, "[^\d]", "")) > 8 then
				Return False
			' Valida o cep caso existam números repetidos seguidamente, exemplo: 00000-000
			else if Regex.IsMatch(Regex.Replace(cep, "[^\d]", ""), "0{8,}|1{8,}|2{8,}|3{8,}|4{8,}|5{8,}|6{8,}|7{8,}|8{8,}|9{8,}") then
				Return False
			else 
				Return True
			end if		

		End Function

		'// ================================================================
		'//	Utilidade para uso com o WebForms
		'// ================================================================
		public function ruleXMLWithServerSideId(ByVal id as String, Optional ByVal value As String = "")
			Return "<item server-side-id='" & id & "' value='" & value & "'></item>"
		end function

		'// ================================================================
		'//	Utilidade para uso com o WebForms
		'// ================================================================
		public function ruleFromXML(ByVal id)
			dim xml = ""
			xml &= "<item id='" & id & "'></item>"
			Return xml
		end function

		'// ================================================================
		'//	Toda requisição de parâmetros deve chamar essa função.
		'//	Evita que os caracteres venham com codificação diferente.
		'//	Para completo funcionamento deve ser usado a função
		'//	escape(valor) do JavaScript
		'// ================================================================
		Public Function replaceAjaxChars(ByVal valor As String) As String
			Return replace(replace(HttpUtility.UrlDecode(valor, System.Text.Encoding.Default()), "'", ""), "'", "")
		End Function

		'// ================================================================
		'//	URL Decode para caracteres escapados do JS
		'// ================================================================
		Public Function url_decode(ByVal valor As String) As String
			Return HttpUtility.UrlDecode(valor, System.Text.Encoding.Default())
		End Function

		'// ================================================================
		'//	Remove tags HTML do texto
		'//	Utilizar quando é enviado email para o cliente, impedindo
		'//	que seja utilizado algum código malicioso.
		'// ================================================================
		Public Function util_removeHTML(ByVal html as String)
			if html = Nothing then
				return ""
			else 
				Return Regex.Replace(html, "<.*?>", "")
			end if
		End Function

		'// ================================================================
		'//	Remove tudo que não for número
		'// ================================================================
		Public Function util_onlyNumbers(ByVal value as String)
			if value = Nothing then
				return ""
			else 
				Return Regex.Replace(value, "[^\d]", "")
			end if
		End Function

		'// ================================================================
		'//	Converte texto para link, ou seja, removendo caracteres
        '// especiais de uma url.
		'// ================================================================
        Public Function util_toLink(ByVal text As String) As String

            text = util_removeHTML(text)

			text = lcase(text)
            text = trim(text)

            text  = Regex.Replace(text, "[\.\=\+\*\,\?\:\[\]\{\}\'\/\\\" & chr(34) & "]", "")
			text  = Regex.Replace(text, "[áâàåãä]", "a")
			text  = Regex.Replace(text, "æ", "ae")
			text  = Regex.Replace(text, "[éêèë]", "e")
			text  = Regex.Replace(text, "[íîïì]", "i")
			text  = Regex.Replace(text, "[óôòõö]", "o")
			text  = Regex.Replace(text, "[ø]", "0")
			text  = Regex.Replace(text, "[úûùü]", "u")
			text  = Regex.Replace(text, "ç", "c")
			text  = Regex.Replace(text, "ñ", "n")
			text  = Regex.Replace(text, "ý", "y")
			text  = Regex.Replace(text, "[\s_]", "-")
			text  = Regex.Replace(text, "[^\w\d-]", "")
            
            return text

        End Function 

		'// ================================================================
		'//	Recupera a primeira imagem do HTML
		'//	@param html Conteúdo HTML para recuperar a imagem
		'//	@param onlyImagePath Se true, retorna apenas o caminho da
        '// imagem, caso contrário o HTML da imagem
        '// @return Retorna vazio caso não encontre imagem
		'// ================================================================
		Public Function util_firstImageFromHTML(ByVal html As String, Optional ByVal onlyImagePath as Boolean = False) As String
			if onlyImagePath = Nothing then
				return ""
			else 
                Dim match As Match = Regex.Match(html, "<img.+?>")

                if match.success then
                    match = Regex.Match(match.value, "src=[""](.+?)[""]")
                    if onlyImagePath = False then
                        return match.value
                    end if
                    if match.success then
                        return match.groups(1).value 
                    end if
                end if
			end if
            return ""
		End Function

		'// ================================================================
		'//	Utilidade para gerar JSON no formato:
		'//	"propriedade":"valor"
		'// ================================================================
		Public Function formataJSON(ByVal propriedade As String, ByVal valor As String)
			Return """" & propriedade & """:""" & valor & """"
		End Function

		'// ================================================================
		'//	Utilidade para gerar JSON no formato:
		'//	{"status":"[STATUS]", "message":"[MESSAGE]"[EXTRA]}
		'//	Para o parâmetro extra pode ser passado qualquer coisa, 
		'//	incluindo outro JSON. Só não se esqueça de incluir a vírgula
		'//	antes do [EXTRA], exemplo:
		'//	{"status":"[STATUS]", "message":"[MESSAGE]", [EXTRA]}
		'// ================================================================
		Public Function json_default(ByVal status as String, Optional ByVal id as String = "", Optional ByVal message As String = "", Optional ByVal extra as String = "")

			Dim json As String = ""

			json += "{"
			json += formataJSON("status", status)
			json += "," + formataJSON("id", id)
			json += "," + formataJSON("message", message)
			json += extra
			json += "}"

			Return json

		End Function

		'// ================================================================
		'//	Tenta recuperar o valor de um objeto JSON
		'// ================================================================
		Public Function json_getValue(ByVal json As Object, ByVal propriedade As String)
			Try
				return json(propriedade)
			Catch ex As Exception
				return ""
			End Try
		End Function

		'// ================================================================
		'//	Retorna o texto até um limite máximo de caracteres
        '// @param ppost O texto
        '// @param limite Máximo de caracteres
		'// ================================================================
		Public Function textoResumo(byval ppost as string, byval limite as integer, optional byval addContinue as boolean = True) as String
			Dim r as string = ""
			
			if trim(ppost) <> "" then
				
				Dim tmp as string = util_removeHTML(ppost)

				tmp = Regex.Replace(tmp, "[\r\n]", " ")
				
				if tmp.length > limite then
					'r = left(tmp, limite) &"..."
                    if addContinue then limite = limite - 3
					Dim regexResumo As New Regex("^(.{0," & limite.ToString() & "})\s.*", RegexOptions.IgnorePatternWhitespace)
					r = regexResumo.Replace(tmp, "$1")
					r = Regex.Replace(r, "(.{0," & limite.ToString() & "}).*", "$1")
                    if addContinue then r = r & "..."
				else
					r = tmp
				end if
				
			end if
			
			return r
		End Function

		'// ================================================================
		'//	Recupera o dominio atual
		'// ================================================================
        Public Function currentDomain() As String
            return Request.Url.Scheme+"://"+Request.Url.Host
        End Function

		'// ================================================================
		'//	Recupera a URL atual
        '// @param extraText Opcional. Caso precise incluir algo a mais
        '// na URL. Exemplo: ?teste=2
		'// ================================================================
        Public Function currentURL(Optional ByVal extraText As String = "") As String
            return currentDomain()+Request.RawURL+extraText
        End Function

        '// ================================================================
        '// Recupera o nome do browser, exemplo: Safari
        '// ================================================================
        Public Function browser_name(Optional ByVal prefix As String = "browser-") As String
            try 
                dim name As String = lcase(Request.Browser.Browser)
                if Regex.IsMatch(name, "^(internetexplorer|internet.explorer|ie|i.e)$") then
                    return prefix & "ie"
                else 
                    return prefix & name
                end if
            catch ex As Exception
                return ""
            end try
        End Function

        '// ================================================================
        '// Recupera a versão do browser, exemplo: 9
        '// ================================================================
        Public Function browser_version(Optional ByVal prefix As String = "browser-") As String
            try 
                return prefix & Request.Browser.MajorVersion
            catch ex As Exception
                return ""
            end try
        End Function

        '// ================================================================
        '// Recupera a versão completa do browser, exemplo: 9.0
        '// ================================================================
        Public Function browser_fullVersion(Optional ByVal prefix As String = "browser-") As String
            try 
                return prefix & Regex.Replace(Request.Browser.Version, "\.", "-")
            catch ex As Exception
                return ""
            end try
        End Function

        '// ================================================================
        '// Retorna true se for iOS, Android ou Windows Phone
        '// ================================================================
        Public Function platform_isMobileOS() As Boolean
            Dim userAgent As String = Request.UserAgent
            try 
                if Regex.IsMatch(userAgent, "(iPhone|iPad|Android|Windows\sPhone)", RegexOptions.IgnoreCase) then
                    return True
                else
                    return False
                end if
            catch ex As Exception
                return False
            end try
            return False
        End Function

		'// ================================================================
		'//	Carrega o conteúdio de uma URL externa
        '// @param u URL de destino
        '// @param enc Opcional. Tipo de encoding
		'// ================================================================
		Public Function loadRemote(ByVal u As String, Optional ByVal enc As String = "UTF8") As String
			Dim r As String = ""
			Try
			
				Dim w As System.Net.WebClient = new System.Net.WebClient()
				Dim tb() as Byte
				
				tb = w.DownloadData(u)
				
				If enc = "UTF8" Then
					r = System.Text.Encoding.UTF8.GetString(tb)
				Else				
					r = System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(tb)
				End If
				
				w.Dispose()
				
			Catch ex As Exception
				r = ex.ToString() &"<br><br><br>"& u
			End Try
			
			return r
		End Function
		
		'// ================================================================
		'//	Carrega o conteúdo de uma URL local
        '// @param u URL de destino
        '// @param enc Opcional. Tipo de encoding
		'// ================================================================
		Public Function loadLocal(ByVal u As String, Optional ByVal enc As String = "UTF8") As String
			Dim r As String = ""
			
			Try
				
				Dim sw as New System.IO.StringWriter
				
				HttpContext.Current.Server.Execute(u, sw)
				r = sw.toString()
				
			Catch ex As Exception
				r = ex.ToString() &"<br><br><br>"& u
			End Try
			
			return r
		End Function

		'// ================================================================
		'//	Atalho para o ConfigurationManager.AppSettings
        '// @param key Nome da chave no web.config
		'// ================================================================
        Public Function settings(ByVal key As String) As String
            return ConfigurationManager.AppSettings(key)
        End Function

	End Class

	Public Class AbstractData
		
		public status As String = ""
		public mensagem As String = ""
		public id As String = ""
				
	End Class

End Namespace
