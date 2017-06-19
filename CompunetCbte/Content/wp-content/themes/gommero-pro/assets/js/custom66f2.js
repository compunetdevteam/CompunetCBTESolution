
jQuery(document).ready(function (jQuery) {
    /**
     * Mean Menu JS
     */
    jQuery('#main_menu').meanmenu({
        meanScreenWidth: "767"
    });
    /**
     * Flex Slider
     */
    jQuery('.flexslider').flexslider();
    /**
     * Scroll to top
     */
    jQuery('.home_icon').click(function () {
        jQuery('html, body').animate({
            scrollTop: 0
        }, 'slow');
        return false;
    });
    /**
     * Header Fixed
     */
    jQuery(window).on("scroll", function (e) {
        if (jQuery('.header-wrap').width() > 1170) {
            var outerHeaderTopPosition = jQuery('.header-wrap').offset().top;
            if (jQuery(window).scrollTop() > 0) {
                jQuery('.header-wrap').addClass('is-sticky');
            } else {
                jQuery('.header-wrap').removeClass('is-sticky');
            }
        }
    });
    if (jQuery('.wrapper.box-layout').width() == 1170) {
        jQuery('.main-content-wrapper').addClass('boxed25');
        jQuery('.gallery .main-content-wrapper').addClass('boxed30');
    }
    /**
     * Parallax Image Effect
     */
    $window = jQuery(window);
    jQuery('section[data-type="background"]').each(function () {
        var $bgobj = jQuery(this); // assigning the object                    
        jQuery(window).scroll(function () {
// Scroll the background at var speed
// the yPos is a negative value because we're scrolling it UP!								
            var yPos = -($window.scrollTop() / $bgobj.data('speed'));
            // Put together our final background position
            var coords = '50% ' + yPos + 'px';
            // Move the background
            $bgobj.css({backgroundPosition: coords});
        }); // window scroll Ends
    });
    /**
     * BX Slider For Testimonials
     */
    jQuery('.bxslider').bxSlider({
        mode: 'fade',
        slideMargin: 5,
        auto: true,
        autoControls: true
    });
    /**
     * Logo Image Size
     * @param {type} param
     */
    if (typeof image_size != "undefined") {
        var image_height = image_size.height;
        console.log(image_size.height / 2);
        jQuery('.header-wrapper').css({'padding-top': image_height / 2, 'padding-bottom': image_height / 2});
    }
//    $("p").css({"background-color": "yellow", "font-size": "200%"});
});
jQuery(window).resize(function () {
//    if (jQuery(window).width() < 768) {
//        jQuery('#main_menu > ul').removeClass('mega-menu');
//    } else {
//        jQuery('#main_menu > ul').addClass('mega-menu');
//    }
});
//Slider 
jQuery(function () {
    jQuery('#slides').slides({
        preload: true,
        effect: 'slide',
        generateNextPrev: true,
        preloadImage: 'img/loading.gif',
        play: 5000,
        pause: 2500,
        hoverPause: true
    });
});
//Zoombox   
jQuery(function () {
    jQuery('a.zoombox').zoombox();
});
function header_height() {
    var headerHeight = jQuery('.header-wrap').outerHeight();
    jQuery('.slider_wrapper, .breadcrumb-wrapper').css({'margin-top': headerHeight});
}

jQuery(document).ready(function () {
    header_height();
});
jQuery(window).resize(function () {
    header_height();
});