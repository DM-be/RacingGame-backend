﻿using SuperAwesomeRaptorRacingGame_Backend.Entities;
using SuperAwesomeRaptorRacingGame_Backend.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAwesomeRaptorRacingGame_Backend.Dtos;

namespace SuperAwesomeRaptorRacingGame_Backend.Services
{

    public interface IScoreService
    {
        Task<ICollection<ScoreDto>> GetScoresForUser(int UserId);
        Task<ICollection<ScoreDto>> GetAllScores();
        Task AddScore(Score score);
        Task<Score> GetScoreById(int id);
        Task<string> GetTopScoreByTrack(string TrackName);
    }

    public class ScoreService : IScoreService
    {

        private DataContext _context;

        public ScoreService(DataContext context)
        {
            _context = context;
        }

        public async Task AddScore(Score score)
        {
            _context.Scores.Add(score);
            await _context.SaveChangesAsync();
        }


        public async Task<ICollection<ScoreDto>> GetScoresForUser(int UserId)
        {
            return await _context.Scores.Where(score => score.User.UserId == UserId).Select(sc => new ScoreDto
            {
                TrackName = sc.TrackName,
                Time = sc.Time,
                FirstName = sc.User.FirstName,
                LastName = sc.User.LastName,
                Username  = sc.User.Username
            }).OrderBy(scd => scd.Time).ToListAsync();
        }

        public async Task<ICollection<ScoreDto>>GetAllScores()
        {
            return await _context.Scores.Select(sc => new
            ScoreDto
            {
                TrackName = sc.TrackName,
                Time = sc.Time,
                FirstName = sc.User.FirstName,
                LastName = sc.User.LastName,
                Username = sc.User.Username
            }).OrderBy(scd => scd.Time).ToListAsync();
        }

        public async Task<Score> GetScoreById(int id) {

            return await _context.Scores.FindAsync(id);
        }

        public async Task<string> GetTopScoreByTrack(string TrackName)
        {

            var topScore = await _context.Scores.Where(score => score.TrackName == TrackName).Select(sc => new ScoreDto
            {
                Time = sc.Time,
            }).OrderBy(scd => scd.Time).FirstOrDefaultAsync();
            return topScore.Time;

        }


    }
}