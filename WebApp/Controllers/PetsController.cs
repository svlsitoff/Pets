using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PetsController : Controller
    {
        private readonly MyDbContext _context;

        public PetsController(MyDbContext context)
        {
            _context = context;
            if (!_context.Shelter.Any())
            {
                Shelter shelter = new Shelter { AmountFunds = 1000, Place = 19 };
                _context.Shelter.Add(shelter);
                _context.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> Index()
        {
            Shelter shelter = _context.Shelter.FirstOrDefault();
            ViewBag.Places = shelter.Place;
            ViewBag.Money = shelter.AmountFunds;
            var pets = await _context.Pets.ToListAsync();
            return View(pets);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        public IActionResult Create()
        {
            return View(PetGenerate());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PetViewModel model)
        {
            var shelter = _context.Shelter.FirstOrDefault();
            if (shelter.Place <= 0)
            {
                return RedirectToAction(nameof(NoPlaces));
            }

            if (ModelState.IsValid)
            {

                Pet pet = new Pet
                {
                    Name = model.Name,
                    Age = model.Age,
                    Kind = model.Kind,
                    PetPicture = model.PetPicture,
                    HealthStatus = model.HealthStatus,
                    Health = model.Health

                };
                _context.Add(pet);
                shelter.Place--;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }
            return View(pet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pet pet)
        {
            if (id != pet.ID)
            {
                return NotFound();
            }

            Shelter shelter = _context.Shelter.FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    if (pet.Health == 1)
                    {
                        if (shelter.AmountFunds >= 146.00)
                        {
                            shelter.AmountFunds -= 146.00;
                        }
                        else
                        {
                            return RedirectToAction(nameof(NoMoney));
                        }
                    }
                    if (pet.Health == 0)
                    {
                        if (shelter.AmountFunds >= 243.00)
                        {
                            shelter.AmountFunds -= 243.00;

                        }
                        else
                        {
                            return RedirectToAction(nameof(NoMoney));
                        }
                    }

                    pet.Health = 2;
                    pet.HealthStatus = "The state of health is good. Treatment and recovery activities are not required";
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pet);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Random random = new Random();
            int donate = random.Next(10, 300);
            var shelter = _context.Shelter.FirstOrDefault();
            shelter.AmountFunds += donate;
            shelter.Place++;
            var pet = await _context.Pets.FindAsync(id);
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.ID == id);
        }

        /// <summary>
        /// I have defined this method to generate the "Pet" object
        /// </summary>
        private PetViewModel PetGenerate()
        {
            string[] CatsNames = { "Oliver", "Lea", "Milo", "Charlie", "Max", "Jack", "Simba", "Loki", "Luna", "Bella", "Lily", "Lucy", "Kitty", "Calie", "Nala", "Zoe" };
            string[] DogsNames = { "Bella", "Molly", "Coco", "Ruby", "Lucy", "Baily", "Daisy", "Rosie", "Charlie", "Max", "Buddy", "Oscar", "Milo", "Archie", "Ollie", "Toby" };
            string[] Healthstatus = {
                "Sick. Revealed severe exhaustion, damage to the skin, as well as diseases of the organs of vision.It is required to carry out drug treatment and a set of restorative measures for the pet.",
                "Satisfactory: Slight damage to the skin, emaciation. It is necessary to take measures to restore weight and balance vitamins and nutrients, as well as to cleanse skin parasites.",
                "The state of health is good. Treatment and recovery activities are not required"

            };
            string[] CatsImages = { "1cat.jpg", "2cat.jpg", "3cat.jpg", "4cat.jpg", "5cat.jpg", "6cat.jpg", "7cat.jpg", "8cat.jpg" };
            string[] DogsImages = { "1dog.jpg", "2dog.jpg", "3dog.jpg", "4dog.jpg", "5dog.jpg", "6dog.jpg", "7dog.jpg", "8dog.jpg" };

            PetViewModel pet = null;
            Random random = new Random();
            int catOrDog = random.Next(1, 10);
            if (catOrDog <= 5)
            {
                string kind = "Cat";
                string name = CatsNames[random.Next(15)];
                int randHealth = random.Next(2);
                string healthstat = Healthstatus[randHealth];
                string image = CatsImages[random.Next(7)];
                int health = randHealth;
                int age = random.Next(1, 10);
                pet = new PetViewModel { Name = name, HealthStatus = healthstat, Age = age, PetPicture = "Cats/" + image, Kind = kind, Health = health };


            }
            if (catOrDog > 5)
            {
                string kind = "Dog";
                string name = DogsNames[random.Next(15)];
                int randHealth = random.Next(2);
                string healthstat = Healthstatus[randHealth];
                int health = randHealth;
                string image = DogsImages[random.Next(7)];
                int age = random.Next(1, 10);
                pet = new PetViewModel { Name = name, HealthStatus = healthstat, Age = age, PetPicture = "Dogs/" + image, Kind = kind, Health = health };

            }
            return pet;

        }
        public IActionResult NoPlaces()
        {
            return View();
        }
        public IActionResult NoMoney()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
    }
}
