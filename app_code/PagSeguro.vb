Imports System
Imports System.IO
Imports System.Data
Imports System.Net
Imports System.Collections.Specialized
Imports System.XML

Namespace PagSeguro
	
	Public Class PagSeguro2
		Inherits iPSData
		Implements IDisposable
		
		Public resultado as String = ""
		Public auxData as String = ""
		
		Public Sub New()
			'Inicializando tabela de itens
			
			psItems.columns.add("itemId", System.Type.GetType("System.String"))
			psItems.columns.add("itemDescription", System.Type.GetType("System.String"))
			psItems.columns.add("itemAmount", System.Type.GetType("System.String"))
			psItems.columns.add("itemQuantity", System.Type.GetType("System.String"))
			
		End Sub
		
		Public Sub Dispose() Implements System.IDisposable.Dispose
			'Limpeza de atributos aqui
			
		End Sub
		
		'Médoto getSession() - Checkout Transparente - Passo 1
		'A Sessão é utilizada no JS do Pag Seguro
		Public Function getSession() as boolean
			Dim r as Boolean = false
			Dim url as String = urlSessions(1)
			
			if modo = "teste" then
				url = urlSessions(0)
			end if
			
			Dim client as WebClient = new WebClient()
		    Dim parametros as new NameValueCollection()
			
			parametros.add("email", psEmail)
			parametros.add("token", psToken)
			
			Dim bytes() as byte = client.UploadValues(url, parametros)
			
			resultado = System.Text.Encoding.UTF8.GetString(bytes)
			
			'Desrinchando XML
			Dim doc as XmlDocument = new XmlDocument()
			doc.LoadXML(resultado)
			
			Dim raiz as XmlNodeList = doc.GetElementsByTagName("session")
				For each x as XMLNode in raiz
					psSessionID = x.Item("id").InnerText
				Next
			
			if psSessionID <> "" then
				r = true
			end if
			
			return r
		End Function
		
		
		'-------------------------------
		'getBoleto
		Public Function getBoleto() as boolean
			Dim r as boolean = false
			
			Dim url as String = urlTransactions(1)
			
			if modo = "teste" then
				url = urlTransactions(0)
			end if
			
		    Dim parametros as new NameValueCollection()
			Dim contador as Integer = 1
			
			parametros.add("email", psEmail)
			parametros.add("token", psToken)
			
			parametros.add("paymentMode", "default")
			parametros.add("paymentMethod", "boleto")
			parametros.add("receiverEmail", psEmail)
			parametros.add("currency", psMoeda)
			parametros.add("extraAmount", psExtraAmount)
			
			'-----------------------
			'Produtos
			For each x as system.data.datarow in psItems.rows
				
				parametros.add("itemId"& contador, x("itemId") )
				parametros.add("itemDescription"& contador, x("itemDescription") )
				parametros.add("itemAmount"& contador, x("itemAmount") )
				parametros.add("itemQuantity"& contador, x("itemQuantity") )
				
			Next
			
			'URL de notificacao
			parametros.add("notificationURL", notificationURL)
			
			'Numero do pedido
			parametros.add("reference", solNPedido)
			
			'Dados comprador
			parametros.add("senderName", solNome)
			parametros.add("senderCPF", senderCPF)
			parametros.add("senderAreaCode", solTDDD)
			parametros.add("senderPhone", solTNum)
			parametros.add("senderEmail", solEmail)
			parametros.add("senderHash", senderHash)
			
			'Dados entrega
			parametros.add("shippingAddressStreet", solEndereco)
			parametros.add("shippingAddressNumber", solNumero)
			parametros.add("shippingAddressComplement", solComplemento)
			parametros.add("shippingAddressDistrict", solBairro)
			parametros.add("shippingAddressPostalCode", solCep)
			parametros.add("shippingAddressCity", solCidade)
			parametros.add("shippingAddressState", solUF)
			parametros.add("shippingAddressCountry", solPais)
			parametros.add("shippingType", solEntrega)
			parametros.add("shippingCost", psFrete)
			
			Dim parsa = HttpUtility.ParseQueryString(parametros.toString())
			
			Dim dadosEnviados as String = ""
			
			for each x as string in parametros
			
				dadosEnviados &= IIF(dadosEnviados = "", "", "&") &  x &"="& parametros(x)
			
			next
			
			auxData = dadosEnviados
			
			resultado = POST(url, dadosEnviados)
			
			'-----------------
			'Desrinchando XML
			Dim doc as XmlDocument = new XmlDocument()
			doc.LoadXML(resultado)
			 
			try
				
				Dim elemento As XmlElement = doc.DocumentElement
				
				consDate 		= elemento.SelectSingleNode("date").InnerText
				consLastEDate 	= elemento.SelectSingleNode("lastEventDate").innerText
				consCode 		= elemento.SelectSingleNode("code").InnerText
				consRef 		= elemento.SelectSingleNode("reference").InnerText
				consType 		= elemento.SelectSingleNode("type").InnerText
				consStatus 		= elemento.SelectSingleNode("status").InnerText
				consPMType 		= elemento.SelectSingleNode("paymentMethod").item("type").innerText
				consPMCode 		= elemento.SelectSingleNode("paymentMethod").item("code").innerText
				paymentLink 	= elemento.SelectSingleNode("paymentLink").innerText
				
				r = true
				
			catch exXML as Exception
				
				Dim listaErros as XmlNodeList = doc.GetElementsByTagName("errors")
				
				For each x as XMLNode in listaErros
					erros &= "<hr/>CÓDIGO: "& x.item("error").item("code").innerText &" - MENSAGEM: "& x.item("error").item("message").innerText
				Next
				
			end try
			
			
			return r
		End Function
		'fim getBoleto
		'-------------------------------
		
		
		
		
		
		
		
		'-------------------------------
		'getDebito
		Public Function getDebito() as boolean
			Dim r as Boolean = false
			
			Dim url as String = urlTransactions(1)
			
			if modo = "teste" then
				url = urlTransactions(0)
			end if
			
		    Dim parametros as new NameValueCollection()
			Dim contador as Integer = 1
			
			parametros.add("email", psEmail)
			parametros.add("token", psToken)
			
			parametros.add("paymentMode", "default")
			parametros.add("paymentMethod", "eft")
			
			parametros.add("bankName", bankName)
			parametros.add("receiverEmail", psEmail)
			parametros.add("currency", "BRL")
			parametros.add("extraAmount", psExtraAmount)
			
			'-----------------------
			'Produtos
			For each x as system.data.datarow in psItems.rows
				
				parametros.add("itemId"& contador, x("itemId") )
				parametros.add("itemDescription"& contador, x("itemDescription") )
				parametros.add("itemAmount"& contador, x("itemAmount") )
				parametros.add("itemQuantity"& contador, x("itemQuantity") )
				
			Next
			
			'URL de notificacao
			parametros.add("notificationURL", notificationURL)
			
			'Numero do pedido
			parametros.add("reference", solNPedido)
			
			'Dados comprador
			parametros.add("senderName", solNome)
			parametros.add("senderCPF", senderCPF)
			parametros.add("senderAreaCode", solTDDD)
			parametros.add("senderPhone", solTNum)
			parametros.add("senderEmail", solEmail)
			parametros.add("senderHash", senderHash)
			
			'Dados entrega
			parametros.add("shippingAddressStreet", solEndereco)
			parametros.add("shippingAddressNumber", solNumero)
			parametros.add("shippingAddressComplement", solComplemento)
			parametros.add("shippingAddressDistrict", solBairro)
			parametros.add("shippingAddressPostalCode", solCep)
			parametros.add("shippingAddressCity", solCidade)
			parametros.add("shippingAddressState", solUF)
			parametros.add("shippingAddressCountry", solPais)
			parametros.add("shippingType", solEntrega)
			parametros.add("shippingCost", psFrete)
			
			Dim parsa = HttpUtility.ParseQueryString(parametros.toString())
			
			Dim dadosEnviados as String = ""
			
			for each x as string in parametros
			
				dadosEnviados &= IIF(dadosEnviados = "", "", "&") &  x &"="& parametros(x)
			
			next
			
			auxData = dadosEnviados
			
			resultado = POST(url, dadosEnviados)
			
			'-----------------
			'Desrinchando XML
			Dim doc as XmlDocument = new XmlDocument()
			doc.LoadXML(resultado)
			
			try
				
				Dim elemento As XmlElement = doc.DocumentElement
				
				consDate 		= elemento.SelectSingleNode("date").InnerText
				consLastEDate 	= elemento.SelectSingleNode("lastEventDate").innerText
				consCode 		= elemento.SelectSingleNode("code").InnerText
				consRef 		= elemento.SelectSingleNode("reference").InnerText
				consType 		= elemento.SelectSingleNode("type").InnerText
				consStatus 		= elemento.SelectSingleNode("status").InnerText
				consPMType 		= elemento.SelectSingleNode("paymentMethod").item("type").innerText
				consPMCode 		= elemento.SelectSingleNode("paymentMethod").item("code").innerText
				paymentLink 	= elemento.SelectSingleNode("paymentLink").innerText
				
				r = true
				
			catch exXML as Exception
				
				Dim listaErros as XmlNodeList = doc.GetElementsByTagName("errors")
				
				For each x as XMLNode in listaErros
					erros &= "<hr/>CÓDIGO: "& x.item("error").item("code").innerText &" - MENSAGEM: "& x.item("error").item("message").innerText
				Next
				
			end try
			
			
			return r
		End Function
		'fim getDebito
		'-------------------------------
		
		
		
		
		
		'-------------------------------
		'getCC
		Public Function getCC()
			Dim r as Boolean = false
			
			Dim url as String = urlTransactions(1)
			
			if modo = "teste" then
				url = urlTransactions(0)
			end if
			
		    Dim parametros as new NameValueCollection()
			Dim contador as Integer = 1
			
			parametros.add("email", psEmail)
			parametros.add("token", psToken)
			
			parametros.add("paymentMode", "default")
			parametros.add("paymentMethod", "creditCard")
			
			parametros.add("receiverEmail", psEmail)
			parametros.add("currency", "BRL")
			parametros.add("extraAmount", psExtraAmount)
			
			'-----------------------
			'Produtos
			For each x as system.data.datarow in psItems.rows
				
				parametros.add("itemId"& contador, x("itemId") )
				parametros.add("itemDescription"& contador, x("itemDescription") )
				parametros.add("itemAmount"& contador, x("itemAmount") )
				parametros.add("itemQuantity"& contador, x("itemQuantity") )
				
			Next
			
			'URL de notificacao
			parametros.add("notificationURL", notificationURL)
			
			'Numero do pedido
			parametros.add("reference", solNPedido)
			
			'Dados comprador
			parametros.add("senderName", solNome)
			parametros.add("senderCPF", senderCPF)
			parametros.add("senderAreaCode", solTDDD)
			parametros.add("senderPhone", solTNum)
			parametros.add("senderEmail", solEmail)
			parametros.add("senderHash", senderHash)
			
			'Dados entrega
			parametros.add("shippingAddressStreet", solEndereco)
			parametros.add("shippingAddressNumber", solNumero)
			parametros.add("shippingAddressComplement", solComplemento)
			parametros.add("shippingAddressDistrict", solBairro)
			parametros.add("shippingAddressPostalCode", solCep)
			parametros.add("shippingAddressCity", solCidade)
			parametros.add("shippingAddressState", solUF)
			parametros.add("shippingAddressCountry", solPais)
			parametros.add("shippingType", solEntrega)
			parametros.add("shippingCost", psFrete)
			
			'Dados CC
			parametros.add("creditCardToken", creditCardToken)
			parametros.add("installmentQuantity", installmentQuantity)
			parametros.add("installmentValue", installmentValue)
			parametros.add("noInterestInstallmentQuantity", noInterestInstallmentQuantity)
			parametros.add("creditCardHolderName", creditCardHolderName)
			parametros.add("creditCardHolderCPF", creditCardHolderCPF)
			parametros.add("creditCardHolderBirthDate", creditCardHolderBirthDate)
			parametros.add("creditCardHolderAreaCode", creditCardHolderAreaCode)
			parametros.add("creditCardHolderPhone", creditCardHolderPhone)
			parametros.add("billingAddressStreet", billingAddressStreet)
			parametros.add("billingAddressNumber", billingAddressNumber)
			parametros.add("billingAddressComplement", billingAddressComplement)
			parametros.add("billingAddressDistrict", billingAddressDistrict)
			parametros.add("billingAddressPostalCode", billingAddressPostalCode)
			parametros.add("billingAddressCity", billingAddressCity)
			parametros.add("billingAddressState", billingAddressState)
			parametros.add("billingAddressCountry", billingAddressCountry)
			
			
			Dim parsa = HttpUtility.ParseQueryString(parametros.toString())
			
			Dim dadosEnviados as String = ""
			
			for each x as string in parametros
			
				dadosEnviados &= IIF(dadosEnviados = "", "", "&") &  x &"="& parametros(x)
			
			next
			
			auxData = dadosEnviados
			
			resultado = POST(url, dadosEnviados)
			
			'-----------------
			'Desrinchando XML
			Dim doc as XmlDocument = new XmlDocument()
			doc.LoadXML(resultado)
			
			try
				
				Dim elemento As XmlElement = doc.DocumentElement
				
				consDate 		= elemento.SelectSingleNode("date").InnerText
				consLastEDate 	= elemento.SelectSingleNode("lastEventDate").innerText
				consCode 		= elemento.SelectSingleNode("code").InnerText
				consRef 		= elemento.SelectSingleNode("reference").InnerText
				consType 		= elemento.SelectSingleNode("type").InnerText
				consStatus 		= elemento.SelectSingleNode("status").InnerText
				consPMType 		= elemento.SelectSingleNode("paymentMethod").item("type").innerText
				consPMCode 		= elemento.SelectSingleNode("paymentMethod").item("code").innerText
				
				r = true
				
			catch exXML as Exception
				
				Dim listaErros as XmlNodeList = doc.GetElementsByTagName("errors")
				
				For each x as XMLNode in listaErros
					erros &= "<hr/>CÓDIGO: "& x.item("error").item("code").innerText &" - MENSAGEM: "& x.item("error").item("message").innerText
				Next
				
			end try
			
			
			return r
		End Function
		'fim getCC
		'-------------------------------
		
		
		
		
		
		
		
		
		'-------------------------------
		'getTransactionByRef
		Public Function getTransactionByRef(byval pRef as string, Optional ByVal pDataInicial As String = "", Optional ByVal pDataFinal As String = "" ) as boolean
			Dim r as Boolean = false
			
			if pRef <> "" then
			
				Dim url as String = urlTransactions(1)
				
				if modo = "teste" then
					url = urlTransactions(0)
				end if
				
				Dim parametros as new NameValueCollection()
				Dim contador as Integer = 1
				
				parametros.add("email", psEmail)
				parametros.add("token", psToken)
				parametros.add("reference", pRef)
				
				'Parametros opcionais
				'Fazendo o tratamento dos parametros recebidos
				if IsDate(pDataInicial) then
					
					Dim dtInicial as Datetime
					Dim dtFinal as Datetime
					
					'Inicializando valor em data inicial
					dtInicial = pDataInicial
					
					'Verificando se data final existe e ou foi enviada corretamente, senao assumira a data de hoje
					if IsDate(pDataFinal) then
						dtFinal = pDataFinal
					else
						dtFinal = Datetime.Now()
					end if
					
					parametros.add("initialDate", dtInicial.toString("yyyy-MM-dd'T'HH:mm"))
					parametros.add("finalDate", dtFinal.toString("yyyy-MM-dd'T'HH:mm"))
					
				end if
				
				'Fazendo o post
				Dim dadosEnviados as String = ""
			
				for each x as string in parametros
				
					dadosEnviados &= IIF(dadosEnviados = "", "", "&") &  x &"="& parametros(x)
				
				next
				'AuxData apenas para verificar os dados que estao sendo postados
				auxData = dadosEnviados
				resultado = GETDATA(url, dadosEnviados)
				
				
				'-----------------
				'Desrinchando XML
				Dim doc as XmlDocument = new XmlDocument()
				doc.LoadXML(resultado)
				
				try
					
					Dim elementos as XmlNodeList = doc.GetElementsByTagName("transactions")
					
					For Each x as XMLNode in  elementos
						
						'	if String.IsNullOrEmpty(trim(x.Item("id").InnerText)) then
						transactions.add(new Transaction( x.item("transaction").item("date").InnerText, _ 
														  x.item("transaction").item("reference").InnerText, _ 
														  x.item("transaction").item("code").InnerText, _ 
														  x.item("transaction").item("type").InnerText, _ 
														  x.item("transaction").item("status").InnerText, _ 
														  x.item("transaction").item("paymentMethod").item("type").InnerText, _ 
														  x.item("transaction").item("grossAmount").InnerText, _ 
														  x.item("transaction").item("discountAmount").InnerText, _ 
														  x.item("transaction").item("feeAmount").InnerText, _ 
														  x.item("transaction").item("netAmount").InnerText, _ 
														  x.item("transaction").item("extraAmount").InnerText))
						
						
					Next
					
					r = true
					
				catch exXML as Exception
					
					Dim listaErros as XmlNodeList = doc.GetElementsByTagName("errors")
					
					For each x as XMLNode in listaErros
						erros &= "<hr/>CÓDIGO: "& x.item("error").item("code").innerText &" - MENSAGEM: "& x.item("error").item("message").innerText
					Next
					
				end try
				
				
			end if
			
			return r
		End Function
		'fim getTransactionByRef
		'-----------------------------------
		
		
		
		
		'-----------------------------------
		'getTransactionByCode
		Public Function getTransactionByCode(byval pCode as String) as boolean
			Dim r as boolean = false
			
			if( pCode <> "" )then
				
				Dim url as String = urlTransactionsV3(1)
				
				if modo = "teste" then
					url = urlTransactionsV3(0)
				end if
				
				Dim parametros as new NameValueCollection()
				Dim contador as Integer = 1
				
				parametros.add("email", psEmail)
				parametros.add("token", psToken)
				
				
				'Fazendo o post
				Dim dadosEnviados as String = ""
			
				for each x as string in parametros
				
					dadosEnviados &= IIF(dadosEnviados = "", "", "&") &  x &"="& parametros(x)
				
				next
				'AuxData apenas para verificar os dados que estao sendo postados
				auxData = dadosEnviados
				resultado = GETDATA(url & pCode &"/", dadosEnviados)
				
				
				'-----------------
				'Desrinchando XML
				Dim doc as XmlDocument = new XmlDocument()
				doc.LoadXML(resultado)
				'
				try
				'	
					Dim elemento As XmlElement = doc.DocumentElement
					
					consDate 				= elemento.SelectSingleNode("date").InnerText
					consLastEDate 			= elemento.SelectSingleNode("lastEventDate").innerText
					consCode 				= elemento.SelectSingleNode("code").InnerText
					consRef 				= elemento.SelectSingleNode("reference").InnerText
					consType 				= elemento.SelectSingleNode("type").InnerText
					consStatus 				= elemento.SelectSingleNode("status").InnerText
					consPMType 				= elemento.SelectSingleNode("paymentMethod").item("type").innerText
					consPMCode 				= elemento.SelectSingleNode("paymentMethod").item("code").innerText
					consGrossAmout 			= elemento.SelectSingleNode("grossAmount").InnerText 'Valor Bruto da transação
					consDiscountAmount 		= elemento.SelectSingleNode("discountAmount").InnerText 'Valor do desconto dado
					consfeeAmount 			= elemento.SelectSingleNode("creditorFees").item("intermediationFeeAmount").InnerText 'Valor das taxas cobradas
					consNetAmount 			= elemento.SelectSingleNode("netAmount").InnerText'Valor liquido da  transação
					consExtraAmount			= elemento.SelectSingleNode("extraAmount").InnerText 'Valor extra que foi somado ou subtraido. seja um desconto ou seja um valor a mais
					consInstallmentCount	= elemento.SelectSingleNode("installmentCount").InnerText'Numero de parcelas
					
				'	
				'*****
				'Dados Abaixo não estão sendo capturados ainda
				'
				'<?xml version="1.0" encoding="UTF-8"?>
				'<transaction>
				'   
				'   <creditorFees>
				'	  <installmentFeeAmount>0.00</installmentFeeAmount>
				'	  <intermediationRateAmount>0.40</intermediationRateAmount>
				'	  <intermediationFeeAmount>3.55</intermediationFeeAmount>
				'   </creditorFees>
				'   <netAmount>85.05</netAmount>
				'   <extraAmount>0.00</extraAmount>
				'   <installmentCount>1</installmentCount>
				'   <itemCount>1</itemCount>
				'   <items>
				'	  <item>
				'		 <id>001</id>
				'		 <description>Tenis LED - Azul Cobalto - 27</description>
				'		 <quantity>1</quantity>
				'		 <amount>89.00</amount>
				'	  </item>
				'   </items>
				'   <sender>
				'	  <name>Teste Spacelab</name>
				'	  <email>c02260557074396464917@sandbox.pagseguro.com.br</email>
				'	  <phone>
				'		 <areaCode>11</areaCode>
				'		 <number>33110862</number>
				'	  </phone>
				'	  <documents>
				'		 <document>
				'			<type>CPF</type>
				'			<value>03283429634</value>
				'		 </document>
				'	  </documents>
				'   </sender>
				'   <shipping>
				'	  <address>
				'		 <street>Av. Angelica</street>
				'		 <number>688</number>
				'		 <complement />
				'		 <district>Higienopolis</district>
				'		 <city>Sao Paulo</city>
				'		 <state>SP</state>
				'		 <country>BRA</country>
				'		 <postalCode>01228000</postalCode>
				'	  </address>
				'	  <type>1</type>
				'	  <cost>0.00</cost>
				'   </shipping>
				'   <gatewaySystem>
				'	  <type>cielo</type>
				'	  <rawCode xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true" />
				'	  <rawMessage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true" />
				'	  <normalizedCode xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true" />
				'	  <normalizedMessage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:nil="true" />
				'	  <authorizationCode>0</authorizationCode>
				'	  <nsu>0</nsu>
				'	  <tid>0</tid>
				'	  <establishmentCode>1056784170</establishmentCode>
				'	  <acquirerName>CIELO</acquirerName>
				'   </gatewaySystem>
				'   <primaryReceiver>
				'	  <publicKey>PUB598E6FA8D89D48419A5067E037337D3A</publicKey>
				'   </primaryReceiver>
				'</transaction>
					
					r = true
					
				catch exXML as Exception
					
					Dim listaErros as XmlNodeList = doc.GetElementsByTagName("errors")
					
					For each x as XMLNode in listaErros
						erros &= "<hr/>CÓDIGO: "& x.item("error").item("code").innerText &" - MENSAGEM: "& x.item("error").item("message").innerText
					Next
					
				end try
				
				
				
			end if
			
			return r
		End Function
		'fim getTransactionByCode
		'-----------------------------------
		
		
		
		
		
		
		
		'========================================================================================================================================
		'========================================================================================================================================
		
		
		
		
		
		
		'-----------------------------------
		'Métodos auxiliares
		Public Function adicionaItem(ByVal id As String, ByVal produto As String, ByVal preco As String, ByVal quantidade As String) As boolean
			Dim r as Boolean = false
			
			Try
				
				Dim linha As DataRow
				
				linha = psItems.NewRow()
				linha("itemId") = id
				linha("itemDescription") = produto
				linha("itemAmount") = preco
				linha("itemQuantity") = quantidade
				psItems.Rows.Add(linha)
				
			Catch ex As Exception
				erros &= "adicionaItem:"& ex.ToString() &"<br/><br/>"
			End Try
			
			return r
		End Function
		
		
		
		
		
		'----------------------------------------------------------
		'Método que faz um web POST
		Public Function POST(byval url as String, ByVal dados as String, Optional ByVal metodo as String = "POST") As String
			Dim r as String = ""
			
			Dim wr As WebRequest = WebRequest.Create(url)
			
			Select Case(metodo)
				Case "POST" : wr.Method = WebRequestMethods.Http.Post'"POST"'"PUT"
				Case "PUT" : wr.Method = "PUT"
			End Select
			
			'wr.Credentials = new NetworkCredential(psEmail, psToken)
			
			Dim postData As String = dados
			Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postData)
			
			wr.ContentType = "application/x-www-form-urlencoded; charset=ISO-8859-1"'"application/json"
			wr.ContentLength = byteArray.Length
			
			Dim dataStream As Stream = wr.GetRequestStream()
			
			dataStream.Write(byteArray, 0, byteArray.Length)
			
			dataStream.Close()
			Try
				
				Dim rs As WebResponse = wr.GetResponse()
				dataStream = rs.GetResponseStream()
				
				Dim reader As New StreamReader(dataStream)
				
				Dim responseFromServer As String = reader.ReadToEnd()
				
				r = responseFromServer
				
			Catch ex as WebException
				
				Dim res = ex.Response
				Dim sr as StreamReader = new StreamReader(res.GetResponseStream())
				Dim returnvalue as String = sr.ReadToEnd()
				r = returnvalue
				
			End Try
			
			return r
		End Function
		'----------------------------------------------------------
		
		
		
		
		'----------------------------------------------------------
		'método que faz uma chamada GET com parametros
		Public Function GETDATA(Byval url as String, ByVal dados as String) As String
			Dim r as String = ""
			
			Dim client as WebClient = new WebClient()
			Try
				r = client.DownloadString(url &"?"& dados)
			Catch ex as WebException
				
				Dim res = ex.Response
				Dim sr as StreamReader = new StreamReader(res.GetResponseStream())
				Dim returnvalue as String = sr.ReadToEnd()
				r = returnvalue
				
			End Try
			
			return r
		End Function
		'----------------------------------------------------------
		
	End Class
	
	
	
	
	
	Public Class iPSData
		Implements IDisposable
		
		'ConfiguraçÃµes
		Public modo As String = "teste" 'teste ou producao 'default é teste
		
		'Atributos Monetarios
		'---
		
		'Atributos PagSeguro
		Public psEmail As String = ""
		Public psToken As String = ""
		Public psMoeda As String = "BRL"		
		Public psItems as System.Data.Datatable = new System.Data.Datatable()
		Public notificationURL as String = ""
		Public psSessionID As String = ""
		Public paymentLink as String = ""
		Public senderHash as String = ""
		Public bankName as String = ""
		Public psExtraAmount as String = "0.00"
		
		'Atributos do solicitante
		Public senderCPF as String = ""
		Public solNPedido As String = "" 'Numero do pedido
		Public solNome As String = "" ' Nome
		Public solTDDD As String = "" ' DDD
		Public solTNum As String = "" ' Numero do telefone
		Public solEmail As String = "" ' Email
		Public solEntrega As String = "1" ' Tipo de entrega 1- Encomenda Normal, 2- Sedex, 3- Não definida
		Public solEndereco As String = "" ' Endereco
		Public solNumero As String = "" ' Numero do endereco
		Public solComplemento As String = "" ' Complemento
		Public solBairro As String = "" ' Bairro
		Public solCep As String = "" ' Cep
		Public solCidade As String = "" ' Cidade
		Public solUF As String = "" ' Estado
		Public solPais As String = "BRA" 'País
		Public solDesconto As String = "" 'Desconto formato decimal , usando ponto (.)  para separar as casas decimais. Ex.:  12,50 -> 12.50
		
		'Atributos do Cartão de Crédito
		Public creditCardToken as String = ""
		Public installmentQuantity as String = ""
		Public installmentValue as String = "0.00"
		Public noInterestInstallmentQuantity as String = "1"
		Public creditCardHolderName as String = ""
		Public creditCardHolderCPF as String = ""
		Public creditCardHolderBirthDate as String = ""
		Public creditCardHolderAreaCode as String = ""
		Public creditCardHolderPhone as String = ""
		Public billingAddressStreet as String = ""
		Public billingAddressNumber as String = ""
		Public billingAddressComplement as String = ""
		Public billingAddressDistrict as String = ""
		Public billingAddressPostalCode as String = ""
		Public billingAddressCity as String = ""
		Public billingAddressState as String = ""
		Public billingAddressCountry as String = ""
		
		'Atributos de Depuração
		Public erros As String = ""
		
		'Atributos de entrega
		Public psFrete As String = "0.00"
		
		'Atributos de consulta
		Public consDate As String = ""
		Public consLastEDate As String = ""
		Public consCode As String = "" 'Codigo da transação (ID unico da transação no Pag-Seguro)
		Public consRef As String = "" 'Referencia é o ID de pedido enviado pelo seu sistema 
		Public consType As String = "" 'Tipo da trasação no caso, pagamento 1
		Public consStatus As String = "" 'Status descrito abaixo:
		'1	Aguardando pagamento: o comprador iniciou a transação, mas até o momento o PagSeguro não recebeu nenhuma informação sobre o pagamento.
		'2	Em análise: o comprador optou por pagar com um cartão de crédito e o PagSeguro está analisando o risco da transação.
		'3	Paga: a transação foi paga pelo comprador e o PagSeguro já recebeu uma confirmação da instituição financeira responsável pelo processamento.
		'4	Disponível: a transação foi paga e chegou ao final de seu prazo de liberação sem ter sido retornada e sem que haja nenhuma disputa aberta.
		'5	Em disputa: o comprador, dentro do prazo de liberação da transação, abriu uma disputa.
		'6	Devolvida: o valor da transação foi devolvido para o comprador.
		'7	Cancelada: a transação foi cancelada sem ter sido finalizada.
		Public consCancellationSource As String = "" 'Origem do cancelamento : INTERNAL -> PagSeguro  |   EXTERNAL -> Instituição Financeira
		Public consPMType As String = ""' PaymentMethod: 'Forma de pagamento
		'1	Cartão de crédito: o comprador escolheu pagar a transação com cartão de crédito.
		'2	Boleto: o comprador optou por pagar com um boleto bancário.
		'3	Débito online (TEF): o comprador optou por pagar a transação com débito online de algum dos bancos conveniados.
		'4	Saldo PagSeguro: o comprador optou por pagar a transação utilizando o saldo de sua conta PagSeguro.
		'5	Oi Paggo: o comprador escolheu pagar sua transação através de seu celular Oi.
		Public consPMCode As String = ""' PaymentCode: 'Codigo que identifica o meio de pagamento
		'101	Cartão de crédito Visa.
		'102	Cartão de crédito MasterCard.
		'103	Cartão de crédito American Express.
		'104	Cartão de crédito Diners.
		'105	Cartão de crédito Hipercard.
		'106	Cartão de crédito Aura.
		'107	Cartão de crédito Elo.
		'108	Cartão de crédito PLENOCard.
		'109	Cartão de crédito PersonalCard.
		'110	Cartão de crédito JCB.
		'111	Cartão de crédito Discover.
		'112	Cartão de crédito BrasilCard.
		'113	Cartão de crédito FORTBRASIL.
		'201	Boleto Bradesco. *
		'202	Boleto Santander.
		'301	Débito online Bradesco.
		'302	Débito online ItaÃº.
		'303	Débito online Unibanco. *
		'304	Débito online Banco do Brasil.
		'305	Débito online Banco Real. *
		'306	Débito online Banrisul.
		'307	Débito online HSBC.
		'401	Saldo PagSeguro.
		'501	Oi Paggo.
		Public consGrossAmout As String = "" 'Valor Bruto da transação
		Public consDiscountAmount As String = "" 'Valor do desconto dado
		Public consfeeAmount As String = "" 'Valor das taxas cobradas
		Public consNetAmount As String = "" 'Valor liquido da  transação
		Public consEscrowEndDate As String = "" 'Data de crédito.Formato: YYYY-MM-DDThh:mm:ss.sTZD,
		Public consExtraAmount As String = "" 'Valor extra que foi somado ou subtraido. seja um desconto ou seja um valor a mais
		Public consInstallmentCount As String = ""'Numero de parcelas
		Public consItemCount As String = "" ' Quantidade de itens
		Public consItems() As String ' Array de itens bruto com dados contatenados com |  (pipe)
		Public consSenEmail As String = "" ' E-mail do comprador
		Public consSenName As string = "" ' Nome do comprador
		Public consSenPhoneAreaCode As String = "" 'Codigo de area do telefone do comprador
		Public consSenPhoneNumber As String = "" ' Numero do telefone do comprador
		Public consEntType As String = "" 'Tipo de entrega:
		'1	Encomenda normal (PAC).
		'2	SEDEX.
		'3	Tipo de frete não especificado.
		Public consEntCost As String = "" 'Custo do frete
		Public consEntCountry As String = "" 'Pais de entrega
		Public consEntState As String = "" 'Estado de entrega
		Public consEntCity As String = "" 'Cidade de entrega
		Public consEntPostalCode As String = "" 'Codigo postal de entrega
		Public consEntDistrict As String = "" 'Bairro de entrega
		Public consEntStreet As String = "" 'Rua de entrega
		Public consEntNumber As String = "" 'Numero do endereco de entrega
		Public consEntComplement As String = "" 'Complemento do endereço de entrega
		
		'URLS - Sendo 0 = Sandbox e 1 = Producao
		'--URLs para método Checkout Transparente
		Public urlSessions() as String = {"https://ws.sandbox.pagseguro.uol.com.br/v2/sessions", 
										  "https://ws.pagseguro.uol.com.br/v2/sessions"}
		
		Public urlJS() as String = {"https://stc.sandbox.pagseguro.uol.com.br/pagseguro/api/v2/checkout/pagseguro.directpayment.js",
									"https://stc.pagseguro.uol.com.br/pagseguro/api/v2/checkout/pagseguro.directpayment.js"}
		
		'--URLs para método entrar no ambiente do PAGSEGURO
		Public urlCheckout() as String = {"https://ws.sandbox.pagseguro.uol.com.br/v2/checkout",
										  "https://ws.pagseguro.uol.com.br/v2/checkout"}
		
		Public urlPagamento() as String = {"https://sandbox.pagseguro.uol.com.br/v2/checkout/payment.html?code=", 
		                                   "https://pagseguro.uol.com.br/v2/checkout/payment.html?code="}
		
		
		'--URL de TRANSACOES / BOLETO, DÉBITO e CARTÃO DE CRÉDITO
		Public urlTransactions() as String = {"https://ws.sandbox.pagseguro.uol.com.br/v2/transactions",
											  "https://ws.pagseguro.uol.com.br/v2/transactions"}
		
		'v3
		Public urlTransactionsV3() as String = {"https://ws.sandbox.pagseguro.uol.com.br/v3/transactions/",
												"https://ws.pagseguro.uol.com.br/v3/transactions/"}
		
		'--URLs de retorno de informações
		Public urlNotifications() as String = {"https://ws.sandbox.pagseguro.uol.com.br/v2/transactions/notifications",
											   "https://ws.pagseguro.uol.com.br/v2/transactions/notifications"}
		
				
		Public  transactions as List(Of Transaction) = new List(Of Transaction)
		
		
		'----
		
		
		
		Public Sub New()
		End Sub
		
		Public Sub Dispose() Implements System.IDisposable.Dispose
			'Limpando atributos aqui
			
		End Sub
		
		
		'----
		
		
		
		'Metodo de retorna status da transação
		Public Function rStatus(ByVal n As String) As String
			Dim r As String = ""
			
			Try
				Select Case(trim(n))
					
					Case "1" : r = "Aguardando pagamento"': o comprador iniciou a transação, mas até o momento o PagSeguro não recebeu nenhuma informação sobre o pagamento.
					Case "2" : r = "Em análise"': o comprador optou por pagar com um cartão de crédito e o PagSeguro está analisando o risco da transação.
					Case "3" : r = "Pago"': a transação foi paga pelo comprador e o PagSeguro já recebeu uma confirmação da instituição financeira responsável pelo processamento.
					Case "4" : r = "Disponível"': a transação foi paga e chegou ao final de seu prazo de liberação sem ter sido retornada e sem que haja nenhuma disputa aberta.
					Case "5" : r = "Em disputa"': o comprador, dentro do prazo de liberação da transação, abriu uma disputa.
					Case "6" : r = "Devolvida"': o valor da transação foi devolvido para o comprador.
					Case "7" : r = "Cancelada"': a transação foi cancelada sem ter sido finalizada.
					Case "8" : r = "Debitado"': a transação foi cancelada sem ter sido finalizada.
					Case "9" : r = "Retenção temporária"': o valor da transação foi devolvido para o comprador.
					
				End Select
			Catch ex As Exception
				r = ex.ToString()
				erros &= "428 - rStatus: "& ex.ToString() &"<br/><br/>"
			End Try
			
			return r
		End Function
		
		'Metodo de retorma forma de pagamento
		Public Function rPMType(ByVal n As String) As String
			Dim r As String = ""
			
			Try
				Select Case(trim(n))
					Case "1" : r = "Cartão de crédito"': o comprador escolheu pagar a transação com cartão de crédito.
					Case "2" : r = "Boleto"': o comprador optou por pagar com um boleto bancário.
					Case "3" : r = "Débito online (TEF)"': o comprador optou por pagar a transação com débito online de algum dos bancos conveniados.
					Case "4" : r = "Saldo PagSeguro"': o comprador optou por pagar a transação utilizando o saldo de sua conta PagSeguro.
					Case "5" : r = "Oi Paggo"': o comprador escolheu pagar sua transação através de seu celular Oi.
					Case "6" : r = "Depósito em conta"': o comprador optou por fazer um depósito na conta corrente do PagSeguro. 
				End Select
			Catch ex As Exception
				r = ex.ToString()
				erros &= "451 - rPMType: "& ex.ToString() &"<br/><br/>"
			End Try
			
			return r
		End Function
		
		'Metodo que identifica o meio de pagamento
		Public Function rPMCode(ByVal n As String) As String
			Dim r As String = ""
			
			Try
				Select Case(trim(n))
					Case "101" : r = "Cartão de crédito Visa."
					Case "102" : r = "Cartão de crédito MasterCard."
					Case "103" : r = "Cartão de crédito American Express."
					Case "104" : r = "Cartão de crédito Diners."
					Case "105" : r = "Cartão de crédito Hipercard."
					Case "106" : r = "Cartão de crédito Aura."
					Case "107" : r = "Cartão de crédito Elo."
					Case "108" : r = "Cartão de crédito PLENOCard."
					Case "109" : r = "Cartão de crédito PersonalCard."
					Case "110" : r = "Cartão de crédito JCB."
					Case "111" : r = "Cartão de crédito Discover."
					Case "112" : r = "Cartão de crédito BrasilCard."
					Case "113" : r = "Cartão de crédito FORTBRASIL."
					Case "114" : r = "Cartão de crédito CARDBAN. *"
					Case "115" : r = "Cartão de crédito VALECARD."
					Case "116" : r = "Cartão de crédito Cabal."
					Case "117" : r = "Cartão de crédito Mais!."
					Case "118" : r = "Cartão de crédito Avista."
					Case "119" : r = "Cartão de crédito GRANDCARD."
					Case "120" : r = "Cartão de crédito Sorocred."
					Case "201" : r = "Boleto Bradesco. *"
					Case "202" : r = "Boleto Santander."
					Case "301" : r = "Débito online Bradesco."
					Case "302" : r = "Débito online Itaú."
					Case "303" : r = "Débito online Unibanco. *"
					Case "304" : r = "Débito online Banco do Brasil."
					Case "305" : r = "Débito online Banco Real. *"
					Case "306" : r = "Débito online Banrisul."
					Case "307" : r = "Débito online HSBC."
					Case "401" : r = "Saldo PagSeguro."
					Case "501" : r = "Oi Paggo."
					Case "701" : r = "Depósito em conta - Banco do Brasil."
					Case "702" : r = "Depósito em conta - HSBC."
				End Select
			Catch ex As Exception
				r = ex.ToString()
				erros &= "472 - rPMCode: "& ex.ToString() &"<br/><br/>"
			End Try
			
			return r
		End Function
		
		'Metodo que retorna valor de origem de cancelamento
		Public Function rCancellation(ByVal n As String) As String
			Dim r As String = ""
			
			Try
				Select Case(n)
					Case "INTERNAL" : r = "PagSeguro"
					Case "EXTERNAL" : r = "Instituição financeira"
				End Select
			Catch ex As Exception
				r = ex.TOString()
				erros &= "511 - rCancellation: "& ex.ToString() &"<br/><br/>"
			End Try
			
			return r
		End Function
		
		'Metodo que retorna valor de tipo de entrega
		Public Function rEntType(ByVal n As String) As String
			Dim r As String = ""
			
			Try
				Select Case (n)
					Case "1" : r = "Encomenda normal (PAC)."
					Case "2" : r = "SEDEX."
					Case "3" : r = "Não especificado."
				End Select
			Catch ex As Exception
				r = ex.ToString()
				erros &= "528 - rEntType: "& ex.ToString()
			End Try
			
			return r
		End Function
		
		
	End Class
	
	
	Public Class Transaction
		Implements IDisposable
		
		private sdate as string = ""
		private sreference as string = ""
		private scode as string = ""
		private stype as string = ""
		private sstatus as string = ""
		private spmtype as string = ""
		private sgrossAmount as string = ""
		private sdiscountAmount as string = ""
		private sfeeAmount as string = ""
		private snetAmount as string = ""
		private sextraAmount as string = ""
		
		
		Public Property data() as String
			Get
				return sdate
			End Get
			Set(byval v as string)
				sdate = v
			End Set
		End Property
		
		Public Property reference() as String
			Get
				return sreference
			End Get
			Set(byval v as string)
				sreference = v
			End Set
		End Property
		
		Public Property code() as String
			Get
				return scode
			End Get
			Set(byval v as string)
				scode = v
			End Set
		End Property
		
		Public Property tipo() as String
			Get
				return stype
			End Get
			Set(byval v as string)
				stype = v
			End Set
		End Property
		
		Public Property status() as String
			Get
				return sstatus
			End Get
			Set(byval v as String)
				sstatus = v
			End Set
		End Property
		
		Public Property pmtype() as String
			Get
				return spmtype
			End Get
			Set(byval v as String)
				spmtype = v
			End Set
		End Property
		
		Public Property grossAmount() as String
			Get
				return sgrossAmount
			End Get
			Set(byval v as string)
				sgrossAmount = v
			End Set
		End Property
		
		Public Property discountAmount() as String
			Get
				return sdiscountAmount
			End Get
			Set(byval v as string)
				sdiscountAmount = v
			End Set
		End Property
		
		Public Property feeAmount() as String
			Get
				return sfeeAmount
			End Get
			Set(byval v as string)
				sfeeAmount = v
			End Set
		End Property
		
		Public Property netAmount() as String
			Get
				return snetAmount
			End Get
			Set(byval v as string)
				snetAmount = v
			End Set
		End Property
		
		Public Property extraAmount() as String
			Get
				return sextraAmount
			End Get
			Set(byval v as string)
				sextraAmount = v
			End Set
		End Property
		
		'-------------
						
		Public Sub New()
		End Sub
		
		Public Sub New(byval pdata as string, byval preference as string, byval pcode as string, byval ptipo as string, byval pstatus as string, _
		byval ppmtype as string, byval pgrossAmount as string, byval pdiscountAmout as string, byval pfeeAmount as string, byval pnetAmount as string, _
		byval pextraAmount as string)
			
			data = pdata
			reference = preference
			code = pcode
			tipo = ptipo
			status = pstatus
			pmtype = ppmtype
			grossAmount = pgrossAmount
			discountAmount = pdiscountAmout
			feeAmount = pfeeAmount
			netAmount = pnetAmount
			extraAmount = pextraAmount
			
		End Sub
		
		Public Sub Dispose() Implements System.IDisposable.Dispose
			
			data = ""
			reference = ""
			code = ""
			tipo = ""
			status = ""
			pmtype = ""
			grossAmount = ""
			discountAmount = ""
			feeAmount = ""
			netAmount = ""
			extraAmount = ""
			
		End Sub
		
	End Class
	
	
End Namespace