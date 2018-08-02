Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections

Namespace Spacelab 
	
	Public Class Utils
		Implements IDisposable
		
		
		'Método Construtor 
		Public Sub New()
		'--
		End Sub
		
		'Método que faz limpeza do objeto
		Public Sub Dispose() Implements System.IDisposable.Dispose
			
		End Sub
		
		'*********************************************************************
		'  Método : AcentoHibrido
		'  Parametros : 6 (id, modulo, disponivel, restrito, dominio, posicao)
		'  Retorno : Ok se nao houver erro, e 0000 caso haja erro
		'*********************************************************************
		Public Shared Function AcentoHibrido(ByVal texto As String, ByVal modo As String) As String
			dim resultado as String = texto
			dim x as Integer
			dim expr1() As String = {"Á","á","É","é","Í","í","Ó","ó","Ú","ú","Ã","ã","Õ","õ","Â","â","Ê","ê","Î","î","Ô","ô","Û","û","À","à","È","è","Ì","ì","Ò","ò","Ù","ù","Ç","ç"}
			dim expr2() As String = {"&Aacute;","&aacute;","&Eacute;","&eacute;","&Iacute;","&iacute;","&Oacute;","&oacute;","&Uacute;","&uacute;","&Atilde;","&atilde;","&Otilde;","&otilde;","&Acirc;","&acirc;","&Ecirc;","&ecirc;","&Icirc;","&icirc;","&Ocirc;","&ocirc;","&Ucirc;","&ucirc;","&Agrave;","&agrave;","&Egrave;","&egrave;","&Igrave;","&igrave;","&Ograve;","&ograve;","&Ugrave;","&ugrave;","&Ccedil;","&ccedil;"}
			dim expr3() As String = {"A","a","E","e","I","i","O","o","U","u","A","a","O","o","A","a","E","e","I","i","O","o","U","u","A","a","E","e","I","i","O","o","U","u","C","c"}
			
			If modo = "aPARAhtml" Then
				For x = 0 to ubound(expr1)
					resultado = Regex.Replace(resultado, expr1(x), expr2(x))
				Next
			End if
			
			If modo = "aPARAsemA" Then
				For x = 0 to ubound(expr1)
					resultado = Regex.Replace(resultado, expr1(x), expr3(x))
				Next
			End if
			
			If modo = "htmlPARAa" Then
				For x = 0 to ubound(expr1)
					resultado = Regex.Replace(resultado, expr2(x), expr1(x))
				Next
			End if
			
			If modo = "htmlPARAsemA" Then
				For x = 0 to ubound(expr1)
					resultado = Regex.Replace(resultado, expr2(x), expr3(x))
				Next
			End if
			
			return resultado
			
		End Function
		
		'[INICIO] Método para formatar o link para url amigavel
		'------------------------------------------------------
		Public Function rLink(byval v as string) as String
		Dim r as String = ""
		
		r = lcase(replace(replace(replace(replace(replace(replace(replace(replace(replace(AcentoHibrido(v,"aPARAsemA")," ", "-"),".",""),",",""),"+","-"),"=","-"),":",""),"<br>",""),">",""),"<",""))
		
		return r
	End Function
		
		'Método que verifica cookie pelo nome e retorna seu valor caso houver
		Public Shared Function rCookie(byval pnome as string) As String
			Dim r as String = ""
				
				If Not HttpContext.Current.Request.Cookies(pnome) Is Nothing then
					r = HttpContext.Current.Request.Cookies(pnome).value
				End If
				
			return r
		End Function
		
		Public Shared Function rDataMask(byval v as object, byval pmask as string) as String
			Dim r as String = ""
			
			If Not ISDBNull(v) then
				If IsDate(v) then
					
					Dim dtAux as Datetime = v
					r = dtAux.toString(pmask)
					
				End If
			End If
			
			return r
		End Function
		
		Public Shared Function toLink(byval v as string) as String
			Dim r as String = ""
			
			if trim(v) <> "" then
			
			r = AcentoHibrido(v, "aPARAsemA")
			'Substituindo caracteres que podem causar erros
			Dim caracErros() as String = {".","=","+","*","/","\"," ","?","*",":","[","]","{","}",";","'",chr(34),"`","~","^","<",">"}
			Dim x as Integer = 0
			
			For x = 0 to ubound(caracErros)
				r = replace(r, caracErros(x), "-")
			Next
			
			'Retornando lower case
			r = lcase(r)
			
			r = Regex.Replace(r, "[áâàåãä]", "a", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "æ", "ae", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "[éêèë]", "e", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "[íîïì]", "i", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "[óôòõö]", "o", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "[ø]", "0", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "[úûùü]", "u", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "ç", "c", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "ñ", "n", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "ý", "y", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "(<br\/?>)", "-", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "[\s_]", "-", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "[^\w\d-]", "", RegexOptions.IgnoreCase)
			r = Regex.Replace(r, "-{2,}", "-", RegexOptions.IgnoreCase)
			
			end if
			
			return r
		End Function
		
		Public Shared Function toPlainText(byval v as string) as String
			Dim r as String = ""
			
			r = Regex.Replace(v, "<[^>]+>", " ")
			
			return r
		End Function
		
		Public Shared Function mes(byval v as string) as String
			Dim r as String = ""
			
			Dim meses() as String = {"","Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"}
			
			if isnumeric(v) then
				if cInt(v) > 0 and cInt(v) < 13 then
					r = meses(cInt(v))
				end if
			end if
			
			return r
		End Function
		
		Public Shared Function blogImg(byval pimg as string, byval ptit as string, byval ppost as string) as String
			Dim r as String = ""
			
			if trim(pimg) <> "" then
				r = "<img src='/_upload/blog/capas/"& pimg &"'  alt='"& ptit &"'>"
			else
				Dim imgSrc = getImg(ppost)
				
				If imgSrc <> "" Then
					r = "<img src='"& imgSRC &"'  alt='"& ptit &"'>"
				End If
			end if
			
			return r
		End Function
		
		Public Shared Function blogResumo(byval ppost as string, byval limite as integer) as String
			Dim r as string = ""
			
			if trim(ppost) <> "" then
				
				Dim tmp as string = toPlainText(ppost)

				tmp = Regex.Replace(tmp, "[\r\n]", " ")
				
				if tmp.length > limite then
					'r = left(tmp, limite) &"..."
					Dim regexResumo As New Regex("^(.{0," & limite.ToString() & "})\s.*", RegexOptions.IgnorePatternWhitespace)
					r = regexResumo.Replace(tmp, "$1")&"..."
				else
					r = tmp
				end if
				
			end if
			
			return r
		End Function
		
		Public Shared Function getImg(byval ptext as string) as string
			Dim r as string = ""
			
			if trim(ptext) <> "" then
				Dim regex As Regex = New Regex("<img\b[^>]+?src\s*=\s*['""]?([^\s'""?#>]+)")
				Dim match As Match = regex.Match(ptext)

				If match.Success Then
					r = match.Groups(1).Value
				End If
			end if
			
			return r
		End Function
		
		Public Shared Function porcentagem(byval ptotal as string, byval pparte as string) as string
			Dim r as string = "0"
				
				if isnumeric(ptotal) then
					if isnumeric(pparte) then
						if ((cDbl(ptotal) > 0) and (cDbl(pparte) > 0))then
							r = replace(formatNumber(((cDbl(pparte) * 100)/cDbl(ptotal)),2), ",00", "")
						end if
					end if
				end if
				
			return r
		End Function
		
		
		
		'----------------------------------------------
		'2016
		Public Shared Function remote(ByVal u As String, Optional ByVal enc As String = "UTF8") As String
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
				r = ex.ToString() &"<br><br><br>"& u
			End Try
			
			return r
		End Function
		
		
		Public Shared Function local(ByVal u As String, Optional ByVal enc As String = "UTF8") As String
			Dim r As String = ""
			
			Try
				
			'	Dim ms as System.IO.MemoryStream = new System.IO.MemoryStream()
			'	Dim writer as System.IO.StreamWriter = new System.IO.StreamWriter(ms)
			'	HttpContext.Current.Server.Execute(u, writer)
			'	
			'	If enc = "UTF8" Then
			'		r = System.Text.Encoding.UTF8.GetString(ms.toArray())
			'	Else				
			'		r = System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(ms.toArray())
			'	End If
				
				Dim sw as New System.IO.StringWriter
				
				HttpContext.Current.Server.Execute(u, sw)
				r = sw.toString()
				
			Catch ex As Exception
				r = ex.ToString() &"<br><br><br>"& u
			End Try
			
			return r
		End Function
		
		
		'----------------------------------------------
		'2017
		Public Shared Function pegaOS() as String
			Dim r as String = ""
			
			Dim v as String  = HttpContext.Current.Request.UserAgent.toString()
			
			Dim aux() as String
			aux = split(v, ")")
			Dim aux2() as String
			
			if( ubound(aux) >= 1 )then
				aux2 = split(aux(0),"(")
				r = aux2(1)
			end if
			
			return r
		End Function
		
		
		Public Shared Function pegaIP() as String
			Dim r as String = ""
			
			Dim ipList as String = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
			
			if (Not string.IsNullOrEmpty(ipList))then
				r = ipList.Split(",")(0)
			else
				r = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
			end if
			
			return r
		End Function
		
		Public Shared Function pegaNavegador() as String
			Dim r as String = ""
			
			r = HttpContext.Current.Request.Browser.Browser &" v."& HttpContext.Current.Request.Browser.Version
			
			return r
		End Function
		
		
		Public Shared Function isLogged() as String
			Dim r as String = "false"
			
			Dim cnome as String = "A"& System.Configuration.ConfigurationManager.AppSettings("CIN")
			
			'Método que verifica cooki
			If Not HttpContext.Current.Request.Cookies(cnome) Is Nothing then
				r = "true"
			End If
			
			return r
		End Function
		
		
		
		Public Shared Function hasCart() as String
			Dim r as String = "false"
			
			Dim cnome as String = "C"& System.Configuration.ConfigurationManager.AppSettings("CIN")
			
			'Método que verifica cooki
			If Not HttpContext.Current.Request.Cookies(cnome) Is Nothing then
				r = "true"
			End If
			
			return r
		End Function
		
		
	End Class
	
End Namespace