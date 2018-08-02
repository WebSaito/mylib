Imports System
Imports System.IO
Imports System.Data
Imports System.Net
Imports System.Collections.Specialized


Namespace Cielo
	
	Public Class Cielo
		
		Public OrderNumber as String = "" 
		Public SoftDescriptor as String = ""
		
		
		
		'-----------------------------------------
		'Mapa: JSON-Checkout Cielo
		'{  
		'   "OrderNumber":"Pedido01",
		'   "SoftDescriptor":"Exemplo",
		'   "Cart":{  
		'	  "Discount":{  
		'		 "Type":"Percent",
		'		 "Value":00
		'	  },
		'	  "Items":[  
		'		 {  
		'			"Name":"Produto01",
		'			"Description":"ProdutoExemplo01",
		'			"UnitPrice":100,
		'			"Quantity":1,
		'			"Type":"Asset",
		'			"Sku":"ABC001",
		'			"Weight":500
		'		 },
		'		]
		'   },
		'   "Shipping":{  
		'	  "SourceZipCode":"20020080",
		'	  "TargetZipCode":"21911130",
		'	  "Type":"FixedAmount",
		'	  "Services":[  
		'		 {  
		'			"Name":"Motoboy",
		'			"Price":1,
		'			"Deadline":15,
		'			"Carrier":null
		'		 },
		'		 {  
		'			"Name":"UPS Express",
		'			"Price":1,
		'			"Deadline":2,
		'			"Carrier":null
		'		 }
		'	  ],
		'	  "Address":{  
		'		 "Street":"Rua Cambui",
		'		 "Number":"92",
		'		 "Complement":"Apto 201",
		'		 "District":"Freguesia",
		'		 "City":"Rio de Janeiro",
		'		 "State":"RJ"
		'	  }
		'   },
		'   "Payment":{  
		'	  "BoletoDiscount":15,
		'	  "DebitDiscount":10,
		'	  "Installments":null,
		'	  "MaxNumberOfInstallments": null
		'   },
		'   "Customer":{  
		'	  "Identity":"84261300206",
		'	  "FullName":"Test de Test",
		'	  "Email":"test@cielo.com.br",
		'	  "Phone":"21987654321"
		'   },
		'   "Options":{  
		'	 "AntifraudEnabled":true,
		'	 "ReturnUrl": "http://www.cielo.com.br"
		'   },
		'   "Settings":null
		'}
		
		
		
		'-----------------------------------------
		'Descrição dos Campos
		'OrderNumber	Alphanumeric	Opcional	64	Número do pedido da loja.	
		'SoftDescriptor	Alphanumeric	Opcional	13	Texto exibido na fatura do comprador. Sem caracteres especiais ou espaços. EX: Loja_ABC_1234	
		'Cart.Discount.Type	Alphanumeric	Condicional	255	Tipo do desconto a ser aplicado: Amount ou Percent.	Obrigatório caso Cart.Discount.Value for maior ou igual a zero.
		'Cart.Discount.Value	Numeric	Condicional	18	Valor do desconto a ser aplicado: Valor ou Percentual	Obrigatório caso Cart.Discount.Type for Amount ou Percent.
		'Cart.Items.Name	Alphanumeric	Sim	128	Nome do item no carrinho.	
		'Cart.Items.Description	Alphanumeric	Opcional	256	Descrição do item no carrinho.	
		'Cart.Items.UnitPrice	Numeric	Sim	18	Preço unitário do produto em centavos. Ex: R$ 1,00 = 100	
		'Cart.Items.Quantity	Numeric	Sim	9	Quantidade do item no carrinho.	
		'Cart.Items.Type	Alphanumeric	Sim	255	Tipo do item no carrinho: 
		'Asset
		'Digital
		'Service
		'Payment
		'Cart.Items.Sku	Alphanumeric	Opcional	32	Sku do item no carrinho.	
		'Cart.Items.Weight	Numeric	Condicional	9	Peso em gramas do item no carrinho.	Necessário caso Shipping.Type for “Correios”.
		'Payment.BoletoDiscount	Numeric	Condicional	3	Desconto, em porcentagem, para pagamentos a serem realizados com boleto.	
		'Payment.DebitDiscount	Numeric	Condicional	3	Desconto, em porcentagem, para pagamentos a serem realizados com débito online.	
		'FirstInstallmentDiscount	Numeric	Condicional	3	Desconto, em porcentagem, para pagamentos a serem realizados com crédito a vista.	
		'MaxNumberOfInstallments	Numeric	Condicional	2	Define valor máximo de parcelas apresentadas no transacional, ignorando configuração do Backoffice	
		'Customer.Identity	Numeric	Condicional	14	CPF ou CNPJ do comprador.	Não obrigatório na API, mas obrigatório na tela transacional
		'Customer.FullName	Alphanumeric	Condicional	288	Nome completo do comprador.	Não obrigatório na API, mas obrigatório na tela transacional
		'Customer.Email	Alphanumeric	Condicional	64	Email do comprador.	Não obrigatório na API, mas obrigatório na tela transacional
		'Customer.Phone	Numeric	Condicional	11	Telefone do comprador.	Não obrigatório na API, mas obrigatório na tela transacional
		'Options.AntifraudEnabled	Boolean	Condicional	n/a	Habilitar ou não a análise de fraude para o pedido: true ou false.	
		'Options.ReturnUrl	Strin	Condicional	255	Define para qual url o comprador será enviado após finalizar a compra.	Uma URL fixa pode ser registrada no Backoffice Checkout
		
		
		
	End Class
	
End Namespace