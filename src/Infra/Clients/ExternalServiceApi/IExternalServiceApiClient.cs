using Refit;
using Domain.Models;

namespace Infra.Clients.ExternalServiceApi;

public interface IExternalServiceApiClient
{
    [Post("/somethings/create")]
    Task CreateSomethingAsync(Something something);
}
