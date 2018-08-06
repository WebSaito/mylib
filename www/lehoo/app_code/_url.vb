Imports System

Namespace nsUrl
    ' CLASSE URL
	' Metodos de URL
    Public Class clsUrl
		
		
		'[INICIO] Função para retornar o conteúdo de uma página em String =============================================================
		Public Function funcModulo(ByVal u As String, ByVal enc As String, byval referer as string) As String
			Dim r As String = ""
			
			Try
			
				Dim w As System.Net.WebClient = new System.Net.WebClient()
				Dim tb() as Byte

				if(referer <> "")then
				
					w.Headers.Add("Referer", referer)
				
				end if
				tb = w.DownloadData(u)
				
				If enc = "UTF8" Then
					r = System.Text.Encoding.UTF8.GetString(tb)
				Else				
					r = System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(tb)
				End If
				
				w.Dispose()
				
			Catch ex As Exception
				r = ex.ToString()
			End Try
			
			return r
		End Function
		'[FIM] Função para retornar o conteúdo de uma página em String ================================================================
		
		'[INICIO] funcModulo > HELP ===================================================================================================
		public function funcModulo(byval strTxt as string) as string
			
			dim helpRetorno as string 			'Retorno do Texto Descritivo					
			
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionlidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Método utilizado para fazer a leitura de uma página remota (Ex.:http://www.google.com.br) e retornar o conteúdo em texto (String)."
			helpParametros 		 = "(ByVal u As String, ByVal enc As String)"			
			helpFuncionlidades  = "u: String contendo o caminho da pagina que deseja executar e retornar o conteudo."
			helpFuncionlidades += "<br/>enc: String com o tipo de codificação de página retornada - 'UTF8' ou 'ISO-8859-1'."			
			helpExemplo 		 = "nsUrl.clsUrl.funcModulo("& chr(34) &"http://www.spacelab.com.br/gds/teste.aspx"& chr(34) &","& chr(34) &"UTF8"& chr(34) &")"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionlidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
			return helpRetorno
			
		end function
		'[FIM] funcModulo > HELP =====================================================================================================				
		
		'[INICIO] Método utilizado para retornar segmento de uma URL =================================================================
		Public Function funcURLSegmento(ByVal u As Uri, ByVal segmento As Integer) As String
			Dim r As String = ""
			Dim p As Uri = u
			
				r = p.Segments(p.Segments.length - segmento)
			
			return r
		End Function
		'[FIM] Método utilizado para retornar segmento de uma URL =====================================================================		
		
		'[INICIO] funcURLSegmento > HELP ==============================================================================================
		public function funcURLSegmento(byval strTxt as string) as string
			
			dim helpRetorno as string 			'Retorno do Texto Descritivo					
			
			dim helpDescricao as string 		'Para que serve
			dim helpParametros as string		'parametro
			dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
			dim helpExemplo as string			'exemplo de como deve ser chamado
			
			helpDescricao 		 = "Método utilizado para retornar segmento de uma URL completa conforme o indice indicado e retornar o conteúdo em texto (String)."
			helpParametros 		 = "(ByVal u As Object, ByVal segmento As Integer)"			
			helpFuncionalidades  = "u: String contendo o caminho da pagina que deseja retornar segmento."
			helpFuncionalidades += "<br/>segmento: numero inteiro que define indice da parte da URL a retornar."			
			helpExemplo 		 = "nsUrl.clsUrl.funcURLSegmento("& chr(34) &"http://www.spacelab.com.br/gds/teste.aspx"& chr(34) &","& chr(34) &"UTF8"& chr(34) &")"			
			
			helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
			helpRetorno += "# Parametros: " & helpParametros & "<br/>"
			helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
			helpRetorno += "# Exemplo: " & helpExemplo 
			
			return helpRetorno
			
		end function
		'[FIM] funcURLSegmento > HELP =================================================================================================											
		
		'[INICIO] Método para retornar UM dominio =====================================================================================
		Public Function funcURLDominio(ByVal u As Uri) As String
			Dim r As String = ""
			Dim p As Uri = u
				
				r = p.Authority
								
			return r
		End Function
		'[FIM] Método para retornar UM dominio ========================================================================================				
		
		'[INICIO] funcURLDominio > HELP ==============================================================================================
		public function funcURLDominio(byval strTxt as string, Byval strTxt2 as Boolean) as string

			dim helpRetorno as string = "" 			'Retorno do Texto Descritivo					
			
			If strTxt = "help" and strTxt2 = true Then

				dim helpDescricao as string 		'Para que serve
				dim helpParametros as string		'parametro
				dim helpFuncionalidades as string	'como usar os parametros e tipo de dados
				dim helpExemplo as string			'exemplo de como deve ser chamado
				
				helpDescricao 		 = "Método utilizado para retornar dominio de uma url enviada e retornar o conteúdo em texto (String)."
				helpParametros 		 = "(ByVal u As Uri)"			
				helpFuncionalidades  = "u: String contendo o caminho da pagina que deseja retornar o dominio."
				helpExemplo 		 = "nsUrl.clsUrl.funcURLDominio("& chr(34) &"http://www.spacelab.com.br/gds/teste.aspx"& chr(34) &")"			
				
				helpRetorno  = "# Descri&ccedil;&atilde;o: " & helpDescricao & "<br/>"
				helpRetorno += "# Parametros: " & helpParametros & "<br/>"
				helpRetorno += "# Funcionalidades: " & helpFuncionalidades & "<br/>"
				helpRetorno += "# Exemplo: " & helpExemplo 
				
			End If
			
			return helpRetorno
			
		end function
		'[FIM] funcURLDominio > HELP =================================================================================================															
		
		'[INICIO] funcURLDominio ==================================
		Public Function funcURLSub(ByVal u As Uri) As String
			
			Dim r As String = ""
			Dim p As Uri = u
			Dim a As String
			Dim v() As String
			
				a = p.Authority
				v = split(a,".")
				r = v(0)
				
			return r
		End Function
		'[FIM] funcURLDominio ==================================
		
	End Class
End Namespace