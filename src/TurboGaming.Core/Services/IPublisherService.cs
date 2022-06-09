using OneOf;
using OneOf.Types;
using TurboGaming.Core.Models;

namespace TurboGaming.Core.Services;

public interface IPublisherService
{
    Task<IEnumerable<Publisher>> GetPublishersAsync();

    Task<OneOf<Publisher, NotFound>> GetPublisherByIdAsync(string id);

    Task<Publisher> CreatePublisherAsync(Publisher publisher);

    Task<OneOf<Success, NotFound>> UpdatePublisherAsync(Publisher publisher);

    Task<OneOf<Success, NotFound>> DeletePublisherAsync(string id);
}
