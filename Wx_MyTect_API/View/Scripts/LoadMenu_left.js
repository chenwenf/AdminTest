function LoadMenu_left(navData)
{
    var leftHtml = "";
    for (var i = 0; i < navData.length; i++) {
        if (navData[i].children !== undefined && navData[i].children.length > 0)
        {
            leftHtml += "<dl><dt>";
            if (navData[i].icon !== undefined && navData[i].icon !== '') {
                leftHtml += " <i class=\"iconfont\">" + navData[i].icon +"</i>&nbsp;&nbsp;";
            }
            leftHtml += navData[i].title + "<i class=\"iconfont menu_dropdown-arrow\">&#xe685;</i></dt>";
            leftHtml += "                    <dd>";
            leftHtml += "                        <ul>";
            for (var j = 0; j < navData[i].children.length; j++) {
                leftHtml += "  <li onclick=\"OnLoadTab('" + navData[i].children[j].href + "','"
                    + navData[i].children[j].title + "') \"><a  href=\"javascript: ;\"  title=\""
                    + navData[i].children[j].title + "\">";
                if (navData[i].children[j].icon !== undefined && navData[i].children[j].icon !== '') {
                    leftHtml += " <i class=\"iconfont\">" + navData[i].children[j].icon +"</i>&nbsp;&nbsp;";
                }
                leftHtml += navData[i].children[j].title + "</a></li>";
            }
            leftHtml += "                        </ul>";
            leftHtml += "                    </dd>";
            leftHtml += "                </dl>";
        }
    }
    return leftHtml;
}

function OnLoadTab(url,title)
{
    $("#Child_Div iframe").attr("src", url)

    $("#Child_Div .c-666").html(title);

}

function iframeReplace()
{
    var timestamp = new Date().getTime();
    $("#Child_Div iframe").attr("src", $('#Child_Div iframe').attr('src') + "?"+timestamp)
}

