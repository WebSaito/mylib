Imports System
Imports System.Net.Mail

Namespace email2
	Public Class clsEmail
		Inherits iConta
		Implements IDisposable
		
		Public contas as List(of iConta) = new List(of iConta)
		Private totalContas as String = "1"
		Private auxInt as Integer = 0
		'---
		'Estrutura de endereços
		Public remetente as MailAddress
		Public destinatario as List(of MailAddress) = new List(of MailAddress)
		Public responderPara as MailAddress
		Public retorno as MailAddress
		Public copiaCarbono as List(of MailAddress) = new List(of MailAddress)
		Public copiaOculta as List(of MailAddress) = new List(of MailAddress)
		'---
		Public erros as String = ""
		
		'Dispose
		Public Sub Dispose() Implements System.IDisposable.Dispose
			
			'remetente.Address = ""
			'remetente.DisplayName = ""
			'remetente.Host = ""
			'remetente.User = ""
					  
			destinatario.clear()
			'responderPara.finalize()
			'retorno.finalize()
			copiaCarbono.clear()
			copiaOculta.clear()
			
			conta = ""
			host = ""
			port = ""
			nome = ""
			email = ""
			pwd = ""

			contas.clear()
			
			erros = ""
		End Sub
		
		'Construtor
		Public Sub New()
			
			'Carregando contas do web.config
			totalContas = system.configuration.configurationManager.appSettings("strTotalEmails")
			
			For auxInt = 1 to cInt(totalContas)
				
				contas.add(new iConta( _
					System.Configuration.ConfigurationManager.AppSettings("smtp_conta"& auxInt), _
					System.Configuration.ConfigurationManager.AppSettings("smtp_address"& auxInt), _
					System.Configuration.ConfigurationManager.AppSettings("smtp_porta"& auxInt), _
					System.Configuration.ConfigurationManager.AppSettings("smtp_nome"& auxInt), _
					System.Configuration.ConfigurationManager.AppSettings("smtp_email"& auxInt), _
					System.Configuration.ConfigurationManager.AppSettings("smtp_password"& auxInt) _
				))
				
				
				if conta = "" then
					conta = System.Configuration.ConfigurationManager.AppSettings("smtp_conta"& auxInt)
					host = System.Configuration.ConfigurationManager.AppSettings("smtp_address"& auxInt)
					port = System.Configuration.ConfigurationManager.AppSettings("smtp_porta"& auxInt)
					nome = System.Configuration.ConfigurationManager.AppSettings("smtp_nome"& auxInt)
					email = System.Configuration.ConfigurationManager.AppSettings("smtp_email"& auxInt)
					pwd = System.Configuration.ConfigurationManager.AppSettings("smtp_password"& auxInt)
				end if
				
			Next
			
		End Sub
		
		
		
		'[INICIO] Função para enviar email =================================
		Public Function send(ByVal cmpPara As String, ByVal strEmailReplyTo As String, _
			ByVal strEmailReturnPath As String, ByVal cmpAssunto As String, ByVal cmpMsn As String, _ 
			ByVal textHtml As String, Optional ByVal emailBcc As String = "", Optional ByVal emailCC As String = "") as Boolean
			
			erros = ""
			
			Dim r as boolean = false
			'---
			'Validando os parametros
			if( trim(cmpPara) = "" )then
				erros &= "<br>Parametro destinatario não pode ser vazio<br> send( <strong>destinatario</strong>, responderPara, emailRetornoErros, assunto, mensagem, formatoHTML?, copiaOculta, copiaCarbono)<br>"
			end if
			
			if( trim(strEmailReplyTo) = "" )then
				erros &= "<br>Parametro responderPara não pode ser vazio<br> send( destinatario, <strong>responderPara</strong>, emailRetornoErros, assunto, mensagem, formatoHTML?, copiaOculta, copiaCarbono)<br>"
			end if
			
			if( trim(strEmailReturnPath) = "" )then
				erros &= "<br>Parametro emailRetornoErros não pode ser vazio<br> send( destinatario, responderPara, <strong>emailRetornoErros</strong>, assunto, mensagem, formatoHTML?, copiaOculta, copiaCarbono)<br>"
			end if
			
			if( trim(cmpMsn) = "" )then
				erros &= "<br>Mensagem não pode ser vazia<br> send( destinatario, responderPara, emailRetornoErros, assunto, <strong>mensagem</strong>, formatoHTML?, copiaOculta, copiaCarbono)<br>"
			end if
			
			
			'---
			if( erros = "" )then
			
				Try
					'---
					'Tratando os parametros
					remetente = new MailAddress(email, nome)
					if( inStr(strEmailReplyTo, "|") > 0 )then
						responderPara = new MailAddress( trim(strEmailReplyTo.split("|")(1)), trim(strEmailReplyTo.split("|")(0))  )
					else
						responderPara = new MailAddress( trim(strEmailReplyTo) )
					end if
					
					
						Dim mailer As New System.Net.Mail.MailMessage()
						mailer.From = remetente
						mailer.ReplyTo = responderPara
						
						dim Para
						dim splEmails = split(cmpPara, ";")
							
						if(ubound(splEmails) > 0)then
							
							dim i=0
							for i=0 to ubound(splEmails)
								if( inStr(splEmails(i), "|") > 0 )then 
									Para = new System.Net.Mail.mailAddress(	trim(splEmails(i).split("|")(1)), trim(splEmails(i).split("|")(0)) )
								else
									Para = new System.Net.Mail.mailAddress( splEmails(i).Replace(" ", "") )
								end if
								mailer.To.Add(Para)
							next
							
						else
							
							if( inStr(cmpPara, "|") > 0 )then 
								Para = new System.Net.Mail.mailAddress( trim(cmpPara.split("|")(1)) , trim(cmpPara.split("|")(0)) )
							else
								Para = new System.Net.Mail.mailAddress(cmpPara.Replace(" ", ""))
							end if
							mailer.To.Add(Para)
							
						end if
						
						'---
						'Cópias ocultas
						If Not emailBCC Is Nothing Then
							If emailBCC <> "" then
								
								Dim bcc as System.Net.Mail.mailAddress
								if( inStr(emailBcc, ";") > 0 )then
									Dim strCopias() as String = split(emailBcc, ";")
									For each s as String in strCopias
										if( inStr(s, "|") > 0 )then
											bcc = new MailAddress( trim(s.split("|")(1)), trim(s.split("|")(0)) )
										else
											bcc = new MailAddress(trim(s))
										end if
										mailer.Bcc.Add(bcc)
									Next
								else
									if( inStr(emailBcc, "|") > 0 )then
										bcc = new MailAddress( trim(emailBcc.split("|")(1)), trim(emailBcc.split("|")(0)) )
									else
										bcc = new MailAddress(trim(emailBcc))
									end if
									mailer.Bcc.Add(bcc)
								end if
								
							end if
						end if
						
						'---
						'Cópias carbono
						If Not emailCC Is Nothing Then
							If emailCC <> "" then
								
								Dim cc as System.Net.Mail.mailAddress
								if( inStr(emailCC, ";") > 0 )then
									Dim strCopias() as String = split(emailCC, ";")
									For each s as String in strCopias
										if( inStr(s, "|") > 0 )then
											cc = new MailAddress( trim(s.split("|")(1)), trim(s.split("|")(0)) )
										else
											cc = new MailAddress(trim(s))
										end if
										mailer.cc.Add(cc)
									Next
								else
									if( inStr(emailCC, "|") > 0 )then
										cc = new MailAddress( trim(emailCC.split("|")(1)), trim(emailCC.split("|")(0)) )
									else
										cc = new MailAddress(trim(emailCC))
									end if
									mailer.cc.Add(cc)
								end if
								
							end if
						end if
						
						mailer.Priority = System.Net.Mail.MailPriority.Normal
						mailer.Subject = cmpAssunto
						'mailer.Headers.Add("Reply-To", strEmailReplyTo)
						mailer.Headers.Add("Return-Path", strEmailReturnPath)
						
						If textHtml = "sim" Then  				
							mailer.IsBodyHtml = True
						End If
						mailer.Body = cmpMsn
						
						'//usado para servidores
						dim oEnviar as new System.Net.Mail.SmtpClient(host, port)', smtpPort)
						oEnviar.Host = host
						'oEnviar.EnableSsl = true
						oEnviar.UseDefaultCredentials = false
						oEnviar.Credentials = New System.Net.NetworkCredential(email, pwd)
						oEnviar.Send(mailer)
						mailer.Dispose()
						
						r = true
						
				Catch ex As Exception
					erros &= ex.toString()
				End Try
			
			end if
			
			return r
		End Function
		'[FIM] Função para enviar email ====================================
		
		
		
		
		
		
		
		'Método para selecionar conta
		Public Function SelecionaConta(byval pconta as string) as Boolean
			dim r as boolean = false
			
			if trim(pconta) <> trim(conta) then
				
				For each x as iConta in contas
					if(x.conta = pconta) then
						conta = x.conta
						host = x.host
						port = x.port
						nome = x.nome
						email = x.email
						pwd = x.pwd
						
						r = true
					end if
				Next
				
			end if
			
			return r
		End Function
		
		
	End Class
	
	
	
	Public class iConta
		Implements IDisposable
		
		private sConta as string = ""
		private sHost as string = ""
		private sPort as string = ""
		private sNome as string = ""
		private sEmail as string = ""
		private sPwd as string = ""
		
		public property conta() as string
			get
				return sConta
			end get
			set(byval v as string)
				sConta = v
			end set
		end property
		
		public property host() as string
			get
				return sHost
			end get
			set(byval v as string)
				sHost = v
			end set
		end property
		
		public property port() as string
			get
				return sPort
			end get
			set(byval v as string)
				sPort = v
			end set
		end property
		
		public property nome() as string
			get
				return sNome
			end get
			set(byval v as string)
				sNome = v
			end set
		end property
		
		public property email() as string
			get
				return sEmail
			end get
			set(byval v as string)
				sEmail = v
			end set
		end property
		
		public property pwd() as string
			get
				return sPwd
			end get
			set(byval v as string)
				sPwd = v
			end set
		end property
		
		
		
		public sub new()
			
		end sub
		
		public sub new(byval pconta as string, byval phost as string, byval pport as string, byval pnome as string, byval pemail as string, byval ppwd as string)
			conta = pconta
			host = phost
			port = pport
			nome = pnome
			email = pemail
			pwd = ppwd
		end sub

		public sub dispose() implements System.IDisposable.Dispose
			conta = ""
			host = ""
			port = ""
			nome = ""
			email = ""
			pwd = ""
		end sub
		
	End Class
	
	
	
	
	
End Namespace