using System.Collections.Generic; 
using IronRod.Models;

namespace IronRod.Data
{
    public interface IScriptureMasteryRepository
    {
        IEnumerable<SMVolume> GetSMVolumes();
        SMVolume GetSMVolumeById(int id);
        IEnumerable<Set> GetSetsByVolume(int volumeid);
        Set GetSetById(int id); 
        IEnumerable<int> GetVerseIdsBySet(int setid);
    }
}