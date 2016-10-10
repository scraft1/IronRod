using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; 
using IronRod.Models;

namespace IronRod.Data
{
    public class PassagesContextSeedData
    {
        private PassagesDbContext _context;
        public PassagesContextSeedData(PassagesDbContext context){
            _context = context;
        }
        public async Task PlantSeedData(){
            if(!_context.Passages.Any()){
                var passage = new Passage() {
                     UserID = 0,
                    Title = "Seed Passage",
                    Verses = new List<PassageVerse>()
                };
                _context.Passages.Add(passage);
                _context.PassageVerses.AddRange(passage.Verses);

                passage = new Passage() {
                    UserID = 0,
                    Title = "Seed Passage 2",
                    Verses = new List<PassageVerse>()
                };
                _context.Passages.Add(passage);
                _context.PassageVerses.AddRange(passage.Verses);

                passage = new Passage() {
                    UserID = 0,
                    Title = "Seed Passage 3",
                    Verses = new List<PassageVerse>()
                };
                _context.Passages.Add(passage);
                _context.PassageVerses.AddRange(passage.Verses);

                await _context.SaveChangesAsync();
            }
        }
    }
}