<?php
class WeatherForecast{
	
	    private $city;// = "Roskilde,DK";
        private $dataType = "json";
        private $unitType = "metric";
        private $appID = "629f3309109159c6beff00ede0af4940";
        private $url;// = "http://api.openweathermap.org/data/2.5/weather?q=Roskilde,DK&mode=json&units=metric&APPID=629f3309109159c6beff00ede0af4940";
		private $resultObject;


		function __construct($city, $country){
			$this->city = $city.",".$country;
			$this->url = $this->SetUrl();
			
			$header = array(
			"POST / HTTP/1.1",
			"Content-Type: text/xml;charset=\"utf-8\"",
			"Accept: text/json",
			"Cache-Control: no-cache",
			"Pragma: no-cache",
			);

			
			$curlInit = curl_init($this->url);
			curl_setopt($curlInit, CURLOPT_POST,true);
			curl_setopt($curlInit, CURLOPT_URL, $this->url);
			curl_setopt($curlInit, CURLOPT_HTTPHEADER, $header);
			curl_setopt($curlInit, CURLOPT_RETURNTRANSFER, true); // return the output in string format
			$content = curl_exec($curlInit);
			curl_close($curlInit);

			$this->resultObject = json_decode($content);
		}
	
		private function SetUrl(){
			return "http://api.openweathermap.org/data/2.5/weather?q=".$this->city."&mode=".$this->dataType."&units=".$this->unitType."&APPID=".$this->appID;
		}
		
		public function FullWeatherInformation(){
			return $this->resultObject;
		}
		
		public function BasicWeatherInformation(){
			$returnValue = new stdClass();
			
			$returnValue->ShortDescription = $this->resultObject->weather[0]->main;
			$returnValue->LongDescription = $this->resultObject->weather[0]->description;
			$returnValue->Temperature = $this->resultObject->main->temp . " C";
			
			return $returnValue;
		}
		
		public function ExtendedWeatherInformation(){
			$returnValue = new stdClass();
			
			$returnValue->ShortDescription = $this->resultObject->weather[0]->main;
			$returnValue->LongDescription = $this->resultObject->weather[0]->description;
			$returnValue->Temperature = $this->resultObject->main->temp . " C";
			$returnValue->Humidity = $this->resultObject->main->humidity . " %";
			$returnValue->TemperatureMin = $this->resultObject->main->temp_min . " C";
			$returnValue->TemperatureMax = $this->resultObject->main->temp_max . " C";
			$returnValue->GroundLevel = $this->resultObject->main->grnd_level . " M";
			$returnValue->SeaLevel = $this->resultObject->main->sea_level . " M";
			$returnValue->WindSpeed = $this->resultObject->wind->speed . " m/s";
			$returnValue->WindDirection = $this->resultObject->wind->deg . "&#176;";
			$returnValue->Sunrise = date("H:m:s", $this->resultObject->sys->sunrise);
			$returnValue->Sunset = date("H:m:s", $this->resultObject->sys->sunset);
			
			
			return $returnValue;
		}
	
	
	
	
	

}

?>