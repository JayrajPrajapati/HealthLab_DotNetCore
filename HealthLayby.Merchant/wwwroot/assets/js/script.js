$('.sidemenuinner .sublist .sidebarlink').click(function(){
    if($(this).parent().hasClass('open')){
        $(this).parent().removeClass('open');
        $(this).parent().find('.submenu').slideUp();
    }else{
        $('.sidemenuinner .sublist .menuarrow').parent().find('.submenu').slideUp();
        $('.sidemenuinner .sublist .menuarrow').parent().removeClass('open');
        $(this).parent().addClass('open');
        $(this).parent().find('.submenu').slideDown();
    }
});