<!doctype html>

<html lang="en">
<head>
  <meta charset="utf-8">

  <title>The HTML5 Herald</title>
  <meta name="description" content="The HTML5 Herald">
  <meta name="author" content="SitePoint">

  <link rel="stylesheet" href="css/styles.css?v=1.0">

  <!--[if lt IE 9]>
  <script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
  <![endif]-->
</head>

<body>
  <script src="js/scripts.js"></script>

<?php
	include 'lib/Authenticator.php';
	$auth = new Authenticator("token.txt", "asd@asd.com", "Password1234!");
	var_dump($auth->GetStuff("http://awesomeduckninjas.azurewebsites.net/api/Values"));
	#$resp = getStuff("http://awesomeduckninjas.azurewebsites.net/api/Values/1", $tokenFile, $user, $pass);
	#echo $resp;
?>
</body>
</html>

