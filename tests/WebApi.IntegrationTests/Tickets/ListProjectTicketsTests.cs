using System.Net.Http.Json;
using FluentAssertions;
using WebApi.IntegrationTests.Helpers;
using WebApi.Tests.Helpers.Builders;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Api.Dtos.Response.Common;
using WebApi.Api.Dtos.Response.Tickets;

namespace WebApi.IntegrationTests.Tickets;

public class ListProjectTicketsTests : BaseIntegrationTest
{
    public ListProjectTicketsTests(TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task 正常系_ListProjectTickets_デフォルトページネーション()
    {
        // Arrange
        var (_, project) = await LoginWithProjectAsync();
        var tickets = Enumerable.Range(0, 30)
            .Select(i => new TicketBuilder()
                .WithProjectId(project.Id)
                .WithTitle($"Ticket {i}")
                .Build());
        await SaveManyAsync(tickets);

        // Act
        var response = await Client.GetAsync($"/api/v1/projects/{project.Id}/tickets");
        response.EnsureSuccessStatusCode();
        var result = await response.Content
            .ReadFromJsonAsync<PaginatedResponse<TicketResponse>>();

        // Assert
        result!.Items.Count.Should().Be(20);
        result.TotalCount.Should().Be(30);
        result.PageNumber.Should().Be(1);
    }

    [Fact]
    public async Task 正常系_ListProjectTickets_ページサイズ()
    {
        // Arrange
        var (_, project) = await LoginWithProjectAsync();
        await SaveManyAsync(
            Enumerable.Range(0, 30)
                .Select(i => new TicketBuilder()
                    .WithProjectId(project.Id)
                    .Build()));

        // Act
        var response = await Client.GetAsync(
            $"/api/v1/projects/{project.Id}/tickets?pageSize=5");
        var result = await response.Content
            .ReadFromJsonAsync<PaginatedResponse<TicketResponse>>();

        // Assert
        result!.Items.Count.Should().Be(5);
    }

    [Fact]
    public async Task 正常系_ListProjectTickets_ページ番号()
    {
        // Arrange
        var (_, project) = await LoginWithProjectAsync();
        await SaveManyAsync(
            Enumerable.Range(0, 30)
                .Select(i => new TicketBuilder()
                    .WithProjectId(project.Id)
                    .WithTitle($"Ticket {i}")
                    .Build()));

        // Act
        var response = await Client.GetAsync(
            $"/api/v1/projects/{project.Id}/tickets?pageNumber=2&pageSize=10");
        var result = await response.Content
            .ReadFromJsonAsync<PaginatedResponse<TicketResponse>>();

        // Assert
        result!.Items.Count.Should().Be(10);
        result.PageNumber.Should().Be(2);
    }

    [Fact]
    public async Task 正常系_ListProjectTickets_ステータスでフィルタリング()
    {
        // Arrange
        var (_, project) = await LoginWithProjectAsync();
        await SaveAsync(
                new TicketBuilder()
                    .WithProjectId(project.Id)
                    .WithStatus(TicketStatus.StatusType.Todo)
                    .Build());
        await SaveAsync(
            new TicketBuilder()
                .WithProjectId(project.Id)
                .WithStatus(TicketStatus.StatusType.Done)
                .Build());

        // Act
        var response = await Client.GetAsync(
            $"/api/v1/projects/{project.Id}/tickets?status=Todo");
        var result = await response.Content
            .ReadFromJsonAsync<PaginatedResponse<TicketResponse>>();

        // Assert
        result!.Items.Count.Should().Be(1);
        result.Items.First().Status.Should().Be("Todo");
    }

    [Fact]
    public async Task 正常系_ListProjectTickets_担当者でフィルタリング()
    {
        // Arrange
        var (user, project) = await LoginWithProjectAsync();

        await SaveAsync(
            new TicketBuilder()
                .WithProjectId(project.Id)
                .WithAssigneeId(user.Id)
                .Build());
        await SaveAsync(
            new TicketBuilder()
                .WithProjectId(project.Id)
                .Build());

        // Act
        var response = await Client.GetAsync(
            $"/api/v1/projects/{project.Id}/tickets?assigneeId={user.Id}");
        var result = await response.Content
            .ReadFromJsonAsync<PaginatedResponse<TicketResponse>>();

        // Assert
        result!.Items.Count.Should().Be(1);
    }

    [Fact]
    public async Task 正常系_ListProjectTickets_作成日時でソート()
    {
        // Arrange
        var (_, project) = await LoginWithProjectAsync();

        await SaveAsync(
            new TicketBuilder()
                .WithProjectId(project.Id)
                .WithTitle("Old")
                .WithCreatedAt(DateTime.UtcNow.AddDays(-1))
                .Build());
        await SaveAsync(
            new TicketBuilder()
                .WithProjectId(project.Id)
                .WithTitle("New")
                .WithCreatedAt(DateTime.UtcNow)
                .Build());

        // Act
        var response = await Client.GetAsync(
            $"/api/v1/projects/{project.Id}/tickets?sortBy=createdAt&sortOrder=Desc");
        var result = await response.Content
            .ReadFromJsonAsync<PaginatedResponse<TicketResponse>>();

        // Assert
        result!.Items.First().Title.Should().Be("New");
    }

    [Fact]
    public async Task 正常系_ListProjectTickets_TotalCount()
    {
        // Arrange
        var (_, project) = await LoginWithProjectAsync();
        await SaveManyAsync(
            Enumerable.Range(0, 25)
                .Select(i => new TicketBuilder()
                    .WithProjectId(project.Id)
                    .Build()));

        // Act
        var response = await Client.GetAsync(
            $"/api/v1/projects/{project.Id}/tickets?pageSize=10");
        var result = await response.Content
            .ReadFromJsonAsync<PaginatedResponse<TicketResponse>>();

        // Assert
        result!.TotalCount.Should().Be(25);
    }

    [Fact]
    public async Task 正常系_ListProjectTickets_ページが大きすぎる場合空の結果を返す()
    {
        // Arrange
        var (_, project) = await LoginWithProjectAsync();
        await SaveManyAsync(
            Enumerable.Range(0, 10)
                .Select(i => new TicketBuilder()
                    .WithProjectId(project.Id)
                    .Build()));

        // Act
        var response = await Client.GetAsync(
            $"/api/v1/projects/{project.Id}/tickets?pageNumber=5&pageSize=10");
        var result = await response.Content
            .ReadFromJsonAsync<PaginatedResponse<TicketResponse>>();

        // Assert
        result!.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task 正常系_ListProjectTickets_プロジェクトScoped()
    {
        // Arrange
        var (_, project1) = await LoginWithProjectAsync();
        var project2 = await SaveAsync(new ProjectBuilder().Build());

        await SaveAsync(
            new TicketBuilder()
                .WithProjectId(project1.Id)
                .Build());
        await SaveAsync(
            new TicketBuilder()
                .WithProjectId(project2.Id)
                .Build());

        // Act
        var response = await Client.GetAsync(
            $"/api/v1/projects/{project1.Id}/tickets");
        var result = await response.Content
            .ReadFromJsonAsync<PaginatedResponse<TicketResponse>>();

        // Assert
        result!.Items.Count.Should().Be(1);
    }
}
