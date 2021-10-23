using FundooManager.Interface;
using FundooModel;
using FundooRepository.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FundooManager.Manager
{
    public class NoteManager : INoteManager
    {
        private readonly INotesRepository repository;
        public NoteManager(INotesRepository repository)
        {
            this.repository = repository;
        }

        public bool AddNewNote(AddNote noteData, int userId)
        {
            try
            {
                return this.repository.AddNewNote(noteData, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<NotesModel> GetAllNotes(int userId)
        {
            try
            {
                return this.repository.GetAllNotes(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotesModel> GetBinNotes(int userId)
        {
            try
            {
                return this.repository.GetBinNotes(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<NotesModel> GetArchiveNotes(int userId)
        {
            try
            {
                return this.repository.GetArchiveNotes(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool BinRestoreNote(int noteId, int userId)
        {
            try
            {
                return this.repository.BinRestoreNote(noteId, userId);
            }
            catch
            {
                throw;

            }
        }

        public List<NotesModel> GetNoteByNoteId(int userId, int noteId)
        {
            try
            {
                return this.repository.GetNoteByNoteId(userId, noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ArchiveUnarchiveNote(int noteId, int userId)
        {
            try
            {
                return this.repository.ArchiveUnarchiveNote(noteId, userId);
            }
            catch
            {
                throw;
            }
        }

        public bool DeleteNote(int noteId, int userId)
        {
            try
            {
                return repository.DeleteNote(noteId, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdateNote(NotesModel update, int noteId, int userId)
        {
            try
            {
                return this.repository.UpdateNote(update, noteId, userId);
            }
            catch
            {
                throw;
            }
        }

        IEnumerable INoteManager.GetNoteByNoteId(int userId, int noteId)
        {
            throw new NotImplementedException();
        }
        public bool PinUnpinNote(int noteId, int userId)
        {
            try
            {
                return this.repository.PinUnpinNote(noteId, userId);
            }
            catch
            {
                throw;
            }
        }
        public bool AddColor(int noteId, string color, int userId)
        {
            try
            {
                return this.repository.AddColor(noteId, color, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddImage(int noteId, IFormFile imagePath)
        {
            try
            {
                return this.repository.AddImage(noteId, imagePath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool DeleteImage(int noteId)
        {
            try
            {
                return this.repository.DeleteImage(noteId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}