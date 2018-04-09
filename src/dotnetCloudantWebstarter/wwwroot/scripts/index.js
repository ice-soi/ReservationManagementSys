// index.js

//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// Const
//,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    const SCH_DATA = 'api/schedule';  // ｽｹｼﾞｭｰﾙ取得用ｺﾝﾄﾛｰﾗ
    const AM = '0';                   // 午前
    const PM = '1';                   // 午後
    const REQUEST_PREV  = '/0_';      // 前月
    const REQUEST_NEXT  = '/1_';      // 次月 
    const REQUEST_MONTH = '0';        // 月
    const REQUEST_WEEK  = '1';        // 週
    const REQUEST_DAY   = '2';        // 日
    // ｴﾗｰﾒｯｾｰｼﾞ
    const TIME_VALIDATE_MESSAGE    = '・開始時間が終了時間より後になっています。';
    const TITLE_VALIDATE_MESSAGE   = '・タイトルが入力されていません。';
    const REMARKS_VALIDATE_MESSAGE = '・内容が入力されていません。';

//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// Function
//,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    
    //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    // Get
    //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        
        //'''''''''''''''''''''''''''''''''''''''''
        // name   : createCalender
        // param  : none
        // remark : ｶﾚﾝﾀﾞｰﾃﾞｰﾀを取得しHTMLに展開する
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        function createCalender() {
            // ﾛｰﾃﾞｨﾝｸﾞの表示
            dispLoading();
            // ｶﾚﾝﾀﾞｰﾃﾞｰﾀの取得ﾘｸｴｽﾄ
            xhrGet(SCH_DATA, function (calender){
                // ｶﾚﾝﾀﾞｰの初期化
                setScheduleCalender(calender);
                // 入力画面の初期化
                setScheduleInput();
                // 前月・次月のｲﾍﾞﾝﾄ設定
                getMoveCalender();
                // ﾛｰﾃﾞｨﾝｸﾞの非表示
                removeLoading();
            }, function (err) {
                console.error(err);
                // ﾛｰﾃﾞｨﾝｸﾞの非表示
                removeLoading();
            });    
            
        }
        //'''''''''''''''''''''''''''''''''''''''''
        // name   : setScheduleCalender
        // param  : none
        // remark : ｶﾚﾝﾀﾞｰの初期化
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        function setScheduleCalender(_cal){
            var _today, _weekstart, _weekend;
            // ｶﾚﾝﾀﾞｰ展開処理
            var setDaySchedule = function (_m, _callback) {
                // ｶﾚﾝﾀﾞｰに展開したｽｹｼﾞｭｰﾙｸﾘｯｸ時に予定入力画面にﾊﾟﾗﾒｰﾀを設定
                _m.Schedule.forEach(function (s) {
                    // ｺｰﾙﾊﾞｯｸ
                    _callback(s);
                    // ｽｹｼﾞｭｰﾙｸﾘｯｸ時の処理
                    $('.' + s.Id).on('click', function () {
                        $('.error').text('');             // ｴﾗｰｸﾘｱ
                        $('#data-id').val(s.Id);          // id
                        $('#data-rev').val(s.Rev);        // ReVision
                        $('#from-date').val(s.FromDate);  // 開始日
                        $('#from-time').val(s.FromTime);  // 開始時間
                        $('#to-date').val(s.ToDate);      // 終了日
                        $('#to-time').val(s.ToTime);      // 終了時間
                        $('#title').val(s.Title);         // ﾀｲﾄﾙ
                        $('#remarks').val(s.Remarks);     // 内容
                        // ｽｹｼﾞｭｰﾙ登録画面に遷移
                        $("#js-pager").children().children().eq(3).click();
                        // 登録ﾎﾞﾀﾝ:表示 更新ﾎﾞﾀﾝ:非表示 削除ﾎﾞﾀﾝ:非表示
                        $('.add').hide();
                        $('.update').show();
                        $('.delete').show();
                    });
                });
                // 日付ｸﾘｯｸ時の処理
                var ymd = _m.Year + _m.Month + _m.Day;
                $('.' + ymd).find('span').on('click', function () {
                    // ｽｹｼﾞｭｰﾙ入力画面初期化
                    setScheduleInput(new Date(_m.Year,_m.Month - 1, _m.Day));
                    // ｽｹｼﾞｭｰﾙ登録画面に遷移
                    $("#js-pager").children().children().eq(3).click();
                // ｶｰｿﾙを設定
                }).css('cursor','pointer');
            };

            // 週ｽｹｼﾞｭｰﾙ展開処理
            var setWeekSchedule = function (_m, _ymd) {
                if (_m.ThisWeek === '1') {
                    var dow = (_m.Dow === 'Saturday') ? 'sat' : (_m.Dow === 'Sunday') ? 'sun' : '';
                    // 週ｽｹｼﾞｭｰﾙ(end)設定
                    _weekend = _m;
                    // 週ｽｹｼﾞｭｰﾙ(start)設定
                    if (!_weekstart) {
                        _weekstart = _weekend;
                    }
                    // 曜日設定
                    $('.weeks').children().append('<li class="dow ' + dow + '"><span>' + _m.Dow + '</span></li>');
                    // 週ｶﾚﾝﾀﾞｰ設定
                    $('#thisweek').children().append('<li class="' + _ymd + ' scroll-area"><span>' + _m.Day + '</span></li>');
                }
            };

            // 当日ｽﾀｲﾙ設定
            var setToday = function (_ymd, _current) {
                var dt = new Date();
                var y = dt.getFullYear();                       // 年
                var m = ('0' + (dt.getMonth() + 1)).slice(-2);  // 月(00)
                var d = ('0' +dt.getDate()).slice(-2);          // 日(00)

                // 年月日が一致する日が当日としてｽﾀｲﾙを変更する
                if (_current.Year == y && _current.Month == m && _current.Day == d) {
                    $('.' + _ymd).find('span').addClass('currentDay');
                } 
            }

            // 前月のｽｹｼﾞｭｰﾙ設定
            _cal.BeforeMonth.forEach(function (before) {
                // 年月日の設定
                var ymd = before.Year + before.Month + before.Day;
                // 週ｽｹｼﾞｭｰﾙ設定
                setWeekSchedule(before, ymd);
                // 先月の基準日開始日以外は表示しない
                if (before.BeforeStartDay === "0") { return; }
                // ｶﾚﾝﾀﾞｰ表示
                $('#lastmonth').children().append('<li class="' + ymd + ' scroll-area"><span>' + before.Day + '</span></li>');
                // 日ｽｹｼﾞｭｰﾙ設定
                setDaySchedule(before, function (s) {
                    $('.' + ymd).append('<p class="' + s.Id + '">' + s.FromTime + '</br>' + s.Title + '</p>');
                });
            });

            // 当月のｽｹｼﾞｭｰﾙ設定
            _cal.CurrentMonth.forEach(function (current) {
                var ymd = current.Year + current.Month + current.Day;
                $('#thismonth').children().append('<li class="' + ymd + ' scroll-area"><span>' + current.Day + '</span></li>');
                // 週ｽｹｼﾞｭｰﾙ設定
                setWeekSchedule(current, ymd);
                // 当日の保存
                if (current.ThisDay === '1') {
                    _today = current;
                }
                // 日ｽｹｼﾞｭｰﾙ設定
                setDaySchedule(current, function (s) {
                    $('.' + ymd).append('<p class="' + s.Id + '">' + s.FromTime + '</br>' + s.Title + '</p>');
                    if (current.ThisDay === '1') {
                        addDaySchedule(s, ymd);
                    }
                });
                // 当日のｽﾀｲﾙ設定
                setToday(ymd,current);
            });

            // 翌月のｽｹｼﾞｭｰﾙを設定
            _cal.AfterMonth.forEach(function (after) {
                // 年月日の設定
                var ymd = after.Year + after.Month + after.Day;
                // 週ｽｹｼﾞｭｰﾙ設定
                setWeekSchedule(after, ymd);
                if (after.AfterEndDay === "0") { return; }
                // ｶﾚﾝﾀﾞｰ表示
                $('#nextmonth').children().append('<li class="' + ymd + ' scroll-area"><span>' + after.Day + '</span></li>');
                // 日ｽｹｼﾞｭｰﾙ設定
                setDaySchedule(after, function (s) {
                    $('.' + ymd).append('<p class="' + s.Id + '">' + s.FromTime + '</br>' + s.Title + '</p>');
                });
            });
      
            // 月ﾀｲﾄﾙ設定
            $('.title').eq(0).children().eq(1).text(_today.Year + ' / ' + _today.Month);

            // 週ﾀｲﾄﾙ設定
            var start = _weekstart.Year + ' / ' + _weekstart.Month + ' / ' + _weekstart.Day;
            var end = _weekend.Year + ' / ' + _weekend.Month + ' / ' + _weekend.Day;
            $('.title').eq(1).children().eq(1).text(start + ' - ' + end);

            // 曜日取得
            var dow = (_today.Dow === 'Saturday') ? 'sat' : (_today.Dow === 'Sunday') ? 'sun' : '';
            // 日ﾀｲﾄﾙ設定
            $('.title').eq(2).children().eq(1).text(_today.Year + ' / ' + _today.Month + ' / ' + _today.Day + ' ');
            $('.title').eq(2).children().eq(1).append('<span class="' + dow + '"> ' + _today.Dow + '</span>');

            // 基準日の設定
            $('#base-date').val(_today.Year + '_' + _today.Month + '_' + _today.Day);
            
        }
        //'''''''''''''''''''''''''''''''''''''''''
        // name   : setScheduleInput
        // param  : _d 日付ﾃﾞｰﾀ
        // remark : 予定入力画面の初期化
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        function setScheduleInput(_d)
        {
            // 日付ｵﾌﾟｼｮﾝ
            var date_options = {
                language: "ja",
                clearBtn: true,
                dateFormat: "yy / mm / dd"
            };

            // ﾊﾟﾗﾒｰﾀに日付ﾃﾞｰﾀがない場合は改めて取得
            var d = (_d) ? _d : new Date();

            // datepicker
            $("#from-date").datepicker(date_options).datepicker("setDate", d);
            $("#to-date").datepicker(date_options).datepicker("setDate", d);

            // timepickerのﾃﾞﾌｫﾙﾄ時間を設定
            var dt = new Date();
            var h = dt.getHours();
            var m = dt.getMinutes();
            m = Math.floor(m / 15) * 15;

            $('#from-time').val(('0' + h).slice(-2) + " : " + ('0' + m).slice(-2));
            $('#to-time').val(('0' + h).slice(-2) + " : " + ('0' + m).slice(-2));

            // 時間ｵﾌﾟｼｮﾝ
            var time_options = {
                show_meridian: false,
                min_hour_value: 0,
                max_hour_value: 23,
                step_size_minutes: 15,
                overflow_minutes: true,
                increase_direction: 'up',
                disable_keyboard_mobile: true
            };
            // timepicker
            $('#from-time').timepicki(time_options);
            $('#to-time').timepicki(time_options);

            // ｽｹｼﾞｭｰﾙ入力画面初期化
            $('#data-id').val('');   // id
            $('#data-rev').val('');  // ReVision
            $('#title').val('');    // ﾀｲﾄﾙ
            $('#remarks').val('');  // 内容

            // 更新ﾎﾞﾀﾝを非表示
            $('.add').show();
            $('.update').hide();
            $('.delete').hide();
        }
        //'''''''''''''''''''''''''''''''''''''''''
        // name   : getMoveCalender
        // param  : 予定ﾃﾞｰﾀ
        // remark : 日ｽｹｼﾞｭｰﾙを設定
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        function getMoveCalender() {
            // ｶﾚﾝﾀﾞｰの初期化
            var clearCalender = function () {
                $('#lastmonth').children().children().remove();  // 前月のｶﾚﾝﾀﾞｰ
                $('#thismonth').children().children().remove();  // 当月のｶﾚﾝﾀﾞｰ
                $('#nextmonth').children().children().remove();  // 次月のｶﾚﾝﾀﾞｰ
                $('#thisweek').children().children().remove();   // 当週のｶﾚﾝﾀﾞｰ
                $('.am').children().remove();                    // 当日午前
                $('.pm').children().remove();                    // 当日午後
                $('.weeks').children().children().remove();      // 曜日
            };
            // 遷移する期間
            var chkReqSpan = function (_t) {
                // 月
                if (_t.hasClass('month')) {
                    return REQUEST_MONTH;
                }
                // 週
                if (_t.hasClass('week')) {
                    return REQUEST_WEEK;
                }
                // 日
                if (_t.hasClass('day')) {
                    return REQUEST_DAY;
                }
            }

            // 前月へ
            $('.prev-anchor').click(function () {
                var span = '_' + chkReqSpan($(this));
                // 基準日の取得
                var ymd = $('#base-date').val();
                // ﾛｰﾃﾞｨﾝｸﾞの表示
                dispLoading();
                // Getﾘｸｴｽﾄ
                xhrGet(SCH_DATA + REQUEST_PREV + ymd + span, function (calender) {
                    clearCalender();
                    // ｶﾚﾝﾀﾞｰの初期化
                    setScheduleCalender(calender);
                    // 月ﾀｲﾄﾙ設定
                    $('.title').eq(0).children().eq(1).text(calender.CurrentMonth[0].Year + ' / ' + calender.CurrentMonth[0].Month);
                    // ﾛｰﾃﾞｨﾝｸﾞの非表示
                    removeLoading();
                }, function (err) {
                    console.error(err);
                    // ﾛｰﾃﾞｨﾝｸﾞの非表示
                    removeLoading();
                });
            });
            // 次月へ
            $('.next-anchor').click(function () {
                var span = '_' + chkReqSpan($(this));
                // 年月の取得
                var ymd = $('#base-date').val();
                // ﾛｰﾃﾞｨﾝｸﾞの表示
                dispLoading();
                // Getﾘｸｴｽﾄ
                xhrGet(SCH_DATA + REQUEST_NEXT + ymd + span, function (calender) {
                    clearCalender();
                    // ｶﾚﾝﾀﾞｰの初期化
                    setScheduleCalender(calender);
                    // 月ﾀｲﾄﾙ設定
                    $('.title').eq(0).children().eq(1).text(calender.CurrentMonth[0].Year + ' / ' + calender.CurrentMonth[0].Month);
                    // ﾛｰﾃﾞｨﾝｸﾞの非表示
                    removeLoading();
                }, function (err) {
                    console.error(err);
                    // ﾛｰﾃﾞｨﾝｸﾞの非表示
                    removeLoading();
                });
            });
        }
        //'''''''''''''''''''''''''''''''''''''''''
        // name   : addDaySchedule
        // param  : 予定ﾃﾞｰﾀ
        // remark : 日ｽｹｼﾞｭｰﾙを設定
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        function addDaySchedule(_s, ymd) {
            // 日ｽｹｼﾞｭｰﾙﾀｸﾞ
            var schdule = '<p class="' + _s.Id + '">' + _s.FromTime + '</br>' + _s.Title + '</p>';
            // 午前・午後のどちらかにｽｹｼﾞｭｰﾙ設定する
            if (_s.AmPm === AM) {
                $('.am').append(schdule);
            } else {
                $('.pm').append(schdule);
            }
        }
    //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    // Regist
    //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //'''''''''''''''''''''''''''''''''''''''''
        // name   : saveItem
        // param  : none
        // remark : ｶﾚﾝﾀﾞｰの初期化
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        function saveItem() {
            // ｸﾘｯｸｲﾍﾞﾝﾄ
            var setClickEvent = function (d) {
                var ymd = d.FromDate.replace(/\s+/g, "");
                ymd = ymd.replace(/\//g, '');

                $('.' + d.Id).remove();
                $('.' + ymd).children().children().append('<p class="' + d.Id + '">' + d.FromTime + '</br>' + d.Title + '</p>');

                $("#js-pager").children().children().eq(0).click();

                // 午前午後判定
                var chkTime = d.FromTime.slice(0,2);
                d.AmPm = (chkTime > 12) ? PM : AM;
                // 当日の場合
                if (d.ThisDay == '1') {
                    // 日ｽｹｼﾞｭｰﾙ設定
                    addDaySchedule(d, ymd);
                }
                // ｽｹｼﾞｭｰﾙｸﾘｯｸ時の処理
                $('.' + d.Id).on('click', function () {
                    $('.error').text('');             // ｴﾗｰｸﾘｱ
                    $('#data-id').val(d.Id);          // id
                    $('#data-rev').val(d.Rev);        // ReVision
                    $('#from-date').val(d.FromDate);  // 開始日
                    $('#from-time').val(d.FromTime);  // 開始時間
                    $('#to-date').val(d.ToDate);      // 終了日
                    $('#to-time').val(d.ToTime);      // 終了時間
                    $('#title').val(d.Title);         // ﾀｲﾄﾙ
                    $('#remarks').val(d.Remarks);     // 内容
                    // ｽｹｼﾞｭｰﾙ登録画面に遷移
                    $("#js-pager").children().children().eq(3).click();

                    // 登録ﾎﾞﾀﾝ:表示 更新ﾎﾞﾀﾝ:非表示
                    $('.add').hide();
                    $('.update').show();
                    $('.delete').show();
                });
                // ｽｹｼﾞｭｰﾙ入力画面初期化
                setScheduleInput();
            };

            // 入力ﾁｪｯｸ
            var validateCheck = function (d) {
                // 開始日時
                var _date = d.FromDate.split(' / ');
                var _time = d.FromTime.split(' : ');
                var start = new Date(_date[0], _date[1], _date[2], _time[0], _time[1]);
                // 終了日時
                _date = d.ToDate.split(' / ');
                _time = d.ToTime.split(' : ');
                var end = new Date(_date[0], _date[1], _date[2], _time[0], _time[1]);

                // ｴﾗｰｸﾘｱ
                $('.error').text('');

                // 日時
                if (start.getTime() > end.getTime()) {
                    $('.error').eq(0).text(TIME_VALIDATE_MESSAGE);
                    return true;
                }
                // ﾀｲﾄﾙ
                if (!d.Title) {
                    $('.error').eq(1).text(TITLE_VALIDATE_MESSAGE);
                    return true;
                }
                // 内容
                if (!d.Remarks) {
                    $('.error').eq(2).text(REMARKS_VALIDATE_MESSAGE);
                    return true;
                }

                // ｴﾗｰｸﾘｱ
                $('.error').text('');
                return false;
            };

            // 入力内容を取得
            var _id = $('#data-id').val();         // id
            var _rev = $('#data-rev').val();       // ReVision
            var _fromdate = $('#from-date').val(); // 開始日
            var _fromtime = $('#from-time').val(); // 開始時間
            var _todate = $('#to-date').val();     // 終了日;
            var _totime = $('#to-time').val();     // 終了時間;
            var _title = $('#title').val();        // ﾀｲﾄﾙ
            var _remarks = $('#remarks').val();    // 内容

            // ﾊﾟﾗﾒｰﾀとして設定
            var data = {
                Id: _id,
                Rev: _rev,
                FromDate: _fromdate,
                FromTime: _fromtime,
                ToDate: _todate,
                ToTime: _totime,
                Title: _title,
                Remarks: _remarks
            };

            // 入力ﾁｪｯｸに該当した場合はｻｰﾊﾞｺｰﾙしない
            if (validateCheck(data)) {
                $("#js-pager").children().children().eq(3).click();
                return false;
            }
            // ﾛｰﾃﾞｨﾝｸﾞの表示
            dispLoading();
            // idが設定されている場合は更新
            if (_id) {
                xhrPut(SCH_DATA, data, function (item) {
                    data.Rev = item.rev;
                    setClickEvent(data);
                    // ﾛｰﾃﾞｨﾝｸﾞの非表示
                    removeLoading();
                }, function (err) {
                    console.error(err);
                    // ﾛｰﾃﾞｨﾝｸﾞの非表示
                    removeLoading();
                });
            } else {
                xhrPost(SCH_DATA, data, function (item) {
                    data.Id = item.id;
                    data.Rev = item.rev;
                    setClickEvent(data);
                    // ﾛｰﾃﾞｨﾝｸﾞの非表示
                    removeLoading();
                }, function (err) {
                    console.error(err);
                    // ﾛｰﾃﾞｨﾝｸﾞの非表示
                    removeLoading();
                });
            }
        }
        //'''''''''''''''''''''''''''''''''''''''''
        // name   : deleteItem
        // param  : none
        // remark : ｽｹｼﾞｭｰﾙの削除処理を行う
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        function deleteItem() {
            var _id = $('#data-id').val();    // id
            var _rev = $('#data-rev').val();  // ReVision

            // ﾛｰﾃﾞｨﾝｸﾞの表示
            dispLoading();
            // 削除ﾘｸｴｽﾄを送信
            xhrDelete(SCH_DATA + '?id=' + _id + "&rev=" + _rev, function () {
                // 表示しているｽｹｼﾞｭｰﾙを削除
                $('.' + _id).remove();
                // ｶﾚﾝﾀﾞｰ表示に戻る
                $("#js-pager").children().children().eq(0).click();
                // ｽｹｼﾞｭｰﾙ入力画面初期化
                setScheduleInput();
                // ﾛｰﾃﾞｨﾝｸﾞの非表示
                removeLoading();
            }, function (err) {
                console.error(err);
                // ﾛｰﾃﾞｨﾝｸﾞの非表示
                removeLoading();
            });
        }

        //'''''''''''''''''''''''''''''''''''''''''
        // name   : dispLoading
        // param  : none
        // remark : ﾛｰﾃﾞｨﾝｸﾞｲﾒｰｼﾞを表示
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        function dispLoading() {
            // ｴﾗｰｸﾘｱ
            $('.error').text('');
            // ﾛｰﾃﾞｨﾝｸﾞが表示されていない場合表示する
            if ($("#loading").length == 0) {
                $("body").append("<div id='loading'><div class='loadingMsg'>Now Loading</div></div>");
            }
        }

        //'''''''''''''''''''''''''''''''''''''''''
        // name   : removeLoading
        // param  : none
        // remark : ﾛｰﾃﾞｨﾝｸﾞｲﾒｰｼﾞを非表示
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
        function removeLoading() {
            // ｽｸﾛｰﾙｽﾀｲﾙを設定
            $('.scroll-area').jScrollPane();
            // ﾛｰﾃﾞｨﾝｸﾞを削除
            $("#loading").remove();
        }

//''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
// Load
//,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    // 画面初期表示処理
    createCalender();
