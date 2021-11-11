//using FundooModel;
//using Models;
//using System;
//using System.Collections.Generic;
//using System.Text;


// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="BridgeLabs">
//   Copyright © 2021 Company="BridgeLabs"
// </copyright>
// <creator name="Vishal"/>
// ----------------------------------------------------------------------------------------------------------

namespace FundooRepository.Interface
{
    using System.Collections.Generic;
    using FundooModel;
    using Models;

    /// <summary>
    /// ILabelRepository interface
    /// </summary>
    public interface ILabelRepository
    {
        /// <summary>
        /// Adds the label to note.
        /// </summary>
        /// <param name="labelData">The label data.</param>
        /// <returns>return string success or not</returns>
        string AddLabelToNote(LabelModel labelModel);

        /// <summary>
        /// Adds the label to user.
        /// </summary>
        /// <param name="labelData">The label data.</param>
        /// <returns>return string success or not</returns>
        string AddLabelToUser(LabelModel labelModel);

        /// <summary>
        /// Deletes the label on note.
        /// </summary>
        /// <param name="labelId">The label identifier.</param>
        /// <returns>return true if success</returns>
        string DeleteLabelOnNote(int labelId);

        /// <summary>
        /// Deletes the label.
        /// </summary>
        /// <param name="labelData">The label data.</param>
        /// <returns>true if label deleted</returns>
        string DeleteLabel(LabelModel labelModel);

        /// <summary>
        /// Edits the name of the label.
        /// </summary>
        /// <param name="labelData">The label data.</param>
        /// <returns>true if label updated</returns>
        bool EditLabelName(LabelModel labelModel);

        /// <summary>
        /// Gets the labels.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>all Labels</returns>
        List<string> GetLabels(int userId);

        /// <summary>
        /// Gets the labels notes.
        /// </summary>
        /// <param name="labelData">The label data.</param>
        /// <returns>all notes of label</returns>
        List<NotesModel> GetLabelsNotes(LabelModel labelModel);

        /// <summary>
        /// Gets the notes label.
        /// </summary>
        /// <param name="noteId">The note identifier.</param>
        /// <returns>list of Labels of note</returns>
        List<LabelModel> GetNotesLabel(int noteId);
    }
}