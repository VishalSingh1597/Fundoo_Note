﻿using FundooManager.Interface;
using FundooModel;
using FundooRepository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooManager.Manager
{
    public class CollaboratorManager : ICollaboratorManager
    {
        private readonly ICollaboratorRepository repository;

        public CollaboratorManager(ICollaboratorRepository repository)
        {
            this.repository = repository;
        }

        public string AddCollaborator(CollaboratorModel collaboratorData)
        {
            try
            {
                return this.repository.AddCollaborator(collaboratorData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string DeleteCollaborator(int noteId, int collaboratorId)
        {
            try
            {
                return this.repository.DeleteCollaborator(noteId, collaboratorId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CollaboratorModel> GetCollaborator(int noteId)
        {
            try
            {
                return this.repository.GetCollaborator(noteId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
        
   
