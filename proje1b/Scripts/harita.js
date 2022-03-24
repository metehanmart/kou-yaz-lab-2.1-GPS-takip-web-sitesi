var locationObj;

function setMap(jsondata) {
    locationObj = $.parseJSON(jsondata);
    var qMapTypeId = google.maps.MapTypeId.SATELLITE

    mapOptions = {
        zoom: 1,
        center: myLatlng,  // this line won't work yet
        mapTypeControl: true,
        mapTypeControlOptions: {
            style: google.maps.MapTypeControlStyle.DROPDOWN_MENU
        }
    }


    map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions);

    map.setTilt(45);
    map.setHeading(0);
    map.setMapTypeId(qMapTypeId);

    for (i in locationObj) {
        var LatLngPair = new google.maps.LatLng(jsondata[i]["Lat"], jsondata[i]["Lng"]);

        var hItem = jsondata[i];
        var qLat = parseFloat(hItem["Lat"]);
        var qLong = parseFloat(hItem["Lng"]);

        var myLatlng;
        if (qLat != 0 && qLong != 0) {
            myLatlng = new google.maps.LatLng(qLat, qLong);


            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map
            });

            google.maps.event.addListener(marker, "click", function (event) {

            });
        }

    }
    $("#map-canvas").show();
    $("#divLoadingMessageMap").css("display", "none");
}