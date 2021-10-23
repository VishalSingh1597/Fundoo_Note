using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FundooModel
{
        public class LabelModel
        {
            [Key]
            public int LabelId { get; set; }
            [Required]
            public string LabelName { get; set; }
            public int UserId { get; set; }
            [ForeignKey("UserId")]
            //public virtual LoginResponse LoginResponse { get; set; }
            public int? NoteId { get; set; }
            [ForeignKey("NoteId")]
            public virtual NotesModel NotesModel { get; set; }
        }
    }
