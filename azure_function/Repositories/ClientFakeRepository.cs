using azure_function.Models;

namespace azure_function.Repositories
{
    public class ClientFakeRepository : IClientFakeRepository
    {
        public ClientDetailModel GetClientDetails(int clientId)
        {
            if (clientId != 1) return null;

            return new ClientDetailModel { Id = 1, Name = "Client 1" };
        }
    }

    public interface IClientFakeRepository
    {
        ClientDetailModel GetClientDetails(int clientId);
    }
}
