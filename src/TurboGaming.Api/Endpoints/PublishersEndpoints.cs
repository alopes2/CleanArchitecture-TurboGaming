using EndpointsMapper.Types;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TurboGaming.Api.Resources;

namespace TurboGaming.Api.Endpoints;

public class PublishersEndpoints : IEndpoint
{
    // This is just a temporary solution while we don't have services.
    private static List<PublisherResource> _publisherResources = new()
    {
        new PublisherResource("1", "Publisher 1"),
        new PublisherResource("2", "Publisher 2"),
        new PublisherResource("3", "Publisher 3")
    };

    public void MapEndpoints(WebApplication app)
    {
        app.MapGet("/publishers", () =>
        {
            return Results.Ok(_publisherResources);
        })
        .WithTags("Publishers")
        .WithName("GetPublishers");

        app.MapGet("/publishers/{id}", ([FromRoute] string id) =>
        {
            var game = _publisherResources.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(game);
        })
        .WithTags("Publishers")
        .WithName("GetPublisher");

        app.MapPost("/publishers", async (
            [FromBody] SavePublisherResource savePublisherResource,
            IValidator<SavePublisherResource> validator) =>
        {
            var validationResult = await validator.ValidateAsync(savePublisherResource);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.Select(e => new ApiValidationError(e.PropertyName, e.ErrorMessage)));
            }

            var newPublisherId =  (_publisherResources.Max(g => int.Parse(g.Id)) + 1).ToString();

            var publisher = new PublisherResource(
                newPublisherId,
                savePublisherResource.Name
            );

            _publisherResources.Add(publisher);

            return Results.Created(publisher.Id, publisher);
        })
        .WithTags("Publishers")
        .WithName("CreatePublisher");

        app.MapPut("/publishers/{id}", async (
            [FromRoute] string id,
            [FromBody] SavePublisherResource savePublisherResource,
            IValidator<SavePublisherResource> validator) =>
        {
            var validationResult = await validator.ValidateAsync(savePublisherResource);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.Select(e => new ApiValidationError(e.PropertyName, e.ErrorMessage)));
            }

            var publisher = _publisherResources.FirstOrDefault(g => g.Id == id);

            if (publisher == null)
            {
                return Results.NotFound();
            }
            
            _publisherResources.Remove(publisher);

            _publisherResources.Add(publisher with { Name = savePublisherResource.Name });

            return Results.Ok(publisher);
        })
        .WithTags("Publishers")
        .WithName("UpdatePublisher");

        app.MapDelete("/publishers/{id}", ([FromRoute] string id) =>
        {
            var publisher = _publisherResources.FirstOrDefault(g => g.Id == id);

            if (publisher == null)
            {
                return Results.NotFound();
            }
            
            _publisherResources.Remove(publisher);

            return Results.NoContent();
        })
        .WithTags("Publishers")
        .WithName("DeletePublisher");
    }
}