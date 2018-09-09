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
    public class InsightControllerTests
    {
        [Fact]
        public void Post_WhenCalled_ReturnsInsightPosted()
        {
            // Arrange
            var service = new InsightsService("Post_WhenCalled_ReturnsInsightPosted");
            var controller = new InsightsController(service);


            var insight = new Insight()
            {
                Title = "Test Title",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow,
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };

            // Act
            var result = controller.Post(insight);
            
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<Insight>(((OkObjectResult)result).Value);
        }
        [Fact]
        public void Get_WhenCalled_ReturnsInsightPage()
        {
            // Arrange
            var service = new InsightsService("Get_WhenCalled_ReturnsInsightPage");
            var controller = new InsightsController(service);
            // Act
            var result = controller.Get();
            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<InsightPage>(((OkObjectResult)result).Value);
        }
        [Fact]
        public void Get_WhenCalled_ReturnsPostedInsight()
        {
            // Arrange
            var service = new InsightsService("Get_WhenCalled_ReturnsPostedInsight");
            var controller = new InsightsController(service);


            var insight = new Insight()
            {
                Title = "Test Title",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow,
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };

            // Act
            controller.Post(insight);
            var result = controller.Get();
            var insightResult = ((InsightPage)(((OkObjectResult)result).Value)).Insights[0];
            // Assert
            Assert.Equal(insight.Title, insightResult.Title);
            Assert.Equal(insight.Type, insightResult.Type);
            Assert.Equal(insight.DatePublished, insightResult.DatePublished);
            Assert.Equal(insight.Author, insightResult.Author);
            Assert.Equal(insight.Content, insightResult.Content);
        }
        [Fact]
        public void Get_WhenCalled_ReturnsOnlyActivePostedInsight()
        {
            // Arrange
            var service = new InsightsService("Get_WhenCalled_ReturnsPostedInsight");
            var controller = new InsightsController(service);


            var insight = new Insight()
            {
                Title = "Test Incative",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow,
                Author = "Adnan",
                Active = false,
                Content = "Post test content"
            };

            var insightActive = new Insight()
            {
                Title = "Test Active",
                Type = InsightType.Event,
                DatePublished = DateTime.UtcNow,
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };

            controller.Post(insight);
            controller.Post(insightActive);
            // Act
            var result = controller.Get();
            var insights = ((InsightPage)(((OkObjectResult)result).Value)).Insights;
            // Assert
            Assert.Single(insights);
            Assert.Equal(insightActive.Title, insights[0].Title);
            Assert.Equal(insightActive.Active, insights[0].Active);
        }
        [Fact]
        public void Get_WhenCalled_ReturnsOnlyPastPublishedPostedInsights()
        {
            // Arrange
            var service = new InsightsService("Get_WhenCalled_ReturnsOnlyPastPublishedPostedInsights");
            var controller = new InsightsController(service);


            var insight = new Insight()
            {
                Title = "Test Past",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow,
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };

            var insightFuture = new Insight()
            {
                Title = "Test Future",
                Type = InsightType.Event,
                DatePublished = DateTime.UtcNow.AddSeconds(1),
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };

            // Act
            controller.Post(insight);
            controller.Post(insightFuture);
            var result = controller.Get();
            var insights = ((InsightPage)(((OkObjectResult)result).Value)).Insights;
            // Assert
            Assert.Single(insights);
            System.Threading.Thread.Sleep(1000);
            result = controller.Get();
            insights = ((InsightPage)(((OkObjectResult)result).Value)).Insights;
            Assert.Equal(2, insights.Count);
        }
        [Fact]
        public void Get_WhenCalled_ReturnsInsightsInDatePublishedDescOrder()
        {
            // Arrange
            var service = new InsightsService("Get_WhenCalled_ReturnsInsightsInDatePublishedDescOrder");
            var controller = new InsightsController(service);

            var insightA = new Insight()
            {
                Title = "Test A",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow,
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };
            var insightB = new Insight()
            {
                Title = "Test B",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow.AddSeconds(-1),
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };
            var insightC = new Insight()
            {
                Title = "Test C",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow.AddSeconds(-2),
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };
            var insightD = new Insight()
            {
                Title = "Test  D",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow.AddSeconds(-3),
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };

            controller.Post(insightA);
            controller.Post(insightB);
            controller.Post(insightC);
            controller.Post(insightD);

            // Act
            var result = controller.Get();
            var insights = ((InsightPage)(((OkObjectResult)result).Value)).Insights;
            // Assert
            Assert.Equal(4, insights.Count);
            Assert.Equal(insightA.DatePublished, insights[0].DatePublished);
            Assert.Equal(insightB.DatePublished, insights[1].DatePublished);
            Assert.Equal(insightC.DatePublished, insights[2].DatePublished);
            Assert.Equal(insightD.DatePublished, insights[3].DatePublished);
        }
        [Fact]
        public void Get_WhenCalled_ReturnsOnlyPageddInsights()
        {
            // Arrange
            var service = new InsightsService("Get_WhenCalled_ReturnsOnlyPageddInsights");
            var controller = new InsightsController(service);

            var insightAPage1 = new Insight()
            {
                Title = "Test A Page 1",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow,
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };
            var insightBPage1 = new Insight()
            {
                Title = "Test B Page 1",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow.AddSeconds(-1),
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };
            var insightAPage2 = new Insight()
            {
                Title = "Test A Page 2",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow.AddSeconds(-2),
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };
            var insightBPage2 = new Insight()
            {
                Title = "Test  B Page 2",
                Type = InsightType.News,
                DatePublished = DateTime.UtcNow.AddSeconds(-3),
                Author = "Adnan",
                Active = true,
                Content = "Post test content"
            };

            controller.Post(insightAPage1);
            controller.Post(insightBPage1);
            controller.Post(insightAPage2);
            controller.Post(insightBPage2);

            // Act
            var result = controller.Get(2, 1);
            var insightsPage1 = ((InsightPage)(((OkObjectResult)result).Value)).Insights;
            result = controller.Get(2, 2);
            var insightsPage2 = ((InsightPage)(((OkObjectResult)result).Value)).Insights;
            // Assert
            Assert.Equal(2, insightsPage1.Count);
            Assert.Equal(insightAPage1.Title, insightsPage1[0].Title);
            Assert.Equal(insightBPage1.Title, insightsPage1[1].Title);
            Assert.Equal(2, insightsPage2.Count);
            Assert.Equal(insightAPage2.Title, insightsPage2[0].Title);
            Assert.Equal(insightBPage2.Title, insightsPage2[1].Title);
        }
    }
}
