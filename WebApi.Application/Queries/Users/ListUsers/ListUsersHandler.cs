using AutoMapper;
using MediatR;
using WebApi.Application.Queries.Users.Dtos;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Queries.Users.ListUsers;

public class ListUsersHandler : IRequestHandler<ListUsersQuery, IReadOnlyList<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ListUsersHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<UserDto>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.ListAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<UserDto>>(users);
    }
}
