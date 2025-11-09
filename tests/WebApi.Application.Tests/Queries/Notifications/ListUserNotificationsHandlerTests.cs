using FluentAssertions;
using Moq;
using WebApi.Application.Common.Pagination;
using WebApi.Application.Queries.Notifications.Dtos;
using WebApi.Application.Queries.Notifications.ListUserNotifications;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Common;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Queries.Notifications;

public class ListUserNotificationsHandlerTests : BaseQueryHandlerTest
{
    private readonly ListUserNotificationsHandler _handler;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly NotificationBuilder _notificationBuilder;
    private readonly UserBuilder _userBuilder;

    public ListUserNotificationsHandlerTests()
    {
        _notificationRepository = new Mock<INotificationRepository>();
        _notificationBuilder = new NotificationBuilder();
        _userBuilder = new UserBuilder();

        Mapper.Setup(m => m.Map<IReadOnlyList<NotificationDto>>(It.IsAny<IEnumerable<Notification>>()))
            .Returns<IEnumerable<Notification>>(notifications =>
                notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Category = n.Category.Value.ToString(),
                    RecipientId = n.RecipientId,
                    SubjectId = n.SubjectId,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedBy = n.AuditInfo.CreatedBy,
                    CreatedAt = n.AuditInfo.CreatedAt,
                    UpdatedBy = n.AuditInfo.UpdatedBy,
                    UpdatedAt = n.AuditInfo.UpdatedAt
                }).ToList());

        _handler = new ListUserNotificationsHandler(
            _notificationRepository.Object,
            Mapper.Object);
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var user = _userBuilder.Build();
        var notifications = new List<Notification>
        {
            _notificationBuilder
                .WithRecipientId(user.Id)
                .WithCategory(NotificationCategory.Category.ProjectMemberInvited)
                .WithMessage("通知1")
                .Build(),
            _notificationBuilder
                .WithRecipientId(user.Id)
                .WithCategory(NotificationCategory.Category.TicketCreated)
                .WithMessage("通知2")
                .Build()
        };

        _notificationRepository
            .Setup(r => r.ListByRecipientIdAsync(
                user.Id,
                0, 20, "UpdatedAt", SortOrder.Desc,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<Notification>(notifications, notifications.Count));

        // Act
        var query = new ListUserNotificationsQuery(user.Id);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultDto<NotificationDto>>();

        result.TotalCount.Should().Be(notifications.Count);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(20);

        result.Items.Should().BeEquivalentTo(
            notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Category = n.Category.Value.ToString(),
                RecipientId = n.RecipientId,
                SubjectId = n.SubjectId,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedBy = n.AuditInfo.CreatedBy,
                CreatedAt = n.AuditInfo.CreatedAt,
                UpdatedBy = n.AuditInfo.UpdatedBy,
                UpdatedAt = n.AuditInfo.UpdatedAt
            }),
            options => options.WithStrictOrdering()
        );

        _notificationRepository.Verify(r => r.ListByRecipientIdAsync(
            user.Id,
            0, 20, "UpdatedAt", SortOrder.Desc,
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }
}
