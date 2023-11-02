function validateQuantity(input, min, max) {
    var value = parseInt(input.value)

    if (isNaN(value) || value < min) {
        input.value = min;
    }
    else if (value > max) {
        input.value = max;
    }
}