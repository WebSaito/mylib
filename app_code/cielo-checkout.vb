imports System
imports System.Net
Imports System.Net.Security
Imports System.Net.Security.SslPolicyErrors
imports System.IO
imports System.Text
imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates

Namespace nsCielo
	
	Public Class clsCheckoutCielo
		
		'[WEB.CONFIG]
		public caminhoDBR as string = configurationSettings.appSettings("sqlRead")	
		
		'[VARIAVEIS]
		Public str, totalRows, contador, splQtde, quantidade, preco, retorno, json
		Public tbl_pedidos = "crz_pedidos"
		Public tbl_cadastros = "crz_pedidos_usuarios"
		Public tbl_entrega = "crz_pedidos_enderecoEntrega"
		
		'[INTERFACE]
		Public checkout as new iCheckout()
		Public rowsCheck as List(Of iCheckout) = new List(Of iCheckout)
		
		
		'[INICIO] Função para gerar o json e redirecionar para o gateway de pagamento do Cielo
		'-------------------------------------------------------------------------------------
		Public function fCheckoutCielo(Optional byval chave as String = "")
			try
				
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
				
				Dim rRequest as webRequest = webRequest.create("https://cieloecommerce.cielo.com.br/api/public/v1/orders")
				
				'//Funcao para os dados fixos
				'fDadosFixos()
				
				'//funcao para carregar os dados do pedido
				'fDadosPedido(chave)
				
				'//funcao para carregar os dados do carrinho
				'fDadosCarrinho(checkout.orderNumber)
				
				'//funcao para carregar os dados do cliente
				'fDadosCliente(checkout.orderNumber)
				
				rRequest.Method = "POST"
				rRequest.ContentType = "text/json" '"application/json"
				rRequest.UseDefaultCredentials = true
				rRequest.Headers("MerchantId") = checkout.merchantID '6f619787-f663-4c26-b223-4d202b4ee9fb
				
				json = "{"
				json &= "    "& chr(34) &"OrderNumber"& chr(34) &": "& chr(34) & checkout.orderNumber & chr(34) &","
				json &= "    "& chr(34) &"SoftDescriptor"& chr(34) &": "& chr(34) & checkout.softDescriptor & chr(34) &","
				json &= "    "& chr(34) &"Cart"& chr(34) &": {"
				json &= "        "& chr(34) &"Discount"& chr(34) &": {"
				json &= "            "& chr(34) &"Type"& chr(34) &": "& chr(34) &""& checkout.cartType &""& chr(34) &","
				json &= "            "& chr(34) &"Value"& chr(34) &": "& checkout.cartValue &""
				json &= "        },"
				json &= "        "& chr(34) &"Items"& chr(34) &": ["

'				'//Para teste de carrinho
'				json &= "            {"
'				json &= "                "& chr(34) &"Name"& chr(34) &": "& chr(34) & checkout.itensName & chr(34) &","
'				json &= "                "& chr(34) &"Description"& chr(34) &": "& chr(34) & checkout.itensDescription & chr(34) &","
'				json &= "                "& chr(34) &"UnitPrice"& chr(34) &": "& checkout.itensUnitPrice &","
'				json &= "                "& chr(34) &"Quantity"& chr(34) &": "& checkout.itensQuantity &","
'				json &= "                "& chr(34) &"Type"& chr(34) &": "& chr(34) & checkout.itensType & chr(34) &","
'				json &= "                "& chr(34) &"Sku"& chr(34) &": "& chr(34) & checkout.itensSku & chr(34) &","
'				json &= "                "& chr(34) &"Weight"& chr(34) &": "& checkout.itensWeight &""
'				json &= "            }"

				'[INICIO] LOOP DOS PRODUTOS
				totalRows = rowsCheck.count
				contador = 1
				for each x as nsCielo.iCheckout in rowsCheck
					json &= "            {"
					json &= "                "& chr(34) &"Name"& chr(34) &": "& chr(34) & x.itensName & chr(34) &","
					json &= "                "& chr(34) &"Description"& chr(34) &": "& chr(34) & x.itensDescription & chr(34) &","
					json &= "                "& chr(34) &"UnitPrice"& chr(34) &": "& x.itensUnitPrice &","
					json &= "                "& chr(34) &"Quantity"& chr(34) &": "& x.itensQuantity &","
					json &= "                "& chr(34) &"Type"& chr(34) &": "& chr(34) & x.itensType & chr(34) &","
					json &= "                "& chr(34) &"Sku"& chr(34) &": "& chr(34) & x.itensSku & chr(34) &","
					json &= "                "& chr(34) &"Weight"& chr(34) &": "& x.itensWeight &""
					json &= "            }"
					
					if(contador < totalRows)then
						json &= ","
					end if				
					contador = contador+1
				next
				rowsCheck.clear
				
				json &= "        ]"
				json &= "    },"
				json &= "    "& chr(34) &"Shipping"& chr(34) &": {"
				json &= "        "& chr(34) &"Type"& chr(34) &": "& chr(34) & checkout.shipType & chr(34) &","
				json &= "        "& chr(34) &"SourceZipCode"& chr(34) &": "& chr(34) & checkout.sourceZipcode & chr(34) &","
				json &= "        "& chr(34) &"TargetZipCode"& chr(34) &": "& chr(34) & checkout.targetZipCode & chr(34) &","
				json &= "        "& chr(34) &"Address"& chr(34) &": {"
				json &= "            "& chr(34) &"Street"& chr(34) &": "& chr(34) & checkout.addressStreet & chr(34) &","
				json &= "            "& chr(34) &"Number"& chr(34) &": "& chr(34) & checkout.addressNumber & chr(34) &","
				json &= "            "& chr(34) &"Complement"& chr(34) &": "& chr(34) & checkout.addressComplement & chr(34) &","
				json &= "            "& chr(34) &"District"& chr(34) &": "& chr(34) & checkout.addressDistrict & chr(34) &","
				json &= "            "& chr(34) &"City"& chr(34) &": "& chr(34) & checkout.addressCity & chr(34) &","
				json &= "            "& chr(34) &"State"& chr(34) &": "& chr(34) & checkout.addressState & chr(34) &""
				json &= "        },"
				json &= "        "& chr(34) &"Services"& chr(34) &": ["
				json &= "            {"
				json &= "                "& chr(34) &"Name"& chr(34) &": "& chr(34) & checkout.serviceName & chr(34) &","
				json &= "                "& chr(34) &"Price"& chr(34) &": "& IIF(checkout.servicePrice <> "", replace(replace(checkout.servicePrice, ",",""),".",""), "") &","
				json &= "                "& chr(34) &"Deadline"& chr(34) &": "& checkout.serviceDeadline &""
				json &= "            }"
				json &= "        ]"
				json &= "    },"
				json &= "    "& chr(34) &"Payment"& chr(34) &": {"
				json &= "        "& chr(34) &"BoletoDiscount"& chr(34) &": "& checkout.paymentBoletoDiscount &","
				json &= "        "& chr(34) &"DebitDiscount"& chr(34) &": "& checkout.paymentDebitDiscount &""
				json &= "     },"
				json &= "     "& chr(34) &"Customer"& chr(34) &": {"
				json &= "         "& chr(34) &"Identity"& chr(34) &": "& chr(34) & checkout.CustomerIdentity & chr(34) &","
				json &= "         "& chr(34) &"FullName"& chr(34) &": "& chr(34) & checkout.CustomerFullname & chr(34) &","
				json &= "         "& chr(34) &"Email"& chr(34) &": "& chr(34) & checkout.CustomerEmail & chr(34) &","
				json &= "         "& chr(34) &"Phone"& chr(34) &": "& chr(34) & checkout.CustomerPhone & chr(34) &""
				json &= "     },"
				json &= "     "& chr(34) &"Options"& chr(34) &": {"
				json &= "         "& chr(34) &"AntifraudEnabled"& chr(34) &": "& checkout.optionsAntifraudEnabled &""
				json &= "     }"
				json &= "}"
				
				using escreve as new streamWriter(rRequest.GetRequestStream())
					escreve.write(json)
					escreve.close()
				end using
				
				Dim resposta as HttpWebResponse = CType(rRequest.GetResponse(), httpWebResponse)
				
				Dim rStream as Stream = resposta.GetResponseStream()
				
				Dim lStream as new streamReader(rStream, Encoding.UTF8)
				
				Dim texto as string = lStream.ReadToEnd()
				
				return texto
				
				'return json
				
			catch ex as webException
				'return ex.tostring()
				dim retorno
				using r as webResponse = ex.response
					dim httpResponse as HttpWebResponse = r
					retorno = "Erro: " & httpResponse.StatusCode & "<br>"
					using dados as stream = r.GetResponseStream()
						using reader = new streamReader(dados)
							dim re = reader.ReadToEnd()
							retorno &= re 
						end using
					end using
				end using
				return json &"<br><br>"& retorno
			end try
						
		end Function
		
		'[INICIO] Função para carregar os dados do pedido
		'------------------------------------------------
		Public Function fDadosPedido(byval chave)
			
			str = "select * from "& tbl_pedidos &" where chave='"& chave &"'"
			using conn as new sqlConnection(caminhoDBR), web as new sqlCommand(str, conn)
				conn.open()
				using dr as sqlDataReader = web.executeReader()
					if(dr.read())then
						checkout.orderNumber = dr("id").tostring()
						checkout.valorCielo = dr("valorCobrado").tostring()
						checkout.ServicePrice = dr("freteCielo").toString()
						checkout.shipType = fFormaEnvio("4")
					end if
				end using
			end using
			
		End Function
		
		'[INICIO] Função para carregar o carrinho
		'----------------------------------------
		Public Function fDadosCarrinho(byval numero_pedido)
			
'			str = " select * from tbl_pedidos_itens as tpi "
'			str &= " inner join tbl_pedidos_produtos as tpp "
'			str &= " on tpi.fk_idProdutos = tpp.fk_idProd and tpi.numero_pedido = tpp.numero_pedido"
'			str &= " where tpi.numero_pedido=" & numero_pedido
'			str &= " order by tpi.id desc "
			
			'httpContext.current.response.write(str)
			
			'using conn as new sqlConnection(caminhoDBR), web as new sqlCommand(str, conn)
'				conn.open()
'				using dr as sqlDAtaReader = web.executeReader()
'					while dr.read()
'						splQtde = split(dr("quantidade").tostring(), "!@!")
'						for i=0 to ubound(splQtde)-1
'							quantidade = cint(splQtde(i)) + quantidade
'						next
'						preco = quantidade * cdbl(dr("preco").tostring())
'						rowsCheck.Add(new iCheckout(dr("nomeProd").tostring(), "", fPrecoFormatado(dr("preco").tostring()), quantidade, checkout.itensType, checkout.itensSku, checkout.itensWeight))
'						quantidade = 0
'					end while
'				end using
'			end using


			str = "select * from "& tbl_pedidos &" where id="& numero_pedido &" "
			using conn as new sqlConnection(caminhoDBR), web as new sqlCommand(str, conn)
				conn.open()
				using dr as sqlDAtaReader = web.executeReader()
					if( dr.read())then
						rowsCheck.Add(new iCheckout("Pedido " & dr("id").tostring(), "", fPrecoFormatado(dr("valorCobrado").tostring()), 1, checkout.itensType, checkout.itensSku, checkout.itensWeight))
						quantidade = 0
					end if
				end using
			end using
			
		End Function
		
		'[INICIO] Função para buscar os dados do cliente tbl_pedidos_cadastros
		'---------------------------------------------------------------------
		Public Function fDadosCliente(byval numero_pedido)
			
			str = " select top 1  "
			str &= " C.nome, C.email, C.rg, C.cpf, C.telddd, C.tel "
			str &= ", E.cep, E.endereco, E.numero, E.complemento, E.bairro, E.cidade, E.uf, E.destinatario "
			str &= " from " & tbl_cadastros & " as C "
			str &= " inner join "& tbl_entrega &" as E ON C.fk_idPedido = E.fk_idPedido and C.fk_idUsr = E.idUsuario "
			str &= " where C.fk_idPedido=" & numero_pedido
			
			using conn as new sqlConnection(caminhoDBR), web as new sqlCommand(str, conn)
				conn.open()
				using dr as sqlDAtaReader = web.executeReader()
					if(dr.read())then
						
						if(dr("cpf").toString() <> "")then
							checkout.CustomerIdentity = replace(replace(left(dr("cpf").toString(), 14),".",""),"-","")
						else
							checkout.CustomerIdentity = configurationSettings.appSettings("CustomerIdentity")
						end if
							
						if(dr("nome").tostring() <> "")then
							checkout.CustomerFullname = left(dr("nome").tostring(), 288)
						'else
						'	checkout.CustomerFullname = left(dr("nome_fantasia").tostring(), 288)
						end if
						
						if(dr("email").tostring() <> "")then
							checkout.CustomerEmail = left(dr("email").tostring(), 64)
						end if
						
						if(dr("telddd").tostring() <> "" Or dr("tel").tostring() <> "")then
							checkout.CustomerPhone = replace(replace(replace(replace(left(dr("telddd").tostring() & dr("tel").tostring(), 11), "-",""),"(",""),")","")," ","")
						end if

						if(dr("cep") <> "" )then 
							checkout.targetZipCode = replace(dr("cep").toString(), "-", "")
						else
							checkout.targetZipCode = configurationSettings.appSettings("targetZipCode")
						end if
						
						if(dr("endereco") <> "")then
							checkout.addressStreet = left(dr("endereco").toString(), 256)
						else
							checkout.addressStreet = configurationSettings.appSettings("addressStreet") 
						end if
						
						if(dr("numero") <> "")then
							checkout.addressNumber = left(dr("numero").tostring(), 8)
						else
							checkout.addressNumber = configurationSettings.appSettings("addressNumber")
						end if
						
						if(dr("complemento").tostring() <> "")then
							checkout.addressComplement = left(dr("complemento").tostring(), 256)
						else
							checkout.addressComplement = ""
						end if
						
						if(dr("cidade") <> "")then
							checkout.AddressDistrict = left(dr("cidade").tostring(), 64)
							checkout.AddressCity = left(dr("cidade").tostring(), 64)
						else
							checkout.AddressDistrict = configurationSettings.appSettings("AddressDistrict") 
							checkout.AddressCity = configurationSettings.appSettings("AddressCity") 
						end if
						
						if(dr("uf") <> "")then
							checkout.AddressState = left(dr("uf").tostring(), 2)
						else
							checkout.AddressState = configurationSettings.appSettings("AddressState") 
						end if
					end if
				end using
			end using
		End Function
		
		
		'[INICIO] Função para listar os dados fixos
		'------------------------------------------
		Public Function fDadosFixos()			
			checkout.merchantID = configurationSettings.appSettings("merchantID")	
			checkout.SoftDescriptor = left(configurationSettings.appSettings("SoftDescriptor")	, 13)
			checkout.cartType = configurationSettings.appSettings("cartType") 
			checkout.cartValue = configurationSettings.appSettings("cartValue")
			'checkout.shipType = configurationSettings.appSettings("shipType") 
			checkout.sourcezipcode = configurationSettings.appSettings("sourcezipcode") 
			checkout.OptionsAntifraudEnabled = configurationSettings.appSettings("OptionsAntifraudEnabled")
			checkout.itensType = "Asset"
			checkout.itensSku = "1"
			checkout.itensWeight = "100"
			
			checkout.ServiceName = configurationSettings.appSettings("ServiceName") 
			'checkout.ServicePrice = configurationSettings.appSettings("ServicePrice") 
			checkout.ServiceDeadline = configurationSettings.appSettings("ServiceDeadline") 
			checkout.PaymentBoletoDiscount = configurationSettings.appSettings("PaymentBoletoDiscount")
			checkout.PaymentDebitDiscount = configurationSettings.appSettings("PaymentDebitDiscount")
		End Function
		
		'[INICIO] Função auxiliar: formata o preco conforme o padrão
		'-------------------------------------------------------------
		Public function fPrecoFormatado(byval preco)
			retorno = "0"
			
			if(preco <> "")then
				retorno = replace(replace(preco,",",""),".","")
			end if
			
			return retorno
			
		end Function
		
		'[INICIO] Função auxiliar: verficar qual tipo de envio
		'------------------------------------------------------
		Public Function fFormaEnvio(byval formaEnvio)
			retorno = "Free"
			'Tipo do frete: “Correios”, “FixedAmount”, “Free”, “WithoutShippingPickUp”, “WithoutShipping”.
			if(formaEnvio = "1")then retorno = "FixedAmount"
			if(formaEnvio = "2")then retorno = "FixedAmount"
			if(formaEnvio = "3")then retorno = "FixedAmount"'"Correios"
			if(formaEnvio = "4")then retorno = "FixedAmount"'"Correios"
			
			return retorno
		
		End Function
		
		
		'[INICIO] funcao auxiliar: formata do tipo de pagamento
		'------------------------------------------------------
		Public function fFormaPagamento(byval tipo)
			retorno = ""
			
			if(tipo = 1)then retorno = "Cartão de Crédito"
			if(tipo = 2)then retorno = "Boleto Bancário"
			if(tipo = 3)then retorno = "Débito Online"
			if(tipo = 4)then retorno = "Cartão de Débito"
			
			return retorno
			
		end function
		
		'[FUNCAO] funcao auxiliar: formata o tipo de cartão utilizado
		'-------------------------------------------------------------
		Public function fBandeira(byval tipo)
			retorno = ""
			
			if(tipo = 1)then retorno = "Visa"
			if(tipo = 2)then retorno = "Mastercard"
			if(tipo = 3)then retorno = "American Express"
			if(tipo = 4)then retorno = "Diners"
			if(tipo = 5)then retorno = "Elo"
			if(tipo = 6)then retorno = "Aura"
			if(tipo = 7)then retorno = "JCB"
			
			return retorno
			
		end function
		
		'[INICIO] funcao status do cielo
		'-------------------------------
		public function fStatusCielo(byval estatus)
			dim retorno = ""
			
			if(estatus = "1")then retorno = "1:Pendente"
			if(estatus = "2")then retorno = "2:Pago"
			if(estatus = "3")then retorno = "3:Negado"
			if(estatus = "4")then retorno = "4:Expirado"
			if(estatus = "5")then retorno = "5:Cancelado"
			if(estatus = "6")then retorno = "6:Não Finalizado"
			if(estatus = "7")then retorno = "7:Autorizado"
			if(estatus = "8")then retorno = "8:Chargeback"
			
			return retorno
			
		end function 
		
		'[INICIO] limpeza
		'----------------
		Public function fLimpeza(byval r)
			dim retorno = r
			if(r <> "")then
				retorno = replace(replace(replace(replace(r, "'", ""), "--", ""), "-1", ""), "'OR", "") 
			end if
			return retorno
		end function 
		
	
	End Class
	
	
	
	'[INTERFACE]
	Public class iCheckout
		Implements IDisposable
	
		'[VARIAVEIS]
		private pOrderNumber
		private pSoftDescriptor 
		private pcartType
		private pcartValue
		private pitensName
		private pitensDescription
		private pitensUnitPrice
		private pitensQuantity
		private pitensType
		private pitensSku
		private pitensWeight
		private pshipType
		Private psourcezipcode
		Private ptargetZipCode
		Private paddressStreet
		Private paddressNumber
		Private paddressComplement
		Private pAddressDistrict
		Private pAddressCity
		Private pAddressState
		Private pServiceName
		Private pServicePrice
		Private pServiceDeadline
		Private pPaymentBoletoDiscount
		Private pPaymentDebitDiscount
		Private pCustomerIdentity
		Private pCustomerFullname
		Private pCustomerEmail
		Private pCustomerPhone
		Private pOptionsAntifraudEnabled
		private pmerchantID
		private pvalorCielo
		
		public property valorCielo as string
			get
				return pvalorCielo
			end get
			set (byval x as string)
				pvalorCielo = x
			end set
		end property
		
		public property merchantID as string
			get
				return pmerchantID
			end get
			set (byval x as string)
				pmerchantID = x
			end set
		end property
		
		public property OptionsAntifraudEnabled as string
			get
				return pOptionsAntifraudEnabled
			end get
			set (byval x as string)
				pOptionsAntifraudEnabled = x
			end set
		end property
		
		public property CustomerPhone as string
			get
				return pCustomerPhone
			end get
			set (byval x as string)
				pCustomerPhone = x
			end set
		end property
		
		public property CustomerEmail as string
			get
				return pCustomerEmail
			end get
			set (byval x as string)
				pCustomerEmail = x
			end set
		end property
		
		public property CustomerFullname as string
			get
				return pCustomerFullname
			end get
			set (byval x as string)
				pCustomerFullname = x
			end set
		end property
		
		public property CustomerIdentity as string
			get
				return pCustomerIdentity
			end get
			set (byval x as string)
				pCustomerIdentity = x
			end set
		end property
		
		public property PaymentDebitDiscount as string
			get
				return pPaymentDebitDiscount
			end get
			set (byval x as string)
				pPaymentDebitDiscount = x
			end set
		end property
		
		public property PaymentBoletoDiscount as string
			get
				return pPaymentBoletoDiscount
			end get
			set (byval x as string)
				pPaymentBoletoDiscount = x
			end set
		end property
		
		public property serviceDeadline as string
			get
				return pServiceDeadline
			end get
			set (byval x as string)
				pServiceDeadline = x
			end set
		end property
		
		public property ServicePrice as string
			get
				return pServicePrice
			end get
			set (byval x as string)
				pServicePrice = x
			end set
		end property
		
		public property ServiceName as string
			get
				return pServiceName
			end get
			set (byval x as string)
				pServiceName = x
			end set
		end property
		
		public property AddressState as string
			get
				return pAddressState
			end get
			set (byval x as string)
				pAddressState = x
			end set
		end property
		
		public property AddressCity as string
			get
				return pAddressCity
			end get
			set (byval x as string)
				pAddressCity = x
			end set
		end property
		
		public property AddressDistrict as string
			get
				return pAddressDistrict
			end get
			set (byval x as string)
				pAddressDistrict = x
			end set
		end property
		
		public property addressComplement as string
			get
				return paddressComplement
			end get
			set (byval x as string)
				paddressComplement = x
			end set
		end property
		
		public property addressNumber as string
			get
				return paddressNumber
			end get
			set (byval x as string)
				paddressNumber = x
			end set
		end property
		
		public property addressStreet as string
			get
				return paddressStreet
			end get
			set (byval x as string)
				paddressStreet = x
			end set
		end property
		
		public property targetZipCode as string
			get
				return ptargetZipCode
			end get
			set (byval x as string)
				ptargetZipCode = x
			end set
		end property
		
		public property sourcezipcode as string
			get
				return psourcezipcode
			end get
			set (byval x as string)
				psourcezipcode = x
			end set
		end property
		
		public property shipType as string
			get
				return pshipType
			end get
			set (byval x as string)
				pshipType = x
			end set
		end property
		
		public property itensWeight as string
			get
				return pitensWeight
			end get
			set (byval x as string)
				pitensWeight = x
			end set
		end property
		
		public property itensSku as string
			get
				return pitensSku
			end get
			set (byval x as string)
				pitensSku = x
			end set
		end property
		
		public property itensType as string
			get
				return pitensType
			end get
			set (byval x as string)
				pitensType = x
			end set
		end property
		
		public property itensQuantity as string
			get
				return pitensQuantity
			end get
			set (byval x as string)
				pitensQuantity = x
			end set
		end property
		
		public property itensUnitPrice as string
			get
				return pitensUnitPrice
			end get
			set (byval x as string)
				pitensUnitPrice = x
			end set
		end property
		
		public property itensDescription as string
			get
				return pitensDescription
			end get
			set (byval x as string)
				pitensDescription = x
			end set
		end property
		
		public property itensName as string
			get
				return pitensName
			end get
			set (byval x as string)
				pitensName = x
			end set
		end property
		
		public property cartValue as string
			get
				return pcartValue
			end get
			set (byval x as string)
				pcartValue = x
			end set
		end property
		
		public property cartType as string
			get
				return pcartType
			end get
			set (byval x as string)
				pcartType = x
			end set
		end property
		
		public property softDescriptor as string
			get
				return psoftDescriptor
			end get
			set (byval x as string)
				psoftDescriptor = x
			end set
		end property
		
		public property orderNumber as string
			get
				return pOrderNumber
			end get
			set (byval x as string)
				pOrderNumber = x
			end set
		end property
		
		'Sub New
		Public sub new
		end sub
		
		Public sub new(byval sitensName  as string, byval sitensDescription  as string, byval sitensUnitPrice  as string, byval sitensQuantity  as string, byval sitensType  as string, byval sitensSku  as string, byval sitensWeight  as string)
			
			itensName = sitensName
			itensDescription = sitensDescription
			itensUnitPrice = sitensUnitPrice
			itensQuantity = sitensQuantity
			itensType = sitensType
			itensSku = sitensSku
			itensWeight = sitensWeight
	
		end sub
		
		Public Sub Dispose() Implements System.IDisposable.Dispose
			OrderNumber = ""
			SoftDescriptor  = ""
			cartType = ""
			cartValue = ""
			itensName = ""
			itensDescription = ""
			itensUnitPrice = ""
			itensQuantity = ""
			itensType = ""
			itensSku = ""
			itensWeight = ""
			shipType = ""
			sourcezipcode = ""
			targetZipCode = ""
			addressStreet = ""
			addressNumber = ""
			addressComplement = ""
			AddressDistrict = ""
			AddressCity = ""
			AddressState = ""
			ServiceName = ""
			ServicePrice = ""
			ServiceDeadline = ""
			PaymentBoletoDiscount = ""
			PaymentDebitDiscount = ""
			CustomerIdentity = ""
			CustomerFullname = ""
			CustomerEmail = ""
			CustomerPhone = ""
			OptionsAntifraudEnabled = ""
			valorCielo = ""
		end sub
		
	end class
	
End Namespace


