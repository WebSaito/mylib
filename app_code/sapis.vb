Imports System
Imports System.Collections
Imports System.IO
Imports System.Net

Namespace SAPI
	
	Public Class CLI1
		Inherits objCliApi
		Implements IDisposable
		
		Public erros as String = ""
		
		Public Sub New()
			contentType = "application/x-www-form-urlencoded"
		End Sub
		
		Public Sub Dispose() Implements System.IDisposable.Dispose
			cliente = ""
			segredo = ""
			token = ""
			retorno = ""
			tipoRetorno = ""
			'contentType = ""
			'---
			erros = ""
		End Sub
		
		'----------------------------------------------------------
		'Método que faz transferencia de dados via webRequest
		Public Function SEND(byval url as String, ByVal dados as String, Optional ByVal pmetodo as String = "POST") As String
			Dim r as String = ""
			
			'---
			'CASO GET
			if pmetodo = "GET" then	
				
				Dim client as WebClient = new WebClient()
				Try
					
					for each x as objCliHeader in headers
						client.headers.add(x.chave , x.valor)
					next
					
					r = client.DownloadString(url &"?"& dados)
				Catch ex as WebException
					
					Dim res = ex.Response
					Dim sr as StreamReader = new StreamReader(res.GetResponseStream())
					Dim returnvalue as String = sr.ReadToEnd()
					r = returnvalue
					
				End Try
				
			else
			'---
			'DEMAIS CASOS
				
				Dim wr As WebRequest = WebRequest.Create(url)
				Dim dataStream As Stream
				
				Select Case(pmetodo)
					Case "POST" : wr.Method = WebRequestMethods.Http.Post'"POST"'"PUT"
					Case "PUT" : wr.Method = "PUT"
					Case "DELETE" : wr.Method = "DELETE"
				End Select
				
				Dim postData As String = dados
				Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
				
				wr.ContentType = contentType'"application/x-www-form-urlencoded"'"; charset=ISO-8859-1"'"application/json"
				wr.ContentLength = byteArray.Length
				
				for each x as objCliHeader in headers
					wr.headers.add(x.chave , x.valor)
				next
				
				dataStream = wr.GetRequestStream()
				
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
				
			end if
			
			return r
		End Function
		'----------------------------------------------------------
		
		
		
		'----------------------------------------------------------
		Public Function pegaToken()
			Dim r as String = ""
			
			
			
			return r
		End Function
		'----------------------------------------------------------
		
	End Class
	
	
	
	Public Class objCliApi
		Implements IDisposable
		
		Private sendpoint as String = ""
		Private scliente as String = ""
		Private ssegredo as String = ""
		Private stoken as String = ""
		Private sretorno as String = ""
		Private stipoRetorno as String = ""
		
		Private scontentType as String = ""
		
		Public Property endpoint() as String
			Get
				return sendpoint
			End Get
			Set(byval v as string)
				sendpoint = v
			End Set
		End Property
		
		Public Property cliente() as String
			Get
				return scliente
			End Get
			Set(byval v as string)
				scliente = v
			End Set
		End Property
		
		Public Property segredo() as String
			Get
				return ssegredo
			End Get
			Set(byval v as string)
				ssegredo = v
			End Set
		End Property
		
		Public Property token() as String
			Get
				return stoken
			End Get
			Set(byval v as string)
				stoken = v
			End Set
		End Property
		
		Public Property retorno() as String
			Get
				return sretorno
			End Get
			Set(byval v as string)
				sretorno = v
			End Set
		End Property
		
		Public Property tipoRetorno() as String
			Get
				return stipoRetorno
			End Get
			Set(byval v as string)
				stipoRetorno = v
			End Set
		End Property
		
		Public Property contentType() as String
			Get
				return scontentType
			End Get
			Set(byval v as string)
				scontentType = v
			End Set
		End Property
		
		Public headers as List(Of objCliHeader) = new List(Of objCliHeader)
		
		Public Sub New()
			
		End Sub
		
		Public Sub Dispose() Implements System.IDisposable.Dispose
			endpoint = ""
			cliente = ""
			segredo = ""
			token = ""
			retorno = ""
			tipoRetorno = ""
			contentType = ""
			headers.clear()
		End Sub
		
	End Class
	
	Public Class objCliHeader
		Implements IDisposable
		
		Private schave as String = ""
		Private svalor as String = ""
		
		Public Property chave() as String
			Get
				return schave
			End Get
			Set(byval v as string)
				schave = v
			End Set
		End Property
		
		Public Property valor() as String
			Get
				return svalor
			End Get
			Set(byval v as string)
				svalor = v
			End Set
		End Property
		
		Public Sub New()
		'---
		End Sub
		
		Public Sub Dispose() Implements System.IDisposable.Dispose
			chave = ""
			valor = ""
		End Sub
		
	End Class
	
End Namespace