<?php
require 'WeiyouxiClient.php';
try
{
	$weiyouxi = new WeiyouxiClient( '2603432055' , 'cca518f66402e5423aa82171030af4bd' );
	$userId = $weiyouxi->getUserId();
	//����API�ӿ�
	$info = $weiyouxi->get( 'user/show' , array( 'uid' => 1936344094 ) );
	echo '����ӿڳɹ�:';
	var_dump( $info );
}
catch( Exception $e )
{
	echo '����ӿ�ʧ��:';
	var_dump($e);
}
?>
