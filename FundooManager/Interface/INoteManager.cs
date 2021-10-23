using FundooModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FundooManager.Interface
{
    public interface INoteManager
    {
        bool AddNewNote(AddNote noteData, int userId);
        List<NotesModel> GetAllNotes(int userId);
        List<NotesModel> GetBinNotes(int userId);
        List<NotesModel> GetArchiveNotes(int userId);
        bool BinRestoreNote(int noteId, int userId);
        bool ArchiveUnarchiveNote(int noteId, int userId);
        bool DeleteNote(int noteId, int userId);
        IEnumerable GetNoteByNoteId(int userId, int noteId);
        bool UpdateNote(NotesModel update, int noteId, int userId);
        bool PinUnpinNote(int noteId, int userId);
        bool AddColor(int noteId, string color, int userId);
        bool AddImage(int noteId, IFormFile imagePath);
        bool DeleteImage(int noteId);
    }
}
