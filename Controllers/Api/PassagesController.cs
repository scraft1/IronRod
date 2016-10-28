using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Logging;
using AutoMapper;
using IronRod.Data;
using IronRod.Models; 

namespace IronRod.Controllers.Api
{
    [Authorize] 
    [Route("api/passages")]
    public class PassagesController : Controller
    {
        private IPassagesRepository _repository; 
        private IScripturesRepository _scriptures; 
        private ILogger<PassagesController> _logger; 
        public PassagesController(IPassagesRepository repository, 
                                IScripturesRepository scriptures, 
                                IScriptureMasteryRepository smrepo,
                                ILogger<PassagesController> logger)
        {
            _repository = repository; 
            _scriptures = scriptures;
            _logger = logger;
        }
        [HttpGet("")]
        public IActionResult GetAllPassages(){
            try {
                var results = _repository.GetAllPassagesByUser(this.User.Identity.Name); 
                return Ok(Mapper.Map<IEnumerable<PassageViewModel>>(results));
            } catch (Exception ex){
                _logger.LogError($"Failed to get all passages: {ex}");
                return BadRequest("Error occurred");
            }
        }

        [HttpGet("backup")]
        public IActionResult GetPassagesBackup(){
            try {
                var results = _repository.GetPassagesWithVerses(this.User.Identity.Name); 
                return Ok(Mapper.Map<IEnumerable<PassageBackup>>(results));
            } catch (Exception ex){
                _logger.LogError($"Failed to get backup passages: {ex}");
                return BadRequest("Error occurred");
            }
        }
        [HttpPost("backup")]
        public async Task<IActionResult> PostPassagesBackup([FromBody] IList<PassageBackup> backups){
            var minPassages = 5;
            if(ModelState.IsValid && backups.Count >= minPassages){
                var passages = Mapper.Map<IEnumerable<Passage>>(backups);
                foreach (var p in passages) {
                    p.UserName = this.User.Identity.Name;
                    p.FirstVerse = p.Verses.First().VerseID;
                    foreach(var v in p.Verses) {
                        var verse = _scriptures.GetVerseById(v.VerseID);
                        v.ChapterID = verse.chapter_id;
                        v.VerseNumber = verse.verse_number;
                        v.VerseText = verse.verse_text;
                    }
                }
                
                // or keep existing verses ?? 
                _repository.RemoveAllPassagesByUser(this.User.Identity.Name); 

                _repository.AddPassages(passages);
                if(await _repository.SaveChangesAsync()) {
                    return Created($"api/passages/backup", "Successfully added "+backups.Count+" verses");
                } 
            } 
            return BadRequest("Failed to post the passages"); 
        }
        // [HttpPost("")]
        // public async Task<IActionResult> Post([FromBody] PassageViewModel pvm){
        //     if(ModelState.IsValid){
        //         var passage = Mapper.Map<Passage>(pvm);
        //         passage.UserName = this.User.Identity.Name;
        //         _repository.AddPassage(passage);

        //         if(await _repository.SaveChangesAsync()) {
        //             return Created($"api/passages/{passage.Title}", Mapper.Map<PassageViewModel>(passage)); // url ?? 
        //         } 
        //     } 
        //     return BadRequest("Failed to save the passage"); // ModelState ??
        // }
    }
}