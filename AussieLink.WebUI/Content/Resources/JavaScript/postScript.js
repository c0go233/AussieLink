var SuburbType = { SYDNEY: 1, MELBOURNE: 2, BRISBANE: 3, ADELAIDE: 4, PERTH: 5 }
var Tag = { SELECTSUBURB: "select#Suburb", SELECTPLACE: "select#PlaceId"}

var PICTURENUMBERLIMIT = 5;
var PICTURESIZELIMITINMB = 2;
var IMGFILEONLYERRORTEXT = "Only image file is accepted";
var REACHEDLIMITNUMBER = "5 image files are allowed";
var REACHEDLIMITSIZE = "Up to 2MB is allowed";

var DEFAULTPICTUREITEMID = "picture-1";
var PICTUREITEMSTATUS = { SAVED: "saved", UNSAVED: "unsaved"};
var FORMINPUTNUMBER = 9;
var unsavedPictures = [];
var savedPictures = storedPictures;

$(document).ready(function () {

    //----------SHARE POST SCRIPTS---------------------------//
    setSuburbDropdown($("select#PlaceId option:selected").val());
    setSuburb();
    enablePlaceDropdownChangeEvent();

    setDatePicker();
    enableDatePickerInputChangeEvent();

    enableDropZoneDragEvent();
    enablePicFileInputChangeEvent();
    enablePictureItemDeleteBtnDelegate();
    setSavedPictures();

    enableSubmitBtnClickEvent();
});

function setSavedPictures() {
    if (savedPictures != null && savedPictures.length)
        addSavedPictures();
}

function addSavedPictures() {
    for (var i = 0; i < savedPictures.length; i++) {
        var picture = savedPictures[i];
        addPictureElement(picture.PictureId, PICTUREITEMSTATUS.SAVED,
            picture.ImageSrc, picture.PictureSize, picture.PictureName);
    }
}

function setSuburb() {
    var selectedSuburb = $("input[name='Suburb']").val();
    if (selectedSuburb.length) {
        var selectedOption = $("select[name='Suburb']").children("option[value='" + selectedSuburb + "']");
        selectedOption.attr("selected", "selected");
    }
}

function enablePictureItemDeleteBtnDelegate() {
    $("div#c-drop-zone__pic-cntr").on("click", "button.c-delete-btn", function (e) {
        e.preventDefault();
        var pictureId = $(e.target).attr("data-pic-id");
        var pictureStatus = $(e.target).attr("data-pic-status");
        deletePictureElement($(e.target));
        tryDeleteItemExistClassFromDropZone();
        tryDeletePictureItem(pictureId, pictureStatus);
        hideImageInsertionError();
    });
}

function tryDeleteItemExistClassFromDropZone() {
    var pictureElementContainer = $("div#c-drop-zone__pic-cntr");
    var pictureElements = pictureElementContainer.children("div.sh-post__pic-input-wraper");
    if (pictureElements.length === 0)
        $("div#drop-zone").removeClass("c-drop-zone__item-exist");
}

function deletePictureElement(deleteBtn) {
    var pictureElementWraper = deleteBtn.parents("div.sh-post__pic-input-wraper");
    pictureElementWraper.remove();
}

function tryDeletePictureItem(pictureItemId, pictureItemStatus) {
    if (pictureItemStatus === PICTUREITEMSTATUS.UNSAVED)
        deletePictureItemById(unsavedPictures, pictureItemId);
    else
        deletePictureItemById(savedPictures, pictureItemId);
}

function deletePictureItemById(pictureList, pictureItemId) {
    for (var i = 0; i < pictureList.length; i++) {
        var pictureItem = pictureList[i];
        if (pictureItem.PictureId === pictureItemId) {
            pictureList.splice(i, 1);
            return;
        }
    }
}

function enablePicFileInputChangeEvent() {
    $("input#pic-file-input").on("change", function (e) {
        tryAddPictures(e.target.files);
        resetFileInput($(e.target));
    });
}

function resetFileInput(inputElement) {
    inputElement.wrap("<p></p>");
    inputElement.wrap("<form id='reset-form'></form>");
    inputElement.parent("form#reset-form").get(0).reset();
    inputElement.unwrap("<form class='sh-post__temp-form'></form>");
    inputElement.unwrap("<p></p>");
}

function enableDropZoneDragEvent() {
    enableDragOverEvent();
    enableDragLeaveEvent();
    enableDropEvent();
}

function enableDropEvent() {
    var dropzone = document.getElementById("drop-zone");
    dropzone.ondrop = function (e) {
        e.preventDefault();
        $(this).removeClass("c-drag-over");
        tryAddPictures(e.dataTransfer.files);
    };
}

function enableDragOverEvent() {
    var dropzone = document.getElementById("drop-zone");
    dropzone.ondragover = function () {
        $(this).addClass("c-drag-over");
        return false;
    };
}

function enableDragLeaveEvent() {
    var dropzone = document.getElementById("drop-zone");
    dropzone.ondragleave = function () {
        $(this).removeClass("c-drag-over")
        return false;
    };
}

function tryAddPictures(files) {
    if (filesHaveWrongType(files))
        showImageInsertionError(IMGFILEONLYERRORTEXT);
    else if (hasNumberOfPictureReachedTo(PICTURENUMBERLIMIT, files.length))
        showImageInsertionError(REACHEDLIMITNUMBER);
    else if (hasSizeOfPictureReachedTo(PICTURESIZELIMITINMB, files))
        showImageInsertionError(REACHEDLIMITSIZE);
    else {
        hideImageInsertionError();
        for (var i = 0; i < files.length; i++) {
            addPictureFrom(files[i]);
        }
    }
}

function filesHaveWrongType(filesToAdd) {
    for(var i = 0;  i < filesToAdd.length; i++) {
        var picture = filesToAdd[i];
        if (!picture.type.match("image/*"))
            return true;
    }
    return false;
}

function hasSizeOfPictureReachedTo(sizeLimit, filesToAdd) {
    var totalSize = getTotalSizeFromSavedPictures() + getTotalSizeFromUnsavedPictures() + getTotalSizeFromFilesToAdd(filesToAdd);
    var sizeImMb = convertToMB(totalSize);
    return sizeImMb > PICTURESIZELIMITINMB;
}

function convertToMB(size) {
    var sizeInMB = size / (1024 * 1024);
    return sizeInMB;
}

function getTotalSizeFromFilesToAdd(filesToAdd) {
    var totalSize = 0;
    for (var i = 0; i < filesToAdd.length; i++) {
        totalSize += filesToAdd[i].size;
    }
    return totalSize;
}

function getTotalSizeFromSavedPictures() {
    var totalSize = 0;
    if (savedPictures != null) {
        for (var i = 0; i < savedPictures.length; i++) {
            totalSize += savedPictures[i].PictureSize;
        }
    }
    return totalSize;
}

function getTotalSizeFromUnsavedPictures() {
    var totalSize = 0;
    if (unsavedPictures != null) {
        for (var i = 0; i < unsavedPictures.length; i++) {
            totalSize += unsavedPictures[i].file.size;
        }
    }
    return totalSize;
}

function hideImageInsertionError() {
    $("span#sh-post-input-file-val-error").text("");
}

function showImageInsertionError(errorMsg) {
    $("span#sh-post-input-file-val-error").text(errorMsg)
}

function hasNumberOfPictureReachedTo(numberLimit, filesToAddLength) {
    var count = filesToAddLength;
    if (unsavedPictures != null)
        count += unsavedPictures.length;
    if (savedPictures != null)
        count += savedPictures.length;

    return count > numberLimit
}

function addPictureFrom(file) {
    var pictureReader = new FileReader();
    pictureReader.onload = (function (pFile) {
        return function (e) {
            var pictureItemId = addUnsavedPictureItem(pFile);
            addPictureElement(pictureItemId, PICTUREITEMSTATUS.UNSAVED, e.target.result, e.loaded, pFile.name);
        }
    })(file)
    pictureReader.readAsDataURL(file);
}

function addPictureElement(pictureItemId, pictureItemStatus, imgSrc, fileSize, fileName) {
    var pictureElementContainer = $("div.c-drop-zone__pic-cntr");
    var pictureElementWraper = $("<div class='sh-post__pic-input-wraper fx-spc-btw fx-algn-center' aria-selected='false' ></div>");
    getImageWraperForPictureElement(imgSrc, fileName).appendTo(pictureElementWraper);
    getBtnWraperForPictureElement(fileSize, pictureItemId, pictureItemStatus).appendTo(pictureElementWraper);
    pictureElementWraper.appendTo(pictureElementContainer);

    //set dropdown zone class to item-exist
    $("div#drop-zone").addClass("c-drop-zone__item-exist");
}

function getImageWraperForPictureElement(imgSrc, fileName) {
    var imgWraper = $("<div class='fx fx-algn-center sh-post__img-wraper'></div>");
    $("<img src='" + imgSrc + "' class='sh-post__pic' />").appendTo(imgWraper);
    $("<span class='c-mg-l-5 text-over-ellipsis'>" + fileName + "</span>").appendTo(imgWraper);
    return imgWraper;
}

function getBtnWraperForPictureElement(fileSize, pictureItemId, pictureItemStatus) {
    var btnWraper = $("<div class='sh-post__pic-btn-wraper fx fx-algn-center'></div>");
    var sizeInKb = convertToKbInSize(fileSize);
    $("<span class='c-mg-r-5 sh-post__size'>" + sizeInKb + "KB" + "</span>").prependTo(btnWraper);
    $("<button class='c-delete-btn sh-post__pic-delete-btn' data-pic-id='"
        + pictureItemId + "' data-pic-status='" + pictureItemStatus + "'>Delete</button>").appendTo(btnWraper);
    return btnWraper;
}

function addUnsavedPictureItem(file) {
    var pictureItemId = getPictureItemId();
    var pictureItem = getPictureItem(pictureItemId, file);
    unsavedPictures.push(pictureItem);
    return pictureItemId;
}

function convertToKbInSize(fileSize) {
    var sizeInKb = fileSize / 1024;
    sizeInKb = (Math.round(sizeInKb * 100) / 100);
    sizeInKb = Math.floor(sizeInKb);
    return sizeInKb;
}

function getPictureItem(pictureItemId, file) {
    var pictureItem = { PictureId: pictureItemId, file: file };
    return pictureItem;
}

function getPictureItemId() {
    var pictureItemId = DEFAULTPICTUREITEMID;
    if (unsavedPictures.length !== 0) {
        var lastPictureItem = unsavedPictures[(unsavedPictures.length - 1)];
        var lastPictureItemId = lastPictureItem.PictureId;
        var pictureItemId = generateNextPictureItemId(lastPictureItemId);
    }
    return pictureItemId;
}

function generateNextPictureItemId(currentPictureItemid) {
    var currentNumber = currentPictureItemid.substr((currentPictureItemid.length - 1));
    var currentNumberInInt = parseInt(currentNumber);
    var nextNumber = currentNumberInInt + 1;
    var prefix = currentPictureItemid.substr(0, (currentPictureItemid.length - 1));
    var nextPictureItemId = prefix + nextNumber;
    return nextPictureItemId;
}

function enableSubmitBtnClickEvent() {
    $("button#post-sbmt-btn").on("click", function (e) {
        e.preventDefault();
        if ($("form").valid()) {
            $.ajax({
                type: "POST",
                url: "/Post/SharePost",
                processData: false,
                contentType: false,
                headers: getAjaxHeaderWithToken(),
                dataType: "json",
                data: getDataToPost(),
                success: function (data) {

                },
                error: function (errorList) {
                    addErrors(errorList);
                }
            });
        }
    });
}

function getDataToPost() {
    var dataToPost = new FormData();
    setFormInputDataToPost(dataToPost);
    setPicturesToPost(dataToPost);
    setSavedPicturesToPost(dataToPost);
    return dataToPost;
}

function setFormInputDataToPost(dataToPost) {
    var formInputs = $("form#post-form").serialize();
    var ampersandSplitInputs = formInputs.split("&");
    for (var i = 0; i < FORMINPUTNUMBER; i++) {
        var equalSplitInput = ampersandSplitInputs[i].split("=");
        dataToPost.append("vm." + equalSplitInput[0], equalSplitInput[1]);
    }
}

function setPicturesToPost(dataToPost) {
    if (unsavedPictures != null) {
        for (var i = 0; i < unsavedPictures.length; i++) {
            dataToPost.append("unsavedPictures[" + i + "]", unsavedPictures[i].file);
        }
    }
}

function setSavedPicturesToPost(dataToPost) {
    if (savedPictures != null) {
        for (var i = 0; i < savedPictures.length; i++) {
            dataToPost.append("vm.SavedPictures[" + i + "].PictureId", savedPictures[i].PictureId);
        }
    }
}

function getAjaxHeaderWithToken() {
    var token = $('input[name=__RequestVerificationToken]').val();
    var headers = {};
    headers["__RequestVerificationToken"] = token;
    return headers;
}

function addErrors(errorList) {
    for (var i = 0; i < errorList.responseJSON.length; i++)
    {
        var error = errorList.responseJSON[i];
        var key = error.Key;
        var errorMsg = error.ErrorMsg;
        var errorElement = $("form").find("[data-valmsg-for='" + key + "']");
        errorElement.addClass("field-validation-error");
        errorElement.text(errorMsg);
        var element = $("form").find("#" + key);
        element.addClass("input-validation-error");

    }
}


























//-------------ENABLE PLACE & SUBURB DROPDOWN SETS --------------------//

function enablePlaceDropdownChangeEvent() {
    $(Tag.SELECTPLACE).on("change", function () {
        //change the value of plae dorpdown to name
        var selectedPlace = $(this).children("option:selected").val();
        setSuburbDropdown(selectedPlace);
    });
}

function setSuburbDropdown(selectedPlace) {
    if (!selectedPlace.length)
        resetSuburbDropdown();
    else
        updateSuburbDropdown(selectedPlace);
}

function resetSuburbDropdown() {
    $(Tag.SELECTSUBURB).empty();
    $(Tag.SELECTSUBURB).addClass("c-inactive-select");
    $("<option value=''>Select place first</option>").appendTo($(Tag.SELECTSUBURB));
}

function updateSuburbDropdown(selectedPlace) {
    var suburbList = getSelectedSuburbList(selectedPlace);
    $(Tag.SELECTSUBURB).empty();
    $(Tag.SELECTSUBURB).removeClass("c-inactive-select");
    addSuburbDropdownOptions(suburbList);
}

function getSelectedSuburbList(selectedPlace) {
    selectedPlace = parseInt(selectedPlace);
    if (selectedPlace === SuburbType.SYDNEY)
        return SydneySuburbs;
    else if (selectedPlace === SuburbType.MELBOURNE)
        return MelbourneSuburbs;
    else if (selectedPlace === SuburbType.BRISBANE)
        return BrisbaneSuburbs;
    else if (selectedPlace === SuburbType.ADELAIDE)
        return AdelaideSuburbs;
    else if (selectedPlace === SuburbType.PERTH)
        return PerthSuburbs;
}

function addSuburbDropdownOptions(suburbList) {
    var suburbDropdown = $(Tag.SELECTSUBURB);
    $("<option value=''>Select suburb</option>").appendTo(suburbDropdown);
    for (var i = 0; i < suburbList.length; i++) {
        var suburbName = suburbList[i].Name;
        $("<option value='" + suburbName + "'>" + suburbName + "</option>").appendTo(suburbDropdown);
    }
}


//-----------DATE PICKER-------------------------//

function enableDatePickerInputChangeEvent() {
    $("input#sh-date-picker").on("change", function () {
        $(this).valid();
    });
}

function setDatePicker() {
    $("#sh-date-picker").datepicker({
        minDate: 0,
        dateFormat: 'dd/mm/yy'
    });
}



























//$("div#sh-post__picture-input-wraper").on("change", "input[type=file]")

//$("input#picture").on("change", function (e) {
//    var files = e.target.files;
//    var filesArr = Array.prototype.slice.call(files);
//    filesArr.forEach(function (f) {
//        if (!f.type.match("image.*")) {
//            return;
//        }
//        storedFiles.push(f);
//    });
//});




//$("button#sh-post__sbm-btn").on("click", function (e) {
//    e.preventDefault();

//    var token = $('input[name=__RequestVerificationToken]').val();
//    var headers = {};
//    headers["__RequestVerificationToken"] = token;

//    var data = new FormData();

//    for (var i = 0, len = storedFiles.length; i < len; i++) {
//        data.append('files', storedFiles[i]);
//    }


//    var mo = { PlaceId: 1, Suburb: "fuck" };
//    var model = JSON.stringify({
//        PlaceId: 1, Suburb: "Fuck"
//    });

//    data.append('vm.placeId', "123");

//    $.ajax({
//        type: "POST",
//        url: "/Post/SharePost",
//        headers: headers,
//        data: data,
//        dataType: "html",
//        async: false,
//        processData: false,
//        contentType: false,
//        success: function (data) {
//            var d = data;

//        },
//        error: function (e) {
//            var responseHtml = e.responseText;
//            $("form#sh-post-form").valid();

//        }
//    });

//});

////=======================SHARE POST SCRIPTS ===============================================================================//

////-------------PICTURE UPLOADER---------------------//

//function enableFileInputChangeEventDelegate() {
//    $("div#sh-post-pic-box").on("change", "input[type='file']", function (e) {
//        var input = e.target;
//        if (input.files && input.files[0])
//            tryCreatePictureItem(input);
//    });
//}

//function tryCreatePictureItem(input) {
//    var errorMsg = { error: "" };
//    if (validatePictureItemToCreate(input, errorMsg))
//        readPictureItemToCreate(input);
//    else
//        displayInputFileErrorSet(errorMsg.error);
//}

//function validatePictureItemToCreate(input, errorMsg) {
//    var file = input.files[0];
//    if (!file.type.match("image.*")) {
//        errorMsg.error = IMGFILEONLYERRORTEXT;
//        resetFileInput($(input));
//        return false;
//    } else if (hasSizeOfPictureItemReachedTo(PICTURESIZELIMITINMB, file.size)) {
//        errorMsg.error = REACHEDLIMITSIZE;
//        resetFileInput($(input));
//        return false;
//    }
//    return true;
//}

//function hasSizeOfPictureItemReachedTo(sizeLimit, sizeToAdd) {
//    var inputWrapers = $("div#sh-post-pic-box").children("div.sh-post__pic-input-wraper[aria-selected='true']");
//    var currentSize = converToMbInSize(sizeToAdd);
//    inputWrapers.each(function () {
//        var input = $(this).find("input[type='file']");
//        currentSize += converToMbInSize(input[0].files[0].size);
//    });
//    return currentSize > sizeLimit;
//}

//function converToMbInSize(fileSize) {
//    var sizeInMb = fileSize / (1024 * 1024);
//    return sizeInMb;
//}


//function readPictureItemToCreate(input) {
//    var inputWraper = $(input).parents("div.sh-post__pic-input-wraper");
//    var btnWraper = inputWraper.find("div.sh-post__pic-btn-wraper");
//    var addBtn = inputWraper.find("label.sh-post__pic-add-btn");
//    var inputId = $(input).attr("id");
//    var fileName = input.files[0].name;

//    var imgDir = new FileReader();

//    imgDir.onload = (function (pInputWraper, pBtnWraper, pAddBtn, pInputId, pFileName) {
//        return function (e) {
//            addNewPictureItem(pInputWraper, pBtnWraper, pAddBtn, pInputId, e.target.result, e.loaded, pFileName);
//            if (!hasNumberOfPictureItemReachedTo(PICTURENUMBERLIMIT))
//                addFileInput(pInputId);

//            removeInputFileError();
//        }
//    })(inputWraper, btnWraper, addBtn, inputId, fileName);
//    imgDir.readAsDataURL(input.files[0]);
//}

//function addNewPictureItem(inputWraper, btnWraper, addBtn, inputId, imgSrc, loadedSize, fileName) {
//    inputWraper.attr("aria-selected", "true");
//    var imgWraper = $("<div class='fx fx-algn-center sh-post__img-wraper'></div>");
//    $("<img src='" + imgSrc + "' class='sh-post__pic' />").appendTo(imgWraper);
//    $("<span class='c-mg-l-5 text-over-ellipsis'>" + fileName + "</span>").appendTo(imgWraper);
//    imgWraper.prependTo(inputWraper);
//    var sizeInKb = convertToKbInSize(loadedSize);
//    $("<span class='c-mg-r-5 sh-post__size'>" + sizeInKb + "KB" + "</span>").prependTo(btnWraper);
//    $("<button class='c-delete-btn sh-post__pic-delete-btn'>Delete</button>").appendTo(btnWraper);
//    addBtn.addClass("hide");
//}

//function convertToKbInSize(fileSize) {
//    var sizeInKb = fileSize / 1024;
//    sizeInKb = (Math.round(sizeInKb * 100) / 100);
//    sizeInKb = Math.floor(sizeInKb);
//    return sizeInKb;
//}

//function addFileInput(currentInputId) {
//    var nextIndex = getNextFileInputIndex(currentInputId);
//    var nextFileInputId = getNextFileInputId(nextIndex, currentInputId);
//    var newFileInput = getNewFileInput(nextIndex, nextFileInputId);
//    newFileInput.appendTo("div#sh-post-pic-box");
//}

//function getNewFileInput(index, fileInputId) {
//    var inputWraper = $("<div class='sh-post__pic-input-wraper p-relative fx-spc-btw fx-algn-center' aria-selected='false' ></div>");
//    var btnWraper = $("<div class='sh-post__pic-btn-wraper fx fx-algn-center'></div>");
//    var label = $("<label class='c-btn c-btn-border sh-post__pic-add-btn c-mg-r-5' for='" + fileInputId + "'><i class='ion-upload c-mg-r-5 sh-post__add-icon'></i>Add Picture</label>");
//    var input = $("<input class='sh-post__file-input' type='file' name='Pictures[" + index + "]' id='" + fileInputId + "' />");

//    label.appendTo(btnWraper);
//    input.appendTo(btnWraper);
//    btnWraper.appendTo(inputWraper);

//    return inputWraper;
//}

//function getNextFileInputId(nextIndex, inputId) {
//    var prefix = inputId.substr(0, (inputId.length - 1));
//    return prefix + nextIndex;
//}

//function getNextFileInputIndex(inputId) {
//    var currentNumber = inputId.substr((inputId.length - 1));
//    var currentNumberInInt = parseInt(currentNumber);
//    var nextNumber = currentNumberInInt + 1;
//    return nextNumber;
//}

//function enableDeleteClickEvent() {
//    $("div#sh-post-pic-box").on("click", "button.sh-post__pic-delete-btn", function (e) {
//        e.preventDefault();
//        deletePictureFileInput($(e.target));
//        if (hasNumberOfPictureItemReachedTo((PICTURENUMBERLIMIT - 1))) {
//            var lastPictureFileInputId = getLastPictureFileInputId();
//            addFileInput(lastPictureFileInputId);
//        }
//        removeInputFileError();
//    });
//}

//function resetFileInput(inputElement) {
//    inputElement.wrap("<p></p>");
//    inputElement.wrap("<form id='reset-form'></form>");
//    inputElement.parent("form#reset-form").get(0).reset();
//    inputElement.unwrap("<form class='sh-post__temp-form'></form>");
//    inputElement.unwrap("<p></p>");
//}

//function getLastPictureFileInputId() {
//    var lastId = $("div#sh-post-pic-box")
//        .children("div.sh-post__pic-input-wraper[aria-selected='true']")
//        .last().find("input[type=file]").attr("id");
//    return lastId;
//}

//function deletePictureFileInput(deleteBtn) {
//    var inputWraper = deleteBtn.parents("div.sh-post__pic-input-wraper");
//    inputWraper.remove();
//}



//function displayInputFileErrorSet(errorMsg) {
//    removeInputFileError();
//    displayInputFileError(errorMsg);
//}

//function displayInputFileError(errorMsg) {
//    var errorMsgBox = $("span#sh-post-input-file-val-error");
//    $("<span id='sh-post-input-file-error-msg'>" + errorMsg + "</span>").appendTo(errorMsgBox);
//}

//function removeInputFileError() {
//    var errorMsg = $("span#sh-post-input-file-val-error").find("span#sh-post-input-file-error-msg");
//    errorMsg.remove();
//}




