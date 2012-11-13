SimpleRtRest
============

SimpleRtRest is really simple Rest client for WinRt applications. It use new WinRt HttpClient as a base and gives you ability to consume Restfull services from your Windows 8 application with just few lines of code.

This project is based on great .NET Rest client [RestSharp](https://github.com/restsharp/RestSharp) and main goal is to have something similar to use in Windows RT applications.

Please visit [SimpleRtRest](http://rjovic.github.com/SimpleRtRest/) web page for more information. More details TBD.

Initial version only support limited set of features (basic RestFull verb requests, only form url encoded parameters content on POST/PUT, url parameters on GET, JSON deserialization) which are not
tested and documented. Not for production use. This is a only preview of work in progress.

##### Basic usage

```csharp

var client = new RestClient();
client.Authenticator = new HttpBasicAuthenticator(username, password);

// url value which is included in all requests
client.AddDefaultUrlSegment("MyDefaultParam", someDefaultValue);

Require.Argument("MyParam", someValue);

var request = new RestRequest(HttpMethod.POST);
request.Resource = "resource-url.json";
request.AddParameter("MyParam", someValue);

var response = await client.Execute<YourClass>(request);

var myObject = response.Data;

```
