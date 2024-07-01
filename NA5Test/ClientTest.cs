using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;

public class ClientTests
{
    [Test]
    public async Task SendMessage_Success()
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var serverUrl = "http://localhost/api/messages";

        await Program.SendMessage(httpClient, serverUrl, "Test message");

        mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post
                && req.RequestUri == new Uri(serverUrl + "/send_message")
            ),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Test]
    public async Task ListMessages_Success()
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var messages = new List<Message>
        {
            new Message { Id = 1, Content = "Message 1", Read = false, Timestamp = DateTime.UtcNow },
            new Message { Id = 2, Content = "Message 2", Read = false, Timestamp = DateTime.UtcNow }
        };

        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(messages)
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var serverUrl = "http://localhost/api/messages";

        var result = await Program.ListMessages(httpClient, serverUrl);

        Xunit.Assert.Equal(messages.Count, result.Count);
        Xunit.Assert.Equal(messages[0].Content, result[0].Content);
    }
}