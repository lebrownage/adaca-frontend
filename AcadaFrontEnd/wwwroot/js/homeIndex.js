
//variables for the add product
const modalAddProduct = $('#add-product-modal')
const modalUpdateProduct = $('#update-product-modal')



//flag for an running action
let isRunning = false

function OpenAdd() {
    modalAddProduct.modal('show')
    $(`#add-form`).trigger('reset')
}

function OpenUpdate(id, name, description, price) {

    $('#update-id').val(id)
    $('#update-name').val(name)
    $('#update-description').val(description)
    $('#update-price').val(price)

    modalUpdateProduct.modal('show')
}

function UpsertProductEvent(e, prepend) {
    const btn = $(e.currentTarget)

    if (isRunning) { true }

    const addProductName = $(`#${prepend}-name`)
    const addProductDescription = $(`#${prepend}-description`)
    const addProductPrice = $(`#${prepend}-price`)
    const addProductForm = $(`#${prepend}-form`)

    //clear warning messages
    addProductForm.find('small.text-warning').remove()
    //flag for proceeding
    let willPass = true

    //check the fields
    if (!addProductName.val() || addProductName.val().trim().length == 0) {
        addProductName.after(`<small class="text-warning">Required</small>`)
        willPass = false
    }
    //check character limit
    if (addProductName.val().length > 500) {
        addProductName.after(`<small class="text-warning">Character Exceeded</small>`)
        willPass = false
    }

    //check the fields
    if (!addProductDescription.val() || addProductDescription.val().trim().length == 0) {
        addProductDescription.after(`<small class="text-warning">Required</small>`)
        willPass = false
    }
    //check character limit
    if (addProductDescription.val().length > 2000) {
        addProductDescription.after(`<small class="text-warning"> Character Exceeded</small>`)
        willPass = false
    }
    let priceValue = Number(addProductPrice.val())
    if (isNaN(priceValue) || priceValue <= 0) {
        addProductPrice.after(`<small class="text-warning">Invalid Value</small>`)
        willPass = false
    }

    //cancel event
    if (!willPass) { return }

    var formData = addProductForm.serialize();
    btn.prop('disabled', true)
    isRunning = true

    $.ajax({
        type: prepend == 'add' ?'POST' : 'PUT',
        url: '/home/' + prepend + "/" + $('#update-id').val(),
        data: formData,
        success: function (res) {
            btn.prop('disabled', false)
            alert('Success, sucessfully ' + prepend)
            location.reload()
        },
        error: function () {
            isRunning = false
            btn.prop('disabled', false)
            alert('Failed on ' + prepend + ' product')
        }
    })
}


function DeleteProduct(id,name) {
    if (!id) { return }
    //show confirmation message for the delete
    if (confirm(`Delete product named ${name}`)) {
        $.ajax({
            type: 'DELETE',
            url: '/home/delete/' + id,
            success: function (res) {
                alert('Success, sucessfully deleted')
                location.reload()
            },
            error: function () {
                isRunning = false
                alert('Failed on delete')
            }
        })
    }
}