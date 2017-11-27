using AussieLink.Contracts.Dtos;
using AussieLink.Contracts.Enums;
using AussieLink.Contracts.ExtensionMethods;
using AussieLink.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace AussieLink.WebUI.Controllers.Api
{
    public class CommentController : ApiController
    {
        private readonly ICommentService CommentService;

        public CommentController(ICommentService commentService)
        {
            this.CommentService = commentService;
        }

        [HttpGet]
        public IHttpActionResult GetComments(string postId, string postType)
        {
            var comments = CommentService.GetComments(postId, postType, GetUserEmailFromAuthCookie());
            return Ok(comments);
        }

        [HttpDelete]
        public IHttpActionResult Delete(string commentId)
        {
            if (!CommentService.DeleteComment(commentId, GetUserEmailFromAuthCookie()))
                return BadRequest(ErrorCode.COMMENTBADREQUEST.ToDescription());

            return Ok();
        }

        //api/comment/
        [HttpPost]
        //[Authorize]
        public IHttpActionResult Save(CommentRequestDto dto)
        {
            var response = CommentService.SaveComment(dto, GetUserEmailFromAuthCookie());
            if (!response.Success)
                return BadRequest(response.ErrorCode.ToDescription());

            return Ok(response.Comment);
        }

        private string GetUserEmailFromAuthCookie()
        {
            var cookies = Request.Headers.GetCookies(FormsAuthentication.FormsCookieName).FirstOrDefault();
            var userEmail = "";
            if (cookies != null)
            {
                foreach (var cookie in cookies.Cookies)
                {
                    if (cookie.Name == FormsAuthentication.FormsCookieName)
                    {
                        var encryptedUserEmail = cookie.Value;
                        var decryptedTicket = FormsAuthentication.Decrypt(encryptedUserEmail);
                        userEmail = decryptedTicket.Name;
                    }
                }
            }
            return userEmail;
        }
    }
}
