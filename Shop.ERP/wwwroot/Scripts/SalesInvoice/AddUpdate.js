$(document).ready(function () {

    var IsEdit = $('#SALES_MASTER_IsEdit').val();
    if (IsEdit === "") {
        $('#SALES_MASTER_SALES_NO').val('INV-XYZ');
        $('#SALES_MASTER_TRN_DATE').val(new Date());
        $('#SALES_MASTER_CUSTOMER_NAME').val('Walking Customer');
    } else {
        $.ajax({
            url: '/api/EditSalesInvoice',
            type: 'GET',
            data: { id: IsEdit },
            success: function (data) {
                $('#SALES_MASTER_ID').val(data.SALES_MASTER.ID);
                $('#SALES_MASTER_SALES_NO').val(data.SALES_MASTER.SALES_NO);
                $('#SALES_MASTER_TRN_DATE').val(data.SALES_MASTER.TRN_DATE);
                $('#SALES_MASTER_CUSTOMER_NAME').val(data.SALES_MASTER.CUSTOMER_NAME);
                $('#SALES_MASTER_TRN_NOTE').val(data.SALES_MASTER.TRN_NOTE);

                $.each(data.SALES_DETAIL_VM, function (index, item) {
                    CreateCartTable(item);
                });
            },
            error: function (xhr) {
                $.notify('Error: ' + xhr.status + ' ' + xhr.statusText, "error");
            },
            complete: function (xhr, status) {
                $("#categoryListLoader").addClass("d-none");
            }
        });
    }

    $('body').addClass('sidebar-collapse');

    $.ajax({
        url: '/api/ProductCategory/GetAll',
        type: 'GET',
        success: function (data) {
            $.each(data, function (index, item) {
                CreateCategoryButton(item);
            });
        },
        error: function (xhr) {
            $.notify('Error: ' + xhr.status + ' ' + xhr.statusText, "error");
        },
        complete: function (xhr, status) {
            $("#categoryListLoader").addClass("d-none");
        }
    });
});
function CreateCategoryButton(item) {
    var d1 = $('<div class="btn btn-flat btn-default m-1 bg-info" data-categoryid="' + item.ID + '" onclick="LoadProducts(this)"></div>');
    var d2 = $('<span class="text-center text-sm">' + item.CATEGORY_NAME + '</span>');
    d1.append(d2);
    $('.categoryList').append(d1);
}

function LoadProducts(e) {
    var categoryid = $(e).data('categoryid');

    $.ajax({
        url: '/api/Products/GetByCategoryID',
        type: 'GET',
        contentType: 'application/json',
        data: { id: categoryid },
        success: function (data, status, xhr) {
            $('#tblproductList tbody tr').remove();
            $.each(data, function (index, item) {
                CreateProductListTable(item)
            });
        },
        error: function (xhr) {
            $.notify('Error: ' + xhr.status + ' ' + xhr.statusText, "error");
        },
        complete: function (xhr, status) {

        }
    });
}
function CreateProductListTable(item) {
    var row = $('<tr data-id="' + item.ID + '" data-product_name="' + item.PRODUCT_NAME + '" data-unit_id="' + item.UNIT_ID + '" data-unit_name="' + item.UNIT_NAME + '" data-product_rate="' + item.PRODUCT_RATE + '" onclick="AddToCart(this)"></tr>');
    row.append('<td>' + item.PRODUCT_NAME + '</td>');
    row.append('<td>' + item.UNIT_NAME + '</td>');
    row.append('<td>' + item.PRODUCT_RATE + '</td>');
    $('#tblproductList tbody').append(row);
}
function AddToCart(e) {
    var exist = false;
    var id = $(e).data('id');

    if ($('#tblcartList tbody tr').length > 0) {
        $('#tblcartList tbody tr').each(function () {
            var row = $(this);
            var rowId = row.find('td:first').text();
            if (id === rowId) {
                row.find('td.qty').html(function (index, value) {
                    return parseInt(value) + 1;
                });

                row.find('td.amount').text(function (index, value) {
                    var rate = row.find('td.rate').html();
                    var quantity = row.find('td.qty').html();
                    return parseInt(rate) * parseInt(quantity);
                });
                exist = true;
            }
        });
    }

    if (exist == false) {
        var product_name = $(e).data('product_name');
        var unit_id = $(e).data('unit_id');
        var unit_name = $(e).data('unit_name');
        var product_rate = $(e).data('product_rate');

        var row = $('<tr onclick="RemoveFromCart(this)"></tr>');
        row.append('<td class="d-none">' + id + '</td>');
        row.append('<td>' + product_name + '</td>');
        row.append('<td class="d-none">' + unit_id + '</td>');
        row.append('<td>' + unit_name + '</td>');
        row.append('<td class="rate">' + product_rate + '</td>');
        row.append('<td class="qty">1</td>');
        row.append('<td class="amount">' + product_rate + '</td>');
        $('#tblcartList tbody').append(row);
    }

    calculateTotals();
}

function CreateCartTable(item) {
    //console.log(item);
    var row = $('<tr onclick="RemoveFromCart(this)"></tr>');
    row.append('<td class="d-none">' + item.PRODUCT_ID + '</td>');
    row.append('<td>' + item.PRODUCT_NAME + '</td>');
    row.append('<td class="d-none">' + item.UNIT_ID + '</td>');
    row.append('<td>' + item.UNIT_NAME + '</td>');
    row.append('<td class="rate">' + item.PRODUCT_RATE + '</td>');
    row.append('<td class="qty">' + item.PRODUCT_QTY+'</td>');
    row.append('<td class="amount">' + item.PRODUCT_AMOUNT + '</td>');
    $('#tblcartList tbody').append(row);
    calculateTotals();
}
function RemoveFromCart(e) {
    if (confirm("Are you sure you want to remove this item?")) {
        e.parentNode.removeChild(e);
        calculateTotals();
    }
}
function calculateTotals() {
    var totalQty = 0;
    var totalAmount = 0;

    $('#tblcartList tbody tr').each(function () {
        var row = $(this);
        totalQty += parseInt(row.find('td.qty').html());
        totalAmount += parseInt(row.find('td.amount').html());
    });

    $('#totalQty').text(totalQty);
    $('#totalAmount').text(totalAmount);
}

function SubmitSalesInvoice(e) {
    var sales_master_id = $('#SALES_MASTER_ID').val() || '';
    var sales_master_sales_no = $('#SALES_MASTER_SALES_NO').val() || '';
    var sales_master_trn_date = $('#SALES_MASTER_TRN_DATE').val() || '';
    var sales_master_customer_name = $('#SALES_MASTER_CUSTOMER_NAME').val() || '';
    var sales_master_trn_note = $('#SALES_MASTER_TRN_NOTE').val() || '';

    if (sales_master_customer_name == '') {
        $.notify("Enter sub Customer Name", "warn");
        return;
    }
    var cartRows = $('#tblcartList tbody tr');
    if (cartRows.length === 0) {
        $.notify("No items in the cart to save!", "error");
        return;
    }

    $(e).html('<span class="spinner-border spinner-border-sm"></span> Saving...');

    const salesMaster = {
        ID: sales_master_id,
        SALES_NO: sales_master_sales_no,
        TRN_DATE: sales_master_trn_date,
        CUSTOMER_NAME: sales_master_customer_name,
        TRN_NOTE: sales_master_trn_note
    };

    var salesDetail = [];
    cartRows.each(function () {
        var row = $(this);
        var item = {
            PRODUCT_ID: row.find('td').eq(0).text(), // 0 is Id, 1 is Name
            UNIT_ID: row.find('td').eq(2).text(),  // 2 Unit ID, 3 is Unit Name
            PRODUCT_RATE: parseFloat(row.find('td').eq(4).text()) || 0, // 4 is Rate
            PRODUCT_QTY: parseFloat(row.find('td').eq(5).text()) || 0,   // 5 is Qty
            PRODUCT_AMOUNT: parseFloat(row.find('td').eq(6).text()) || 0 // 6 is Amount
        };
        salesDetail.push(item);
    });

    var sales_md_vm = {
        SALES_MASTER: salesMaster,
        SALES_DETAIL_VM: salesDetail
    }

    $.ajax({
        url: '/api/SaveSalesInvoice',
        type: 'POST',
        data: sales_md_vm,
        success: function (response) {
            console.log(response);
            //$('#SALES_MASTER_SALES_NO').val(response.SALES_MASTER.SALES_NO);
            $.notify(response.SALES_MASTER.SALES_NO + " - Invoice saved successfully!", "success");
        },
        error: function (xhr, status, error) {
            $.notify('Error: ' + xhr.status + ' ' + xhr.statusText, "error");
        },
        complete: function () {
            $(e).html('<i class="fa fa-save"></i> Submit');
            e.disabled = true;
        }
    });
}