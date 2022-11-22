using infonetBackend.Data;
using infonetBackend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infonetBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private ApplicationDbContext _context;
        private readonly string AppDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PersonalController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [Route("getData")]
        public async Task<List<PersonalInformation>> Get()
        {
            List<PersonalInformation> personalInformation = new List<PersonalInformation>();
            // var personalInformation = GetEmployee();
            personalInformation = await (Task.Run(() => _context.PersonalInformations.ToList()));
            return personalInformation;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] PdfAndDoc pdfAndDoc)
        {
            string folder = "Files/";
            pdfAndDoc.FileUrl = await UploadImage(folder, pdfAndDoc.File);

            var personalInformation = new PersonalInformation()
            {
                Id = pdfAndDoc.Id,
                Name = pdfAndDoc.Name,
                CountryCode = pdfAndDoc.CountryCode,
                CityCode = pdfAndDoc.CityCode,
                LanguageSkills = pdfAndDoc.LanguageSkills,
                DateOfBirth = pdfAndDoc.DateOfBirth,
                FileUrl = pdfAndDoc.FileUrl
            };

             await _context.AddAsync(personalInformation);

             await _context.SaveChangesAsync();

            return Ok(personalInformation);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromForm] PersonalInformation personalInformation)
        {
            // int id = personalInformation.Id;
            PersonalInformation findPersonId = await (Task.Run(() => _context.PersonalInformations.Find(personalInformation.Id)));
            if (findPersonId != null)
            {
                findPersonId.Name = personalInformation.Name;
                findPersonId.CountryCode = personalInformation.CountryCode;
                findPersonId.CityCode = personalInformation.CityCode;
                findPersonId.LanguageSkills = personalInformation.LanguageSkills;
                findPersonId.DateOfBirth = personalInformation.DateOfBirth;
                _context.Update(findPersonId);
                _context.SaveChangesAsync();
                return Ok(findPersonId);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var findId = await (Task.Run(() => _context.PersonalInformations.Find(id)));
            if (findId != null)
            {
                _context.Remove(findId);
                _context.SaveChangesAsync();
                return Ok(findId);
            }
            return Ok(null);
        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return "/" + folderPath;
        }
    }
}
