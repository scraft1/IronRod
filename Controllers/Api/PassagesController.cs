using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
                return Ok(Mapper.Map<IEnumerable<PassageListViewModel>>(results));
            } catch (Exception ex){
                _logger.LogError($"Failed to get all passages: {ex}");
                return BadRequest("Error occurred");
            }
        }
        [HttpGet("review")]
        public IActionResult GetReviewList(){
            try {
                var passages =  _repository.GetReviewPassagesByUser(this.User.Identity.Name);
                // TODO: get passed today 
                return Ok(Mapper.Map<IEnumerable<PassageListViewModel>>(passages)); // custom review view model ?? 
            } catch (Exception ex){
                _logger.LogError($"Failed to get review passages: {ex}");
                return BadRequest("Error occurred");
            }
        }
        [HttpGet("detail/{id}")]
        public IActionResult GetPassage(int id){
            try {
                var passage =  _repository.GetPassageById(id);
                // check if passage belongs to user ?? 
                return Ok(Mapper.Map<PassageDetailViewModel>(passage));
            } catch (Exception ex){
                _logger.LogError($"Failed to get passage: {ex}");
                return BadRequest("Error occurred");
            }
        }
        [HttpPost("passed/{id}")]
        public async Task<IActionResult> PassagePassed(int id){
            try {
                var passage =  _repository.GetPassageById(id);
                // check if passage belongs to user ?? 
                if(passage.DatePassed < DateTime.Today || passage.Level == 0){
                    passage.Level = passage.Level + 1;
                    passage.DatePassed = DateTime.Today;
                }

                if(await _repository.SaveChangesAsync()) {
                    return Ok(Mapper.Map<PassageListViewModel>(passage)); 
                } 
            } 
            catch (Exception ex){
                _logger.LogError($"Failed to get and pass passage: {ex}");
            }
            return BadRequest("Failed to pass the passage");
        }

        [HttpGet("stats")]
        public IActionResult GetStats(){
            var stats = new Stats();
            stats.TotalVerses = _repository.CountTotalVerses(this.User.Identity.Name);
            // total passages 
            return Ok(stats);
        }

        [Authorize(Roles = "Privileged")]
        [HttpPost("setlevel")]
        public async Task<IActionResult> SetLevel(int id, int level){
            try {
                Console.WriteLine("id: "+id);
                Console.WriteLine("level: "+level);
                var passage = _repository.GetPassageById(id);
                if(passage == null) return BadRequest("Passage is null");

                else if(level == passage.Level) return StatusCode(304);
                else if(level >= 0){
                    passage.Level = level;
                    await _repository.SaveChangesAsync();  
                    return StatusCode(200);
                }
            } catch (Exception ex){
                _logger.LogError($"Failed to set level for passage: {ex}");
            }
            return BadRequest("Failed to set level for passage");
        }

        [HttpGet("backup")]
        public IActionResult GetDataBackup(){
            try {
                var passages = _repository.GetBackupPassages(this.User.Identity.Name); 
                var results = new List<PassageBackup>();
                foreach(var passage in passages){
                    var pb = Mapper.Map<PassageBackup>(passage);
                    pb.Topics = _repository.GetTopicsByPassage(passage).Select(t => t.Title).ToList();
                    results.Add(pb);
                }
                return Ok(results);
            } catch (Exception ex){
                _logger.LogError($"Failed to get backup passages: {ex}");
                return BadRequest("Error occurred");
            }
        }

        [Authorize(Roles = "Privileged")]
        [HttpPost("backup")]
        public async Task<IActionResult> PostDataBackup([FromBody] IList<PassageBackup> backups){
            var minPassages = 5;
            var username = this.User.Identity.Name;

            if(ModelState.IsValid && backups.Count >= minPassages){
                var passages = new List<Passage>();
                var topics = new List<Topic>();

                foreach (var pb in backups) {
                    var passage = Mapper.Map<Passage>(pb);
                    passage.UserName = username;
                    passage.FirstVerse = passage.Verses.First().VerseID;
                    foreach(var v in passage.Verses) {
                        var verse = _scriptures.GetVerseById(v.VerseID);
                        v.ChapterID = verse.chapter_id;
                        v.VerseNumber = verse.verse_number;
                        v.VerseText = verse.verse_text;
                    }
                    foreach(var t in pb.Topics){
                        var topic = topics.FirstOrDefault(top => top.Title == t);
                        if(topic == null){
                            topic = new Topic(username, t);
                            topics.Add(topic);
                        }
                        var pt = new PassageTopic(passage,topic);
                        topic.PassageTopics.Add(pt);
                    }
                    passages.Add(passage);
                }
                
                // or keep existing data ?? 
                _repository.RemoveAllDataByUser(this.User.Identity.Name); 

                _repository.AddPassages(passages);
                _repository.AddTopics(topics);

                if(await _repository.SaveChangesAsync()) {
                    return Created($"api/passages/backup", "Successfully added "+backups.Count+" passages");
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