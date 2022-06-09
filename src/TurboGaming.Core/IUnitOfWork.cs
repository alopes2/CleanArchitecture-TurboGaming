using TurboGaming.Core.Repositories;

namespace TurboGaming.Core;

public interface IUnitOfWork : IDisposable
{
    IGameRespository Games { get; }

    IPublisherRepository Publishers { get; }

    Task CommitAsync();
}