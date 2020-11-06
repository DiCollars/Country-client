using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Client.Models;
using Country_client.Models;
using Country_client.Helper;
using System;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        public ApiCatcher _api = new ApiCatcher();
        public ApplicationContext _context;

        public HomeController(ApplicationContext context)
        {
            _context = context;
        }

        public  async Task<RedirectToActionResult> AddData(string name, string code, string capital, string area, string population, string region)
        {
            int capitalId = 0;
            int regionId = 0;
            
            if (!_context.Cities.All(city => city.Name != capital))
            {
                capitalId = (from city in _context.Cities where city.Name == capital select city.Id).First();
            }

            else
            {
                City currentCity = new City() { Name = capital };
                _context.Cities.Add(currentCity);
                _context.SaveChanges();
                capitalId = (from city in _context.Cities where city.Name == capital select city.Id).First();
            }

            if (!_context.Regions.All(regi => regi.Name != region))
            {
                regionId = (from regi in _context.Regions where regi.Name == region select regi.Id).First();
            }

            else
            {
                Region currentRegion = new Region() { Name = region};
                _context.Regions.Add(currentRegion);
                _context.SaveChanges();
                regionId = (from regi in _context.Regions where regi.Name == region select regi.Id).First();
            }

            if (!_context.Countries.All(countr => countr.Code != code))
            {
                var currentCountry = from country in _context.Countries where country.Code == code select country;

                currentCountry.First().Area = Convert.ToDouble(area);
                currentCountry.First().Code = code;
                currentCountry.First().CapitalId = capitalId;
                currentCountry.First().Name = name;
                currentCountry.First().Population = Convert.ToInt32(population);
                currentCountry.First().RegionId = regionId;
                _context.Countries.Update(currentCountry.First());
            }

            else
            {
                Country newCountry = new Country()
                {
                    Area = Convert.ToDouble(area),
                    Code = code,
                    CapitalId = capitalId,
                    Name = name,
                    Population = Convert.ToInt32(population),
                    RegionId = regionId
                };
                _context.Countries.Add(newCountry);
                
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Search", "Home");
        }

        public async Task<IActionResult> Search(string? request)
        {
            if (request == default)
            {
                return View();
            }

            else
            {
                List<CountryJson> country = new List<CountryJson>();
                HttpClient client = _api.Initial();
                HttpResponseMessage response = await client.GetAsync("/rest/v2/name/" + request);

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    country = JsonConvert.DeserializeObject<List<CountryJson>>(result);

                    City capital = new City(){ Name = country.First().capital};

                    Region region = new Region(){ Name = country.First().region };

                    Country countryForDb = new Country()
                    {
                        Name = country.First().demonym,
                        Code = country.First().callingCodes.First(),
                        Capital = capital,
                        Area = Convert.ToDouble(country.First().area),
                        Population = Convert.ToInt32(country.First().population),
                        Region = region
                    };

                    return View(countryForDb);
                }

                else
                {
                    ViewBag.Message = "Country not found.";
                    return View();
                }
            }
        }

        public IActionResult Display()
        {
            var AllCountries = from country in _context.Countries
                               select country;

            var All = _context.Countries.Include(p => p.Capital).Include(p => p.Region);

            return View(All.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
