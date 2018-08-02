Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient
Imports System.Web.Script.Serialization
Imports System.Security.Cryptography

'-----------------------------------------
'Classe DB
Public Class DB
	Inherits iDB
	Implements IDisposable
	
	'Objetos
	Public bancos as List(Of iDB) = new List(Of iDB)
	
	'Propriedades
	Private numeroDBS as String = "1"
	Private auxInt as Integer = 0
	private serros as String = ""
	
	Public Property erros() as String
		get
			return serros
		end get
		set(byval v as string)
			serros = v
		end set
	End Property
	
	Public tabela as List(Of iDBTabela) = new List(Of iDBTabela)
	
	Public tabelaJSON as String = ""
	
	Public FV as new DBFV()
	
	'Construtor
	Public Sub New()
		
		'Pegando quantidade de DBs
		numeroDBS = System.Configuration.ConfigurationManager.AppSettings("strTotalDB")
		
		For auxInt = 1 to cInt(numeroDBS)
			
			bancos.add(new iDB( _
				System.Configuration.ConfigurationManager.AppSettings("strDB"& auxInt), _
				System.Configuration.ConfigurationManager.AppSettings("strDBType"& auxInt), _
				System.Configuration.ConfigurationManager.AppSettings("SqlRead"& auxInt), _
				System.Configuration.ConfigurationManager.AppSettings("sqlWrite"& auxInt) _
			))
			
			if db = "" then
				db = System.Configuration.ConfigurationManager.AppSettings("strDB"& auxInt)
				dbType = System.Configuration.ConfigurationManager.AppSettings("strDBType"& auxInt)
				dbR = System.Configuration.ConfigurationManager.AppSettings("SqlRead"& auxInt)
				dbW = System.Configuration.ConfigurationManager.AppSettings("sqlWrite"& auxInt)
			end if
			
		Next
		
	End Sub
	
	'Limpeza
	Public Sub Dispose() Implements System.IDisposable.Dispose
		bancos.Clear()
		db = ""
		dbR = ""
		dbW = ""
		dbType = ""
		
		tabela.Clear()
		tabelaJSON = ""
	End Sub
	
	
	
	'Métodos
	'***********************************************************
	'Consulta
	Public Function consulta(byval pretorno as string, byval pstrSql as String) as String
		Dim r as String = ""
		Dim strSql as String = pstrSql
		
		'------------------------------------------------------
		'MSSQL
		if dbType = "MSSQL"
		
			Dim cx as SqlConnection = new SqlConnection(dbR)
			Dim web as SqlCommand
			Dim dr as SqlDataReader
			
			cx.open()
			try
				web = new SqlCommand(strSql, cx)
				dr = web.executeReader()
				if dr.read() then
					r = dr(pretorno).toString()
				end if
				dr.close()
			catch ex as exception
				erros &= "<br>consulta("& pretorno &", "& pstrSql &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try
		
		end if
		'FIM MSSQL
		'------------------------------------------------------
		
		
		'------------------------------------------------------
		'MYSQL
		if dbType = "MYSQL" then
			
			Dim cx as MySqlConnection = new MySqlConnection(dbR)
			Dim web as MySqlCommand
			Dim dr as MySqlDataReader
			
			cx.open()
			try
				web = new MySqlCommand(strSql, cx)
				dr = web.executeReader()
				if dr.read() then
					r = dr(pretorno).toString()
				end if
				dr.close()
			catch ex as exception
				erros &= "<br>consulta("& pretorno &", "& pstrSql &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try

		end if
		'FIM MYSQL
		'------------------------------------------------------
		
		return r
	End Function
	'Fim Consulta
	'***********************************************************
	
	
	'***********************************************************
	'Consulta Multiplos Campos
	Public Function consulta(byval pv() as string, byval pstrSql as String) as HashTable
		Dim r as new HashTable
		Dim strSql as String = pstrSql
		
		'------------------------------------------------------
		'MSSQL
		if dbType = "MSSQL"
		
			Dim cx as SqlConnection = new SqlConnection(dbR)
			Dim web as SqlCommand
			Dim dr as SqlDataReader
			
			cx.open()
			try
				web = new SqlCommand(strSql, cx)
				dr = web.executeReader()
				if dr.read() then
					if pv.length = 1 then
						r.add(pv(0), dr(pv(0)).toString())
					else
						
						for each x as string in pv
							r.add(x, dr(x).toString())
						next
						
					end if
				end if
				dr.close()
			catch ex as exception
				erros &= "<br>consulta("& pstrSql &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try
		
		end if
		'FIM MSSQL
		'------------------------------------------------------
		
		
		'------------------------------------------------------
		'MYSQL
		if dbType = "MYSQL" then
			
			Dim cx as MySqlConnection = new MySqlConnection(dbR)
			Dim web as MySqlCommand
			Dim dr as MySqlDataReader
			
			cx.open()
			try
				web = new MySqlCommand(strSql, cx)
				dr = web.executeReader()
				if dr.read() then
					if pv.length = 1 then
						r.add(pv(0), dr(pv(0)).toString())
					else
						
						for each x as string in pv
							r.add(x, dr(x).toString())
						next
						
					end if
				end if
				dr.close()
			catch ex as exception
				erros &= "<br>consulta("& pstrSql &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try

		end if
		'FIM MYSQL
		'------------------------------------------------------
		
		return r
	End Function
	'Fim Consulta Multiplos Campos
	'***********************************************************
	
	
	'***********************************************************
	'Consulta Multiplos Varias linhas em datatable
	Public Function consulta(byval pstrSql as String) as DataTable
		Dim r as new Datatable()
		Dim strSql as String = pstrSql
		
		'------------------------------------------------------
		'MSSQL
		if dbType = "MSSQL"
		
			Dim cx as SqlConnection = new SqlConnection(dbR)
			Dim da as SqlDataAdapter
			
			cx.open()
			try
				da = new SqlDataAdapter(strSql, cx)
				da.Fill(r)
				da.Dispose()
			catch ex as exception
				erros &= "<br>consulta("& pstrSql &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try
		
		end if
		'FIM MSSQL
		'------------------------------------------------------
		
		
		'------------------------------------------------------
		'MYSQL
		if dbType = "MYSQL" then
			
			Dim cx as MySqlConnection = new MySqlConnection(dbR)
			Dim da as MySqlDataAdapter
			
			cx.open()
			try
				da = new MySqlDataAdapter(strSql, cx)
				da.Fill(r)
				da.Dispose()
			catch ex as exception
				erros &= "<br>consulta("& pstrSql &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try

		end if
		'FIM MYSQL
		'------------------------------------------------------
		
		return r
	End Function
	'Fim Consulta Multiplos Varias linhas em datatable
	'***********************************************************
	
	
	
	'***********************************************************
	'Consulta todos os campos de uma linha
	Public Function consultaLinha(byval pstrSql as String) as Hashtable
		Dim r as new Hashtable()
		Dim strSql as String = pstrSql
		Dim dt as new Datatable()
		
		'------------------------------------------------------
		'MSSQL
		if dbType = "MSSQL"
			
			Dim cx as SqlConnection = new SqlConnection(dbR)
			Dim da as SqlDataAdapter
			
			cx.open()
			try
				da = new SqlDataAdapter(strSql, cx)
				da.Fill(dt)
				da.Dispose()
				
				for each y as system.data.datarow in dt.rows
					
					for each x as system.data.datacolumn in dt.columns
						r.add(x.columnName, y(x.columnName))
					next
					
				next
				
			catch ex as exception
				erros &= "<br>consulta("& pstrSql &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try
			
		end if
		'FIM MSSQL
		'------------------------------------------------------
		
		
		'------------------------------------------------------
		'MYSQL
		if dbType = "MYSQL" then
			
			Dim cx as MySqlConnection = new MySqlConnection(dbR)
			Dim da as MySqlDataAdapter
			
			cx.open()
			try
				da = new MySqlDataAdapter(strSql, cx)
				da.Fill(dt)
				da.Dispose()
				
				for each y as system.data.datarow in dt.rows
					
					for each x as system.data.datacolumn in dt.columns
						r.add(x.columnName, y(x.columnName))
					next
					
				next
				
			catch ex as exception
				erros &= "<br>consulta("& pstrSql &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try

		end if
		'FIM MYSQL
		'------------------------------------------------------
		
		return r
	End Function
	'Fim Consulta Multiplos Varias linhas em datatable
	'***********************************************************
	
	
	
	
	'***********************************************************
	'Executa
	Public Function executa(byval pstrSql as string) as boolean
		Dim r as boolean = false
		Dim strSql as String = pstrSql
		
		'------------------------------------------------------
		'MSSQL
		if dbType = "MSSQL" then
		
			Dim cx as SqlConnection = new SqlConnection(dbW)
			Dim web as SqlCommand
			
			cx.open()
			try
				web = new sqlCommand(strSql, cx)
				web.executeNonQuery()
				r = true
			catch ex as exception
				erros &= "<br>executa("& pstrSql &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try
		
		end if
		'FIM MSSQL
		'------------------------------------------------------
		
		'------------------------------------------------------
		'MYSQL
		if dbType = "MYSQL" then
		
			Dim cx as MySqlConnection = new MySqlConnection(dbW)
			Dim web as MySqlCommand
			
			cx.open()
			try
				web = new MysqlCommand(strSql, cx)
				web.executeNonQuery()
				r = true
			catch ex as exception
				erros &= "<br>executa("& pstrSql &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try
		
		end if
		'FIM MYSQL
		'------------------------------------------------------
		
		return r
	End Function
	'Fim Executa
	'***********************************************************
	
	'***********************************************************
	'Stored Procedure
	Public Function SP(byval pv() as string, byval pstrSql as string) as String
		Dim r as String = ""
		Dim strSql as String = pstrSql
		
		'------------------------------------------------------
		'MSSQL
		if dbType = "MSSQL" then
			
			Dim cx as SqlConnection = new SqlConnection(dbW)
			Dim web as SqlCommand = new SqlCommand(strSql, cx)
			Dim dr as SqlDataReader
			
			cx.open()
			try
				
				dr = web.executeReader()
				if dr.read() then
					if pv.length = 1 then
						r = dr(pv(0)).toString()
					else
						
						for each x as string in pv
							
							if r = "" then
								r &= ""& dr(x).toString()
							else
								r &= "|"& dr(x).toString()
							end if
						next
						
					end if
				end if
				dr.close()
				
			catch ex as exception
				erros &= "<br>SP("& pstrSql &"): "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try
			
		end if
		'FIM MSSQL
		'------------------------------------------------------
		
		'------------------------------------------------------
		'MYSQL
		if dbType = "MYSQL" then
			
			Dim cx as MySqlConnection = new MySqlConnection(dbW)
			Dim web as MySqlCommand = new MySqlCommand(strSql, cx)
			Dim dr as MySqlDataReader
			
			cx.open()
			try
				
				dr = web.executeReader()
				if dr.read() then
					if pv.length = 1 then
						r = dr(pv(0)).toString()
					else
						
						for each x as string in pv
							
							if r = "" then
								r &= ""& dr(x).toString()
							else
								r &= "|"& dr(x).toString()
							end if
						next
						
					end if
				end if
				dr.close()
				
			catch ex as exception
				erros &= "<br>SP("& pstrSql &"): "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try
			
		end if
		'FIM MYSQL
		'------------------------------------------------------
		
		return r
	End Function
	'Fim Stored Procedure
	'***********************************************************
	
	
	
	'***********************************************************
	'Descreve Tabela
	Public Function descTable(byval pTabela as string) as String
		Dim r as String = ""
		Dim strSql as String = ""
		Dim dt as new Datatable()
		
		tabela.clear()
		
		'------------------------------------------------------
		'MSSQL
		if dbType = "MSSQL" then
		
			strSql = "select top 1 * from  "& pTabela &" "
			
			Dim cx as SqlConnection = new SqlConnection(dbW)
			Dim da as SqlDataAdapter = new SqlDataAdapter(strSql, cx)
			
			cx.open()
			try
				da.FillSchema(dt, SchemaType.Source)
				da.Fill(dt)
				da.Dispose()
				
				for each x as DataColumn in dt.columns
					tabela.add(new iDBTabela(x.columnName.toString(),  x.dataType.toString(), x.defaultValue.toString(), x.autoIncrement.toString(), x.maxLength.toString()))
				next
				
				tabelaJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(tabela)
				
			catch ex as exception
				erros &= "<br>descTable("& pTabela &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try
		
		end if
		'FIM MSSQL
		'------------------------------------------------------
		
		'------------------------------------------------------
		'MYSQL
		if dbType = "MYSQL" then
			
			strSql = "select * from  "& pTabela &" limit 1 "
			
			Dim cx as MySqlConnection = new MySqlConnection(dbW)
			Dim da as MySqlDataAdapter = new MySqlDataAdapter(strSql, cx)
			
			cx.open()
			try
				da.FillSchema(dt, SchemaType.Source)
				da.Fill(dt)
				da.Dispose()
				
				for each x as DataColumn in dt.columns
					tabela.add(new iDBTabela(x.columnName.toString(),  x.dataType.toString(), x.defaultValue.toString(), x.autoIncrement.toString(), x.maxLength.toString()))
				next
				
				tabelaJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(tabela)
				
			catch ex as exception
				erros &= "<br>descTable("& pTabela &") : "& ex.toString()
			finally
			cx.close()
			cx.dispose()
			end try
			
		end if
		'FIM MYSQL
		'------------------------------------------------------
		
		r = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(tabela)
		
		return r
	End Function
	'Fim Descreve Tabela
	'***********************************************************
	
	
	
	
	'***********************************************************
	''Métodos internos
	'***********************************************************
	
	'inSelecionaDB - Método interno que seleciona a base de dados no caso do cliente ter mais de 1 base de dados
	Public Function SelecionaDB(byval pdb as string) as Boolean
		dim r as boolean = false
		
		if trim(pdb) <> trim(db) then
			
			Dim tmp as iDB
			For each tmp in bancos
				if(tmp.db = pdb) then
					db = tmp.db
					dbType = tmp.dbType
					dbR = tmp.dbR
					dbW = tmp.dbW
				end if
			Next
			
		end if
		
		return r
	End Function
	
	
	'***********************************************************
	''Métodos auxiliares
	'***********************************************************
	
	'SS - Secure Sql - Método utilizado para filtrar valores de dados SQL para impedir ataques
	Public Function SS(ByVal v as String) as String
		dim r as String = ""
		dim aux() as String = {"--","insert", "update", "delete","drop","select"}
		dim x as integer = 0
		
		for x = 0 to ubound(aux) 
			v = replace(v,aux(x),"")
		next
		
		r = trim(replace(replace(v,"'","''"), chr(34),"''"))
		
		return r
	End Function
	
	
	
	
	
End Class
'Fim classe DB


'----------------------------------------------------------
'Interface iDB
Public class iDB
	Implements IDisposable
	
	private sdb as string = ""
	private sdbtype as string = ""
	private sdbr as string = ""
	private sdbw as string = ""
	
	public property db() as string
		get
			return sdb
		end get
		set(byval v as string)
			sdb = v
		end set
	end property
	
	public property dbType() as string
		get
			return sdbtype
		end get
		set(byval v as string)
			sdbtype = v
		end set
	end property
	
	public property dbR() as string
		get
			return sdbr
		end get
		set(byval v as string)
			sdbr = v
		end set
	end property
	
	public property dbW() as string
		get
			return sdbw
		end get
		set(byval v as string)
			sdbw = v
		end set
	end property
	
	public sub new()
		
	end sub
	
	public sub new(byval pdb as string, byval pdbtype as string, byval pdbr as string, byval pdbw as string)
		db = pdb
		dbType = pdbtype
		dbR = pdbr
		dbW = pdbw
	end sub

	public sub dispose() implements System.IDisposable.Dispose
		db = ""
		dbType = ""
		dbR = ""
		dbW = ""
	end sub
	
End Class


Public Class iDBTabela
	Implements IDisposable
	
	private sfield as String = ""
	private sdatatype as String = ""
	private sdefaultValue as String = ""
	private sautoIncrement as String = ""
	private smaxLength as String = ""
	
	public property field() as string
		get
			return sfield
		end get
		set(byval v as string)
			sfield = v
		end set
	end property
	
	public property dataType() as string
		get
			return sdataType
		end get
		set(byval v as string)
			sdataType = v
		end set
	end property
	
	public property defaultValue() as string
		get
			return sdefaultValue
		end get
		set(byval v as string)
			sdefaultValue = v
		end set
	end property
	
	public property autoIncrement() as string
		get
			return sautoIncrement
		end get
		set(byval v as string)
			sautoIncrement = v
		end set
	end property
	
	public property maxLength() as string
		get
			return smaxLength
		end get
		set(byval v as string)
			smaxLength = v
		end set
	end property
	
	Public Sub New()
		
	End Sub
	
	Public Sub New(byval pfield as string, byval pdataType as string, byval pdefaultValue as string, byval pautoIncrement as string, byval pmaxLength as string)
		
		field = pfield
		dataType = pdataType
		defaultValue = pdefaultValue
		autoIncrement = pautoIncrement
		maxLength = pmaxLength
		
	End Sub
	
	Public Sub Dispose() implements System.IDisposable.Dispose
		field = ""
		dataType = ""
		defaultValue = ""
		autoIncrement = ""
		maxLength = ""
	End Sub
	
End Class



'-----------------------------------------------------------------------------
'Classe para dados FIELD VALUE
Public Class DBFV
	Implements IDisposable
	
	Public dados as List(Of iFV) = new List(Of iFV)
	
	Public Sub New()
	End Sub
	
	Public Function retorna(byval ptipo as string) as String
		Dim r as String = ""
			
			Dim aux as String = ""
			
			For each x as iFV in dados
				
				if ptipo = "campos" then
					if r = "" then
						r &= x.campo
					else
						r &= ","& x.campo
					end if
				end if
				
				if ptipo = "valores" then
					
					if x.texto = 1 then
						aux = "'"
					else
						aux = ""
					end if
					
					if r = "" then
						r &= aux & x.valor & aux
					else
						r &= ","& aux & x.valor & aux
					end if
				end if
				
				if ptipo = "camposValores" then
					if x.texto = 1 then
						aux = "'"
					else
						aux = ""
					end if
					
					if r = "" then
						r = x.campo &"="& aux & x.valor & aux
					else
						r &= ","& x.campo &"="& aux & x.valor & aux
					end if
				end if
				
				if ptipo = "json" then
					if r = "" then
						r = chr(34) & x.campo & chr(34) &":"& chr(34) & replace(x.valor, "'", "") & chr(34)
					else
						r &= ","& chr(34) & x.campo & chr(34) &":"& chr(34) & replace(x.valor, "'", "") & chr(34)
					end if
				end if
				
			Next
		
		if ptipo = "json" then
			r = "{"& r &"}"
		end if
		
		return r
	End Function
	
	Public Sub Dispose() Implements System.IDisposable.Dispose
		dados.clear()
	End Sub
	
End Class

Public Class iFV
	Implements IDisposable
	
	private scampo as string = ""
	private svalor as string = ""
	private itexto as integer = 1
	
	public property campo() as string
		get
			return scampo
		end get
		set(byval v as string)
			scampo = v
		end set
	end property
	
	public property valor() as string
		get
			return svalor
		end get
		set(byval v as string)
			svalor = SECDATA(v)
		end set
	end property
	
	public property texto() as integer
		get
			return itexto
		end get
		set(byval v as integer)
			itexto = v
		end set
	end property
	
	
	Public Sub New()
	End Sub
	
	Public Sub New(byval pcampo as string, byval pvalor as string)
		campo = pcampo
		valor = pvalor
		texto = 1
	End Sub
	
	Public Sub New(byval pcampo as string, byval pvalor as string, byval ptexto as string)
		campo = pcampo
		valor = pvalor
		if isNumeric(ptexto) then
			texto = cInt(ptexto)
		end if
	End Sub
	
	Public Sub Dispose() Implements System.IDisposable.Dispose
		campo = ""
		valor = ""
		texto = 1
	End Sub
	
	
	'******
	'SS - Secure Sql - Método utilizado para filtrar valores de dados SQL para impedir ataques
	Private Function SECDATA(ByVal v as String) as String
		dim r as String = ""
		'dim aux() as String = {"--","insert", "update", "delete", "drop", "select"}
		dim aux() as String = {"--", "insert", "delete"}
		dim x as integer = 0
		
		for x = 0 to ubound(aux) 
			v = replace(v,aux(x),"")
		next
		
		r = trim(replace(replace(v,"'","''"), chr(34),"''"))
		
		return r
	End Function
	
End Class


