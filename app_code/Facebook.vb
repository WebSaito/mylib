Imports System.Web.Script.Serialization
Imports RedesSociais

Namespace RedesSociais
	
	Public Class Facebook

		'// ================================================================
		'//	FACEBOOK
		'//	Classe para ser utilizada em conjunto com JS
		'// ================================================================

		'// ================================================================
		'//	Função de Exemplo, não deve ser chamada
		'// ================================================================
		Private Sub exemplo()

			Dim facebook As New Facebook()
			Dim accessTokenApp_data As FacebookData
			Dim accessTokenUser_data As FacebookData
			Dim userData As FacebookData
			Dim userDataJSON As Object

			' Dados que gostaria de receber (só vai trazer isso se no JS foi espeficado esses dados)
			facebook.user_data = "id,name,email,birthday,hometown"

			' Acess Token do usuário (após clicar no botão de login o FB retorna o Token)
			facebook.user_accessToken = ""

			' ID do App no Facebook (Encontrado no gerenciamento do App)
			facebook.app_ID = ""

			' App Secret (Encontrado no gerenciamento do App)
			facebook.app_Secret = ""

			' Faz requisição do Access Token do App, para depois fazer a requisição dos dados do usuário
			accessTokenApp_data = facebook.accessTokenApp_request()

			' Erro devido a parâmetros inválidos passados ao Facebook
			If accessTokenApp_data.erro Then
				'response.write(json_default("erro", accessTokenApp_data.erro_message))
				return
			End If

			' Verifica se é Access Token do usuário é válido
			accessTokenUser_data = facebook.accessTokenUser_check(accessTokenApp_data.result, facebook.user_accessToken)

			' Erro devido a parâmetros inválidos passados ao Facebook 
			If accessTokenUser_data.erro Then
				'response.write(json_default("erro", accessTokenUser_data.erro_message))
				return
			End If

			userData = facebook.userData(facebook.user_accessToken)
			
			' Erro ao recuperar os dados do usuário
			If userData.erro Then
				'response.write(json_default("erro", userData.erro_message))
				return
			End If	

			' FIM - Tudo OK
			' Agora pode trabalhar com os dados do usuário em JSON
			userDataJSON = New JavaScriptSerializer().Deserialize(Of Object)(userData.result)

			'response.write(String.Format("ID: {0}"), userDataJSON("id"))
			'response.write(String.Format("Name: {0}"), userDataJSON("name"))
			'response.write(String.Format("Email: {0}"), userDataJSON("email"))

		End Sub

		'// ================================================================
		'//	Insira aqui dados que deseja recuperar do usuário, exemplo:
		'//	id,name,email,birthday,hometown
		'// ================================================================
		Public user_data As String

		'// ================================================================
		'//	Dados recebidos do Facebook via JS
		'// ================================================================
		Public user_accessToken As String

		'// ================================================================
		'//	Dados via configuração do App
		'// ================================================================
		Public app_ID As String
		Public app_Secret As String

		'// ================================================================
		'//	Faz a requisição do Access Token do App
		'//	Retorna o Access Token
		'// ================================================================
		Public Function accessTokenApp_request() As FacebookData

			Dim url as String = "https://graph.facebook.com/oauth/access_token?client_id={client-id}&client_secret={client-secret}&grant_type=client_credentials"
			Dim facebookData As FacebookData

			' App ID
			url = url.Replace("{client-id}", app_ID)

			' App Secret
			url = url.Replace("{client-secret}", app_Secret)

			facebookData = apiRequest(url)

			if facebookData.erro = False then 
                Dim accessTokenAppJSON = New JavaScriptSerializer().Deserialize(Of Object)(facebookData.result)
				facebookData.result = accessTokenAppJSON("access_token")
			end if

			Return facebookData

		End Function

		'// ================================================================
		'//	Verifica se o Access Token do usuário é legítimo
		'// ================================================================
		Public Function accessTokenUser_check(ByVal app_accessToken As String, ByVal user_accessToken as String) As FacebookData

			Dim url = "https://graph.facebook.com/debug_token?input_token={token-to-inspect}&access_token={app-token-or-admin-token}"

			' Token retornado pelo JS, de quando o usuário loga no FB
			url = url.Replace("{token-to-inspect}", user_accessToken)

			' Token do App
			url = url.Replace("{app-token-or-admin-token}", app_accessToken)

			return apiRequest(url)

		End Function

		'// ================================================================
		'//	Recupera informações do usuário
		'// ================================================================
		Public Function userData(ByVal user_accessToken As String) As FacebookData

			Dim url = "https://graph.facebook.com/me?fields={user-data}&access_token={access-token}"

			url = url.Replace("{user-data}", user_data)
			url = url.Replace("{access-token}", user_accessToken)

			return apiRequest(url)

		End Function

		'// ================================================================
		'//	Utilidade para efetuar requisições no Facebook
		'// ================================================================
		Public Function apiRequest(ByVal url_request As String) As FacebookData

			Dim webClient As System.Net.WebClient = new System.Net.WebClient()
			Dim data() As Byte
			Dim html As String
			Dim queryParams As String = ""
			Dim apiURL as String = url_request
			Dim facebookData As new FacebookData()
			Dim facebookResponseJSON As Object

			Try

				'webClient.Headers.Add("Referer", referer)

				data = webClient.DownloadData(apiURL)

				webClient.Dispose()

				html = System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(data)

				Try
					facebookResponseJSON = New JavaScriptSerializer().Deserialize(Of Object)(html)

					if facebookResponseJSON("data")("is_valid") = False Then
						facebookData.erro = True
						facebookData.erro_id = facebookResponseJSON("data")("error")("code")
						facebookData.erro_message = facebookResponseJSON("data")("error")("message")
						facebookData.result = html
						facebookData.result_original = html
					else
						facebookData.erro = False
						facebookData.result = html
						facebookData.result_original = html
					end if

				Catch ex_json As Exception

					facebookData.erro = False
					facebookData.result = html
					facebookData.result_original = html

				End Try
				
				return facebookData

			Catch ex As Exception
				
				dim erro_id as String = ""

				If Regex.IsMatch(ex.Message, "Bad\sRequest") Then erro_id = "400"

				facebookData.erro = True
				facebookData.erro_id = erro_id
				facebookData.erro_message = ex.Message

				return facebookData

			End Try

		End Function

		'// ================================================================
		'//	UTILS	
		'// ================================================================

		Public Function formataJSON(ByVal propriedade, ByVal valor)
			Return """" & propriedade & """:""" & valor & """"
		End Function

		Public Function json_default(ByVal status as String, Optional ByVal message As String = "", Optional ByVal extra as String = "")

			Dim json As String = ""

			json += "{"
			json += formataJSON("status", status)
			json += "," + formataJSON("message", message)
			json += extra
			json += "}"

			Return json

		End Function

	End Class	

End Namespace

Namespace RedesSociais

	Public Class FacebookData

		Public erro As Boolean = False
		Public erro_id As String = ""
		Public erro_message As String = ""
		Public result As String = ""
		Public result_original As String = ""

		Public Function parseToJSON() As String

        	return new JavaScriptSerializer().Serialize(Me)

		End Function

	End Class

End Namespace
