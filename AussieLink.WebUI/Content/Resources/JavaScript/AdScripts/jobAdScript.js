var FilterMarkup = { SELECTEDFILTERS: "span.selected-filters", SELECTEDFILTER: "span.selected-filter" };
var FilterType = { RADIO: "radio", DROPDOWN: "dropdown", TEXT: "text" };
var FilterName = {
    PLACE: "Place", CONTRACTTYPE: "ContractType", DAY: "Day", JOBTYPE: "JobType", SALARY: "Salary",
    SALARYTYPE: "SalaryType", KEYWORD: "Keyword", PAGEINDEX: "PageIndex", CURRENTPOSTID: "CurrentPostId"
};
var COMMENTTAG = {
    CommentInputSubmitBtn: "button#comment-submit-btn", CommentList: "ul#comment-list",
    CommentEditCommentBtn: "button.c-comment__edit-comment-btn", CommentEditBtn: "button.c-comment__edit-btn",
    CommentEditCancelBtn: "button.c-comment__edit-cancel-btn", CommentDeleteBtn: "button.c-comment__delete-btn",
    PostType: "PostType"
};
var SalaryDropdownId = { HOURLYDROPDONW: "A_Hour", WEEKLYDROPDOWN: "A_Week" }
var DetailPageLink = "https://localhost:44308/Ad/JobAdDetail?currentPostId=";
var DefaultPageIndex = 1;



$(document).ready(function () {
    enableAdListItemClickDelegateEvent();
    addAdList();
    setDetailedPost();
    setSalaryTypeSettings()
    setFilters();
    enableFilterEvents();
});

function enableAdListItemClickDelegateEvent() {
    $("ul#ad-list").on("click", "div.ad-list__ad-link", function () {
        var postId = $(this).attr("data-post-param");
        addDetailedAd(postId, true);
    });
}

function setDetailedPost() {
    var currentPostId = $("input[name='" + FilterName.CURRENTPOSTID + "']").val();
    if (currentPostId.length)
        addDetailedAd(currentPostId, false);
}

function setSalaryTypeSettings() {
    setSalaryTypeOpenClass();
    enableSalaryTypeChangeEvent();
} 

function setFilters() {
    setFilterFromRadioButton(FilterName.PLACE);
    setFilterFromRadioButton(FilterName.CONTRACTTYPE);
    setFilterFromDropdownList(FilterName.DAY);
    setFilterFromDropdownList(FilterName.JOBTYPE);
    setFilterFromSalaryDropdownList();
    setFilterFromInput(FilterName.KEYWORD);
}

function enableFilterEvents() {
    enableRadioBtnChangeEvent(FilterName.PLACE);
    enableRadioBtnChangeEvent(FilterName.CONTRACTTYPE);
    enableDropdownChangeEvent(FilterName.DAY);
    enableDropdownChangeEvent(FilterName.JOBTYPE);
    enableSalaryDropdownChangeEvent(SalaryDropdownId.HOURLYDROPDONW);
    enableSalaryDropdownChangeEvent(SalaryDropdownId.WEEKLYDROPDOWN);
    enableTextInputFilterBtnClickEvent(FilterName.KEYWORD);
    enableSelectedFilterClickEvent();
}

//==================GET DETAIL AD DATA SET=============================================//

function addDetailedAd(postId, needToResetMark) {
    if (needToResetMark) {
        resetSelectedMark();
        markPostSelected(postId);
    }
    updatePostIdInUrl(postId);
    $.ajax({
        url: "/Ad/JobAdDetail",
        data: {
            currentPostId: postId,
            returnUrl: window.location.pathname + window.location.search
        },
        dataType: "html",
        beforeSend: function () {

        },
        success: function (data) {
            if (data.length != 0) {
                $("div#ad-detail-page").html(data);
                getComments();
                enableEventsForDetailedPage();
                $(window).scrollTop(0);
            }
        },
        error: function (e) {
            $("div#ad-detail-page").html(e.responseText);
        }
    });
}

function enableEventsForDetailedPage() {
    enableBackToListLinkClickEvent();
    enableCommentClickEvent();
    enableEditClickEvent();
    enableCancelClickEvent();
    enableDeleteClickEvent();
    enableEditCommentClickEvent();
}

function enableBackToListLinkClickEvent() {
    $("span#back-to-list-link").on("click", function () {
        var scrollPosition = $("ul#ad-list").find("div.post-selected").offset().top;
        $(window).scrollTop(scrollPosition);
    });
}

function markPostSelected(postId) {
    var selectedPostTag = $("ul#ad-list").find("div[data-post-param='" + postId + "']");
    selectedPostTag.addClass("post-selected");
}

function resetSelectedMark() {
    var listItems = $("ul#ad-list").children();
    listItems.each(function () {
        var postTag = $(this).children("div.ad-list__ad-link");
        if (postTag.hasClass("post-selected")) {
            postTag.removeClass("post-selected");
            return false;
        }
    });
}

function updatePostIdInUrl(postId) {
    $("input[name='" + FilterName.CURRENTPOSTID + "']").val(postId);
    updateUrl();
}

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
    for(var i = 0; i < data.length; i++) {
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

//==================Add AdList SET========================================================//


function addAdList() {
    $.ajax({
        type: "GET",
        url: "/Ad/GetJobAd",
        data: {
            Place: getValueFromSelectedFilter(FilterName.PLACE),
            ContractType: getValueFromSelectedFilter(FilterName.CONTRACTTYPE),
            Salary: getValueFromSelectedFilter(FilterName.SALARY),
            SalaryType: getSalaryTypeFromSelectedFilter(),
            Day: getValueFromSelectedFilter(FilterName.DAY),
            JobType: getValueFromSelectedFilter(FilterName.JOBTYPE),
            Keyword: getValueFromSelectedFilter(FilterName.KEYWORD),
            PageIndex: $("input[name='" + FilterName.PAGEINDEX + "']").val(),
            SelectedPostId: $("input[name='" + FilterName.CURRENTPOSTID + "']").val()
        },
        dataType: "json",
        beforeSend: function () {
            $("#progress-bar").addClass("progress-bar-show");
        },
        success: function (data) {
            $("#progress-bar").removeClass("progress-bar-show");
            addJobAds(data);
        },
        error: function () {
            //add redirect to error page?
            $("#progress-bar").removeClass("progress-bar-show");
        }
    });
}

function resetAdList(resetPageIndex) {
    $("input[name='" + FilterName.PAGEINDEX + "']").val(resetPageIndex);
    $("input[name='" + FilterName.CURRENTPOSTID + "']").val("");
    $("div#ad-detail-page").empty();
    $("#ad-list").empty();
    $("#pager-cntr").empty();
}

function addJobAds(data) {
    if (data.PagedPosts.length == 0)
        showNoResultPage();
    else {
        for (var i = 0; i < data.PagedPosts.length; i++) {
            addAdListItem(data.PagedPosts[i], i);
        }
        addPager(data.Pager);
        removeNoresultPage();
    }
}

function showNoResultPage() {
    $("#ad-list-no-result").addClass("show");
    $("div#pager-cntr").addClass("hide");
}

function removeNoresultPage() {
    $("#ad-list-no-result").removeClass("show");
    $("div#pager-cntr").removeClass("hide");
}

function getSalaryTypeFromSelectedFilter() {
    var salaryFilter = $(FilterMarkup.SELECTEDFILTERS).children("span[name='" + FilterName.SALARY + "']");
    if (salaryFilter.length != 0)
        return salaryFilter.attr("data-salary-type");
    return "";
}

function getValueFromSelectedFilter(filterName) {
    return $(FilterMarkup.SELECTEDFILTERS).children("span[name='" + filterName + "']").attr("value");
}




//================ADD PAGER====================================================//



function addPager(pager) {
    var pagerCntr = $("div#pager-cntr");
    var pagerList = $("<ul class='pager__list fx'></ul>");
    var firstDirectLink = getPagerDirectLink(pager.CurrentPage, 1, 1, "<i class='ion-chevron-left c-mg-r-5'></i>First");
    firstDirectLink.appendTo(pagerList);
    
    for (var page = pager.StartPage; page <= pager.EndPage; page++) {
        var numberLink = getPagerNumberLink(page, pager.CurrentPage);
        numberLink.appendTo(pagerList);
    }

    var lastDirectLink = getPagerDirectLink(pager.CurrentPage, pager.TotalPages, pager.TotalPages, "Last<i class='ion-chevron-right c-mg-l-5'></i>");
    lastDirectLink.appendTo(pagerList);

    pagerList.appendTo(pagerCntr);
}

function getPagerNumberLink(page, currentPage) {
    var isSameNumber = page == currentPage;
    var numberLinkClass = isSameNumber ? "pager__number-inactive" : "pager__number";
    var eventFunction = isSameNumber ? "" : "updatePage(this.value)";
    var numberLink = $("<li class='" + numberLinkClass + "' onclick='"
            + eventFunction + "' value='" + page + "'>" + page + "</li>");
    return numberLink;
}

function getPagerDirectLink(currentPage, comparePageNumber, pageValue, directLinkInnerTag) {
    var isSameNumber = currentPage == comparePageNumber;
    var directLinkClass = isSameNumber ? "pager__direct-link-inactive" : "pager__direct-link";
    var eventFunction = isSameNumber ? "" : "updatePage(this.value)";
    var directLink = $("<li class='" + directLinkClass + "' onclick='" + eventFunction
        + "' value='" + pageValue + "'>" + directLinkInnerTag + "</li>");

    return directLink;
}

function updatePage(value) {
    $("input[name='" + FilterName.PAGEINDEX + "']").val(value);
    resetAdList(value);
    updateUrl();
    addAdList();
    moveToAdListPageTop();
}




//==================UPDATE URL SET=======================================//

function updateUrl() {
    var baseUrl = [location.protocol, '//', location.host, location.pathname].join('');
    var newFullQueryString = getFullNewQueryString();
    var newUrl = baseUrl + newFullQueryString;
    history.replaceState(null, null, newUrl);
}

function getFullNewQueryString() {
    var newQueryString = "";
    var selectedFilters = $(FilterMarkup.SELECTEDFILTERS).children(FilterMarkup.SELECTEDFILTER);
    if (selectedFilters.length != 0) {
        selectedFilters.each(function (index) {
            var isFirstQuery = index == 0 ? true : false;
            newQueryString += getQueryStringFromFilter($(this), isFirstQuery);
        });
    }
    newQueryString += getNonFilterQueryString(FilterName.PAGEINDEX, newQueryString);
    newQueryString += getNonFilterQueryString(FilterName.CURRENTPOSTID, newQueryString);
    return newQueryString;
}

function getNonFilterQueryString(filterName, newQueryString) {
    var filterTag = $("input[name='" + filterName + "']");
    var selectedValue = filterTag.val();
    var queryString = "";

    if (filterName === FilterName.PAGEINDEX)
        queryString = getPageIndexQueryString(newQueryString, filterName ,selectedValue);
    else
        queryString = getCurrentPostIdQueryString(newQueryString, filterName ,selectedValue);

    return queryString;
}

function getPageIndexQueryString(newQueryString, field ,selectedValue) {
    var queryString = "";
    if (selectedValue > DefaultPageIndex) {
        var isFirstQuery = newQueryString.length == 0 ? true : false;
        queryString = getQueryString(field, selectedValue, isFirstQuery);
    }
    return queryString;
}

function getCurrentPostIdQueryString(newQueryString, field ,selectedValue) {
    var queryString = "";
    if (selectedValue.length) {
        var isFirstQuery = newQueryString.length == 0 ? true : false;
        queryString = getQueryString(field, selectedValue, isFirstQuery);
    }
    return queryString;
}

function getQueryStringFromFilter(selectedFilter, isFirstQuery) {
    var field = selectedFilter.attr("name");
    var value = selectedFilter.attr("value");
    var newQueryString = getQueryString(field, value, isFirstQuery);

    //salary specific
    if (field === FilterName.SALARY) {
        newQueryString += "&" + "_" + FilterName.SALARYTYPE + "=" + encodeURIComponent(selectedFilter.attr("data-salary-type"));
    }
    return newQueryString;
}

function getQueryString(field, value, isFirstQuery) {
    var querySeparator = isFirstQuery ? "?" : "&";
    var newQueryString = querySeparator + "_" + field + "=" + encodeURIComponent(value);
    return newQueryString;
}

//=======================UPDATE AND REMOVE SELECTED FILTER SET=================================



function removeSameFilterAs(filterName) {
    var selectedFilter = $(FilterMarkup.SELECTEDFILTERS).children("span[name='" + filterName + "']");
    if (selectedFilter.length !== 0)
        selectedFilter.remove();
}

function addSelectedFilter(filterValue, filterName, filterText, filterType) {
    var newFilter = $("<span class='selected-filter' name = '"
        + filterName + "' value = '" + filterValue + "' data-filter-type = '" + filterType + "'><span class='selected-filter-text'>"
        + filterText + "</span><i class='ion-android-cancel c-mg-l-5'></i></span>");
    
    //Salary specific 
    if (filterName == FilterName.SALARY)
        setSalaryFilterWithSalaryType(newFilter);

    $(FilterMarkup.SELECTEDFILTERS).append(newFilter);
}

function replaceSelectedFilterWith(filterValue, filterName ,filterText, oldFilter) {
    oldFilter.attr("value", filterValue);
    oldFilter.children(".selected-filter-text").text(filterText);

    //Salary specific
    if (filterName == FilterName.SALARY)
        setSalaryFilterWithSalaryType(oldFilter);
}

function updateSelectedFilter(filterValue, filterName, filterText, filterType) {
    var oldFilter = $("span[name='" + filterName + "']");
    if (!oldFilter.length)
        addSelectedFilter(filterValue, filterName, filterText, filterType);
    else
        replaceSelectedFilterWith(filterValue, filterName, filterText, oldFilter);
}

function setSalaryFilterWithSalaryType(salaryFilter) {
    var salaryType = $("input[name='" + FilterName.SALARYTYPE + "']:checked").val();
    salaryFilter.attr("data-salary-type", salaryType);
}


//==============SET FILTERS FROM SETS============================================================//

function setFilterFromSalaryDropdownList() {
    var salaryTypeId = $("input[name='" + FilterName.SALARYTYPE + "']:checked").attr("id");
    var elementMarkup = "select#" + salaryTypeId + " option:selected";
    addFilterFrom(elementMarkup, FilterName.SALARY, FilterType.DROPDOWN);
}

function setFilterFromRadioButton(radioName) {
    var elementMarkup = "input:radio[name='" + radioName + "']:checked"
    addFilterFrom(elementMarkup, radioName, FilterType.RADIO);
}

function setFilterFromDropdownList(dropdownName) {
    var elementMarkup = "select[name='" + dropdownName + "'] option:selected";
    addFilterFrom(elementMarkup, dropdownName, FilterType.DROPDOWN);
}

function setFilterFromInput(inputName) {
    var elementMarkup = "input[name='" + inputName + "']";
    addFilterFrom(elementMarkup, inputName, FilterType.TEXT);
}

function addFilterFrom(elementMarkup, filterName, filterType) {
    var selectedValue = $(elementMarkup).val();
    var filterText = selectedValue;

    //salart specific
    if (filterName == FilterName.SALARY)
        filterText = "$" + filterText + " " + $("input[name='" + FilterName.SALARYTYPE + "']:checked").val();

    if (selectedValue.length != 0)
        updateSelectedFilter(selectedValue, filterName, filterText, filterType);
}


//====================ENABLE FILTER CHANGE & CLICK EVENT SETS=================================================//

function enableSalaryDropdownChangeEvent(dropdownId) {
    $("select#" + dropdownId).on("change", function () {
        var filterName = $(this).attr("name");
        var selectedOption = $("select#" + $(this).attr("id") + " option:selected");
        var filterText = selectedOption.text() + " " + $("input[name='" + FilterName.SALARYTYPE + "']:checked").val();
        resetAnotherSalaryFilter($(this).attr("id"));
        updateOnFilterChange(selectedOption.val(), filterName, filterText, FilterType.DROPDOWN);
    });
}

function enableDropdownChangeEvent(dropdownName) {
    $("select[name='" + dropdownName + "']").on("change", function () {
        var filterName = $(this).attr("name");
        var selectedValue = $("select[name='" + filterName + "'] option:selected").val();
        var filterText = $("select[name='" + filterName + "'] option:selected").text();
        updateOnFilterChange(selectedValue, filterName, filterText, FilterType.DROPDOWN);
    });
}

function enableRadioBtnChangeEvent(radioName) {
    $("input:radio[name='" + radioName + "']").on("change", function () {
        var selectedValue = $(this).attr("value");
        var filterName = $(this).attr("name");
        updateOnFilterChange(selectedValue, filterName, selectedValue, FilterType.RADIO);
    });
}

function enableTextInputFilterBtnClickEvent(textInputbtnName) {
    $("button[name='" + textInputbtnName + "']").on("click", function () {
        var filterName = $(this).attr("name");
        var enteredValue = $("input[name='" + filterName + "']").val();
        updateOnFilterChange(enteredValue, filterName, enteredValue, FilterType.TEXT);
    });
}

function updateOnFilterChange(selectedValue, filterName, filterText, filterType) {
    if (!selectedValue.length) {
        removeSameFilterAs(filterName);
        if (filterName == FilterName.SALARY)
            resetFilter(filterName, filterType);
    }
    else
        updateSelectedFilter(selectedValue, filterName, filterText, filterType);

    resetAdList(DefaultPageIndex);
    moveToAdListPageTop();
    updateUrl();
    addAdList();
}

function resetAnotherSalaryFilter(salaryDropdownId) {
    salaryDropdownId = salaryDropdownId == SalaryDropdownId.HOURLYDROPDONW ?
        SalaryDropdownId.WEEKLYDROPDOWN : SalaryDropdownId.HOURLYDROPDONW;
    $("select#" + salaryDropdownId + " option[value='']").prop("selected", true);

}






//============ADD AD LISTS=========================================================//


function addAdListItem(ad, index) {
    var adList = $("#ad-list");
    getAdListItem(ad).appendTo(adList);
}

function getAdListItem(ad) {
    var adListItemTag = $("<li></li>");
    var adAnchorTagClass = ad.Selected ? "ad-list__ad-link post-selected" : "ad-list__ad-link";
    var adAnchorTag = $("<div class='" + adAnchorTagClass + "' data-post-param='" + ad.PostId + "'></div>");
    getAdBody(ad).appendTo(adAnchorTag);
    getAdFooter(ad).appendTo(adAnchorTag);
    adAnchorTag.appendTo(adListItemTag);
    return adListItemTag;
}

function getAdBody(ad) {
    var adBody = $("<div class='ad-list__ad-body fx-spc-btw'></div>");
    getAdBodyLeft(ad).appendTo(adBody);
    getAdBodyRight(ad).appendTo(adBody);
    return adBody;
}

function getAdBodyLeft(ad) {
    var adBodyLeft = $("<div class='ad-list__ad-body-left'></div>");
    $("<p class='ad-list__ad-title text-over-ellipsis'>" + ad.Title + "</p>").appendTo(adBodyLeft);
    $("<p class='ad-list__ad-desc text-over-ellipsis'>" + ad.Description + "</p>").appendTo(adBodyLeft);
    return adBodyLeft;
}

function getAdBodyRight(ad) {
    var adBodyRight = $("<div class='ad-list__ad-body-right'></div>");
    $("<p class='ad-list__ad-place'>" + ad.Place + "</p>").appendTo(adBodyRight);
    $("<p class='ad-list__ad-date'>" + ad.DateCreated + "</p>").appendTo(adBodyRight);
    return adBodyRight;
}

function getAdFooter(ad) {
    var adFooter = $("<div class='ad-list__ad-footer'></div>");

    if (ad.Salary != null)
        getAdMetadata(ad.Salary).appendTo(adFooter);

    if (ad.JobDay != null)
        getAdMetadata(ad.JobDay).appendTo(adFooter);

    if (ad.ContractType != null)
        getAdMetadata(ad.ContractType).appendTo(adFooter);

    if (ad.JobType != null)
        getAdMetadata(ad.JobType).appendTo(adFooter);

    return adFooter;
}

function getAdMetadata(metadata) {
    var adMetadata = $("<span class='ad-list__ad-metadata'>" + metadata + "</span>");
    return adMetadata;
}



//==============ENABLE SELECTED FILTER CLICK EVENT SET====================================//



function enableSelectedFilterClickEvent() {
    $(FilterMarkup.SELECTEDFILTERS).on("click", FilterMarkup.SELECTEDFILTER, function () {
        resetFilter($(this).attr("name"), $(this).attr("data-filter-type"));
        $(this).remove();
        resetAdList(DefaultPageIndex);
        moveToAdListPageTop();
        updateUrl();
        addAdList();
    });
}

function resetFilter(filterName, filterType) {
    if (filterType === FilterType.RADIO)
        $("input[name='" + filterName + "']").filter("[value = '']").prop("checked", true);
    else if (filterType === FilterType.DROPDOWN)
        $("select[name='" + filterName + "'] option[value='']").prop("selected", true);
    else
        $("input[name='" + filterName + "']").val("");
}


//=================SALARY TYPE SPECIFIC FUNCTIONS ===============================//

function enableSalaryTypeChangeEvent() {
    $("input[name='" + FilterName.SALARYTYPE + "']").on("change", function () {
        var selectedSalaryTypeId = $(this).attr("id");
        setSelectBoxWithOpenSalaryTypeClass(selectedSalaryTypeId);
    });
}

function setSalaryTypeOpenClass() {
    var selectedSalaryTypeId = $("input[name='" + FilterName.SALARYTYPE + "']:checked").attr("id");
    setSelectBoxWithOpenSalaryTypeClass(selectedSalaryTypeId);
}

function setSelectBoxWithOpenSalaryTypeClass(selectedSalaryTypeId) {
    var salarySelectBoxWraper = $(".filter__salary-select-box-wraper");
    if (selectedSalaryTypeId == SalaryDropdownId.HOURLYDROPDONW) {
        salarySelectBoxWraper.removeClass("weekly-dropdown-open");
        salarySelectBoxWraper.addClass("hourly-dropdown-open");
    } else {
        salarySelectBoxWraper.removeClass("hourly-dropdown-open");
        salarySelectBoxWraper.addClass("weekly-dropdown-open");
    }
}



//function enableFilteringSalaryInput() {
//    $("input[name='Salary']").keydown(function (e) {
//        if (!((e.keyCode > 95 && e.keyCode < 106)
//            || (e.keyCode > 47 && e.keyCode < 58)
//            || e.keyCode == 8)) {
//            return false;
//        }
//    });
//}


//============ENABLE WINDOW SCROLL EVENT=================================================// 

//function enableWindowScrollEvent() {
//    $(window).scroll(function () {
//        if ($(window).scrollTop() == ($(document).height() - $(window).height())) {
//            getData();
//        }
//    });
//}


//function addToPageIndex() {
//    var pageIndexInput = $("input[name='" + FilterName.PAGEINDEX + "']");
//    var pageIndex = pageIndexInput.val();
//    pageIndex++;
//    pageIndexInput.val(pageIndex);
//}