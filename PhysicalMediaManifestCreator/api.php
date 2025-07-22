<?php

function loadEnv(){
    $env = array();
    $env_text = fopen("../.env", "r");

    while ($line = fgets($env_text)) {
        $data = explode('=', $line, 2);
        if (count($data) > 0) {
            $env[$data[0]] = trim($data[1]);
        }
    }
    return $env;
}

if (!isset($_GET["api"])) {
    die();
}

function GetGameData()
{
    
    $env = loadEnv();

    $curl = curl_init();

    curl_setopt_array($curl, array(
        CURLOPT_URL => 'https://www.steamgriddb.com/api/v2/search/autocomplete/' . urlencode($_GET["gameName"]),
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_ENCODING => '',
        CURLOPT_MAXREDIRS => 10,
        CURLOPT_TIMEOUT => 0,
        CURLOPT_FOLLOWLOCATION => true,
        CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
        CURLOPT_CUSTOMREQUEST => 'GET',
        CURLOPT_HTTPHEADER => array(
            'Authorization: Bearer ' . $env["STEAM_GRID_DB"]
        ),
    ));

    $response = curl_exec($curl);
    curl_close($curl);
    echo $response;
}

function GetGameIcons()
{
    
    $env = loadEnv();

    $curl = curl_init();

    curl_setopt_array($curl, array(
        CURLOPT_URL => 'https://www.steamgriddb.com/api/v2/icons/game/' . urlencode($_GET["gameId"]) . '?dimensions=256',
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_ENCODING => '',
        CURLOPT_MAXREDIRS => 10,
        CURLOPT_TIMEOUT => 0,
        CURLOPT_FOLLOWLOCATION => true,
        CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
        CURLOPT_CUSTOMREQUEST => 'GET',
        CURLOPT_HTTPHEADER => array(
            'Authorization: Bearer ' . $env["STEAM_GRID_DB"]
        ),
    ));

    $response = curl_exec($curl);
    curl_close($curl);
    echo $response;
}

function GetDataURI(){
    $url = $_GET["img"];
    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_HEADER, 0);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true); 
    $image = curl_exec($ch); 
    curl_close($ch);
    $finfo = finfo_open(FILEINFO_MIME_TYPE);
    $data = base64_encode($image);
    echo 'data:' . finfo_buffer($finfo, $image) . ';base64,' . $data;
}

$_GET["api"]();
