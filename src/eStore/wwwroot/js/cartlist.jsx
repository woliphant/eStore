//
// ReactBootstrap Component variables
//
var ListGroup = ReactBootstrap.ListGroup;
var ListGroupItem = ReactBootstrap.ListGroupItem;
//
// Tray Component
//
var Cart = React.createClass({
    getInitialState() {
        return { showModal: false, cartdetails: [] };
    },
    close() {
        this.setState({ showModal: false });
    },
    open() {
        this.setState({ showModal: true });
        var cart = this.props.cart;
        var url = this.props.source + "/" + cart.Id;
        httpGet(url, function (data) {
            this.setState({ cartdetails: data });
        }.bind(this));
    },
    render: function () {
        return (
        <div>
            <ListGroupItem onClick={this.open}>
                <span className="col-xs-3 text-left">{this.props.cart.Id}</span>
                <span className="col-xs-9 order-line">{formatDate(this.props.cart.DateCreated)}</span>
            </ListGroupItem>
        </div>
        )
    }
});

//
// TrayList Component
//
var Cartlist = React.createClass({
    getInitialState: function () {
        return ({ carts: [] });
    },
    componentDidMount: function () {
        httpGet(this.props.source, function (data) {
            this.setState({ carts: data });
        }.bind(this));
    },
    render: function () {
        var carts = this.state.carts.map(cart =>
            <Cart cart={cart} key={cart.Id} source="/GetCartDetails" />
    );
return (
        <div className="top25">
            <div className="col-sm-4 col-xs-1">&nbsp;</div>
            <div className="col-sm-4 col-xs-12">
                <div className="panel-title text-center">
                    <h3>Carts You've Saved</h3>
                    <img className="smaller-img" src="/img/Cart.png" />
                </div>
                <div className="panel-body">
                    <div>
                        <div className="text-center navbar navbar-default" style={{top:"25px", position:"relative"}}>
                            <div className="col sm-4 col-xs-2" style={{top:"10px", position:"relative"}}>#</div>
                            <div className="col-sm-8 col-xs-10" style={{top:"10px", position:"relative"}}>Date</div>
                        </div>
                        <ListGroup>
                            {carts}
                        </ListGroup>
                    </div>
                </div>
            </div>
        </div>
    )
    }
})
ReactDOM.render(
    <Cartlist source="/GetCarts" />,
    document.getElementById("estore") // html tag
)