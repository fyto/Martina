using Martina.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        //private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context) //, IUserHelper userHelper)
        {
            _context = context;
            //_userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            // Verifica que la BD exista
            await _context.Database.EnsureCreatedAsync();

            await CheckCaresAsync();
            await CheckDiseaseTypeAsync();
            //await CheckBrandsAsync();
            //await CheckDocumentTypesAsync();
            //await CheckProceduresAsync();
            //await CheckRolesAsycn();

            //await CheckUserAsync("1010", "Luis", "Salazar", "luis@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.Admin);
            //await CheckUserAsync("2020", "Juan", "Zuluaga", "zulu@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.User);
            //await CheckUserAsync("3030", "Ledys", "Bedoya", "ledys@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.User);
            //await CheckUserAsync("4040", "Sandra", "Lopera", "sandra@yopmail.com", "311 322 4620", "Calle Luna Calle Sol", UserType.Admin);
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
