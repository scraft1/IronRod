using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IronRod.Models;

namespace IronRod.Data
{
    public class PassagesContextSeedData
    {
        private PassagesDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private string _username = "Spencer";
        private string _email = "spencer@example.com";
        
        public PassagesContextSeedData(PassagesDbContext context, 
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager){
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task SetDefaultUsers(){
            if(await _userManager.FindByEmailAsync(_email) == null){
                var user = new ApplicationUser(){
                    UserName = _username,
                    Email = _email
                };
                await _userManager.CreateAsync(user, "password");
            }
        }
        public async Task SetUserRoles(){
            if(!await _roleManager.RoleExistsAsync("Privileged")){
                var role = new IdentityRole();
                role.Name = "Privileged";
                await _roleManager.CreateAsync(role);

                var me = await _userManager.FindByEmailAsync(_email);
                if(me != null){
                    await _userManager.AddToRoleAsync(me, role.Name);
                }
            }
        }
        public async Task SetDefaultPassages(){
            if(!_context.Passages.Any()){
                var passage = new Passage() {
                    UserName = _username,
                    Title = "Seed Passage 1",
                    Verses = new List<PassageVerse>()
                };
                _context.Passages.Add(passage);
                _context.PassageVerses.AddRange(passage.Verses);

                passage = new Passage() {
                    UserName = _username,
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