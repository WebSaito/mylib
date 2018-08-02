Imports System
Imports SAPI
Imports S

Public Class pc
	Inherits System.Web.UI.Page
	
	Public oAPI as new API1()
	Public oDB as new DB()
	Public oSec as new Seguranca()
	
	Sub Page_Load(byval src as object, byval e as eventArgs)
		
		Dim agora as Datetime = Datetime.Now()
		Dim acao as String = lcase(request("acao"))
		Dim r as String = "" 'resultado
		
		'Verificando método: GET, POST, PUT, DELETE
		'-
		'---
		'GET
		if( oAPI.metodo = "GET" )then
			
			'---
			'.-=[ OAUTH ]=-.
			if(acao = "oauth")then
				
				'Verificando cliente / segredo
				oAPI.pegaClienteSegredo()
				
				'Checkando se existe cliente e segredo
				Dim aux as String = ""
				Dim validade as String = ""
				Dim strSql as String = " select * from lh_auth where cliente='"& oAPI.cliente &"' and segredo='"& oAPI.segredo &"' and email='"& trim(request("email")) &"' and excluido=0 and disponivel=1 "
				Dim dadosUsr as new Hashtable() 
				dadosUsr = oDB.consultaLinha(strSql)
				if( dadosUsr.containsKey("id") )then
				'Encontrado
					'Gerar token com DATA(ss-MM-yyyy)+ID usando e-mail como chave
					aux = agora.toString("ss-MM-yyyy") &"|"& dadosUsr("id")
					oAPI.token = oSec.CID6( aux, dadosUsr("email") )
					
					'Definindo a validade do token
					Dim dtAux as datetime = agora.AddHours(2)
					validade = dtAux.toString("yyyy-MM-dd HH:mm:ss")
					
					'Mapa de retorno sucesso
					'{
					'	'status':'sucesso',
					'	'token':'xxxxxxxxxxxxxxxxxxxxxxxxxxxx',
					'	'validade':'yyyy-MM-dd HH:mm:ss'
					'}
					
					'Gravando token 
					strSql = "insert into lh_auth_tokens(idCliente, token, validade, ativo) values("& dadosUsr("id") &", '"& oAPI.token &"', '"& validade &"', 1)"
					oDB.executa(strSql)
					
					r = ""
					r &= "{"
					r &= "	'status':'sucesso',"
					r &= "	'token':'"& oAPI.token &"',"
					r &= "	'validade':'"& validade &"'"
					r &= "}"
					
				else
				'Não encontrado
					'Mapa de retorno erro
					'{
					'	'status':'erro'
					'	'mensagem':'Está é uma mensagem de erro'
					'	'detalhes':''
					'}
					
					r = ""
					r &= "{"
					r &= "	'status':'erro',"
					r &= "	'mensagem':'Cliente não encontrado ou indisponível',"
					r &= "	'detalhes':''"
					r &= "}"
					
				end if
				
			end if
			'.-=[ FIM OAUTH ]=-.
			'---
			
			
			'.-=[ CONSULTAS USANDO O TOKEN ]=-.
			
			'.-=[ FIM CONSULTAS USANDO O TOKEN ]=-.
			
			
		end if
		'FIM GET
		'---
		'-
		
		'-
		'---
		'POST
		if( oAPI.metodo = "POST" )then
			
		end if
		'FIM POST
		'---
		'-
		
		'-
		'---
		'PUT
		if( oAPI.metodo = "PUT" )then
			
		end if
		'FIM PUT
		'---
		'-
		
		'-
		'---
		'DELETE
		if( oAPI.metodo = "DELETE" )then
			
		end if
		'FIM DELETE
		'---
		'-
		
		
		
		
		response.write(oAPI.metodo)
		response.write(oAPI.cliente)
		response.write(oAPI.segredo)
		
		response.write("<hr/>")
		
	End Sub
	
End Class
