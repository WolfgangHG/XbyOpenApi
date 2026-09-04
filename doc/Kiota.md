# OpenAPI client with Kiota

This project uses [Kiota](https://github.com/microsoft/kiota/) to generate a REST client from the OpenAPI specification of the X services.
The spec is found at https://api.twitter.com/2/openapi.json


# Table of Contents

- [Installing Kiota](#installing-kiota)
- [Generating the client](#generating-the-client)
- [Initializing the project](#initializing-the-project)
- [Using the client](#using-the-client)
- [Hack: logging requests and responses](#hack-logging-requests-and-responses)
- [What about other OpenAPI libraries?](#what-about-other-openapi-libraries)


# Installing Kiota
First of all, we have to install the tool `Microsoft.OpenApi.Kiota` (see https://learn.microsoft.com/en-us/openapi/kiota/install)

This could be done globally, but I prefer to do it locally in my solution/project. So, in the root of the project where 
the client should be placed, call this:

```
dotnet tool install Microsoft.OpenApi.Kiota
```

This will create a file `dotnet-tools.json` with this content:

```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "microsoft.openapi.kiota": {
      "version": "1.35.0",
      "commands": [
        "kiota"
      ],
      "rollForward": false
    }
  }
}
```

# Generating the client

We tell Kiota to pull the OpenAPI file directly from X. I would prefer to add the file to my repository for being able to compare it,
but this is probably against copyright rules.


In my sample, the client is placed in a subdir "Client", and the namespace is thus `XbyOpenApi.Core.Client`.  The class shall be `XClient`.
So we can create the client with this command:

```
dotnet kiota generate -l CSharp -c XClient -n XbyOpenApi.Core.Client -d https://api.twitter.com/2/openapi.json -o ./Client --exclude-backward-compatible
```

The last argument `--exclude-backward-compatible` is set because we don't need backwards compatible code. Omitting it might
cause different client code.

# Initializing the project

Add this Nuget package reference to client project to make it compile:

```xml
  <ItemGroup>
    <PackageReference Include="Microsoft.Kiota.Bundle" Version="2.1.1" />
  </ItemGroup>
```

# Using the client

This could be simple. For example this snippet should fetch the data of the current user:

```c#
XClient client = new XClient();

GetUsersMeResponse response = await client.Two.Users.Me.GetAsync();
```

But we need authorization for each API call. This is described in different sections [OAuth1](OAuth1.md) and [OAuth2](OAuth2.md).


# Hack: logging requests and responses

Unfortunately, Kiota does not provide an easy way log all requests and responses with a simple code snippet.

I found [this](https://github.com/microsoft/kiota-dotnet/issues/482) Kiota issue which pointed me to the concept of a 
body inspection handler. This handler must be registered for each request.

For example to view the raw response from the call to fetch the current user data:


```c#
XClient client = new XClient();

var requestOption = new BodyInspectionHandlerOption { InspectResponseBody = true };

GetUsersMeResponse response = await client.Two.Users.Me.GetAsync(conf =>
{
  conf.Options.Add(requestOption);
}

string plainResponse = GetStringFromStream(requestOption.ResponseBody);
```


The helper method `GetStringFromStream` is defined like this:

```c#
private static string GetStringFromStream(Stream stream)
{
  var reader = new StreamReader(stream);
  using (reader)
  {
    return reader.ReadToEnd();
  }
}
```

The same can be done for inspecting the request. Here is a sample for posting a tweet:

```c#
var requestOption = new BodyInspectionHandlerOption { InspectRequestBody = true, InspectResponseBody = true };

CreatePostsRequest body = new CreatePostsRequest();
body.Text = "Sample post";

CreatePostsResponse response = await xClient.Two.Tweets.PostAsync(body, conf =>
{
  conf.Options.Add(requestOption);
});

string response = GetStringFromStream(requestOption.ResponseBody);
string request = GetStringFromStream(requestOption.RequestBody);
```


This trick might be helpful for error handling. Normally, Kiota creates error objects that contain the error message object from the service.
If you try to delete a tweet with an id that you dont' have write access (e.g. "100"), the service response message is this:

```json
{
  "detail": "You are not authorized to delete this Tweet.",
  "type": "about:blank",
  "title": "Forbidden",
  "status": 403
}
```

This neither matches any of the generated model subclasses of  "Problem" nor the "Error" class, which has different fields
and thus cannot be filled with the response data - the thrown error seems to be invalid.

In this situation, it helped to catch the `Error` class and read the fields from the `AdditionalData` property.

```c#
catch (Error error)
{
  string errorMessage = $"Error on deleting a tweet: StatusCode: {error.ResponseStatusCode}" + Environment.NewLine;
  foreach (var data in error.AdditionalData)
  {
    errorMessage += data.Key + ": " + data.Value + Environment.NewLine;
  }
  MessageBox.Show(this, errorMessage);
}
```

To see the actual error message, I added the `BodyInspectionHandlerOption` and parsed the request in the catch block,
which revealed the actual error.

I don't know whether the X openapi description is wrong here, or whether Kiota does not support this kind of error handling.


# What about other OpenAPI libraries?

Before switching to Kiota, I tested [NSwag](https://github.com/RicoSuter/NSwag/), but this fails miserably: it creates 
a REST client that does not even compile. And digging deeper reveals that it does not support the "oneOf" declaration which is heavily used by X,
NSwag just picks the first class: https://github.com/RicoSuter/NSwag/issues/3738
