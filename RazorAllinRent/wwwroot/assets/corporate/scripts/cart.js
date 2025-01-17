function cartUp() {
    let cartItems = JSON.parse(localStorage.getItem('cartItems'));
    var base_url = window.location.origin;
    var host = window.location.host;
    var pathArray = window.location.pathname.split('/');
    var url = base_url + '/' + pathArray[1];


    if (cartItems == null) {
        cartItems = new Array();
    }
    let cartContainer = document.getElementById('myCart');
    cartContainer.innerHTML = '';

    if (cartItems.length > 0) {


        for (let i = 0; i < cartItems.length; i++) {
            if (cartItems[i].quantity == null) {
                cartItems[i].quantity = 1;
            }
            cartContainer.innerHTML += '<li id="cart-' + cartItems[i].id + '"><a href="shop-item.html"><img src="' + url + '/assets/pages/img/products/' + cartItems[i].image_name + '" alt="Rolex Classic Watch" width="37" height="34"></a><span class="cart-content-count">x ' + cartItems[i].quantity + '</span><strong><a href="shop-item.html">' + cartItems[i].product_name + '</a></strong><em>Ar ' + cartItems[i].price + '</em><button onclick="removeFromCart(' + cartItems[i].id + ')" class="del-goods">&nbsp;</button></li>';
        }
    }

}

function saveReceipt(receipt, quant) {
    let tempPercentage = parseFloat(receipt.percentage) * quant;
    let quantityInt = 0;
    let quantityFloat = 0;
    let quantity = 0;
    // if (parseFloat(receipt.percentage) <= parseFloat(receipt.qty)) {
    quantityFloat = tempPercentage / parseFloat(receipt.qty);
    // } else {
    //     quantityFloat = parseFloat(receipt.qty) / tempPercentage;
    // }
    quantityInt = parseInt(quantityFloat);
    if (quantityFloat > parseFloat(quantityInt)) {
        quantityInt = quantityInt + 1;
    }
    if (quantityInt == 0) {
        quantity = quantityInt + 1;
    } else {
        quantity = quantityInt;
    }
    if (quantity > parseInt(receipt.instock)) {
        return null;
    }
    receipt.quantity = quantity;
    receipt.instock -= quantity;
    console.log("R1");
    console.log(parseFloat(receipt.qty) * receipt.quantity);
    console.log(tempPercentage);
    receipt.reste = (parseFloat(receipt.qty) * receipt.quantity) - tempPercentage;
    return receipt;
}

function saveExistingReceipt(receipt, rec, quant, restePlus) {
    let tempPercentage = parseFloat(rec.percentage) * quant;
    let demande = parseFloat(tempPercentage) - parseFloat(receipt.reste);


    let quantityInt = 0;
    let quantityFloat = 0;
    let quantity = 0;

    if (demande >= 0) {
        // if (parseFloat(demande) <= parseFloat(receipt.qty)) {
        quantityFloat = demande / parseFloat(rec.qty);
        // } else {
        //     quantityFloat = parseFloat(receipt.qty) / demande;
        // }
        console.log(receipt.product_name);
        console.log(receipt.reste);
        console.log(demande);
        quantityInt = parseInt(quantityFloat);
        console.log(quantityFloat);
        console.log(quantityInt);
        console.log(quantityFloat > parseFloat(quantityInt));
        if (quantityFloat > parseFloat(quantityInt)) {
            quantityInt = quantityInt + 1;
        }
        // if (quantityInt == 0) {
        //     quantity = quantityInt + 1;
        // } else {
        //     quantity = quantityInt;
        // }
        if (quantity > parseInt(rec.instock)) {
            return null;
        }

        quantity = quantityInt;
        receipt.quantity += quantity;
        receipt.instock -= quantity;

        receipt.reste = (parseFloat(receipt.qty) * quantity) - demande + restePlus;
    } else {
        // if (parseFloat(-1 * demande) <= parseFloat(receipt.qty)) {
        quantityFloat = (-1 * demande) / parseFloat(rec.qty);
        // } else {
        //     quantityFloat = parseFloat(receipt.qty) / (-1 * demande);
        // }
        console.log(receipt.product_name);
        console.log(receipt.reste);
        console.log(demande);
        quantityInt = parseInt(quantityFloat);
        console.log(quantityFloat);
        console.log(quantityInt);
        console.log(quantityFloat > parseFloat(quantityInt));
        // if (quantityFloat > parseFloat(quantityInt)) {
        //     quantityInt = quantityInt + 1;
        // }
        // if (quantityInt == 0) {
        //     quantity = quantityInt + 1;
        // } else {
        //     quantity = quantityInt;
        // }
        if (quantity > parseInt(rec.instock)) {
            return null;
        }
        quantity = quantityInt;
        receipt.quantity += quantity;
        receipt.instock -= quantity;
        receipt.reste = (-1 * demande) + restePlus;
    }
    return receipt;
}

function addReceiptToCart(receipt_details) {
    let receiptItems = JSON.parse(localStorage.getItem('cartItems'));
    let tempReceipt = new Array();
    let inputQuantity = document.getElementById('receipt-quantity');
    let quant = 1;
    if (inputQuantity != null) {
        if (inputQuantity != "") {
            quant = parseInt(inputQuantity.value);
        }
    }
    if (receiptItems == null) {
        receiptItems = new Array();
    } else {
        for (let j = 0; j < receiptItems.length; j++) {
            tempReceipt.push(Object.assign({}, receiptItems[j]));
        }
    }
    let isEnough = true;
    let ingr = new Array();
    let receipt = null;
    for (let i = 0; i < receipt_details.length; i++) {
        if (receiptItems.length > 0) {
            let exist = null;
            let index = null;
            let rec = null;

            for (let j = 0; j < receiptItems.length; j++) {
                if (receiptItems[j].id == receipt_details[i].id && receiptItems[j].receipt_id != null) {
                    exist = receiptItems[j];
                    rec = receipt_details[i];
                    index = j;
                    break;
                }
            }
            if (exist != null && index != null) {
                console.log("exist");
                console.log(exist);
                console.log("rec");
                console.log(rec);

                console.log("confirm");
                console.log(exist.reste >= (parseFloat(rec.percentage) * quant));
                if (exist.reste >= (parseFloat(rec.percentage) * quant)) {
                    let text = "Il y a des restes qu'on peut utiliser pour " + rec.product_name + "\nVoulez-vous l'utilisez?!\nCliquez sur OK pour OUI ou cliquez sur Cancel pour NON.";
                    let confirm = window.confirm(text)
                    let restePlus = 0;
                    console.log("confirm =" + confirm);
                    if (confirm) {
                        receipt = saveExistingReceipt(exist, rec, quant, restePlus);
                    } else {
                        restePlus = exist.reste;
                        exist.reste = 0;
                        receipt = saveExistingReceipt(exist, rec, quant, restePlus);
                    }
                } else {
                    receipt = saveExistingReceipt(exist, rec, quant, 0);
                }
            } else {
                receipt = saveReceipt(receipt_details[i], quant);
            }
            if (receipt != null) {
                if (exist != null && index != null) {
                    receiptItems[index] = receipt;
                } else {
                    receiptItems.push(receipt);
                }
            } else {
                isEnough = false;
                ingr.push(receipt_details[i].product_name);
            }
        } else {

            receipt = saveReceipt(receipt_details[i], quant);
            if (receipt != null) {
                receiptItems.push(receipt);
            } else {
                isEnough = false;
                ingr.push(receipt_details[i].product_name);
            }
        }
        localStorage.setItem('cartItems', JSON.stringify(receiptItems));
    }
    console.log(isEnough);
    if (!isEnough) {
        localStorage.removeItem('cartItems');
        console.log(tempReceipt);
        localStorage.setItem('cartItems', JSON.stringify(tempReceipt));
        alert("Quantite en stock insuffisant pour un ingredient : " + JSON.stringify(ingr));
    }
    cartUp();
}


function addToCart(product) {

    let cartItems = JSON.parse(localStorage.getItem('cartItems'));
    product.receipt_id = null;
    console.log(product);
    console.log(cartItems);
    if (cartItems != null) {
        var existing_prod = cartItems.find(p => p.id == product.id && p.receipt_id == null);
    }

    if (product.instock > 0) {


        if (existing_prod == null) {
            var base_url = window.location.origin;
            var host = window.location.host;
            var pathArray = window.location.pathname.split('/');
            var url = base_url + '/' + pathArray[1];

            let inputQuantity = document.getElementById('product-quantity');
            product.quantity = 1;
            product.receipt_id = null;
            product.reste = null;
            product.percentage = null;


            if (inputQuantity != null) {
                product.quantity = parseInt(inputQuantity.value);
            }
            if (parseInt(product.quantity) <= parseInt(product.instock)) {
                if (cartItems == null) {
                    cartItems = new Array();
                }

                product.instock = parseInt(product.instock) - parseInt(product.quantity);
                cartItems.push(product);
                // let cartContainer = document.getElementById('myCart');
                // cartContainer.innerHTML += '<li id="cart-' + product.id + '"><a href="shop-item.html"><img src="' + url + '/assets/pages/img/products/' + product.image_name + '" alt="Rolex Classic Watch" width="37" height="34"></a><span class="cart-content-count">x ' + product.quantity + '</span><strong><a href="shop-item.html">' + product.product_name + '</a></strong><em>Ar ' + product.price + '</em><button onclick="removeFromCart(' + product.id + ')" class="del-goods">&nbsp;</button></li>';
                localStorage.setItem('cartItems', JSON.stringify(cartItems));
            } else {
                alert("Quantité en stock non suffisant 1!");
            }

        } else {

            let index = cartItems.findIndex(p => p.id == product.id && p.receipt_id == null);
            let inputQuantity = document.getElementById('product-quantity');
            product.quantity = 1;
            product.receipt_id = null;
            product.reste = null;
            product.percentage = null;


            if (inputQuantity != null) {
                product.quantity = parseInt(inputQuantity.value);
            }

            if ((parseInt(existing_prod.quantity) + parseInt(product.quantity)) <= parseInt(existing_prod.instock)) {

                existing_prod.quantity = parseInt(existing_prod.quantity) + parseInt(product.quantity);
                existing_prod.instock = parseInt(existing_prod.instock) - parseInt(product.quantity);
                cartItems[index] = existing_prod;

                localStorage.setItem('cartItems', JSON.stringify(cartItems));
            } else {
                alert("Quantité en stock non suffisant 2!");
            }
        }
        cartUp();
    } else {
        alert("Quantité en stock non suffisant 3!");
    }
}


function removeFromCart(product_id) {
    let prod = String(product_id);
    let cartElement = document.getElementById('cart-' + prod);
    cartElement.remove();
    let cartItems = JSON.parse(localStorage.getItem('cartItems'));
    localStorage.removeItem('cartItems');
    cartItems = cartItems.filter(item => item.id != product_id);
    localStorage.setItem('cartItems', JSON.stringify(cartItems));
    cartTable();
    changeCartTotal();
}

function cartTable() {
    let content = ''

    let cartItems = JSON.parse(localStorage.getItem('cartItems'));

    var base_url = window.location.origin;
    var host = window.location.host;
    var pathArray = window.location.pathname.split('/');
    var url = base_url + '/' + pathArray[1];


    if (cartItems == null) {
        cartItems = new Array();
    }

    let cartContainer = document.getElementById('cart-items-list');
    if (cartContainer != null) {
        if (cartItems.length > 0) {
            cartContainer.innerHTML = '';

            for (let i = 0; i < cartItems.length; i++) {
                if (cartItems[i].quantity == null) {
                    cartItems[i].quantity = 1;
                }
                content += '<tr>';
                content += '<td class="goods-page-image"><a href="javascript:;"><img src="' + url + '/assets/pages/img/products/' + cartItems[i].image_name + '" alt="Berry Lace Dress"></a></td>';
                content += '<td class="goods-page-description"><h3><a href="javascript:;">' + cartItems[i].product_name + '</a></h3><p><strong>Item 1</strong> - Color: Green; Size: S</p><em>More info is here</em></td>';
                content += '<td class="goods-page-ref-no" id="instock">' + cartItems[i].instock + '</td>';
                content += '<td class="goods-page-quantity"><div class="product-quantity"><input id="product-quantity' + cartItems[i].id + '" type="number" onchange="changeQuantity(' + cartItems[i].id + ')" value="' + cartItems[i].quantity + '" class="form-control input-sm"></div></td>';
                content += '<td class="goods-page-price"><strong><span>Ar </span>' + cartItems[i].price + '</strong></td>';
                content += '<td class="goods-page-total"><strong><span>Ar </span><span id="item-total-' + cartItems[i].id + '">' + cartItems[i].price * cartItems[i].quantity + '</span></strong></td><td class="del-goods-col"><a class="del-goods" href="javascript:;" onclick="removeFromCart(' + cartItems[i].id + ')">&nbsp;</a></td>';
                content += '</tr>';
                cartContainer.innerHTML += content;
                content = '';

            }

            changeCartTotal();
        } else {
            cartContainer.innerHTML = '';
        }
    }
}

function changeQuantity(product_id) {
    let inputQuantity = document.getElementById('product-quantity' + product_id);
    if (inputQuantity.value >= 0) {
        let cartItems = JSON.parse(localStorage.getItem('cartItems'));
        let product = cartItems.find(p => p.id == product_id);
        let qty = inputQuantity.value;
        if (product.instock >= 0 && (product.instock - (-1 * (product.quantity - qty))) >= 0) {
            product.instock = product.instock - (-1 * (product.quantity - qty));
            product.quantity = qty;
            let instockText = document.getElementById('instock');
            instockText.innerHTML = product.instock;

            let index = cartItems.findIndex(p => p.id == product_id);
            cartItems[index] = product;
            localStorage.setItem('cartItems', JSON.stringify(cartItems));
            changeCartItemTotal(product);
            changeCartTotal();
        } else {
            inputQuantity.value = product.quantity;
        }
    } else {
        inputQuantity.value = 0;
    }
}


function setCartHidden() {
    let cartHidden = document.getElementById('cart-hidden');
    if (cartHidden != null) {
        let cartItems = localStorage.getItem('cartItems');
        if (cartItems == null) {
            cartItems = '';
        }
        cartHidden.value = cartItems;
        console.log(cartHidden.value);
    }
}


function changeCartItemTotal(product) {
    let itemTotal = document.getElementById('item-total-' + product.id);
    itemTotal.innerHTML = product.price * product.quantity;
}


function changeCartTotal() {
    let cartTotal = document.getElementById('cart-total');
    if (cartTotal != null) {
        let cartItems = JSON.parse(localStorage.getItem('cartItems'));
        let total = 0;
        for (let i = 0; i < cartItems.length; i++) {
            total += (cartItems[i].price * cartItems[i].quantity);
        }
        cartTotal.innerHTML = total;
    }
}
