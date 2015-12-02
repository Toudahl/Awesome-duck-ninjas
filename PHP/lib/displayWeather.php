<?php
require_once 'WeatherForecast.php';
/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

$city = "Roskilde";
$country = "DK";
$infoType = 0;


if (hasGetContent()) {
    $city = $_GET["city"];
    $country = $_GET["country"];
    $infoType = $_GET["infoType"];
}


$weather = new WeatherForecast($city, $country);

function hasGetContent() {
    return (isset($_GET["city"]) && isset($_GET["country"])) && ($_GET["city"] != "" && $_GET["country"] != "");
}

function isBasicRequest($param) {
    return $param == 0;
}

function BasicInformation($weather){
    return "Description: ". $weather->BasicWeatherInformation()->LongDescription .
           "<br />Temperature: ". $weather->BasicWeatherInformation()->Temperature;
}

function isExtendedRequest($param){
    return $param == 1;
}

function ExtendedInformation($weather) {
        return  "Humidity: ". $weather->ExtendedWeatherInformation()->Humidity.
            "<br />Ground level: ". $weather->ExtendedWeatherInformation()->GroundLevel.
            "<br />Sea level: ". $weather->ExtendedWeatherInformation()->SeaLevel.
            "<br />Wind speed: ". $weather->ExtendedWeatherInformation()->WindSpeed.
            "<br />Wind direction: ". $weather->ExtendedWeatherInformation()->WindDirection.
            "<br />Sunrise: ". $weather->ExtendedWeatherInformation()->Sunrise.
            "<br />Sunset: ". $weather->ExtendedWeatherInformation()->Sunset;
}

function isFullRequest($param) {
    return $param == 2;
}

function FullInformation($weather) {
    return  "";
}


function GetWeatherForm($city){
    return
    '
<form>
    <fieldset>
        <legend>Location</legend>
        <input type="text" name="city" value="'.$city.'">
        <select name="country">
            <option value="DK">Denmark</option>
            <option value="CZ">Czech Republic</option>
            <option value="HR">Croatia</option>
            <option value="SK">Slovakia</option>
        </select>
    </fieldset>
    
    <fieldset>
        <legend>Amount of information</legend>
        <input type="radio" name="infoType" value="0" checked> Basic 
        <input type="radio" name="infoType" value="1" > Extended 
        <!--<input type="radio" name="infoType" value="2" > Full -->
    </fieldset>
    
    <button type="submit">Find weather</button>
</form>
'
    ;
}

function GetWeatherDisplay($weather, $infoType, $city, $country) {
    
    $dynamic = "";
            if (isBasicRequest($infoType)) {
                $dynamic = BasicInformation($weather);
            }
            if (isExtendedRequest($infoType)) {
               $dynamic = BasicInformation($weather). "<br />".ExtendedInformation($weather);
            }
            
            /*
            if (isFullRequest($infoType)) {
                echo BasicInformation($weather). "<br />";                
                echo ExtendedInformation($weather). "<br />";
                echo FullInformation($weather);
            }
            */
    return '<div id="WeatherInformation">    
    <h1 id="weatherHeader">'.$weather->BasicWeatherInformation()->ShortDescription.'</h1>
    <p id="weatherText">
        '.$dynamic.'
        <br/><br/>Weather for: '.$city .", ". $country.'
    </p>
</div>';
}



echo GetWeatherForm($city);
echo GetWeatherDisplay($weather, $infoType, $city, $country);


?>





