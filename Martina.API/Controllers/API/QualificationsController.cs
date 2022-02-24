using Common.Models.Request;
using Martina.API.Data;
using Martina.API.Data.Entities;
using Martina.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Martina.API.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class QualificationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public QualificationsController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [HttpPost]
        public async Task<IActionResult> PostQualification([FromBody] QualificationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            User userQualification = await _context.Users
                .Include(p => p.Qualifications)
                .FirstOrDefaultAsync(p => p.Id == request.UserId);

            if (userQualification == null)
            {
                return NotFound("Error002");
            }

            if (userQualification.Qualifications == null)
            {
                userQualification.Qualifications = new List<Qualification>();
            }

            userQualification.Qualifications.Add(new Qualification
            {
                Date = DateTime.UtcNow,
                UserQualified = userQualification,
                UserQualifyingId = user.Id,           
                Remarks = request.Remarks,
                Score = request.Score,
                //UserQualifications = userQualification
            });

            _context.Users.Update(userQualification);
            await _context.SaveChangesAsync();
            return Ok(userQualification);
        }
    }
}
