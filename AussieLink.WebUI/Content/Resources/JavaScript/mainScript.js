$(document).ready(function () {
    enableMobileMenuBar();
    enableMobileNavBar();
});

function enableMobileMenuBar() {
    $("#mobile-menu-btn").on("click", function () {
        $(".menu-bar__list").toggleClass("mobile-menu-show");
    });
}

function enableMobileNavBar() {
    $(".mobile-nav-btn").on("click", function () {
        $(".mask").toggleClass("mask-open");
        $(".mb-nav").toggleClass("mobile-nav-show");
        $("body").toggleClass("oflow-y-hidden");
    });
}
