# httpstat.us

Welcome to [httpstat.us](https://httpstat.us), your simplest way to test HTTP status codes!

Point your app to [httpstat.us](https://httpstat.us) and append the status code you want to test, then make a request and we'll return that for you.

```js
async function getData(url) {
  const res = await fetch(url);

  if (!res.ok) {
    throw new Error("Failed to get data");
  }
  return await res.json();
}

getData("https://httpstat.us/500").then(console.log).catch(console.error);
```

Learn more at [httpstat.us](https://httpstat.us).

## Tech

The site is .NET 9 and it is hosted as a [containerised Azure AppService](https://azure.microsoft.com/services/app-service/containers/?WT.mc_id=dotnet-00000-aapowell#overview).

## Self hosting

If you want, you are able to self-host the service using the provided image, which can be found [on GitHub packages](https://github.com/aaronpowell/httpstatus/pkgs/container/httpstatus). This may be useful for testing HTTP status codes which are not available in Azure, or time outs longer than we support in the hosted version.

## LICENSE

[License](./License.md).
