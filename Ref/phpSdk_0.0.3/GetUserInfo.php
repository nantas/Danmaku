<?php
require 'config.php';
require 'WeiyouxiClient.php';
try
{
	$weiyouxi = new WeiyouxiClient( $appKey , $appSecret );
	$userId = $weiyouxi->getUserId();
	//调用API接口
	$info = $weiyouxi->get( 'user/show' , array( 'uid' => $userId ) );
	echo '请求接口成功:';
	var_dump( $info );
}
catch( Exception $e )
{
	echo '请求接口失败:';
	var_dump($e);
}

?>

