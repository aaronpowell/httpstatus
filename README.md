# httpstat.us

Welcome to [httpstat.us](https://httpstat.us), your simplest way to test HTTP status codes!

Point your app to [httpstat.us](https://httpstat.us) and append the status code you want to test, then make a request and we'll return that for you.

```js
async function getData(url) {
  const res = await fetch(url);

  if (!res.ok) {
    throw new Error('Failed to get data');
  }
  return await res.json();
}

getData('https://httpstat.us/500').then(console.log).catch(console.error);
```

Learn more at [httpstat.us](https://httpstat.us).

## Tech

The site is .NET 6 and it is hosted as a [containerised Azure AppService](https://azure.microsoft.com/services/app-service/containers/?WT.mc_id=dotnet-00000-aapowell#overview).

## LICENSE

[License](./License.md).
