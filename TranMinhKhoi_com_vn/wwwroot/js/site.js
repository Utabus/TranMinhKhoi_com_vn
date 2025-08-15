// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
jQuery(window).bind('scroll', function () {
    var wwd = jQuery(window).width();
    if (wwd > 939) {
        var navHeight = jQuery(window).height() - 600;
    }
});


jQuery(window).load(function () {
    jQuery('#slider').nivoSlider({
        effect: 'random', //sliceDown, sliceDownLeft, sliceUp, sliceUpLeft, sliceUpDown, sliceUpDownLeft, fold, fade, random, slideInRight, slideInLeft, boxRandom, boxRain, boxRainReverse, boxRainGrow, boxRainGrowReverse
        animSpeed: 500,
        pauseTime: 4000,
        directionNav: false,
        controlNav: true,
        pauseOnHover: false,
    });
});


jQuery(window).load(function () {
    jQuery('.owl-carousel').owlCarousel({
        loop: true,
        autoplay: true,
        autoplayTimeout: 8000,
        margin: 30,
        nav: true,
        autoHeight: false,
        navText: ["<i class='fa-solid fa-angle-left'></i>", "<i class='fa-solid fa-angle-right'></i>"],
        dots: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    })

});

jQuery(document).ready(function () {

    jQuery('.link').on('click', function (event) {
        var $this = jQuery(this);
        if ($this.hasClass('clicked')) {
            $this.removeAttr('style').removeClass('clicked');
        } else {
            $this.css('background', '#7fc242').addClass('clicked');
        }
    });

});