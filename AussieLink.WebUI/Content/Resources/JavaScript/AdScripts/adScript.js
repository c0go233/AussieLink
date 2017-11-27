$(document).ready(function () {
    enableMbFilterBtn();
});

function moveTo(value) {
    console.log(value);
    $(window).scrollTop(value);
}

function moveToAdListPageTop() {
    var offset = $("#ad-list-page").offset();
    $(window).scrollTop(offset.top);
}

function enableMbFilterBtn() {
    $("#mb-filter-btn").on("click", function () {
        $(".c-mc__cntr").toggleClass("mb-filter-open");
        $(".c-mc").toggleClass("oflow-x-hidden");
    });
}
