using FundooManager.Interface;
using FundooModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteManager manager;
        private readonly INoteManager repository;
        public NotesController(INoteManager manager)
        {
            this.manager = manager;
            this.repository = repository;
        }
        [HttpPost]
        public ActionResult CreateNote(AddNote notes, int userId)
        {
            try
            {
                //int userId= Convert.ToInt32(User.FindFirst(x => x.Type == "userId").Value);
                var isNoteAdded = this.manager.AddNewNote(notes, userId);

                if (isNoteAdded == true)
                {
                    return Ok(new { message = "Added Note", data = notes });
                }
                return BadRequest(new { message = "Failed " });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        private int GetIdFromToken()
        {
            return Convert.ToInt32(User.FindFirst(x => x.Type == "userId").Value);
        }

        [HttpGet]
        public ActionResult GetAllNotes()
        {
            try
            {
                int userId = GetIdFromToken();
                var notes = repository.GetAllNotes(userId);

                if (notes != null)
                {
                    return Ok(new { message = "Notes are as follows", data = notes });
                }
                return BadRequest(new { message = "Notes not available" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("{noteId}")]
        public ActionResult GetNoteByNoteId(int noteId)
        {
            try
            {
                int userId = GetIdFromToken();
                var note = repository.GetNoteByNoteId(userId, noteId);

                if (note != null)
                {
                    return Ok(new { message = "Note is as following", data = note });
                }
                return BadRequest(new { message = "Note not available" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("trash")]
        public ActionResult GetBinNotes()
        {
            try
            {
                int userId = GetIdFromToken();
                var trashNotes = repository.GetBinNotes(userId);

                if (trashNotes != null)
                {
                    return Ok(new { message = "Bin notes are as follows", data = trashNotes });
                }
                return BadRequest(new { message = "Bin notes not available" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet("archive")]
        public ActionResult GetArchiveNotes()
        {
            try
            {
                int userId = GetIdFromToken();
                var archiveNotes = repository.GetArchiveNotes(userId);

                if (archiveNotes != null)
                {
                    return Ok(new { message = "Archive notes are as follows", data = archiveNotes });
                }
                return BadRequest(new { message = "Archive notes not available" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpPut("{noteId}/trash-restore")]
        public ActionResult BinRestoreNote(int noteId)
        {
            try
            {
                int userId = GetIdFromToken();
                var noteToBeMoved = repository.BinRestoreNote(noteId, userId);

                if (noteToBeMoved == true)
                {
                    return Ok(new { message = "Operation successfull" });
                }
                return BadRequest(new { message = "operation unsuccessfull " });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpPut("{noteId}/archive-unarchive")]
        public ActionResult ArchiveUnarchiveNote(int noteId)
        {
            try
            {
                int userId = GetIdFromToken();
                var noteToBeArchived = repository.ArchiveUnarchiveNote(noteId, userId);

                if (noteToBeArchived == true)
                {
                    return Ok(new { message = "Operation successfull" });
                }
                return BadRequest(new { message = "operation unsuccessfull" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpDelete("{noteId}")]
        public ActionResult DeleteNote(int noteId)
        {
            try
            {
                int userId = GetIdFromToken();
                var noteToBeDeleted = repository.DeleteNote(noteId, userId);

                if (noteToBeDeleted == true)
                {
                    return Ok(new { message = "Note Deleted Successfully" });
                }
                return BadRequest(new { message = "operation unsuccessfull" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpPut("{noteId}/update")]
        public ActionResult UpdateNote(NotesModel update, int noteId)
        {
            try
            {
                int userId = GetIdFromToken();
                var noteToBeUpdated = repository.UpdateNote(update, noteId, userId);

                if (noteToBeUpdated == true)
                {
                    return Ok(new { message = "Note Updated Successfully", data = update });
                }
                return BadRequest(new { message = "operation unsuccessfull " });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpPut("{noteId}/pin-unpin")]
        public ActionResult PinUnpinNote(int noteId)
        {
            try
            {
                int userId = GetIdFromToken();
                var noteToBePinned = repository.PinUnpinNote(noteId, userId);

                if (noteToBePinned == true)
                {
                    return Ok(new { message = "Operation successfull" });
                }
                return BadRequest(new { message = "operation unsuccessfull" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpPut("color")]
        public ActionResult AddColor(int noteId, string color)
        {
            try
            {
                int userId = GetIdFromToken();
                var noteToBeColored = repository .AddColor(noteId, color, userId);

                if (noteToBeColored == true)
                {
                    return Ok(new { message = color + "added" });
                }
                return BadRequest(new { message = "color not added" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpPut]
        [Route("api/addImage")]
        public IActionResult AddImage(int noteId, IFormFile imagePath)
        {
            try
            {
                bool result = repository.AddImage(noteId, imagePath);
                if (result)
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Image Uploaded" });
                }

                return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Error!!Image Uploaded Failed" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut]
        [Route("api/deleteImage")]
        public IActionResult DeleteImage(int noteId)
        {
            try
            {
                bool result = this.manager.DeleteImage(noteId);
                if (result)
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Image deleted Successfully" });
                }

                return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Error!!delete failed" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }
    }
    }
