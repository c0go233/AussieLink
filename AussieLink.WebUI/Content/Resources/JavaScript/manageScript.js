$(document).ready(function() {
    enablePostTypeDropDownClickEvent();
});

function enablePostTypeDropDownClickEvent() {
    $("select#PostType").on("change", function () {
        var postType = $(this).children("option:selected").val();
        var localPath = window.location.pathname;
        if (!postType.length) {
            location.href = localPath;
        }
        else {
            location.href = localPath + "?postType=" + encodeURIComponent(postType);
        }
    });
}