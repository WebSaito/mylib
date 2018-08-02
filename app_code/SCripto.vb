Imports System
Imports System.Web.Security
Imports System.Security.Cryptography

Namespace S
	Public Class Seguranca
		
		Public Const ascMin As Double = 36
		'Public Const ascMin As Double = 40
		Public Const ascMax As Double = 255
		'Public Const ascMax As Double = 122
		
		Public Sub New()
		End Sub
		
		Public Function Codificar(ByVal texto As String, ByVal codigo As String) As String
			
			'declarando as variaveis utilizadas no método
			Dim resultado As String = ""
			Dim caractere() As String
			Dim valorChave As Double = 0
			Dim x, y , letra As Double
			Dim max, min As Double
			
			'inicializando variáveis
			x = 0
			y = 0
			max = ascMax
			min = ascMin
			
			For x = 0 to (len(codigo)-1)
				y = x
				
				if (y mod 2) <> 0 then
					valorChave = valorChave + (cDbl(asc(ucase(codigo(x)))) * x)
				else
					valorChave = valorChave - (cDbl(cDbl(asc(ucase(codigo(x)))) / 2) * x)
				end if
			Next
			
			For x = 0 to (len(texto)-1)
				y = x
				
				if y = 0 then
					resultado &= asc(texto(x))
					y = 1
				else
					resultado &= ","& asc(texto(x))
				end if
			Next
			
			caractere = split(resultado,",")
			resultado = ""                
			
			For x = 0 to ubound(caractere)
				y = x
				
				if caractere(x) <> 32 then
					if (y mod 2) = 0 then
						letra = caractere(x) + valorChave
					else
						letra = caractere(x) - valorChave
					end if
				 
					if letra < min then
						letra  = AjustaNumero(letra, "menor", min , max)
					end if
				 
					if letra > max then
						letra = AjustaNumero(letra, "maior", min, max )
					end if
				else
					letra = caractere(x)
				end if
					
				resultado &= chr(letra)
			Next
			
			Return resultado
		End Function
		
		Public Function DeCodificar(ByVal texto As String,ByVal codigo As String) As String
			
			'declarando as variaveis utilizadas no método
			Dim resultado As String = ""
			Dim caractere() As String
			Dim valorChave As Double = 0
			Dim x, y , letra As Double
			Dim max, min As Double
			
			'inicializando variáveis
			x = 0
			y = 0
			max = ascMax
			min = ascMin
			
			For x = 0 to (len(codigo)-1)
				y = x
				
				if (y mod 2) <> 0 then
					valorChave = valorChave + (cDbl(asc(ucase(codigo(x)))) * x)
				else
					valorChave = valorChave - (cDbl(cDbl(asc(ucase(codigo(x)))) / 2) * x)
				end if
			Next
			
			For x = 0 to (len(texto)-1)
				y = x
				
				if y = 0 then
					resultado &= asc(texto(x))
					y = 1
				else
					resultado &= ","& asc(texto(x))
				end if
			Next
			
			caractere = split(resultado,",")
			resultado = ""                
			
			For x = 0 to ubound(caractere)
				y = x
				
				if caractere(x) <> 32 then
					if (y mod 2) = 0 then
						letra = caractere(x) - valorChave
					else
						letra = caractere(x) + valorChave
					end if
					
					if letra < min then
						letra  = AjustaNumero(letra, "menor", min, max)
					end if
					
					if letra > max then
						letra = AjustaNumero(letra, "maior", min , max )
					end if
				else
					letra = caractere(x)
				end if
					
					resultado &= chr(letra)
			Next
			
			Return resultado
		End Function
		
		Public Function AjustaNumero(ByVal numero As Double, ByVal opcao As String , ByVal minimo As Double, ByVal maximo As Double) As Double
			if opcao = "maior" then
				numero = minimo + (numero - maximo)
				if numero >= maximo then
					numero = AjustaNumero(numero, "maior" , minimo, maximo)
				end if
			end if
			
			if opcao = "menor" then
				numero = (numero + maximo) - minimo
				if numero <= minimo then
					numero = AjustaNumero(numero, "menor" , minimo, maximo)
				end if
			end if
			
			Return numero
		End Function
		
		Public Function FiltraCaractere(ByVal numero As Integer) As Boolean
			Dim sFiltro() As Integer = {32,33,34,39}
			Dim x As Integer = 0
			Dim y As Integer = 0
			
			For x = 0 to ubound(sFiltro)
				if numero = sFiltro(x) then
					y = 1
				end if
			Next
			
			If y = 1 then
				return false
			Else
				return true
			End if
		End Function
		
		Public Function CodificaCE(ByVal caractere As String) As String
			
			'Declarando Variaveis
			Dim resultado As String = ""
			resultado = "#"& caractere
			
			return resultado
		End Function
		
		Public Function DecodificaCE(ByVal caractere As String) As String
			
			'Declarando Variaveis
			Dim resultado As String = ""
			resultado = chr(right(caractere,2))
			
			return resultado
		End Function
		
		'-----------------------------------------------------------------------
		
		Public Function CID(byval v as string, byval pchave as string) as string
			Dim r as String = ""
				
				Dim aux as String = Datetime.Now.toString("ssmmHHddMMyy") &":"& v &":"& Datetime.Now.toString("yyMMddHHmmss")
				r = HttpUtility.URLEncode(Codificar(aux, pchave))
				
			return r
		End Function
		
		Public Function RCID(byval v as string, byval pchave as string) as string
			Dim r as String = ""
				
				r = DeCodificar(HttpUtility.URLDEcode(v), pchave)
				Dim aux() as String = split(r,":")
				r = aux(1)
				
			return r
		End Function
		
		
		'------------------------------------------------------------------------
		
		
		
		Public Function CID6(byval v as string, byval pchave as string) as string
			Dim r as String = ""
				
				Dim aux as String = Datetime.Now.toString("ssmmHHddMMyy") &":"& v &":"& Datetime.Now.toString("yyMMddHHmmss")
				r = base64Encode(base64Encode(Codificar(aux, pchave)))
				
			return r
		End Function
		
		Public Function RCID6(byval v as string, byval pchave as string) as string
			Dim r as String = ""
				
				r = DeCodificar(base64Decode(base64Decode(v)), pchave)
				Dim aux() as String = split(r,":")
				r = aux(1)
				
			return r
		End Function
		
		
		'Codifica em base 64
		Public Function base64Encode(ByVal sData as String) As String
			
			Dim r As String = ""
			
			try
				
				Dim encData_byte() as byte 				 
				encData_byte = System.Text.Encoding.UTF8.GetBytes(sData) 
				 
				Dim encodedData as String = Convert.ToBase64String(encData_byte)
				r = encodedData
			 
			catch ex As Exception
				r = ex.ToString()
			end try
			
			return r
			
		End Function		
		
		
		'Decodifica base 64
		Public Function base64Decode(ByVal sData as String) As String
			
			Dim todecode_byte() as byte = Convert.FromBase64String(sData)
			 
			Dim r as String '= new String(decoded_char)
			r = System.Text.Encoding.UTF8.GetString(todecode_byte)
			
			return r
			
		End Function
		
		
		'Métodos mais antigos
		'----------------------------------------------------------------------------------------------
		public function spaCriptoSHA1 (byVal strTexto as String) as String
			Dim txt as String = "spa" & strTexto & "celab"
			txt = FormsAuthentication.HashPasswordForStoringInConfigFile(txt, "MD5")
			txt = StrReverse(txt)
			txt = FormsAuthentication.HashPasswordForStoringInConfigFile(txt, "SHA1")
			return txt
		end function
		
		public function spaGerarSenha () as String
			Dim newPass as String
			Dim t as DateTime = DateTime.now.toUniversalTime()
			newPass = FormsAuthentication.HashPasswordForStoringInConfigFile(t.toString(), "SHA1")
			newPass = Left (newPass, 5)
			return newPass
		end function
		
		
		
	End Class
End Namespace