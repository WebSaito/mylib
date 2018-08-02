Imports System
Imports System.Web.UI.Page

Imports nsUrl
Imports System.Web.Script.Serialization

'// ====================================================================================
'//	Todas as páginas do Website devem estender esta classe
'//	
'//	Herança direta: 
'//	<%@ Page Language="VB" Inherits="dressviva.Core" debug="true" %>
'//
'//	Herança via classe:
'//	<%@ Page Language="VB" CodeFile="arquivo.vb" Inherits="[CLASSE]" debug="true" %>
'//	
'//	Public Class [CLASSE]
'//	Inherits dressviva.Core
'//	
'//	Sub Page_Load(Src As Object, E As EventArgs) 
'//		MyBase.Page_Load(Src, E)
'//	End Sub
'//	
'//	End Class
'// ====================================================================================

Namespace dressviva

	Public Class Core

	    Inherits spacelab.Core

	    Public meta_description As String
	    Public meta_title As String
		
		Public pre as String = System.Configuration.ConfigurationManager.AppSettings("prefixo")
		Public CIN as String = System.Configuration.ConfigurationManager.AppSettings("CIN")
		Public WSLK as String = System.Configuration.ConfigurationManager.AppSettings("WSLK")
		Public WSCK as String = System.Configuration.ConfigurationManager.AppSettings("WSCK")
		
	    Protected Sub Page_Load(byVal E As EventArgs)
			
			MyBase.OnLoad(E)
			
	    End Sub

        '// ================================================================
		'//	Redireciona para o Login se não logado
		'// ================================================================
        Public Function authArea(Optional ByVal redirectLoggedOut As String = "", Optional ByVal redirectLoggedIn As String = "", Optional ByVal acao As String = "")

            Dim url As String

            if Spacelab.Utils.isLogged() = "false" then

                url = redirectLoggedOut

                if url = "" then
                    url = Request.RawURL
                end if

                response.redirect(String.Format("/login?acao={0}&r={1}", acao, HttpUtility.UrlEncode(url)))

            else

                url = redirectLoggedIn

                if url <> "" then
                    response.redirect(url)
                end if

            end if

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
				return Nothing
			End Try

		End Function
		
        '// ================================================================
		'//	Calcula Parcelas
		'// ================================================================
		Public Function calculaParcelas(ByVal preco as String, ByVal maxPar as String, ByVal parMin as String) as String
			Dim r as String = ""
			
			Dim parcelas as Integer = 0
			Dim numeroFinal as Integer = 0
			Dim valorParcela as Double = 0
			Dim parar as boolean = false
			
			For parcelas = 1 to cInt(maxPar)
				
				if( Not parar )then
				
					if( (cDbl(preco)/parcelas) >= cDbl(parMin) )then
						valorParcela = (cDbl(preco)/parcelas)
						numeroFinal = parcelas
					else
						parar = true
					end if
					
				end if
				
			Next
			
			r = numeroFinal &"#"& valorParcela
			
			return r
		End Function

        '// ================================================================
		'//	Valida upload de imagem
		'// ================================================================
		public function validaImagem(byval file as HttpPostedFile) as String

			dim erro = "ok"

			' valida extensão 'gif, jpg, bmp ou png
			dim rgx as new Regex("\.(gif|jpg|bmp|png)$")
			if Not rgx.IsMatch(file.FileName.toString()) then
				erro = "extension"
			end if

			' valida tamanho - 2MB
			if file.ContentLength > 200000 then
				erro = "size"
			end if

			return erro

		end function

        '// ================================================================
		'//	Tratamento Título Pagamento
		'// ================================================================
        public function pagamentoTitulo(ByVal pagamento As String, Optional ByVal bandeira as String = "") As String

            select case(pagamento)
                case "cartao" : return "Cartão de Crédito"& IIF( bandeira <> "", " - "& bandeira, "" ) 
                case "pagseguro" : return "PagSeguro"& IIF( bandeira <> "", " - "& bandeira, "" ) 
                case else : return "Depósito / Transferência"
            end select

        end function

        Public Function noCache()
            HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false)
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            HttpContext.Current.Response.Cache.SetNoStore()
            Response.Cache.SetExpires(DateTime.Now)
            Response.Cache.SetValidUntilExpires(true)
        End Function

    End Class

End Namespace
