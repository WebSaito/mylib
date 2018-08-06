Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Collections
Imports Spacelab.Upload

<WebService(Namespace:="http://readymake1.hospedagemdesites.ws/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
public class readymake
	 
	 Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function excluiImagem(ByVal sFoto As String) As String
        Dim lfoto As String = System.Web.Hosting.HostingEnvironment.MapPath("~/_upload/"& sFoto)
        Dim resultado As String = ""
        
        Try
            File.Delete(lfoto)
            resultado = "ok"
        Catch ex As Exception
            resultado = ex.Message.ToString()
        End Try
        
        return resultado
    End Function

	<WebMethod()> _
	Public Function SaveFileRename(ByVal sFileName As String, ByVal bytFileByteArr As Byte(), ByVal sDir As String, ByVal sCopia As String, ByVal sTamanho As String, ByVal sPrefixo As String)
		Dim r as String = ""
		Dim oUp as new Upload()
		
		r = oUp.gravaImagem(sFileName, bytFileByteArr, sDir, sCopia, sTamanho, sPrefixo)
		
		return r
    End Function
	
	

End Class
