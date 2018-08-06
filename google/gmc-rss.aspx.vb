Imports System

Public Class pc
	Inherits System.Web.UI.Page
	
	Public oDB as new DB()
	
	Sub Page_Load(byval src as object, byval e as eventArgs)
		
		Response.ContentType = "text/xml"
		Response.ContentEncoding = System.Text.Encoding.UTF8
		
		loadData()
		
	End Sub
	
	'--//--
	
	Public Sub loadData()
		
		Dim r as String = ""
		Dim dt as new system.data.datatable()
		
		r &= "<?xml version="& chr(34) &"1.0"& chr(34) &"?>"& _
			 "<rss version="& chr(34) &"2.0"& chr(34) &" xmlns:g="& chr(34) &"http://base.google.com/ns/1.0"& chr(34) &">"& _
			 "<channel>"& _
			 "<title>Cuz da Terra Santa</title>"& _
			 "<link>http://cruzterrasanta.com.br</link>"& _ 
			 "<description><![CDATA[Artigos Religiosos, Medalhas de Santos, Terços de Santos, Jóias Religiosas]]</description>"
			 
			 'Produtos
			 Dim strSql as String = "select a.id, a.produto, a.resenha, a.preco, a.precoPromocional, a.prazoEntrega, a.dataModificacao "& _
			 " , b.linha, c.categoria, d.nome from crz_produtos as a inner join crz_linhas as b on a.idLinha=b.id inner join "& _
			 " crz_categorias as c on a.idCategoria=c.id inner join crz_santos as d on a.idSanto=d.id where a.disponivel=1 and a.excluido=0 order by a.idLinha, a.idCategoria, a.idSanto"
			 
			 dt = oDB.consulta(strSql)
			 
			 response.write(oDB.erros)
			 
			 for each x as system.data.datarow in dt.rows
			 
		r &= "<item>"& _
				"<title><![CDATA["& x("produto") &"]]</title>"& _
				"<link>http://cruzterrasanta.com.br/"& Spacelab.Utils.toLink(x("produto")) &"/"& x("id") &"/111/</link>"& _
				"<description><![CDATA["& x("resenha") &"]]</description>"& _
				"<g:id>"& x("id") &"</g:id>"& _
				"<g:image_link>"& rImagem(x("id")) &"</g:image_link>"& _
				"<g:availability>in stock</g:availability>"& _
				"<g:price>"& formataPreco(x("preco")) &" BRL</g:price>"& _
				"<g:condition>novo</g:condition>"& _
				"<g:google_product_category>97</g:google_product_category>"& _
				"<g:product_type><![CDATA["& x("linha") &" &gt; "& x("categoria") &"]]</g:product_type>"& _
			 "</item>" 
			 
			 next
			 dt.clear()
			 
		r &= "</channel>"& _ 
			 "</rss>"
		
		
		
		
		response.write(r)
		
	End Sub
	
	
	
	Private Function rImagem(byval idProduto as String) as String
		Dim r as String = ""
		
		Dim img as String = oDB.consulta("imagem", "select top 1 imagem from crz_produtos_imagens where idProduto="& idProduto &" order by principal desc ")
		
		if(img <> "")then
			r = "http://cruzterrasanta.com.br/_global/_ssf/ssf.aspx?d=/_upload/produtos/o/&arquivo="& img
		end if
		
		return r
	End Function
	
	
	Private Function formataPreco(byval preco as String) as String
		Dim r as String = ""
		
		r = formatNumber(preco, 2)
		r = r.replace(".", "")
		r = r.replace(",", ".")
		
		return r
	End Function
	
	
	
End Class