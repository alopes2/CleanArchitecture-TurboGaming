using OneOf;
using OneOf.Types;
using TurboGaming.Core.Models;

namespace TurboGaming.Core.Services;

public interface IGameService
{
    Task<IEnumerable<Game>> GetGamesAsync();

    Task<OneOf<Game, NotFound>> GetGameByIdAsync(string id);

    Task<Game> CreateGameAsync(Game game);

    Task<OneOf<Success, NotFound>> UpdateGameAsync(Game game);

    Task<OneOf<Success, NotFound>> DeleteGameAsync(string id);
}