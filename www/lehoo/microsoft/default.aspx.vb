Imports System

Public Class pc
	Inherits System.Web.UI.Page
	
	Public cid as String = "557c4b21-3052-4ad5-b015-1e97eb8836ef"
	Public msAuth as String = "https://login.microsoftonline.com/common/oauth2/authorize?client_id="& cid &"&scope=openid+profile&response_type=id_token&redirect_uri=&nonce=134234234"
	
	''557c4b21-3052-4ad5-b015-1e97eb8836ef
	''Scope:live?
	
	Sub Page_Load(byval src as object, byval e as eventArgs)
		
		
		response.write(msAuth)
		
		
	End Sub
	
End Class