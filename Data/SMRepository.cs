using System.Collections.Generic; 
using System.Linq; 
using IronRod.Models;

namespace IronRod.Data
{
    public class SMRepository : IScriptureMasteryRepository
    {
        private ScriptureMasteryDbContext _context;
        public SMRepository(ScriptureMasteryDbContext context){
            _context = context;
        }
        public IEnumerable<SMVolume> GetSMVolumes(){
            return _context.Volumes.ToList();
        }
        public SMVolume GetSMVolumeById(int id){
            return _context.Volumes.SingleOrDefault(v => v.id == id);
        }
        public IEnumerable<Set> GetSetsByVolume(int volumeid){
            return _context.Sets.Where(s => s.volume_id == volumeid).ToList();
        }
        public Set GetSetById(int id){
            return _context.Sets.SingleOrDefault(s => s.id == id);
        } 
        public IEnumerable<int> GetVerseIdsBySet(int setid){
            return _context.Verses.Where(v => v.set_id == setid).Select(v => v.verse_id).ToList();
        }
    }
}