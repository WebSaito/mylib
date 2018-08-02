Imports System
Imports System.Data.SqlClient
Imports System.Data
Imports MySql.Data.MySqlClient

Namespace SP
	
	Public Class Dados
		
		Implements IDisposable
		
		Private stabela as String = ""
		Private scamposTabela as String = ""
		
		Private sAuxFiltro as String = ""
		Private sAuxOrdem as String = ""
		
		'Propriedades
		Public erros As String = ""
		Public errosUsr As String = ""
		
		Public JSONpaginas as String = ""
		Public JSONregistros as String = ""
		Public JSONregistrosFinal as String = ""
		
		Public Property tabela() as String
			Get
				return stabela
			End Get
			Set(ByVal v as String)
				stabela = v
			End Set
		End Property
		
		Public Property camposTabela() as String
			Get
				return scamposTabela
			End Get
			Set(ByVal v as String)
				scamposTabela = v
			End Set
		End Property
		
		Public Property auxFiltro() as String
			Get
				return sAuxFiltro
			End Get
			Set(ByVal v as String)
				sAuxFiltro = v
			End Set
		End Property
		
		
		Public Property auxOrdem() as String
			Get
				return sAuxOrdem
			End Get
			Set(ByVal v as String)
				sAuxOrdem = v
			End Set
		End Property
		
		
		' - Objetos
		Public filtro As New iFiltro()
		Public pagina As New iPagina()
		Public ordem As New iOrdem() 
		
		Public oDB as new DB()
		
		Public mapa as List(Of iMapaTabela) = new List(Of iMapaTabela)
		Public registros as List(Of iRegistro) = new List(Of iRegistro)
		
		'Construtor vazio
		Public Sub New()
			
		End Sub
		
		'Dispose
		Public Sub Dispose() Implements System.IDisposable.Dispose
			
			tabela = ""
			camposTabela = ""
			auxFiltro = ""
			auxOrdem = ""
			
			filtro.dispose()
			pagina.dispose()
			ordem.dispose()
			
			mapa.clear()
			registros.clear()
			
		End Sub
		
		'--------------------------------------------------------------------------------
		'Método que processa os dados de objetos e retorna os registros
		Public Function processarQueryDados() As String
			Dim r As String = ""
				'Montando string de busca
				
				if oDB.dbType = "MSSQL" then
					r = "with tbl_tmp as(select row_number() over("& retornaOrdem() &") as nlinha , "& camposTabela &" from " & tabela & " "& trataBusca() &"  ) select * from tbl_tmp where nlinha between "& rangeRegistros() & fixOrdem(retornaOrdem())
				end if
				
				if oDB.dbType = "MYSQL" then
					r = "select "& camposTabela &" from " & tabela & " "& trataBusca() &" "& retornaOrdem() &" limit "& rangeRegistros() 
				end if
				
			return r
		End Function
		'--------------------------------------------------------------------------------
		
		'----------------------------------------------------------------------------------
		'Método que retorna os dados e calcula paginas dentro do objeto
		Public Function processarDados() as DataTable
			Dim r As New DataTable()
			Dim strSql as String = ""
			
			'--------------------------------------------------------------
			'MSSQL
			if oDB.dbType = "MSSQL" then
				
				'---------------------------------------------------------------------
				'Update 2017-04-28 - 
				
				'Registros
				strSql = "with tbl_tmp as(select row_number() over("& retornaOrdem() &") as nlinha , "& camposTabela &" from " & tabela & " "& trataBusca() &"  ) select * from tbl_tmp where nlinha between "& rangeRegistros() & fixOrdem(retornaOrdem())
				r = oDB.consulta(strSql)
				
				'Total
				pagina.totalRegistros = oDB.consulta("total", "select count(*) as total from "& tabela &" "& trataBusca())
				
				'Calculando páginas
				calcularPaginas()
				
				
			end if
			'FIM MSSQL
			'--------------------------------------------------------------
			
			'--------------------------------------------------------------
			'MYSQL
			if oDB.dbType = "MYSQL" then
				
				'---------------------------------------------------------------------
				'Update 2017-04-28 -
				
				'Registros
				strSql = "select "& camposTabela &" from " & tabela & " "& trataBusca() &" "& retornaOrdem() &" limit "& rangeRegistros() 
				r = oDB.consulta(strSql)
				
				'Total
				pagina.totalRegistros = oDB.consulta("total", "select count(*) as total from "& tabela &" "& trataBusca())
				
				'Calculando páginas
				calcularPaginas()
				
			end if
			'FIM MYSQL
			'--------------------------------------------------------------
			
			return r
		End Function
		
		'----------------------------------------------------------------------------------
		'Processa a query e devolve o resultado JSON utilizando o nome dos campos no DB
		Public Function processarDadosJSON() as String
			Dim r as String = ""
			Dim strSql as String = ""
			Dim dt as new system.data.datatable()
			Dim indice as Integer = 0
			
			'--------------------------------------------------------------
			'MSSQL
			if oDB.dbType = "MSSQL" then
				
				'Mapa
				strSql = "select top 1 "& camposTabela &" from "& tabela
				dt = oDB.consulta(strSql)
				for each x as System.Data.DataColumn in dt.columns
					mapa.add(new iMapaTabela(x.columnName.toString(),  x.dataType.toString(), x.maxLength.toString()))
				next
				dt.clear()
				
				'Registros
				indice = 0
				strSql = "with tbl_tmp as(select row_number() over("& retornaOrdem() &") as nlinha , "& camposTabela &" from " & tabela & " "& trataBusca() &"  ) select * from tbl_tmp where nlinha between "& rangeRegistros() & fixOrdem(retornaOrdem())
				dt = oDB.consulta(strSql)
				for each x as System.Data.DataRow in dt.rows
					
					registros.add(new iRegistro(x(0)))
					
					for each y as iMapaTabela in mapa
						registros(indice).dados.add(new iCamposValores(y.campo, IIF(IsDBNull(x(y.campo)) = true,"",x(y.campo)) ))
					next
					indice = indice + 1
				next
				dt.clear()
				
				'Total
				pagina.totalRegistros = oDB.consulta("total", "select count(*) as total from "& tabela &" "& trataBusca())
				
				'Calculando páginas
				calcularPaginas()
				
				
				'JSON
				JSONregistros = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(registros)
				
				JSONpaginas = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pagina)
				
				trataJSON()
				
			end if
			'FIM MSSQL
			'--------------------------------------------------------------
			
			'--------------------------------------------------------------
			'MYSQL
			if oDB.dbType = "MYSQL" then
				
				'Mapa
				strSql = "select "& camposTabela &" from "& tabela &" limit 1"
				dt = oDB.consulta(strSql)
				for each x as System.Data.DataColumn in dt.columns
					mapa.add(new iMapaTabela(x.columnName.toString(),  x.dataType.toString(), x.maxLength.toString()))
				next
				dt.clear()
				
				'Registros
				indice = 0
				strSql = "select "& camposTabela &" from " & tabela & " "& trataBusca() &" "& retornaOrdem() &" limit "& rangeRegistros() 
				dt = oDB.consulta(strSql)
				for each x as System.Data.DataRow in dt.rows
					
					registros.add(new iRegistro(x(0)))
					
					for each y as iMapaTabela in mapa
						registros(indice).dados.add(new iCamposValores(y.campo, IIF(IsDBNull(x(y.campo)) = true,"",x(y.campo)) ))
					next
					indice = indice + 1
				next
				dt.clear()
				
				'Total
				pagina.totalRegistros = oDB.consulta("total", "select count(*) as total from "& tabela &" "& trataBusca())
				
				'Calculando páginas
				calcularPaginas()
				
				'JSON
				JSONregistros = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(registros)
				
				JSONpaginas = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pagina)
				
				trataJSON()
				
			end if
			'FIM MYSQL
			'--------------------------------------------------------------
				
			return r
		End Function
		'Fim processarDadosJSON
		'----------------------------------------------------------------------------------
		
		
		'****************************************
		'Métodos auxiliares
		
		
		Public Sub trataJSON()
			'JSONregistros
			JSONregistrosFinal = JSONregistros
			JSONregistrosFinal = replace(replace(JSONregistrosFinal,chr(34) &"campo"& chr(34) &":", ""), ","& chr(34) &"valor"& chr(34), "" )
			JSONregistrosFinal = replace(JSONregistrosFinal, "}],"& chr(34) &"FST"& chr(34) &":", ", "& chr(34) &"FST"& chr(34) &":")
			JSONregistrosFinal = replace(JSONregistrosFinal, chr(34) &"},{"& chr(34), chr(34) &","& chr(34) )
			JSONregistrosFinal = replace(JSONregistrosFinal, ","& chr(34) &"dados"& chr(34) &":[", "}," )
			JSONregistrosFinal = replace(JSONregistrosFinal, "{"& chr(34) &"dados"& chr(34) &":[", ""  )
			
		End Sub
		
		
		
		
		
		'---------------------------------------------------------
		'Retorna ordem
		Public Function retornaOrdem() as String
			Dim r as String = ""
			
			Dim x As iDadosOrdem
			
			if auxOrdem = "" then
				
				For each x in ordem.dados
					If r = "" then
						r &= " order by "& x.campo &" "& x.orientacao &" " 
					Else
						r &= ", "& x.campo &" "& x.orientacao &" "
					End If
				Next
				
				auxOrdem = r
				
			Else
				r = auxOrdem 
			End If
			
			return r
		End Function
		
		
		'FIX
		Public Function fixOrdem(byval v as string) as String
			Dim r as String = ""
			
			r = replace(replace(replace(replace(replace(replace(v,"a.", " "), "b.", " "), "c.", " "), "d."," "), "e.", " "), "f.", " ")
			
			return r
		End Function
		
		
		'-----------------------------------------
		'Range dos Registros(limite)
		Public Function rangeRegistros() as String
			Dim r as String = ""
				
				Dim inicio As Double = 0
				Dim fator As Integer = 1
				Dim nreg As Integer = pagina.limite
				Dim soma As Double = 0
				
				If isNumeric(pagina.paginaAtual) then
					If cInt(pagina.paginaAtual) > 0 then
						fator = cInt(pagina.paginaAtual)
						soma = (fator * nreg)
					End If
				End If
				
				'-------------------------------------------------
				'MSSQL
				If oDB.dbType="MSSQL" then
					If cInt(pagina.paginaAtual) > 0 then
						r = " "& ((inicio +1) + soma) &" and "& (inicio + soma + nreg ) &" "
					else
						r = " "& (inicio + soma) &" and "& (inicio + soma + nreg ) &" "
					end if
				End If
				'FIM MSSQL
				'-------------------------------------------------
				
				'-------------------------------------------------
				'MYSQL
				If oDB.dbType="MYSQL" then
					'If cInt(pagina.paginaAtual) > 0 then
					'	r = " "& ((inicio +1) + soma) &","& (inicio + soma + nreg ) &" "
					'else
					'	r = " "& (inicio + soma) &","& (inicio + soma + nreg ) &" "
					'end if
					If cInt(pagina.paginaAtual) > 0 then
						r = " "& ((inicio) + soma) &","& (nreg) &" "
					else
						r = " "& (inicio + soma) &","& (nreg) &" "
					end if
					
				End If
				'FIM MYSQL
				'-------------------------------------------------
			return r
		End Function
		
		'-----------------------------------------
		'Trata a busca
		Public Function trataBusca() as String
			Dim r as String = ""
			
			'Verificando se objeto possui query definida manualmente
			If filtro.queryPronta <> "" then
				r = " where "&  filtro.queryPronta
			Else
				
				if auxFiltro = "" then
				
					Dim blocoOr as String = ""
					Dim blocoAnd as String = ""
					
					'Primeiro bloco OR
					Dim x as iDadosFiltro
					For each x in filtro.dadosOr
						
						If blocoOr = "" then
							blocoOr &= " "& x.query
						Else
							blocoOr &= " or "& x.query
						End If
						
					Next
					
					'Agora o bloco And
					Dim y as iDadosFiltro
					For each y in filtro.dadosAnd
						
						If blocoAnd = "" then
							blocoAnd &= " "& y.query
						Else
							blocoAnd &= " and "& y.query
						End If
						
					Next
					
					'Finalizando, primeiro verificar se blocoAnd possui valor
					If blocoAnd <> "" then
						r = " where "& blocoAnd 
						
						if blocoOr <> "" then
							r = r &" and ("& blocoOr &") "
						end if
						
					Else
						
						if blocoOr <> "" then
							r = " where "& blocoOr
						end if
						
					End If
				
				Else
					r = auxFiltro
				End If
				
			End If
			
			if auxFiltro = "" then
				auxFiltro = r
			end if
			
			return r
		End Function
		
		'-----------------------------------------
		'calculaPaginas
		Public Function calcularPaginas() as Boolean
			Dim r as Boolean = false
			Dim x as Integer = 0
			Dim cont as Integer = 0
			
			try
				'-------------------------------------------------------------------------------
				'calcular o número de páginas
				if pagina.totalRegistros > pagina.limite then
					pagina.totalPaginas = Math.Ceiling(pagina.totalRegistros / pagina.limite)
				else
					pagina.totalPaginas = 1
				end if
				
				'--------------------------------------------------------------------------------
				'definindo anterior e proximo baseado em pagina atual
				'Anterior
				if pagina.paginaAtual > 0 then
					pagina.anterior = pagina.paginaAtual - 1
					
					Dim tmpCounter as Integer = 0
					Dim tmpInicio as Integer = (pagina.paginaAtual - 3)
					if tmpInicio < 0 then
						tmpInicio = 0
					end if
					For x = tmpInicio to (pagina.paginaAtual-1)
						if tmpCounter < 3 then
							pagina.linksA.add(new iLinks(x))
						end if
						tmpCounter = tmpCounter + 1
					Next
					
				else
					pagina.anterior = -1
				end if
				
				'Proximo
				if pagina.paginaAtual < (pagina.totalPaginas - 1) then
					pagina.proximo = pagina.paginaAtual + 1
					
					'links proximos
					'If (pagina.totalPaginas - pagina.paginaAtual) <= 3 then
					'	For x = (pagina.paginaAtual + 1) to pagina.totalPaginas
					'		pagina.linksP.add(new iLinks(x))
					'	Next
					'Else
					'	For x = (pagina.paginaAtual + 1) to (pagina.paginaAtual + 3)
					'		pagina.linksP.add(new iLinks(x))
					'	Next
					'End If
					
					Dim tmpCounter as Integer = 0
					Dim tmpInicio as Integer = (pagina.paginaAtual + 1)
					For x = tmpInicio to (pagina.totalPaginas - 1)
						if tmpCounter < 3 then
							pagina.linksP.add(new iLinks(x))
						end if
						tmpCounter = tmpCounter + 1
					Next
					
				else
					pagina.proximo = -1
				end if
				
				'-----------------------------------------------------------
				'Definindo miolo da paginação
				
			catch ex as Exception
				erros &= "<br><br>calcularPaginas: "& ex.toString()
			end try
			
			return r
		End Function
		
		
		
	End Class
	
	
	
	
	
	
	
	
	
	
	
	
	'Dados Filtro
	Public Class iDadosFiltro
		Implements IDisposable
		
		Private squery as String = ""
		
		Public Property query() as String
			Get
				return squery
			End Get
			Set(ByVal v as String)
				squery = v
			End Set
		End Property 
		
		'Construtor
		Public Sub New(ByVal pquery as String)
			query = pquery
		End Sub
		
		Public Sub Dispose() implements System.IDisposable.Dispose
			query = ""
		End Sub
		
	End Class
	
	'-------------------------------------------
	'Filtro
	Public Class iFiltro
		Implements IDisposable
		
		
		Private squeryPronta as String = ""
		
		Public Property queryPronta() as String
			Get
				return squeryPronta
			End Get
			Set(ByVal v as String)
				squeryPronta = v
			End Set
		End Property
		
		Public dadosOr As List(Of iDadosFiltro) = new List(Of iDadosFiltro)
		Public dadosAnd As List(Of iDadosFiltro) = new List(Of iDadosFiltro)
		
		Public Sub Dispose() implements System.IDisposable.Dispose
			queryPronta = ""
			dadosOr.clear()
			dadosAnd.clear()
		End Sub
		
	End Class
	
	
	'------------------------------------------
	'Paginacao
	Public Class iPagina
		Implements IDisposable
		
		Private ilimite as Integer = 0
		Private dtotalRegistros as Double = 0
		Private itotalPaginas as Integer = 0
		Private ipaginaAtual as Integer = 0
		
		'links de paginação
		Private sproximo as String = ""
		Private santerior as String = ""
		
		Public Property limite() as Integer
			Get
				return ilimite
			End Get
			Set(ByVal v as Integer)
				ilimite = v
			End Set
		End Property
		
		Public Property totalRegistros() as Double
			Get
				return dtotalRegistros
			End Get
			Set(Byval v as Double)
				dtotalRegistros = v
			End Set
		End Property
		
		Public Property totalPaginas() as Integer
			Get
				return itotalPaginas
			End Get
			Set(ByVal v as Integer)
				itotalPaginas = v
			End Set
		End Property
		
		Public Property paginaAtual() as Integer
			Get
				return ipaginaAtual
			End Get
			Set(ByVal v as Integer)
				ipaginaAtual = v
			End Set
		End Property
		
		Public Property proximo() as String
			Get
				return sproximo 
			End Get
			Set(ByVal v as String)
				sproximo = v
			End Set
		End Property
		
		Public Property anterior() as String
			Get
				return santerior
			End Get
			Set(ByVal v as String)
				santerior = v
			End Set
		End Property
		
		Public linksA as List(Of iLinks) = new List(Of iLinks)
		Public linksP as List(Of iLinks) = new List(Of iLinks)
		
		Public Sub Dispose() implements System.IDisposable.Dispose
			limite = 0
			totalRegistros = 0
			totalPaginas = 0
			paginaAtual = 0
			proximo = ""
			anterior = ""
			linksA.clear()
			linksP.clear()
		End Sub
		
	End Class
	
	'Links Paginacao
	Public Class iLinks
		Implements IDisposable
		
		Private slink as String = ""
		
		Public Property link() as String
			Get
				return slink
			End Get
			Set(ByVal v as String)
				slink = v
			End Set
		End Property
		
		Public Sub New()
		End Sub
		
		Public Sub New(ByVal plink as String)
			link = plink
		End Sub
		
		Public Sub Dispose() implements System.IDisposable.Dispose
			link = ""
		End Sub
		
	End Class
	
	
	'----------------------------------------------------
	'Dados Ordem
	Public Class iDadosOrdem
		Implements IDisposable
		
		Private scampo as String = ""
		Private sorientacao as String = ""
		
		Public Property campo() as String
			Get
				return scampo
			End Get
			Set(ByVal v as String)
				scampo = v
			End Set
		End Property
		
		Public Property orientacao() as String
			Get
				return sorientacao
			End Get
			Set(ByVal v as String)
				sorientacao = v
			End Set
		End Property
		
		Public Sub New(ByVal pcampo as String, ByVal porientacao as String)
			campo = pcampo
			orientacao = porientacao
		End Sub
		
		Public Sub Dispose() implements System.IDisposable.Dispose
			campo = ""
			orientacao = ""
		End Sub
		
	End Class
	
	'Ordem
	Public Class iOrdem
		Implements IDisposable
		
		Private schave as String = ""
		
		Public Property chave() as String
			Get
				return schave
			End Get
			Set(ByVal v as String)
				schave = v
			End Set
		End Property
		
		Public dados As List(Of iDadosOrdem) = new List(Of iDadosOrdem)
		
		Public Sub Dispose() implements System.IDisposable.Dispose
			chave = ""
			dados.clear()
		End Sub
		
	End Class
	
	'--------------------------------------------------------------------------
	'JSON
	
	Public Class iRegistro
		Implements IDisposable
		
		Private sFirst as String = ""
		
		Public Property FST() as String
			Get
				return sFirst
			End Get
			Set(byval v as string)
				sFirst = v
			End Set
		End Property
		
		Public dados as List(Of iCamposValores) = new List(Of iCamposValores) 
		
		Public Sub New(byval pFST as String)
			FST = pFST
		End Sub
		
		Public Sub dispose() implements System.IDisposable.dispose
			FST = ""
			dados.clear()
		End Sub
		
	End Class
	
	Public Class iCamposValores
		Implements IDisposable
		
		Private sCampo as String = ""
		Private sValor as String = ""
		
		Public Property campo() as String
			Get
				return sCampo
			End Get
			Set(byval v as string)
				sCampo = v
			End Set
		End Property
		
		Public Property valor() as String
			Get
				return sValor
			End Get
			Set(byval v as String)
				sValor = v
			End Set
		End Property
		
		Public Sub New(byval pcampo as string, byval pvalor as string)
			campo = pcampo
			valor = pvalor
		End Sub
		
		Public Sub dispose() implements System.IDisposable.Dispose
			campo = ""
			valor = ""
		End Sub
		
	End Class
	
	Public Class iMapaTabela
		Implements IDisposable
		
		Private sCampo as String = ""
		Private sTipo as String = ""
		Private sMaximo as String = ""
		
		Public Property campo() as String
			Get
				return sCampo
			End Get
			Set(byval v as string)
				sCampo = v
			End Set
		End Property
		
		Public Property tipo() as String
			Get
				return sTipo
			End Get
			Set(byval v as string)
				sTipo = v
			End Set
		End Property
		
		Public Property maximo() as String
			Get
				return sMaximo
			End Get
			Set(byval v as string)
				sMaximo = v
			End Set
		End Property
		
		Public Sub New(byval pcampo as string, byval ptipo as string, byval pmaximo as string)
			
			campo = pcampo
			tipo = ptipo
			maximo = pmaximo
			
		End Sub
		
		Public Sub Dispose() implements System.IDisposable.Dispose
			
			campo = ""
			tipo = ""
			maximo = ""
			
		End Sub
		
	End Class
	
End Namespace
