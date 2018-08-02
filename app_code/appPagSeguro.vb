Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
imports System.Net
imports System.IO
imports System.Text
Imports System.XML
Imports SwebSender
Imports System.Collections.Specialized

Namespace appPagSeguro
	Public Class pagSeguro
		
		'Configura��es
		Public modo As String = "teste" 'teste ou producao 'default � teste
		
		'Atributos Monetarios
		'---
		
		'Atributos PagSeguro
		Public psEmail As String = ""
		Public psToken As String = ""
		Public psMoeda As String = "BRL"		
		Public psItems As New Hashtable()
		
		'Atributos do solicitante
		Public solNPedido As String = "" 'Numero do pedido
		Public solNome As String = "" ' Nome
		Public solTDDD As String = "" ' DDD
		Public solTNum As String = "" ' Numero do telefone
		Public solEmail As String = "" ' Email
		Public solEntrega As String = "1" ' Tipo de entrega 1- Encomenda Normal, 2- Sedex, 3- N�o definida
		Public solEndereco As String = "" ' Endereco
		Public solNumero As String = "" ' Numero do endereco
		Public solComplemento As String = "" ' Complemento
		Public solBairro As String = "" ' Bairro
		Public solCep As String = "" ' Cep
		Public solCidade As String = "" ' Cidade
		Public solUF As String = "" ' Estado
		Public solPais As String = "BRA" 'Pa�s
		Public solDesconto As String = "" 'Desconto formato decimal , usando ponto (.)  para separar as casas decimais. Ex.:  12,50 -> 12.50
		
		'Atributos de Depura��o
		Public erros As String = ""
		
		'Atributos de entrega
		Public psFrete As String = "0.00"
		
		'Atributos de consulta
		Public consDate As String = ""
		Public consLastEDate As String = ""
		Public consCode As String = "" 'Codigo da transa��o (ID unico da transa��o no Pag-Seguro)
		Public consRef As String = "" 'Referencia � o ID de pedido enviado pelo seu sistema 
		Public consType As String = "" 'Tipo da trasa��o no caso, pagamento 1
		Public consStatus As String = "" 'Status descrito abaixo:
		'1	Aguardando pagamento: o comprador iniciou a transa��o, mas at� o momento o PagSeguro n�o recebeu nenhuma informa��o sobre o pagamento.
		'2	Em an�lise: o comprador optou por pagar com um cart�o de cr�dito e o PagSeguro est� analisando o risco da transa��o.
		'3	Paga: a transa��o foi paga pelo comprador e o PagSeguro j� recebeu uma confirma��o da institui��o financeira respons�vel pelo processamento.
		'4	Dispon�vel: a transa��o foi paga e chegou ao final de seu prazo de libera��o sem ter sido retornada e sem que haja nenhuma disputa aberta.
		'5	Em disputa: o comprador, dentro do prazo de libera��o da transa��o, abriu uma disputa.
		'6	Devolvida: o valor da transa��o foi devolvido para o comprador.
		'7	Cancelada: a transa��o foi cancelada sem ter sido finalizada.
		Public consCancellationSource As String = "" 'Origem do cancelamento : INTERNAL -> PagSeguro  |   EXTERNAL -> Institui��o Financeira
		Public consPMType As String = ""' PaymentMethod: 'Forma de pagamento
		'1	Cart�o de cr�dito: o comprador escolheu pagar a transa��o com cart�o de cr�dito.
		'2	Boleto: o comprador optou por pagar com um boleto banc�rio.
		'3	D�bito online (TEF): o comprador optou por pagar a transa��o com d�bito online de algum dos bancos conveniados.
		'4	Saldo PagSeguro: o comprador optou por pagar a transa��o utilizando o saldo de sua conta PagSeguro.
		'5	Oi Paggo: o comprador escolheu pagar sua transa��o atrav�s de seu celular Oi.
		Public consPMCode As String = ""' PaymentCode: 'Codigo que identifica o meio de pagamento
		'101	Cart�o de cr�dito Visa.
		'102	Cart�o de cr�dito MasterCard.
		'103	Cart�o de cr�dito American Express.
		'104	Cart�o de cr�dito Diners.
		'105	Cart�o de cr�dito Hipercard.
		'106	Cart�o de cr�dito Aura.
		'107	Cart�o de cr�dito Elo.
		'108	Cart�o de cr�dito PLENOCard.
		'109	Cart�o de cr�dito PersonalCard.
		'110	Cart�o de cr�dito JCB.
		'111	Cart�o de cr�dito Discover.
		'112	Cart�o de cr�dito BrasilCard.
		'113	Cart�o de cr�dito FORTBRASIL.
		'201	Boleto Bradesco. *
		'202	Boleto Santander.
		'301	D�bito online Bradesco.
		'302	D�bito online Ita�.
		'303	D�bito online Unibanco. *
		'304	D�bito online Banco do Brasil.
		'305	D�bito online Banco Real. *
		'306	D�bito online Banrisul.
		'307	D�bito online HSBC.
		'401	Saldo PagSeguro.
		'501	Oi Paggo.
		Public consGrossAmout As String = "" 'Valor Bruto da transa��o
		Public consDiscountAmount As String = "" 'Valor do desconto dado
		Public consFreeAmount As String = "" 'Valor das taxas cobradas
		Public consNetAmount As String = "" 'Valor liquido da  transa��o
		Public consEscrowEndDate As String = "" 'Data de cr�dito.Formato: YYYY-MM-DDThh:mm:ss.sTZD,
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
		'3	Tipo de frete n�o especificado.
		Public consEntCost As String = "" 'Custo do frete
		Public consEntCountry As String = "" 'Pais de entrega
		Public consEntState As String = "" 'Estado de entrega
		Public consEntCity As String = "" 'Cidade de entrega
		Public consEntPostalCode As String = "" 'Codigo postal de entrega
		Public consEntDistrict As String = "" 'Bairro de entrega
		Public consEntStreet As String = "" 'Rua de entrega
		Public consEntNumber As String = "" 'Numero do endereco de entrega
		Public consEntComplement As String = "" 'Complemento do endere�o de entrega
	
		
		Public Function pagSeguro() As String
			'[s][a][i][t][o]
		End Function
		
		Public Function pagSeguro(ByVal email As String, ByVal token As String) As String
			Dim resultado As String = ""
			
			Try
			
				psEmail = email
				psToken = token
				
			Catch ex As Exception
				resultado = ex.ToString()
				erros &= "124 - pagSeguro:"& ex.ToString() &"<br/><br/>"
			End Try
			
			return resultado
		End Function
		
		Public Function pagSeguro(ByVal email As String, ByVal token As String, ByVal moeda As String) As String
			Dim resultado As String = ""
			
			Try
			
				psEmail = email
				psToken = token
				psMoeda = moeda
				
			Catch ex As Exception
				resultado = ex.ToString()
				erros &= "140 - pagSeguro:"& ex.ToString() &"<br/><br/>"
			End Try
			
			return resultado
		End Function
		
		Public Function adicionaItem(ByVal id As String, ByVal produto As String, ByVal preco As String, ByVal quantidade As String) As String
			Dim resultado As String = ""
			
			Try
				Dim x As Integer = psItems.Count + 1
				
				If x > 1 then
					psFrete = "0.00"
				End If
				
				Dim psItemStr As String = ""
				psItemStr &= "&itemId"& x &"="& id
				psItemStr &= "&itemDescription"& x &"="& produto
				psItemStr &= "&itemAmount"& x &"="& formataPreco(preco)
				psItemStr &= "&itemQuantity"& x &"="& quantidade
				psItemStr &= "&itemShippingCost"& x &"="& psFrete
				
				psItems.add(id, psItemStr)
				
			Catch ex As Exception
				resultado = ex.ToString()
				erros &= "157 - adicionaItem:"& ex.ToString() &"<br/><br/>"
			End Try
			
			return resultado
		End Function
		
		Public Function authCode(ByVal urlRetorno As String) As String
			Dim resultado As String = ""
			
			Dim parametros As String = ""
			Dim aux As String = ""
			Dim codigo As String = ""
			Dim dataFinalizacao As String = ""
			Dim todos As String = ""
			
			Dim owner As String = "email="& psEmail &"&token="& psToken &"&currency="& psMoeda
			Dim items As String = retPsItems()
			Dim buyer As String = retPSBuyerInfo()
			parametros = owner & items & buyer &"&redirectURL="& urlRetorno
			
			Try
				
				Dim urlCheckout as String = "https://ws.pagseguro.uol.com.br/v2/checkout/"
				
				if modo = "teste" then
					urlCheckout = "https://ws.sandbox.pagseguro.uol.com.br/v2/checkout"
				end if
				
				aux = POST(urlCheckout, parametros)
				Dim dados As String = ""& trim(aux.ToString())
				
				'todos = dados
				
				Dim doc As XmlDocument = new XmlDocument()
				doc.LoadXml(dados)
								
				Dim elem As XmlElement = doc.DocumentElement
				'				
				codigo = Elem.ChildNodes.Item(0).InnerText
				dataFinalizacao = Elem.ChildNodes.Item(1).InnerText
				
				resultado = codigo & todos
				
			Catch ex As Exception
				resultado = ex.ToString()
				erros &= "184 - authCode:"& ex.ToString() &"<br/><br/>"& todos
			End Try
						
			return resultado
		End Function
		
		Public Function pagamento(ByVal uriRet As String) As String
			Dim resultado As String = ""
			Try
				Dim urlPS as String = "https://pagseguro.uol.com.br/v2/checkout/payment.html?code="
				
				if modo = "teste" then
					urlPS = "https://sandbox.pagseguro.uol.com.br/v2/checkout/payment.html?code="
				end if
				
				resultado = urlPS & authCode(uriRet)
				
			Catch ex As Exception
				
				resultado = ex.ToString()
				erros &= "225 - pagamento:"& ex.ToString() &"<br/><br/>"
			End Try
			
			return resultado
		End Function
		
		Public Function retPsItems() As String
			Dim resultado As String = ""
			Dim Items As DictionaryEntry
			
			Try
				For each Items in psItems
					resultado &= Items.Value
				Next
			Catch ex As Exception
				resultado = ex.ToString()
				erros &= "240 - retPsItems:"& ex.ToString() &"<br/><br/>"
			End Try
			
			return resultado
		End Function
		
		
		Public Function retPSBuyerInfo() As String
			Dim resultado As String = ""
			
			Try
				
				resultado &= "&reference="& solNPedido
				resultado &= "&senderName="& solNome
				resultado &= "&senderAreaCode="& solTDDD
				resultado &= "&senderPhone="& solTNum
				resultado &= "&senderEmail="& solEmail
				resultado &= "&shippingType="& solEntrega
				resultado &= "&shippingAddressStreet="& solEndereco
				resultado &= "&shippingAddressNumber="& solNumero
				resultado &= "&shippingAddressComplement="& solComplemento
				resultado &= "&shippingAddressDistrict="& solBairro
				resultado &= "&shippingAddressPostalCode="& solCep
				resultado &= "&shippingAddressCity="& solCidade
				resultado &= "&shippingAddressState="& solUF
				resultado &= "&shippingAddressCountry="& solPais
				
				if solDesconto <> "" then
					
					resultado &= "&extraAmount="& solDesconto
					
				end if
				
			Catch ex As Exception
				
				resultado = ex.ToString()
				erros &= "257 - retPSBuyerInfo:"& ex.ToString() &"<br/><br/>"
			End Try
			
			return resultado
			
		End Function
		
		Private Function formataPreco(ByVal valor As String) As String
			Dim resultado As String = ""
			
			Try
				if valor <> "" then
					If valor.indexOf(",") > 0 Then
						resultado = replace(valor,",",".")
					Else
						Dim aux As Double = cDbl(valor)
						Dim rconta As Double
						rconta = aux / 100
						resultado = formatNumber(rconta,2)
						resultado = replace(resultado,",",".")
					End If
				end if
			Catch ex As Exception
				resultado = ex.ToString()
				erros &= "287 - formataPreco:"& ex.ToString() &"<br/><br/>"
			End Try
			
			return resultado
		End Function
		
		Public Function consultaTransacao2(ByVal codigo As String) As String
			Dim r As String = ""
			
			Try
				Dim x As integer = 0
				Dim aux As String = ""
				Dim dados As String = ""
				Dim parametros As String = "email="& psEmail &"&token="& psToken
				Dim wP as new SwebSender.webPOST()
				
				aux = wp.executar("https://ws.pagseguro.uol.com.br/v2/transactions/"& codigo &"?"& parametros)
				dados = ""& trim(aux.ToString())
				r = dados
			Catch ex As Exception
				r = ex.ToString()
				erros &= "310 - consultaTransacao2:"& ex.ToString() &"<br/><br/>"
			End Try
			return r
		End Function
		
		Public Function consultaTransacao(ByVal codigo As String) As String
			Dim r As String = ""
			
			Try
				Dim x As integer = 0
				Dim aux As String = ""
				Dim saux As String = ""
				Dim dados As String = ""
				Dim parametros As String = "email="& psEmail &"&token="& psToken
				Dim wP as new SwebSender.webPOST()
				
				aux = wp.executar("https://ws.pagseguro.uol.com.br/v2/transactions/"& codigo &"?"& parametros)
				dados = ""& trim(aux.ToString())
				
				Dim doc As XmlDocument = new XmlDocument()
				doc.LoadXml(dados)
								
				Dim elem As XmlElement = doc.DocumentElement
								
				consDate = Elem.SelectSingleNode("date").InnerText
				consCode = Elem.SelectSingleNode("code").InnerText
				consRef = Elem.SelectSingleNode("reference").InnerText
				consType = Elem.SelectSingleNode("type").InnerText
				consStatus = Elem.SelectSingleNode("status").InnerText
				consLastEDate = Elem.SelectSingleNode("lastEventDate").InnerText
				
				
				Try
					consCancellationSource = Elem.SelectSingleNode("cancellationSource").InnerText
				Catch ex0 As Exception
				End Try
				
				consPMType = Elem.SelectSingleNode("paymentMethod").SelectSingleNode("type").InnerText
				consPMCode = Elem.SelectSingleNode("paymentMethod").SelectSingleNode("code").InnerText
				consGrossAmout = Elem.SelectSingleNode("grossAmount").InnerText
				consDiscountAmount = Elem.SelectSingleNode("discountAmount").InnerText
				consFreeAmount = Elem.SelectSingleNode("feeAmount").InnerText
				consNetAmount = Elem.SelectSingleNode("netAmount").InnerText
				
				Try
					consEscrowEndDate = Elem.SelectSingleNode("escrowEndDate").InnerText
				Catch ex1 As Exception
				End Try
				
				consExtraAmount = Elem.SelectSingleNode("extraAmount").InnerText
				consInstallmentCount = Elem.SelectSingleNode("installmentCount").InnerText
				consItemCount = Elem.SelectSingleNode("itemCount").InnerText
				
				Dim xLem As XmlNode
				
				For Each xLem in  Elem.SelectSingleNode("items")
					If x = 0 Then
						saux = ""& xLem.SelectSingleNode("id").InnerText &"|"& xLem.SelectSingleNode("description").InnerText &"|"& xLem.SelectSingleNode("amount").InnerText &"|"& xLem.SelectSingleNode("quantity").InnerText
					Else
						saux &= "#"& xLem.SelectSingleNode("id").InnerText &"|"& xLem.SelectSingleNode("description").InnerText &"|"& xLem.SelectSingleNode("amount").InnerText &"|"& xLem.SelectSingleNode("quantity").InnerText
					End If
					x = x + 1
				Next
				
				consItems = split(saux,"#")
			
				consSenEmail = Elem.SelectSingleNode("sender").SelectSingleNode("email").InnerText
				consSenName = Elem.SelectSingleNode("sender").SelectSingleNode("name").InnerText
				
				Try
					
					consSenPhoneAreaCode = Elem.SelectSingleNode("sender").SelectSingleNode("phone").SelectSingleNode("areaCode").InnerText
					consSenPhoneNumber = Elem.SelectSingleNode("sender").SelectSingleNode("phone").SelectSingleNode("number").InnerText
					
				Catch ex2 As Exception
				End Try
				
				consEntType = Elem.SelectSingleNode("shipping").SelectSingleNode("type").InnerText
				consEntCost = Elem.SelectSingleNode("shipping").SelectSingleNode("cost").InnerText
				
				Try
				
					consEntCountry = Elem.SelectSingleNode("shipping").SelectSingleNode("address").SelectSingleNode("country").InnerText
					consEntState = Elem.SelectSingleNode("shipping").SelectSingleNode("address").SelectSingleNode("state").InnerText
					consEntCity = Elem.SelectSingleNode("shipping").SelectSingleNode("address").SelectSingleNode("city").InnerText
					consEntPostalCode = Elem.SelectSingleNode("shipping").SelectSingleNode("address").SelectSingleNode("postalCode").InnerText
					consEntDistrict = Elem.SelectSingleNode("shipping").SelectSingleNode("address").SelectSingleNode("district").InnerText
					consEntStreet = Elem.SelectSingleNode("shipping").SelectSingleNode("address").SelectSingleNode("street").InnerText
					consEntNumber = Elem.SelectSingleNode("shipping").SelectSingleNode("address").SelectSingleNode("number").InnerText
					consEntComplement = Elem.SelectSingleNode("shipping").SelectSingleNode("address").SelectSingleNode("complement").InnerText
					
				Catch ex3 As Exception
				End Try
				
			Catch ex As Exception
				r = ex.Message.ToString()
				erros &= "330 - consultaTransacao:"& ex.ToString() &"<br/><br/>"
			End Try
			
			return r
		End Function
		
		'Metodo de retorna status da transa��o
		Public Function rStatus(ByVal n As String) As String
			Dim r As String = ""
			
			Try
				Select Case(n)
					
					Case "1" : r = "Aguardando pagamento"': o comprador iniciou a transa��o, mas at� o momento o PagSeguro n�o recebeu nenhuma informa��o sobre o pagamento.
					Case "2" : r = "Em an�lise"': o comprador optou por pagar com um cart�o de cr�dito e o PagSeguro est� analisando o risco da transa��o.
					Case "3" : r = "Pago"': a transa��o foi paga pelo comprador e o PagSeguro j� recebeu uma confirma��o da institui��o financeira respons�vel pelo processamento.
					Case "4" : r = "Dispon�vel"': a transa��o foi paga e chegou ao final de seu prazo de libera��o sem ter sido retornada e sem que haja nenhuma disputa aberta.
					Case "5" : r = "Em disputa"': o comprador, dentro do prazo de libera��o da transa��o, abriu uma disputa.
					Case "6" : r = "Devolvida"': o valor da transa��o foi devolvido para o comprador.
					Case "7" : r = "Cancelada"': a transa��o foi cancelada sem ter sido finalizada.
					
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
				Select Case(n)
					Case "1" : r = "Cart�o de cr�dito"': o comprador escolheu pagar a transa��o com cart�o de cr�dito.
					Case "2" : r = "Boleto"': o comprador optou por pagar com um boleto banc�rio.
					Case "3" : r = "D�bito online (TEF)"': o comprador optou por pagar a transa��o com d�bito online de algum dos bancos conveniados.
					Case "4" : r = "Saldo PagSeguro"': o comprador optou por pagar a transa��o utilizando o saldo de sua conta PagSeguro.
					Case "5" : r = "Oi Paggo"': o comprador escolheu pagar sua transa��o atrav�s de seu celular Oi.
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
				Select Case(n)
					Case "101" : r = "Cart�o de cr�dito Visa."
					Case "102" : r = "Cart�o de cr�dito MasterCard."
					Case "103" : r = "Cart�o de cr�dito American Express."
					Case "104" : r = "Cart�o de cr�dito Diners."
					Case "105" : r = "Cart�o de cr�dito Hipercard."
					Case "106" : r = "Cart�o de cr�dito Aura."
					Case "107" : r = "Cart�o de cr�dito Elo."
					Case "108" : r = "Cart�o de cr�dito PLENOCard."
					Case "109" : r = "Cart�o de cr�dito PersonalCard."
					Case "110" : r = "Cart�o de cr�dito JCB."
					Case "111" : r = "Cart�o de cr�dito Discover."
					Case "112" : r = "Cart�o de cr�dito BrasilCard."
					Case "113" : r = "Cart�o de cr�dito FORTBRASIL."
					Case "201" : r = "Boleto Bradesco. *"
					Case "202" : r = "Boleto Santander."
					Case "301" : r = "D�bito online Bradesco."
					Case "302" : r = "D�bito online Ita�."
					Case "303" : r = "D�bito online Unibanco. *"
					Case "304" : r = "D�bito online Banco do Brasil."
					Case "305" : r = "D�bito online Banco Real. *"
					Case "306" : r = "D�bito online Banrisul."
					Case "307" : r = "D�bito online HSBC."
					Case "401" : r = "Saldo PagSeguro."
					Case "501" : r = "Oi Paggo."
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
					Case "EXTERNAL" : r = "Institui��o financeira"
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
					Case "3" : r = "N�o especificado."
				End Select
			Catch ex As Exception
				r = ex.ToString()
				erros &= "528 - rEntType: "& ex.ToString()
			End Try
			
			return r
		End Function
		
		
		
		
		
		
		
		'----------------------------------------------------------
		'M�todo que faz um web POST
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
		'm�todo que faz uma chamada GET com parametros
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
End Namespace