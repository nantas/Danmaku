<?php
require 'WeiyouxiClient.php';
try
{
	$weiyouxi = new WeiyouxiClient( '2603432055' , 'cca518f66402e5423aa82171030af4bd' );
	$userId = $weiyouxi->getUserId();
	//调用API接口
	$info = $weiyouxi->get( 'user/show' , array( 'uid' => 1936344094 ) );
	echo '请求接口成功:';
	var_dump( $info );
}
catch( Exception $e )
{
	echo '请求接口失败:';
	var_dump($e);
}
?>
