Imports System
Imports System.Web.Security
Imports System.Security.Cryptography

Namespace nCripto

	public class cCripto1
		
		'Criptografa string em MD5 e SHA1 (one way)
		public function criptoSpaceLab (byVal strTexto as String) as String
			Dim txt as String = "spa" & strTexto & "celab"
			txt = FormsAuthentication.HashPasswordForStoringInConfigFile(txt, "MD5")
			txt = StrReverse(txt)
			txt = FormsAuthentication.HashPasswordForStoringInConfigFile(txt, "SHA1")
			return txt
		end function
		
		'Gera uma senha aleatoria de 8 digitos
		public function spaGerarSenha () as String
			Dim newPass as String
			Dim t as DateTime = DateTime.now.toUniversalTime()
			newPass = FormsAuthentication.HashPasswordForStoringInConfigFile(t.toString(), "SHA1")
			newPass = Left (newPass, 8)
			return newPass
		end function
		
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
		
	end class
	
	'*
	'Futura implementação de metodo de criptografia proprio com codificação e decodificação
	
	
	
end namespace