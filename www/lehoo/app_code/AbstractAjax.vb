Imports System
Imports System.Web.UI.Page
Imports System.Web.Script.Serialization
Imports nsUrl
Imports nsEmail
Imports nsValidadores

'// ====================================================================================
'//	Todas os arquivos que recebem requisições Ajax devem extender esta classe
'//	
'//	Herança direta: 
'//	<%@ Page Language="VB" Inherits="abstract.AbstractAjax" debug="true" %>
'//
'//	Herança via classe:
'//	<%@ Page Language="VB" CodeFile="arquivo.vb" Inherits="[CLASSE]" debug="true" %>
'//	
'//	Public Class [CLASSE]
'//	Inherits abstract.Abstract
'//	
'//	Sub Page_Load(Src As Object, E As EventArgs) 
'//		
'//	End Sub
'//	
'//	End Class
'// ====================================================================================

Namespace abstract

	Public Class AbstractAjax

	    Inherits Abstract

	    Public obj_URL As clsUrl
	    Public obj_Validadores As clsValidadores
	    Public obj_Email As clsEmail
	    Public dom As String = System.Configuration.ConfigurationManager.AppSettings("dom")

	    Protected Sub Page_Load(byVal E As EventArgs)

			MyBase.OnLoad(E)

	    End Sub

	    '// ================================================================
	    '//	Valida CEP em formato simples: 01228-000
	    '// ================================================================
		Public Function valida_cep(ByVal cep As String) As Boolean

			' Valida se o total de caracteres é menor que 8 ou maior que 8
			if len(Regex.Replace(cep, "[^\d]", "")) < 8 or len(Regex.Replace(cep, "[^\d]", "")) > 8 then
				Return False
			' Valida o cep caso existam números repetidos seguidamente, exemplo: 00000-000
			else if Regex.IsMatch(Regex.Replace(cep, "[^\d]", ""), "0{8,}|1{8,}|2{8,}|3{8,}|4{8,}|5{8,}|6{8,}|7{8,}|8{8,}|9{8,}") then
				Return False
			else 
				Return True
			end if		

		End Function

		'// ================================================================
		'//	Utilidade para uso com o WebForms
		'// ================================================================
		public function ruleXMLWithServerSideId(ByVal id as String, Optional ByVal value As String = "")
			Return "<item server-side-id='" & id & "' value='" & value & "'></item>"
		end function

		'// ================================================================
		'//	Utilidade para uso com o WebForms
		'// ================================================================
		public function ruleFromXML(ByVal id)
			dim xml = ""
			xml &= "<item id='" & id & "'></item>"
			Return xml
		end function

		'// ================================================================
		'//	Toda requisição de parâmetros deve chamar essa função.
		'//	Evita que os caracteres venham com codificação diferente.
		'//	Para completo funcionamento deve ser usado a função
		'//	escape(valor) do JavaScript
		'// ================================================================
		Public Function replaceAjaxChars(ByVal valor As String) As String
			Return replace(replace(HttpUtility.UrlDecode(valor, System.Text.Encoding.Default()), "'", ""), "'", "")
		End Function

		'// ================================================================
		'//	URL Decode para caracteres escapados do JS
		'// ================================================================
		Public Function url_decode(ByVal valor As String) As String
			Return HttpUtility.UrlDecode(valor, System.Text.Encoding.Default())
		End Function

		'// ================================================================
		'//	Remove tags HTML do texto
		'//	Utilizar quando é enviado email para o cliente, impedindo
		'//	que seja utilizado algum código malicioso.
		'// ================================================================
		Public Function util_removeHTML(ByVal html as String)
			if html = Nothing then
				return ""
			else 
				Return Regex.Replace(html, "<.*?>", "")
			end if
		End Function

		'// ================================================================
		'//	Utilidade para gerar JSON no formato:
		'//	"propriedade":"valor"
		'// ================================================================
		Public Function formataJSON(ByVal propriedade As String, ByVal valor As String)
			Return """" & propriedade & """:""" & valor & """"
		End Function

		'// ================================================================
		'//	Utilidade para gerar JSON no formato:
		'//	{"status":"[STATUS]", "message":"[MESSAGE]"[EXTRA]}
		'//	Para o parâmetro extra pode ser passado qualquer coisa, 
		'//	incluindo outro JSON. Só não se esqueça de incluir a vírgula
		'//	antes do [EXTRA], exemplo:
		'//	{"status":"[STATUS]", "message":"[MESSAGE]", [EXTRA]}
		'// ================================================================
		Public Function json_default(ByVal status as String, Optional ByVal id as String = "", Optional ByVal message As String = "", Optional ByVal extra as String = "")

			Dim json As String = ""

			json += "{"
			json += formataJSON("status", status)
			json += "," + formataJSON("id", id)
			json += "," + formataJSON("message", message)
			json += extra
			json += "}"

			Return json

		End Function

		'// ================================================================
		'//	Tenta recuperar o valor de um objeto JSON
		'// ================================================================
		Public Function json_getValue(ByVal json As Object, ByVal propriedade As String)
			Try
				return json(propriedade)
			Catch ex As Exception
				return ""
			End Try
		End Function


		'// ================================================================
		'//	Retorna um JSON com os dados de endereço do dado
		'// ================================================================
		Public Function carrega_CEP(ByVal cep As String)

			Try
				Dim oUrl as new clsUrl()
				Dim json_string As String = ""

				json_string = oUrl.funcModulo("http://www.spacelab.com.br/_webservices/cep/cep.v2.aspx?acao=buscar&formato=json&cep="&cep, "", "http://"&System.Configuration.ConfigurationManager.AppSettings("dom"))

				Dim json_cep = New JavaScriptSerializer().Deserialize(Of Object)(json_string)
				
				return json_cep

			Catch ex As Exception
				return "erro carrega_CEP"
			End Try

		End Function
		
	End Class

End Namespace