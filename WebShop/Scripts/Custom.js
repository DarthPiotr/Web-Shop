// Gets parameter from url's query string
var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
};

var setCheckbox = function setCheckbox(itemvalue, checkboxname, i){
        console.log('#MainContent_' + checkboxname + '_' + i + " [itemvalue]: " + itemvalue + ", [checked]: " + list.indexOf(itemvalue));
        if (list.indexOf(itemvalue) != -1) {
            $('#MainContent_' + checkboxname + '_' + i).attr("checked", "checked");
        }
}

var addListNodes = function addListNodes(id, activePage) {
    var text = $(id).html();
    text = text.replace(/\<a/g, "<li><a");
    text = text.replace(/\<\/a\>/g, "</a></li>");
    
    var i = 0;
    var start = 0;
    //console.log("Before before: " + text);
    while (i < activePage && text.indexOf("<li>", start) != -1) {
        i++;
        if (i == activePage) {
            //console.log("Before: " + text);
            text = text.substring(0, text.indexOf("<li>", start) + 3) + " class='active'" + text.substring(text.indexOf("<li>", start) + 3, text.length - 1);
            //console.log("After: " + text);
            break;
        }
        start = text.indexOf("<li>", start) + 3;
    }

    $(id).html(text);
}