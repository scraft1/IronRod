using System.Collections.Generic; 
using System.Linq; 
using IronRod.Models;


namespace IronRod.Data
{
    public class ScripturesRepository : IScripturesRepository
    {
        private ScripturesDbContext _context;
        public ScripturesRepository(ScripturesDbContext context){
            _context = context;
        }
        public IEnumerable<Volume> GetVolumes(){
            return _context.Volumes.ToList();
        }
        public Volume GetVolumeById(int volume_id){
            return _context.Volumes.SingleOrDefault(v => v.id == volume_id);
        }
        public IEnumerable<Book> GetBooks(int volume_id){
            return _context.Books.Where(b => b.volume_id == volume_id).ToList();
        }
        public Book GetBookById(int id){
            return _context.Books.SingleOrDefault(b => b.id == id);
        }
        public IEnumerable<Chapter> GetChapters(int book_id){
            return _context.Chapters.Where(c => c.book_id == book_id).ToList();
        }
        public Chapter GetChapterById(int id) {
            return _context.Chapters.SingleOrDefault(c => c.id == id);
        }
        public IEnumerable<Verse> GetVerses(int chapter_id){
            return _context.Verses.Where(v => v.chapter_id == chapter_id).ToList();
        }
        public Verse GetVerseById(int id){
            return _context.Verses.SingleOrDefault(v => v.id == id);
        }
        public IEnumerable<Verse> GetVersesByIds(IEnumerable<int> ids){
            var verses = new List<Verse>();
            foreach(var vid in ids){
                verses.Add(GetVerseById(vid));
            }
            return verses;
        }
        public IEnumerable<int> GetVerseNumsByIds(IEnumerable<int> ids){
            var verseids = new List<int>();
            foreach(var id in ids){
                var num = _context.Verses.Where(v => v.id == id).Select(v => v.verse_number).SingleOrDefault();
                verseids.Add(num);
            }
            return verseids;
        }
    }
}