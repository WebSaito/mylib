Imports System
Imports System.Collections

Namespace Spacelab
	
	Public Class sURL
		Implements IDisposable
		
		'Propriedades
		Public paginas as list(of iUPaginas) = new list(of iUPaginas)
		
		Public erros as String = ""
		
		'Método Construtor 
		Public Sub New()
		'--
		End Sub
		
		'Método que faz limpeza do objeto
		Public Sub Dispose() Implements System.IDisposable.Dispose
			paginas.clear()
		End Sub
		
		'Método que inicializa dados de paginas
		Public Function adicionarPaginas(byval pv() as String) as boolean
			Dim r as boolean = false
			Dim aux() as String
			Dim y as Integer = 0
			
			Dim pagina as String = ""
			Dim codigo as String = ""
			Dim parametros() as String
			
			For each x as String in pv
				aux = split(x,":")
				
				paginas.add(new iUPaginas(aux(0), aux(1), aux(2)))
				
			Next
			
			return r
		End Function
		
		'Métdo que segmenta a url e retorna segmento - indice
		Public Shared Function segmento(ByVal pu As Uri, ByVal pindice As Integer) As String
			Dim r As String = ""
			Dim p As Uri = pu
			
				r = replace(p.Segments(p.Segments.length - pindice), "/","")
			
			return r
		End Function
		
		
		'Método que retorna o destino baseado em código da url
		Public Function destino(byval purl as uri) As String
			Dim r as String = ""
			
			Dim x as Integer = 1
			Dim y as Integer = 1
			Dim cod as String = segmento(purl, 1)
			Dim par as String = ""
			Dim aux() as String
			
			if paginas.count > 0 then
				Try
				'-------------------------------------------------
				'VERIFICANDO SE É ACTION DE SUBMIT
				if cod="enviar" then
					
					'V.1- precisa ser melhorado para encontrar automaticamente pastas e subpastas
					r = "/"& segmento(purl, 3) &"/"& segmento(purl, 2) &".aspx?acao=enviar"
					
				else
				'-------------------------------------------------
				'USANDO CODIGO PARA PAGINAS
				
					'Encontrar a pagina atraves do código
					For each tpag as iUPaginas in paginas
						if trim(tpag.codigo) = trim(cod) then
						
							aux = split(tpag.parametros, "|")
							
							'Pagina encontrada agora montar os parametros da pagina de acordo com os parametros do registro
							if aux.length > 1 then
								y = ubound(aux) + 2
								For x = 0 to ubound(aux)
									if par = "" then
										par &= ""& aux(x) &"="& segmento(purl, y)
									else
										par &= "&"& aux(x) &"="& segmento(purl, y)
									end if
									
									y = y -1
								Next
								
							else
								par = aux(0) &"="& segmento(purl, 2)
							end if
							
							If par <> "" then
								par = "?"& par &"&urlcod="& cod
							else
								par = par &"&urlcod="& cod
							End If
							
							r = tpag.pagina & par
							
						end if
					Next
					
					'------------------------------------------------------
					'TRATAR AQUI AS PAGINAS FORA DO RANGE DE CODIGO
					if r = "" then
					
					end if
					
				end if
				
				
				
				Catch ex as Exception
					erros &= "erro:"& ex.toString()
				End Try
			else
				erros &= "Paginas de destino não foram adicionadas ao objeto"
			end if
			
			return r
		End Function
		
		Public Shared Function incluir(ByVal u As String, ByVal enc As String) As String
			Dim r As String = ""
			
			Try
			
				Dim w As System.Net.WebClient = new System.Net.WebClient()
				Dim tb() as Byte
				
				tb = w.DownloadData(u)
				
				If enc = "UTF8" Then
					r = System.Text.Encoding.UTF8.GetString(tb)
				Else				
					r = System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(tb)
				End If
				
				w.Dispose()
				
			Catch ex As Exception
				'r = ex.ToString() &"<br><br><br>"& u
				'erros &= ex.ToString() &"<br><br><br>"& u
				
			End Try
			
			return r
		End Function
		
		
	End Class
	
	
	Public Class iUPaginas
		Implements IDisposable
		
		private scodigo as string = ""
		private spagina as string = ""
		private sparametros as string = ""
		
		public property codigo() as string
			get
				return scodigo
			end get
			set(byval v as string)
				scodigo = v
			end set
		end property
		
		public property pagina() as string
			get
				return spagina
			end get
			set(byval v as string)
				spagina = v
			end set
		end property
		
		public property parametros() as string
			get
				return sparametros
			end get
			set(byval v as string)
				sparametros = v
			end set
		end property
		
		Public Sub New()
		
		End Sub
		
		Public Sub New(byval pcodigo as string, byval ppagina as string, byval pparametros as string)
			codigo = pcodigo
			pagina = ppagina
			parametros = pparametros
		End Sub
		
		Public Sub Dispose() Implements System.IDisposable.Dispose
			codigo = ""
			pagina = ""
			parametros = ""
		End Sub
		
	End Class

	
	
	
	
	
End Namespace