using FundooModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooRepository.Interface
{
    public interface ICollaboratorRepository
    { 
    string AddCollaborator(CollaboratorModel collaboratorData);
    string DeleteCollaborator( int collaboratorId);

    List<CollaboratorModel> GetCollaborator(int noteId);
       //string DeleteCollaborator(int collaboratorId);
    }
}