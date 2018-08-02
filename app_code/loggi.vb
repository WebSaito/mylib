Imports System
Imports System.IO
Imports System.Data
Imports System.Net
Imports System.Collections.Specialized

Namespace Motoboy
	Public Class loggi
		Implements IDisposable
		
		
		'Endpoints
		Public mode as Integer = 0  '0 teste/1 producao
		
		Public host() as String = {"staging.loggi.com", "www.loggi.com"}
		Public entrypoint() as String = {"https://staging.loggi.com/public-graphql", "https://www.loggi.com/public-graphql"}
		Public loginpoint() as String = {"https://staging.loggi.com/graphql", "https://www.loggi.com/graphql"}
		
		Public logEmail as String = "websaito07@gmail.com"
		Public loglKey as String = "" 'Credencial / Token 
		Public apiKey as String = "f7754b0c9a075fd25f971ed53b338d3794d039e1"
		
		Public resultado as String = ""
		
		'Construtorzao
		Public Sub New()
			
		End Sub
		
		'Dispose
		Public Sub Dispose() Implements System.IDisposable.Dispose
			'Limpeza de atributos aqui
			
			resultado = ""
			
		End Sub
		
		
		Public Function query(ByVal strQuery as String) as Boolean
			Dim r as boolean = false
			
			resultado = POST( entrypoint(mode), strQuery )
			
			return r
		End Function
		
		
		
		
		
		
		'----------------------------------------------------------
		'Métodos auxiliares:
		
		
		'----------------------------------------------------------
		'Método que faz um web POST
		Public Function POST(byval url as String, ByVal dados as String, Optional ByVal metodo as String = "POST", Optional ByVal tipo as String = "QUERY") As String
			Dim r as String = ""
			
			Dim wr As WebRequest = WebRequest.Create(url)
			
			Select Case(metodo)
				Case "POST" : wr.Method = WebRequestMethods.Http.Post'"POST"'"PUT"
				Case "PUT" : wr.Method = "PUT"
			End Select
			
			'wr.Credentials = new NetworkCredential(psEmail, psToken)
			
			Dim postData As String = dados
			Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
			wr.ContentType = "application/json;"'"application/json; charset=UTF-8"
			wr.ContentLength = byteArray.Length
			wr.Headers.add("autorization", logEmail &"."& apiKey )
			'wr.Headers.add("Host", host(mode))
			
			
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
		
	End Class
End Namespace