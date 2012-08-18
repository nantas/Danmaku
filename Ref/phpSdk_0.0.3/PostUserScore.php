<?php
require 'config.php';
require 'WeiyouxiClient.php';

try {
	$weiyouxi = new WeiyouxiClient($appKey, $appSecret);
	
	$params = array(
		'rank_id' => $_POST['rank_id'],
		'value' => $_POST['value'],
	);
	$data = $weiyouxi->post('leaderboards/set', $params);

	var_dump( $data );
	
} catch (Exception $e) {
	var_dump($e);
}

?>
