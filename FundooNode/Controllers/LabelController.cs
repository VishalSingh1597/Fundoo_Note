//using FundooManager.Interface;
//using FundooModel;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;


// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="BridgeLabs">
//   Copyright © 2021 Company="BridgeLabs"
// </copyright>
// <creator name="Vishal"/>
// ----------------------------------------------------------------------------------------------------------

namespace FundooNote.Controllers
{
    using System;
    using System.Collections.Generic;
    using FundooModel;
    using FundooManager.Interface;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    /// <summary>
    /// LabelController class
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Authorize]
    public class LabelController : ControllerBase
    {
        /// <summary>
        /// The manager
        /// </summary>
        private readonly ILabelManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelController"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public LabelController(ILabelManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Adds the label to note.
        /// </summary>
        /// <param name="labelData">The label data.</param>
        /// <returns>"Label Added Successfully or not</returns>
        [HttpPost]
        [Route("api/addLabelToNote")]
        public IActionResult AddLabelToNote([FromBody] LabelModel labelModel)
        {
            try
            {
                string result = this.manager.AddLabelToNote(labelModel);
                if (result.Equals("Label Added Successfully"))
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

        /// <summary>
        /// Adds the label to user.
        /// </summary>
        /// <param name="labelData">The label data.</param>
        /// <returns>Label Added Successfully or not</returns>
        [HttpPost]
        [Route("api/addLabelToUser")]
        public IActionResult AddLabelToUser([FromBody] LabelModel labelModel)
        {
            try
            {
                string result = this.manager.AddLabelToUser(labelModel);
                if (result.Equals("Label Added Successfully"))
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

        /// <summary>
        /// Deletes the label on note.
        /// </summary>
        /// <param name="labelId">The label identifier.</param>
        /// <returns>return true if Label Deleted Successfully</returns>
        [HttpDelete]
        [Route("api/deleteLabelOnNote")]
        public IActionResult DeleteLabelOnNote(int labelId)
        {
            try
            {
                string message = this.manager.DeleteLabelOnNote(labelId);
                if (message.Equals("Label removed successfully in notes"))
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = message });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = message });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes the label.
        /// </summary>
        /// <param name="labelData">The label data.</param>
        /// <returns>return true if Label Deleted Successfully</returns>
        [HttpDelete]
        [Route("api/deleteLabel")]
        public IActionResult DeleteLabel([FromBody] LabelModel labelModel)
        {
            try
            {
                string message = this.manager.DeleteLabel(labelModel);
                if (message.Equals("Deleted Label Successfully"))
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = message });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = message });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Edits the name of the label.
        /// </summary>
        /// <param name="labelData">The label data.</param>
        /// <returns>return true if Label updated Successfully</returns>
        [HttpPut]
        [Route("api/editLabelName")]
        public IActionResult EditLabelName([FromBody] LabelModel labelModel)
        {
            try
            {
                bool result = this.manager.EditLabelName(labelModel);
                if (result)
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Label Name Updated Successfully" });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Edit Label Failed !! Label Not Exist" });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Gets the labels.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>All Labels</returns>
        [HttpGet]
        [Route("api/getLabels")]
        public IActionResult GetLabels(int userId)
        {
            try
            {
                var result = this.manager.GetLabels(userId);
                if (result.Count > 0)
                {
                    return this.Ok(new ResponseModel<List<string>>() { Status = true, Message = "All Labels are fetched Successfully", Data = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Getting Labels Failed" });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Gets the labels notes.
        /// </summary>
        /// <param name="labelData">The label data.</param>
        /// <returns>all notes attach with the label</returns>
        [HttpGet]
        [Route("api/getLabelsNotes")]
        public IActionResult GetLabelsNotes([FromBody] LabelModel labelModel)
        {
            try
            {
                var result = this.manager.GetLabelsNotes(labelModel);
                if (result.Count > 0)
                {
                    return this.Ok(new ResponseModel<List<NotesModel>>() { Status = true, Message = "All Labels are fetched Successfully", Data = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Getting Labels Failed" });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Gets the notes label.
        /// </summary>
        /// <param name="noteId">The note identifier.</param>
        /// <returns>list of labels</returns>
        [HttpGet]
        [Route("api/getNotesLabel")]
        public IActionResult GetNotesLabel(int noteId)
        {
            try
            {
                var result = this.manager.GetNotesLabel(noteId);
                if (result.Count > 0)
                {
                    return this.Ok(new ResponseModel<List<LabelModel>>() { Status = true, Message = "All Labels are fetched Successfully", Data = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Getting Labels Failed" });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }
    }
}