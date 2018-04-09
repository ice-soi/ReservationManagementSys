//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// Scroll
//,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
$(function () {
    var $section = $('.center');   // ｶﾚﾝﾀﾞｰｴﾘｱ
    var $pager = $('#js-pager');   // ﾍﾟｰｼﾞｬｰ枠

    // scrollify初期設定
    $.scrollify({
        section: ".center",
        scrollbars: false,
        before: function (index, section) {
            // ﾍﾟｰｼﾞｬｰに対応する順番にｸﾗｽ名を付与
            pagerCurrent(index); 
        },
        afterRender: function () {
            // ﾍﾟｰｼﾞｬｰの作成
            createPager(); 
        }
    });

    //'''''''''''''''''''''''''''''''''''''''''
    // name   : createPager
    // param  : none
    // remark : ﾍﾟｰｼﾞｬｰの新規作成
    //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    function createPager() {
        $section.each(function (i, e) {
            // ﾍﾟｰｼﾞ内ﾘﾝｸ先の作成
            var sectionName = $(e).attr('data-section-name');
            // 最初のliにはｸﾗｽを付与
            var addClass = '';
            if (i === 0) {
                addClass = 'is-current';
            }
            // liのHTML作成
            var html = '';
            html += '<li class="' + addClass + '">';
            html += '<a href="#' + sectionName + '">' + sectionName + '</a>';
            html += '</li>';
            $pager.append(html);
        });
        // ﾍﾟｰｼﾞｬｰのｸﾘｯｸｲﾍﾞﾝﾄ
        pagerLink();
    }

    //'''''''''''''''''''''''''''''''''''''''''
    // name   : pagerLink
    // param  : none
    // remark : ﾍﾟｰｼﾞｬｰのｸﾘｯｸｲﾍﾞﾝﾄ
    //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    function pagerLink() {
        $pager.find('a').on('click', function () {
            // ﾍﾟｰｼﾞｬｰｸﾘｯｸ時に該当のsectionにｽｸﾛｰﾙ
            $.scrollify.move($(this).attr("href"));
        });
    }
    
    //'''''''''''''''''''''''''''''''''''''''''
    // name   : pagerCurrent
    // param  : index ﾃﾞﾌｫﾙﾄ値は0
    // remark : 現在のﾍﾟｰｼﾞｬｰにｸﾗｽを設定
    //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    function pagerCurrent(index = 0) {
        var $li = $pager.find('li');
        $li.removeClass('is-current');
        $li.eq(index).addClass('is-current');
    }

    // 先頭のﾍﾟｰｼﾞｬｰをｸﾘｯｸ
    $('.is-current').children().eq(0).click();
});