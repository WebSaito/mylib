Imports System
Imports System.Data
Imports System.Text
Imports System.Web

Namespace nsValidadores

 Public Class clsValidadores
	    
   '[INICIO] Valida CPF ===============================================================================================================
	public function funcValidaCPF(cpf)
		
		dim resultado, cpf2, dv2, i, soma, dvs, dv1		
		
		cpf = replace(replace(cpf, ".", ""), "-", "")
		
		if (cpf = "") then 
			resultado = false
		elseif not isnumeric(cpf)then
			
			resultado = false
			
		else
		
			if (len(cpf)=11) then 		
							
				for i = 1 to 9
				soma = soma + (11-i)*(int(mid(cpf,i,1)))
				next
				dv1 = 11 - (soma Mod 11)
				if (dv1 = 10) or (dv1=11) then dv1=0
				cpf2 = cpf & dv1
				soma = 0
				for i = 1 to 10
				soma = soma + (12-i)*(int(mid(cpf2,i,1)))
				dv2 = 11 - (soma Mod 11)
				next
				if (dv2 = 10) or (dv2=11) then dv2=0
				dvs = cstr(dv1) & cstr(dv2)
	
				if dvs = mid(cpf,10,2) then resultado = true else resultado = false
				
				dim numero
				dim posicao
				dim erro
				dim contErro = 0
				for numero = 0 to 9							
					for posicao = 1 to 11							
						if cStr(Mid(cpf, posicao, 1)) = cStr(numero) then									
							contErro = contErro + 1								
						end if															
					next
					if cInt(contErro) = 11 then 
						resultado = false
					end if
					contErro = 0
				next 												
								
			else
				resultado = false
			end if
		
		end if

'		Dim rEx As Regex
'		if(rEx.IsMatch(cpf, "0{11,}|1{11,}|2{11,}|3{11,}|4{11,}|5{11,}|6{11,}|7{11,}|8{11,}|9{11,}") = True) then
'			resultado = True
'		else
'			resultado = False
'		end if
		
		return resultado 		
		
	end function
   '[FIM] Valida CPF ==================================================================================================================
   
	'[INICIO] funcValidaCPF > HELP =============================================================================================
	public function funcValidaCPF(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then			
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Função para validar CPF. Retorna True quando for válido e False quando for inválido"
			helpParametros 		 = "(cpf)"
			helpFuncionalidades  = "cpf: texto com CPF desejado"
			helpExemplo 		 = "nsSqlServer.clsSqlServer.funcValidaCPF('37817359822')"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] funcValidaCPF > HELP ================================================================================================		

'	[INICIO] valida CNPJ ==============================================================================================================
	function funcValidaCNPJ(cnpj)
	
		Dim RecebeCNPJ, Numero(14), soma, resultado1, resultado2
		Dim s, x, ch
		
		RecebeCNPJ = cnpj
		
		if RecebeCNPJ = "98765432123456" then
			return true
		end if
		
		s="" 
		for x=1 to len(RecebeCNPJ)
				ch=mid(RecebeCNPJ,x,1)
			if asc(ch)>=48 and asc(ch)<=57 then
				s=s & ch
			end if
		next
		RecebeCNPJ = s
		
		if len(RecebeCNPJ) <> 14 then
			return false
		elseif RecebeCNPJ = "00000000000000" then
			return false
		else
		
		Numero(1) = Cint(Mid(RecebeCNPJ,1,1))
		Numero(2) = Cint(Mid(RecebeCNPJ,2,1))
		Numero(3) = Cint(Mid(RecebeCNPJ,3,1))
		Numero(4) = Cint(Mid(RecebeCNPJ,4,1))
		Numero(5) = Cint(Mid(RecebeCNPJ,5,1))
		Numero(6) = CInt(Mid(RecebeCNPJ,6,1))
		Numero(7) = Cint(Mid(RecebeCNPJ,7,1))
		Numero(8) = Cint(Mid(RecebeCNPJ,8,1))
		Numero(9) = Cint(Mid(RecebeCNPJ,9,1))
		Numero(10) = Cint(Mid(RecebeCNPJ,10,1))
		Numero(11) = Cint(Mid(RecebeCNPJ,11,1))
		Numero(12) = Cint(Mid(RecebeCNPJ,12,1))
		Numero(13) = Cint(Mid(RecebeCNPJ,13,1))
		Numero(14) = Cint(Mid(RecebeCNPJ,14,1))
		
		soma = Numero(1) * 5 + Numero(2) * 4 + Numero(3) * 3 + Numero(4) * 2 + Numero(5) * 9 + Numero(6) * 8 + Numero(7) * 7 + Numero(8) * 6 + Numero(9) * 5 + Numero(10) * 4 + Numero(11) * 3 + Numero(12) * 2
		
		soma = soma -(11 * (int(soma / 11)))
		
		if soma = 0 or soma = 1 then
			resultado1 = 0
		else
			resultado1 = 11 - soma
		end if
		if resultado1 = Numero(13) then
			soma = Numero(1) * 6 + Numero(2) * 5 + Numero(3) * 4 + Numero(4) * 3 + Numero(5) * 2 + Numero(6) * 9 + Numero(7) * 8 + Numero(8) * 7 + Numero(9) * 6 + Numero(10) * 5 + Numero(11) * 4 + Numero(12) * 3 + Numero(13) * 2
			soma = soma - (11 * (int(soma/11)))
			if soma = 0 or soma = 1 then
				resultado2 = 0
			else
				resultado2 = 11 - soma
			end if
			if resultado2 = Numero(14) then
				return true '""
			else
				return false'"Preencha corretamente o campo CNPJ"
			end if
			else
				return false'"Preencha corretamente o campo CNPJ"
			end if
		end if
	end function
'	[FIM] valida CNPJ =================================================================================================================		

	'[INICIO] funcValidaCNPJ > HELP ===================================================================================================
	public function funcValidaCNPJ(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then			
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Função para validar CNPJ. Retorna True quando for válido e False quando for inválido"
			helpParametros 		 = "(cnpj)"
			helpFuncionalidades  = "cnpj: texto com CNPJ desejado"
			helpExemplo 		 = "nsSqlServer.clsSqlServer.funcValidaCNPJ('81489311000168')"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] funcValidaCNPJ > HELP =====================================================================================================		
						
	'[INICIO] Função para validar Domínio =============================================================================================	
	Public Function funcValDominio(ByVal valor As String) As String
		Dim resultado As String = ""
		dim retorno 
		
		Try
			Dim rEx As Regex
			Dim ResultadoHum, ResultadoDois, ResultadoTres

			'rEx.IgnoreCase = True         ' Sensitivo ou não
			'rEx.Global = True             ' 
			
			' Caracteres Excluidos
			ResultadoHum = rEx.IsMatch(valor,"^([a-z0-9]([-a-z0-9]*[a-z0-9])?\.)+((a[cdefgilmnoqrstuwxz]|aero|arpa)|(b[abdefghijmnorstvwyz]|biz)|(c[acdfghiklmnorsuvxyz]|cat|com|coop)|d[ejkmoz]|(e[ceghrstu]|edu)|f[ijkmor]|(g[abdefghilmnpqrstuwy]|gov)|h[kmnrtu]|(i[delmnoqrst]|info|int)|(j[emop]|jobs)|k[eghimnprwyz]|l[abcikrstuvy]|(m[acdghklmnopqrstuvwxyz]|mil|mobi|museum)|(n[acefgilopruz]|name|net)|(om|org)|(p[aefghklmnrstwy]|pro)|qa|r[eouw]|s[abcdeghijklmnortvyz]|(t[cdfghjklmnoprtvwz]|travel)|u[agkmsyz]|v[aceginu]|w[fs]|y[etu]|z[amw])$")
						
			If ResultadoHum Then
				resultado = "ok"
			Else
				resultado = "erro"
			End If
		Catch ex As Exception
			'resultado = ex.ToString()
		End Try
		
		return resultado
	End Function	
	'[FIM] Função para validar Domínio ================================================================================================
			
	'[INICIO] funcValDominio > HELP ===================================================================================================
	public function funcValDominio(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then			
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Função para Validar se o texto digitado corresponde a um Domínio. Caso o Domínio for válido retorna 'ok'"
			helpParametros 		 = "(ByVal valor As String)"
			helpFuncionalidades  = "valor: valor do texto ditigado"
			helpExemplo 		 = "funcValDominio(request('cmpDominio'))"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] funcValDominio > HELP =====================================================================================================							
	
	'[INICIO] Função para validar E-mail =============================================================================================	
	Public Function funcValEmail(ByVal email As String) As String
		Dim resultado As String = ""
		
		Try
			Dim rEx As Regex
			Dim ResultadoHum, ResultadoDois, ResultadoTres

			'rEx.IgnoreCase = True         ' Sensitivo ou não
			'rEx.Global = True             ' 
			
			' Caracteres Excluidos
			ResultadoHum = rEx.IsMatch(email,"[^@\-\.\w]|^[_@\.\-]|[\._\-]{2}|[@\.]{2}|(@)[^@]*\1")
			
			' Caracteres validos
			ResultadoDois = rEx.IsMatch(email,"@[\w\-]+\.")
			
			' Caracteres de fim
			ResultadoTres = rEx.IsMatch(email,"\.[a-zA-Z]{2,3}$")
			
			If (Not (ResultadoHum)) And (ResultadoDois) And (ResultadoTres) Then
				resultado = "ok"
			Else
				resultado = "erro"
			End If
		Catch ex As Exception
			'resultado = ex.ToString()
		End Try
		
		return resultado
	End Function	
	'[FIM] Função para validar E-mail ================================================================================================
			
	'[INICIO] funcValEmail > HELP ===================================================================================================
	public function funcValEmail(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then			
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Função para Validar se o texto digitado corresponde a um Email. Caso o Email for válido retorna 'ok'"
			helpParametros 		 = "(ByVal email As String)"
			helpFuncionalidades  = "email: valor do texto ditigado"
			helpExemplo 		 = "funcValEmail(request('cmpEmail'))"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] funcValEmail > HELP =====================================================================================================		
			
	'[INICIO] Função para validar Texto =============================================================================================				
	Public Function funcValTxt(ByVal valor As String) As String
		Dim resultado As String = ""
		
		Try
			If trim(valor) = "" Then
				resultado = "erro"
			Else
				resultado = "ok"
			End If
		Catch ex As Exception
			'resultado = ex.ToString()
		End Try
		
		return resultado
	End Function
	'[FIM] Função para validar Texto =================================================================================================					
			
	'[INICIO] funcValTxt > HELP ====================================================================================================
	public function funcValTxt(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then			
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Função para validar se o texto esta vazio. Retorna 'erro' quando vazio, ou 'ok' se tiver valor"
			helpParametros 		 = "(ByVal valor As String)"
			helpFuncionalidades  = "valor: valor do texto que será validado."
			helpExemplo 		 = "funcValTxt(request('cmpNome'))"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] funcValTxt > HELP ========================================================================================================				
	
'	[INICIO] Função para tratar URL's do YouTube retornando um objeto com o vídeo =====================
	Public Function funcConvYouTube(ByVal url As String) As String
		Dim resultado As String = ""
		
		Try
			
			Dim chave As String = ""
			Dim partes() As String = split(url, "/")
			Dim ultimoBloco As String = partes(ubound(partes))

			if inStr(1, ultimoBloco,"?") > 0 then
				Dim partes2() As String 
				partes2 = split(ultimoBloco,"?")
				Dim partes3() As string
				Dim partes4() As String
				partes3 = split(partes2(ubound(partes2)),"&")
				Dim x As Integer
				For x = 0 to ubound(partes3)
					if left(partes3(x),2) = "v=" then
						partes4 = split(partes3(x),"=")
						chave = partes4(ubound(partes4))
					end if
				Next
			else
				chave = ultimoBloco
			end if
			
			'Modo embed 3
			resultado = "<object width='320' height='240'><param name='movie' value='http://www.youtube.com/v/"& chave &"?version=3&amp;hl=pt_BR'></param><param name='wmode' value='transparent'></param><param name='allowFullScreen' value='true'></param><param name='allowscriptaccess' value='always'></param><embed src='http://www.youtube.com/v/"& chave &"?version=3&amp;hl=pt_BR' type='application/x-shockwave-flash' width='320' height='240' wmode='transparent' allowscriptaccess='always' allowfullscreen='true'></embed></object>"
			
		Catch ex As Exception
			resultado = ex.ToString()
		End Try
		
		return resultado
	End Function
'	[FIM] Função para tratar URL's do YouTube retornando um objeto com o vídeo ========================	

	'[INICIO] funcConvYouTube > HELP ==================================================================================================
	public function funcConvYouTube(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Método que converte trata o conteudo de um post de acordo com o seu tipo"
			helpParametros 		 = "(ByVal url As String)"
			helpFuncionalidades  = "url: endereço do youtube"
			helpExemplo 		 = "funcConvYouTube('http://www.youtube.com.br/G8D8H1R3SD1H6R')"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] funcConvYouTube > HELP =====================================================================================================				
		
    '[INICIO] Função para converter a data no SQL server ==============================================================================
    Public Function funcData(ByVal s As DateTime)
        Dim da As DateTime = s
        Dim mes As Integer = da.Month()
        Dim dia As Integer = da.Day()
        Dim ano As Integer = da.Year()

        Dim horas As Integer = da.Hour()
        Dim minutos As Integer = da.Minute()
        Dim segundos As Integer = da.Second()
        Dim dataFinal As String

        If IsDate(s) Then
            dataFinal = Convert.ToString(ano) + "-" + Convert.ToString(mes) + "-" + Convert.ToString(dia) + " " + Convert.ToString(horas) + ":" + Convert.ToString(minutos) + ":" + Convert.ToString(segundos)
        Else
            dataFinal = Convert.ToString(dia) + "/" + Convert.ToString(mes) + "/" + Convert.ToString(ano) + " " + Convert.ToString(horas) + ":" + Convert.ToString(minutos) + ":" + Convert.ToString(segundos)
        End If

        Return dataFinal
		
    End Function
	'[FIM] Função para converter a data no SQL server =================================================================================
	
	'[INICIO] funcData > HELP ========================================================================================================
	public function funcData(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Função para formatar para o valor de um campo DateTime no padrão do SQL Server. Retorno em String."
			helpParametros 		 = "(ByVal s As DateTime)"
			helpFuncionalidades  = "s: valor do Campo DateTime a ser formatado"
			helpExemplo 		 = "funcData('02/05/2012 15:52:32')"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] funcData > HELP ===========================================================================================================
	
    '[INICIO] Função para converter a data no SQL server ==============================================================================
    Public Function funcDataFull_Bra(ByVal s As DateTime, ByVal Tipo as string)
        Dim da As DateTime = s
        Dim mes As Integer = da.Month()
        Dim dia As Integer = da.Day()
        Dim ano As Integer = da.Year()

        Dim horas As Integer = da.Hour()
        Dim minutos As Integer = da.Minute()
        Dim segundos As Integer = da.Second()
        Dim dataFinal As String
			
        If Tipo = "1" Then 'data SQL
            dataFinal = Convert.ToString(ano) + "-" + Convert.ToString(mes) + "-" + Convert.ToString(dia) + " " + Convert.ToString(horas) + ":" + Convert.ToString(minutos) + ":" + Convert.ToString(segundos)
        ElseIf Tipo = "2" then 'data completa Brasileiro
            dataFinal = Convert.ToString(dia) + "/" + Convert.ToString(mes) + "/" + Convert.ToString(ano) + " " + Convert.ToString(horas) + ":" + Convert.ToString(minutos) + ":" + Convert.ToString(segundos)
        ElseIf Tipo = "3" then 'Hora:Minuto:Segundo
            dataFinal  = IIF(Len(Convert.ToString(horas)) <= 1, "0"+Convert.ToString(horas), Convert.ToString(horas)) + ":"
			dataFinal += IIF(Len(Convert.ToString(minutos)) <= 1, "0"+Convert.ToString(minutos), Convert.ToString(minutos)) + ":"
			dataFinal += IIF(Len(Convert.ToString(segundos)) <= 1, "0"+Convert.ToString(segundos), Convert.ToString(segundos))
        ElseIf Tipo = "4" then 'Hora:Minuto
            dataFinal  = IIF(Len(Convert.ToString(horas)) <= 1, "0"+Convert.ToString(horas), Convert.ToString(horas)) + ":"
			dataFinal += IIF(Len(Convert.ToString(minutos)) <= 1, "0"+Convert.ToString(minutos), Convert.ToString(minutos))
        End If

        Return dataFinal
		
    End Function
	'[FIM] Função para converter a data no SQL server =================================================================================
	
	'[INICIO] funcData > HELP ========================================================================================================
	public function funcDataFull_Bra(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Função para formatar para o valor de um campo DateTime com horas no padrão brasileiro. Retorno em String."
			helpParametros 		 = "(ByVal s As DateTime)"
			helpFuncionalidades  = "s: valor do Campo DateTime a ser formatado"
			helpFuncionalidades  += "Tipo: Tipo de retorno. Valor 1 retorna o campo no formato Americano (compativel com Datas no SQL Server) com horas/min/sec"			
			helpFuncionalidades  += "<br/ >Valor 2 retorna o campo no formato Brasileiro"						
			helpFuncionalidades  += "<br/ >Valor 3 retorna o campo formatado apenas com o Horário Brasileiro (Hora:Minuto:Segundo)"									
			helpFuncionalidades  += "<br/ >Valor 4 retorna o campo formatado apenas com o Horário Brasileiro (Hora:Minuto)"												
			helpExemplo 		 = "funcDataFull_Bra('2005-08-17 09:50:32.000', '3')"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] funcData > HELP ===========================================================================================================	

    '[INICIO] Função para formatar a data sem a hora para SQL Server ==================================================================
    Public Function funcDataSmallAmericano(ByVal s As DateTime)
        Dim da As DateTime = s
        Dim mes As Integer = da.Month()
        Dim dia As Integer = da.Day()
        Dim ano As Integer = da.Year()

        If (dia < 10) Then dia = "0" + dia
        If (mes < 10) Then mes = "0" + mes

        'Dim dataFinal As String = Convert.ToString(ano) + "-" + Convert.ToString(mes) + "-" + Convert.ToString(dia)

        Dim dataFinal = ano & "-" & mes & "-" & dia

        Return dataFinal
		
    End Function
	'[FIM] Função para formatar a data sem a hora para SQL Server =====================================================================
	
	'[INICIO] funcDataSmallAmericano > HELP ===========================================================================================
	public function funcDataSmallAmericano(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Função para formatar para o valor de um campo DateTime no padrão Americano. Retorno em date."
			helpParametros 		 = "(ByVal s As DateTime)"
			helpFuncionalidades  = "s: valor do Campo DateTime a ser formatado"
			helpExemplo 		 = "funcDataSmallAmericano('02/05/2012 15:52:32')"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] funcDataSmallAmericano > HELP ==============================================================================================	

    '[INCIO]Função para imprimir na tela a data formatada (padrão brasileiro) ===================================
    Public Function funcDataSmallBrasil(ByVal s As DateTime)

        Dim dataFinal As String

        If (Not IsNothing(s)) Then

            If IsDate(s) Then
                Dim da As DateTime = s
                Dim mes As Integer = da.Month()
                Dim dia As Integer = da.Day()
                Dim ano As Integer = da.Year()


                If (dia < 10) Then dia = "0" + dia
                If (mes < 10) Then mes = "0" + mes

                dataFinal = dia & "/" & mes & "/" & ano

                'dataFinal = Convert.ToString(dia) + "/" + Convert.ToString(mes) + "/" + Convert.ToString(ano)
            Else

                dataFinal = CDate("00/00/0000")

            End If

        End If

        Return dataFinal

    End Function
	'[FIM]Função para imprimir na tela a data formatada (padrão brasileiro) ===================================	
	
	'[INICIO] funcDataSmallBrasil > HELP ===========================================================================================
	public function funcDataSmallBrasil(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Função para formatar para o valor de um campo DateTime no padrão Brasileito. Retorno em date."
			helpParametros 		 = "(ByVal s As DateTime)"
			helpFuncionalidades  = "s: valor do Campo DateTime a ser formatado"
			helpExemplo 		 = "funcDataSmallBrasil('2012/05/02 15:52:32')"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] funcDataSmallBrasil > HELP ==============================================================================================	
	
    '[INCIO]Função para mascarar CPF e CNPJ ===================================
	Public Function func_maskT(ByVal valor As String, ByVal tipo As String) As String
		Dim r As String = ""
		
		'//99.999.999/9999-99 CNPJ
		If tipo = "PJ" then
		
			if valor <> "" then
				
				if(len(valor) = 14)then
				valor = replace(replace(replace(valor, ".", ""), "-", ""), "/", "")
				
				r = left(valor,2) &"."& right(left(valor,5),3) &"."& right(left(valor,8),3) &"/"& right(left(valor,12),4) &"-"& right(valor,2)
				else
					r = valor
				end if
			end if
				
		End if
		
		'//xxx.xxx.xxx-33 CPF
		If tipo = "PF" then
			
			if valor <> "" then
			
				if(len(valor) = 11)then
				
					valor = replace(replace(replace(valor, ".", ""), "-", ""), "/", "")
				
					r = left(valor,3) &"."& right(left(valor,6),3) &"."& right(left(valor,9),3) &"-"& right(valor,2)
					
				else
				
					r = valor
					
				end if
				
			end if
				
		End If
		

		return r
		
	End Function
	'[FIM]Função para mascarar CPF e CNPJ ======================================
	
	'[INICIO] funcDataSmallBrasil > HELP ===========================================================================================
	public function func_maskT(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Função para mascarar uma string com numeros no formato de CPF ou CNPJ"
			helpParametros 		 = "(ByVal valor As String, ByVal tipo As String)"
			helpFuncionalidades  = "valor: valor do texto que será formatado (string, contendo somente numeros)"
			helpFuncionalidades += "tipo: tipo de pessoa. Aceita 2 valores: PJ / PF"			
			helpExemplo 		 = "func_maskT('79921867458', 'PF')"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] funcDataSmallBrasil > HELP ==============================================================================================		
	
    '[INICIO] funcao para tirar os acentos ===========================================================================
    public Function func_tiraAcento(ByVal str)

        Dim retorno

        If (str <> "") Then

            'possibilidades
            'ãÃáÁàÀâÂäÄ / éÉèÈêÊëË / íÍìÌïÏ / õÕóÓòÒôÔöÖ / úÚùÙûÛüÜ / çÇ
            'a A
            retorno = Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(Replace(str, "ã", "a"), "Ã", "A"), "á", "a"), "Á", "A"), "à", "a"), "À", "A"), "â", "a"), "Â", "A"), "ä", "a"), "Ä", "a"), "é", "e"), "É", "E"), "è", "e"), "È", "E"), "ê", "e"), "Ê", "E"), "ë", "e"), "Ë", "E"), "í", "i"), "Í", "I"), "ì", "i"), "Ì", "I"), "î", "i"), "Î", "I"), "ï", "i"), "Ï", "I"), "õ", "otilde;"), "Õ", "Otilde;"), "ó", "o"), "Ó", "O"), "ò", "o"), "Ò", "O"), "ô", "o"), "Ô", "O"), "ö", "o"), "Ö", "O"), "ú", "u"), "Ú", "U"), "ù", "u"), "Ù", "U"), "û", "u"), "Û", "U"), "ü", "u"), "Ü", "U"), "ç", "c"), "Ç", "C"), "º", "º"), "ª", "ª")


        End If

        Return retorno

    End Function
    '[FIM] funcao para tirar os acentos ==============================================================================	
	
	'[INICIO] func_tiraAcento > HELP ===========================================================================================
	public function func_tiraAcento(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "funcao para tirar os acentos"
			helpParametros 		 = "(ByVal str)"
			helpFuncionalidades  = "str: valor ou texto para ser retirado os caractéres"
			helpExemplo 		 = "func_tiraAcento('Utilização de app_code mantém bons hábitos de programação')"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] func_tiraAcento > HELP ==============================================================================================		
	
	
	'[INICIO] Função para delimitar qtde de caractéres em uma string, sem que corte no meio de uma palavra/fraze ====
	public function func_LimitaTexto(byval strTexto as string, byval strQtde as string) as string
	
		dim retorno as string = ""

		if strTexto <> "" and strQtde <> "" then
			
			dim strCorte as string = Mid(strTexto, 1, cInt(strQtde))
			dim strExCorte as string = "0"
			
			'//Se texto exceder o tamanho...
			if len(strTexto) > cInt(strQtde) then
				'//Se ultimo caractér do texto cortar no meio de uma palavra/fraze...
				if Mid(strCorte, len(strCorte), 1) <> " " and Mid(strCorte, len(strCorte), 1) <> "." then
					
					dim i as integer
					'//Loop para percorrer o texto...
					for i = 1 to len(strCorte)
						'//Se posição for igual espaço (entre palavras)...
						if Mid(strCorte, i, 1) = " " then
							'//Retorna o texto todo antes do ultimo espaço dentro do limite de caractéres
							strExCorte = "1"
							'//Adiciona reticências
							retorno = Mid(strCorte, 1, i) & "..."' + " |" + Mid(strCorte, len(strCorte)-2, 2)
						
						end if
					
					next
				
				end if
				
				'//Caso o texto não tenha nenhum espaço entre palavras, retorna o mesmo dentro do limite de caractéres
				if strExCorte = "0" then retorno = strCorte	
			
			'//Caso o texto não exceder o tamanho, é exibido na íntegra	
			else
			
				retorno = strTexto	
				
			end if	
		
		end if
		
		return retorno
	
	end function	
	'[FIM] Função para delimitar qtde de caractéres em uma string, sem que corte no meio de uma palavra/fraze =======
	
	'[INICIO] func_LimitaTexto > HELP ===========================================================================================
	public function func_LimitaTexto(byval strTxt as string, Byval strTxt2 as Boolean) as string
		
		dim helpRetorno as string 				'Retorno do Texto Descritivo					
		
		If strTxt = "help" and strTxt2 = true Then
		
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Função para delimitar qtde de caractéres em uma string, sem que corte no meio de uma palavra/fraze"
			helpParametros 		 = "(byval strTexto as string, byval strQtde as string)"
			helpFuncionalidades  = "strTexto: texto a ser delimitado"
			helpFuncionalidades += "strQtde: qtde de caractéres desejados. Lembrando que a função irá acrescentar reticências no final."			
			helpFuncionalidades += "<br/>Caso deseje um número exato de caractéres, informar a qtde menos 3 (ex: para 150 carac., informe 147)"						
			helpExemplo 		 = "func_LimitaTexto(Dr('nome').ToString(), '147')"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
		end if	
		
		return helpRetorno
		
	end function
	'[FIM] func_LimitaTexto > HELP ==============================================================================================
	
	
	'[INICIO] funcao para converter moeda ========================================================
	public function fMoeda(byval moeda as string, byval tipo as string)
	
		dim retorno
		dim a, b, c
		
		if(moeda <> "")then
		
			moeda = formatNumber(moeda)
			
			if(tipo = "US")then
				
				a = replace(moeda, ",", ";")
				a = replace(a, ".", ",")
				a = replace(a, ";", ".")
				
				retorno = a
				
			end if
			
			if(tipo = "BR")then
				
				retorno = moeda
			
			end if
		
		end if
		
		return retorno
	
	end function
	'[FIM] funcao para converter moeda ========================================================		
	
	'=================================================================================
	'[INICIO] funcao para formatar dados números como CPF, CNPJ, TELEFONE, CEP, PRECO
	Public function fFormat(byval campo, byval formato) 
		
		dim retorno = campo
		
		if(campo <> "")then
			
			campo = replace(replace(replace(replace(campo, ".",""), "-",""), "/",""), ",","")
			
			if(isnumeric(campo))then
				retorno = convert.Toint64(campo).toString(formato)
			end if
		
		end if
		
		return retorno
	
	End Function
	'['FIM] funcao para formatar dados números como CPF, CNPJ, TELEFONE, CEP, PRECO
	'===============================================================================
	
				
 End Class

End Namespace