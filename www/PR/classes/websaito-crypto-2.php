<?php
//Author:WebSaito - 2017

class wsCripto{
	
	private $serros;
	private $ini_ch = 36;
	private $fim_ch = 255;
	
	function __construct(){
		
	}
	
	function cenha(string $psenha):string
	{
		/*
		$options = [
			'cost' => 12,
		];
		return password_hash($psenha, PASSWORD_BCRYPT, $options);
		*/
		return sha1($psenha);
	}
	
	function criptografa(string $v, string $chave):string
	{
		//Retorno
		$str = "";
		
		//Pegando valor da chave
		$valorChave = 0;
		for($x=0; $x<strlen($chave); $x++){
			$valorChave = $valorChave + (intval(ord($chave[$x])));
		}
		
		
		//Acao
		$act = "";
		$valorChar = 0;
		
		//varrendo os caracteres
		for($x=0; $x<strlen($v); $x++ ){
			
			if($v[$x] == " "){
				
				$str .= $v[$x];
				
			}else{
			
				if(($x % 2) == 0){
					$valorChar = intval(ord($v[$x])) + $valorChave;
				}else{
					$valorChar = intval(ord($v[$x])) - $valorChave;
				}
				
				
				
				if($valorChar < $this->ini_ch){
					$valorChar = $this->voltas($valorChar, "MENOR");
				}
				
				if($valorChar > $this->fim_ch){
					$valorChar = $this->voltas($valorChar, "MAIOR");
				}
				
				$str .= chr($valorChar);
				
			}
			
			
			
		}
		
		
		//return base64_encode(base64_encode($str));
		return $str;
	}
	
	function descriptografa(string $v, string $chave):string
	{
		$r = "";
		$str = "";
		
		//Pegando valor da chave
		$valorChave = 0;
		for($x=0; $x<strlen($chave); $x++){
			$valorChave = $valorChave + (intval(ord($chave[$x])));
		}
		
		//Acao
		$act = "";
		$valorChar = 0;
		
		//Valor
		//$str = base64_decode(base64_decode($v));
		$str = $v;
		
		//varrendo os caracteres
		for($x=0; $x < strlen($str); $x++ ){
			
			if($str[$x] == " "){
				$r .= " ";
			}else{
				
				if(($x % 2) == 0){
					$valorChar = intval(ord($str[$x])) - $valorChave;
				}else{
					$valorChar = intval(ord($str[$x])) + $valorChave;
				}
				
				
				
				if($valorChar < $this->ini_ch){
					$valorChar = $this->voltas($valorChar, "MENOR");
				}
				
				if($valorChar > $this->fim_ch){
					$valorChar = $this->voltas($valorChar, "MAIOR");
				}
				
				
				$r .= chr($valorChar);
			}
			
		}
		
		
		
		return $r;
	}
	
	
	
	
	function CID6(string $v, string $pchave):string
	{
		$r = "";
			
			$aux = substr(date("siHdmY"), 0, (strlen(date("siHdmY"))-2)) .":". $v .":". substr(date("YmdHis"), 2, (strlen(date("YmdHis"))-2) );
			$r = base64_encode(base64_encode($this->criptografa($aux, $pchave)));
			
		return $r;
	}
	
	function RCID6(string $v, string $pchave):string
	{
		$r = "";
			
			$r = $this->descriptografa(base64_decode(base64_decode($v)), $pchave);
			$aux = explode(":", $r);
			$r = $aux[1];
			
		return $r;
	}
	
	
	
	
	
	//Métodos auxiliares
	function voltas($total, $estado):int
	{
		$r = 0;
		
		if($estado == 'MAIOR'){
			
			$total = $this->ini_ch + ($total - $this->fim_ch);
			if($total >= $this->fim_ch){
				$total = $this->voltas($total, 'MAIOR');
			}
			
		}else{
		
			$total = ($total + $this->fim_ch) - $this->ini_ch;
			if($total <= $this->ini_ch){
				$total = $this->voltas($total, 'MENOR');
			}
			
		}
		
		$r = intval($total);
		
		return $r;
	}
	
	
}

?>