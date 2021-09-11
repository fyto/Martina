using Martina.API.Data.Entities;
using Martina.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Helpers
{
    public interface IConverterHelper
    {
        Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew);

        Task<UserViewModel> ToUserViewModel(User user);

        Task<Disease> ToDiseaseAsync(AddDiseaseViewModel model, bool isNew);

        //DiseaseViewModel ToDiseaseViewModel(Disease disease);

    }
}
