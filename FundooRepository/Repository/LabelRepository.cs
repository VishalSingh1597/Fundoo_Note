//using FundooModel;
//using FundooRepository.Context;
//using FundooRepository.Interface;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="BridgeLabs">
//   Copyright © 2021 Company="BridgeLabs"
// </copyright>
// <creator name="Vishal"/>
// ----------------------------------------------------------------------------------------------------------
namespace FundooRepository.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using FundooRepository.Context;
    using FundooRepository.Interface;
    using FundooModel;

    /// <summary>
    /// LabelRepository class
    /// </summary>
    public class LabelRepository : ILabelRepository
    {
        /// <summary>
        /// user Context
        /// </summary>
        private readonly UserContext userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelRepository"/> class.
        /// </summary>
        /// <param name="userContext">userContext identifier</param>
        public LabelRepository(UserContext userContext)
        {
            this.userContext = userContext;
        }

        /// <summary>
        /// Adds the label to note.
        /// </summary>
        /// <param name="labelData">The label data.</param>
        /// <returns>return string success or not</returns>
        public string AddLabelToNote(LabelModel labelModel)
        {
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
        }

            /// <summary>
            /// Adds the label to user.
            /// </summary>
            /// <param name="labelData">The label data.</param>
            /// <returns>return string success or not</returns>
            public string AddLabelToUser(LabelModel labelModel)
        {
            try
            {
                var noteId = labelModel.NoteId;
                this.AddLabelToNote(labelModel);
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

        /// <summary>
        /// Deletes the label.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="labelName">Name of the label.</param>
        /// <returns>return true if success</returns>
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

        public string DeleteLabel(LabelModel labelModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the label on note.
        /// </summary>
        /// <param name="labelId">The label identifier.</param>
        /// <returns>return true if success</returns>
        public string DeleteLabelOnNote(int labelId)
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


        /// <summary>
        /// Edits the name of the label.
        /// </summary>
        /// <param name="labelData"></param>
        /// <returns>true if label updated</returns>
        public bool EditLabelName(LabelModel labelModel)
        {
            try
            {
                var changeLabel = this.userContext.Labels.Find(labelModel.LabelId);
                var checkLabel = this.userContext.Labels.Where(x => x.UserId == labelModel.UserId && x.LabelName.Equals(changeLabel.LabelName)).ToList();
                if (checkLabel != null)
                {
                    checkLabel.ForEach(a => a.LabelName = labelModel.LabelName);
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

        /// <summary>
        /// Gets the labels.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>all Labels</returns>
        public List<string> GetLabels(int userId)
        {
            try
            {
                var labels = this.userContext.Labels.Where(x => x.UserId == userId).Select(i => i.LabelName).Distinct().ToList();
                if (labels.Count > 0)
                {
                    return labels;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets the labels notes.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="labelName">Name of the label.</param>
        /// <returns>all notes of labels</returns>
        public List<NotesModel> GetLabelsNotes(LabelModel labelModel)
        {
            try
            {
                var labels = (from label in this.userContext.Labels
                              join notes in this.userContext.Notes on label.NoteId equals notes.NoteId
                              where (label.UserId == labelModel.UserId && label.LabelName.Equals(labelModel.LabelName))
                              select notes).ToList();
                if (labels != null)
                {
                    return labels;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  get Notes label
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns>list of labels for note</returns>
        public List<LabelModel> GetNotesLabel(int noteId)
        {
            try
            {
                var labels = this.userContext.Labels.Where(x => x.NoteId == noteId).ToList();
                if (labels.Count != 0)
                {
                    return labels;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}