Imports System
Imports System.Collections
Imports System.IO
Imports System.Net

Namespace SAPI
	
	Public Class API1
		Inherits objApi
		Implements IDisposable
		
		Public erros as String = ""
		Public jss as new System.Web.Script.Serialization.JavaScriptSerializer()
		Public backdata as new Hashtable()
		Public postdata as String = ""
		Public jsonPost
		
		Public Sub New()
			metodo = HttpContext.Current.Request.HttpMethod
			contentType = "application/x-www-form-urlencoded"
		End Sub
		
		Public Sub Dispose() Implements System.IDisposable.Dispose
			metodo = ""
			cliente = ""
			segredo = ""
			token = ""
			retorno = ""
			tipoRetorno = ""
			'---
			erros = ""
		End Sub
		
		'---
		Public Function pegaClienteSegredo() as Boolean
			Dim r as Boolean = false
			
				try
					Dim valoresHeader() as IEnumerable = HttpContext.Current.Request.Headers.GetValues("Cliente")
					cliente = valoresHeader.FirstOrDefault()
					
					valoresHeader = HttpContext.Current.Request.Headers.GetValues("Segredo")
					segredo = valoresHeader.FirstOrDefault()
					
					r = true
				catch ex as exception
					erros &= " pegaClienteSegredo(): "& ex.toString()
				end try
				
			return r
		End Function
		
		
		'---
		Public Function pegaDados() as boolean
			Dim r as boolean = false
				
				try
					postdata = ""
					
					for each chave as String in HttpContext.Current.Request.Form.AllKeys
						postdata &= IIF(postdata = "", "", ",") &"'"& chave &"':'"& HttpContext.Current.Request.Form(chave) &"'"
					next
					
					postdata = "{"& postdata &"}"
					jsonPost = jss.DeserializeObject(postdata)
					
					r = true
				catch ex as exception
					erros &= " pegaDados(): "& ex.toString()
				end try
				
			return r
		End Function
		
		
		Public Function pegaJSON() as boolean
			Dim r as boolean = false
				
				try
					
					postdata = ""
					HttpContext.Current.Request.InputStream.Position = 0
					Dim inputStream = new StreamReader(HttpContext.Current.Request.InputStream)
					postdata = inputStream.ReadToEnd()
					
					jsonPost = jss.DeserializeObject(postdata)
					
					r = true
				catch ex as exception
					erros &= " pegaJSON(): "& ex.toString()
				end try
				
			return r
		End Function
		
		
		'---
		Public Function postaRedireciona() as String
			
			for each chave as string in backdata.keys
				
			next
			
		End Function
		
	End Class
	
	
	Public Class objApi
		Implements IDisposable
		
		Private smetodo as String = ""
		Private scliente as String = ""
		Private ssegredo as String = ""
		Private stoken as String = ""
		Private sretorno as String = ""
		Private stipoRetorno as String = ""
		
		Private scontentType as String = ""
		
		Public Property metodo() as String
			Get
				return smetodo
			End Get
			Set(byval v as string)
				smetodo = v
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
		
		Public headers as List(Of objHeader) = new List(Of objHeader)
		
		Public Sub New()
			
		End Sub
		
		Public Sub Dispose() Implements System.IDisposable.Dispose
			metodo = ""
			cliente = ""
			segredo = ""
			token = ""
			retorno = ""
			tipoRetorno = ""
			contentType = ""
			headers.clear()
		End Sub
		
	End Class
	
	Public Class objHeader
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