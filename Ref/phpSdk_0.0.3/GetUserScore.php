<?php
require 'config.php';
require 'WeiyouxiClient.php';

//使用SDK调用接口 
try
{
        //$appKey : your app key (source) ;  $appSecret: your app secret
        $weiyouxi = new WeiyouxiClient( $appKey , $appSecret );

        $data = $weiyouxi->get( 'leaderboards/get_total' , array( 'rank_id' => 1 ) );
         
        var_dump( $data );
}
catch( Exception $e )
{
        var_dump($e);
}

?>
