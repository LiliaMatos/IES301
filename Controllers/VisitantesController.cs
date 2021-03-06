﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PortariaInteligente.Data;
using PortariaInteligente.Models;

namespace PortariaInteligente.Controllers
{
    public class VisitantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VisitantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Visitantes
        public async Task<IActionResult> Index(string nome, string empresa, string email)
        {
            var visitantes = from m in _context.Visitantes
                         select m;

            if (!String.IsNullOrEmpty(nome))
            {
                visitantes = visitantes.Where(s => s.VisitanteNome.Contains(nome));
            }
            if (!String.IsNullOrEmpty(empresa))
            {
                visitantes = visitantes.Where(s => s.Empresa.Contains(empresa));
            }
            if (!String.IsNullOrEmpty(email))
            {
                visitantes = visitantes.Where(s => s.VisitanteEmail.Contains(email));
            }
            return View(await visitantes.ToListAsync());
        }

        // GET: Visitantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitante = await _context.Visitantes
                .Include(v => v.Documentos)
                .Include(v => v.Papeis)
                .FirstOrDefaultAsync(m => m.VisitanteID == id);
            if (visitante == null)
            {
                return NotFound();
            }

            return View(visitante);
        }

        // GET: Visitantes/Create
        public IActionResult Create()
        {
            ViewData["DocumentoID"] = new SelectList(_context.Documentos, "DocumentoID", "DocumentoNome");
            ViewData["PapelID"] = new SelectList(_context.Papeis, "PapelID", "PapelNome");
            return View();
        }

        // POST: Visitantes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisitanteID,VisitanteNome,VisitanteEmail,VisitanteCel,Empresa,PapelID,DocumentoID,DocumentoNumero,CarroMarca,CarroCor,CarroModelo,CarroPlaca")] Visitante visitante)
        {
            visitante.PapelID = 1;
            if (ModelState.IsValid)
            {
                _context.Add(visitante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DocumentoID"] = new SelectList(_context.Documentos, "DocumentoID", "DocumentoNome", visitante.DocumentoID);
            ViewData["PapelID"] = new SelectList(_context.Papeis, "PapelID", "PapelNome", visitante.PapelID);
            return View(visitante);
        }

        // GET: Visitantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitante = await _context.Visitantes.FindAsync(id);
            if (visitante == null)
            {
                return NotFound();
            }
            ViewData["DocumentoID"] = new SelectList(_context.Documentos, "DocumentoID", "DocumentoNome", visitante.DocumentoID);
            ViewData["PapelID"] = new SelectList(_context.Papeis, "PapelID", "PapelNome", visitante.PapelID);
            return View(visitante);
        }

        // POST: Visitantes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VisitanteID,VisitanteNome,VisitanteEmail,VisitanteCel,Empresa,PapelID,DocumentoID,DocumentoNumero,CarroMarca,CarroCor,CarroModelo,CarroPlaca")] Visitante visitante)
        {
            visitante.PapelID = 1;
            if (id != visitante.VisitanteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visitante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitanteExists(visitante.VisitanteID))
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
            ViewData["DocumentoID"] = new SelectList(_context.Documentos, "DocumentoID", "DocumentoNome", visitante.DocumentoID);
            ViewData["PapelID"] = new SelectList(_context.Papeis, "PapelID", "PapelNome", visitante.PapelID);
            return View(visitante);
        }

        // GET: Visitantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitante = await _context.Visitantes
                .Include(v => v.Documentos)
                .Include(v => v.Papeis)
                .FirstOrDefaultAsync(m => m.VisitanteID == id);
            if (visitante == null)
            {
                return NotFound();
            }

            return View(visitante);
        }

        // POST: Visitantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visitante = await _context.Visitantes.FindAsync(id);
            _context.Visitantes.Remove(visitante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitanteExists(int id)
        {
            return _context.Visitantes.Any(e => e.VisitanteID == id);
        }
    }
}
