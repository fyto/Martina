using Martina.API.Data;
using Martina.API.Data.Entities;
using Martina.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        public async Task<UserDisease> ToUserDisease(DiseasesByUserViewModel model)
        {
            return new UserDisease
            {
                UserId = model.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DiseaseName = model.DiseaseName,
                DiseaseId = model.DiseaseId
            };
        }

        public async Task<Disease> ToDiseaseAsync(AddDiseaseViewModel model, bool isNew)
        {
            return new Disease
            {
                //Id = isNew ? 0 : model.Id,
                Description = model.Description,
                DiseaseType = await _context.DeseaseTypes.FindAsync(model.DiseaseTypeId)
            };
        }

        public async Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                Address = model.Address,
                Email = model.Email,
                FirstName = model.FirstName,
                Id = isNew ? Guid.NewGuid().ToString() : model.Id,
                ImageId = imageId,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email,
                UserType = model.UserType
            };
        }

        public async Task<UserViewModel> ToUserViewModel(User user)
        {
            return new UserViewModel
            {
                Address = user.Address,
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                ImageId = user.ImageId,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            };
        }

       

   
    }
}
