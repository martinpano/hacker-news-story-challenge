using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Moq;
using HackerNews.Stories.Services.Services;
using System.Threading;
using Moq.Protected;
using Microsoft.Extensions.Logging;
using System;

namespace HackerNews.Stories.Tests
{
    [TestClass]
    public class HttpServiceTests
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _mockHttpClient;
        private HttpService _httpService;
        private Mock<ILogger<HttpService>> _loggerMock;

        [TestInitialize]
        public void Initialize()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockHttpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _loggerMock = new Mock<ILogger<HttpService>>();

            _httpClientFactoryMock.Setup(x => x.CreateClient("HackerNewsClient")).Returns(_mockHttpClient);
            _loggerMock.Setup(x => x.Log(
        It.IsAny<LogLevel>(),
        It.IsAny<EventId>(),
        It.IsAny<It.IsAnyType>(),
        It.IsAny<Exception>(),
        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));


            _httpService = new HttpService(_httpClientFactoryMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetData_Success_ReturnsDeserializedObject()
        {
            // Arrange
            var expectedResponse = new { data = "test" };
            var url = "https://example.com/api/data";
            var responseContent = JsonConvert.SerializeObject(expectedResponse);
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _httpService.GetData<object>(url);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse.data, ((dynamic)result).data.ToString());
        }

        [TestMethod]
        public async Task GetData_Failure_LogsError()
        {
            // Arrange
            // Wrong url on purpose to trigger failure
            var url = "https:/example.com/api/data";
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("")
                });

            // Act
            await _httpService.GetData<object>(url);

            // Assert
            Assert.AreEqual(1, _loggerMock.Invocations.Count);
        }
    }
}
