using FundooModel;
using FundooRepository.Context;
using FundooRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FundooRepository.Repository
{
    public class LabelRepository : ILabelRepository
        {
            private readonly UserContext userContext;
            public LabelRepository(UserContext userContext)
            {
                this.userContext = userContext;
            }
            public string CreateLabel(LabelModel labelModel)
            {
                try
                {
                    var checkLabelName = this.userContext.Labels.Where(a => a.LabelName.Equals(labelModel.LabelName) && a.UserId == labelModel.UserId).SingleOrDefault();
                    if (checkLabelName == null)
                    {
                        labelModel.NoteId = null;
                        this.userContext.Labels.Add(labelModel);
                        this.userContext.SaveChanges();

                        return "Created Label Successfully";
                    }

                    return "Label already exists";
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            public string AddLabel(LabelModel labelModel)
            {
                try
                {
                    var noteId = labelModel.NoteId;
                    this.CreateLabel(labelModel);
                    labelModel.NoteId = noteId;
                    var checkNoteId = this.userContext.Labels.Where(a => a.LabelName.Equals(labelModel.LabelName) && a.NoteId == labelModel.NoteId).SingleOrDefault();
                    if (checkNoteId == null)
                    {
                        labelModel.LabelId = 0;
                        this.userContext.Labels.Add(labelModel);
                        this.userContext.SaveChanges();
                        return "Label added successfully";
                    }

                    return "label not added successfully";
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            public string RemoveLabelInNotes(int labelId)
            {
                try
                {
                    var checkLabelId = this.userContext.Labels.Find(labelId);
                    if (checkLabelId != null)
                    {
                        this.userContext.Labels.Remove(checkLabelId);
                        this.userContext.SaveChanges();
                        return "Label removed successfully in notes";
                    }

                    return "label not removed";
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public string DeleteLabel(int labelId)
            {
                try
                {
                    var result = this.userContext.Labels.Where(a => a.LabelId == labelId).SingleOrDefault();
                    var exists = this.userContext.Labels.Where(a => a.LabelName == result.LabelName && a.UserId == result.UserId).ToList();
                    if (exists.Count != 0)
                    {
                        this.userContext.Labels.RemoveRange(exists);
                        this.userContext.SaveChanges();
                        return "Deleted Label Successfully";
                    }

                    return "no Label exist";
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        public List<LabelModel> GetAllLabels(int userId)
        {
            try
            {
                var checkuserId = this.userContext.Labels.Where(a => a.UserId == userId && a.NoteId == null).ToList();
                if (checkuserId.Count > 0)
                {
                    return checkuserId;
                }

                return default;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<LabelModel> GetLabelByNotes(int noteId, int userId)
            {
                try
                {
                    var checkNoteId = this.userContext.Labels.Where(a => a.NoteId == noteId && a.UserId == userId).ToList();
                    if (checkNoteId.Count > 0)
                    {
                        return checkNoteId;
                    }

                    return default;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public string EditLabel(LabelModel labelModel)
            {
                string message = "Updated successfully";
                try
                {
                    var oldLabeldata = this.userContext.Labels.Where(x => x.LabelId == labelModel.LabelId).SingleOrDefault();
                    var updateList = this.userContext.Labels.Where(x => x.LabelName.Equals(oldLabeldata.LabelName) && x.UserId == labelModel.UserId).ToList();
                    var checkLabelName = this.userContext.Labels.Where(x => x.LabelName.Equals(labelModel.LabelName) && x.UserId == labelModel.UserId).FirstOrDefault();
                    if (checkLabelName != null)
                    {
                        var mergeLabel = this.userContext.Labels.Find(labelModel.LabelId);
                        updateList.Remove(mergeLabel);
                        this.userContext.Labels.Remove(mergeLabel);
                        this.userContext.SaveChanges();
                        message = "Merge the" + oldLabeldata.LabelName + " label with the" + checkLabelName.LabelName + " label? All notes labelled with" + oldLabeldata.LabelName + " will be labelled with" + checkLabelName.LabelName + " and the" + oldLabeldata.LabelName + " label will be deleted";
                    }

                    foreach (var data in updateList)
                    {
                        data.LabelName = labelModel.LabelName;
                    }

                    this.userContext.Labels.UpdateRange(updateList);
                    this.userContext.SaveChanges();
                    return message;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            public List<NotesModel> GetNotesByLabel(LabelModel labelModel)
            {
                try
                {
                    var noteIdList = this.userContext.Labels.Where(a => a.LabelName == labelModel.LabelName && a.UserId == labelModel.UserId && a.NoteId != null).Select(x => x.NoteId).ToList();
                    List<NotesModel> notesList = new List<NotesModel>();
                    foreach (var data in noteIdList)
                    {
                        var d = this.userContext.Notes.Where(x => x.NoteId == data).SingleOrDefault();
                        notesList.Add(d);
                    }

                    return notesList;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }