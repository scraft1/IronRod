using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; 
using Microsoft.AspNetCore.Identity;
using IronRod.Models;

namespace IronRod.Data
{
    public class PassagesContextSeedData
    {
        private PassagesDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public PassagesContextSeedData(PassagesDbContext context, UserManager<ApplicationUser> userManager){
            _context = context;
            _userManager = userManager;
        }
        public async Task PlantSeedData(){
            if(await _userManager.FindByEmailAsync("spencer@example.com") == null){
                var user = new ApplicationUser(){
                    UserName = "spencer",
                    Email = "spencer@example.com"
                };
                await _userManager.CreateAsync(user, "P@ssw0rd!");
            }

            if(!_context.Passages.Any()){
                var passage = new Passage() {
                    UserName = "spencer",
                    Title = "Seed Passage 1",
                    Verses = new List<PassageVerse>()
                };
                _context.Passages.Add(passage);
                _context.PassageVerses.AddRange(passage.Verses);

                passage = new Passage() {
                    UserName = "spencer",
                    Title = "Seed Passage 2",
                    Verses = new List<PassageVerse>()
                };
                _context.Passages.Add(passage);
                _context.PassageVerses.AddRange(passage.Verses);

                await _context.SaveChangesAsync();
            }
        }
    }
}