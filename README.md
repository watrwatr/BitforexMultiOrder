# BitforexMultiOrder

Looking for a solution to successfully execute Bitforex api-call 'placeMultiOrder'

Found the problem to execute the api-call [placeMultiOrder](https://github.com/bitforexapi/API_Doc_en/wiki/Muliti-order)

This sample ptoject written in (net5.0)[https://dotnet.microsoft.com/download/dotnet/5.0] trying to place multi orders


### How to run

##### 1. set ApiKey and ApiSec values:
```
	static void Main(string[] args)
		{
			string apiKey = "<PLACE-HERE-YOUR-APIKEY>";
			string apiSec = "<PLACE-HERE-YOUR-APISECRET>";
```

##### 2. Buil project
```
$ dotnet build
```

##### 3. Run test application
```
$ cd bin/Debug/net5.0
$ ./BitforexMultiOrder
```

###### Problem output:
```
REQ_BODY : accessKey=<YOUR-API-KEY>&nonce=1637938441220&ordersData=[{"price":0.9,"amount":15,"side":1},{"price":0.91,"amount":15,"side":1}]&symbol=coin-usdt-xrp&signData=<SIGNED-DATA>
REQ_URL  : https://api.bitforex.com/api/v1/trade/placeMultiOrder
RESP_CODE: OK
RESP_TEXT: {"code":"1011","success":false,"time":1637938442802,"message":"NeedParam accessKey and signData"}
```
