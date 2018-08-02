Imports System
Imports System.IO
Imports System.Net

Namespace videos
Public Class vimeo
	Implements IDisposable
	'---
	'Variaveis e objetos
	Public client_id as String = ""
	Public client_secret as String = ""
	Public authHeader as String = ""
	Public token as String = ""
	Public erros as String = ""
	
	Public resultado 
	
	'---
	'Construtor
	Public Sub New()
		client_id = system.configuration.configurationManager.appSettings("vimeo_client_id")
		client_secret = system.configuration.configurationManager.appSettings("vimeo_client_secret")
	End Sub
	
	'---
	'Limpeza
	Public Sub Dispose() Implements System.IDisposable.Dispose
		client_id = ""
		client_secret = ""
		authHeader = ""
		erros = ""
	End Sub
		
		
		Public Function getUnauthorizedToken() as String
			Dim r as String = ""
			'POST https://api.vimeo.com/oauth/authorize/client
			'grant_type
			'scope
			'Header "Authorization : basic " + base64(client_id + ":" + client_secret)
			authHeader = "basic "& base64Encode( client_id  &":"& client_secret )
			
			r = POST("https://api.vimeo.com/oauth/authorize/client", "grant_type=client_credentials&scope=private interact create edit upload delete public" )
			resultado = New System.Web.Script.Serialization.JavaScriptSerializer().Deserialize(Of Object)( r )
			
			if( resultado.containsKey("access_token") )then
				
				token = resultado("access_token")
				
			end if
			
			return r
		End Function
		
		
		Public Function getAuthorizedToken(byval retorno as string, byval pcode as string) as String
			Dim r as String = ""
			'POST https://api.vimeo.com/oauth/access_token
			'grant_type
			'redirect_uri
			'code
			authHeader = "basic "& base64Encode( client_id &":"& client_secret )
			
			r = POST("https://api.vimeo.com/oauth/access_token", "grant_type=authorization_code&redirect_uri="& retorno &"&code="& pcode)
			resultado = New System.Web.Script.Serialization.JavaScriptSerializer().Deserialize(Of Object)( r )
			
			if( resultado.containsKey("access_token") )then
				
				token = resultado("access_token")
				
			end if
			
			return r
		End Function
		
		Public Function getCodeUri(byval retorno as String) as String
			Dim r as String = ""
			
			r = "https://api.vimeo.com/oauth/authorize?client_id="& client_id &"&response_type=code&redirect_uri="& retorno &"&state=teste"
			
			return r
		End Function
		
		
		Public Function DOIT(byval url as string, byval dados as string, Optional byVal metodo as string = "POST") as String
			Dim r as String = ""
			
			authHeader = "bearer "& token
			
			
			if( trim(dados) = "" )then
				r = GETDATA(url, dados)
			else
				r = POST(url, dados, metodo)
			end if
			resultado = New System.Web.Script.Serialization.JavaScriptSerializer().Deserialize(Of Object)( r )
			
			return r
		End Function
		
		'Método de envio de video 
		'Basic: up to 500MB of video per week.
		'Plus: up to 5GB of video per week.
		'PRO: up to 20GB of video per week.
		Public Function upload() as String
			Dim r as String = ""
			
			'Atalho para o usuário atual: /me
			'Endpoint:https://api.vimeo.com/me
			
			'Pegando o tiket de upload
			r = DOIT("https://api.vimeo.com/me/videos", "type=POST&redirect_url=http://www.spacelab.com.br/testes/vimeo")
			
			'Destrinchando o resultado
			
			
			
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
				Case "PATCH" : wr.Method = "PATCH"
			End Select
			
			'wr.Credentials = new NetworkCredential(psEmail, psToken)
			
			Dim postData As String = dados
			Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
			
			if authHeader <> "" then
				wr.Headers.Add("Authorization", authHeader)
			end if
			
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
				if authHeader <> "" then
					client.Headers.Add("Authorization", authHeader)
				end if
				r = client.DownloadString(url & IIF( trim(dados) <> "", "?"& dados, ""))
			Catch ex as WebException
				
				Dim res = ex.Response
				Dim sr as StreamReader = new StreamReader(res.GetResponseStream())
				Dim returnvalue as String = sr.ReadToEnd()
				r = returnvalue
				
			End Try
			
			return r
		End Function
		'----------------------------------------------------------
	
		
		'Codifica em base 64
		Public Function base64Encode(ByVal sData as String) As String
			
			Dim r As String = ""
			
			try
				
				Dim encData_byte() as byte 				 
				encData_byte = System.Text.Encoding.UTF8.GetBytes(sData) 
				 
				Dim encodedData as String = Convert.ToBase64String(encData_byte)
				r = encodedData
			 
			catch ex As Exception
				r = ex.ToString()
			end try
			
			return r
			
		End Function		
		
		
		'Decodifica base 64
		Public Function base64Decode(ByVal sData as String) As String
			
			Dim todecode_byte() as byte = Convert.FromBase64String(sData)
			 
			Dim r as String '= new String(decoded_char)
			r = System.Text.Encoding.UTF8.GetString(todecode_byte)
			
			return r
			
		End Function
		
		
	
End Class
End Namespace