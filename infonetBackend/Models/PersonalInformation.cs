using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace infonetBackend.Models
{
    public class PersonalInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string LanguageSkills { get; set; }
        public string DateOfBirth { get; set; }
        public string FileUrl { get; set; }
    }

    public class PdfAndDoc
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string CityCode { get; set; }
        public string LanguageSkills { get; set; }/**/
        public string DateOfBirth { get; set; }
        public string FileUrl { get; set; }
        public IFormFile File { get; set; }
    }


}
