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
        private ILogger<PassagesController> _logger; 
        public PassagesController(IPassagesRepository repository, 
                                IScripturesRepository scriptures, 
                                IScriptureMasteryRepository smrepo,
                                ILogger<PassagesController> logger)
        {
            _repository = repository; 
            _logger = logger;
        }
       [HttpGet("")]
        public IActionResult GetAllPassages(){
            try {
                var results = _repository.GetAllPassagesByUser(this.User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<PassageViewModel>>(results));
            } catch (Exception ex){
                _logger.LogError($"Failed to get review passages: {ex}");
                return BadRequest("Error occurred");
            }
            
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