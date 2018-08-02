Imports System
Imports System.Web
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.Collections

Namespace Spacelab.Upload
    ' CLASSE Upload
	' Metodos de envio local
    Public Class Upload
	    
		'##[ gravaImagem ]
		'# Método que grava imagem
		'# Parametros:
		'# sFilename - Nome do arquivo, nome que sera gravado
		'# bytFileByteArr - Array dos Bytes do arquivo
		'# sDir - Diretorio onde será gravado o arquivo deve conter barras(/)
		'# sCopia - Tipo de gravação  1 = redimensiona pela largura , 2 = cria uma copia p_, 3 cria N copias, com N prefixos
		'# sTamanho - tamanho da dimensao que deseja usar em pixel
		'# sPrefixo - prefixo ou diretorios onde serao armazenados os arquivos.
		Public Function gravaImagem(ByVal sFileName As String, _
		ByVal bytFileByteArr As Byte(), ByVal sDir As String, ByVal sCopia As String, ByVal sTamanho As String, ByVal sPrefixo As String) As String
			
			Dim wP as String = ""
			Dim hP as String = ""
			Dim wO as String = ""
			Dim hO as String = ""
			
			Dim pathDados as String = "" '"\..\Dados"
			
			Dim sFileType As String
			sFileType = System.IO.Path.GetExtension(sFileName)
			sFileType = sFileType.ToLower()
			
			Dim strFile As String = _
			System.IO.Path.GetFileNameWithoutExtension(sFileName)
			strFile = System.Web.Hosting.HostingEnvironment.MapPath _
			("~") & pathDados &  sDir  & strFile & sFileType
			
			
			'2016
			'Verificando se diretorio existe
			if(Not System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados &  sDir))then
				System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados &  sDir)
			end if
			
			
			Try
				
				Dim stream As New FileStream(strFile, FileMode.OpenOrCreate) 'FileMode.CreateNew
				stream.Write(bytFileByteArr, 0, bytFileByteArr.Length)
				stream.Close()
				
				'--------------------------------------------------------------------------------------------
	            '[QUADRADA]
				if sCopia = "quadrada" then
					
				    Dim strFilename As String
					Dim TImage As System.Drawing.Image
					
					'strFilename = System.Web.Hosting.HostingEnvironment.MapPath("~"& sDir & sFileName)
					strFilename = System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados & sDir & sFileName
					'-----------------------------------
					'Copiando original para pasta /o/
					'2016
					'Verificando se diretorio \o\ existe
					if(Not System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados &  sDir &"o\"))then
						System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados &  sDir &"o\")
					end if
					
					Dim strCopiaOriginal As String
					strCopiaOriginal = System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados & sDir &"\o\"& sFileName
					File.Copy(strFilename, strCopiaOriginal)
					'-----------------------------------
					
					Dim b as Bitmap
					Dim g as Graphics
					Dim width As Integer
					Dim height As Integer
					Dim stringSave as String
					
                    stringSave = left(strFilename,(len(strFilename)-len(sFileName)))
					TImage = System.Drawing.Image.FromFile(strFilename)
					
					'Carregando dados para retornar
					wO = TImage.Width
					hO = TImage.Height
					
					Dim dimensoes() as String
					Dim prefixos() as String
					Dim eixos() as String
					Dim x as Integer
					Dim posX as Integer = 0
					Dim posY as Integer = 0
					
					dimensoes = split(sTamanho, "|")
					prefixos = split(sPrefixo, "|")
					
					For x=0 to ubound(dimensoes)
						
						eixos = split(dimensoes(x),":")
						posX = 0
						posY = 0
						width = 0
						height = 0
						
						'Setando objetos
						'Desenhando o tamanho do quadro
						b = new System.Drawing.Bitmap(cInt(eixos(0)), cInt(eixos(1)), PixelFormat.Format32bppRgb)
						b.SetResolution(72,72)
						g = Graphics.FromImage(b)
						g.Clear(Color.White)
						g.CompositingQuality = CompositingQuality.HighQuality
						g.SmoothingMode = SmoothingMode.HighQuality
						g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
						g.InterpolationMode = InterpolationMode.HighQualityBicubic
						
						'Verificando se imagem tiver altura maior do que largura
						If TImage.height > TImage.width then
						
							'Definindo largura
							If TImage.width > cInt(eixos(0)) then
								'Caso imagem original seja maior
								width = cInt(eixos(0)) + 4 'Mais 4 pixels para evitar bordas brancas na reduçao de imagem
								posX = -2
							Else
								If Timage.width < cInt(eixos(0)) then
									'Caso imagem original seja menor , entao descobrir a diferença para alterar posX
									width = TImage.width
									posX = cInt((cInt(eixos(0)) - width)/2)
								Else
									'Caso seja igual
									width = cInt(eixos(0))
									posX = 0
								End If
							End If
							
							height = cInt((TImage.Height * width) / TImage.Width)
							posY = cInt((cInt(eixos(1)) - height)/2)
							
						Else
							
							If TImage.width > TImage.height then
								'Caso imagem tenha largura maior do que altura
								
								'Definindo altura
								If TImage.height > cInt(eixos(1)) then
									'Caso imagem original seja maior
									height = cInt(eixos(1)) + 4
									posY = -2
								Else
									If TImage.height < cInt(eixos(1)) then
										'Caso imagem original seja menor
										height = TImage.height
										posY = cInt((cInt(eixos(1)) - height)/2)
									Else
										height = cInt(eixos(1))
										posY = 0
									End If
								End If
								
								width = cInt((TImage.Width * height) / TImage.Height)
								posX = cInt((cInt(eixos(0)) - width)/2)
								
								
							Else
								'Caso imagem seja quadrada
								
								If TImage.width > cInt(eixos(0)) then
									width = cInt(eixos(0)) + 4
									height = cInt(eixos(1)) + 4
									posX = -2
									posY = -2
								Else
									If TImage.width < cInt(eixos(0)) then
										width = TImage.width
										height = TImage.height
										posX = cInt((cInt(eixos(0)) - width)/2)
										posY = cInt((cInt(eixos(1)) - height)/2)
									Else
										width = eixos(0)
										height = eixos(0)
										posX = 0
										posY = 0
									End If
								End if
								
							End If
							
						End If
						
						'Desenhando a imagem dentro do quadro
						g.DrawImage(TImage, posX, posY, width, height)
						
						select case sFileType
							case ".jpg"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Jpeg)
							case ".gif"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Gif)
							case ".png"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Png)
							case ".bmp"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Bmp)
						end select
						
					Next
					
					
					
					b.Dispose()
					g.Dispose()

					TImage.Dispose()
				   
					Timage = nothing
					b = nothing
					g = nothing					
					
					'Excluindo a imagem original
					Dim lfoto As String = strFilename
					File.Delete(lfoto)
					
				end if
	            '[FIM QUADRADA]
				'--------------------------------------------------------------------------------------------
				
				
				
				
				
				
				
				
				
				'--------------------------------------------------------------------------------------------
	            '[Altura Fixa]
				if sCopia = "alturaFixa" then
					
				    Dim strFilename As String
					Dim TImage As System.Drawing.Image
					
					'strFilename = System.Web.Hosting.HostingEnvironment.MapPath("~"& sDir & sFileName)
					strFilename = System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados & sDir & sFileName
					'-----------------------------------
					'Copiando original para pasta /o/
					'2016
					'Verificando se diretorio \o\ existe
					if(Not System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados &  sDir &"o\"))then
						System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados &  sDir &"o\")
					end if
					
					Dim strCopiaOriginal As String
					strCopiaOriginal = System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados & sDir &"o\"& sFileName
					File.Copy(strFilename, strCopiaOriginal)
					'-----------------------------------
					
					Dim b as Bitmap
					Dim g as Graphics
					Dim width As Integer
					Dim height As Integer
					Dim stringSave as String
					
                    stringSave = left(strFilename,(len(strFilename)-len(sFileName)))
					TImage = System.Drawing.Image.FromFile(strFilename)
					
					wO = TImage.Width
					hO = TImage.Height
					
					Dim dimensoes() as String
					Dim prefixos() as String
					Dim eixos() as String
					Dim x as Integer
					Dim posX as Integer = 0
					Dim posY as Integer = 0
					
					dimensoes = split(sTamanho, "|")
					prefixos = split(sPrefixo, "|")
					
					For x=0 to ubound(dimensoes)
						
						eixos = split(dimensoes(x),":")
						posX = 0
						posY = 0
						width = 0
						height = cInt(eixos(1))
						width = cint((TImage.Width * height) / TImage.Height)
						
						wP = width
						
						'Setando objetos
						'Desenhando o tamanho do quadro
						b = new System.Drawing.Bitmap(width, height, PixelFormat.Format32bppRgb)
						b.SetResolution(72,72)
						g = Graphics.FromImage(b)
						g.Clear(Color.White)
						g.CompositingQuality = CompositingQuality.HighQuality
						g.SmoothingMode = SmoothingMode.HighQuality
						g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
						g.InterpolationMode = InterpolationMode.HighQualityBicubic
						
						
						
						'Desenhando a imagem dentro do quadro
						g.DrawImage(TImage, posX, posY, width, height)
						
						select case sFileType
							case ".jpg"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Jpeg)
							case ".gif"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Gif)
							case ".png"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Png)
							case ".bmp"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Bmp)
						end select
						
					Next
					
					
					
					b.Dispose()
					g.Dispose()

					TImage.Dispose()
				   
					Timage = nothing
					b = nothing
					g = nothing					
					
					'Excluindo a imagem original
					Dim lfoto As String = strFilename
					File.Delete(lfoto)
					
				end if
	            '[FIM Altura Fixa]
				'--------------------------------------------------------------------------------------------
				
				
				
				
				
				
				'--------------------------------------------------------------------------------------------
	            '[Largura Fixa]
				if sCopia = "larguraFixa" then
					
				    Dim strFilename As String
					Dim TImage As System.Drawing.Image
					
					'strFilename = System.Web.Hosting.HostingEnvironment.MapPath("~"& sDir & sFileName)
					strFilename = System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados & sDir & sFileName
					'-----------------------------------
					'Copiando original para pasta /o/
					'2016
					'Verificando se diretorio \o\ existe
					if(Not System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados &  sDir &"o\"))then
						System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados &  sDir &"o\")
					end if
					
					Dim strCopiaOriginal As String
					strCopiaOriginal = System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados & sDir &"o\"& sFileName
					File.Copy(strFilename, strCopiaOriginal)
					'-----------------------------------
					
					Dim b as Bitmap
					Dim g as Graphics
					Dim width As Integer
					Dim height As Integer
					Dim stringSave as String
					
                    stringSave = left(strFilename,(len(strFilename)-len(sFileName)))
					TImage = System.Drawing.Image.FromFile(strFilename)
					
					wO = TImage.Width
					hO = TImage.Height
					
					Dim dimensoes() as String
					Dim prefixos() as String
					Dim eixos() as String
					Dim x as Integer
					Dim posX as Integer = 0
					Dim posY as Integer = 0
					
					dimensoes = split(sTamanho, "|")
					prefixos = split(sPrefixo, "|")
					
					For x=0 to ubound(dimensoes)
						
						eixos = split(dimensoes(x),":")
						posX = 0
						posY = 0
						height = 0
						width = cInt(eixos(0))
						height = cint((TImage.Height * width) / TImage.Width)
						
						
						hP = height
						
						'Setando objetos
						'Desenhando o tamanho do quadro
						b = new System.Drawing.Bitmap(width, height, PixelFormat.Format32bppRgb)
						b.SetResolution(72,72)
						g = Graphics.FromImage(b)
						g.Clear(Color.White)
						g.CompositingQuality = CompositingQuality.HighQuality
						g.SmoothingMode = SmoothingMode.HighQuality
						g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
						g.InterpolationMode = InterpolationMode.HighQualityBicubic
						
						'Desenhando a imagem dentro do quadro
						g.DrawImage(TImage, posX, posY, width, height)
						
						select case sFileType
							case ".jpg"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Jpeg)
							case ".gif"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Gif)
							case ".png"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Png)
							case ".bmp"
								b.Save(stringSave &"/"& prefixos(x) &""& sFileName, ImageFormat.Bmp)
						end select
						
					Next
					
					
					
					b.Dispose()
					g.Dispose()

					TImage.Dispose()
				   
					Timage = nothing
					b = nothing
					g = nothing					
					
					'Excluindo a imagem original
					Dim lfoto As String = strFilename
					File.Delete(lfoto)
					
				end if
	            '[FIM Altura Fixa]
				'--------------------------------------------------------------------------------------------
				
				
				
				
				
				
				
				
				
				
				
				'--------------------------------------------------------------------------------------------
	            '[Largura Fixa]
				if sCopia = "semCopias" then
					
				    Dim strFilename As String
					Dim TImage As System.Drawing.Image
					
					'strFilename = System.Web.Hosting.HostingEnvironment.MapPath("~"& sDir & sFileName)
					strFilename = System.Web.Hosting.HostingEnvironment.MapPath("~") & pathDados & sDir & sFileName
					
					Dim stringSave as String
					
                    stringSave = left(strFilename,(len(strFilename)-len(sFileName)))
					TImage = System.Drawing.Image.FromFile(strFilename)
					
					wO = TImage.Width
					hO = TImage.Height

					TImage.Dispose()
				   
					Timage = nothing
					
				end if
	            '[FIM Altura Fixa]
				'--------------------------------------------------------------------------------------------
				
				
				
				
				
				
				
				
				'----------------------------------------------------------------------------------------------------------
				
				Dim tmpAux as String = ""
				if wO <> "" then
					tmpAux &= "|"& wO &"|"& hO &"|"& wP &"|"& hP
				end if
				
				Return "ok" & tmpAux
				
			Catch ex As Exception
	
				Return ex.ToString()
	
			End Try
	
		End Function
		

		'##[ gravaArquivo ]
		'# Método que grava arquivo
		'# Parametros:
		'# sFilename - Nome do arquivo, nome que sera gravado
		'# bytFileByteArr - Array dos Bytes do arquivo
		'# sDir - Diretorio onde será gravado o arquivo deve conter barras(/)
		'# sCopia - Tipo de gravação  1 = redimensiona pela largura , 2 = cria uma copia p_, 3 cria N copias, com N prefixos
		Public Function gravaArquivo(ByVal sFileName As String, ByVal bytFileByteArr As Byte(), ByVal sDir As String)
		    Dim resultado As String = ""
			
		    Try
			    
				Dim sFileType As String
				sFileType = System.IO.Path.GetExtension(sFileName)
				sFileType = sFileType.ToLower()
		
				Dim strFile As String = _
				System.IO.Path.GetFileNameWithoutExtension(sFileName)
				strFile = System.Web.Hosting.HostingEnvironment.MapPath _
				("~"& sDir  & strFile & sFileType)
				
				Dim stream As New FileStream(strFile, FileMode.OpenOrCreate) 'FileMode.CreateNew
				stream.Write(bytFileByteArr, 0, bytFileByteArr.Length)
				stream.Close()
				
				'Pegando dimensoes
				Dim TImage As System.Drawing.Image
				TImage = System.Drawing.Image.FromFile(strFile)
				
				Dim wO as String = ""
				Dim hO as String = ""
				
				wO = TImage.Width
				hO = TImage.Height
				
				resultado = "ok|"& wO &"|"& hO
			Catch ex As Exception
			    resultado = ex.ToString()
			End Try
			
			return resultado
		End Function
		
		
		Public Function ajustaWidth(ByVal altura As Integer, ByVal largura As Integer, ByVal meta As Integer) As Integer
		    Dim resultado As Integer
		    
			Try
                Dim larg As Integer = 1
				Dim alt As Integer = cInt((altura * larg) / largura)

			    while alt < meta
				    larg = larg + 1
					alt = cint((altura * larg) / largura)
                end while
				
				resultado = larg
			Catch ex As Exception
			    resultado = 0
			End Try
			return resultado
		End Function
			
	End Class
End Namespace