using System;
using System.Collections.Generic;
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
    [Route("api/review")]
    public class ReviewController : Controller
    {
        private IPassageRepository _passages; 
        private ITopicRepository _topics;
        private ILogger<PassagesController> _logger; 
        public ReviewController(IPassageRepository passages, 
                                ITopicRepository topics,
                                IScripturesRepository scriptures, 
                                IScriptureMasteryRepository smrepo,
                                ILogger<PassagesController> logger)
        {
            _passages = passages; 
            _topics = topics;
            _logger = logger;
        }
        [HttpGet("all")]
        public IActionResult GetReviewList(){
            try {
                var passages =  _passages.GetReviewPassagesByUser(this.User.Identity.Name);
                // TODO: get passed today 
                return Ok(Mapper.Map<IEnumerable<PassageListViewModel>>(passages)); // custom review view model ?? 
            } catch (Exception ex){
                _logger.LogError($"Failed to get review passages: {ex}");
                return BadRequest("Error occurred");
            }
        }
        [HttpGet("topics")]
        public IActionResult GetReviewTopics(){
            try {
                var topics =  _topics.GetReviewTopicsByUser(this.User.Identity.Name);
                return Ok(topics); 
            } catch (Exception ex){
                _logger.LogError($"Failed to get review topics: {ex}");
                return BadRequest("Error occurred");
            }
        }
        [HttpGet("topic/{id}")]
        public IActionResult GetReviewTopicPassages(int id){
            try {
                var passages =  _topics.GetReviewPassagesByTopic(id);
                return Ok(Mapper.Map<IEnumerable<PassageListViewModel>>(passages)); 
            } catch (Exception ex){
                _logger.LogError($"Failed to get review topics: {ex}");
                return BadRequest("Error occurred");
            }
        }
        [HttpPost("passed/{id}")]
        public async Task<IActionResult> PassagePassed(int id){
            try {
                var passage =  _passages.GetPassageById(id);
                // check if passage belongs to user ?? 
                if(passage.DatePassed < DateTime.Today || passage.Level == 0){
                    passage.Level = passage.Level + 1;
                    passage.DatePassed = DateTime.Today;
                }

                if(await _passages.SaveChangesAsync()) {
                    return Ok(Mapper.Map<PassageListViewModel>(passage)); 
                } 
            } 
            catch (Exception ex){
                _logger.LogError($"Failed to get and pass passage: {ex}");
            }
            return BadRequest("Failed to pass the passage");
        }

        [Authorize(Roles = "Privileged")]
        [HttpPost("setlevel/{id}/{level}")]
        public async Task<IActionResult> SetLevelApi(int id, int level){
            try {
                var passage = _passages.GetPassageById(id);
                if(passage == null || passage.UserName != this.User.Identity.Name) 
                    return BadRequest("Invalid passage");

                else if(level == passage.Level) return StatusCode(304);
                else if(level >= 0){
                    passage.Level = level;
                    await _passages.SaveChangesAsync();  
                    return StatusCode(200);
                }
            } catch (Exception ex){
                _logger.LogError($"Failed to set level for passage: {ex}");
            }
            return BadRequest("Failed to set level for passage");
        }

    }
}