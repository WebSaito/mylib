imports System
imports System.Net
Imports System.Net.Security
Imports System.Net.Security.SslPolicyErrors
imports System.IO
imports System.Text
imports System.Web
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates



Namespace SwebSender

    Public Class webPOST
		
		Public metodoEnvio As String = "POST"
		Public tipoCaracteres As String = "ISO-8859-1"
		
        Public Function Enviar(ByVal url As String, ByVal query As String) As String
			Dim resultado As String
            Dim requestStream as Stream
            Dim wres as WebResponse
            Dim reader as StreamReader 

            Try
            
                Dim wreq as WebRequest = WebRequest.Create(url)
				
				If metodoEnvio = "POST" then
                	wreq.Method = WebRequestMethods.Http.Post 'WebRequestMethods.Http.Get
				Else
					wreq.Method = WebRequestMethods.Http.Get
				End If
				'ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AddressOf VCertificadoRemoto)
				'System.Net.ServicePointManager.Expect100Continue = false
				'System.Net.ServicePointManager.Expect100Continue = true
				
                ' Neste ponto, você está setando a propriedade ContentType da página 
                ' para urlencoded para que o comando POST seja enviado corretamente
                wreq.ContentType = "application/x-www-form-urlencoded; charset="& tipoCaracteres'"application/x-www-form-urlencoded"
				
				'wreq.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
				'wreq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

                Dim urlEncoded as String = ""

                ' Separando cada parâmetro
                Dim reserved() as Char = { "?", "=", "&amp;" }

                ' alocando o bytebuffer
                Dim byteBuffer() as byte

                ' caso a URL seja preenchida
                if (query <> "") then
                
                    Dim i as Integer = 0
					Dim j as Integer = 0
                    ' percorre cada caractere da url atraz das palavras reservadas para separação
                    ' dos parâmetros
                    while (i < query.Length)
                    
                        j = query.IndexOfAny(reserved, i)
                        if (j = -1) then
                        
                            urlEncoded &= query.Substring(i, query.Length - i)
                            Exit While
                        end if
                        urlEncoded &= query.Substring(i, j - i)
                        urlEncoded &= query.Substring(j, 1)
                        i = j + 1
                    end while
                    ' codificando em UTF8 (evita que sejam mostrados códigos malucos em caracteres especiais
                    byteBuffer = System.Text.Encoding.UTF8.GetBytes(urlEncoded.ToString())

                    wreq.ContentLength = byteBuffer.Length
                    requestStream = wreq.GetRequestStream()
                    requestStream.Write(byteBuffer, 0, byteBuffer.Length)
                    requestStream.Close()
                
                Else
                
                    wreq.ContentLength = 0
                End if

                ' Dados recebidos 
                wres = wreq.GetResponse()
                Dim responseStream as Stream = wres.GetResponseStream()

                ' Codifica os caracteres especiais para que possam ser exibidos corretamente
                Dim encoding as System.Text.Encoding = System.Text.Encoding.Default

                ' Preenche o reader
                reader = new StreamReader(responseStream, encoding)

                Dim charBuffer(256) As Char
                Dim contador As Integer = reader.Read(charBuffer, 0, charBuffer.Length)

                Dim Dados as String = ""

                ' Lê cada byte para preencher meu stringbuilder
                While (contador > 0)
                    Dados &= new String(charBuffer, 0, contador)
                    contador = reader.Read(charBuffer, 0, charBuffer.Length)
                End While

                'Imprimo o que recebi
                resultado = Dados
            
            Catch e As Exception
                'Ocorreu algum erro
                resultado = e.ToString() &"<br><br><br>"& query
            Finally
                'Fechando tudo
                'requestStream.Close()
                'wres.Close()
                'reader.Close()
            End Try
			
			return resultado
        End Function
		
		
		Public Function executar(ByVal pagina As String) As String
			Dim r As String = ""
			
			Try

				Dim wC As WebClient = new WebClient()
				'Dim stream As Stream = wC.OpenRead(URI)
				'r = reader.ReadToEnd()
				r = wC.DownloadString(pagina)
				
			Catch ex As Exception
				r = ex.ToString()
			End Try
			
			return r
		End Function
		
		
		Private Function VCertificadoRemoto(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As 			X509Chain, ByVal policyErrors As SslPolicyErrors) As Boolean
			return true
		End Function
		
    End Class
End Namespace