using Martina.API.Data.Entities;
using Martina.API.Helpers;
using Martina.Common.Enums;
using Microsoft.EntityFrameworkCore;
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
            await CheckDiseasesAsync();

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
                _context.Cares.Add(new Care { Description = "Aseo personal",
                                              CreationDate = DateTime.Now,
                                              FirstName = "Cristofher",
                                              LastName = "Ambiado" });
                _context.Cares.Add(new Care { Description = "Beber 2 litros de agua",
                                              CreationDate = DateTime.Now,
                                              FirstName = "Cristofher",
                                              LastName = "Ambiado" });
                _context.Cares.Add(new Care { Description = "Alimentación saludable" ,
                                              CreationDate = DateTime.Now,
                                              FirstName = "Cristofher",
                                              LastName = "Ambiado"});
                _context.Cares.Add(new Care { Description = "Cuidados de la piel",
                                              CreationDate = DateTime.Now,
                                              FirstName = "Cristofher",
                                              LastName = "Ambiado"});
                _context.Cares.Add(new Care { Description = "Usar ropa de algodón",
                                              CreationDate = DateTime.Now,
                                              FirstName = "Cristofher",
                                              LastName = "Ambiado"});
                _context.Cares.Add(new Care { Description = "Utilizar bastón o andador",
                                              CreationDate = DateTime.Now,
                                              FirstName = "Cristofher",
                                              LastName = "Ambiado"});
                _context.Cares.Add(new Care { Description = "Estimulación",
                                              CreationDate = DateTime.Now,
                                              FirstName = "Cristofher",
                                              LastName = "Ambiado"});

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

        private async Task CheckDiseasesAsync()
        {
            if (!_context.Deseases.Any())
            {

                var TranstornoMentalList = await _context.DeseaseTypes.Where(x => x.Description.Equals("Trastornos mentales y del comportamiento")).ToListAsync();
                var TranstornoMental = TranstornoMentalList.FirstOrDefault();

                var OncologicaList = await _context.DeseaseTypes.Where(x => x.Description.Equals("Oncológicas")).ToListAsync();
                var Oncologica = OncologicaList.FirstOrDefault();

                var DeLaSangreList = await _context.DeseaseTypes.Where(x => x.Description.Equals("De la sangre")).ToListAsync();
                var DeLaSangre = OncologicaList.FirstOrDefault();

                _context.Deseases.Add(new Disease
                {
                    Description = "Alzheimer",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(TranstornoMental.Id)     
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Demencia senil",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(TranstornoMental.Id)
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Parkinson",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(TranstornoMental.Id)
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Ictus",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(TranstornoMental.Id)
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Amiloidosis",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(Oncologica.Id)
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Cáncer",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(Oncologica.Id)
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Cáncer ampular",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(Oncologica.Id)
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Cáncer de boca",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(Oncologica.Id)
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Cáncer de huesos",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(Oncologica.Id)
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Cáncer de próstata",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(Oncologica.Id)
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Leucemia",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(DeLaSangre.Id)
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Hemocromatosis",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(DeLaSangre.Id)
                });

                _context.Deseases.Add(new Disease
                {
                    Description = "Hemofilia",
                    DiseaseType = await _context.DeseaseTypes.FindAsync(DeLaSangre.Id)
                });

                await _context.SaveChangesAsync();
            }
        }


    }
}
