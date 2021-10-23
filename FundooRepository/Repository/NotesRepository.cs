using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FundooModel;
using FundooRepository.Context;
using FundooRepository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Account = Microsoft.VisualStudio.Services.Account.Account;

namespace FundooRepository.Repository
{
    public class NotesRepository : INotesRepository
    {
        private readonly UserContext userContext;
        private readonly object notesId;

        public NotesRepository(UserContext userContext)
        {
            this.userContext = userContext;
        }
        UserModel user = new UserModel();
        private object configuration;
        private int noteId;

        public bool AddNewNote(AddNote noteData, int userId)
        {
            try
            {
                NotesModel notes = new NotesModel()
                {
                    Title = noteData.Title,
                    Description = noteData.Description,
                    Reminder = noteData.Reminder,
                    Collaborator = noteData.Collaborator,
                    Color = noteData.Color,
                    Image = noteData.Image,
                    IsArchive = noteData.IsArchive,
                    IsPin = noteData.IsPin,
                    IsTrash = noteData.IsTrash
                };

                user = userContext.Users.FirstOrDefault(x => x.UserId == userId);
                notes.Users = user;

                if (noteData.Title != null || noteData.Description != null)
                {
                    userContext.Notes.Add(notes);
                    userContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<NotesModel> GetAllNotes(int userId)
        {
         var notes = this.userContext.Notes.Where(x => x.UserId == userId && x.IsArchive == false && x.IsPin == false).ToList();
            try
            {
                if (notes != null)
                {
                    return notes;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public List<NotesModel> GetArchiveNotes(int userId)
        {
            try
            {
                var result = userContext.Notes.Where(e => e.UserId == userId && e.IsArchive == true).ToList();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
            }

        public List<NotesModel> GetBinNotes(int userId)
        {
            var notes = this.userContext.Notes.Where(x => x.UserId == userId && x.IsArchive == false && x.IsBin == false).ToList();
            try
            {
                if (notes != null)
                {
                    return notes;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool BinRestoreNote(int noteId, int userId)
        {
            try
            {
                var note = userContext.Notes.FirstOrDefault(x => x.NoteId == noteId && x.UserId == userId);

                if (note != null)
                {
                    if (note.IsBin == false)
                    {
                        note.IsBin = true;
                        userContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        note.IsBin = false;
                        userContext.SaveChanges();
                        return true;
                    }
                }
                return false;
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
                var note = userContext.Notes.FirstOrDefault(x => x.NoteId == noteId && x.UserId == userId);

                if (note != null)
                {
                    if (note.IsArchive == false)
                    {
                        note.IsArchive = true;
                        userContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        note.IsArchive = false;
                        userContext.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteNote(int noteId, int userId)
        {
            try
            {
                var noteToBeRemoved = userContext.Notes.FirstOrDefault(x => x.NoteId == noteId && x.UserId == userId && x.IsBin == true);

                if (noteToBeRemoved != null)
                {
                    userContext.Notes.Remove(noteToBeRemoved);
                    userContext.SaveChanges();
                    return true;
                }
                return false;
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
                var noteToBeUpdated = userContext.Notes.FirstOrDefault(x => x.NoteId == noteId && x.UserId == userId && x.IsBin == false);

                noteToBeUpdated.Title = update.Title;
                noteToBeUpdated.Description = update.Description;

                if (update.Title != null || update.Description != null)
                {
                    userContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool PinUnpinNote(int noteId, int userId)
        {
            try
            {
                var note = userContext.Notes.FirstOrDefault(x => x.NoteId == noteId && x.UserId == userId);

                if (note != null)
                {
                    if (note.IsPin == false)
                    {
                        note.IsPin = true;
                        userContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        note.IsPin = false;
                        userContext.SaveChanges();

                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddColor(int noteId, string color, int userId)
        {
            try
            {
                var note = userContext.Notes.SingleOrDefault(x => x.NoteId == noteId && x.IsBin == false);

                if (note != null)
                {
                    note.Color = color;
                    userContext.Entry(note).State = EntityState.Modified;
                    userContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
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
                var note = this.userContext.Notes.Where(x => x.NoteId == noteId && x.IsBin == false).SingleOrDefault();
                if (note != null)
                {
                    Account account = new Account(this.configuration.Equals("CloudinaryAccount").Equals("CloudName").ToString, this.configuration.Equals("CloudinaryAccount").Equals("ApiKey").ToString, this.configuration.Equals("CloudinaryAccount").Equals("ApiSecret").ToString);
                    Cloudinary cloudinary = new Cloudinary(account);

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(imagePath.FileName, imagePath.OpenReadStream())
                    };
                    var uploadResult = cloudinary.Upload(uploadParams);
                    Uri x = uploadResult.Url;
                    note.Image = x.ToString();
                    this.userContext.SaveChanges();
                    return true;
                }

                return false;
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
                var checkNote = this.userContext.Notes.Find(noteId);
                if (checkNote != null)
                {
                    checkNote.Image = null;
                    this.userContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<NotesModel> GetNoteByNoteId(int userId, int noteId)
        {
            throw new NotImplementedException();
        }
    }
    }
 
