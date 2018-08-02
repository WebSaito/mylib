Imports System
Imports System.IO
Imports System.Net

Namespace google
Public Class youtube
	Implements IDisposable
	'---
	'Variaveis e objetos
	Public endPCode as String = "https://accounts.google.com/o/oauth2/auth"
	'GET
	'client_id	Obrigatório. O ID do cliente OAuth 2.0 para seu aplicativo. É possível encontrar este valor no APIs Console .
	'redirect_uri	Obrigatório. Um redirect_uri registrado para seu ID de cliente. Registre URIs de redirecionamento válidos para seu aplicativo no APIs Console.
	'response_type	Obrigatório. Determina se o endpoint do OAuth 2.0 do Google retorna um código de autorização. Defina o valor do parâmetro para code.
	'scope	Obrigatório. Uma lista de escopos com espaço delimitado que identifica os recursos que seu aplicativo pode acessar em nome do usuário. Esses valores determinam quais permissões são listadas na página de consentimento que o Google exibe ao usuário. 

		'A YouTube Data API é compatível com os seguintes escopos:

		'Alcances
		'https://www.googleapis.com/auth/youtube	Gerenciar sua conta do YouTube.
		'https://www.googleapis.com/auth/youtube.readonly	Visualizar sua conta do YouTube.
		'https://www.googleapis.com/auth/youtube.upload	Enviar e gerenciar seus vídeos do YouTube.
		'https://www.googleapis.com/auth/youtubepartner-channel-audit	Recuperar a parte auditDetails em um recurso channel.
	'approval_prompt	Opcional. Este parâmetro indica se o usuário deve receber uma solicitação para conceder acesso da conta ao aplicativo sempre que tentar completar uma ação específica. O valor padrão é auto, indicando que o usuário precisa apenas conceder acesso na primeira vez que tentar acessar um recurso protegido.

	'Defina o valor do parâmetro para force para direcionar o usuário a uma página de consentimento, mesmo que já tenha concedido acesso a seu aplicativo por um conjunto de escopos determinado.
	
	'access_type	Recomendação. Este parâmetro indica se seu aplicativo pode atualizar tokens de acesso quando o usuário não estiver presente no navegador. Os valores de parâmetro válidos são online e offline. Defina o valor deste parâmetro para offline para permitir que o aplicativo use tokens atualizados quando o usuário não estiver presente (este é o método de atualização de tokens de acesso descritas posteriormente neste documento).
	'state	Opcional. Uma string usada por seu aplicativo para manter o estado entre solicitar e redirecionar a resposta. O valor exato enviado é retornado como um par name=value no fragmento de hash (#) de redirect_uri depois que o usuário consentir ou negar a solicitação de acesso de seu aplicativo. Este parâmetro pode ser usado para diversas finalidades, como direcionar o usuário ao recurso correto em seu aplicativo, enviar nonces e diminuir a falsificação de solicitações de outros sites.
	'login_hint	Opcional. Se seu aplicativo souber qual usuário está tentando autenticar, este parâmetro poderá ser usado para apresentar um hint ao Servidor de autenticação do Google. O servidor usa o hint para simplificar o fluxo de login preenchendo o campo de e-mail no formulário de login ou selecionando a sessão de multilogin apropriada.
	
	'Exemplo:https://accounts.google.com/o/oauth2/auth?
	' 	client_id=1084945748469-eg34imk572gdhu83gj5p0an9fut6urp5.apps.googleusercontent.com&
	' 	redirect_uri=http%3A%2F%2Flocalhost%2Foauth2callback&
	' 	scope=https://www.googleapis.com/auth/youtube&
	' 	response_type=code&
	' 	access_type=offline
	
	Public client_id as String = ""
	Public client_secret as String = ""
	Public redirect_uri as String = ""
	Public code as String = ""
	Public token as String = ""
	Public refresher as String = ""
	
	Public endPToken as String = "https://accounts.google.com/o/oauth2/token"
	'Post 
	'code	O código de autorização que o Google retornou para redirect_uri na etapa 3.
	'client_id	ID do cliente do OAuth 2.0 para seu aplicativo. Esse valor é exibido no Google APIs console.
	'client_secret	O segredo do cliente associado a seu ID do cliente. Esse valor é exibido no Google APIs console.
	'redirect_uri	redirect_uri registrada para seu ID do cliente.
	'grant_type	Defina esse valor como authorization_code.
		
	Public erros as String = ""
	
	Public resultado
	
	Public endPOpt as String = "https://www.googleapis.com/youtube/v3/"
	'Operações:
	'activity				
	'caption				
	'channel				
	'channelBanner				
	'channelSection				
	'comment				
	'commentThread				
	'guideCategory				
	'i18nLanguage				
	'i18nRegion				
	'playlist				
	'playlistItem				
	'search result				
	'subscription				
	'thumbnail				
	'video				
	'videoCategory				
	'watermark				
	
	
	
	
	
	
	
	
	
	
	
	'---
	'Construtor
	Public Sub New()
		client_id = system.configuration.configurationManager.appSettings("youtube_client_id")
		client_secret = system.configuration.configurationManager.appSettings("youtube_client_secret")
		redirect_uri = system.configuration.configurationManager.appSettings("youtube_oauth_redirect")
	End Sub
	
	'---
	'Limpeza
	Public Sub Dispose() Implements System.IDisposable.Dispose
		
	End Sub
	
		
		Public Function getCodeUri() as String
			Dim r as String = ""
			
			r = endPCode &"?"& _
			"client_id="& client_id &"&"& _
			"redirect_uri="& httpUtility.urlEncode(redirect_uri) &"&"& _
			"scope=https://www.googleapis.com/auth/youtube&"& _
			"response_type=code&"& _
			"access_type=offline"
			
			return r
		End Function
		
		
		Public Function getTokenUri() as String
			Dim r as String = ""
			
			Dim dados as String = "code="& code &"&"& _
			"client_id="& client_id &"&"& _
			"client_secret="& client_secret &"&"& _
			"redirect_uri="& redirect_uri &"&"& _
			"grant_type=authorization_code"
			
			r = POST(endPToken, dados)
			
			resultado = New System.Web.Script.Serialization.JavaScriptSerializer().Deserialize(Of Object)( r )
			
			'Validando resposta
			if( resultado.containsKey("access_token") )then
				
				token = resultado("access_token")
				'refresher = resultado("refresh_token")
				
			else
				'erro
				erros &= r
			end if
			
			return r
		End Function
		
		
		
		
		
		
		
		'----------------------------------------------------------
		'Método que faz um web POST
		Public Function POST(byval url as String, ByVal dados as String, Optional ByVal metodo as String = "POST") As String
			Dim r as String = ""
			
			Dim wr As WebRequest = WebRequest.Create(url)
			
			Select Case(metodo)
				Case "POST" : wr.Method = WebRequestMethods.Http.Post'"POST"'"PUT"
				Case "PUT" : wr.Method = "PUT"
				Case "GET" : wr.Method = WebRequestMethods.Http.Get
			End Select
			
			'wr.Credentials = new NetworkCredential(psEmail, psToken)
			
			Dim postData As String = dados
			Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
			
			wr.ContentType = "application/x-www-form-urlencoded"'"; charset=ISO-8859-1"'"application/json"
			wr.ContentLength = byteArray.Length
			
			Dim dataStream As Stream = wr.GetRequestStream()
			
			dataStream.Write(byteArray, 0, byteArray.Length)
			
			dataStream.Close()
			Try
				
				Dim rs As WebResponse = wr.GetResponse()
				dataStream = rs.GetResponseStream()
				
				Dim reader As New StreamReader(dataStream)
				
				Dim responseFromServer As String = reader.ReadToEnd()
				
				r = responseFromServer
				
			Catch ex as WebException
				
				Dim res = ex.Response
				Dim sr as StreamReader = new StreamReader(res.GetResponseStream())
				Dim returnvalue as String = sr.ReadToEnd()
				r = returnvalue
				
			End Try
			
			return r
		End Function
		'----------------------------------------------------------
		
		
		
		
		'----------------------------------------------------------
		'método que faz uma chamada GET com parametros
		Public Function GETDATA( Byval url as String, ByVal dados as String ) As String
			Dim r as String = ""
			
			Dim client as WebClient = new WebClient()
			Try
				if( token <> "" )then
					client.headers.add("Authorization", "Bearer "& token )
				end if
				
				r = client.DownloadString(url &"?"& dados)
			Catch ex as WebException
				
				Dim res = ex.Response
				Dim sr as StreamReader = new StreamReader(res.GetResponseStream())
				Dim returnvalue as String = sr.ReadToEnd()
				r = returnvalue
				
			End Try
			
			return r
		End Function
		'----------------------------------------------------------
	
	
	
End Class
End Namespace