using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ELibrary.Domain.Models;
using ELibrary.Repository;
using Microsoft.AspNetCore.Authorization;
using ELibrary.Service.Interface;
using ELibrary.Service.RDF.Interface;
using ELibrary.Service.RDF.Extension;

namespace ELibrary.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly IRDFService _rdfService;
        public AuthorsController(ApplicationDbContext context, IAuthorService authorService, IRDFService rdfService)
        {
            _authorService = authorService;
            _rdfService = rdfService;
        }

        // GET: Authors
        public IActionResult Index()
        {
            return View(_authorService.GetAll());
        }

        // GET: Authors/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _authorService.Get(id);
            if (author == null)
            {
                return NotFound();
            }

            if (Request.Headers.ContainsKey("Accept"))
            {
                if (Request.Headers["Accept"] == "text/turtle")
                {
                    Response.Headers.Add("Content-Type", "text/turtle");
                    return File(author.GetRDFGraph(Service.RDF.Enum.Syntax.Turtle), "text/turtle");
                }
                if (Request.Headers["Accept"] == "application/rdf+xml")
                {
                    Response.Headers.Add("Content-Type", "application/rdf+xml");
                    return File(author.GetRDFGraph(Service.RDF.Enum.Syntax.RDFXML), "application/rdf+xml");
                }
            }

            return View(author);
        }

        // GET: Authors/MoreDetails/5
        public IActionResult MoreDetails(Guid? id)
        {
            throw new NotImplementedException();
        }

        // GET: Authors/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("Name,Surname,Country,Image,Id")] Author author)
        {
            if (ModelState.IsValid)
            {
                author.Id = Guid.NewGuid();
                _authorService.Insert(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _authorService.Get(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, [Bind("Name,Surname,Country,Image,Id")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _authorService.Update(author);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            // cannot delete author if he has books

            if (id == null)
            {
                return NotFound();
            }

            var author = _authorService.Get(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var author = _authorService.Get(id);
            _authorService.Delete(author);
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(Guid id)
        {
            return _authorService.Get(id) != null;
        }
    }
}
