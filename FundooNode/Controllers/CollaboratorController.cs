using FundooManager.Interface;
using FundooManager.Manager;
using FundooModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNote.Controllers
{
    [Authorize]
    public class CollaboratorController : ControllerBase
    {
 
        private readonly CollaboratorManager manager;

        public CollaboratorController(CollaboratorManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("api/addCollaborator")]
        public IActionResult AddNote([FromBody] CollaboratorModel collaboratorModel)
        {
            try
            {
                string result = this.manager.AddCollaborator(collaboratorModel);
                if (result.Equals("collaborator Added Successfully"))
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = result });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("api/deleteCollaborator")]
        public IActionResult DeleteCollaborator( int collaboratorId)
        {
            try
            {
                string result = this.manager.DeleteCollaborator( collaboratorId);
                if (result.Equals("Collaborator Deleted Successfully"))
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = result });
                }

                return this.BadRequest(new ResponseModel<string>() { Status = false, Message = result });
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }


        [HttpGet]
        [Route("api/getCollaborator")]
        public IActionResult GetCollaborator(int noteId)
        {
            try
            {
                var result = this.manager.GetCollaborator(noteId);
                if (result.Count > 0)
                {
                    return this.Ok(new ResponseModel<List<CollaboratorModel>>() { Status = true, Message = "All Notes are Succesfully Fetched", Data = result });
                }

                return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Error!!No Note Found on Archive" });
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }
    }
}