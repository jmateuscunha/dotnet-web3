using application.api.startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq.AutoMock;
using sample.test.api.Integrations.Configuration;

namespace sample.test.api.Integrations;

[Collection(nameof(IntegrationApiTestsFixtureCollection))]
public class TransactionServiceTest
{
    private readonly AutoMocker _mocker;
    private readonly IntegrationTestsFixture<StartupIntegrationTest> _testsFixture;
    public TransactionServiceTest(IntegrationTestsFixture<StartupIntegrationTest> testsFixture)
    {
        _testsFixture = testsFixture;
    }

    [Fact(DisplayName = "Get gas price")]
    [Trait("Category", "Transaction")]
    public async Task Transaction_GetGasPrice_WithSuccess()
    {     
        // Act
        var postResponse = await _testsFixture.Client.GetAsync("api/transaction/gas");
        var contentString = await postResponse.Content.ReadAsStringAsync();

        // Assert
        postResponse.EnsureSuccessStatusCode();
        Assert.NotEmpty(contentString);
    }
}