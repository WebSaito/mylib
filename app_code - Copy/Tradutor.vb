Imports System
Imports System.Collection.Generic

Public Class Dicio
	
	Public Shared dicionario as Dictionary(OF String, String()) = new Dictionary(OF String, String())
	
	Public Sub New()
		carregarVocabulario()
	End Sub
	
	'----------------------
	'Tradutor
	Public Shared Sub carregarVocabulario()
		
		
		if dicionario.ContainsKey("projetos") then
		else
			'Carregando palavras
			dicionario.add( "projetos", new String(){"projects", "proyectos"} )
			dicionario.add( "setores", new String(){"sectors", "sectores"} )
		end if
		
	End Sub
	
	'Método para traduzir uma palavra
	Public Shared Function traduzir(byval strPortugues as String) as String
		Dim r as String = ""
	
		'carregarVocabulario()
		if dicionario.ContainsKey("projetos") then
		else
			carregarVocabulario()
		end if
		
		
		if System.Globalization.CultureInfo.CurrentCulture.Name = "pt-BR" then
			r = strPortugues
		else
			Dim coluna as Integer
			coluna = IIF(System.Globalization.CultureInfo.CurrentCulture.Name = "en-US", 0, 1 )
			
			if dicionario.ContainsKey(strPortugues) then
				r = dicionario(strPortugues)(coluna)
			else
				r = "palavra-não-encontrada"
			end if
			
		end if
		
		return r
	End Function
	'Fim Tradutor
	'----------------------
	
End Class