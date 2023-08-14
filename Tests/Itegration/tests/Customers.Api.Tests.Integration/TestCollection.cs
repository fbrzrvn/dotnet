using Microsoft.AspNetCore.Mvc.Testing;

namespace Customers.Api.Tests.Integration;

[CollectionDefinition("CustomerApi Collection")]
public class TestCollection : ICollectionFixture<WebApplicationFactory<IApiMarker>>
{
}

