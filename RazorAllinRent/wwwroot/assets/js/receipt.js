var i = 0;

function addDetail() {
    let productInput = document.getElementById('product');
    let qtyInput = document.getElementById('percentage');
    if (productInput != null & qtyInput != null) {
        if (productInput.value != null && qtyInput.value != null && parseInt(qtyInput.value) > 0) {
            let receiptDetails = JSON.parse(localStorage.getItem('receiptDetails'));

            let product = {
                id: productInput.value,
                productName: productInput.options[productInput.selectedIndex].text,
                percentage: qtyInput.value,
            }
            if (receiptDetails == null) {
                receiptDetails = new Array();

                let receiptOut = document.getElementById('receipt-details');
                let content = '';
                if (receiptOut != null) {
                    content += '<tr id="row-' + product.id + '">';
                    content += '<td>' + product.productName + '</td>';
                    content += '<td>' + product.percentage + '</td>';
                    content += '<td><button class="btn btn-danger" onclick=\'remove(' + product.id + ')\'>Supprimer</button></td>';
                    content += '</tr>';
                }
                receiptOut.innerHTML += content;
                receiptDetails.push(product);
                localStorage.setItem('receiptDetails', JSON.stringify(receiptDetails));
            } else {

                let existing = receiptDetails.find(r => r.id == product.id);
                let existing_index = receiptDetails.findIndex(r => r.id == product.id);
                console.log(existing);

                if (existing != null) {
                    let row = document.getElementById('row-' + existing.id);
                    if (row != null) {
                        let receiptDetails = JSON.parse(localStorage.getItem('receiptDetails'));
                        receiptDetails[existing_index].percentage = parseInt(product.percentage) + parseInt(existing.percentage);
                        localStorage.setItem('receiptDetails', JSON.stringify(receiptDetails));
                        let content = '';
                        content += '<tr id="row-' + existing.id + '">';
                        content += '<td>' + existing.productName + '</td>';
                        content += '<td>' + (parseInt(product.percentage) + parseInt(existing.percentage)) + '</td>';
                        content += '<td><button class="btn btn-danger" onclick=\'remove(' + existing.id + ')\'>Supprimer</button></td>';
                        content += '</tr>';
                        row.innerHTML = content;
                    }
                } else {

                    let receiptOut = document.getElementById('receipt-details');
                    let content = '';
                    if (receiptOut != null) {
                        content += '<tr id="row-' + product.id + '">';
                        content += '<td>' + product.productName + '</td>';
                        content += '<td>' + product.percentage + '</td>';
                        content += '<td><button class="btn btn-danger" onclick=\'remove(' + product.id + ')\'>Supprimer</button></td>';
                        content += '</tr>';
                    }
                    receiptOut.innerHTML += content;
                    receiptDetails.push(product);
                    localStorage.setItem('receiptDetails', JSON.stringify(receiptDetails));
                }
            }
        }
    }
    setReceiptHidden();
}

function deleteDetail() {
    localStorage.removeItem('receiptDetails');
}

function remove(id) {
    let row = document.getElementById('row-' + id);
    if (row != null) {
        let receiptDetails = JSON.parse(localStorage.getItem('receiptDetails'));
        receiptDetails = receiptDetails.filter(r => r.id != id);
        localStorage.setItem('receiptDetails', JSON.stringify(receiptDetails));
        row.parentNode.removeChild(row);
    }
}

function setReceiptHidden() {
    let productNameInput = document.getElementById('receipt-name');
    let detailsReceipt = document.getElementById('details-receipt');
    let receiptInput = document.getElementById('receipt');

    if (receiptInput != null) {
        if (receiptInput.value.trim() != '') {
            if (productNameInput != null & detailsReceipt != null) {
                let receiptDetails = localStorage.getItem('receiptDetails');
                if (receiptDetails != null) {
                    productNameInput.value = receiptInput.value;
                    detailsReceipt.value = receiptDetails;
                }
            }
        }
    }
}
