Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
imports System.Net
imports System.IO
imports System.Text
imports System.Web
Imports System.XML
Imports SwebSender

Namespace SLGoogleMaps
	Public Class GoogleMaps
		
		'Objetos publicos
		Public oWP As New webPOST()
		Public erro As String = ""
		
		'Propriedades Estrutura
		Public key As String = ""
		
		'Propriedades Mapa Exibição Estatico
		Public mapWidth As String = "300"
		Public mapHeight As String = "300"
		Public mapCenter As String = "-15.707663,-53.789062"
		Public mapZoom As String = "4"
		Public mapType As String = "roadmap"
		Public mapMarkers As New Hashtable()
		Public mapMIcon As String = ""
		Public mapMarkersIcon As New Hashtable()
		
		'Propriedades Geocoding

 		Public geoStatus As String = ""
		'Valore de Status:
			'"OK" Sem erros
			'"ZERO_RESULTS" sem resultados
			'"OVER_QUERY_LIMIT" sem limite da cota
			'"REQUEST_DENIED" indicates that your request was denied, generally because of lack of a sensor parameter.
			'"INVALID_REQUEST" generally indicates that the query (address or latlng) is missing.
			
			'Resultados Geocoding
			Public geoRTypes As New Hashtable()
			Public geoRFAddress As New Hashtable()
			Public geoRNumero As Integer = 0
			Public geoRAddressComponets As New Hashtable() 'Indice 0:0 , 0:1, 0:2, ... 1:0, 1:2 ,...
			Public geoRLocation As New Hashtable() 'Indice 0,1,2 - LatLng (0000,00000)
			Public geoRVieportSW As New Hashtable() 'Indice 0,1,2 - LatLng (00000,00000)
			Public geoRVieportNE As New Hashtable() 'Indice 0,1,2 - LatLng (00000,00000)	
		
		'Resultados de pesquisa de endereco
		'Objeto de dados
		Public dadosEndereco As List(Of dadosConsulta) = new List(Of dadosConsulta)
		
		Public dadosPC As List(Of dadosCEP) = new List(Of dadosCEP)
		
		Public srcType As String = "(regions)"
		'geocode - instructs the Place Autocomplete service to return only geocoding (address) results. Generally, you use this request to disambiguate results where the location specified may be indeterminate.
		'establishment - instructs the Place Autocomplete service to return only business results.
		'(regions) - type collection instructs the Places service to return any result matching the following types:
		'locality
		'sublocality
		'postal_code
		'country
		'administrative_area_level_1
		'administrative_area_level_2
		'(cities) - type collection instructs the Places service to return results that match locality or administrative_area_level_3.
		
		Public sRef As String = ""
		Public sLat As String = ""
		Public sLng As String = ""
		Public sID As String = ""
		Public sCEP As String = ""
		
		'Construtor
		Public Function GoogleMaps() As String
			Dim r As String = ""
				'Construtor vazio			
			return r
		End Function
		
		'Construtor com chave
		Public Function GoogleMaps(ByVal apiKey As String) As String
			Dim r As String = ""
			
			Try
				key = apiKey
			Catch ex As Exception
				r = ex.ToString()
				erro &= "<b>GoogleMaps:</b>"& ex.ToString() &"<br><br>"
			End Try
			
			return r
		End Function
		
		'Renderizando mapa
		Public Function renderizaMapa() As String
			Dim r As String = ""
			
			Try
				'Marcadores
				Dim mPersonalizados As String = formataMarcadores("icones")
				Dim mPadrao As String = formataMarcadores("padrao")
				
				Dim url As String = "http://maps.googleapis.com/maps/api/staticmap?center="& mapCenter &"&zoom="& mapZoom &"&size="& mapWidth &"x"& mapHeight &"&maptype="& mapType & mPersonalizados & mPadrao &"&sensor=false"'& key
				
				r = url'oWP.executar(url)
				
			Catch ex As Exception
				r = ex.ToString()
				erro &= "<b>renderizaMapa:</b>"& ex.ToString() &"<br><br>"
			End Try
			
			return r
		End Function
		
		Public Function adicionaMarkerIcon(ByVal local As String) As String
			Dim r As String = ""
			
			Try
				Dim x As Integer = mapMarkersIcon.Count + 1
				
				Dim auxStr As String = "&markers="
				auxStr &= "icon:"& mapMIcon
				auxStr &= "%7C"& rLocal(local)
				
				mapMarkersIcon.add(x, auxStr)
			Catch ex As Exception
				r = ex.ToString()
				erro &= "<b>adicionaMarkerIcon:</b>"& ex.ToString() &"<br><br>"
			End Try
			
			return r
		End Function
		
		Public Function adicionaMarker(ByVal local As String) As String
			Dim r As String = ""
			
			Try
				Dim x As Integer = mapMarkersIcon.Count + 1
				
				Dim auxStr As String = "&markers="
				auxStr &= "icon"& mapMIcon
				auxStr &= "%7C"& rLocal(local)
				
				mapMarkersIcon.add(x, auxStr)
			Catch ex As Exception
				r = ex.ToString()
				erro &= "<b>adicionaMarker:</b>"& ex.ToString() &"<br><br>"
			End Try
			
			return r
		End Function
		
		Public Function formataMarcadores(ByVal tipo As String) As String
			Dim r As String = ""
			Dim Items As DictionaryEntry
			
			Try
				
				If tipo = "icones" Then
					If mapMarkersIcon.Count > 0 Then
						For each Items in mapMarkersIcon
							r &= Items.Value
						Next
					End If
				End If
				
				If tipo = "padrao" Then
					If mapMarkers.Count > 0 Then
						For each Items in mapMarkers
							r &= Items.Value
						Next
					End If
				End If
				
			Catch ex As Exception
				r = ex.ToString()
				erro &= "<b>formataMarcadores:</b>"& ex.ToString() &"<br><br>"
			End Try
			
			return r
		End Function
		
		Private Function rLocal(ByVal valor As String) As String
			Dim r As String = ""
			
			Try
				r = HttpUtility.URLEncode(Replace(valor," ","+"))
			Catch ex As Exception
				r = ex.ToString()
				erro &= "<b>rLocal:</b>"& ex.ToString() &"<br><br>"
			End Try
			
			return r
		End Function
		
		'https://maps.googleapis.com/maps/api/place/autocomplete/output?parameters
		Public Function consultaCEP(ByVal cep As String) As String
			Dim r As String = ""
			
			dadosPC.Clear()
			
			Try
			
				Dim aux As String = ""
				Dim dados As String = ""
				Dim url As String = "https://maps.googleapis.com/maps/api/place/autocomplete/xml?input="& cep &"&types="& srcType &"&language=pt_BR&key="& key
				Dim referencia As String = ""
				
				aux = oWP.executar(url)
				dados = ""& trim(aux.ToString())
								
				Dim doc As XmlDocument = new XmlDocument()
				doc.LoadXml(dados)
								
				Dim Elem As XmlElement = doc.DocumentElement
				Dim x As Integer = 0
				Dim y As Integer = 0
				
				geoStatus = Elem.SelectSingleNode("status").InnerText
				
				if geoStatus = "OK" then
					
					
					
					Dim xNL As XmlNodeList = doc.GetElementsByTagName("prediction")
					Dim xNL2 As XmlNodeList
					
					geoRNumero = xNL.Count
					
					Dim xLem As XmlNode
					Dim xLem2 As XmlNode
					'dim xLem3 As XmlNode
					
					For Each xLem in xNL
							
							referencia = xLem.SelectSingleNode("reference").InnerText
							
							'r &= "<a href='javascript://' onclick="& chr(34) &"selecionaLatLng('"& xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText &","& xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText &"','"& idCampo &"')"& chr(34) &">"&  xLem.SelectSingleNode("formatted_address").InnerText &"</a><br/>"
							
							'dadosEndereco.Add(new dadosConsulta(xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText, xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText, xLem.SelectSingleNode("formatted_address").InnerText))

						x = x + 1
						
					Next
				
				End If
				
				r = dados &"<br><br>"& detalhesRef(referencia)
				
			Catch ex As Exception
				r = ex.ToString()
			End Try
			
			return r
		End Function	
		
		Public Function detalhesRef(ByVal ref As String) As String
			Dim r As String = ""
				'https://maps.googleapis.com/maps/api/place/details/xml?reference=CmRYAAAAciqGsTRX1mXRvuXSH2ErwW-jCINE1aLiwP64MCWDN5vkXvXoQGPKldMfmdGyqWSpm7BEYCgDm-iv7Kc2PF7QA7brMAwBbAcqMr5i1f4PwTpaovIZjysCEZTry8Ez30wpEhCNCXpynextCld2EBsDkRKsGhSLayuRyFsex6JA6NPh9dyupoTH3g&sensor=false&key=YOUR_API_KEY
				Try
			
				Dim aux As String = ""
				Dim dados As String = ""
				Dim url As String = "https://maps.googleapis.com/maps/api/place/details/xml?reference="& ref &"&sensor=false&key="& key
				Dim referencia As String = ""
				
				aux = oWP.executar(url)
				dados = ""& trim(aux.ToString())
								
				Dim doc As XmlDocument = new XmlDocument()
				doc.LoadXml(dados)
								
				Dim Elem As XmlElement = doc.DocumentElement
				Dim x As Integer = 0
				Dim y As Integer = 0
				
				geoStatus = Elem.SelectSingleNode("status").InnerText
				
				if geoStatus = "OK" then
					
					
					
					Dim xNL As XmlNodeList = doc.GetElementsByTagName("result")
					Dim xNL2 As XmlNodeList
					
					geoRNumero = xNL.Count
					
					Dim xLem As XmlNode
					Dim xLem2 As XmlNode
					'dim xLem3 As XmlNode
					
					For Each xLem in xNL
							
							sRef = xLem.SelectSingleNode("reference").InnerText
							sLat = xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText
							sLng = xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText
							sID = xLem.SelectSingleNode("id").InnerText
							
							'r &= "<a href='javascript://' onclick="& chr(34) &"selecionaLatLng('"& xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText &","& xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText &"','"& idCampo &"')"& chr(34) &">"&  xLem.SelectSingleNode("formatted_address").InnerText &"</a><br/>"
							
							'dadosEndereco.Add(new dadosConsulta(xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText, xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText, xLem.SelectSingleNode("formatted_address").InnerText))

						x = x + 1
						
					Next
				
				End If
				
				r = dados
				
			Catch ex As Exception
				r = ex.ToString()
			End Try
			
			return r
		End Function
		
		Public Function consultaEndereco(ByVal valor As String, ByVal idCampo As String) As String
			Dim r As String = ""
			
			Try
				
				dadosEndereco.Clear()
				
				Dim aux As String = ""
				Dim dados As String = ""
				Dim url As String = "http://maps.googleapis.com/maps/api/geocode/xml?address="& rLocal(valor) &"&sensor=true"

				
				aux = oWP.executar(url)
				dados = ""& trim(aux.ToString())
								
				Dim doc As XmlDocument = new XmlDocument()
				doc.LoadXml(dados)
								
				Dim Elem As XmlElement = doc.DocumentElement
				Dim x As Integer = 0
				Dim y As Integer = 0
				
				geoStatus = Elem.SelectSingleNode("status").InnerText
				
				If geoStatus = "OK" Then
					
					Dim xNL As XmlNodeList = doc.GetElementsByTagName("result")
					Dim xNL2 As XmlNodeList
					
					geoRNumero = xNL.Count
					
					Dim xLem As XmlNode
					Dim xLem2 As XmlNode
					'dim xLem3 As XmlNode
					
					For Each xLem in xNL
						
						'teste &= "<br>"&  xLem.ChildNodes.Item(x).InnerText
						
						'For Each xLem2 in  xLem							
							'geoRTypes.add(geoRNumero, xLem2.SelectSingleNode("type").InnerText)
							'geoRFAddress.add(geoRNumero, xLem2.SelectSingleNode("formatted_address").InnerText)
							
							'If xLem.ChildNodes.Item(x).Name = "result" Then	
							'	For Each xLem3 in  xLem2.SelectSingleNode("address_component")
									
							'	Next
							'End If
							r &= "<a href='javascript://' onclick="& chr(34) &"selecionaLatLng('"& xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText &","& xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText &"','"& idCampo &"')"& chr(34) &">"&  xLem.SelectSingleNode("formatted_address").InnerText &"</a><br/>"
							
					'		y = y + 1
						'Next
							dadosEndereco.Add(new dadosConsulta(xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText, xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText, xLem.SelectSingleNode("formatted_address").InnerText))
						
						
						
					'	If xLem.ChildNodes.Item(x).LocalName = "result" Then
							
					'		geoRNumero = geoRNumero + 1
					'	End If
						
						x = x + 1
						
					Next
					
				End If
				
				r &= "<script type='text/javascript'>function selecionaLatLng(v1,v2){try{document.getElementById(v2).value=v1}catch(ex){}}</script>"
				
			Catch ex As Exception
				r = ex.ToString()
			End Try
			
			return r
		End Function
		
		
		
		Public Function consultaEndereco(ByVal valor As String) As String
			Dim r As String = ""
			
			Try
				
				dadosEndereco.Clear()
				
				Dim aux As String = ""
				Dim dados As String = ""
				Dim url As String = "http://maps.googleapis.com/maps/api/geocode/xml?address="& rLocal(valor) &"&sensor=true"

				
				aux = oWP.executar(url)
				dados = ""& trim(aux.ToString())
								
				Dim doc As XmlDocument = new XmlDocument()
				doc.LoadXml(dados)
								
				Dim Elem As XmlElement = doc.DocumentElement
				Dim x As Integer = 0
				Dim y As Integer = 0
				
				geoStatus = Elem.SelectSingleNode("status").InnerText
				
				If geoStatus = "OK" Then
					
					Dim xNL As XmlNodeList = doc.GetElementsByTagName("result")
					Dim xNL2 As XmlNodeList
					
					geoRNumero = xNL.Count
					
					Dim xLem As XmlNode
					Dim xLem2 As XmlNode
					'dim xLem3 As XmlNode
					
					For Each xLem in xNL
						
						'teste &= "<br>"&  xLem.ChildNodes.Item(x).InnerText
						
						'For Each xLem2 in  xLem							
							'geoRTypes.add(geoRNumero, xLem2.SelectSingleNode("type").InnerText)
							'geoRFAddress.add(geoRNumero, xLem2.SelectSingleNode("formatted_address").InnerText)
							
							'If xLem.ChildNodes.Item(x).Name = "result" Then	
							'	For Each xLem3 in  xLem2.SelectSingleNode("address_component")
									
							'	Next
							'End If
							'r &= "<a href='javascript://' onclick="& chr(34) &"selecionaLatLng('"& xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText &","& xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText &"','"& idCampo &"')"& chr(34) &">"&  xLem.SelectSingleNode("formatted_address").InnerText &"</a><br/>"
							
					'		y = y + 1
						'Next
							dadosEndereco.Add(new dadosConsulta(xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lat").InnerText, xLem.SelectSingleNode("geometry").SelectSingleNode("location").SelectSingleNode("lng").InnerText, xLem.SelectSingleNode("formatted_address").InnerText))
						
						
						
					'	If xLem.ChildNodes.Item(x).LocalName = "result" Then
							
					'		geoRNumero = geoRNumero + 1
					'	End If
						
						x = x + 1
						
					Next
					
				End If
				
				r &= "<script type='text/javascript'>function selecionaLatLng(v1,v2){try{document.getElementById(v2).value=v1}catch(ex){}}</script>"
				
			Catch ex As Exception
				r = ex.ToString()
			End Try
			
			return r
		End Function
		
		
		Public Function jDados() As String
			Dim r As String = ""
			
			Try
				r = "{dados:["
				
				For each y As dadosConsulta in dadosEndereco
					r &= "{lat:"& y.lat &", long:"& y.lng &", addr:'"& y.endereco &"'}"
				Next
				
				r &= "]}"
				
			Catch ex As Exception
				r = ex.ToString()
			End Try
			
			return r
		End Function
		
		Public Function aDados() As String
			Dim r As String = ""
			
			Try
				
				For each y As dadosConsulta in dadosEndereco
					If r = "" then
						r &= "lat:"& y.lat &"|long:"& y.lng &"|addr:'"& y.endereco &""
					Else
						r &= "#lat:"& y.lat &"|long:"& y.lng &"|addr:'"& y.endereco &""
					End If
				Next
				
			Catch ex As Exception
				r = ex.ToString()
			End Try
			
			
			return r
		End Function
		
	End Class
	
	Public Class dadosCEP
		Private sEndereco As String = ""
		Private sCidade As String = ""
		Private sBairro As String = ""
		Private sEstado As String = ""
		Private sPais As String = ""
		
		'Endereco
		Public Property endereco() As String
			Get
				return sEndereco
			End Get
			Set(ByVal v As String)
				sEndereco = v
			End Set
		End Property
		
		'Cidade
		Public Property cidade() As String
			Get
				return sCidade
			End Get
			Set(ByVal v As String)
				sCidade = v
			End Set
		End Property
		
		Public Property bairro() As String
			Get
				return sBairro
			End Get
			Set(ByVal v As String)
				sBairro = v
			End Set
		End Property
		
		Public Property estado() As String
			Get
				return sEstado
			End Get
			Set(ByVal v As String)
				sEstado = v
			End Set
		End Property
		
		Public Property pais() As String
			Get
				return sPais
			End Get
			Set(ByVal v As String)
				sPais = v
			End Set
		End Property
		
		Public Sub New(ByVal en As String, ByVal ba As String, ByVal ci As String, ByVal es As String, ByVal pa As String)
			
			endereco = en
			bairro = ba
			cidade = ci
			estado = es
			pais = pa
			
		End Sub
		
	End Class
	
	Public Class dadosConsulta
		
		Private sLAT As String = ""
		Private sLON As String = ""
		Private sENDERECO As String = ""
		
		'Latitude
		Public Property lat() As String
			Get
				return sLAT
			End Get
			Set(ByVal v As String)
				sLAT = v
			End Set
		End Property
		
		'Longitude
		Public Property lng() As String
			Get
				return sLON
			End Get
			Set(ByVal v As String)
				sLON = v
			End Set
		End Property
		
		Public Property endereco() As String
			Get
				return sENDERECO
			End Get
			Set(ByVal v As String)
				sENDERECO = v
			End Set
		End Property
		
		Public Sub New(ByVal lt As String, ByVal ln As String, ByVal en As String)
			
			lat = lt
			lng = ln
			endereco = en
			
		End Sub
		
		
		
	End Class
End Namespace