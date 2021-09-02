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

        //public async Task<Disease> ToDiseaseAsync(DiseaseViewModel model, bool isNew)
        //{
        //    return new Disease
        //    {
        //        Description = model.Description,                
        //        //DiseaseType = await _context.DeseaseTypes.FindAsync(model.DiseaseTypeId)
        //    };
        //}

        //public DiseaseViewModel ToDiseaseViewModel(Disease disease)
        //{
        //    return new DiseaseViewModel
        //    {
        //        Id = disease.Id,
        //        Description = disease.Description,
        //        //DiseaseTypeId = disease.DiseaseType.Id,
        //        //DiseaseTypes = _combosHelper.GetComboDiseaseTypes()
               
        //    };
        //}

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
                UserType = user.UserType,
                UserTypes = _combosHelper.GetComboUserTypes()
            };
        }

       

   
    }
}
