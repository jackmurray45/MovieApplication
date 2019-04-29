using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MovieApplication.Data;
using MovieApplication.Models;

namespace MovieApplication.Views
{
    [Route("Movies")]
    public class UserMoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserMoviesController(ApplicationDbContext context)
        {
            _context = context;
            _userManager = _context.GetService<UserManager<ApplicationUser>>();
        }

        // GET: UserMovies
        [AllowAnonymous]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var userFavoritesIds = _context.UserMovies.Where(u => u.UserId == _userManager.GetUserId(HttpContext.User))
                .Select(u => u.MovieId ).ToHashSet();
            var movies = await _context.Movies.ToListAsync();
            var MoviesAndUsers = new MovieAndUser
            {
                Movies = movies,
                UserMovies = userFavoritesIds
            };
            return View(MoviesAndUsers);
        }

        // GET: UserMovies/Details/5
        [AllowAnonymous]
        [Route("Details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: UserMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Favorite")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Favorite(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    UserMovie userMovie = new UserMovie { MovieId = id, UserId = userId };
                    _context.Add(userMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));

        }

        [Route("Unfavorite")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Unfavorite(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);

                    _context.Remove(_context.UserMovies.Single(m => m.MovieId == id && m.UserId == userId));
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
