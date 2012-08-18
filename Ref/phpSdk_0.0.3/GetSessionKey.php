<?php
require 'config.php';
require 'WeiyouxiClient.php';

try
{
    $weiyouxi = new WeiyouxiClient( $appKey, $appSecret );

    //if program can't get params(session_key,signature...) by GET
    //如果当前程序无法从GET中获取参数 则需执行下面这行代码 将你保存的session_key作为参数传入
    $weiyouxi->setAndCheckSessionKey('session_key');

    //如果还想检测签名 则需执行下面这行代码 将你保存的signature作为参数传入
    $weiyouxi->setAndCheckSignature('signature');

    $userId = $weiyouxi->getUserId();
    //call api
    $me = $weiyouxi->get('user/show');
    var_dump($me);
}
catch( Exception $e )
{
    var_dump($e);
}

?>
