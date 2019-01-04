﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperAwesomeRaptorRacingGame_Backend.Entities;
using SuperAwesomeRaptorRacingGame_Backend.Helpers;

namespace SuperAwesomeRaptorRacingGame_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoresController : ControllerBase
    {
        private readonly DataContext _context;

        public ScoresController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Scores
        [HttpGet]
        public IEnumerable<Score> GetScores()
        {
            return _context.Scores;
        }

        // GET: api/Scores/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScore([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var score = await _context.Scores.FindAsync(id);

            if (score == null)
            {
                return NotFound();
            }

            return Ok(score);
        }

        // PUT: api/Scores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScore([FromRoute] int id, [FromBody] Score score)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != score.ScoreId)
            {
                return BadRequest();
            }

            _context.Entry(score).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Scores
        [HttpPost]
        public async Task<IActionResult> PostScore([FromBody] Score score)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScore", new { id = score.ScoreId }, score);
        }

        // DELETE: api/Scores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScore([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var score = await _context.Scores.FindAsync(id);
            if (score == null)
            {
                return NotFound();
            }

            _context.Scores.Remove(score);
            await _context.SaveChangesAsync();

            return Ok(score);
        }

        private bool ScoreExists(int id)
        {
            return _context.Scores.Any(e => e.ScoreId == id);
        }
    }
}