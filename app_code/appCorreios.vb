Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
imports System.Net
imports System.IO
imports System.Text
imports System.Web
Imports System.XML

Namespace nsCorreios
	Public Class clsCorreios
		
		'---------------------------------
		'Atributos dados de envio
		'---
		Public nCdEmpresa As String = "" 'C'digo administrativo junto à ECT. O código esta disponível no corpo do contrato firmado com os Correios
		Public sDsSenha As String = "" 'Senha para acesso ao serviço, associada ao seu código administrativo. A senha inicial corresponde aos
		'8 primeiros dígitos do CNPJ informado no contrato. A qualquer momento, é possível alterar a senha no
		'endereço:  http://www.corporativo.correios.com.br/encomendas/servicosonline/recuperaSenha
		Public nCdServico As String = "04510"
		'//Desde 05/05/2017 ele foi substituido pelo 04510.
		'04014 SEDEX
		'04510 PAC
		'//DESATUALIZADO
		'41106 PAC Varejo
		'40290 SEDEX Hoje Varejo
		'40215 SEDEX 10 Varejo
		'40045 SEDEX a Cobrar Varejo
		'40010 SEDEX Varejo
		Public sCepOrigem As String = "" 'Sem hífen
		Public sCepDestino As String = "" 'Sem hífen
		Public nVlPeso As String = ""
		'Peso da encomenda, incluindo sua emalagem. O peso deve ser informado em quilogramas. Se o formato for Envelope, o máximo permitido sera 1kg.
		Public nCdFormato As Integer 'Formato da encomenda (incluindo embalagem)
		'Valores possíveis 1, 2 ou 3
		'1 - Formato caixa/pacote
		'2 - Formato rolo/prisma
		'3 - Envelope
		Public nVlComprimento As Double 'Comprimento da encomenda (incluindo embalagem), em centímetros.
		Public nVlAltura As Double = 0 'Altura da encomenda (incluind embalagem), em centímetros. Se o formato for envelope, informar zero(0).
		Public nVlLargura As Double 'Largura da encomenda (incluindo embalagem), em centímetros.
		Public nVlDiametro As Double 'Diâmetro da encomenda (incluindo embalagem), em centímetros.
		Public sCdMaoPropria As String = "" 'Indica se a encomenda será entregue com o serviço adicional mão própria. 
		'Valores possíveis: S ou N  ( Sim ou Não )
		Public nVlValorDeclarado As Double = 0 'Indica se a encomenda será entregue com o serviço adicional valor declarado. Neste campo
		'deve ser apresentado o valor declarado desejado, em Reais.
		Public sCdAvisoRecebimento As String = "" 'Indica se a encomenda sera entregue com o servico adicional aviso de recebimento
		'Valores possíveis: S ou N (Sim ou Não)
		
		
		'-----------------------------------------------
		'Atributo dados de retorno
		Public rCodigo As String = ""
		Public rValor As String = ""
		Public rPrazoEntrega As String = ""
		Public rValorMaoPropria As String = ""
		Public rValorAvisoRecebimento As String = ""
		Public rValorValorDeclarado As String = ""
		Public rEntregaDomiciliar As String = ""
		Public rEntregaSabado As String = ""
		Public rErro As String = ""
		Public rMsgErro As String = ""
		Public rValorSemAdicionais As String = "" 
		
		
		'-----------------------------------------------
		'Atributo que armazena os logs de erro da classe
		Public erros As String = ""
		Public debugURL as String = ""
		
		'Inicializador vazio		
		Public Sub New()
			'[s][a][i][t][o]
		End Sub
		
		'Init com parametros
		Public Function clsCorreios(ByVal codigo As String, ByVal senha As String) As String
			Dim resultado As String = ""
			
			Try
			
				nCdEmpresa = codigo
				sDsSenha = senha
				
			Catch ex As Exception
				resultado = ex.ToString()
				erros &= "54 - clsCorreios:"& ex.ToString() &"<br/><br/>"
			End Try
			
			return resultado
		End Function
		
		Public Function CalcPrecoPrazo() As String
			Dim resultado As String = ""
			
			Dim parametros As String = ""
			Dim aux As String = ""

			parametros &= "&nCdEmpresa="& nCdEmpresa
			parametros &= "&sDsSenha="& sDsSenha
			parametros &= "&nCdServico="& nCdServico
			parametros &= "&sCepOrigem="& sCepOrigem
			parametros &= "&sCepDestino="& sCepDestino
			parametros &= "&nVlPeso="& nVlPeso
			parametros &= "&nCdFormato="& nCdFormato
			parametros &= "&nVlComprimento="& nVlComprimento
			parametros &= "&nVlAltura="& nVlAltura
			parametros &= "&nVlLargura="& nVlLargura
			parametros &= "&nVlDiametro="& nVlDiametro
			parametros &= "&sCdMaoPropria="& sCdMaoPropria
			parametros &= "&nVlValorDeclarado="& nVlValorDeclarado
			parametros &= "&sCdAvisoRecebimento="& sCdAvisoRecebimento 
			
			Try
				Dim u As String = "http://ws.correios.com.br/calculador/CalcPrecoPrazo.asmx/CalcPrecoPrazo?"& parametros
				Dim w As System.Net.WebClient = new System.Net.WebClient()
				Dim tb() as Byte
				
				debugURL = u
				
				tb = w.DownloadData(u)
				aux = System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(tb)
				
				w.Dispose()
				
				Dim dados As String = ""& trim(aux.ToString())
				
				
				Dim doc As XmlDocument = new XmlDocument()
				doc.LoadXml(trim(dados))
				Dim node As XmlNode = doc.DocumentElement
				
				
				rCodigo = node("Servicos")("cServico")("Codigo").InnerText
				rValor = node("Servicos")("cServico")("Valor").InnerText
				rPrazoEntrega = node("Servicos")("cServico")("PrazoEntrega").InnerText
				rValorMaoPropria = node("Servicos")("cServico")("ValorMaoPropria").InnerText
				rValorAvisoRecebimento = node("Servicos")("cServico")("ValorAvisoRecebimento").InnerText
				rValorValorDeclarado = node("Servicos")("cServico")("ValorValorDeclarado").InnerText
				rEntregaDomiciliar = node("Servicos")("cServico")("EntregaDomiciliar").InnerText
				rEntregaSabado = node("Servicos")("cServico")("EntregaSabado").InnerText
				rErro = node("Servicos")("cServico")("Erro").InnerText
				rMsgErro = node("Servicos")("cServico")("MsgErro").InnerText
				rValorSemAdicionais = node("Servicos")("cServico")("ValorSemAdicionais").InnerText
				
			Catch ex As Exception
				resultado = ex.ToString()
				erros &= "184 - CalcPrecoPrazo:"& ex.ToString() &"<br/><br/>"
			End Try
						
			return resultado
		End Function
		
	End Class
End Namespace