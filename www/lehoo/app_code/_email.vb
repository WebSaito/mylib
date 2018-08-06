Imports System
Imports System.Web
Imports System.Web.Mail

Namespace nsEmail
	Public Class clsEmail
	
	
	
	'[INICIO] Função para enviar email com copia =================================
	Public Function funcEnviaReplyTo2(ByVal cmpDE As String, ByVal cmpPara As String, ByVal strEmailReplyTo As String, ByVal strEmailReturnPath As String, ByVal cmpAssunto As String, ByVal cmpMsn As String, ByVal textHtml As String, ByVal emailBcc As String, ByVal emailCC As String)
		Dim emailX As Exception
		Try
			If Not cmpPara Is Nothing Then
				Dim mailer As New MailMessage
				
				mailer.From = cmpDE.Replace(" ", "")
				
				mailer.To = cmpPara.Replace(" ", "")
				
				If Not emailBCC Is Nothing Then
					mailer.Bcc = emailBCC
				end if
				
				If Not emailCC Is Nothing Then
					mailer.cc = emailCC
				end if
				
				mailer.Subject = cmpAssunto
				mailer.Headers.Add("Reply-To", strEmailReplyTo)
				mailer.Headers.Add("Return-Path", strEmailReturnPath)
				
				If textHtml = "sim" Then
					mailer.BodyFormat = MailFormat.Html
				End If
				mailer.Body = cmpMsn
				SmtpMail.SmtpServer = "smtp2.locaweb.com.br"
				SmtpMail.Send(mailer)
				
			End If
			Return "ok"
		Catch ex as exception
			Return "erro ao enviar" & ex.toString()
		End Try
	End Function
	'[FIM] Função para enviar email com copia oculta e anexo ====================================
	
	'[INICIO] Função para enviar email com copia =================================
	Public Function funcEnviaReplyTo2(ByVal cmpDE As String, ByVal cmpPara As String, Byval strNomeReply as String, ByVal strEmailReplyTo As String, ByVal strEmailReturnPath As String, ByVal cmpAssunto As String, ByVal cmpMsn As String, ByVal textHtml As String, ByVal emailBcc As String, ByVal emailCC As String)
		Dim emailX As Exception
		Try
			If Not cmpPara Is Nothing Then
				Dim mailer As New MailMessage
				
				mailer.From = strNomeReply &"<"& cmpDE.Replace(" ", "") &">"
				
				mailer.To = cmpPara.Replace(" ", "")
				
				If Not emailBCC Is Nothing Then
					mailer.Bcc = emailBCC
				end if
				
				If Not emailCC Is Nothing Then
					mailer.cc = emailCC
				end if
				
				mailer.Subject = cmpAssunto
				mailer.Headers.Add("Reply-To", strEmailReplyTo)
				mailer.Headers.Add("Return-Path", strEmailReturnPath)
				
				If textHtml = "sim" Then
					mailer.BodyFormat = MailFormat.Html
				End If
				mailer.Body = cmpMsn
				SmtpMail.SmtpServer = "smtp2.locaweb.com.br"
				SmtpMail.Send(mailer)
				
			End If
			Return "ok"
		Catch
			Return "erro ao enviar"
		End Try
	End Function
	'[FIM] Função para enviar email com copia oculta e anexo ====================================
	
		'[INICIO] funcEnviaReplyTo2 > HELP ============================================================================================
		public function funcEnviaReplyTo2(byval strTxt as string, Byval strTxt2 as Boolean) as string
			
			dim helpRetorno as string 				'Retorno do Texto Descritivo					
			
			If strTxt = "help" and strTxt2 = true Then			
			
				dim helpDescricao as string 		'Para que serve
				dim helpParametros as string		'parametro
				dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
				dim helpExemplo as string			'exemplo de como deve ser chamado
				
				helpDescricao 		 = "Função para enviar email com Reply-To e com Cópia Oculta"
				helpParametros 		 = "(ByVal cmpDE As String, ByVal cmpPara As String, ByVal strEmailReplyTo As String, ByVal strEmailReturnPath As String, ByVal cmpAssunto As String, ByVal cmpMsn As String, ByVal textHtml As String, ByVal emailBcc As String, ByVal emailCC As String)"
				helpFuncionalidades  = "cmpDe: Email de Remetente"
				helpFuncionalidades  += "<br/>"+"cmpPara: Email de Destino"				
				helpFuncionalidades  += "<br/>"+"strEmailReplyTo: Email Remetente que sobrepõe o Remetente "								
				helpFuncionalidades  += "<br/>"+"strEmailReturnPath: Caso o Email não seja enviado, sera disparado notificação para o email informado neste parametro"												
				helpFuncionalidades  += "<br/>"+"cmpAssunto: Assunto do Email"												
				helpFuncionalidades  += "<br/>"+"cmpMsn: Corpo do Email"																
				helpFuncionalidades  += "<br/>"+"textHtml: Parametro que permite compilar tags HTML ou não ('1' para sim '0' para não)"																				
				helpFuncionalidades  += "<br/>"+"emailBcc: Envia cópia INVISÍVEL para este email "																												
				helpFuncionalidades  += "<br/>"+"emailCC: Envia cópia VISÍVEL para este email "																								
				helpExemplo 		 = "nsSqlServer.clsSqlServer.funcEnviaReplyTo2('remetente@spacelab.com.br', 'destino@spacelab', 'replay-to-remetente@spacelab.com.br', 'teste@spacelab.com.br', 'Enviando Email com Cópia Oculta', 'Mensagem', '1', 'spacelab@spacelab.com.br', 'fulano@spacelab.com.br;siclano@spacelab.com.br')"			
				
				helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
				helpRetorno += "# Parametros: " & helpParametros & "<br/>"
				helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
				helpRetorno += "# Exemplo: " & helpExemplo 
				
			end if	
			
			return helpRetorno
			
		end function
		'[FIM] funcEnviaReplyTo2 > HELP ===============================================================================================			
		
		'[INICIO] Função para Enviar email com Reply To =============================================================
		Public Function funcEnviaReplyTo(ByVal de As String, ByVal para As String , ByVal reply As String, ByVal assunto As String, ByVal msg As String) As String

			Dim resultado As String = ""
			
			Try
				'Instancia o Objeto Email como MailMessage 
				Dim Email As MailMessage = new MailMessage() 
				 
				'Atribui ao método From o valor do Remetente 
				Email.From = de
				 
				'Atribui ao método To o valor do Destinatário 
				Email.To = para
				
				Email.Headers.Add("Reply-To", reply)

				'Atribui ao método Cc o valor do com Cópia 
				'Email.Cc = "email2@dominio"; 
				 
				'Atribui ao método Bcc o valor do com Cópia oculta 
				'Email.Bcc = "email3@dominio"; 
				 
				'Atribui ao método Subject o assunto da mensagem 
				Email.Subject = assunto
				 
				'Define o formato da mensagem que pode ser Texto ou Html 
				Email.BodyFormat = MailFormat.Html
				 
				'Atribui ao método Body a texto da mensagem 
				Email.Body = msg
				 
				'Define qual a url que deve ser usada como caminho para as imagens informadas no código html 
				Email.UrlContentBase = "http:'www.steff.com.br"
				 
				'Define qual o host a ser usado para envio de mensagens. 
				SmtpMail.SmtpServer = "localhost" 
				 
				'Envia a mensagem baseado nos dados do objeto Email 
				SmtpMail.Send(Email)
				 
				'Escreve no label que a mensagem foi enviada 
				resultado = "ok"
			Catch ex As Exception
				resultado = ex.ToString()
				Throw ex
			End Try
			
			return resultado
		End Function
		
		
		'[INICIO] Função para Enviar email com Reply To =============================================================
		Public Function funcEnviaReplyTo(ByVal de As String, ByVal para As String , ByVal replyName as String, ByVal reply As String, ByVal assunto As String, ByVal msg As String) As String

			Dim resultado As String = ""
			
			Try
				'Instancia o Objeto Email como MailMessage 
				Dim Email As MailMessage = new MailMessage() 
				 
				'Atribui ao método From o valor do Remetente 
				Email.From = de
				 
				'Atribui ao método To o valor do Destinatário 
				Email.To = para
				
				Email.Headers.Add(replyName, reply)

				'Atribui ao método Cc o valor do com Cópia 
				'Email.Cc = "email2@dominio"; 
				 
				'Atribui ao método Bcc o valor do com Cópia oculta 
				'Email.Bcc = "email3@dominio"; 
				 
				'Atribui ao método Subject o assunto da mensagem 
				Email.Subject = assunto
				 
				'Define o formato da mensagem que pode ser Texto ou Html 
				Email.BodyFormat = MailFormat.Html
				 
				'Atribui ao método Body a texto da mensagem 
				Email.Body = msg
				 
				'Define qual a url que deve ser usada como caminho para as imagens informadas no código html 
				Email.UrlContentBase = "http:'www.steff.com.br"
				 
				'Define qual o host a ser usado para envio de mensagens. 
				SmtpMail.SmtpServer = "localhost" 
				 
				'Envia a mensagem baseado nos dados do objeto Email 
				SmtpMail.Send(Email)
				 
				'Escreve no label que a mensagem foi enviada 
				resultado = "ok"
			Catch ex As Exception
				resultado = ex.ToString()
				Throw ex
			End Try
			
			return resultado
		End Function
		
		
		
		'Help da função
		Public Function funcEnviaReplyTo(ByVal c As String, ByVal tf As Boolean) As String

			Dim r As String = ""
			
			If c = "help" and tf = true then
				
				Dim helpDescricao As String = ""		'Para que serve
				Dim helpParametros As String = ""		'parametro
				Dim helpFuncionalidades As String = ""	'como usar os parametros e tipo de dados
				Dim helpExemplo As String = ""			'exemplo de como deve ser chamado
				
				helpDescricao 		 = "Método utilizado para enviar email com parametro de resposta reply to."
				helpParametros 		 = "(ByVal de As String, ByVal para As String , ByVal reply As String, ByVal assunto As String, ByVal msg As String)"			
				helpFuncionalidades  += "de: String contendo endereço do remetente ("& chr(34) &"teste@spacelab.com.br"& chr(34) &").<br/>"
				helpFuncionalidades  += "para: String contendo endereço do destinatario ("& chr(34) &"fulano@spacelab.com.br"& chr(34) &").<br/>"
				helpFuncionalidades  += "reply: String contendo endereço de resposta ("& chr(34) &"receberesposta@spacelab.com.br"& chr(34) &").<br/>"
				helpFuncionalidades  += "assunto: String contendo o assunto do e-mail.<br/>"
				helpFuncionalidades  += "msg: Corpo do email.<br/>"
				helpExemplo 		 += "funcEnviaReplyTo("& chr(34) &"de@email.com"& chr(34) &","& chr(34) &"para@email.com"& chr(34) &","& chr(34) &"responder.para@email.com"& chr(34) &","& chr(34) &"Assunto da mensagem"& chr(34) &","& chr(34) &"A mensagem vai aqui neste parametro"& chr(34) &")"
				
				r  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
				r += "# Parametros: " & helpParametros & "<br/>"
				r += "# Funcionalidades: <br/>" & helpFuncionalidades & "<br/>"
				r += "# Exemplo: " & helpExemplo 
				
			End If
			
			return r
		End Function
		'[FIM] Função para Enviar email com Reply To  =============================================================
		
		
		'[INICIO] Função para Enviar email com Reply To para mais destinos  =============================================================
		Public Function funcEnviaReplyToC(ByVal de As String, ByVal para As String , ByVal reply As String, ByVal copia As String, ByVal assunto As String, ByVal msg As String) As String

			Dim resultado As String = ""
			
			Try
				'Instancia o Objeto Email como MailMessage 
				Dim Email As MailMessage = new MailMessage() 
				 
				'Atribui ao método From o valor do Remetente 
				Email.From = de
				 
				'Atribui ao método To o valor do Destinatário 
				Email.To = para
				 
				Email.Headers.Add("Reply-To", reply &";"& copia)
				
				'Email.Headers.Add("Reply-To", copia)
				 
				'Atribui ao método Cc o valor do com Cópia 
				'Email.Cc = copia
				 
				'Atribui ao método Bcc o valor do com Cópia oculta 
				'Email.Bcc = "email3@dominio"; 
				 
				'Atribui ao método Subject o assunto da mensagem 
				Email.Subject = assunto
				 
				'Define o formato da mensagem que pode ser Texto ou Html 
				Email.BodyFormat = MailFormat.Html
				 
				'Atribui ao método Body a texto da mensagem 
				Email.Body = msg
				 
				'Define qual a url que deve ser usada como caminho para as imagens informadas no código html 
				Email.UrlContentBase = "http:'www.steff.com.br"
				 
				'Define qual o host a ser usado para envio de mensagens. 
				SmtpMail.SmtpServer = "localhost" 
				 
				'Envia a mensagem baseado nos dados do objeto Email 
				SmtpMail.Send(Email)
				 
				'Escreve no label que a mensagem foi enviada 
				resultado = "ok"
			Catch ex As Exception
				resultado = ex.ToString()
				Throw ex
			End Try
			
			return resultado
		End Function
		
		
		Public Function funcEnviaReplyToC(ByVal c As String, ByVal tf As Boolean) As String
			Dim r As String = ""
			
			If c = "help" and tf = true then
				
				Dim hd As String = ""		'Para que serve
				Dim hp As String = ""		'parametro
				Dim hf As String = ""	'como usar os parametros e tipo de dados
				Dim he As String = ""			'exemplo de como deve ser chamado
				
				hd 		 = "Método utilizado para enviar email com parametro de resposta reply to para mais destino(s)."
				hp 		 = "(ByVal de As String, ByVal para As String , ByVal reply As String, ByVal copia As String, ByVal assunto As String, ByVal msg As String)"			
				hf  += "de: String contendo endereço do remetente ("& chr(34) &"teste@spacelab.com.br"& chr(34) &").<br/>"
				hf  += "para: String contendo endereço do destinatario ("& chr(34) &"fulano@spacelab.com.br"& chr(34) &").<br/>"
				hf  += "reply: String contendo endereço de resposta ("& chr(34) &"receberesposta@spacelab.com.br"& chr(34) &").<br/>"
				hf  += "copia: String contendo endereço(s) para cópia de resposta ("& chr(34) &"receberesposta@spacelab.com.br"& chr(34) &").<br/>"
				hf  += "assunto: String contendo o assunto do e-mail.<br/>"
				hf  += "msg: Corpo do email.<br/>"
				he 		 += "funcEnviaReplyToC("& chr(34) &"de@email.com"& chr(34) &","& chr(34) &"para@email.com"& chr(34) &","& chr(34) &"responder.para@email.com"& chr(34) &","& chr(34) &"recebe.resposta.tambem@email.com"& chr(34) &","& chr(34) &"Assunto da mensagem"& chr(34) &","& chr(34) &"A mensagem vai aqui neste parametro"& chr(34) &")"
				
				r  = "# Descri&ccedil;&atilde;o: " & hd & "<br/>"
				r += "# Parametros: <br/>" & hp & "<br/>"
				r += "# Funcionalidades: <br/>" & hf & "<br/>"
				r += "# Exemplo: " & he 
				
			End If
			
			return r
		End Function
		'[FIM] Função para Enviar email com Reply To para mais destinos  =============================================================
		
		
		'[INICIO] Função para Enviar email com Reply To para mais destinos  =============================================================
		Public Function funcEnviaAnexo(ByVal de As String, ByVal para As String , ByVal reply As String, ByVal assunto As String, ByVal msg As String, ByVal arquivo As String) As String

			Dim resultado As String = ""
			
			Try
				'Instancia o Objeto Email como MailMessage 
				Dim Email As MailMessage = new MailMessage() 
				Dim Anexo As MailAttachment = new MailAttachment(System.Web.Hosting.HostingEnvironment.MapPath("~"& arquivo))
				
				'Anexando arquivo
				Email.Attachments.Add(Anexo)
				
				'Atribui ao método From o valor do Remetente 
				Email.From = de
				 
				'Atribui ao método To o valor do Destinatário 
				Email.To = para
				 
				Email.Headers.Add("Reply-To", reply)
				 
				'Atribui ao método Cc o valor do com Cópia 
				'Email.Cc = "email2@dominio"; 
				 
				'Atribui ao método Bcc o valor do com Cópia oculta 
				'Email.Bcc = "email3@dominio"; 
				 
				'Atribui ao método Subject o assunto da mensagem 
				Email.Subject = assunto
				 
				'Define o formato da mensagem que pode ser Texto ou Html 
				Email.BodyFormat = MailFormat.Html
				 
				'Atribui ao método Body a texto da mensagem 
				Email.Body = msg
				 
				'Define qual a url que deve ser usada como caminho para as imagens informadas no código html 
				Email.UrlContentBase = "http:'www.spacelab.com.br"
				 
				'Define qual o host a ser usado para envio de mensagens. 
				SmtpMail.SmtpServer = "localhost" 
				 
				'Envia a mensagem baseado nos dados do objeto Email 
				SmtpMail.Send(Email)
				 
				'Escreve no label que a mensagem foi enviada 
				resultado = "ok"
			Catch ex As Exception
				resultado = ex.ToString()
				Throw ex
			End Try
			
			return resultado
		End Function
		
		
		Public Function funcEnviaAnexo(ByVal c As String, ByVal tf As Boolean) As String
			Dim r As String = ""
			
			If c = "help" and tf = true then
				
				Dim hd As String = ""		'Para que serve
				Dim hp As String = ""		'parametro
				Dim hf As String = ""	'como usar os parametros e tipo de dados
				Dim he As String = ""			'exemplo de como deve ser chamado
				
				hd 		 = "Método utilizado para enviar email com parametro de resposta reply to para mais destino(s)."
				hp 		 = "(ByVal de As String, ByVal para As String , ByVal reply As String, ByVal assunto As String, ByVal msg As String, ByVal arquivo As String)"			
				hf  += "de: String contendo endereço do remetente ("& chr(34) &"teste@spacelab.com.br"& chr(34) &").<br/>"
				hf  += "para: String contendo endereço do destinatario ("& chr(34) &"fulano@spacelab.com.br"& chr(34) &").<br/>"
				hf  += "reply: String contendo endereço de resposta ("& chr(34) &"receberesposta@spacelab.com.br"& chr(34) &").<br/>"
				hf  += "assunto: String contendo o assunto do e-mail.<br/>"
				hf  += "msg: Corpo do email.<br/>"
				hf  += "arquivo: String contendo caminho do arquivo anexo.<br/>"
				he 		 += "funcEnviaAnexo("& chr(34) &"de@email.com"& chr(34) &","& chr(34) &"para@email.com"& chr(34) &","& chr(34) &"responder.para@email.com"& chr(34) &","& chr(34) &"Assunto da mensagem"& chr(34) &","& chr(34) &"A mensagem vai aqui neste parametro"& chr(34) &","& chr(34) &"/pasta/sub-pasta/arquivo.ext"& chr(34) &")"			
				
				r  = "# Descri&ccedil;&atilde;o: " & hd & "<br/>"
				r += "# Parametros: <br/>" & hp & "<br/>"
				r += "# Funcionalidades: <br/>" & hf & "<br/>"
				r += "# Exemplo: " & he 
				
			End If
			
			return r
		End Function		
		'[FIM] Função para Enviar email com Reply To para mais destinos  =============================================================
		
	End Class
End Namespace