using System.Collections.Generic;
using Application4Test.Application.Contracts;
using Application4Test.Application.Contracts.DTOs;

namespace EasyArchitecture.Tests.Stuff
{
    public class DummyDogFacade : IDogFacade
    {
        public IList<DogDto> GetDogs(DogDto dog)
        {
            List<DogDto> dogs = new List<DogDto>();

            dogs.Add(new DogDto(){Name = "DummyDog"});

            return dogs;
        }

        public DogDto CreateDog(DogDto dog)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateDog(DogDto dog)
        {
            throw new System.NotImplementedException();
        }

        public IList<DogDto> GetAllOldDogs(int age)
        {
            throw new System.NotImplementedException();
        }

        public IList<DogDto> GetAllDogs()
        {
            throw new System.NotImplementedException();
        }


        public void PutDogToSleep(DogDto dog)
        {
            throw new System.NotImplementedException();
        }


        public DogDto GetDog(int dog)
        {
            throw new System.NotImplementedException();
        }

    }
}