using FundooManager.Interface;
using FundooModel;
using FundooRepository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooManager.Manager
{
    public class LabelManager : ILabelManager
    {

        private readonly ILabelRepository labelRepository;

        public LabelManager(ILabelRepository labelRepository)
        {
            this.labelRepository = labelRepository;
        }

        public string CreateLabel(LabelModel labelModel)
        {
            try
            {
                return this.labelRepository.CreateLabel(labelModel);
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
                return this.labelRepository.AddLabel(labelModel);
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
                return this.labelRepository.RemoveLabelInNotes(labelId);
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
                return this.labelRepository.DeleteLabel(labelId);
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
                return this.labelRepository.GetAllLabels(userId);
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
                return this.labelRepository.GetLabelByNotes(noteId, userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string EditLabel(LabelModel labelModel)
        {
            try
            {
                return this.labelRepository.EditLabel(labelModel);
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
                return this.labelRepository.GetNotesByLabel(labelModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}