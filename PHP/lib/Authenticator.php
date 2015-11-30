<?php
class Authenticator {
	private function isTokenValid($tokenFile)
	{
		if (file_exists($this->tokenFile)) {
			$lines = file($this->tokenFile);//file in to an array
			if (count($lines) == 2){
	    		if(time() < $lines[0]){
	    			return true;
	    		}
	    	}
		} 
		return false;
	}

	private function requestToken($user, $pass)
	{
		$ch = curl_init();
		curl_setopt($ch, CURLOPT_URL, "http://awesomeduckninjas.azurewebsites.net/token" );
		curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
		curl_setopt($ch, CURLOPT_POST, 1);
		curl_setopt($ch, CURLOPT_POSTFIELDS, "username={$user}&password={$pass}&grant_type=password"); 
		curl_setopt($ch, CURLOPT_HTTPHEADER, array('content-type: application/x-www-form-urlencoded')); 
		$resp = curl_exec($ch);
		curl_close($ch);
		return json_decode($resp);
	}

	private function getToken($tokenFile, $user, $pass)
	{
		if (!$this->isTokenValid($tokenFile)){
			$respObj = $this->requestToken($user, $pass);
			$this->storeToken($tokenFile, $respObj);
		}
		$lines = file($tokenFile);
	    return $lines[1];
	}

	private function storeToken($tokenFile, $respObj){
		$myfile = fopen($this->tokenFile, "w") or die("Fuck, something went wrong!");
		fwrite($myfile, strtotime($respObj->{".expires"}) . "\r\n");
		fwrite($myfile, $respObj->{"access_token"} . "\r\n");
		fclose($myfile);
	}

	public function GetStuff($url)
	{
		$ch = curl_init();
		curl_setopt($ch, CURLOPT_URL, $url );
		curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
		curl_setopt($ch, CURLOPT_HTTPHEADER, array('Authorization: bearer ' . $this->getToken($this->tokenFile, $this->user, $this->pass))); 
		$resp = curl_exec($ch);
		curl_close($ch);
		return json_decode($resp);
	}

	public function authenticator($tokenFilename, $username, $password){
		$this->tokenFile = $tokenFilename;
		$this->user = $username;
		$this->pass = $password;
	}

	private $tokenFile;
	private $pass;
	private $user;
}
?>
