//
// Bootstrap Component variables
//
var Modal = ReactBootstrap.Modal;
var Button = ReactBootstrap.Button;
var mapStyle = {
    height: "100%",
    width: "100%",
    marginTop: "10px",
    minHeight: "400px"
}

//
// StoreLocator Component
//
var StoreLocator = React.createClass({
    getInitialState() {
        return { showModal: false, branchdetails: [] };
    },
    close() {
        this.setState({ showModal: false });
    },
    open() {
        var _this = this;
        var lat, lng, map, i, marker;
        var address = this.refs.address.value.trim();
        var geocoder = new google.maps.Geocoder(); // A service for converting between an address to LatLng
        geocoder.geocode({ "address": address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) { // only if google gives us the OK
                lat = results[0].geometry.location.lat();
                lng = results[0].geometry.location.lng();
                httpGet("/GetStores/" + lat + "/" + lng, function (data) {
                    var myLatLng = new google.maps.LatLng(lat, lng);
                    _this.setState({ showModal: true });
                    var map_canvas = _this.refs.map;
                    var options = {
                        zoom: 10,
                        center: myLatLng,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };
                    map = new google.maps.Map(map_canvas, options);
                    var center = map.getCenter();
                    var i2 = 0;
                    var infowindow = null;
                    infowindow = new google.maps.InfoWindow({ content: "holding..." });
                    for (i = 0; i < data.length; i++) {
                        i2 = i + 1;
                        marker = new google.maps.Marker({
                            position: new google.maps.LatLng(data[i].Latitude, data[i].Longitude),
                            map: map,
                            animation: google.maps.Animation.DROP,
                            icon: "../img/marker" + i2 + ".png",
                            title: "Store# " + data[i].Id + " " + data[i].Street + ", "
                            + data[i].City + ", " + data[i].Region,
                            html: "<div>" + "Store# " + data[i].Id + "<br/>" +
                            data[i].Street + ", " + data[i].City + "<br/>" +
                            data[i].Distance.toFixed(2) + " km</div>"
                        });
                        google.maps.event.addListener(marker, 'click', function () {
                            infowindow.setContent(this.html); // added .html to the marker object.
                            infowindow.close();
                            infowindow.open(map, this);
                        });
                    }
                    map.setCenter(center);
                    google.maps.event.trigger(map, "resize");
                }.bind(_this));
            }
        });
    },
    render: function () {
        return (
        <div style={{top:"50px", position:"relative"}}>
<div className="col-sm-4 col-xs-1">&nbsp;</div>
<div className="std-box col-sm-4 col-xs-10 text-center">
<div>
<b>Required Address:</b><br />
<input type="text" ref="address" /><br />
<input type="button" onClick={this.open} value="locate" className="btn btn-default" />
</div>
</div>
<Modal show={this.state.showModal} onHide={this.close}>
<Modal.Header closeButton>
<Modal.Title>
<div className="text-center">3 Closest Stores</div>
</Modal.Title>
</Modal.Header>
<Modal.Body>
<div ref="map" id="map" style={mapStyle}></div>
</Modal.Body>
</Modal>
</div>
)
}
});
ReactDOM.render(
<StoreLocator />,
document.getElementById("stores") // html tag
)