using Martina.API.Data.Entities;
using Martina.API.Helpers;
using Martina.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            // Verifica que la BD exista
            await _context.Database.EnsureCreatedAsync();

            await CheckCaresAsync();
            await CheckDiseaseTypeAsync();

            await CheckRolesAsync();

            await CheckUserAsync("Cristofher", "Ambiado", "cristofher.ambiado@valoralabs.com", "58987975", "Latorre 1117, Concepción", UserType.Admin);
            await CheckUserAsync("Yohanna", "Ambiado", "yambiado@gmail.com", "8975298", "Venado 736, San pedro", UserType.Apoderado);
            await CheckUserAsync("Osvaldo", "Ambiado", "osvaldo@gmail.com", "81273393", "Latorre 1117", UserType.AdultoMayor);
            await CheckUserAsync("Tegualda", "Rodriguez", "tegualda@gmail.com", "87984656", "Latorre 1117", UserType.Cuidador);

        }

        private async Task CheckUserAsync(string firstName, string lastName, string email, string phoneNumber, string address, UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    Address = address,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    UserName = email,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                //string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                //await _userHelper.ConfirmEmailAsync(user, token);
            }
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Apoderado.ToString());
            await _userHelper.CheckRoleAsync(UserType.AdultoMayor.ToString());
            await _userHelper.CheckRoleAsync(UserType.Cuidador.ToString());
        }

        private async Task CheckCaresAsync()
        {
            if (!_context.Cares.Any())
            {
                _context.Cares.Add(new Care { Description = "Aseo personal" });
                _context.Cares.Add(new Care { Description = "Beber 2 litros de agua" });
                _context.Cares.Add(new Care { Description = "Alimentación saludable" });
                _context.Cares.Add(new Care { Description = "Cuidados de la piel" });
                _context.Cares.Add(new Care { Description = "Usar ropa de algodón" });
                _context.Cares.Add(new Care { Description = "Utilizar bastón o andador" });
                _context.Cares.Add(new Care { Description = "Estimulación" });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckDiseaseTypeAsync()
        {
            if (!_context.DeseaseTypes.Any())
            {
                _context.DeseaseTypes.Add(new DiseaseType { Description = "Oncológicas" });
                _context.DeseaseTypes.Add(new DiseaseType { Description = "Infecciosas y parasitarias" });
                _context.DeseaseTypes.Add(new DiseaseType { Description = "De la sangre" });
                _context.DeseaseTypes.Add(new DiseaseType { Description = "Del sistema inmunitario" });
                _context.DeseaseTypes.Add(new DiseaseType { Description = "Endocrinas" });
                _context.DeseaseTypes.Add(new DiseaseType { Description = "Trastornos mentales y del comportamiento" });
                _context.DeseaseTypes.Add(new DiseaseType { Description = "Del sistema nervioso" });

                await _context.SaveChangesAsync();
            }
        }


    }
}
