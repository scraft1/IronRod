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
        private IPassageRepository _repository; 
        private ITopicRepository _topics;
        private IScripturesRepository _scriptures; 
        private ILogger<PassagesController> _logger; 
        public PassagesController(IPassageRepository repository,
                                ITopicRepository topics, 
                                IScripturesRepository scriptures, 
                                IScriptureMasteryRepository smrepo,
                                ILogger<PassagesController> logger)
        {
            _repository = repository; 
            _topics = topics;
            _scriptures = scriptures;
            _logger = logger;
        }
        [HttpGet("")]
        public IActionResult GetAllPassages(){
            try {
                //var results = _repository.GetAllPassagesWithoutTopicsByUser(this.User.Identity.Name); 
                var results = _repository.GetAllPassagesByUser(this.User.Identity.Name); 
                return Ok(Mapper.Map<IEnumerable<PassageListViewModel>>(results));
            } catch (Exception ex){
                _logger.LogError($"Failed to get all passages: {ex}");
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
        [HttpGet("stats")]
        public IActionResult GetStats(){
            var stats = new Stats();
            stats.TotalVerses = _repository.CountTotalVerses(this.User.Identity.Name);
            // total passages 
            return Ok(stats);
        }

        [HttpGet("backup")]
        public IActionResult GetDataBackup(){
            try {
                var passages = _repository.GetBackupPassages(this.User.Identity.Name); 
                var results = new List<PassageBackup>();
                foreach(var passage in passages){
                    var pb = Mapper.Map<PassageBackup>(passage);
                    pb.Topics = _topics.GetTopicsByPassage(passage).Select(t => t.Title).ToList();
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
                _repository.RemoveAllDataByUser(username); 

                // doesn't maintain order of passages or passage verses; sorting needed when querying 
                // add passages and passage verses individually to keep order in database
                _repository.AddPassages(passages); 
                _topics.AddTopics(topics);

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