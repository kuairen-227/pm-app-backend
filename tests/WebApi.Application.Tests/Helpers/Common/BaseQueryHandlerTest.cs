using AutoMapper;
using Moq;

namespace WebApi.Application.Tests.Helpers.Common;

public abstract class BaseQueryHandlerTest
{
    protected readonly Mock<IMapper> Mapper;

    protected BaseQueryHandlerTest()
    {
        Mapper = TestHelpers.CreateMapper();
    }
}
