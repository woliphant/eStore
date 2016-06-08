//
// Call server without jQuery
// - return json to callback function
//
function httpGet(url, callback) {
    var http = new XMLHttpRequest();
    http.onreadystatechange = function () {
        if (http.readyState == 4 && http.status == 200) {
            callback(JSON.parse(http.responseText));
        } // if
    };
    http.open("GET", url, true);
    http.send();
} // httpGet
//
// format date
//
function formatDate(date) {
    var d;
    if (date === undefined) {
        d = new Date(); //no date coming from server
    }
    else {
        var d = new Date(Date.parse(date)); // date from server
    }
    var _day = d.getDate();
    var _month = d.getMonth() + 1;
    var _year = d.getFullYear();
    var _hour = d.getHours();
    var _min = d.getMinutes();
    if (_min < 10) { _min = "0" + _min; }
    return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;
} // formatDate