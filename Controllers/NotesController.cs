using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NotesApi.Controllers
{
    [ApiController]
    [Route("notes")]
    public class NotesController : ControllerBase
    {
        private readonly NoteContext db;
        
        public NotesController(NoteContext db)
        {
            this.db = db;
        }
        
        [HttpPost]
        public IActionResult PostNote(string title, string content)
        {
            if (String.IsNullOrEmpty(content))
            {
                return BadRequest();
            }
            
            var note = new Note {title = title, content = content};
            
            db.Notes.Add(note);
            db.SaveChanges();
            
            return Ok(note);
        }

        [HttpGet]
        public IActionResult GetNotes([FromQuery] string query)
        {
            if (String.IsNullOrWhiteSpace(query))
            {
                return Ok(db.Notes);
            }
            
            var notes = db.Notes.Where(n => n.title.Contains(query) || n.content.Contains(query));
            
            return Ok(notes);
        }

        [HttpGet]
        [Route("/notes/{id}")]
        public IActionResult GetNote([FromRoute] int id)
        {
            var note = db.Notes.FirstOrDefault(n => n.id == id);

            if (note == null)
            {
                return NotFound();
            }
            
            return Ok(note);
        }
        
        [HttpPut]
        [Route("/notes/{id}")]
        public IActionResult PutNote([FromRoute] int id, [FromBody] NoteBody noteBody)
        {
            if (String.IsNullOrEmpty(noteBody.content))
            {
                return BadRequest();
            }
            
            var note = db.Notes.FirstOrDefault(n => n.id == id);

            if (note == null)
            {
                return NotFound();
            }

            note.title = noteBody.title;
            note.content = noteBody.content;

            db.SaveChanges();

            return Ok(note);
        }

        [HttpDelete]
        [Route("/notes/{id}")]
        public IActionResult DeleteNote([FromRoute] int id)
        {
            var note = db.Notes.FirstOrDefault(n => n.id == id);
            if (note == null)
            {
                return NotFound();
            }

            db.Notes.Remove(note);
            db.SaveChanges();

            return NoContent();
        }
    }
}