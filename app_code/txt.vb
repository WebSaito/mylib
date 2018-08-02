Imports System
Imports System.IO

Namespace docs
	
	Public Class txt
		
		Public erros as String = ""
		
		Public Sub New()
		End Sub
		
		Public Function criar(byval parquivo as string, byval pconteudo as string) as boolean
			Dim r as boolean = false
			
			try
				if(File.Exists(parquivo))then
					erros &= "Já existe arquivo com este nome"
                else
				
					Dim sw as StreamWriter 
					sw = File.CreateText(parquivo)
					sw.WriteLine (pconteudo)
					sw.Close()
					
				end if
				
			catch ex as exception
				erros &= ex.toString()
			end try
			
			return r
		End Function
		
	End Class
	
End Namespace