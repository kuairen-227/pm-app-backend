using AutoMapper;
using MediatR;
using WebApi.Application.Common.Pagination;
using WebApi.Application.Queries.Notifications.Dtos;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Queries.Notifications.ListUserNotifications;

public class ListUserNotificationsHandler : IRequestHandler<ListUserNotificationsQuery, PagedResultDto<NotificationDto>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;

    public ListUserNotificationsHandler(INotificationRepository notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<NotificationDto>> Handle(ListUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        var result = await _notificationRepository.ListByRecipientIdAsync(
            request.RecipientId,
            request.Pagination.Skip,
            request.Pagination.PageSize,
            request.Sorting.SortBy,
            request.Sorting.SortOrder,
            cancellationToken
        );

        return new PagedResultDto<NotificationDto>
        {
            Items = _mapper.Map<IReadOnlyList<NotificationDto>>(result.Items),
            TotalCount = result.TotalCount,
            PageNumber = request.Pagination.PageNumber,
            PageSize = request.Pagination.PageSize
        };
    }
}
