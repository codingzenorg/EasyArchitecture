using Application4Test.Domain;
using Application4Test.Domain.Repositories;
using EasyArchitecture.Persistence.Plugin.BultIn;

namespace Application4Test.Infrastructure.Persistence.Repositories
{
    public class DogMemoryRepository : MemoryRepository<Dog>, IDogRepository
    {


    }
}