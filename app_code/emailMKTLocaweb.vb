Imports System
Imports System.Net
Imports System.Net.Security
Imports System.Net.Security.SslPolicyErrors
Imports System.IO
Imports System.Text
Imports System.Web
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.Serialization.Json
Imports System.Web.Script.Serialization

Namespace emailMKTLocaweb
	
	Public Class customFields
		
		Private strNome As String =  ""
		Private strTelefone As String = ""
		
		Public Property nome() As String
			Get
				Return strNome
			End Get
			Set(ByVal valor As String)
				strNome = valor
			End Set
		End Property
		
		Public Property telefone() As String
			Get
				return strTelefone
			End Get
			Set(ByVal valor As String)
				strTelefone = valor
			End Set
		End Property
		
		Public Function customFields()
			
		End Function
		
	End Class
	
	Public Class dadosContato
	
		Private strEmail As String = ""
		Private strLT
		
		Public Property email() As String
			Get
				return strEmail
			End Get
			Set(ByVal valor As String)
				strEmail = valor
			End Set
		End Property
		
		Public Property list_tokens() As String
			Get
				return strLT
			End Get
			Set(ByVal valor As String)
				strLT = valor
			End Set
		End Property
		
		Public custom_fields As customFields = New customFields()
		
		Public Function dadosContato()

		End Function
		
	End Class
	
	
	Public Class lista
		
		Public list As contatos = new contatos()
		
		Public Function lista()
			
		End Function
		
		Public Function enviaLocaweb(ByVal url As String, ByVal texto As String, ByVal chave As String) As String
		
			Dim resultado As String
            Dim requestStream as Stream
            Dim wres as WebResponse
            Dim reader as StreamReader 

            Try
            	
                Dim wreq as WebRequest = WebRequest.Create(url)
				
                'wreq.Method = WebRequestMethods.Http.Post 'WebRequestMethods.Http.Get
				wreq.Headers.Add("X-Auth-Token", chave)
                wreq.ContentType = "application/json; charset=utf-8"
	            'wreq.Accept = "application/json, text/javascript, */*"
            	wreq.Method = "POST"
				'wreq.SendChunked = true
				'ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AddressOf VCertificadoRemoto)
					
				
				Dim jss As JavaScriptSerializer = new JavaScriptSerializer()
				Dim sbRes As StringBuilder = new StringBuilder()
				jss.Serialize(list, sbRes)
				
				Dim urlEncoded as String = ""

                Dim reserved() as Char = { "?", "=", "&amp;" }
                Dim byteBuffer() as byte
				
				'---------------------------------------------------------------------<
				Dim tempTexto As String = replace(texto,"'",chr(34)) 
				
                byteBuffer = System.Text.Encoding.UTF8.GetBytes(tempTexto)'sbRes.ToString())'GetEncoding("ISO-8859-1").GetBytes(urlEncoded.ToString())	
                wreq.ContentLength = byteBuffer.Length
                requestStream = wreq.GetRequestStream()
                requestStream.Write(byteBuffer, 0, byteBuffer.Length)
                requestStream.Close()
              
                wres = wreq.GetResponse()
                Dim responseStream as Stream = wres.GetResponseStream()

                Dim encoding as System.Text.Encoding = System.Text.Encoding.UTF8'Default

                ' Preenche o reader
                reader = new StreamReader(responseStream, encoding)

                Dim charBuffer(256) As Char
                Dim contador As Integer = reader.Read(charBuffer, 0, charBuffer.Length)

                Dim Dados as String = ""

                While (contador > 0)
                    Dados &= new String(charBuffer, 0, contador)
                    contador = reader.Read(charBuffer, 0, charBuffer.Length)
                End While

                resultado = Dados
            
            Catch e As Exception
                resultado = e.ToString()
			End Try
			
			return resultado
        End Function
		
		Private Function VCertificadoRemoto(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As 			X509Chain, ByVal policyErrors As SslPolicyErrors) As Boolean
			return true
		End Function
		
	End Class

	Public Class contatos
		
		Public contacts As List(Of dados) = new List(Of dados)
	   
		Private strOA As Boolean = true
		
		Public Property overwriteattributes() As Boolean
			Get
				return strOA
			End Get
			Set(ByVal valor As Boolean)
				strOA = valor
			End Set
		End Property
		
		Public Function contatos()
			
		End Function
		
	End Class
	
	Public Class dados
			
		Public custom_fields As customFields = New customFields()
		
		Private strEmail As String = ""
		
		Public Property email() As String
			Get
				return strEmail
			End Get
			Set(ByVal valor As String)
				strEmail = valor
			End Set
		End Property
		
		Public Sub New(ByVal tEmail As String, ByVal tNome As String, ByVal tTel As String) 
			email = tEmail
			custom_fields.nome = tNome
			custom_fields.telefone = tTel
		End Sub
		
	End Class
	
	
	Public Class contato
		
		Public contact As dadosContato = new DadosContato()
		
		Public Function contato()

		End Function
		
		Public Function enviaLocaweb(ByVal url As String, ByVal chave As String) As String
		
			Dim resultado As String
            Dim requestStream as Stream
            Dim wres as WebResponse
            Dim reader as StreamReader 

            Try
            	
                Dim wreq as WebRequest = WebRequest.Create(url)
				
                'wreq.Method = WebRequestMethods.Http.Post 'WebRequestMethods.Http.Get
				wreq.Headers.Add("X-Auth-Token", chave)
                wreq.ContentType = "application/json; charset=utf-8"
	            'wreq.Accept = "application/json, text/javascript, */*"
            	wreq.Method = "POST"
				'wreq.SendChunked = true
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AddressOf VCertificadoRemoto)
					
				
				Dim jss As JavaScriptSerializer = new JavaScriptSerializer()
				Dim sbRes As StringBuilder = new StringBuilder()
				jss.Serialize(contact, sbRes)
				
				Dim urlEncoded as String = ""

                Dim reserved() as Char = { "?", "=", "&amp;" }
                Dim byteBuffer() as byte
				
				
				
                byteBuffer = System.Text.Encoding.UTF8.GetBytes(sbRes.ToString())'GetEncoding("ISO-8859-1").GetBytes(urlEncoded.ToString())	
                wreq.ContentLength = byteBuffer.Length
                requestStream = wreq.GetRequestStream()
                requestStream.Write(byteBuffer, 0, byteBuffer.Length)
                requestStream.Close()
              
                wres = wreq.GetResponse()
                Dim responseStream as Stream = wres.GetResponseStream()

                Dim encoding as System.Text.Encoding = System.Text.Encoding.UTF8'Default

                ' Preenche o reader
                reader = new StreamReader(responseStream, encoding)

                Dim charBuffer(256) As Char
                Dim contador As Integer = reader.Read(charBuffer, 0, charBuffer.Length)

                Dim Dados as String = ""

                While (contador > 0)
                    Dados &= new String(charBuffer, 0, contador)
                    contador = reader.Read(charBuffer, 0, charBuffer.Length)
                End While

                resultado = Dados
            
            Catch e As Exception
                resultado = e.ToString()
			End Try
			
			return resultado
        End Function
		
		Private Function VCertificadoRemoto(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As 			X509Chain, ByVal policyErrors As SslPolicyErrors) As Boolean
			return true
		End Function
		
	End Class
	
	
End Namespace