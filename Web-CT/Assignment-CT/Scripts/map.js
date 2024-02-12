
let xmlHttp = new XMLHttpRequest();
xmlHttp.open("GET", "Clinics/GetClinicsList", false);
xmlHttp.send(null);
clinics = JSON.parse(xmlHttp.responseText);

let map;
function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(
        browserHasGeolocation
            ? "Error: The Geolocation service is failed"
            : "Error: Your browser doesn't support geolocation"
    );
    infoWindow.open(map);
}

function geoAddress(map, clinic) {
    var geocoder = new google.maps.Geocoder();
    var content = "<h4>" + clinic.ClinicName + "</h4><hr/><p>" + clinic.ClinicAddress + "</p>"
    var infoWindow = new google.maps.InfoWindow({ content: content });
    geocoder.geocode({ address: clinic.ClinicAddress }, function (result, status) {
        if (status === "OK") {
            var marker = new google.maps.Marker({
                map: map,
                position: result[0].geometry.location
            });
            marker.addListener("click", function () {
                infoWindow.open(map, marker);
            });
        }
    });
}

function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: -37.800, lng: 144.844 },
        zoom: 12,
    });

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
            position => {
                const pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                map.setCenter(pos);
            },
            () => {
                handleLocationError(false, inforWindow, map.getCenter());
            }
        );
    } else {
        handleLocationError(false, inforWindow, map.getCenter());
    }
    for (var i = 0; i < clinics.length; i++) {
        console.log(clinics[i]);
        geoAddress(map, clinics[i]);
    }

    //auto complete the address
    var start = document.getElementById("start");
    const autoComplete = new google.maps.places.Autocomplete(start);
    autoComplete.bindTo("bounds", map);

    //direction
    const directionsRenderer = new google.maps.DirectionsRenderer();
    const directionsService = new google.maps.DirectionsService();
    directionsRenderer.setMap(map);
    directionsRenderer.setPanel(document.getElementById("sidebar"));
    var getDirection = document.getElementById("get-direction")
    getDirection.addEventListener("click", () => {
        directionsService.route({
            origin: {
                query: document.getElementById("start").value
            },
            destination: {
                query: document.getElementById("end").value
            },
            travelMode: google.maps.TravelMode[document.getElementById("mode").value]
        }, (response, status) => {
            if (status === "OK") {
                directionsRenderer.setDirections(response);
            } else {
                window.alert("unable to get direction due to: " + status);
            }
        }
        );
    });
}