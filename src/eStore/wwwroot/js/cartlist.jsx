//
// ReactBootstrap Component variables
//
var ListGroup = ReactBootstrap.ListGroup;
var ListGroupItem = ReactBootstrap.ListGroupItem;
var Modal = ReactBootstrap.Modal;
//
// Tray Component
//

// example of how to do inline style
var qtyStyle = {
    textAlign: "left",
    position: "relative",
    width: "10%",
    paddingRight: "10px"
}

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
        var detailsForModal = this.state.cartdetails.map(details =>
            <ModalDetails details={details} key={details.CartId } />
        );
        return (
        <div>
            <ListGroupItem onClick={this.open}>
                <span className="col-xs-3 text-left">{this.props.cart.Id}</span>
                <span className="col-xs-9 order-line">{formatDate(this.props.cart.DateCreated)}</span>
            </ListGroupItem>
            <Modal show={this.state.showModal} onHide={this.close}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div>
                            <span className="col-xs-12 text-right">Date:{formatDate(this.props.cart.DateCreated)}</span>
                            <span className="col-xs-3 text-center">Cart:{this.props.cart.Id}</span>
                            <span className="col-xs-9 text-right xsmallFont">&nbsp;</span>
                        </div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <ListGroup>
                    <div className="text-center navbar navbar-default">
                        <div className="col sm-5 col-xs-5 top10 bold">Product</div>
                        <div className="col-sm-2 col-xs-2 top10 bold">MSRP</div>
                        <div className="col-sm-1 col-xs-1 top10 bold">QtyS</div>
                        <div className="col-sm-1 col-xs-1 top10 bold">QtyO</div>
                        <div className="col-sm-1 col-xs-1 top10 bold">QtyB</div>
                        <div className="col-sm-2 col-xs-2 top10 bold">Extended</div>
                    </div>
                        {detailsForModal}
                    </ListGroup>
                </Modal.Body>
                <Modal.Footer>
                    <div className="text-right">
                        <span className="col-xs-12 text-right">SubTotal: ${this.props.cart.PriceTotal}</span>
                        <span className="col-xs-12 text-right">Tax: ${this.props.cart.PriceTotal * 0.13}</span>
                        <span className="col-xs-12 text-right">Order Total: ${this.props.cart.PriceTotal * 1.13}</span>
                    </div>
                </Modal.Footer>
            </Modal>
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
                        <div className="text-center navbar navbar-default" style={{ top: "25px", position: "relative" } }>
                            <div className="col sm-4 col-xs-2" style={{ top: "10px", position: "relative" } }>#</div>
                            <div className="col-sm-8 col-xs-10" style={{ top: "10px", position: "relative" } }>Date</div>
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
});

//
// ModalDetails Component
//
var ModalDetails = React.createClass({
    render: function () {
        return (
            <ListGroupItem>
                <div>
                <span className="col-xs-5">{this.props.details.ProductName}</span>
                <span className="col-xs-2">${this.props.details.MSRP}</span>
                <span className="col-xs-1">{this.props.details.QTY}</span>
                <span className="col-xs-1">{this.props.details.QTYOrdered}</span>
                <span className="col-xs-1">{this.props.details.QTYBackOrdered}</span>
                <span className="col-xs-2">${this.props.details.ExtendedPrice}</span>
                </div>
            </ListGroupItem>
       )
    }
});

ReactDOM.render(
    <Cartlist source="/GetCarts" />,
    document.getElementById("estore") // html tag
)