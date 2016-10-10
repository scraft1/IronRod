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
    }
}