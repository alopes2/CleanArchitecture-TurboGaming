using EndpointsMapper.Types;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TurboGaming.Api.Resources;

namespace TurboGaming.Api.Endpoints;

public class GamesEndpoints : IEndpoint
{
    // This is just a temporary solution while we don't have services.
    private static List<GameResource> _gameResources = new()
    {
        new GameResource("1", "Game 1", new PublisherResource("1", "Publisher 1")),
        new GameResource("2", "Game 2", new PublisherResource("2", "Publisher 2")),
        new GameResource("3", "Game 3", new PublisherResource("3", "Publisher 3"))
    };

    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("/games", () =>
        {
            return Results.Ok(_gameResources);
        })
        .WithTags("Games")
        .WithName("GetGames");

        app.MapGet("/games/{id}", ([FromRoute] string id) =>
        {
            var game = _gameResources.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(game);
        })
        .WithTags("Games")
        .WithName("GetGame");

        app.MapPost("/games", async (
            [FromBody] SaveGameResource saveGameResource,
            IValidator<SaveGameResource> validator) =>
        {
            var validationResult = await validator.ValidateAsync(saveGameResource);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.Select(e => new ApiValidationError(e.PropertyName, e.ErrorMessage)));
            }

            var newGameId =  (_gameResources.Max(g => int.Parse(g.Id)) + 1).ToString();
            var game = new GameResource(
                newGameId,
                saveGameResource.Name,
                new PublisherResource(saveGameResource.PublisherId, "Publisher 1")
            );

            _gameResources.Add(game);

            return Results.Created(game.Id, game);
        })
        .WithTags("Games")
        .WithName("CreateGame");

        app.MapPut("/games/{id}", async (
            [FromRoute] string id,
            [FromBody] SaveGameResource saveGameResource,
            IValidator<SaveGameResource> validator) =>
        {
            var validationResult = await validator.ValidateAsync(saveGameResource);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.Select(e => new ApiValidationError(e.PropertyName, e.ErrorMessage)));
            }
            
            var game = _gameResources.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return Results.NotFound();
            }

            _gameResources.Remove(game);

            _gameResources.Add(game with { Name = saveGameResource.Name });

            return Results.Ok(game);
        })
        .WithTags("Games")
        .WithName("UpdateGame");

        app.MapDelete("/games/{id}", async ([FromRoute] string id) =>
        {
            var game = _gameResources.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return Results.NotFound();
            }

            _gameResources.Remove(game);

            return Results.NoContent();
        })
        .WithTags("Games")
        .WithName("DeleteGame");
    }
}