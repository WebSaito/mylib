<?php
//Author:WebSaito - 2017

class wsForms{
	
	private $serros = '';
	
	private $slabel = '';
	private $sfield = '';
	
	function __construct(){
	//Construtor
	}
	
	private function setLabel(string $v){
		$this->slabel = $v;
	}
	
	private function getLabel():string{
		return $this->slabel;
	}
	
	private function setField(string $v){
		$this->sfield = $v;
	}
	
	private function getField():string{
		return $this->sfield;
	}
	
	public function __set(string $item, $v){
		switch($item){
			case 'label' : $this->setLabel($v);
			case 'field' : $this->setField($v);
		}
	}
	
	public function __get(string $item):string{
		switch($item){
			case 'label' : return $this->getLabel();
			case 'field' : return $this->getField();
		}
	}
	
	//Método que pega o value baseado no action
	public function fsVal(string $vdb, string $vform, string $act):string{
		$r = $vdb;
		if($act != ""){
			$r = $vform;
		}
		
		return $r;
	}
	
	//Método que localiza string de erro se houver e retorna valor baseado em opçao
	public function verificaErros(string $pcampo, string $v, string $ptipo):string{
		$r = "";
		
		if( trim($v) <> ""){
			$aux = explode("|", $v);
			foreach($aux as $item){
				$aux2 = explode(":", $item);
				
				if(strtolower(trim($aux2[0])) == strtolower(trim($pcampo)) ){
					
					switch($ptipo){
						case 'L' : $r = $this->__get("label");
									break;
						case 'C' : $r = $this->__get("field");
									break;
						case 'M' : $r = $aux2[1];
									break;
					}
					
				}
			}
		}
		
		return $r;
		
	}
	
	
}
?>