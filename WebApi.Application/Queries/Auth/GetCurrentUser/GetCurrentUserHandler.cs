using AutoMapper;
using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Application.Queries.Users.Dtos;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Queries.Auth.GetCurrentUser;

public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetCurrentUserHandler(
        IUserContext userContext, IUserRepository userRepository, IMapper mapper)
    {
        _userContext = userContext;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(
        GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(_userContext.Id)
            ?? throw new AuthenticationException("USER_NOT_FOUND", "User が見つかりません");
        return _mapper.Map<UserDto>(user);
    }
}
