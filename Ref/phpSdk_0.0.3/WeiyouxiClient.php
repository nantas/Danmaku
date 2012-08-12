<?php
/**
 * API请求类 By YuFei.Liu
 * @example 
try
{
	$weiyouxi = new WeiyouxiClient( 'your app key (source)' , 'your app secret' );
	$userId = $weiyouxi->getUserId();
	//call api
	$me = $weiyouxi->get('user/me');
	var_dump($me);
	//call api with params
	$info = $weiyouxi->get( 'user/get_info' , array('uid'=>1666822612) );
	var_dump($info);
}
catch( Exception $e )
{
	var_dump($e);
}
 */

if ( !function_exists('curl_init') )
{
  throw new Exception('Require CURL extension');
}

if ( !function_exists('json_decode') )
{
  throw new Exception('Require JSON extension');
}

class WeiyouxiClient
{
	
	const PREFIX_PARAM = 'wyx_';
	const VERSION = '0.0.2';
	
	protected $source;
	protected $secret;
	protected $sessionKey;
	protected $signature;
	protected $apiUrl = 'http://api.weibo.com/game/1/';
	protected $userAgent = 'Weiyouxi Agent Alpha 0.0.1';
	protected $connectTimeout = 30;
	protected $timeout = 30;
	protected $httpCode;
	protected $httpInfo;
	/**
	 * user session 用户session
	 */
	protected $session = array(
		'sessionKey' => null,
	  	'userId' => null,
	  	'create' => null,
	  	'expire' => null,
	);

	/**
	 * Initialization 初始化
	 * @param string $source App ID 应用ID
	 * @param string $secret App Secret 应用密钥
	 */
	public function __construct( $source , $secret )
	{
		$this->source = $source;
		$this->secret = $secret;
		$this->sessionKey = empty( $_GET[ self::PREFIX_PARAM . 'session_key' ] ) ? null : $_GET[ self::PREFIX_PARAM . 'session_key' ];
		$this->signature = empty( $_GET[ self::PREFIX_PARAM . 'signature' ] ) ? null : $_GET[ self::PREFIX_PARAM . 'signature' ];
	
		if( empty( $this->source ) )
		{
			throw new Exception('Require source');
		}
		if( empty( $this->secret ) )
		{
			throw new Exception('Require secret');
		}
		
		//是否有签名或会话key
		if( !is_null( $this->sessionKey ) || !is_null( $this->signature ) )
		{
			//检测签名
			$this->checkSignature();
			//检测会话key
			$this->checkSessionKey();
		}
	}

	/**
	 * 在脚本无法自动从$_GET中获取参数又想要检测签名时可使用该函数
	 * set and check signature 设置 并检测签名 
	 * @param string $signature 签名
	 * @param array $getParams 平台提供的参数
	 */
	public function setAndCheckSignature( $signature , $getParams = array() )
	{
		$this->signature = $signature;
		$this->checkSignature( $getParams );
	}
	
	/**
	 * 检测签名
	 * @param array $getParams 平台提供的参数
	 */
	public function checkSignature( $getParams = array() )
	{
		if( empty( $this->signature ) )
		{
			throw new Exception('Require signature');
		}
		
		$params = array();
		$temp = empty( $getParams ) ? $_GET : $getParams ;
		unset( $temp[ self::PREFIX_PARAM . 'signature' ] );
		foreach ( $temp as $k => $v )
		{
			if ( strpos( $k , self::PREFIX_PARAM ) === 0 )
			{
				$params[$k] = $v;
			}
		}
		$baseString = self::buildBaseString( $params );
		if( sha1( $baseString . $this->secret ) != $this->signature )
		{
			throw new Exception('Signature error');
		}
	}
	
	/**
	 * 在脚本无法自动从$_GET中获取参数且调用[必须提供session_key的接口]时可使用该函数
	 * set and check session key 设置 并检测会话key 
	 * @param string $sessionKey 会话key
	 */
	public function setAndCheckSessionKey( $sessionKey )
	{
		$this->sessionKey = $sessionKey;
		$this->checkSessionKey();
	}
	
	/**
	 * 检测会话key
	 */
	public function checkSessionKey()
	{
		if( empty( $this->sessionKey ) )
		{
			throw new Exception('Require session key');
		}
		
		$this->session['sessionKey'] = $this->sessionKey;
		$sessionArr = explode( '_' , $this->session['sessionKey'] );
		if( count( $sessionArr ) < 3 )
		{
			throw new Exception('Session key error');
		}
		$expire = $sessionArr[1];
		$userId = $sessionArr[2];
		$this->session['userId'] = (int)$userId;
		$this->session['create'] = empty( $_GET[ self::PREFIX_PARAM . 'create' ] ) ? null : $_GET[ self::PREFIX_PARAM . 'create' ] ;
		$this->session['expire'] = empty( $_GET[ self::PREFIX_PARAM . 'expire' ] ) ? $expire : $_GET[ self::PREFIX_PARAM . 'expire' ] ;
	}
	
	/**
	 * get userId 获取用户ID
	 * @return int or null
	 */
	public function getUserId()
	{
		return $this->session['userId'];
	}
	
	/**
	 * get session ( sessionKey userId create expire ) 获取session
	 * @return array
	 */
	public function getSession()
	{
		return $this->session;
	}
	
	/**
	 * call api (post) 请求API(post方式)
	 * @param string $api api name 接口名
	 * @param array $data params except source,session_key,timestamp 除去source,session_key,timestamp的接口参数 
	 * @return array
	 */
	public function post( $api , $data = array() )
	{
		return json_decode( $this->http( $this->apiUrl . $api , $this->buildQueryParamStr( $data ) , 1 ) , true );
	}
	/**
	 * call api (get) 请求API(get方式)
	 * @param string $api api name 接口名
	 * @param array $data params except source,session_key,timestamp 除去source,session_key,timestamp的接口参数 
	 * @return array
	 */
	public function get( $api , $data = array() )
	{
		return json_decode( $this->http( $this->apiUrl . $api , $this->buildQueryParamStr( $data ) , 0 ) , true );
	}
	
	public function buildQueryParamStr( &$data )
	{
		$timestamp = microtime(true);
		
		$params = array(
			'source' => $this->source,
			'timestamp' => $timestamp,
		);
		
		if( !is_null( $this->sessionKey ) )
		{
			$params['session_key'] = $this->sessionKey;
		}
		
		$params = array_merge( $params , $data );
		$baseString = self::buildBaseString( $params );
		$signature = sha1( $baseString.$this->secret );
		$baseString .= '&signature=' . $signature;
		return $baseString;
	}
	
	public static function buildBaseString( &$params ) 
	{
		if (!$params) return '';
		$keys = self::urlencodeRfc3986( array_keys( $params ) );
		$values = self::urlencodeRfc3986( array_values( $params ) );
		$params = array_combine( $keys , $values );

		uksort( $params , 'strcmp' );

		$pairs = array();
		foreach ( $params as $parameter => $value ) 
		{
			if ( is_array( $value  ) ) 
			{
				natsort( $value );
				foreach ( $value as $duplicate_value ) 
				{
					$pairs[] = $parameter . '=' . $duplicate_value;
				}
			}
			else 
			
			{
				$pairs[] = $parameter . '=' . $value;
			}
		}
		return implode( '&' , $pairs );
	}
	
	public static function urlencodeRfc3986( $input ) 
	{
		if ( is_array( $input  )) 
		{
			return array_map( array('WeiyouxiClient', 'urlencodeRfc3986') , $input );
		} 
		else if ( is_scalar( $input ) ) 
		{
			return str_replace( '+' , ' ' , str_replace( '%7E' , '~' , rawurlencode( $input ) ) );
		} 
		else 
		{
			return '';
		}
	}
	
	public function http( $url , $dataStr = '' , $isPost = 0 )
	{
		$this->httpInfo = array();
		$ch = curl_init();
		
		curl_setopt( $ch, CURLOPT_HTTP_VERSION , CURL_HTTP_VERSION_1_0 );
		curl_setopt( $ch, CURLOPT_USERAGENT , $this->userAgent );
		curl_setopt( $ch, CURLOPT_CONNECTTIMEOUT , $this->connectTimeout );
		curl_setopt( $ch, CURLOPT_TIMEOUT , $this->timeout );
		curl_setopt( $ch, CURLOPT_RETURNTRANSFER , true );
		if( $isPost )
		{
			curl_setopt( $ch , CURLOPT_POST , true );
			curl_setopt( $ch , CURLOPT_POSTFIELDS , $dataStr );
			curl_setopt( $ch , CURLOPT_URL , $url );	
		}
		else
		{
			curl_setopt( $ch , CURLOPT_URL , $url.'?'.$dataStr );	
		}
		
		$response = curl_exec( $ch );
		$this->httpCode = curl_getinfo( $ch , CURLINFO_HTTP_CODE );
		$this->httpInfo = array_merge( $this->httpInfo , curl_getinfo( $ch ) );
		curl_close( $ch );
		return $response;
	}
	
	public function setUserAgent( $agent = '' )
	{
		$this->userAgent = $agent;
	}
	
	public function setConnectTimeout( $time = 30 )
	{
		$this->connectTimeout = $time;
	}
	
	public function setTimeout( $time = 30 )
	{
		$this->timeout = $time;
	}
	
	public function getHttpCode()
	{
		return $this->httpCode;
	}
	
	public function getHttpInfo()
	{
		return $this->httpInfo;
	}
}