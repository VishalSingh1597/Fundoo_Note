//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Text;

//namespace FundooModel

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="BridgeLabs">
//   Copyright © 2021 Company="BridgeLabs"
// </copyright>
// <creator name="Vishal"/>
// ----------------------------------------------------------------------------------------------------------

namespace Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using FundooModel;

    /// <summary>
    /// LabelModel class
    /// </summary>
    public class LabelModel
    {
        /// <summary>
        /// Gets or sets the label identifier.
        /// </summary>
        [Key]
        public int LabelId { get; set; }

        /// <summary>
        /// Gets or sets the note identifier.
        /// </summary>
        [ForeignKey("NotesModel")]
        public int? NoteId { get; set; }

        /// <summary>
        /// Gets or sets the notes model.
        /// </summary>
        public virtual NotesModel NotesModel { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        [ForeignKey("UserModel")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the register model.
        /// </summary>
        public virtual UserModel UserModel { get; set; }

        /// <summary>
        /// Gets or sets the name of the label.
        /// </summary>
        public string LabelName { get; set; }
    }
}
