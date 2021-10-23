using FundooModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooRepository.Interface
{
    public interface ILabelRepository
    {
        string CreateLabel(LabelModel labelModel);
        string AddLabel(LabelModel labelModel);
        string RemoveLabelInNotes(int labelId);
        string DeleteLabel(int labelId);
        List<LabelModel> GetAllLabels(int userId);
        List<LabelModel> GetLabelByNotes(int noteId, int userId);
        string EditLabel(LabelModel labelModel);
        List<NotesModel> GetNotesByLabel(LabelModel labelModel);
    }
}
