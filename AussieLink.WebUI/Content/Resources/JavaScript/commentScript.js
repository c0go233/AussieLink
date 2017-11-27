var FilterName = {
    CURRENTPOSTID: "CurrentPostId"
};

var COMMENTTAG = {
    CommentInputSubmitBtn: "button#comment-submit-btn", CommentList: "ul#comment-list",
    CommentEditCommentBtn: "button.c-comment__edit-comment-btn", CommentEditBtn: "button.c-comment__edit-btn",
    CommentEditCancelBtn: "button.c-comment__edit-cancel-btn", CommentDeleteBtn: "button.c-comment__delete-btn",
    PostType: "PostType"
};

$(document).ready(function () {
    enableCommentClickEvent();
    enableEditCommentClickEvent();
    enableEditClickEvent();
    enableCancelClickEvent();
    enableDeleteClickEvent();
    getComments();
});

//====================COMMENT ===============================================================//

function getComments() {
    var postId = $("input[name='" + FilterName.CURRENTPOSTID + "']").val();
    var postType = $("input[name='" + COMMENTTAG.PostType + "']").val();

    $.ajax({
        type: "GET",
        contentType: "application/json",
        url: "/api/comment?postId=" + postId + "&postType=" + postType,
        success: function (data) {
            if (data.length)
                addComments(data);
        },
        error: function (e) {
        }
    });
}

function addComments(data) {
    for (var i = 0; i < data.length; i++) {
        addComment(data[i]);
    }
}

function enableCommentClickEvent() {
    $(COMMENTTAG.CommentInputSubmitBtn).on("click", function () {
        var postType = $("input[name='" + COMMENTTAG.PostType + "']").val();
        var postId = $("input[name='" + FilterName.CURRENTPOSTID + "']").val();
        var textarea = $("textarea#comment-textarea");
        var description = textarea.val();
        var errorWraper = $("div#comment-error-wraper");
        postComment(postId, postType, description, "", errorWraper, textarea, $());
    });
}

function enableEditCommentClickEvent() {
    $(COMMENTTAG.CommentList).on("click", COMMENTTAG.CommentEditCommentBtn, function () {
        var commentId = $(this).attr("data-comment-param");
        var postType = $("input[name='" + COMMENTTAG.PostType + "']").val();
        var postId = $("input[name='" + FilterName.CURRENTPOSTID + "']").val();

        var description = $(this).parents("div.c-comment__edit-wraper").children("textarea").val();
        var errorWraper = $(this).parents("div.c-comment__edit-bottom").children("div.c-comment__edit-error-wraper");
        var textToReplace = $(this).parents("li.c-comment__content").find("div.c-comment__content-text");
        var editBox = $(this).parents("li.c-comment__content");
        postComment(postId, postType, description, commentId, errorWraper, textToReplace, editBox);
    });
}

//determine if it updates or creates new comment by editbox
function postComment(postId, postType, description, commentId, errorWraper, textToReplace, editBox) {
    $.ajax({
        type: "POST",
        contentType: 'application/json',
        url: "/api/Comment",
        data: getJsonCommentRequest(postType, postId, description, commentId),
        success: function (data) {
            removeCommentErrorMessage(errorWraper);
            updateComment(editBox, data, textToReplace);
        },
        error: function (e) {
            addCommentErrorMessageSet(e);
        }
    });
}

function addCommentErrorMessageSet(e) {
    var errorMessage = JSON.parse(e.responseText).message;
    //make sure only one message is there 
    removeCommentErrorMessage(errorWraper);
    addCommentErrorMessage(errorMessage, errorWraper);
}

function updateComment(editBox, data, textToReplace) {
    if (!editBox.length) {
        addComment(data);
        textToReplace.val("");
    }
    else {
        textToReplace.text(data.description);
        editBox.removeClass("open-edit-box");
    }
}

function getJsonCommentRequest(postType, postId, description, commentId) {
    return JSON.stringify({
        PostType: postType,
        PostId: postId,
        Description: description,
        CommentId: commentId
    })
}

function addCommentErrorMessage(errorMessage, errorWraper) {
    var errorSpan = $("<span class='c-comment-error'>" + errorMessage + "</span>");
    errorSpan.appendTo(errorWraper);
}

function removeCommentErrorMessage(errorWraper) {
    var errorSpan = errorWraper.children("span.c-comment-error");
    errorSpan.remove();
}

function enableEditClickEvent() {
    $(COMMENTTAG.CommentList).on("click", COMMENTTAG.CommentEditBtn, function () {
        var commentItem = $(this).parents("li.c-comment__content");
        var commentDesc = commentItem.find("div.c-comment__content-text").text();
        var editInput = commentItem.find("div.c-comment__edit-wraper").children("textarea");
        editInput.val(commentDesc);
        commentItem.addClass("open-edit-box");
    });
}

function enableCancelClickEvent() {
    $(COMMENTTAG.CommentList).on("click", COMMENTTAG.CommentEditCancelBtn, function () {
        var commentItem = $(this).parents("li.c-comment__content");
        commentItem.removeClass("open-edit-box");
    });
}

function enableDeleteClickEvent() {
    $(COMMENTTAG.CommentList).on("click", COMMENTTAG.CommentDeleteBtn, function () {
        var commentId = $(this).attr("data-comment-param");
        var commentItem = $(this).parents("li.c-comment__content");
        var errorWraper = $(this).parents("div.c-comment__content-bottom").find("div.c-comment__error-wraper");

        $.ajax({
            type: "DELETE",
            contentType: "application/json",
            url: "/api/comment?commentId=" + commentId,
            success: function () {
                removeCommentErrorMessage(errorWraper);
                commentItem.remove();
            },
            error: function (e) {
                addCommentErrorMessageSet(e);
            }
        });
    });
}

//===================ADD COMMENT===================================================//

function addComment(data) {
    var commentList = $("ul#comment-list");
    var commentItem = $("<li class='c-comment__content'></li>");

    var commentWraper = getCommentWraper(data);
    var editBox = getEditCommentBox(data);

    if (data.isOwned) {
        var commentBottom = getCommentBottom(data);
        commentBottom.appendTo(commentWraper);
    }
    commentWraper.appendTo(commentItem);
    editBox.appendTo(commentItem);

    commentItem.appendTo(commentList);
}

function getCommentBottom(data) {
    var commentBottom = $("<div class='c-comment__content-bottom fx-spc-btw fx-algn-center'></div>");
    var commentErrorWraper = $("<div class='c-comment__error-wraper'></div>");
    var commentBtnWraper = $("<div></div>")
    var commentDeleteAnchor = $("<button class='c-comment__delete-btn c-comment__manage-link c-pd-mg-bd-r-10' data-comment-param='" + data.commentId + "'>Delete</button>");
    var commentEditAnchor = $("<button class='c-comment__edit-btn c-comment__manage-link'>Edit</button>");

    commentDeleteAnchor.appendTo(commentBtnWraper);
    commentEditAnchor.appendTo(commentBtnWraper);
    commentBtnWraper.appendTo(commentBottom);
    commentErrorWraper.appendTo(commentBottom);
    return commentBottom;
}

function getCommentWraper(data) {
    var commentWraper = $("<div class='c-comment__content-wraper'></div>");
    var commentHeader = $("<div class='c-comment__content-header fx'></div>");
    var commentNameAnchor = $("<a href='#' class='c-mg-r-5 c-link'>" + data.userName + "</a>");
    var commentDate = $("<span class='c-comment__content-date'>" + "&middot; " + data.dateCreated + "</span>");
    var commentDesc = $("<div class='c-comment__content-text'>" + data.description + "</div>");

    commentNameAnchor.appendTo(commentHeader);
    commentDate.appendTo(commentHeader);
    commentHeader.appendTo(commentWraper);
    commentDesc.appendTo(commentWraper);
    return commentWraper;
}

function getEditCommentBox(data) {
    var commentEditWraper = $("<div class='c-comment__edit-wraper'></div>");
    var commentEditInput = $("<textarea class='form-input c-mg-b-5 fx-1' type='text' />");
    var commentEditBottom = getCommentEditBottom(data);

    commentEditInput.appendTo(commentEditWraper);
    commentEditBottom.appendTo(commentEditWraper);

    return commentEditWraper;
}

function getCommentEditBottom(data) {
    var commentEditBottom = $("<div class='c-comment__edit-bottom fx-spc-btw fx-algn-center'></div>");
    var commentEditBtnWraper = $("<div class='c-comment__edit-btn-wraper'></div>");
    var cancelBtn = $("<button class='c-comment__edit-cancel-btn c-btn-gray c-comment__edit-box-btn c-mg-r-5'>Cancel</button>");
    var commentBtn = $("<button class='c-comment__edit-comment-btn c-btn c-btn-border c-comment__edit-box-btn' data-comment-param='" + data.commentId + "'>Comment</button>");
    var commentEditErrorWraper = $("<div class='c-comment__edit-error-wraper'></div>");

    cancelBtn.appendTo(commentEditBtnWraper);
    commentBtn.appendTo(commentEditBtnWraper);
    commentEditErrorWraper.appendTo(commentEditBottom);
    commentEditBtnWraper.appendTo(commentEditBottom);
    return commentEditBottom;
}