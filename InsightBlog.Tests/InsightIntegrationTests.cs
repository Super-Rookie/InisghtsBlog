using InsightBlog.Controllers;
using InsightBlog.Logic;
using InsightBlog.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace InsightBlog.Tests
{
    public class InsightInterngrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public InsightInterngrationTests()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                           .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        [Fact]
        public async void Get_WhenCalled_ReturnsInsightPage()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/insights");
            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            // Assert 
            Assert.IsType<InsightPage>(JsonConvert.DeserializeObject<InsightPage>(content));
        }
        [Fact]
        public async void Post_WhenCalled_FailsWithInvalidImageUrlJson()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/insights");

            var insight = new Insight()
            {
                Title = "Test Title",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow,
                Author = "Adnan",
                Active = true,
                Content = "Post test content",
                ImageSource = "Invalid URL"
            };

            var content = JsonConvert.SerializeObject(insight);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/insights", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async void Post_WhenCalled_FailsWithTitleOver200Characters()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/insights");

            var insight = new Insight()
            {
                Title = "This title is over two hundred characters so the test should fail with a bad request due to this fact. If it passes the attributes for the model will need to be double checked to see why this test is not failing.",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow,
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };

            var content = JsonConvert.SerializeObject(insight);
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/insights", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async void Post_WhenCalled_FailsWithMissingTitle()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/insights");

            var insight = @"
            {
                ""Type"":""News"",
                ""DatePublished"":""2018-09-09T12:00:00.000Z"",
                ""Author"":""Adnan"",
                ""Active"":""true"",
                ""Content"":""Post test content""
            }";
            var stringContent = new StringContent(insight, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/insights", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async void Post_WhenCalled_FailsWithMissingType()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/insights");

            var insight = @"
            {
                ""Title"":""Test Title"",
                ""DatePublished"":""2018-09-09T12:00:00.000Z"",
                ""Author"":""Adnan"",
                ""Active"":""true"",
                ""Content"":""Post test content""
            }";
            var stringContent = new StringContent(insight, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/insights", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async void Post_WhenCalled_FailsWithInvalidType()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/insights");

            var insight = @"
            {
                ""Title"":""Test Title"",
                ""Type"":""Invalid"",
                ""DatePublished"":""2018-09-09T12:00:00.000Z"",
                ""Author"":""Adnan"",
                ""Active"":""true"",
                ""Content"":""Post test content""
            }";
            var stringContent = new StringContent(insight, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/insights", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async void Post_WhenCalled_FailsWithMissingDatePublished()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/insights");

            var insight = @"
            {
                ""Title"":""Test Title"",
                ""Type"":""News"",
                ""Author"":""Adnan"",
                ""Active"":""true"",
                ""Content"":""Post test content""
            }";
            var stringContent = new StringContent(insight, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/insights", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async void Post_WhenCalled_FailsWithMissingAuthor()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/insights");

            var insight = @"
            {
                ""Title"":""Test Title"",
                ""Type"":""News"",
                ""DatePublished"":""2018-09-09T12:00:00.000Z"",
                ""Active"":""true"",
                ""Content"":""Post test content""
            }";
            var stringContent = new StringContent(insight, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/insights", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async void Post_WhenCalled_FailsWithMissingActive()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/insights");

            var insight = @"
            {
                ""Title"":""Test Title"",
                ""Type"":""News"",
                ""DatePublished"":""2018-09-09T12:00:00.000Z"",
                ""Author"":""Adnan"",
                ""Content"":""Post test content""
            }";
            var stringContent = new StringContent(insight, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/insights", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async void Post_WhenCalled_FailsWithMissingContent()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/insights");

            var insight = @"
            {
                ""Title"":""Test Title"",
                ""Type"":""News"",
                ""DatePublished"":""2018-09-09T12:00:00.000Z"",
                ""Author"":""Adnan"",
                ""Active"":""true""
            }";
            var stringContent = new StringContent(insight, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PostAsync("/insights", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
