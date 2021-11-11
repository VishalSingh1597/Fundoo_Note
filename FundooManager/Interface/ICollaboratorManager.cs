using FundooModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooManager.Interface
{
    public interface ICollaboratorManager
    {

        string AddCollaborator(CollaboratorModel collaboratorModel);
        string DeleteCollaborator( int collaboratorId);
        List<CollaboratorModel> GetCollaborator(int noteId);
    }
}

