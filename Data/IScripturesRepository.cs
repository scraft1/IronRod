using System.Collections.Generic; 
using IronRod.Models;

namespace IronRod.Data
{
    public interface IScripturesRepository
    {
        IEnumerable<Volume> GetVolumes();
        Volume GetVolumeById(int id);
        IEnumerable<Book> GetBooks(int volume_id);
        Book GetBookById(int id);
        IEnumerable<Chapter> GetChapters(int book_id);
        Chapter GetChapterById(int id);
        IEnumerable<Verse> GetVerses(int chapter_id);
        Verse GetVerseById(int id);
    }
}