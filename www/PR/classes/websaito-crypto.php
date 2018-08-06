<?php
//AUTHOR:WEBSAITO
//2016

class wsCripto{
	
	private $serros;
	
	function __construct(){
		
	}
	
	function cenha(string $psenha):string
	{
		$options = [
			'cost' => 12,
		];
		return password_hash($psenha, PASSWORD_BCRYPT, $options);
	}
	
	function criptografa(string $v, string $chave):string
	{
		$str = "";
		
		$inicio = 0;
		while($inicio >= 0){
			
			$str .= ($str == "" ? "" : "|") . ordutf8($text, $offset);
		}
		
		return $str;
	}
	
	function descriptografa(string $v, string $chave):string
	{
		$str = "";
		
		return $str;
	}
	
}

?>