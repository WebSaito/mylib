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
		
		Dim temTokenValido as boolean = false
		Dim idUsr as String = ""
		Dim dados as new Hashtable()
		Dim dadosToken as new Hashtable()
		
		'---------
		'Verificando Token utilizado em todos os métodos
		if( request("token") <> "" )then
		'Consultando DB para ver se token é valido e confere com cliente segredo
			'Verificando cliente / segredo
			if(oAPI.pegaClienteSegredo())then
				
				'Consultando Cliente/Segredo para saber se é usuario/app valido
				dados = oDB.consultaLinha("select * from lh_auth where cliente='"& oAPI.cliente &"' and segredo='"& oAPI.segredo &"' and excluido=0 and disponivel=1 ")
				
				if( dados.containsKey("id") )then
					'Usr encontrado agora precisamos verificar o token
					if( oDB.consulta("id", "select id from lh_auth_tokens where idCliente="& dados("id") &" and token='"& request("token") &"' and ativo=1 and validade > '"& agora.toString("yyyy-MM-dd HH:mm:ss") &"'  ") <> "" )then
						temTokenValido = true
						oAPI.token = request("token")
					end if
				end if
			end if
			
		end if
		'---------
		
		
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
					r &= "	'mensagem':'Cliente nao encontrado ou indisponivel',"
					r &= "	'detalhes':''"
					r &= "}"
					
				end if
				
			end if
			'.-=[ FIM OAUTH ]=-.
			'---
			
			
			'.-=[ CONSULTAS USANDO O TOKEN ]=-.
			if( temTokenValido )then
				
				
				
			end if
			'.-=[ FIM CONSULTAS USANDO O TOKEN ]=-.
			
		end if
		'FIM GET
		'---
		'-
		
		'-
		'---
		'POST
		if( oAPI.metodo = "POST" )then
			
			if( temTokenValido )then
				
				'-=[--[INICIO POST]
				'	Inserir aqui metodos que farão insert ou qualquer outro tipo de execução que gere novos registros no DB 
				
				'Como a maioria dos servidores compartilhados não possuem permissão para os demais métodos PUT e DELETE, então deve ser colocado aqui em POST também as 
				'acóes de UPDATE e DELETE de registros
				
				
				
				
				
				'==========
				'[PRODUTOS]
				
				'-------
				'> Insere novo
				if( request("acao") = "novo" )then
					
					'Requests
					
					
					
					'Validações
					
					
					
					
					'Gravação
					
					
					
					
				end if
				'> Fim Insere novo
				'-------
				
				
				'-------
				'> Atualiza
				if( request("acao") = "atualizar" )then
					
					'Requests
					
					
					
					
					'Validações
					
					
					
					
					'Gravação
					
					
					
					
				end if
				'> Fim Atualiza
				'-------
				
				
				'-------
				'> Deleta
				if( request("acao") = "deletar" )then
					
					
					
				end if
				'> Fim Deleta
				'-------
				
				
				'[FIM PRODUTOS]
				'==========
				
				
				
				
				
				'[FIM POST]--]=-
				
			else
				
				r = ""
				r &= "{"
				r &= "	'status':'erro',"
				r &= "	'mensagem':'Token inválido',"
				r &= "	'detalhes':''"
				r &= "}"
				
			end if
			
		end if
		'FIM POST
		'---
		'-
		
		
		
		'----------------------------------------------
		'Métodos PUT e DELETE abaixo devem ser verificados se estão habilitados no servidor web
		'-
		'---
		'PUT
		if( oAPI.metodo = "PUT" )then
			
			if( temTokenValido )then
				
				'-=[--[INICIO PUT]
				'	Inserir aqui metodos que farão update ou qualquer outro tipo de execução que atualize registros no DB 
				
				
				
				
				
				'[FIM PUT]--]=-
				
				
			else
				
				r = ""
				r &= "{"
				r &= "	'status':'erro',"
				r &= "	'mensagem':'Token inválido',"
				r &= "	'detalhes':''"
				r &= "}"
				
			end if
			
		end if
		'FIM PUT
		'---
		'-
		
		'-
		'---
		'DELETE
		if( oAPI.metodo = "DELETE" )then
		
			if( temTokenValido )then
				
				'-=[--[INICIO DELETE]
				'	Inserir aqui metodos que farão delete ou qualquer outro tipo de execução que exclua registros no DB 
				
				
				
				
				
				'[FIM DELETE]--]=-
				
			else
				
				r = ""
				r &= "{"
				r &= "	'status':'erro',"
				r &= "	'mensagem':'Token inválido',"
				r &= "	'detalhes':''"
				r &= "}"
				
			end if
			
		end if
		'FIM DELETE
		'---
		'-
		
		response.write(r)
		
	End Sub
	
End Class
