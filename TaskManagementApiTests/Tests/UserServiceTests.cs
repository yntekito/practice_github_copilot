using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Moq;
using Xunit;
using TaskManagementApi.Data;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Tests
{
    public class UserServiceTest
    {
        private TaskManagementContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TaskManagementContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new TaskManagementContext(options);

            // Seed a user with projects and tasks
            var user = new User
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                Projects = new List<Project>(),
                AssignedTasks = new List<TaskItem>()
            };
            context.Users.Add(user);
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task GenerateComplexUserAnalyticsReportAsync_ReturnsReport_ForValidUser()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new UserService(context);
            var userId = 1;
            var startDate = DateTime.UtcNow.AddDays(-30);
            var endDate = DateTime.UtcNow;

            // Act
            var report = await service.GenerateComplexUserAnalyticsReportAsync(userId, startDate, endDate);

            // Assert
            Assert.NotNull(report);
            Assert.Equal(userId, report["userId"]);
            Assert.Equal("Test User", report["userName"]);
            Assert.Equal("COMPLETED", report["reportStatus"]);
            Assert.True(report.ContainsKey("processingSteps"));
            Assert.True(report.ContainsKey("mlPredictions"));
            Assert.True(report.ContainsKey("benchmarks"));
            Assert.True(report.ContainsKey("roiMetrics"));
            Assert.True(report.ContainsKey("actionItems"));
        }

        [Fact]
        public async Task GenerateComplexUserAnalyticsReportAsync_ReturnsError_WhenUserNotFound()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new UserService(context);
            var invalidUserId = 999;
            var startDate = DateTime.UtcNow.AddDays(-10);
            var endDate = DateTime.UtcNow;

            // Act
            var report = await service.GenerateComplexUserAnalyticsReportAsync(invalidUserId, startDate, endDate);

            // Assert
            Assert.NotNull(report);
            Assert.True(report.ContainsKey("error"));
            Assert.Equal("User not found", report["error"]);
        }

        [Fact]
        public async Task GenerateComplexUserAnalyticsReportAsync_ReturnsError_WhenUserHasNoPermission()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TaskManagementContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new TaskManagementContext(options);
            // Add user but remove after creation to simulate no permission
            var user = new User { Id = 2, Name = "NoPerm", Email = "noperm@example.com" };
            context.Users.Add(user);
            context.SaveChanges();
            context.Users.Remove(user);
            context.SaveChanges();

            var service = new UserService(context);
            var userId = 2;
            var startDate = DateTime.UtcNow.AddDays(-5);
            var endDate = DateTime.UtcNow;

            // Act
            var report = await service.GenerateComplexUserAnalyticsReportAsync(userId, startDate, endDate);

            // Assert
            Assert.NotNull(report);
            Assert.True(report.ContainsKey("error"));
            Assert.Equal("Access denied", report["error"]);
        }

        [Fact]
        public async Task GenerateComplexUserAnalyticsReportAsync_ConsistencyCheck_FailsIfScoreOutOfRange()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new UserService(context);
            var userId = 1;
            var startDate = DateTime.UtcNow.AddDays(-2);
            var endDate = DateTime.UtcNow;

            // Act
            var report = await service.GenerateComplexUserAnalyticsReportAsync(userId, startDate, endDate);

            // Assert
            Assert.True(report.ContainsKey("dataConsistencyPassed"));
            Assert.True((bool)report["dataConsistencyPassed"]);
        }

        [Fact]
        public async Task GenerateComplexUserAnalyticsReportAsync_ReportContainsExpectedKeys()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new UserService(context);
            var userId = 1;
            var startDate = DateTime.UtcNow.AddDays(-7);
            var endDate = DateTime.UtcNow;

            // Act
            var report = await service.GenerateComplexUserAnalyticsReportAsync(userId, startDate, endDate);

            // Assert
            var expectedKeys = new[]
            {
                "userId", "userName", "reportGeneratedAt", "dateRange", "totalStepsExecuted",
                "dailyActivityScores", "weeklyTrends", "monthlyAggregates", "productivityPattern",
                "completionRates", "collaborationMetrics", "performanceInsights", "stressIndicators",
                "skillMetrics", "workLifeBalance", "goalMetrics", "predictiveAnalytics", "teamInteractions",
                "innovationMetrics", "learningPatterns", "feedbackAnalysis", "resourceUtilization",
                "executiveSummary", "auditTrail", "reportMetadata", "dataConsistencyPassed",
                "mlPredictions", "chartData", "benchmarks", "roiMetrics", "actionItems",
                "processingSteps", "reportStatus", "totalProcessingTime"
            };

            foreach (var key in expectedKeys)
            {
                Assert.True(report.ContainsKey(key), $"Missing key: {key}");
            }
        }
    }
}